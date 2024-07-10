using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
namespace ADO.Net
{
    public class Program
    {
        static void Main()
        {
            string connectionString = "Server=DESKTOP-AVINVCV\\SQLEXPRESS;Database=tbl_employee;User Id=sa;Password=Sarwan@12;";

            // Inserting a new employee
            InsertEmployee(connectionString, 7,"Sarwan", "Yadav", "Dev", new DateTime(2020, 01, 15), 50000.00m);

            // Displaying all employees after insertion
            DisplayEmployees(connectionString);

            Console.ReadLine(); // Keep console window open
        }

        static void InsertEmployee(string connectionString,int employeeID, string firstName, string lastName, string department, DateTime joinDate, decimal salary)
        {
            string sqlInsert = "INSERT INTO Employee (EmployeeID,FirstName, LastName, Department, JoinDate, Salary) " +
                               "VALUES (@employeeID,@firstName, @lastName, @department, @joinDate, @salary)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlInsert, connection);

                // Add parameters to the command
                command.Parameters.AddWithValue("@EmployeeID", employeeID);
                command.Parameters.AddWithValue("@FirstName", firstName);
                command.Parameters.AddWithValue("@LastName", lastName);
                command.Parameters.AddWithValue("@Department", department);
                command.Parameters.AddWithValue("@JoinDate", joinDate);
                command.Parameters.AddWithValue("@Salary", salary);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine($"Rows Inserted: {rowsAffected}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        static void DisplayEmployees(string connectionString)
        {
            string sqlQuery = "SELECT * FROM Employee";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlQuery, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        // Assuming your table has columns in this order: EmployeeID, FirstName, LastName, Department, HireDate, Salary
                        int employeeId = reader.GetInt32(0); // Assuming EmployeeID is the first column and of type int
                        string firstName = reader.GetString(1); // Assuming FirstName is the second column and of type string
                        string lastName = reader.GetString(2); // Assuming LastName is the third column and of type string
                        string department = reader.GetString(3); // Assuming Department is the fourth column and of type string
                        DateTime joinDate = reader.GetDateTime(4); // Assuming HireDate is the fifth column and of type DateTime
                        decimal salary = reader.GetDecimal(5); // Assuming Salary is the sixth column and of type decimal

                        // Format the output as required
                        Console.WriteLine($"{employeeId}\t{firstName}\t{lastName}\t{department}\t{joinDate.ToShortDateString()}\t{salary.ToString("N2")}");
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }
    }





















    //    static void Main(string[] args)
    //    {
    //        string connectionString = "Server=DESKTOP-AVINVCV\\SQLEXPRESS;Database=tbl_employee;User Id=sa;Password=Sarwan@12;";

    //        using (SqlConnection connection = new SqlConnection(connectionString))
    //        {
    //            string sqlQuery = "SELECT * FROM Employee"; // Replace TableName with your actual table name

    //            SqlCommand command = new SqlCommand(sqlQuery, connection);

    //            try
    //            {
    //                connection.Open();

    //                SqlDataReader reader = command.ExecuteReader();

    //                while (reader.Read())
    //                {
    //                    // Assuming your table has columns in this order: EmployeeID, FirstName, LastName, Department, HireDate, Salary
    //                    int employeeId = reader.GetInt32(0); // Assuming EmployeeID is the first column and of type int
    //                    string firstName = reader.GetString(1); // Assuming FirstName is the second column and of type string
    //                    string lastName = reader.GetString(2); // Assuming LastName is the third column and of type string
    //                    string department = reader.GetString(3); // Assuming Department is the fourth column and of type string
    //                    DateTime hireDate = reader.GetDateTime(4); // Assuming HireDate is the fifth column and of type DateTime
    //                    decimal salary = reader.GetDecimal(5); // Assuming Salary is the sixth column and of type decimal

    //                    // Format the output as required
    //                    Console.WriteLine($"{employeeId}\t{firstName}\t{lastName}\t{department}\t{hireDate.ToShortDateString()}\t{salary.ToString("N2")}");

    //                }

    //                reader.Close();
    //            }
    //            catch (Exception ex)
    //            {
    //                Console.WriteLine(ex.Message);
    //            }
    //            finally
    //            {
    //                connection.Close();
    //            }
    //        }

    //        Console.ReadLine();




    //}
}

