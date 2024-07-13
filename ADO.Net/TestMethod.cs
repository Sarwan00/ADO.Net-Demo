using System;
using System.Data.SqlClient;
using NUnit.Framework;
using Moq;

namespace AdoNetExample.Tests
{
    [TestClass]
    public class ProgramTests
    {
        [TestMethod]
        public void TestAdoNetOperations()
        {
            // Mock SqlConnection
            var mockConnection = new Mock<SqlConnection>("");

            // Mock SqlCommand for CreateTable
            var mockCreateCommand = new Mock<SqlCommand>();
            mockCreateCommand.Setup(cmd => cmd.ExecuteNonQuery()).Verifiable();
            mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCreateCommand.Object);

            // Mock SqlCommand for InsertRecords
            var mockInsertCommand = new Mock<SqlCommand>();
            mockInsertCommand.Setup(cmd => cmd.ExecuteNonQuery()).Returns(3); // 3 rows affected
            mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockInsertCommand.Object);

            // Mock SqlCommand for PerformSelectQueries
            var mockSelectCommand = new Mock<SqlCommand>();
            var mockReader = new Mock<SqlDataReader>();
            mockReader.SetupSequence(r => r.Read())
                .Returns(true).Returns(true).Returns(true) // Simulate 3 rows
                .Returns(false); // End of data
            mockReader.Setup(r => r["Id"]).Returns(1).Returns(2).Returns(3); // Simulate Id values
            mockReader.Setup(r => r["Name"]).Returns("John Doe").Returns("Jane Smith").Returns("Mike Johnson"); // Simulate Name values
            mockReader.Setup(r => r["Department"]).Returns("HR").Returns("IT").Returns("Finance"); // Simulate Department values
            mockSelectCommand.Setup(cmd => cmd.ExecuteReader()).Returns(mockReader.Object);
            mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockSelectCommand.Object);

            // Test the program flow
            using (var program = new Program())
            {
                // Set the connection string in the program (not ideal, but for simplicity here)
                typeof(Program).GetField("connectionString", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic).SetValue(null, "");

                // Execute methods
                program.CreateTable(mockConnection.Object);
                program.InsertRecords(mockConnection.Object);
                program.PerformSelectQueries(mockConnection.Object);
            }

            // Verify expectations
            mockCreateCommand.Verify(cmd => cmd.ExecuteNonQuery(), Times.Once);
            mockInsertCommand.Verify(cmd => cmd.ExecuteNonQuery(), Times.Once);
            mockSelectCommand.Verify(cmd => cmd.ExecuteReader(), Times.Once);
        }
    }
}
