USE [dbSupermarket]
GO
/****** Object:  StoredProcedure [dbo].[GetLargestReceiptByDate]    Script Date: 20.05.2024 12:36:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[GetLargestReceiptByDate]
    @Date DATE
AS
BEGIN
    WITH ReceiptProductNames AS (
        SELECT 
            r.ReceiptID,
            r.ReceiptDate, 
            u.UserName AS CashierName,
            STRING_AGG(p.ProductName, ', ') WITHIN GROUP (ORDER BY p.ProductName) AS ProductNames,
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
        ProductNames,
        TotalQuantity AS Quantity, 
        AmountCollected
    FROM ReceiptProductNames
    ORDER BY AmountCollected DESC
END