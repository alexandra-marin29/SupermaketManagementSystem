USE [dbSupermarket]
GO
/****** Object:  StoredProcedure [dbo].[DeleteCategory]    Script Date: 18.05.2024 00:06:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[DeleteCategory]
    @CategoryId INT
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Products WHERE CategoryID = @CategoryId AND IsActive = 1)
    BEGIN
        UPDATE Categories
        SET IsActive = 0
        WHERE CategoryId = @CategoryId;
    END
    ELSE
    BEGIN
        RAISERROR('Cannot delete category with existing active products.', 16, 1);
    END
END;