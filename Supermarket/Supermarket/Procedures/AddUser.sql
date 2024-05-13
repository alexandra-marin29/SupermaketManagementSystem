CREATE PROCEDURE AddUser
    @Username NVARCHAR(50),
    @Password NVARCHAR(50),
    @Role NVARCHAR(10)
AS
BEGIN
    INSERT INTO Users (Username, Password, Role, IsActive)
    VALUES (@Username, @Password, @Role, 1);
END;
