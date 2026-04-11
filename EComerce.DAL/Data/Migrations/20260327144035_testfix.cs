using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.DAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class testfix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF COL_LENGTH('dbo.Favorites', 'CreatedAt ') IS NOT NULL
   AND COL_LENGTH('dbo.Favorites', 'CreatedAt') IS NULL
BEGIN
    EXEC sp_rename 'dbo.Favorites.[CreatedAt ]', 'CreatedAt', 'COLUMN';
END
");

            migrationBuilder.Sql(@"
IF COL_LENGTH('dbo.AspNetUsers', 'IsActive') IS NULL
BEGIN
    ALTER TABLE [dbo].[AspNetUsers]
    ADD [IsActive] bit NOT NULL CONSTRAINT [DF_AspNetUsers_IsActive] DEFAULT CAST(0 AS bit);
END
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF COL_LENGTH('dbo.AspNetUsers', 'IsActive') IS NOT NULL
BEGIN
    DECLARE @dfName nvarchar(128);

    SELECT @dfName = dc.name
    FROM sys.default_constraints dc
    INNER JOIN sys.columns c
        ON c.default_object_id = dc.object_id
    INNER JOIN sys.tables t
        ON t.object_id = c.object_id
    WHERE t.name = 'AspNetUsers'
      AND c.name = 'IsActive';

    IF @dfName IS NOT NULL
    BEGIN
        EXEC('ALTER TABLE [dbo].[AspNetUsers] DROP CONSTRAINT [' + @dfName + ']');
    END

    ALTER TABLE [dbo].[AspNetUsers] DROP COLUMN [IsActive];
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
