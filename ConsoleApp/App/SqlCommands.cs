using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ConsoleApp.App.Config;
using ConsoleApp.App.Models;

namespace ConsoleApp.App
{
    internal class SqlCommands
    {
        public static void GetAllSampleData()
        {
            using SqlConnection conn = new(AppConfigs.ConnectionBuilder.ConnectionString);

            using SqlCommand command = new()
            {
                CommandText = "SELECT * FROM People;",
                CommandType = CommandType.Text,
                Connection = conn
            };

            conn.Open();

            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine(reader["FirstName"] + " " + reader["LastName"]);
                Console.WriteLine();
            }

            conn.Close();
        }

        public static void InsertPeopleList(List<Person> people)
        {
            Console.WriteLine("InsertPeopleList called. Initializing required variables");

            // create data connection
            using SqlConnection con = new(AppConfigs.ConnectionBuilder.ConnectionString);

            // optional: create an array to track successful insertions
            int[] successes = new int[people.Count];
            int counter = 0;

            Console.WriteLine("Initialization complete. Preparing to begin INSERT loop");

            // open the connection and begin cycling the people list
            con.Open();
            foreach (Person person in people)
            {
                try
                {
                    // create the sql command calling stored procedure
                    SqlCommand command = new()
                    {
                        CommandText = "INSERT_Person",
                        CommandType = CommandType.StoredProcedure,
                        Connection = con
                    };

                    // add parameters 
                    command.Parameters.Add("@FirstName", SqlDbType.NVarChar, 100).Value = person.FirstName;
                    command.Parameters.Add("@LastName", SqlDbType.NVarChar, 100).Value = person.LastName;
                    command.Parameters.Add("@Phone", SqlDbType.NVarChar, 11).Value = person.Phone;
                    command.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = person.Email;
                    command.Parameters.Add("@Address", SqlDbType.NVarChar, 255).Value = person.Address;
                    command.Parameters.Add("@City", SqlDbType.NVarChar, 100).Value = person.City;
                    command.Parameters.Add("@State", SqlDbType.NChar, 2).Value = person.State.Remove(2).ToUpper();
                    command.Parameters.Add("@Zip", SqlDbType.NChar, 5).Value = person.Zip.Remove(5);

                    // execute the non-query and record the response to the array (1 = good, 0 = bad)
                    int dbResponse = command.ExecuteNonQuery();
                    if (dbResponse == 1 || dbResponse == -1)
                        successes[counter] = 1;
                    else
                        successes[counter] = 0;

                    counter++;
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("A SQL exception occured. Message follows: ");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An unknown exception occured. Details follow: ");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine();
                }
            }

            // close the connection and tell user
            con.Close();
            Console.WriteLine("INSERT completed. Preparing to check results");

            // check number of successes and failures, then tell the user
            int totalSuccesses = 0;
            int totalFailures = 0;
            foreach (int x in successes)
            {
                if (x == 1)
                    totalSuccesses++;
                else
                    totalFailures++;
            }
            Console.WriteLine($"Sucesses: {totalSuccesses} | Failures: {totalFailures}");
        }
    }
}
