CREATE PROCEDURE GetLargestReceipt
    @Date DATE
AS
BEGIN
    SELECT TOP 1 
        r.ReceiptID, 
        r.AmountCollected
    FROM 
        Receipts r
    WHERE 
        CAST(r.ReceiptDate AS DATE) = @Date
    ORDER BY 
        r.AmountCollected DESC;
END;
