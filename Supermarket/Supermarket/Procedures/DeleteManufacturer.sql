CREATE PROCEDURE DeleteManufacturer
    @ManufacturerId INT
AS
BEGIN
    UPDATE Manufacturers
    SET IsActive = 0
    WHERE ManufacturerId = @ManufacturerId;
END;