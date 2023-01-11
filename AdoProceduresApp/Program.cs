using System.Data;
using System.Data.SqlClient;

namespace AdoProceduresApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=LibraryDb;Integrated Security=True;Connect Timeout=10;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                // create procudere and add to LibraryDb
                /*
                string proc1 = @"CREATE PROCEDURE [dbo].[InsertBook]
	                                @title NVARCHAR(100),
	                                @author NVARCHAR(100),
	                                @price MONEY = 0
                                AS
	                                INSERT INTO books (title, author, price)
		                                VALUES(@title, @author, @price)

	                                SELECT SCOPE_IDENTITY()

                                GO";
                string proc2 = @"CREATE PROCEDURE [dbo].[GetBooks]
                                    AS
	                                    SELECT * FROM books

                                    GO";

                SqlCommand command = sqlConnection.CreateCommand();
                command.CommandText = proc1;
                command.ExecuteNonQuery();

                command.CommandText = proc2;
                command.ExecuteNonQuery();
                

                string proc3 = @"CREATE PROCEDURE [dbo].[GetPriceRange]
                                    @priceMin INT OUT,
                                    @priceMax INT OUT
                                 AS
                                    SELECT @priceMin = MIN(price), 
                                           @priceMax = MAX(price)
                                        FROM books";
                SqlCommand command = sqlConnection.CreateCommand();
                command.CommandText = proc3;
                command.ExecuteNonQuery();
                */

                /*
                Console.Write("input title: ");
                string title = Console.ReadLine();
                Console.Write("input author: ");
                string author = Console.ReadLine();
                Console.Write("input price: ");
                double price = Double.Parse(Console.ReadLine());

                SqlCommand command = sqlConnection.CreateCommand();
                command.CommandText = "InsertBook";
                command.CommandType = CommandType.StoredProcedure;
                SqlParameter titleParam = new()
                {
                    ParameterName = "@title",
                    Value = title,
                    SqlDbType = SqlDbType.NVarChar,
                };
                SqlParameter authorParam = new()
                {
                    ParameterName = "@author",
                    Value = author,
                    SqlDbType = SqlDbType.NVarChar,
                };
                SqlParameter priceParam = new()
                {
                    ParameterName = "@price",
                    Value = price,
                    SqlDbType = SqlDbType.Money,
                };
                command.Parameters.AddRange(new[] { titleParam, authorParam, priceParam });

                var resultId = command.ExecuteScalar();
                Console.WriteLine($"insert book with id: {resultId}");
                Console.ReadKey();
                */

                SqlCommand command = sqlConnection.CreateCommand();
                command.CommandText = "GetBooks";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Clear();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    //Console.WriteLine($"{reader.GetName(0)}\t{reader.GetName(1)}\t{reader.GetName(2)}\t{reader.GetName(3)}\t");
                    for (int i = 0; i < reader.FieldCount; i++)
                        Console.Write($"{reader.GetName(i)}\t");
                    Console.WriteLine();
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                            Console.Write($"{reader.GetValue(i)}\t");
                        Console.WriteLine();
                    }
                }

                command.CommandText = "GetPriceRange";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Clear();

                SqlParameter priceMin = new()
                {
                    ParameterName = "@priceMin",
                    SqlDbType = SqlDbType.Money,
                    Direction = ParameterDirection.Output,
                };
                SqlParameter priceMax = new()
                {
                    ParameterName = "@priceMax",
                    SqlDbType = SqlDbType.Money,
                    Direction = ParameterDirection.Output,
                };
                command.Parameters.AddRange(new[] { priceMin, priceMax });
                command.ExecuteNonQuery();
                var priceMinValue = command.Parameters["@priceMin"].Value;
                var priceMaxValue = command.Parameters["@priceMax"].Value;

                Console.WriteLine(priceMinValue + " " + priceMaxValue);
            }
        }
    }
}