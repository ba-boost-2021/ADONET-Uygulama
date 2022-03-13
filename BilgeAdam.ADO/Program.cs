using System.Configuration;
using System.Data.SqlClient;

namespace BilgeAdam.ADO
{
    internal class Program
    {
        public static void Main(string[] arg)
        {
            //BasicConnection();
            //ConnectionWithAppConfig();

            //Soru1(); // Disposable olan nesneleri Dispose ediyor (GC ile)

            //Soru2();

            //// Execute Non Query
            //using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["NrthwConnectionString"].ConnectionString))
            //{
            //    connection.Open();
            //    var query = @"INSERT INTO Categories VALUES ('Giyim', 'Test verieisidir.', NULL)";

            //    var command = new SqlCommand(query, connection);
            //    var result = command.ExecuteNonQuery();
            //    if(result > 0)
            //    {
            //        Console.WriteLine("kategori verisi başarıyla eklendi.");
            //    }
            //}

            //Soru3();
        }

        private static void Soru3()
        {
            //User tablosu oluşturun. FirstName, LastName, Email, Password, CreatedAt, CreatedBy

            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["NrthwConnectionString"].ConnectionString))
            {
                try
                {
                    connection.Open();
                    var query = @"CREATE TABLE Users(
                            Id INT PRIMARY KEY IDENTITY(1,1),
                            FirstName NVARCHAR(64) NOT NULL,
                            LastName NVARCHAR(64) NOT NULL,
                            Email NVARCHAR(64) NOT NULL,
                            Password NVARCHAR(128) NOT NULL,
                            CreatedAt DATETIME NOT NULL,
                            CreatedBy NVARCHAR(64) NULL
                    )";

                    var command = new SqlCommand(query, connection);
                    var result = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private static void Soru2()
        {
            // Tedarikçileri listeleyiniz (Bir DTO/Model oluşturup bir listede tutarak daha sonra farklı bir mothod ile konsola yazınız.)

            var data = new List<SupplierViewDto>();
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["NrthwConnectionString"].ConnectionString))
            {
                var query = @"SELECT SupplierID,
                                     CompanyName,
                                     ContactName,
                                     ContactTitle,
                                     City,
                                     Country FROM Suppliers";
                connection.Open();

                var command = new SqlCommand(query, connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        data.Add(new SupplierViewDto()
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            ContactName = reader.GetString(2),
                            ContactTitle = reader.GetString(3),
                            City = reader.GetString(4),
                            Country = reader.GetString(5),
                        });
                    }
                }
            }
            DisplayData(data);
        }

        private static void DisplayData(List<SupplierViewDto> data)
        {
            foreach (var supplier in data)
            {
                Console.WriteLine(supplier);
            }
        }

        private static void Soru1()
        {
            // Hangi kategoride kaç ürün olduğunu console'a listeleyiniz.
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["NrthwConnectionString"].ConnectionString))
            {
                var query = @"SELECT c.CategoryName, Count(c.CategoryID) as ProductCount FROM Categories c
                            INNER JOIN Products p ON p.CategoryID = c.CategoryID
                            GROUP BY c.CategoryName";
                connection.Open();

                var command = new SqlCommand(query, connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["CategoryName"]} --> {reader["ProductCount"]}");
                    }
                }
            }
        }

        #region Uygulama

        private static void ConnectionWithAppConfig()
        {
            var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["NrthwConnectionString"].ConnectionString);
            //var connection2 = new SqlConnection(ConfigurationManager.ConnectionStrings["LibMConnectionString"].ConnectionString);

            connection.Open();

            var query = "SELECT ProductID, ProductName FROM Products";
            var command = new SqlCommand(query, connection);

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                //Console.WriteLine($"{reader.GetInt32(0)} ----> {reader.GetString(1)}");
                Console.WriteLine($"{Convert.ToInt32(reader["ProductID"])} ----> {reader["ProductName"]}");
            }
        }

        private static void BasicConnection()
        {
            var connectionString = "Server=localhost,20000;Database=Northwind;User Id=sa;Password=1q2w3e4R!;";
            var connection = new SqlConnection(connectionString);
            connection.Open();

            var query = "SELECT ProductName FROM Products";
            var command = new SqlCommand(query, connection);

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine(reader.GetString(0));
                //Console.WriteLine(reader["ProductName"]);
                //Console.WriteLine(reader[0].ToString());
            }

            reader.Close();
            connection.Close();
            connection.Dispose();
        }

        #endregion Uygulama
    }
}

// Connected Mimaride (connection'ı kendimizin yönettiği mimari)

// Bağlantıyı oluştur
// Sorguyu oluştur
// Bağlantıyı aç
// Sorguyu çalıştır
// Bağlantıyı kapat