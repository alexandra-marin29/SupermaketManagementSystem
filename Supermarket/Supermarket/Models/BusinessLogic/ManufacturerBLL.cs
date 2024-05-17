using System;
using System.Collections.Generic;
using Supermarket.Models.DataAccessLayer;
using Supermarket.Models.EntityLayer;

namespace Supermarket.Models.BusinessLogic
{
    public class ManufacturerBLL
    {
        private ManufacturerDAL manufacturerDAL = new ManufacturerDAL();

        public List<Manufacturer> GetAllManufacturers()
        {
            return manufacturerDAL.GetAllManufacturers();
        }

        public void AddManufacturer(Manufacturer manufacturer)
        {
            manufacturerDAL.AddManufacturer(manufacturer);
        }

        public void EditManufacturer(Manufacturer manufacturer)
        {
            if (!manufacturerDAL.HasProducts(manufacturer.ManufacturerID))
            {
                manufacturerDAL.EditManufacturer(manufacturer);
            }
            else
            {
                throw new Exception("Cannot edit manufacturer with existing products.");
            }
        }

        public void DeleteManufacturer(int manufacturerId)
        {
            try
            {
                manufacturerDAL.DeleteManufacturer(manufacturerId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
