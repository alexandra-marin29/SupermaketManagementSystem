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
            manufacturerDAL.EditManufacturer(manufacturer);
        }

        public void DeleteManufacturer(int manufacturerId)
        {
            manufacturerDAL.DeleteManufacturer(manufacturerId);
        }
    }
}
