CREATE PROCEDURE AddReceipt
    @CashierID INT,
    @ReceiptDate DATETIME,
    @AmountCollected DECIMAL(18,2)
AS
BEGIN
    DECLARE @ReceiptID INT;

    INSERT INTO Receipts (CashierID, ReceiptDate, AmountCollected)
    VALUES (@CashierId, @ReceiptDate, @AmountCollected);

    SET @ReceiptID = SCOPE_IDENTITY();

    INSERT INTO ReceiptDetails (ReceiptID, ProductID, Quantity, Subtotal)
    SELECT @ReceiptID, ProductID, Quantity, Subtotal
    FROM ReceiptDetails;

    DECLARE @ProductID INT, @Quantity DECIMAL(18,2);

    DECLARE cur CURSOR FOR
    SELECT ProductID, Quantity
    FROM ReceiptDetails;

    OPEN cur;

    FETCH NEXT FROM cur INTO @ProductID, @Quantity;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        UPDATE ProductStocks
        SET Quantity = Quantity - @Quantity
        WHERE ProductID = @ProductID
        AND Quantity >= @Quantity;

        FETCH NEXT FROM cur INTO @ProductID, @Quantity;
    END;

    CLOSE cur;
    DEALLOCATE cur;
END;
