CREATE PROCEDURE AddReceiptDetail
    @ReceiptID INT,
    @ProductID INT,
    @Quantity DECIMAL(18, 2),
    @Subtotal DECIMAL(18, 2)
AS
BEGIN
    INSERT INTO ReceiptDetails (ReceiptID, ProductID, Quantity, Subtotal)
    VALUES (@ReceiptID, @ProductID, @Quantity, @Subtotal);
END
