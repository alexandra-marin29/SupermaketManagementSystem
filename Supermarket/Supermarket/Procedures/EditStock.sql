CREATE PROCEDURE EditStock
    @StockID INT,
    @Quantity DECIMAL(18, 2),
    @UnitOfMeasure NVARCHAR(30),
    @SupplyDate DATE,
    @ExpirationDate DATE,
    @SalePrice DECIMAL(18, 2)
AS
BEGIN
    UPDATE ProductStocks
    SET Quantity = @Quantity,
        UnitOfMeasure = @UnitOfMeasure,
        SupplyDate = @SupplyDate,
        ExpirationDate = @ExpirationDate,
        SalePrice = @SalePrice
    WHERE StockID = @StockID AND IsActive = 1;
END;
