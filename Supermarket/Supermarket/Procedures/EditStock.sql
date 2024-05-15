USE [dbSupermarket]
GO
/****** Object:  StoredProcedure [dbo].[EditStock]    Script Date: 15.05.2024 17:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[EditStock]
    @StockID INT,
    @Quantity DECIMAL(18, 2),
    @UnitOfMeasure NVARCHAR(30),
    @SupplyDate DATE,
    @ExpirationDate DATE,
    @SalePrice DECIMAL(18, 2)
AS
BEGIN
    UPDATE ProductStocks
    SET 
	  Quantity = @Quantity,
        UnitOfMeasure = @UnitOfMeasure,
        SupplyDate = @SupplyDate,
        ExpirationDate = @ExpirationDate,
        SalePrice = @SalePrice
    WHERE StockID = @StockID AND IsActive = 1;
END;
