using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.DAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class EnsureCreatedAtColumnsExist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF COL_LENGTH('dbo.Products', 'CreatedAt') IS NULL
BEGIN
    IF COL_LENGTH('dbo.Products', 'CreatedAt ') IS NOT NULL
        EXEC sp_rename 'dbo.Products.[CreatedAt ]', 'CreatedAt', 'COLUMN';
    ELSE
        ALTER TABLE [dbo].[Products]
        ADD [CreatedAt] datetime2 NULL CONSTRAINT [DF_Products_CreatedAt_Ensure] DEFAULT GETDATE();
END
");

            migrationBuilder.Sql(@"
IF COL_LENGTH('dbo.Categories', 'CreatedAt') IS NULL
BEGIN
    IF COL_LENGTH('dbo.Categories', 'CreatedAt ') IS NOT NULL
        EXEC sp_rename 'dbo.Categories.[CreatedAt ]', 'CreatedAt', 'COLUMN';
    ELSE
        ALTER TABLE [dbo].[Categories]
        ADD [CreatedAt] datetime2 NULL CONSTRAINT [DF_Categories_CreatedAt_Ensure] DEFAULT GETDATE();
END
");

            migrationBuilder.Sql(@"
IF COL_LENGTH('dbo.Orders', 'CreatedAt') IS NULL
BEGIN
    IF COL_LENGTH('dbo.Orders', 'CreatedAt ') IS NOT NULL
        EXEC sp_rename 'dbo.Orders.[CreatedAt ]', 'CreatedAt', 'COLUMN';
    ELSE
        ALTER TABLE [dbo].[Orders]
        ADD [CreatedAt] datetime2 NULL CONSTRAINT [DF_Orders_CreatedAt_Ensure] DEFAULT GETDATE();
END
");

            migrationBuilder.Sql(@"
IF COL_LENGTH('dbo.OrderItems', 'CreatedAt') IS NULL
BEGIN
    IF COL_LENGTH('dbo.OrderItems', 'CreatedAt ') IS NOT NULL
        EXEC sp_rename 'dbo.OrderItems.[CreatedAt ]', 'CreatedAt', 'COLUMN';
    ELSE
        ALTER TABLE [dbo].[OrderItems]
        ADD [CreatedAt] datetime2 NULL CONSTRAINT [DF_OrderItems_CreatedAt_Ensure] DEFAULT GETDATE();
END
");

            migrationBuilder.Sql(@"
IF COL_LENGTH('dbo.Addresses', 'CreatedAt') IS NULL
BEGIN
    IF COL_LENGTH('dbo.Addresses', 'CreatedAt ') IS NOT NULL
        EXEC sp_rename 'dbo.Addresses.[CreatedAt ]', 'CreatedAt', 'COLUMN';
    ELSE
        ALTER TABLE [dbo].[Addresses]
        ADD [CreatedAt] datetime2 NULL CONSTRAINT [DF_Addresses_CreatedAt_Ensure] DEFAULT GETDATE();
END
");

            migrationBuilder.Sql(@"
IF COL_LENGTH('dbo.Favorites', 'CreatedAt') IS NULL
BEGIN
    IF COL_LENGTH('dbo.Favorites', 'CreatedAt ') IS NOT NULL
        EXEC sp_rename 'dbo.Favorites.[CreatedAt ]', 'CreatedAt', 'COLUMN';
    ELSE
        ALTER TABLE [dbo].[Favorites]
        ADD [CreatedAt] datetime2 NOT NULL CONSTRAINT [DF_Favorites_CreatedAt_Ensure] DEFAULT GETUTCDATE();
END
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF COL_LENGTH('dbo.Products', 'CreatedAt') IS NOT NULL
BEGIN
    DECLARE @dfProducts nvarchar(128);

    SELECT @dfProducts = dc.name
    FROM sys.default_constraints dc
    INNER JOIN sys.columns c ON c.default_object_id = dc.object_id
    INNER JOIN sys.tables t ON t.object_id = c.object_id
    WHERE t.name = 'Products' AND c.name = 'CreatedAt';

    IF @dfProducts IS NOT NULL
        EXEC('ALTER TABLE [dbo].[Products] DROP CONSTRAINT [' + @dfProducts + ']');

    ALTER TABLE [dbo].[Products] DROP COLUMN [CreatedAt];
END
");

            migrationBuilder.Sql(@"
IF COL_LENGTH('dbo.Categories', 'CreatedAt') IS NOT NULL
BEGIN
    DECLARE @dfCategories nvarchar(128);

    SELECT @dfCategories = dc.name
    FROM sys.default_constraints dc
    INNER JOIN sys.columns c ON c.default_object_id = dc.object_id
    INNER JOIN sys.tables t ON t.object_id = c.object_id
    WHERE t.name = 'Categories' AND c.name = 'CreatedAt';

    IF @dfCategories IS NOT NULL
        EXEC('ALTER TABLE [dbo].[Categories] DROP CONSTRAINT [' + @dfCategories + ']');

    ALTER TABLE [dbo].[Categories] DROP COLUMN [CreatedAt];
END
");

            migrationBuilder.Sql(@"
IF COL_LENGTH('dbo.Orders', 'CreatedAt') IS NOT NULL
BEGIN
    DECLARE @dfOrders nvarchar(128);

    SELECT @dfOrders = dc.name
    FROM sys.default_constraints dc
    INNER JOIN sys.columns c ON c.default_object_id = dc.object_id
    INNER JOIN sys.tables t ON t.object_id = c.object_id
    WHERE t.name = 'Orders' AND c.name = 'CreatedAt';

    IF @dfOrders IS NOT NULL
        EXEC('ALTER TABLE [dbo].[Orders] DROP CONSTRAINT [' + @dfOrders + ']');

    ALTER TABLE [dbo].[Orders] DROP COLUMN [CreatedAt];
END
");

            migrationBuilder.Sql(@"
IF COL_LENGTH('dbo.OrderItems', 'CreatedAt') IS NOT NULL
BEGIN
    DECLARE @dfOrderItems nvarchar(128);

    SELECT @dfOrderItems = dc.name
    FROM sys.default_constraints dc
    INNER JOIN sys.columns c ON c.default_object_id = dc.object_id
    INNER JOIN sys.tables t ON t.object_id = c.object_id
    WHERE t.name = 'OrderItems' AND c.name = 'CreatedAt';

    IF @dfOrderItems IS NOT NULL
        EXEC('ALTER TABLE [dbo].[OrderItems] DROP CONSTRAINT [' + @dfOrderItems + ']');

    ALTER TABLE [dbo].[OrderItems] DROP COLUMN [CreatedAt];
END
");

            migrationBuilder.Sql(@"
IF COL_LENGTH('dbo.Addresses', 'CreatedAt') IS NOT NULL
BEGIN
    DECLARE @dfAddresses nvarchar(128);

    SELECT @dfAddresses = dc.name
    FROM sys.default_constraints dc
    INNER JOIN sys.columns c ON c.default_object_id = dc.object_id
    INNER JOIN sys.tables t ON t.object_id = c.object_id
    WHERE t.name = 'Addresses' AND c.name = 'CreatedAt';

    IF @dfAddresses IS NOT NULL
        EXEC('ALTER TABLE [dbo].[Addresses] DROP CONSTRAINT [' + @dfAddresses + ']');

    ALTER TABLE [dbo].[Addresses] DROP COLUMN [CreatedAt];
END
");

            migrationBuilder.Sql(@"
IF COL_LENGTH('dbo.Favorites', 'CreatedAt') IS NOT NULL
BEGIN
    DECLARE @dfFavorites nvarchar(128);

    SELECT @dfFavorites = dc.name
    FROM sys.default_constraints dc
    INNER JOIN sys.columns c ON c.default_object_id = dc.object_id
    INNER JOIN sys.tables t ON t.object_id = c.object_id
    WHERE t.name = 'Favorites' AND c.name = 'CreatedAt';

    IF @dfFavorites IS NOT NULL
        EXEC('ALTER TABLE [dbo].[Favorites] DROP CONSTRAINT [' + @dfFavorites + ']');

    ALTER TABLE [dbo].[Favorites] DROP COLUMN [CreatedAt];
END
");
        }
    }
}
