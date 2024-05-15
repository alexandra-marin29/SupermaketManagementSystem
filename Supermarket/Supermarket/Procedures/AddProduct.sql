CREATE PROCEDURE AddProduct
    @ProductName NVARCHAR(50),
    @Barcode NVARCHAR(50),
    @CategoryID INT,
    @ManufacturerID INT
AS
BEGIN
    INSERT INTO Products (ProductName, Barcode, CategoryID, ManufacturerID, IsActive)
    VALUES (@ProductName, @Barcode, @CategoryID, @ManufacturerID, 1);
END;