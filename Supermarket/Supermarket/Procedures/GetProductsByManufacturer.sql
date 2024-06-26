USE [dbSupermarket]
GO
/****** Object:  StoredProcedure [dbo].[GetProductsByManufacturer]    Script Date: 15.05.2024 16:19:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[GetProductsByManufacturer]
    @ManufacturerID INT
AS
BEGIN
    SELECT p.ProductName, c.CategoryName
    FROM Products p
    JOIN Categories c ON p.CategoryID = c.CategoryID
    WHERE p.ManufacturerID = @ManufacturerID AND p.IsActive = 1;
END