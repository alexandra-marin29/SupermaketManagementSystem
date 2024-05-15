CREATE PROCEDURE GetAllCategories
AS
BEGIN
    SELECT CategoryID, CategoryName FROM Categories WHERE IsActive = 1;
END;
