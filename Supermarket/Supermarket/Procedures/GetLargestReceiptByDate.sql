USE [dbSupermarket]
GO
/****** Object:  StoredProcedure [dbo].[GetLargestReceiptByDate]    Script Date: 21.05.2024 14:03:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[GetLargestReceiptByDate]
    @Date DATE
AS
BEGIN
    WITH ReceiptProductDetails AS (
        SELECT 
            r.ReceiptID,
            r.ReceiptDate, 
            u.UserName AS CashierName,
            STRING_AGG(p.ProductName + ':' + CAST(rd.Subtotal AS NVARCHAR(MAX)), ', ') WITHIN GROUP (ORDER BY p.ProductName) AS ProductDetails,
            SUM(rd.Quantity) AS TotalQuantity,
            r.AmountCollected
        FROM Receipts r
        JOIN ReceiptDetails rd ON r.ReceiptID = rd.ReceiptID
        JOIN Users u ON r.CashierID = u.UserID
        JOIN Products p ON rd.ProductID = p.ProductID
        WHERE CAST(r.ReceiptDate AS DATE) = @Date
        GROUP BY r.ReceiptID, r.ReceiptDate, u.UserName, r.AmountCollected
    )
    SELECT TOP 1 
        ReceiptDate, 
        CashierName,
        ProductDetails,
        TotalQuantity AS Quantity, 
        AmountCollected
    FROM ReceiptProductDetails
    ORDER BY AmountCollected DESC
END