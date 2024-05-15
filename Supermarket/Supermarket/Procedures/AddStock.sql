CREATE PROCEDURE AddStock
    @ProductID INT,
    @Quantity DECIMAL(18, 2),
    @UnitOfMeasure NVARCHAR(30),
    @SupplyDate DATE,
    @ExpirationDate DATE,
    @PurchasePrice DECIMAL(18, 2),
    @SalePrice DECIMAL(18, 2)
AS
BEGIN
    INSERT INTO ProductStocks (ProductID, Quantity, UnitOfMeasure, SupplyDate, ExpirationDate, PurchasePrice, SalePrice, IsActive)
    VALUES (@ProductID, @Quantity, @UnitOfMeasure, @SupplyDate, @ExpirationDate, @PurchasePrice, @SalePrice, 1);
END;
