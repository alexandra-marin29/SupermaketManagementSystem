USE [dbSupermarket]
GO
/****** Object:  StoredProcedure [dbo].[DeleteManufacturer]    Script Date: 17.05.2024 21:39:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[DeleteManufacturer]
    @ManufacturerId INT
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Products WHERE ManufacturerID = @ManufacturerId)
    BEGIN
        RAISERROR('Cannot delete manufacturer with existing products.', 16, 1);
    END
    ELSE
    BEGIN
        UPDATE Manufacturers
        SET IsActive = 0
        WHERE ManufacturerId = @ManufacturerId;
    END
END;