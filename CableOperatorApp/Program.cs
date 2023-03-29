using System;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Xml.Linq;

namespace CableOperatorApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("-------------------------------- Welcome To Cable App ----------------------------------");
            LoginUser();

            Console.ReadLine();
        }

        #region Static Methods
        private static void LoginUser()
        {
           
            int loginOption;
            do
            {
                LoginMenu();
                loginOption = int.Parse(Console.ReadLine());
                switch (loginOption)
                {
                    case 1:
                        GetUser();
                        break;
                    case 2:
                        Environment.Exit(0); 
                        break;

                }

            }
            while (loginOption < 2);
        }
        private static void LoginMenu()
        {
            Console.WriteLine("\n1.Login.");
            Console.WriteLine("2.Exit.");
        }
        private static void GetUser()
        {
            UserStore userStore = new UserStore();
            string enteredUserName = GetUserName();
            string enteredPassword = GetPassWord();

            bool credentialsMatch = false;
            foreach (var userCredentail in UserStore.GetUsers())
            {
                if (enteredUserName == userCredentail.UserName && enteredPassword == userCredentail.Password)
                {
                    credentialsMatch = true;
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid Inputs.Please Try Agian");
                    GetUser();
                }
            }
            if (credentialsMatch)
            {
                Console.WriteLine($"\nWelcome {enteredUserName}\nYou have logged in successfully.");
                while (true)
                {
                    int option;
                    do
                    {
                        Menu();
                        option = int.Parse(Console.ReadLine());
                        switch (option)
                        {
                            case 1:
                                AddProduct();
                                break;
                            case 2:
                                UpdateProduct();
                                break;
                            case 3:
                                SearchProduct();
                                break;
                            case 4:
                                RaiseComplaint();
                                break;
                            case 5:
                                ResolveComPlaint();
                                break;
                                
                            case 6:
                                Console.WriteLine($"{enteredUserName} logged out successfully.");
                                return;
                                
                        }

                    }
                    while (option < 7);
                }
            }
        }

        private static void ResolveComplaint()
        {
            try
            {
                string connectionString = @"data source=KARAN\SQLEXPRESS;database=CableOperatorDB;integrated security=SSPI";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("ResolvedComplaint", connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    Console.Write("Enter the complaint ID to resolve: ");
                    int complaintID = int.Parse(Console.ReadLine());
                    cmd.Parameters.Add("@ComplaintID", SqlDbType.Int).Value = complaintID;

                    Console.Write("Enter the agent ID who resolved the complaint: ");
                    int agentID = int.Parse(Console.ReadLine());
                    cmd.Parameters.Add("@ResolvedByAgent", SqlDbType.Int).Value = agentID;

                    connection.Open();

                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Complaint resolved successfully by agent.");
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception Occurred: {ex.Message}");
            }
        }

        private static void RaiseComplaint()
        {
            try
            {
                string connectionString = @"data source=KARAN\SQLEXPRESS;database=CableOperatorDB;integrated security=SSPI";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("Reaise_Complaint", connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    Console.Write("Enter the customer ID: ");
                    int customerId = Convert.ToInt32(Console.ReadLine());
                    Console.Write("Enter the complaint description: ");
                    string description = Console.ReadLine();

                    cmd.Parameters.Add("@CUSTOMERID", SqlDbType.Int).Value = customerId;
                    cmd.Parameters.Add("@DESCRIPTION", SqlDbType.VarChar).Value = description;

                    connection.Open();
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Complaint raised successfully.");
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception Occurred: {ex.Message}");
            }
        }

        private static void SearchProduct()
        {
            try
            {
                string connectionString = @"data source=KARAN\SQLEXPRESS;database=ProductDB;integrated security=SSPI";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("SEARCH_PRODUCT", connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    Console.Write("Enter the Product ID to search for: ");
                    int productId = Convert.ToInt32(Console.ReadLine());

                    cmd.Parameters.Add("@ProductID", SqlDbType.Int).Value = productId;

                    connection.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        Console.WriteLine("Here are the products:");
                        while (reader.Read())
                        {
                            string productName = reader.GetString(reader.GetOrdinal("ProductName"));
                            DateTime manufactureDate = reader.GetDateTime(reader.GetOrdinal("ManufactureDate"));
                            bool isActive = reader.GetBoolean(reader.GetOrdinal("IsActive"));
                            string categories = reader.GetString(reader.GetOrdinal("Categories"));
                            string specifications = reader.GetString(reader.GetOrdinal("Specifications"));

                            Console.WriteLine($"\nProduct Name : {productName},\nManufacture Date : {manufactureDate},\nProduct is active : {isActive},\nCategories :{categories},\nSpecifications : {specifications}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No products found.");
                    }

                    reader.Close();

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception Occurred: {ex.Message}");
            }
        }

        private static void UpdateProduct()
        {
            try
            {
                string connectionString = @"data source=KARAN\SQLEXPRESS;database=ProductDB;integrated security=SSPI";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("Update_Product", connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    Console.Write("Enter the Product ID: ");
                    int productId = Convert.ToInt32(Console.ReadLine());
                    Console.Write("Enter the Product Name: ");
                    string productName = Console.ReadLine();
                    Console.Write("Enter the Manufacture Date (yyyy-mm-dd): ");
                    DateTime manufactureDate = DateTime.Parse(Console.ReadLine());
                    Console.Write("Enter 1 for Active, 0 for Inactive: ");
                    bool isActive = Convert.ToBoolean(Console.ReadLine());
                    Console.Write("Enter the Category IDs (comma separated): ");
                    string categoryIds = Console.ReadLine();
                    Console.Write("Enter the Specification XML: ");
                    string specificationXml = Console.ReadLine();

                    cmd.Parameters.Add("@ProductID", SqlDbType.Int).Value = productId;
                    cmd.Parameters.Add("@ProductName", SqlDbType.VarChar).Value = productName;
                    cmd.Parameters.Add("@ManufactureDate", SqlDbType.Date).Value = manufactureDate;
                    cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = isActive;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.VarChar).Value = categoryIds;
                    cmd.Parameters.Add("@Specification", SqlDbType.Xml).Value = specificationXml;

                    connection.Open();

                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Product updated successfully.");
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception Occurred: {ex.Message}");
            }
        }

        //private static void InsertProduct()
        //{
        //    try
        //    {
        //        string connectionString = @"data source=KARAN\SQLEXPRESS;database=ProductDB;integrated security=SSPI";
        //        using (SqlConnection connection = new SqlConnection(connectionString))
        //        {
        //            SqlCommand cmd = new SqlCommand()
        //            {
        //                CommandText = "InsertProduct",
        //                Connection = connection,
        //                CommandType = CommandType.StoredProcedure
        //            };
        //            SqlParameter param1 = new SqlParameter
        //            {
        //                ParameterName = "@productName",
        //                SqlDbType = SqlDbType.VarChar,
        //                Direction = ParameterDirection.Input,
        //                Value = "Fridge"
        //            };
        //            SqlParameter param2 = new SqlParameter
        //            {
        //                ParameterName = "@categories",
        //                SqlDbType = SqlDbType.VarChar,
        //                Direction = ParameterDirection.Input,
        //                Value = "1,2"
        //            };
        //            SqlParameter param3 = new SqlParameter
        //            {
        //                ParameterName = "@manufactureDate",
        //                SqlDbType = SqlDbType.Date,
        //                Direction = ParameterDirection.Input,
        //                Value = "2023-02-19"
        //            };
        //            SqlParameter param4 = new SqlParameter
        //            {
        //                ParameterName = "@IsActive",
        //                SqlDbType = SqlDbType.Bit,
        //                Direction = ParameterDirection.Input,
        //                Value = 1
        //            };
        //            SqlParameter param5 = new SqlParameter
        //            {
        //                ParameterName = "@specifications",
        //                SqlDbType = SqlDbType.VarChar,
        //                Direction = ParameterDirection.Input,
        //                Value = "<SPECIFICATION NAME=\"Color\" VALUE=\"Pale Bule\"/>\r\n<SPECIFICATION NAME=\"VERSION\" VALUE=\"6.O\"/>"
        //            };
        //            cmd.Parameters.Add(param1);
        //            cmd.Parameters.Add(param2);
        //            cmd.Parameters.Add(param3);
        //            cmd.Parameters.Add(param4);

        //            connection.Open();
        //            SqlDataReader sdr = cmd.ExecuteReader();

        //            while(sdr.Read())
        //            {
        //                Console.WriteLine(sdr);
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Exception Occurred: {ex.Message}");
        //    }
        //}
        private static void AddProduct()
        {
            try
            {
                Console.WriteLine("Enter the name of the product:");
                string productName = Console.ReadLine();

                Console.WriteLine("Enter the categories (comma-separated):");
                string categories = Console.ReadLine();

                Console.WriteLine("Enter the manufacture date (YYYY-MM-DD):");
                DateTime manufactureDate = DateTime.Parse(Console.ReadLine());

                Console.WriteLine("Enter whether the product is active (1 for active, 0 for inactive):");
                bool isActive = bool.Parse(Console.ReadLine());

                Console.WriteLine("Enter the product specifications:");
                string specifications = Console.ReadLine();

                string connectionString = @"data source=KARAN\SQLEXPRESS;database=ProductDB;integrated security=SSPI";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("InsertProduct", connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@productName", SqlDbType.VarChar).Value = productName;
                    cmd.Parameters.Add("@categories", SqlDbType.VarChar).Value = categories;
                    cmd.Parameters.Add("@manufactureDate", SqlDbType.Date).Value = manufactureDate;
                    cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = isActive;
                    cmd.Parameters.Add("@specifications", SqlDbType.VarChar).Value = specifications;

                    connection.Open();
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Product added successfully.");
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
            }
        }

        private static void Menu()
        {
            Console.WriteLine("\nSelect your choice from follwing menu : ");
            Console.WriteLine("\n1.Add Product.");
            Console.WriteLine("2.Modify Product.");
            Console.WriteLine("3.Search Product.");
            Console.WriteLine("4.Raise Complaint.");
            Console.WriteLine("5.Resolve Complaint.");
            Console.WriteLine("6.Logout.\n");
        }
        private static string GetUserName()
        {
            Console.WriteLine("\nEnter the userName : ");
            string input = Console.ReadLine();
            if (User.IsUserNameValid(input))
            {
                return input;
            }
            else
            {
                Console.WriteLine("\nInvalid UserName.\nPlease Re-enter UserName.");
                return GetUserName();
            }
        }
        private static string GetPassWord()
        {
            Console.WriteLine("\nEnter the password : ");
            string input = Console.ReadLine();
            if (User.IsPasswordValid(input))
            {
                return input;
            }
            else
            {
                Console.WriteLine("\nInvaid PassWord or Password exceeds the character limit.\nPlease Re-enter password.");
                return GetPassWord();
            }
        }


        #endregion

    }
}
