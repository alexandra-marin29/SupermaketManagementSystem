USE [dbSupermarket]
GO
/****** Object:  StoredProcedure [dbo].[EditStock]    Script Date: 18.05.2024 21:49:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[EditStock]
    @StockID INT,
    @Quantity DECIMAL(18, 2),
    @UnitOfMeasure NVARCHAR(30),
    @SupplyDate DATETIME,
    @ExpirationDate DATETIME,
    @SalePrice DECIMAL(18, 2),
    @IsActive BIT
AS
BEGIN
    UPDATE ProductStocks
    SET 
	    Quantity = @Quantity,
        UnitOfMeasure = @UnitOfMeasure,
        SupplyDate = @SupplyDate,
        ExpirationDate = @ExpirationDate,
        SalePrice = @SalePrice,
        IsActive = @IsActive
    WHERE StockID = @StockID;
END;
