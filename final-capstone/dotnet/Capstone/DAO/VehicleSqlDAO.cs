﻿using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.DAO
{
    public class VehicleSqlDAO : IVehicleDAO
    {
        private readonly string connectionString;
        public VehicleSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }
        public Vehicle AddVehicle(NewVehicle vehicle)
        {

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("INSERT INTO vehicles (vehicle_make, vehicle_model, vehicle_color, license_plate, patron_id) " +
                                                    "VALUES (@vehicle_make, @vehicle_model, @vehicle_color, @license_plate, (SELECT user_id FROM patrons WHERE email_address = @email_address)", conn);
                    cmd.Parameters.AddWithValue("@vehicle_make", vehicle.VehicleMake);
                    cmd.Parameters.AddWithValue("@vehicle_model", vehicle.VehicleModel);
                    cmd.Parameters.AddWithValue("@vehicle_color", vehicle.VehicleColor);
                    cmd.Parameters.AddWithValue("@license_plate", vehicle.LicensePlate);
                    cmd.Parameters.AddWithValue("@email_address", vehicle.EmailAddress);
                    cmd.ExecuteNonQuery();

                    

                    return Get(vehicle.LicensePlate);
                }
            }
            catch (SqlException)
            {
                throw;
            }
        }

        public Vehicle Get(string licensePlate)
        {
            Vehicle car = null;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT license_plate, vehicle_make, vehicle_model, vehicle_color " +
                                                    "FROM vehicles " +
                                                    "WHERE license_plate = @licensePlate", conn);
                    cmd.Parameters.AddWithValue("@licensePlate", licensePlate);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows && reader.Read())
                    {
                        car.LicensePlate = Convert.ToString(reader["license_plate"]);
                        car.VehicleMake = Convert.ToString(reader["vehicle_make"]);
                        car.VehicleModel = Convert.ToString(reader["vehicle_model"]);
                        car.VehicleColor = Convert.ToString(reader["vehicle_color"]);
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }

            return car;
        }
    }
}