USE [dbSupermarket]
GO
/****** Object:  StoredProcedure [dbo].[EditCategory]    Script Date: 17.05.2024 21:14:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[EditCategory]
    @CategoryId INT,
    @CategoryName NVARCHAR(50)
AS
BEGIN
     IF NOT EXISTS (SELECT 1 FROM Products WHERE CategoryID = @CategoryId)
    BEGIN
        UPDATE Categories
        SET CategoryName = @CategoryName
        WHERE CategoryId = @CategoryId;
    END
    ELSE
    BEGIN
        RAISERROR('Cannot edit category with existing products.', 16, 1);
    END
END;