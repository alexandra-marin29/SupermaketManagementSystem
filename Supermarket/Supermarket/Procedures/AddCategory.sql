USE [dbSupermarket]
GO
/****** Object:  StoredProcedure [dbo].[AddCategory]    Script Date: 14.05.2024 22:47:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[AddCategory]
    @CategoryName NVARCHAR(50)
AS
BEGIN
    INSERT INTO Categories (CategoryName,IsActive)
    VALUES (@CategoryName,1);
END;
