USE [dbSupermarket]
GO
/****** Object:  StoredProcedure [dbo].[EditStock]    Script Date: 19.05.2024 00:23:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[EditStock]
    @StockID INT,
    @ProductID INT,
    @Quantity DECIMAL(18, 2),
    @UnitOfMeasure NVARCHAR(50),
    @SupplyDate DATE,
    @ExpirationDate DATE,
    @PurchasePrice DECIMAL(18, 2),
    @SalePrice DECIMAL(18, 2),
    @IsActive BIT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE ProductStocks
    SET
        ProductID = @ProductID,
        Quantity = @Quantity,
        UnitOfMeasure = @UnitOfMeasure,
        SupplyDate = @SupplyDate,
        ExpirationDate = @ExpirationDate,
        PurchasePrice = @PurchasePrice,
        SalePrice = @SalePrice,
        IsActive = CASE
                      WHEN @Quantity <= 0 OR @ExpirationDate <= GETDATE() THEN 0
                      ELSE @IsActive
                   END
    WHERE StockID = @StockID;
END;