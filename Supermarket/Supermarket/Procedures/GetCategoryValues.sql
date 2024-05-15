CREATE PROCEDURE GetCategoryValues
AS
BEGIN
    SELECT 
        c.CategoryName, 
        SUM(ps.SalePrice * ps.Quantity) AS TotalValue
    FROM 
        Products p
        INNER JOIN Categories c ON p.CategoryID = c.CategoryID
        INNER JOIN ProductStocks ps ON p.ProductID = ps.ProductID
    WHERE 
        p.IsActive = 1
        AND ps.IsActive = 1
    GROUP BY 
        c.CategoryName;
END;
