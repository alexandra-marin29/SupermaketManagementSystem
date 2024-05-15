CREATE PROCEDURE EditManufacturer
    @ManufacturerId INT,
    @ManufacturerName NVARCHAR(50),
    @CountryOfOrigin NVARCHAR(50)
AS
BEGIN
    UPDATE Manufacturers
    SET ManufacturerName = @ManufacturerName,
        CountryOfOrigin = @CountryOfOrigin
    WHERE ManufacturerId = @ManufacturerId;
END;