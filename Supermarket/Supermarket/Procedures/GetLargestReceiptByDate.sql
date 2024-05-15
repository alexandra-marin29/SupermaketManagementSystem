CREATE PROCEDURE GetLargestReceiptByDate
    @Date DATE
AS
BEGIN
    SELECT TOP 1
        r.ReceiptID,
        r.ReceiptDate,
        r.CashierID,
        r.AmountCollected,
        rd.ProductID,
        rd.Quantity,
        rd.Subtotal
    FROM
        Receipts r
    INNER JOIN
        ReceiptDetails rd ON r.ReceiptID = rd.ReceiptID
    WHERE
        CAST(r.ReceiptDate AS DATE) = @Date
    ORDER BY
        r.AmountCollected DESC;
END;
