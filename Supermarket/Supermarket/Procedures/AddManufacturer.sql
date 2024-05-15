CREATE PROCEDURE AddManufacturer
    @ManufacturerName NVARCHAR(50),
    @CountryOfOrigin NVARCHAR(50)
AS
BEGIN
    INSERT INTO Manufacturers (ManufacturerName, CountryOfOrigin, IsActive)
    VALUES (@ManufacturerName, @CountryOfOrigin, 1);
END;