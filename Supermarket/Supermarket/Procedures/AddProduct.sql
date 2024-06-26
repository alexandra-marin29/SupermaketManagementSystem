USE [dbSupermarket]
GO
/****** Object:  StoredProcedure [dbo].[AddProduct]    Script Date: 18.05.2024 01:56:08 ******/
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