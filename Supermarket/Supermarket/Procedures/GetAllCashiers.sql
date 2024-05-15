CREATE PROCEDURE GetAllCashiers
AS
BEGIN
    SELECT UserID, Username FROM Users WHERE Role = 'Cashier' AND IsActive = 1;
END;
