CREATE PROCEDURE GetSalesByUser
    @UserID INT,
    @Month INT,
    @Year INT
AS
BEGIN
    SELECT 
        r.ReceiptDate, 
        SUM(r.AmountCollected) AS DailyTotal
    FROM 
        Receipts r
    WHERE 
        r.CashierID = @UserID
        AND MONTH(r.ReceiptDate) = @Month
        AND YEAR(r.ReceiptDate) = @Year
    GROUP BY 
        r.ReceiptDate
    ORDER BY 
        r.ReceiptDate;
END;
