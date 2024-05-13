CREATE PROCEDURE DeleteUser
    @UserId INT
AS
BEGIN
    UPDATE Users
    SET IsActive = 0
    WHERE UserId = @UserId;
END;
