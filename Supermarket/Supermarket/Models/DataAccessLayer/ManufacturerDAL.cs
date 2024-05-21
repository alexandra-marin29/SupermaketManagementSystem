using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Supermarket.Models.EntityLayer;

namespace Supermarket.Models.DataAccessLayer
{
    public class ManufacturerDAL
    {
        public List<Manufacturer> GetAllManufacturers()
        {
            List<Manufacturer> manufacturers = new List<Manufacturer>();

            using (SqlConnection con = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Manufacturers WHERE IsActive = 1", con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Manufacturer manufacturer = new Manufacturer
                    {
                        ManufacturerID = (int)reader["ManufacturerId"],
                        ManufacturerName = reader["ManufacturerName"].ToString(),
                        CountryOfOrigin = reader["CountryOfOrigin"].ToString(),
                        IsActive = (bool)reader["IsActive"]
                    };
                    manufacturers.Add(manufacturer);
                }
            }

            return manufacturers;
        }

        public void AddManufacturer(Manufacturer manufacturer)
        {
            using (SqlConnection con = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("AddManufacturer", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@ManufacturerName", manufacturer.ManufacturerName);
                cmd.Parameters.AddWithValue("@CountryOfOrigin", manufacturer.CountryOfOrigin);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void EditManufacturer(Manufacturer manufacturer)
        {
            using (SqlConnection con = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("EditManufacturer", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@ManufacturerId", manufacturer.ManufacturerID);
                cmd.Parameters.AddWithValue("@ManufacturerName", manufacturer.ManufacturerName);
                cmd.Parameters.AddWithValue("@CountryOfOrigin", manufacturer.CountryOfOrigin);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteManufacturer(int manufacturerId)
        {
            using (SqlConnection con = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("DeleteManufacturer", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@ManufacturerId", manufacturerId);

                con.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public bool HasProducts(int manufacturerId)
        {
            using (SqlConnection con = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Products WHERE ManufacturerID = @ManufacturerId", con);
                cmd.Parameters.AddWithValue("@ManufacturerId", manufacturerId);

                con.Open();
                int count = (int)cmd.ExecuteScalar();

                return count > 0;
            }
        }

    }
}
