CREATE PROCEDURE SearchProducts
    @ProductName NVARCHAR(50) = NULL,
    @Barcode NVARCHAR(50) = NULL,
    @ExpirationDate DATE = NULL,
    @ManufacturerID INT = NULL,
    @CategoryID INT = NULL
AS
BEGIN
    SELECT 
        p.ProductID,
        p.ProductName,
        p.Barcode,
        c.CategoryName,
        m.ManufacturerName,
        ps.ExpirationDate,
        ps.SalePrice,
        ps.Quantity AS StockQuantity
    FROM 
        Products p
        INNER JOIN Categories c ON p.CategoryID = c.CategoryID
        INNER JOIN Manufacturers m ON p.ManufacturerID = m.ManufacturerID
        INNER JOIN ProductStocks ps ON p.ProductID = ps.ProductID
    WHERE 
        p.IsActive = 1
        AND (@ProductName IS NULL OR p.ProductName LIKE '%' + @ProductName + '%')
        AND (@Barcode IS NULL OR p.Barcode = @Barcode)
        AND (@ExpirationDate IS NULL OR ps.ExpirationDate = @ExpirationDate)
        AND (@ManufacturerID IS NULL OR p.ManufacturerID = @ManufacturerID)
        AND (@CategoryID IS NULL OR p.CategoryID = @CategoryID)
    ORDER BY 
        ps.SupplyDate;
END;
