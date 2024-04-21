Use LearningFiltersShoppingDemoDB;

CREATE PROCEDURE FilterBigBasketGroceries
    @Index INT = NULL,
    @Product NVARCHAR(MAX) = NULL,
    @Category NVARCHAR(MAX) = NULL,
    @SubCategory NVARCHAR(MAX) = NULL,
    @Brand NVARCHAR(MAX) = NULL,
    @SalePrice DECIMAL(10, 2) = NULL,
    @MarketPrice DECIMAL(10, 2) = NULL,
    @Type NVARCHAR(MAX) = NULL,
    @MinRating DECIMAL(3, 1) = NULL,
    @MaxRating DECIMAL(3, 1) = NULL,
    @Description NVARCHAR(MAX) = NULL,
    @Limit INT = NULL,
    @Skip INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @SQL NVARCHAR(MAX);
    SET @SQL = 'SELECT * FROM BigBasketGroceries';

    IF @Index IS NOT NULL
    BEGIN
        SET @SQL = @SQL + ' WHERE [index] = @Index';
    END
    ELSE
    BEGIN
        SET @SQL = @SQL + ' WHERE 1=1';
    END

    IF @Product IS NOT NULL
    BEGIN
        SET @SQL = @SQL + ' AND product = @Product';
    END

    IF @Category IS NOT NULL
    BEGIN
        SET @SQL = @SQL + ' AND category = @Category';
    END

    IF @SubCategory IS NOT NULL
    BEGIN
        SET @SQL = @SQL + ' AND sub_category = @SubCategory';
    END

    IF @Brand IS NOT NULL
    BEGIN
        SET @SQL = @SQL + ' AND brand = @Brand';
    END

    IF @SalePrice IS NOT NULL
    BEGIN
        SET @SQL = @SQL + ' AND sale_price = @SalePrice';
    END

    IF @MarketPrice IS NOT NULL
    BEGIN
        SET @SQL = @SQL + ' AND market_price = @MarketPrice';
    END

    IF @Type IS NOT NULL
    BEGIN
        SET @SQL = @SQL + ' AND [type] = @Type';
    END

    IF @MinRating IS NOT NULL
    BEGIN
        SET @SQL = @SQL + ' AND rating >= @MinRating';
    END

    IF @MaxRating IS NOT NULL
    BEGIN
        SET @SQL = @SQL + ' AND rating <= @MaxRating';
    END

    IF @Description IS NOT NULL
    BEGIN
        SET @SQL = @SQL + ' AND description = @Description';
    END

    IF @Limit IS NOT NULL AND @Skip IS NOT NULL
    BEGIN
        SET @SQL = @SQL + ' ORDER BY [index] OFFSET @Skip ROWS FETCH NEXT @Limit ROWS ONLY';
    END

    EXEC sp_executesql @SQL, N'@Index INT, @Product NVARCHAR(MAX), @Category NVARCHAR(MAX), @SubCategory NVARCHAR(MAX), @Brand NVARCHAR(MAX), @SalePrice DECIMAL(10, 2), @MarketPrice DECIMAL(10, 2), @Type NVARCHAR(MAX), @MinRating DECIMAL(3, 1), @MaxRating DECIMAL(3, 1), @Description NVARCHAR(MAX), @Limit INT, @Skip INT', @Index, @Product, @Category, @SubCategory, @Brand, @SalePrice, @MarketPrice, @Type, @MinRating, @MaxRating, @Description, @Limit, @Skip;
END



--Drop procedure FilterBigBasketGroceries

--- Test

EXEC FilterBigBasketGroceries @MinRating = 4.1, @Limit = 5, @Skip = 0;
