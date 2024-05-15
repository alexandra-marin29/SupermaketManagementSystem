CREATE PROCEDURE GetActiveProducts
AS
BEGIN
    SELECT ProductID, ProductName, Barcode, CategoryID, ManufacturerID, IsActive
    FROM Products
    WHERE IsActive = 1;
END;