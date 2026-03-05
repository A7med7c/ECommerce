document.addEventListener("DOMContentLoaded", function () {

    // ── SweetAlert delete confirmation ──────────────────────────
    document.querySelectorAll(".delete-btn").forEach(function (button) {
        button.addEventListener("click", function (e) {
            e.preventDefault();
            var id = this.getAttribute("data-id");
            var msg = this.getAttribute("data-message") || "This record will be permanently deleted!";

            Swal.fire({
                title: "Are you sure?",
                text: msg,
                icon: "warning",
                showCancelButton: true,
                confirmButtonColor: "#dc2626",
                cancelButtonColor: "#6b7280",
                confirmButtonText: "Yes, delete it!",
                cancelButtonText: "Cancel",
                borderRadius: "14px"
            }).then(function (result) {
                if (result.isConfirmed) {
                    document.getElementById("deleteId").value = id;
                    document.getElementById("deleteForm").submit();
                }
            });
        });
    });

    // ── Floating Chat Assistant ─────────────────────────────────
    var chatFab = document.getElementById("chatFab");
    var chatPanel = document.getElementById("chatPanel");
    var chatClose = document.getElementById("chatClose");
    var chatInput = document.getElementById("chatInput");
    var chatSend = document.getElementById("chatSend");
    var chatBody = document.getElementById("chatBody");

    if (chatFab && chatPanel) {
        chatFab.addEventListener("click", function () {
            chatPanel.classList.toggle("open");
            if (chatPanel.classList.contains("open") && chatInput) {
                chatInput.focus();
            }
        });

        if (chatClose) {
            chatClose.addEventListener("click", function () {
                chatPanel.classList.remove("open");
            });
        }

        // Simple echo behavior for chat UI
        function sendMessage() {
            if (!chatInput || !chatBody) return;
            var text = chatInput.value.trim();
            if (!text) return;

            // User message
            var userMsg = document.createElement("div");
            userMsg.className = "chat-message user";
            userMsg.innerHTML = '<div class="chat-bubble">' + escapeHtml(text) + '</div>';
            chatBody.appendChild(userMsg);

            chatInput.value = "";

            // Bot response
            setTimeout(function () {
                var botMsg = document.createElement("div");
                botMsg.className = "chat-message bot";
                botMsg.innerHTML = '<div class="chat-bubble">Thanks for your message! Our team will get back to you soon. In the meantime, feel free to browse our products.</div>';
                chatBody.appendChild(botMsg);
                chatBody.scrollTop = chatBody.scrollHeight;
            }, 600);

            chatBody.scrollTop = chatBody.scrollHeight;
        }

        if (chatSend) {
            chatSend.addEventListener("click", sendMessage);
        }
        if (chatInput) {
            chatInput.addEventListener("keypress", function (e) {
                if (e.key === "Enter") {
                    e.preventDefault();
                    sendMessage();
                }
            });
        }
    }

    // ── Async Add to Cart ────────────────────────────────────────
    document.addEventListener("submit", function (e) {
        var form = e.target;
        var asyncType = form.getAttribute("data-async");
        if (!asyncType) return;
        e.preventDefault();
        if (asyncType === "cart") {
            handleCartSubmit(form);
        } else if (asyncType === "favorite") {
            handleFavSubmit(form);
        }
    });

    function handleCartSubmit(form) {
        var btn = form.querySelector("button[type='submit']");
        var originalHtml = btn.innerHTML;
        btn.disabled = true;
        btn.innerHTML = "<i class='fa-solid fa-spinner fa-spin me-1'></i>Adding...";

        var params = new URLSearchParams(new FormData(form));

        fetch(form.action, {
            method: "POST",
            headers: { "X-Requested-With": "XMLHttpRequest" },
            body: params
        })
        .then(function (r) {
            if (r.redirected && r.url.toLowerCase().indexOf("/login") !== -1) {
                window.location.href = r.url;
                return null;
            }
            return r.json();
        })
        .then(function (data) {
            btn.disabled = false;
            btn.innerHTML = originalHtml;
            if (!data) return;
            showToast(data.success ? "success" : "error", data.message);
            if (data.success && data.cartCount !== undefined) {
                updateCartBadge(data.cartCount);
            }
        })
        .catch(function () {
            btn.disabled = false;
            btn.innerHTML = originalHtml;
            showToast("error", "Something went wrong. Please try again.");
        });
    }

    function handleFavSubmit(form) {
        var btn = form.querySelector("button[type='submit']");
        var params = new URLSearchParams(new FormData(form));
        var isToggle = form.getAttribute("data-fav-toggle") === "true";

        fetch(form.action, {
            method: "POST",
            headers: { "X-Requested-With": "XMLHttpRequest" },
            body: params
        })
        .then(function (r) {
            if (r.redirected && r.url.toLowerCase().indexOf("/login") !== -1) {
                window.location.href = r.url;
                return null;
            }
            return r.json();
        })
        .then(function (data) {
            if (!data) return;
            showToast(data.success ? "success" : "info", data.message);
            if (isToggle) {
                // Details page: toggle full button state and form action
                if (data.isFavorite) {
                    btn.classList.remove("btn-outline-danger");
                    btn.classList.add("btn-danger");
                    btn.innerHTML = "<i class='fa-solid fa-heart me-2'></i>Unfavourite";
                    form.setAttribute("action", form.getAttribute("action").replace("/Add", "/Remove"));
                } else {
                    btn.classList.remove("btn-danger");
                    btn.classList.add("btn-outline-danger");
                    btn.innerHTML = "<i class='fa-solid fa-heart me-2'></i>Favourite";
                    form.setAttribute("action", form.getAttribute("action").replace("/Remove", "/Add"));
                }
            } else {
                // List pages: visually mark as favourited
                if (data.success) {
                    btn.classList.remove("btn-outline-danger");
                    btn.classList.add("btn-danger");
                    btn.title = "Already in Favourites";
                }
            }
        })
        .catch(function () {
            showToast("error", "Something went wrong. Please try again.");
        });
    }

    function updateCartBadge(count) {
        var badge = document.getElementById("cart-badge");
        if (!badge) return;
        if (count > 0) {
            badge.textContent = count > 99 ? "99+" : count;
            badge.style.display = "";
        } else {
            badge.style.display = "none";
        }
    }

    function showToast(type, message) {
        var container = document.getElementById("shopToastContainer");
        if (!container) return;
        var id = "toast-" + Date.now();
        var bgClass = type === "success" ? "bg-success" : type === "error" ? "bg-danger" : "bg-primary";
        var icon = type === "success" ? "fa-circle-check" : type === "error" ? "fa-circle-xmark" : "fa-circle-info";
        var el = document.createElement("div");
        el.id = id;
        el.className = "toast align-items-center text-white " + bgClass + " border-0";
        el.setAttribute("role", "alert");
        el.setAttribute("aria-live", "assertive");
        el.setAttribute("aria-atomic", "true");
        el.innerHTML =
            "<div class='d-flex'>" +
            "<div class='toast-body'><i class='fa-solid " + icon + " me-2'></i>" + escapeHtml(message) + "</div>" +
            "<button type='button' class='btn-close btn-close-white me-2 m-auto' data-bs-dismiss='toast' aria-label='Close'></button>" +
            "</div>";
        container.appendChild(el);
        var toast = bootstrap.Toast.getOrCreateInstance(el, { delay: 3500 });
        toast.show();
        el.addEventListener("hidden.bs.toast", function () { el.remove(); });
    }

    // ── Auto-dismiss alerts after 5 seconds ─────────────────────
    document.querySelectorAll(".alert-dismissible").forEach(function (alert) {
        setTimeout(function () {
            var bsAlert = bootstrap.Alert.getOrCreateInstance(alert);
            if (bsAlert) bsAlert.close();
        }, 5000);
    });

    // ── Active nav link highlighting ────────────────────────────
    var currentPath = window.location.pathname.toLowerCase();
    document.querySelectorAll(".navbar-nav .nav-link").forEach(function (link) {
        var href = link.getAttribute("href");
        if (href && href !== "#" && currentPath.startsWith(href.toLowerCase())) {
            link.style.color = "var(--shop-primary)";
            link.style.fontWeight = "600";
        }
    });

    // ── Helper: escape HTML ─────────────────────────────────────
    function escapeHtml(text) {
        var div = document.createElement("div");
        div.appendChild(document.createTextNode(text));
        return div.innerHTML;
    }

});
