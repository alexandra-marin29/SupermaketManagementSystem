USE [dbSupermarket]
GO
/****** Object:  StoredProcedure [dbo].[GetCategoryValues]    Script Date: 15.05.2024 16:43:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[GetCategoryValues]
    @CategoryID INT
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
        AND c.CategoryID = @CategoryID
    GROUP BY 
        c.CategoryName;
END;