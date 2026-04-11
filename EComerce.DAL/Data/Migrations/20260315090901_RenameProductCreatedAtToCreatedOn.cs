using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.DAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameProductCreatedAtToCreatedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF COL_LENGTH('dbo.Products', 'CreatedAt') IS NOT NULL
   AND COL_LENGTH('dbo.Products', 'CreatedAt ') IS NULL
BEGIN
    EXEC sp_rename 'dbo.Products.[CreatedAt]', 'CreatedAt ', 'COLUMN';
END
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF COL_LENGTH('dbo.Products', 'CreatedAt ') IS NOT NULL
   AND COL_LENGTH('dbo.Products', 'CreatedAt') IS NULL
BEGIN
    EXEC sp_rename 'dbo.Products.[CreatedAt ]', 'CreatedAt', 'COLUMN';
END
");
        }
    }
}
