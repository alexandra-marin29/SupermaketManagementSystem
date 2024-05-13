CREATE PROCEDURE EditUser
    @UserId INT,
    @Username NVARCHAR(50),
    @Password NVARCHAR(50),
    @Role NVARCHAR(10)
AS
BEGIN
    UPDATE Users
    SET Username = @Username,
        Password = @Password,
        Role = @Role
    WHERE UserId = @UserId AND IsActive = 1;
END;
