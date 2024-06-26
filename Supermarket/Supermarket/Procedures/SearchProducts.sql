USE [dbSupermarket]
GO
/****** Object:  StoredProcedure [dbo].[SearchProducts]    Script Date: 15.05.2024 21:27:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[SearchProducts]
    @ProductName NVARCHAR(50) = NULL,
    @Barcode NVARCHAR(50) = NULL,
    @ExpirationDate DATE = NULL,
    @ManufacturerID INT = NULL,
    @CategoryID INT = NULL
AS
BEGIN
    SELECT p.*, m.ManufacturerName, c.CategoryName
    FROM Products p
    LEFT JOIN Manufacturers m ON p.ManufacturerID = m.ManufacturerID
    LEFT JOIN Categories c ON p.CategoryID = c.CategoryID
    WHERE 
        (@ProductName IS NULL OR p.ProductName LIKE '%' + @ProductName + '%')
        AND (@Barcode IS NULL OR p.Barcode LIKE '%' + @Barcode + '%')
        AND (@ManufacturerID IS NULL OR p.ManufacturerID = @ManufacturerID)
        AND (@CategoryID IS NULL OR p.CategoryID = @CategoryID)
        AND p.IsActive = 1
END