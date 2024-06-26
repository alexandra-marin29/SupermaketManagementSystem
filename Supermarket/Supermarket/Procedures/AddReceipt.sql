USE [dbSupermarket]
GO
/****** Object:  StoredProcedure [dbo].[AddReceipt]    Script Date: 15.05.2024 22:01:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[AddReceipt]
    @CashierID INT,
    @ReceiptDate DATETIME,
    @AmountCollected DECIMAL(18, 2)
AS
BEGIN
    INSERT INTO Receipts (CashierID, ReceiptDate, AmountCollected)
    VALUES (@CashierID, @ReceiptDate, @AmountCollected);

    SELECT SCOPE_IDENTITY() AS ReceiptID;
END