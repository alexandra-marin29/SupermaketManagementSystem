CREATE PROCEDURE DeleteStock
    @StockID INT
AS
BEGIN
    UPDATE ProductStocks
    SET IsActive = 0
    WHERE StockID = @StockID;
END;
