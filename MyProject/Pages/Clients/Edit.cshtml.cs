using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace MyProject.Pages.Clients
{
    public class EditModel : PageModel
    {
		public ClientInfo clientInfo = new ClientInfo();

		// If any error appears during posting create a global variable to show error message
		public string errorMessage = "";
		public string succesMessage = "";
		public void OnGet()
        {
			int id = Convert.ToInt32(Request.Query["@id"]);

			try
			{
				string connectionString = "Data Source=172.20.10.3;Initial Catalog=botas;User ID=admin;Password=1;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;";
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					// Update the row to the table
					string Query = "SELECT * FROM DGaz WHERE id = @id";
					using (SqlCommand command = new SqlCommand(Query, connection))
					{
						command.Parameters.AddWithValue("@id", id);
						using(SqlDataReader reader = command.ExecuteReader())
						{
							if (reader.Read())
							{
								clientInfo.id = reader.GetInt32(0);
								clientInfo.Noktalar = reader.GetString(1);
								clientInfo.Tarih = reader.GetDateTime(2);
								clientInfo.DGdeger = reader.GetInt32(3);
								clientInfo.enlem = reader.GetInt32(4);
								clientInfo.boylam = reader.GetInt32(5);
							}
						}
					}

				}
			}
			catch(Exception ex)
			{
				errorMessage = ex.Message;
			}

        }

		public void OnPost()
		{
			clientInfo.id = Convert.ToInt32(Request.Form["id"]);
			clientInfo.Noktalar = Request.Form["Noktalar"];
			clientInfo.Tarih = Convert.ToDateTime(Request.Form["Tarih"]);
			clientInfo.DGdeger = Convert.ToInt32(Request.Form["DGdeger"]);
			clientInfo.enlem = Convert.ToInt32(Request.Form["Enlem"]);
			clientInfo.boylam = Convert.ToInt32(Request.Form["Boylam"]);

			if (
			   clientInfo.id < 0||clientInfo.Noktalar.Length == 0 || clientInfo.Tarih == null
			   || clientInfo.DGdeger == 0 || clientInfo.enlem == 0 || clientInfo.boylam == 0
			)
			{
				errorMessage = "All fields are required"; return;
			}

			try
			{
				string connectionString = "Data Source=172.20.10.3;Initial Catalog=botas;User ID=admin;Password=1;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;";
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					// Insert new row to the table
					string Query = "UPDATE DGaz " +
						"SET Noktalar=@Noktalar, Tarih=@Tarih,DGdeger=@DGdeger,Enlem=@Enlem,Boylam=@Boylam " +
						"WHERE id=@id";
					using (SqlCommand command = new SqlCommand(Query, connection))
					{
						command.Parameters.AddWithValue("@Noktalar", clientInfo.Noktalar);
						command.Parameters.AddWithValue("@Tarih", clientInfo.Tarih);
						command.Parameters.AddWithValue("@DGdeger", clientInfo.DGdeger);
						command.Parameters.AddWithValue("@Enlem", clientInfo.enlem);
						command.Parameters.AddWithValue("@Boylam", clientInfo.boylam);
						command.Parameters.AddWithValue("@id", clientInfo.id);

						command.ExecuteNonQuery();
					}

				}
			}
			catch (Exception ex)
			{
				errorMessage = ex.Message;
				return;
			}

			Response.Redirect("/Clients/Index");

		}

    }
}
