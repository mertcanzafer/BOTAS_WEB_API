using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;


namespace MyProject.Pages.Clients
{
    public class IndexModel : PageModel
    {
        public List<ClientInfo> ListClients = new List<ClientInfo>();
        public void OnGet()
        {
            try
            {
                string connectionString = "Data Source=172.20.10.3;Initial Catalog=botas;User ID=admin;Password=1;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;";
                Console.WriteLine("Check error0");

				using (SqlConnection connection = new SqlConnection(connectionString))
                {
					Console.WriteLine("Check error1");
					connection.Open();
					Console.WriteLine("Check error2");
					string query = "Select * from DGaz"; // This will be changed!!!
                    using (SqlCommand command = new SqlCommand(query,connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            Console.WriteLine("Check error3");
                            while (reader.Read())
                            {
                                ClientInfo clientInfo = new ClientInfo();
                                clientInfo.id = reader.GetInt32(0);
                                clientInfo.Noktalar = reader.GetString(1);
                                clientInfo.Tarih = reader.GetDateTime(2);
                                clientInfo.DGdeger = reader.GetInt32(3);
                                clientInfo.enlem = reader.GetInt32(4);
                                clientInfo.boylam = reader.GetInt32(5);
                                ListClients.Add(clientInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: Database'e baðlanýlmadý");
            }
        }
    }

    public class ClientInfo
    {
        // Our Table Variables
       public int id;
       public string Noktalar;
       public DateTime? Tarih;
       public int DGdeger;
       public int enlem;
       public int boylam;
    }
}
