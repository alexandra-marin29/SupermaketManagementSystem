USE [dbSupermarket]
GO
/****** Object:  StoredProcedure [dbo].[GetProductsByManufacturer]    Script Date: 15.05.2024 15:35:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[GetProductsByManufacturer]
    @ManufacturerID INT
AS
BEGIN
    SELECT 
        c.CategoryName, 
        p.ProductName,
        ps.SalePrice,
        ps.Quantity
    FROM 
        Products p
        INNER JOIN Categories c ON p.CategoryID = c.CategoryID
        INNER JOIN ProductStocks ps ON p.ProductID = ps.ProductID
    WHERE 
        p.ManufacturerID = @ManufacturerID
        AND p.IsActive = 1
        AND ps.IsActive = 1
    ORDER BY 
        c.CategoryName, p.ProductName;
END;