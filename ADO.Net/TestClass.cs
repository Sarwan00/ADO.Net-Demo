using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Transactions;
using System.IO;

namespace AdoNetExample.Tests
{
    [TestClass]
    public class ProgramTests
    {
        private string _connectionString;
        private SqlConnection _connection;
        private TransactionScope _transaction;

        [TestInitialize]
        public void Initialize()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;
            _connection = new SqlConnection(_connectionString);
            _transaction = new TransactionScope();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _transaction.Dispose();  // Rollback transaction to not affect the actual database
            _connection.Dispose();   // Dispose connection after each test
        }

        [TestMethod]
        public void TestCreateTable()
        {
            try
            {
                Program.CreateTable(_connection);

                // Assert that the table exists
                using (var cmd = new SqlCommand("SELECT COUNT(*) FROM Department_tbl", _connection))
                {
                    int count = (int)cmd.ExecuteScalar();
                    Assert.AreEqual(0, count);  // Expected count based on CreateTable method
                }
            }
            catch (Exception ex)
            {
                Assert.Fail($"Exception thrown: {ex.Message}");
            }
        }

        [TestMethod]
        public void TestInsertRecords()
        {
            try
            {
                Program.CreateTable(_connection);  // Create table first

                Program.InsertRecords(_connection);

                // Assert that records were inserted
                using (var cmd = new SqlCommand("SELECT COUNT(*) FROM Department_tbl", _connection))
                {
                    int count = (int)cmd.ExecuteScalar();
                    Assert.AreEqual(3, count);  // Expected count based on InsertRecords method
                }
            }
            catch (Exception ex)
            {
                Assert.Fail($"Exception thrown: {ex.Message}");
            }
        }

        [TestMethod]
        public void TestPerformSelectQueries()
        {
            try
            {
                Program.CreateTable(_connection);  // Create table first
                Program.InsertRecords(_connection); // Insert records

                // Capture console output for verification
                using (ConsoleOutputCapture capture = new ConsoleOutputCapture())
                {
                    Program.PerformSelectQueries(_connection);
                    string output = capture.GetOutput();

                    // Assert that specific records are found in output
                    Assert.IsTrue(output.Contains("John Doe,  HR"));
                    Assert.IsTrue(output.Contains("Jane Smith,  IT"));
                    Assert.IsTrue(output.Contains("Mike Johnson,  Finance"));
                }
            }
            catch (Exception ex)
            {
                Assert.Fail($"Exception thrown: {ex.Message}");
            }
        }
    }

    // Helper class to capture Console.WriteLine output
    public class ConsoleOutputCapture : IDisposable
    {
        private StringWriter _stringWriter;
        private TextWriter _originalOutput;

        public ConsoleOutputCapture()
        {
            _stringWriter = new StringWriter();
            _originalOutput = Console.Out;
            Console.SetOut(_stringWriter);
        }

        public string GetOutput()
        {
            return _stringWriter.ToString();
        }

        public void Dispose()
        {
            Console.SetOut(_originalOutput);
            _stringWriter.Dispose();
        }
    }
}
