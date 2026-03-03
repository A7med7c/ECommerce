// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code

<script>
    document.addEventListener("DOMContentLoaded", function () {

        document.querySelectorAll(".delete-btn").forEach(button => {

            button.addEventListener("click", function (e) {

                e.preventDefault();

                const id = this.getAttribute("data-id");

                Swal.fire({
                    title: "Are you sure?",
                    text: "This product will be deleted!",
                    icon: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#d33",
                    cancelButtonColor: "#6c757d",
                    confirmButtonText: "Yes, delete it!"
                }).then((result) => {

                    if (result.isConfirmed) {
                        document.getElementById("deleteId").value = id;
                        document.getElementById("deleteForm").submit();
                    }

                });

            });

        });

    });
</script>
