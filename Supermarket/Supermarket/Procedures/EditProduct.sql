CREATE PROCEDURE EditProduct
    @ProductID INT,
    @ProductName NVARCHAR(50),
    @Barcode NVARCHAR(50),
    @CategoryID INT,
    @ManufacturerID INT
AS
BEGIN
    UPDATE Products
    SET ProductName = @ProductName,
        Barcode = @Barcode,
        CategoryID = @CategoryID,
        ManufacturerID = @ManufacturerID
    WHERE ProductID = @ProductID;
END;