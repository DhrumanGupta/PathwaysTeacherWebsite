using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Linq;

namespace EventJsonEncoder
{
    class Program
    {
        private static int _index = 0;

        private static List<Event> _events;

        private delegate void Command();
        private static Dictionary<string, Command> _commands = new Dictionary<string, Command>()
        {
            { "view", View },
            { "show", View },
            { "add", Add },
            { "remove", Delete },
            { "delete", Delete },
            { "save", Push },
            { "push", Push },
            { "get", Get },
            { "close", End },
            { "end", End },
            { "exit", End }
        };

        private static readonly string _jsonPath = @"../../../json.json";
        private static readonly string _jsonPushPath = @"../../../../DigitalDesignProject_Unit4/wwwroot/data/events.json";

        private static void Main(string[] args)
        {
            string data = File.ReadAllText(_jsonPath);
            _events = JsonConvert.DeserializeObject<List<Event>>(data);

            GetCommand();
        }

        private static void GetCommand()
        {
            string command = GetCommandFromUser();

            while (!_commands.ContainsKey(command))
            {
                Console.WriteLine("Invalid Action!");
                command = GetCommandFromUser();
            }

            _commands[command]();
        }

        private static string GetCommandFromUser()
        {
            Console.WriteLine("Action: ");
            string command = Console.ReadLine().ToLower().Trim();
            return command;
        }

        private static void View()
        {
            string events = JsonConvert.SerializeObject(_events, Formatting.Indented);
            Console.WriteLine(events);

            GetCommand();
        }

        private static string GetStringConfirmed(string toWrite)
        {
            Console.WriteLine(toWrite);

            string prop = Console.ReadLine();

            return prop;
        }

        private static void SaveData()
        {
            File.WriteAllText(_jsonPath, JsonConvert.SerializeObject(_events, Formatting.Indented));
        }

        private static int GetValidIndex()
        {
            Console.WriteLine("Which index do you want to access?");

            int result;
            while (!Int32.TryParse(Console.ReadLine(), out result) || result > _events.Count - 1)
            {
                Console.WriteLine("Please give a valid integer, in range");
            }

            return result;
        }

        #region Add Delete

        private static void Add()
        {
            Console.WriteLine("Creating new event..");
            Event e = new Event()
            {
                Name = GetStringConfirmed("Name: "),
                Description = GetStringConfirmed("Description: "),
                Image = GetStringConfirmed("Image: "),
                StartTime = GetStringConfirmed("Start Time: "),
                EndTime = GetStringConfirmed("End Time: ")
            };

            var usedIds = _events.Select(x => x.Id);
            var randomId = Randoms.RandomString(16);

            while (usedIds.Contains(randomId))
            {
                randomId = Randoms.RandomString(16);
            }

            e.Id = randomId;
            _events.Add(e);
            SaveData();

            GetCommand();
        }

        private static void Delete()
        {
            var index = GetValidIndex();
            _events.Remove(_events[index]);
            SaveData();
            GetCommand();
        }

        #endregion

        #region Get Push

        private static void Push()
        {
            var dataToSave = File.ReadAllText(_jsonPath);
            File.WriteAllText(_jsonPushPath, dataToSave);

            Console.WriteLine($"Pushed data to: {Path.GetFullPath(_jsonPushPath)}");

            GetCommand();
        }

        private static void Get()
        {
            var dataToSave = File.ReadAllText(_jsonPushPath);
            File.WriteAllText(_jsonPath, dataToSave);

            Console.WriteLine($"Got data from: {Path.GetFullPath(_jsonPushPath)}");

            GetCommand();
        }

        #endregion

        private static void End()
        {
            Environment.Exit(0);
        }
    }
}
