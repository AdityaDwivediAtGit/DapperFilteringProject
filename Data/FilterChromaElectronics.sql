Use LearningFiltersShoppingDemoDB;

CREATE PROCEDURE FilterChromaElectronics
    @Index INT = NULL,
    @Limit INT = NULL,
    @Skip INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @SQL NVARCHAR(MAX);
    SET @SQL = 'SELECT * FROM ChromaElectronics';

    IF @Index IS NOT NULL
    BEGIN
        SET @SQL = @SQL + ' WHERE [index] = @Index';
    END

    IF @Limit IS NOT NULL AND @Skip IS NOT NULL
    BEGIN
        SET @SQL = @SQL + ' ORDER BY [index] OFFSET @Skip ROWS FETCH NEXT @Limit ROWS ONLY';
    END

    EXEC sp_executesql @SQL, N'@Index INT, @Limit INT, @Skip INT', @Index, @Limit, @Skip;
END

--- Test
EXEC FilterChromaElectronics @Limit = 10, @Skip = 0;