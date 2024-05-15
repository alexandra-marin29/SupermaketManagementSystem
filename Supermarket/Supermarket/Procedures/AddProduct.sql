USE [dbSupermarket]
GO
/****** Object:  StoredProcedure [dbo].[AddProduct]    Script Date: 15.05.2024 20:18:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[AddProduct]
    @ProductName NVARCHAR(50),
    @Barcode NVARCHAR(50),
    @CategoryID INT,
    @ManufacturerID INT,
	@IsActive BIT
AS
BEGIN
    INSERT INTO Products (ProductName, Barcode, CategoryID, ManufacturerID, IsActive)
    VALUES (@ProductName, @Barcode, @CategoryID, @ManufacturerID, @IsActive)
END;