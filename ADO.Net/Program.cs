using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace AdoNetExample
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;

                // Create a connection
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    Console.WriteLine("Checking if table already exists ");
                    DropTable(connection);
                    // Create a table
                    CreateTable(connection);

                    // Insert records
                    InsertRecords(connection);

                    // Perform select queries
                    PerformSelectQueries(connection);
                }
            }
            catch(Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
       public  static void DropTable(SqlConnection connection)
        {
            string DropTable = @"DROP Table Department_tbl";
            SqlCommand command = new SqlCommand(DropTable, connection);
            int RowAffected = command.ExecuteNonQuery();
            if (RowAffected > 0)

            Console.WriteLine("Table Dropped successfully." + "," + "RowAffected:" + RowAffected);
            Console.WriteLine("Table not Exists");



        }

        public static void CreateTable(SqlConnection connection)
        {
            string createTableQuery = @"
                CREATE TABLE Department_tbl (
                    Id INT PRIMARY KEY,
                    Name NVARCHAR(100),
                    Department NVARCHAR(100)
                )";

            SqlCommand command = new SqlCommand(createTableQuery, connection);
            int RowAffected =command.ExecuteNonQuery();

            Console.WriteLine("Table 'Employees' created successfully."+ ","+ "RowAffected:"+ RowAffected);
        }

        public static void InsertRecords(SqlConnection connection)
        {
            string insertQuery = @"
                INSERT INTO Department_tbl (Id, Name, Department)
                VALUES 
                    (1, 'John Doe', 'HR'),
                    (2, 'Jane Smith', 'IT'),
                    (3, 'Mike Johnson', 'Finance')";

            SqlCommand command = new SqlCommand(insertQuery, connection);
            int rowsAffected = command.ExecuteNonQuery();

            Console.WriteLine($"{rowsAffected} row(s) inserted into 'Employees' table.");
        }

           public  static void PerformSelectQueries(SqlConnection connection)
        {
            string selectQuery = @"SELECT Id, Name, Department FROM Department_tbl";
            try
            {
                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {

                    using (SqlDataReader sdr = command.ExecuteReader())
                    {
                        Console.WriteLine("Records in 'Department' table:");
                        Console.WriteLine("--------------------------------------");
                        while (sdr.Read())
                        {
                            Console.WriteLine(sdr[0] + ",  " + sdr[1] + ",  " + sdr[2]);
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

    }
}
