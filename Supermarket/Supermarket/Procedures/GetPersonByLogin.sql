USE [dbSupermarket]
GO
/****** Object:  StoredProcedure [dbo].[GetPersonByLogin]    Script Date: 15.05.2024 19:36:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[GetPersonByLogin]
    @usern NVARCHAR(50),
    @psw NVARCHAR(50)
AS
BEGIN
    SELECT UserID, Username, Role, IsActive
    FROM Users
    WHERE Username = @usern AND Password = @psw
END;
