using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using jwtvalidator.Models;
using jwtvalidator.Repository.Interfaces;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace jwtvalidator.Repository

{
    public class loginModel : IloginModel
    {
        string cstr = @"Data Source=APINP-ELPTOH9CG\SQLEXPRESS;Initial Catalog = studentdb; User ID = tap2023; Password=tap2023;Connect Timeout = 30; Encrypt=False";

        public loginModelForm getuser(string username)
        {
            Console.WriteLine("here" +username);
            // Establish connection to the database
            using (SqlConnection connection = new SqlConnection(cstr))
            {
                                loginModelForm user = new loginModelForm();
                try
                {
                    connection.Open();

                    // Create SQL command to retrieve user details
                    string sql = "SELECT username, password FROM login WHERE username = @username";

                    // Use parameterized query to prevent SQL injection
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);

                        // Execute the query and retrieve results
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Create loginModel object and assign values
                               
                                user.username = reader["username"].ToString();
                                user.password = reader["password"].ToString();

                                  // Return the user object
                            }
                            else
                            {
                                // User not found, handle appropriately (e.g., return null or throw an exception)
                                return null;
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    // Handle database errors gracefully
                    Console.WriteLine("Error connecting to database: " + ex.Message);
                    throw;  // Rethrow the exception to allow for further handling
                }
                return user;
            }

        }

    }
}
