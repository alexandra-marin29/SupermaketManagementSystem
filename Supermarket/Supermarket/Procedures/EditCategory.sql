CREATE PROCEDURE EditCategory
    @CategoryId INT,
    @CategoryName NVARCHAR(50)
AS
BEGIN
    UPDATE Categories
    SET CategoryName = @CategoryName
    WHERE CategoryId = @CategoryId;
END;
