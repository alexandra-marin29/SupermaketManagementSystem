CREATE PROCEDURE GetPersonByLogin
    @usern NVARCHAR(50),
    @psw NVARCHAR(50)
AS
BEGIN
    SELECT Role
    FROM Users
    WHERE UserName = @usern AND Password = @psw;
END;
