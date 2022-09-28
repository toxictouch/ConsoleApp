using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using ConsoleApp.App.Config;
using ConsoleApp.App.Models;

namespace ConsoleApp.App
{
    internal class FileConsumer
    {
        private static readonly string FileName = "raw-people-data.json";

        public static void InsertPeopleFromJson()
        {
            string jsonString = File.ReadAllText(Path.Combine(AppConfigs.JsonFilesPath, FileName));

            List<Person> people = JsonConvert.DeserializeObject<List<Person>>(jsonString);

            SqlCommands.InsertPeopleList(people);
        }

        public static void WritePeopleToScreen()
        {
            string jsonString = File.ReadAllText(Path.Combine(AppConfigs.JsonFilesPath, FileName));

            List<Person> people = JsonConvert.DeserializeObject<List<Person>>(jsonString);

            foreach (Person person in people)
            {
                Console.WriteLine("F-Name: " + person.FirstName + " L-Name: " + person.LastName);
                Console.WriteLine();
            }
        }
    }
}
