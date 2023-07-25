using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;
using System.Data.SqlClient;

namespace MyProject.Pages.Clients
{
    public class CreateModel : PageModel
    {
        public ClientInfo clientInfo = new ClientInfo();

        // If any error appears during posting create a global variable to show error message
        public string errorMessage = "";
        public string succesMessage = "";
        public void OnGet()
        {
		}

        public int GetID()
        {
		   List<ClientInfo> ListClients = new List<ClientInfo>();

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
					using (SqlCommand command = new SqlCommand(query, connection))
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

			return ListClients[ListClients.Count - 1].id;
        }


        public void OnPost()
        {
			clientInfo.id = GetID();
			clientInfo.id++;

            clientInfo.Noktalar = Request.Form["Noktalar"];
			clientInfo.Tarih = Convert.ToDateTime(Request.Form["Tarih"]);
			clientInfo.DGdeger = Convert.ToInt32(Request.Form["DGdeger"]);
			clientInfo.enlem = Convert.ToInt32(Request.Form["Enlem"]);
			clientInfo.boylam = Convert.ToInt32(Request.Form["Boylam"]);

            // Error Condition statement

            if(
                clientInfo.Noktalar.Length == 0 || clientInfo.Tarih == null
                || clientInfo.DGdeger == 0 || clientInfo.enlem == 0 || clientInfo.boylam == 0    
             )
            {
                errorMessage = "All fields are required";return;
            }

            // Save the new form to database

            try
            {
                // Connect to database
                string connectionString = "Data Source=172.20.10.3;Initial Catalog=botas;User ID=admin;Password=1;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;";
				using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    // Insert new row to the table
                    string Query = "INSERT INTO DGaz " +
                        "(id,Noktalar,Tarih,DGdeger,Enlem,Boylam) VALUES " +
                        "(@id,@Noktalar,@Tarih,@DGdeger,@Enlem,@Boylam);";
					using (SqlCommand command = new SqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue("@id", clientInfo.id);
                        command.Parameters.AddWithValue("@Noktalar", clientInfo.Noktalar);
						command.Parameters.AddWithValue("@Tarih", clientInfo.Tarih);
						command.Parameters.AddWithValue("@DGdeger", clientInfo.DGdeger);
						command.Parameters.AddWithValue("@Enlem", clientInfo.enlem);
						command.Parameters.AddWithValue("@Boylam", clientInfo.boylam);

                        command.ExecuteNonQuery();
					}

				}


			}
            catch(Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }



            //Clear the all variables
            clientInfo.Noktalar = "";
            clientInfo.Tarih = null;
            clientInfo.DGdeger = 0;
            clientInfo.enlem = 0;
            clientInfo.boylam = 0;
            succesMessage = "Yeni Gaz akisi eklendi!!";

            Response.Redirect("/Clients/Index");
		}

	}
}
