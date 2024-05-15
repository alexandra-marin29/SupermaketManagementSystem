CREATE PROCEDURE DeleteProduct
    @ProductID INT
AS
BEGIN
    UPDATE Products
    SET IsActive = 0
    WHERE ProductID = @ProductID;
END;