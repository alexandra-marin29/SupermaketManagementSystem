USE [dbSupermarket]
GO
/****** Object:  StoredProcedure [dbo].[DeleteCategory]    Script Date: 14.05.2024 22:56:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[DeleteCategory]
    @CategoryId INT
AS
BEGIN
    UPDATE Categories
    SET IsActive = 0
    WHERE CategoryId = @CategoryId;
END;
