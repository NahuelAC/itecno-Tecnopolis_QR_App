using System;
using System.Data.SqlClient;

namespace Tecnopolis_QR_App
{
    public class DatabaseOnline
    {
        public bool getInfo(string dni, DateTime dt)
        {
            bool response = false;
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
                {
                    DataSource = "<DatabaseName>",
                    UserID = "<Username>",
                    Password = "<Password>",
                    InitialCatalog = "<Initial table>"
                };
                using (SqlConnection con = new SqlConnection(builder.ConnectionString))
                {
                    string query = "";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                                if (dni == reader.GetString(-1) && dt == reader.GetDateTime(-1))
                                    response = true;
                                else
                                    response = false;
                            else
                                response = false;

                        }
                    }
                    
                }
            }
            catch (Exception e)
            {
                response = false;
            }

            return response;
        }
    }
}