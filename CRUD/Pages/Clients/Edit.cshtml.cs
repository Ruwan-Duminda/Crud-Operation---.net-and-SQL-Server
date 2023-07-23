using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace CRUD.Pages.Clients
{
    public class EditModel : PageModel
    {
		public ClientInfo clientInfo = new ClientInfo();
		public String errorMessage = "";
		public String successMessage = "";
		public void OnGet()
        {
			String id = Request.Query["ID"];

			try
			{
				String connectionString = "Data Source=LAPTOP-A01CF880;Initial Catalog=crud;Integrated Security=True";
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					String sql = "SELECT * FROM clients WHERE id=@id";
					using(SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@id", id);

						using(SqlDataReader reader = command.ExecuteReader())
						{
							if(reader.Read())
							{
								clientInfo.id=""+reader.GetInt32(0);
								clientInfo.name=reader.GetString(1);
								clientInfo.email=reader.GetString(2);
								clientInfo.phone=reader.GetString(3);
								clientInfo.address=reader.GetString(4);
							}
						}
					}

				}

			}catch (Exception ex)
			{
				errorMessage = ex.Message;
			}
        }

		public void OnPost()
		{
			clientInfo.id = Request.Form["id"];
			clientInfo.name = Request.Form["name"];
			clientInfo.email = Request.Form["email"];
			clientInfo.phone = Request.Form["phone"];
			clientInfo.address = Request.Form["address"];

			if (clientInfo.name.Length == 0 || clientInfo.email.Length == 0 ||
				clientInfo.phone.Length == 0 || clientInfo.address.Length == 0)
			{
				errorMessage = "All the field are Required";
				return;
			}

			try
			{
				String connectionString = "Data Source=LAPTOP-A01CF880;Initial Catalog=crud;Integrated Security=True";
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();

					String sql = "UPDATE clients" +
						" SET name=@name,email=@email,phone=@phone,address=@address " +
						"WHERE id=@id";

					using(SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@name", clientInfo.name);
						command.Parameters.AddWithValue("@email", clientInfo.email);
						command.Parameters.AddWithValue("@phone", clientInfo.phone);
						command.Parameters.AddWithValue("@address", clientInfo.address);
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
			successMessage = "Clients Info Edited Successfully";
			Response.Redirect("/Clients/Index?successMessage="+successMessage);
		}

		public class ClientInfo
		{
			public String id;
			public String name;
			public String email;
			public String phone;
			public String address;
			public String created_at;
		}
	}
}
