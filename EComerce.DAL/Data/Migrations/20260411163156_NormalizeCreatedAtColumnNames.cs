using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.DAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class NormalizeCreatedAtColumnNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF COL_LENGTH('dbo.Orders', 'CreatedAt ') IS NOT NULL
   AND COL_LENGTH('dbo.Orders', 'CreatedAt') IS NULL
BEGIN
    EXEC sp_rename 'dbo.Orders.[CreatedAt ]', 'CreatedAt', 'COLUMN';
END
");

            migrationBuilder.Sql(@"
IF COL_LENGTH('dbo.OrderItems', 'CreatedAt ') IS NOT NULL
   AND COL_LENGTH('dbo.OrderItems', 'CreatedAt') IS NULL
BEGIN
    EXEC sp_rename 'dbo.OrderItems.[CreatedAt ]', 'CreatedAt', 'COLUMN';
END
");

            migrationBuilder.Sql(@"
IF COL_LENGTH('dbo.Categories', 'CreatedAt ') IS NOT NULL
   AND COL_LENGTH('dbo.Categories', 'CreatedAt') IS NULL
BEGIN
    EXEC sp_rename 'dbo.Categories.[CreatedAt ]', 'CreatedAt', 'COLUMN';
END
");

            migrationBuilder.Sql(@"
IF COL_LENGTH('dbo.Addresses', 'CreatedAt ') IS NOT NULL
   AND COL_LENGTH('dbo.Addresses', 'CreatedAt') IS NULL
BEGIN
    EXEC sp_rename 'dbo.Addresses.[CreatedAt ]', 'CreatedAt', 'COLUMN';
END
");

            migrationBuilder.Sql(@"
IF COL_LENGTH('dbo.Products', 'CreatedAt ') IS NOT NULL
   AND COL_LENGTH('dbo.Products', 'CreatedAt') IS NULL
BEGIN
    EXEC sp_rename 'dbo.Products.[CreatedAt ]', 'CreatedAt', 'COLUMN';
END
");

            migrationBuilder.Sql(@"
IF COL_LENGTH('dbo.Favorites', 'CreatedAt ') IS NOT NULL
   AND COL_LENGTH('dbo.Favorites', 'CreatedAt') IS NULL
BEGIN
    EXEC sp_rename 'dbo.Favorites.[CreatedAt ]', 'CreatedAt', 'COLUMN';
END
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF COL_LENGTH('dbo.Orders', 'CreatedAt') IS NOT NULL
   AND COL_LENGTH('dbo.Orders', 'CreatedAt ') IS NULL
BEGIN
    EXEC sp_rename 'dbo.Orders.[CreatedAt]', 'CreatedAt ', 'COLUMN';
END
");

            migrationBuilder.Sql(@"
IF COL_LENGTH('dbo.OrderItems', 'CreatedAt') IS NOT NULL
   AND COL_LENGTH('dbo.OrderItems', 'CreatedAt ') IS NULL
BEGIN
    EXEC sp_rename 'dbo.OrderItems.[CreatedAt]', 'CreatedAt ', 'COLUMN';
END
");

            migrationBuilder.Sql(@"
IF COL_LENGTH('dbo.Categories', 'CreatedAt') IS NOT NULL
   AND COL_LENGTH('dbo.Categories', 'CreatedAt ') IS NULL
BEGIN
    EXEC sp_rename 'dbo.Categories.[CreatedAt]', 'CreatedAt ', 'COLUMN';
END
");

            migrationBuilder.Sql(@"
IF COL_LENGTH('dbo.Addresses', 'CreatedAt') IS NOT NULL
   AND COL_LENGTH('dbo.Addresses', 'CreatedAt ') IS NULL
BEGIN
    EXEC sp_rename 'dbo.Addresses.[CreatedAt]', 'CreatedAt ', 'COLUMN';
END
");

            migrationBuilder.Sql(@"
IF COL_LENGTH('dbo.Products', 'CreatedAt') IS NOT NULL
   AND COL_LENGTH('dbo.Products', 'CreatedAt ') IS NULL
BEGIN
    EXEC sp_rename 'dbo.Products.[CreatedAt]', 'CreatedAt ', 'COLUMN';
END
");

            migrationBuilder.Sql(@"
IF COL_LENGTH('dbo.Favorites', 'CreatedAt') IS NOT NULL
   AND COL_LENGTH('dbo.Favorites', 'CreatedAt ') IS NULL
BEGIN
    EXEC sp_rename 'dbo.Favorites.[CreatedAt]', 'CreatedAt ', 'COLUMN';
END
");
        }
    }
}
