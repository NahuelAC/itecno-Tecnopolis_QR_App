using System;
using System.Data.SqlClient;

namespace Tecnopolis_QR_App
{
    public class DatabaseOnline
    {
        private string DataSource = "<DatabaseName>";
        private string UserID = "<Username>";
        private string Password = "<Password>";
        
        public bool GetClientByDni(string dni, DateTime dt)
        {
            bool response = false;
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
                {
                    DataSource = this.DataSource,
                    UserID = this.UserID,
                    Password = this.Password,
                    InitialCatalog = "<Initial table>"
                };
                using (SqlConnection con = new SqlConnection(builder.ConnectionString))
                {
                    string query = $"select * from <table> where <dni>='{dni}'";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (dni == reader.GetString(-1) && dt == reader.GetDateTime(-1))
                                {
                                    response = true;
                                    break;
                                }
                                else
                                {
                                    response = false;
                                }
                            }

                        }
                    }
                    
                }
            }
            catch
            {
                response = false;
            }

            return response;
        }
        
        // public bool UploadLocalDbData()
        // {
        //     bool response = false;
        //     try
        //     {
        //         SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
        //         {
        //             DataSource = this.DataSource,
        //             UserID = this.UserID,
        //             Password = this.Password,
        //             InitialCatalog = "<Initial table>"
        //         };
        //         using (SqlConnection con = new SqlConnection(builder.ConnectionString))
        //         {
        //             string query = $"";
        //             using (SqlCommand cmd = new SqlCommand(query, con))
        //             {
        //                 con.Open();
        //                 using (SqlDataReader reader = cmd.ExecuteReader())
        //                 {
        //                     while (reader.Read())
        //                     {
        //                         if (dni == reader.GetString(-1) && dt == reader.GetDateTime(-1))
        //                         {
        //                             response = true;
        //                             break;
        //                         }
        //                         else
        //                         {
        //                             response = false;
        //                         }
        //                     }
        //
        //                 }
        //             }
        //             
        //         }
        //     }
        //     catch
        //     {
        //         response = false;
        //     }
        //
        //     return response;
        // }
    }
}