USE [dbSupermarket]
GO
/****** Object:  StoredProcedure [dbo].[GetLargestReceiptByDate]    Script Date: 19.05.2024 13:23:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[GetLargestReceiptByDate]
    @Date DATE
AS
BEGIN
    SELECT TOP 1 
        r.ReceiptDate, 
        u.UserName AS CashierName,
        STRING_AGG(p.ProductName, ', ') AS ProductNames,
        rd.Quantity, 
        r.AmountCollected
    FROM Receipts r
    JOIN ReceiptDetails rd ON r.ReceiptID = rd.ReceiptID
    JOIN Users u ON r.CashierID = u.UserID
    JOIN Products p ON rd.ProductID = p.ProductID
    WHERE CAST(r.ReceiptDate AS DATE) = @Date
    GROUP BY r.ReceiptID, r.ReceiptDate, u.UserName, rd.Quantity, r.AmountCollected
    ORDER BY r.AmountCollected DESC
END