USE [dbSupermarket]
GO
/****** Object:  StoredProcedure [dbo].[SearchProductsInStock]    Script Date: 18.05.2024 22:21:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[SearchProductsInStock]
    @ProductName NVARCHAR(50) = NULL,
    @Barcode NVARCHAR(50) = NULL,
    @ExpirationDate DATE = NULL,
    @ManufacturerID INT = NULL,
    @CategoryID INT = NULL
AS
BEGIN
    SELECT DISTINCT p.*
    FROM Products p
    JOIN ProductStocks s ON p.ProductID = s.ProductID
    WHERE s.Quantity > 0
    AND s.IsActive = 1
    AND (@ProductName IS NULL OR p.ProductName LIKE '%' + @ProductName + '%')
    AND (@Barcode IS NULL OR p.Barcode LIKE '%' + @Barcode + '%')
    AND (@ExpirationDate IS NULL OR s.ExpirationDate >= @ExpirationDate)
    AND (@ManufacturerID IS NULL OR p.ManufacturerID = @ManufacturerID)
    AND (@CategoryID IS NULL OR p.CategoryID = @CategoryID)
    AND p.IsActive = 1
END;
