CREATE PROCEDURE GetAllManufacturers
AS
BEGIN
    SELECT ManufacturerID, ManufacturerName FROM Manufacturers WHERE IsActive = 1;
END;
