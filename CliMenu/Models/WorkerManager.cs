using System.Globalization;
using System.Text.Json;
using System.Xml.Serialization;
using CliMenu.Enum;
using Microsoft.Extensions.Primitives;

namespace CliMenu.Models
{

    /// <summary>
    /// Manages operations related to workers, such as adding, deleting, and retrieving workers.
    /// </summary>
    internal class WorkerManager
    {
        #region Constants

        readonly static private string workersCsvPath = PathConfiguration.Path["WorkersCsv"];
        readonly static private string workersJsonPath = PathConfiguration.Path["WorkersJson"];
        readonly static private string workersXmlPath = PathConfiguration.Path["WorkersXml"];
        readonly static private string workdayCsvPath = PathConfiguration.Path["WorkDayCsv"];
        readonly static  private string workdayJsonPath = PathConfiguration.Path["WorkDayJson"];
        readonly static private string workdayXmlPath = PathConfiguration.Path["WorkDayXml"];

        #endregion

        #region Public Methods

        private static bool? CheckIfSeparatorIsPresentOnAllLines(string[] lines, char separator){
            bool? isPresentOnAllLines = null;
            foreach(string line in lines){
                if(line.Contains(separator) && isPresentOnAllLines != false){
                    isPresentOnAllLines = true;
                } else {
                    isPresentOnAllLines = false;
                }
            }
            return isPresentOnAllLines;
        }
        

        private static HashSet<char> GetAllPossibleSeparators(string[] lines){
            HashSet<char> set = [];
           
            foreach(string line in lines){
                foreach(char letter in line){
                    if(!char.IsLetterOrDigit(letter)){
                        set.Add(letter);
                    }
                }
            }
            return set;
        }

        internal static void DetectSeparator(){
            string[] dataLines = File.ReadAllLines(workersCsvPath);
            
            HashSet<char> statndardSeparators =  [';', ',',' ','\t'];
            List<char> possibleSeparators = GetAllPossibleSeparators(dataLines).Union(statndardSeparators).ToList();
        
            Console.Write("Possibili separatori individuati: ");
            foreach(char separator in possibleSeparators){
                var sep = $"{separator}";
                if(sep == " "){
                    sep = "spazio";
                } else if(sep == "\t"){
                    sep = "tab";
                }

                Console.Write($"[{sep}] ");
            }
            Console.WriteLine();

            bool?[] isSeparatorPresentOnAllLines = new bool?[possibleSeparators.Count]; 
            int[] possibilities = new int[possibleSeparators.Count];

            Dictionary<int, List<int>> separatorCount = [];

            for(int i = 0; i < possibleSeparators.Count; i++){ 
                isSeparatorPresentOnAllLines[i] = CheckIfSeparatorIsPresentOnAllLines(dataLines, possibleSeparators[i]);
            }

            for(int i = 0; i < dataLines.Length; i++){               
                foreach(var separator in possibleSeparators){
                    if(!separatorCount.ContainsKey(i))
                    {
                        separatorCount.Add(i, [dataLines[i].Count(c => c == separator)]);
                    } else {
                        separatorCount[i].Add(dataLines[i].Count(c => c == separator));
                    }
                }
            } 

            for(int i = 0; i < separatorCount.Keys.Count; i++){               
                for(int j = 0; j < separatorCount[i].Count; j++){
                    possibilities[j] += separatorCount[i][j];
                }  
            } 
            
            int max = possibilities.Max();
            Console.WriteLine("il carattere separatore è: " + possibleSeparators[Array.IndexOf(possibilities, max)]);
        }

        /// <summary>
        /// Retrieves all workers from the CSV file, separating valid workers and error workers.
        /// </summary>
        /// <returns>A tuple containing lists of valid workers and error workers.</returns>
        internal static Tuple<List<WorkerModel>, List<ErrorWorker>> GetAllWorkers()
        {
            try
            {
                string[] workersData = File.ReadAllLines(workersCsvPath);

                List<WorkerModel> workers = [];
                List<ErrorWorker> errorWorkers = [];

                foreach (string workerData in workersData)
                {
                    string[] workerDataValues = workerData.Split(';');
                    
                    if (CheckWorkerProperties(workerDataValues))
                    {
                        Role role = (Role)System.Enum.Parse(typeof(Role), workerDataValues[2].ToUpper());
                       
                        var depToParse = string.Join("_", workerDataValues[3].Split(" ")).ToUpper();
                        Department department = (Department)System.Enum.Parse(typeof(Department), depToParse);
       
                        WorkerModel worker = new(){
                            Matricola = workerDataValues[0], // Matricola
                            FullName = workerDataValues[1], // FullName
                            Role = role, // Role
                            Department = department, // Department
                            Age = int.TryParse(workerDataValues[4], out int age) ? age : null,
                            Address = workerDataValues[5], // Address
                            City = workerDataValues[6], // City
                            Province = workerDataValues[7], // Province
                            Cap = workerDataValues[8], // Cap
                            Phone = workerDataValues[9]  // Phone
                        };

                        try
                        {
                            SetWorkerWorkDay(worker);
                        }
                        catch (DirectoryNotFoundException ex)
                        {
                            HandleException(ex);
                            Directory.CreateDirectory(PathConfiguration.Path["Csv"]);
                            File.Create(workdayCsvPath).Dispose();
                            SetWorkerWorkDay(worker);
                        }
                        catch (FileNotFoundException ex)
                        {
                            HandleException(ex);
                            File.Create(workdayCsvPath).Dispose();
                            SetWorkerWorkDay(worker);
                        }

                        workers.Add(worker);
                    }
                    else
                    {
                        ErrorWorker worker = new(
                            workerDataValues[0], // Matricola
                            workerDataValues[1], // FullName
                            workerDataValues[2], // Role
                            workerDataValues[3], // Department
                            int.TryParse(workerDataValues[4], out int age) ? age : null,
                            workerDataValues[5], // Address
                            workerDataValues[6], // City
                            workerDataValues[7], // Province
                            workerDataValues[8], // Cap
                            workerDataValues[9]  // Phone
                        );
                        errorWorkers.Add(worker);
                    }
                }
                return Tuple.Create(workers, errorWorkers);
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return Tuple.Create(new List<WorkerModel>(), new List<ErrorWorker>());
            }
        }

        /// <summary>
        /// Displays all workers and any error workers found in the data.
        /// </summary>
        internal static void ShowWorkers()
        {
            try
            {
                var allWorkers = GetAllWorkers();
                var validWorkers = allWorkers.Item1;
                var errorWorkers = allWorkers.Item2;

                foreach (var worker in validWorkers)
                {
                    SetWorkerWorkDay(worker);
                    Console.WriteLine(worker.ToConsole());
                    Console.WriteLine();
                }

                if (errorWorkers.Count > 0)
                {
                    Console.WriteLine("===================== ERROR WORKERS =====================");
                    Console.WriteLine("Malformed data found. Details are provided below:");
                    Console.WriteLine("=========================================================");

                    foreach (ErrorWorker worker in errorWorkers)
                    {
                        Console.WriteLine(worker.ToConsole());
                        Console.WriteLine("---------------------------------------------------------");
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        /// <summary>
        /// Adds a new worker to the CSV file.
        /// </summary>
        internal static void AddWorker()
        {
            try
            {
                var worker = CollectWorkerData();
                if (worker != null)
                {
                    WorkerInsert(worker);
                    NotifyUser("Worker added successfully. Press any key to return to the menu...");
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        /// <summary>
        /// Deletes a worker and/or their workdays based on user selection.
        /// </summary>
        /// <param name="worker">The worker to delete.</param>
        internal static void Delete(WorkerModel worker)
        {
            try
            {
                Console.WriteLine("Select an option to delete:");
                Console.WriteLine("1 - The worker.");
                Console.WriteLine("2 - A specific workday.");
                Console.WriteLine("3 - All workdays of the worker.");

                int parsedChoice = GetParsedChoice();

                WorkDayManager manager = new(worker);
                switch (parsedChoice)
                {
                    case 1:
                        DeleteWorkerFromCsv(worker);
                        manager.DeleteWorkerWorkedDays();
                        NotifyUser("Worker deleted. Press any key to continue...");
                        break;
                    case 2:
                        if (manager.DeleteWorkerWorkedDay())
                        {
                            NotifyUser("Workday deleted. Press any key to continue...");
                        }
                        else
                        {
                            NotifyUser("No workdays found for the selected worker. Press any key to continue...");
                        }
                        break;
                    case 3:
                        if (manager.DeleteWorkerWorkedDays())
                        {
                            NotifyUser("All workdays deleted. Press any key to continue...");
                        }
                        else
                        {
                            NotifyUser("No workdays found for the selected worker. Press any key to continue...");
                        }
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Operation aborted.");
                        break;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        /// <summary>
        /// Searches for workers based on user input criteria.
        /// </summary>
        internal static void SearchForWorker()
        {
            try
            {
                List<WorkerModel> workers = GetAllWorkers().Item1;
                int choice = GetSearchChoice();

                string searchValue = GetInput($"Enter the value to search for option {choice}: ");
                PerformSearch(workers, choice, searchValue);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        /// <summary>
        /// Searches for a worker by their matricola (ID).
        /// </summary>
        /// <param name="matricola">The matricola to search for.</param>
        /// <returns>The worker if found; otherwise, null.</returns>
        internal static WorkerModel? SearchByMatricola(string? matricola = null)
        {
            try
            {
                List<WorkerModel> workers = GetAllWorkers().Item1;
                if (matricola == null)
                {
                    matricola = GetValidInput("Enter a 4-character matricola: ",
                        input => !string.IsNullOrEmpty(input) && input.Length == 4);
                }
                return workers.Find(worker => worker.Matricola == matricola);
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return null;
            }
        }

        /// <summary>
        /// Exports a list of data to a JSON file.
        /// </summary>
        /// <typeparam name="T">The type of data to export.</typeparam>
        /// <param name="list">The list of data to export.</param>
        /// <param name="fileName">Optional file name to export to.</param>
        /// <returns>True if export is successful; otherwise, false.</returns>
        internal static bool ExportJson<T>(List<T> list, string? fileName = null)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = fileName == null };
                string file = typeof(T) == typeof(WorkerModel) ? workersJsonPath : workdayJsonPath;

                string json = JsonSerializer.Serialize(list, options);
                File.WriteAllText(options.WriteIndented ? $"{PathConfiguration.Path["Csv"] + fileName}" : file, json);
                return true;
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return false;
            }
        }

        internal static bool ExportJson<T>(List<T> list)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(list, options);
                File.WriteAllText("path", json);
                return true;
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return false;
            }
        }

        /// <summary>
        /// Imports data from a JSON file.
        /// </summary>
        /// <typeparam name="T">The type of data to import.</typeparam>
        /// <returns>A list of imported data if successful; otherwise, null.</returns>
        internal static List<T>? ImportJson<T>()
        {
            try
            {
                string file = typeof(T) == typeof(WorkerModel) ? workersJsonPath : workdayJsonPath;
                string data = File.ReadAllText(file);
                return JsonSerializer.Deserialize<List<T>>(data);
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return null;
            }
        }

        /// <summary>
        /// Exports a list of data to an XML file.
        /// </summary>
        /// <typeparam name="T">The type of data to export.</typeparam>
        /// <param name="list">The list of data to export.</param>
        /// <param name="fileName">Optional file name to export to.</param>
        /// <returns>True if export is successful; otherwise, false.</returns>
        internal static bool ExportXml<T>(List<T> list, string? fileName = null)
        {
            try
            {

                bool custom = fileName == null;
                XmlSerializer xml = new (typeof(List<T>));
                string filePath = typeof(T) == typeof(WorkerModel) ? workersXmlPath : workdayXmlPath;
                using FileStream fs = new (custom ? $"{PathConfiguration.Path["Xml"] + fileName}" :  filePath, FileMode.Create, FileAccess.Write);
                xml.Serialize(fs, list);
                return true;
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return false;
            }
        }

        /// <summary>
        /// Imports data from an XML file.
        /// </summary>
        /// <typeparam name="T">The type of data to import.</typeparam>
        /// <returns>A list of imported data if successful; otherwise, null.</returns>
        internal static List<T>? ImportXml<T>()
        {
            try
            {
                string file = typeof(T) == typeof(WorkerModel) ? workersXmlPath : workdayXmlPath;
                XmlSerializer serializer = new(typeof(List<T>));

                using StreamReader reader = new(file);
                return (List<T>?)serializer.Deserialize(reader);
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return null;
            }
        }

        /// <summary>
        /// Retrieves a list of workers within a specified age range.
        /// </summary>
        /// <param name="min">The minimum age.</param>
        /// <param name="max">The maximum age.</param>
        /// <returns>A list of workers within the specified age range.</returns>
        internal static List<WorkerModel> GetWorkerByAgeRange(int min, int max)
        {
            try
            {
                return GetAllWorkers().Item1.FindAll(worker => worker.Age >= min && worker.Age <= max);
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return [];
            }
        }

        /// <summary>
        /// Displays workers who have worked overtime.
        /// </summary>
        internal static void GetWorkersWithOvertime()
        {
            try
            {
                List<WorkerModel> workers = GetAllWorkers().Item1;
                Dictionary<string, List<WorkDay>> workersWorkDays = [];

                var workingWorkers = workers.Where(worker => worker.WorkedDays.Count > 0).ToList();
                workingWorkers.ForEach(worker => workersWorkDays.Add(worker.WorkedDays[0].Matricola!, worker.WorkedDays));

                foreach (string matricola in workersWorkDays.Keys)
                {
                    decimal? overTime = workersWorkDays[matricola]
                        .Where(elem => (elem.TotalHours > 8 || elem.JobType == "Pre Festivo") && elem.JobType != "Ferie")
                        .Sum(elem => elem.JobType == "Pre Festivo" ? elem.TotalHours : elem.TotalHours - 8);

                    decimal? totalHours = workersWorkDays[matricola]
                        .Where(elem => elem.JobType != "Ferie")
                        .Sum(elem => elem.TotalHours);

                    try
                    {
                        WorkerModel? worker = SearchByMatricola(matricola);
                        string workerInfo = string.Join('\n', worker!.ToConsole(false));
                        Console.WriteLine($@"
                        {workerInfo}

                        Overtime hours: {overTime}
                        Regular hours worked: {totalHours - overTime}
                        ");
                        Console.WriteLine();
                    }
                    catch (Exception ex)
                    {
                        HandleException(ex);
                        Console.WriteLine("Malformed data");
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        /// <summary>
        /// Retrieves workers who have taken holidays.
        /// </summary>
        /// <returns>A list of workers who have taken holidays.</returns>
        internal static List<WorkerModel> GetWorkerWithHoliday()
        {
            try
            {
                List<WorkerModel> workers = GetAllWorkers().Item1;
                Dictionary<string, List<WorkDay>> workersWorkDays = [];
                List<WorkerModel> result = [];

                var workingWorkers = workers.Where(worker => worker.WorkedDays.Count > 0).ToList();
                workingWorkers.ForEach(worker => workersWorkDays.Add(worker.WorkedDays[0].Matricola!, worker.WorkedDays));

                foreach (string matricola in workersWorkDays.Keys)
                {
                    var holidayDays = workersWorkDays[matricola].Where(day => day.JobType == "Ferie").ToList();
                    if (holidayDays.Count > 0)
                    {
                        WorkerModel? worker = SearchByMatricola(matricola);
                        if (worker != null)
                            result.Add(worker);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return [];
            }
        }

        public static void SaveWorkerWithAddedInfo(WorkerModel worker){   
            worker.ToConsole();
            if(worker.Holidays.Count > 0 || worker.Machines.Count > 0 || worker.WeigthChecks.Count > 0){  
                File.WriteAllText($"{PathConfiguration.Path["Json"]}WorkersCompleteInfo.json", JsonSerializer.Serialize(worker, new JsonSerializerOptions { WriteIndented = true }));
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Associates workdays data from the workdays.csv file with the provided worker.
        /// </summary>
        /// <param name="worker">The worker to set workdays for.</param>
        internal static void SetWorkerWorkDay(WorkerModel worker)
        {
            try
            {
            
                // Ensure the file exists
                if (!File.Exists(workdayCsvPath))
                {
                    File.Create(workdayCsvPath).Dispose();
                }

                using var fileStream = new FileStream(workdayCsvPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using StreamReader reader = new(fileStream);
                string[] allWorkedDays = reader.ReadToEnd().Split(Environment.NewLine);
                List<WorkDay> workerWorkedDays = [];

                foreach (string workedDay in allWorkedDays)
                {
                    if (string.IsNullOrWhiteSpace(workedDay)) continue;

                    string[] workedDayDataValues = workedDay.Split(';');

                    if (worker.Matricola == workedDayDataValues[4])
                    {
                        WorkDay workDay = new(){
                            ID = Guid.Parse(workedDayDataValues[0]),
                            ActivityDate = DateTime.ParseExact(workedDayDataValues[1], "d/M/yyyy", CultureInfo.InvariantCulture),
                            JobType = workedDayDataValues[2],
                            TotalHours = decimal.Parse(workedDayDataValues[3]),
                            Matricola = workedDayDataValues[4]
                        };
                        workerWorkedDays.Add(workDay);
                    }
                }

                worker.WorkedDays.Clear(); 
                worker.WorkedDays.AddRange(workerWorkedDays);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        /// <summary>
        /// Collects worker data from user input.
        /// </summary>
        /// <returns>A WorkerModel object if data is valid; otherwise, null.</returns>
        private static WorkerModel? CollectWorkerData()
        {
            try
            {
                string? matricola = GetValidInput("Enter a 4-character matricola: ",
                    input => !string.IsNullOrEmpty(input) && input.Length == 4);

                string? fullName = GetValidInput("Enter the worker's full name: ",
                    input => !string.IsNullOrEmpty(input));
                
                Role role = GetEnum<Role>(); 
                Department department = GetEnum<Department>();

                int? age = GetValidAge();
                string? address = GetInput("Enter the worker's address: ");
                string? city = GetInput("Enter the worker's city: ");
                string? province = GetInput("Enter the worker's province: ");
                string? cap = GetInput("Enter the worker's CAP: ");
                string? phone = GetInput("Enter the worker's phone number: ");

                return new WorkerModel
                {
                    Matricola = matricola,
                    FullName = fullName,
                    Role = role,
                    Department = department,
                    Age = age,
                    Address = address,
                    City = city,
                    Province = province,
                    Cap = cap,
                    Phone = phone
                };
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return null;
            }
        }

        /// <summary>
        /// Inserts a worker into the CSV file.
        /// </summary>
        /// <param name="worker">The worker to insert.</param>
        private static void WorkerInsert(WorkerModel worker)
        {
            try
            {
                if (!Directory.Exists(PathConfiguration.Path["Csv"]))
                {
                    Directory.CreateDirectory(PathConfiguration.Path["Csv"]);
                }
                File.AppendAllText(workersCsvPath, worker.ToCSV() + "\n");
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        /// <summary>
        /// Deletes a worker from the CSV file.
        /// </summary>
        /// <param name="worker">The worker to delete.</param>
        private static void DeleteWorkerFromCsv(WorkerModel worker)
        {
            try
            {
                var lines = File.ReadAllLines(workersCsvPath).ToList();
                var filteredLines = lines.Where(line => !(line.Split(';')[0] == worker.Matricola)).ToList();
                File.WriteAllLines(workersCsvPath, filteredLines);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        /// <summary>
        /// Checks if all worker properties are valid.
        /// </summary>
        /// <param name="workerData">An array of worker data values.</param>
        /// <returns>True if properties are valid; otherwise, false.</returns>
        private static bool CheckWorkerProperties(string[] workerData)
        {
            try
            {
                return CheckWorkerProperty(workerData[0], input => !string.IsNullOrEmpty(input) && input.Length == 4) &&
                       CheckWorkerProperty(workerData[1], input => !string.IsNullOrEmpty(input)) &&
                       CheckWorkerProperty(workerData[2], input => !string.IsNullOrEmpty(input) && System.Enum.TryParse(typeof(Role), input.ToUpper(), out _) == true) &&
                       CheckWorkerProperty(workerData[3], input => !string.IsNullOrEmpty(input) && System.Enum.TryParse(typeof(Department), string.Join("_", input.Split(" ")).ToUpper(), out _) == true) &&
                       CheckWorkerProperty(workerData[4], input => string.IsNullOrEmpty(input) || (int.TryParse(input, out int age) && age >= 18 && age <= 70));
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return false;
            }
        }

        /// <summary>
        /// Validates a single worker property.
        /// </summary>
        /// <param name="input">The property value to validate.</param>
        /// <param name="validator">The validation function.</param>
        /// <returns>True if valid; otherwise, false.</returns>
        private static bool CheckWorkerProperty(string input, Func<string?, bool> validator)
        {
            try
            {
                return validator(input);
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return false;
            }
        }

        /// <summary>
        /// Gets valid input from the user based on a validator function.
        /// </summary>
        /// <param name="prompt">The prompt to display to the user.</param>
        /// <param name="validator">The validation function.</param>
        /// <returns>The valid input string.</returns>
        private static string GetValidInput(string prompt, Func<string?, bool> validator)
        {
            try
            {
                string? input;
                do
                {
                    Console.WriteLine(prompt);
                    input = Console.ReadLine();
                    if (!validator(input))
                    {
                        NotifyUser("Invalid input. Please try again.", ConsoleColor.Red);
                    }
                } while (!validator(input));
                return input!;
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets input from the user without validation.
        /// </summary>
        /// <param name="prompt">The prompt to display to the user.</param>
        /// <returns>The input string.</returns>
        private static string GetInput(string prompt)
        {
            try
            {
                Console.WriteLine(prompt);
                return Console.ReadLine()!;
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return string.Empty;
            }
        }


        private static T? GetEnum<T>()
        {
            while (true)
            {
                if(System.Enum.TryParse(typeof(T), Console.ReadLine(), true, out object? department))
                {
                    return (T) department;
                }
                else
                {
                    
                    if(typeof(T) == typeof(Department)){
                        Console.WriteLine("Invalid department. Please try again.");
                        Console.WriteLine("Available departments:");
                    
                        foreach (Department dept in System.Enum.GetValues(typeof(T)))
                        {
                            Console.WriteLine(dept);
                        }
                    } else if(typeof(T) == typeof(Role)){
                        Console.WriteLine("Invalid role. Please try again.");
                        Console.WriteLine("Available roles:");
                    
                        foreach (Role role in System.Enum.GetValues(typeof(T)))
                        {
                            Console.WriteLine(role);
                        }
                    }
                }
            }
        }

        private static Department? GetRole()
        {
            while (true)
            {
                if(System.Enum.TryParse(typeof(Department), Console.ReadLine(), true, out object? department))
                {
                    return (Department) department;
                }
                else
                {
                    Console.WriteLine("Invalid department. Please try again.");
                    Console.WriteLine("Available departments:");
                   
                    foreach (Department dept in System.Enum.GetValues(typeof(Department)))
                    {
                        Console.WriteLine(dept);
                    }
                }
            }
        }


        /// <summary>
        /// Gets a valid age from the user.
        /// </summary>
        /// <returns>The valid age if provided; otherwise, null.</returns>
        private static int? GetValidAge()
        {
            try
            {
                int? age = null;
                while (true)
                {
                    Console.WriteLine("Enter the worker's age: ");
                    string? input = Console.ReadLine();
                    if (int.TryParse(input, out int parsedAge) && parsedAge >= 18 && parsedAge <= 70)
                    {
                        age = parsedAge;
                        break;
                    }
                    if (string.IsNullOrEmpty(input))
                    {
                        break;
                    }
                    NotifyUser("Invalid age. Please try again.", ConsoleColor.Red);
                }
                return age;
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return null;
            }
        }

        /// <summary>
        /// Gets the user's choice for search criteria.
        /// </summary>
        /// <returns>The user's choice as an integer.</returns>
        private static int GetSearchChoice()
        {
            try
            {
                while (true)
                {
                    Console.WriteLine("Search workers by:");
                    Console.WriteLine("1 - Matricola");
                    Console.WriteLine("2 - Full Name");
                    Console.WriteLine("3 - Role");
                    Console.WriteLine("4 - Department");
                    Console.WriteLine("5 - Age");
                    Console.WriteLine("6 - City");
                    Console.WriteLine("7 - Address");
                    Console.WriteLine("8 - CAP");
                    Console.WriteLine("9 - Phone Number");

                    if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= 9)
                    {
                        return choice;
                    }
                    Console.WriteLine("Invalid choice. Please try again.");
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return 0;
            }
        }

        /// <summary>
        /// Performs the search for workers based on the given criteria.
        /// </summary>
        /// <param name="workers">The list of workers to search.</param>
        /// <param name="choice">The search criterion choice.</param>
        /// <param name="searchValue">The value to search for.</param>
        private static void PerformSearch(List<WorkerModel> workers, int choice, string searchValue)
        {
            try
            {
                int found = 0;

                foreach (var worker in workers)
                {
                    if (MatchesSearchCriteria(worker, choice, searchValue))
                    {
                        Console.WriteLine(new string('-', 100));
                        Console.WriteLine(worker.ToConsole());
                        found++;
                    }
                }

                if (found == 0)
                {
                    Console.WriteLine("No workers found matching the criteria.");
                }

                NotifyUser("\nPress any key to return to the menu...");
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        /// <summary>
        /// Checks if a worker matches the search criteria.
        /// </summary>
        /// <param name="worker">The worker to check.</param>
        /// <param name="choice">The search criterion choice.</param>
        /// <param name="searchValue">The value to search for.</param>
        /// <returns>True if the worker matches; otherwise, false.</returns>
        private static bool MatchesSearchCriteria(WorkerModel worker, int choice, string searchValue)
        {
            try
            {
                return choice switch
                {
                    1 => worker.Matricola == searchValue,
                    2 => worker.FullName == searchValue,
                    3 => worker.Role == (Role) System.Enum.Parse(typeof(Role), searchValue),
                    4 => worker.Department == (Department) System.Enum.Parse(typeof(Department), searchValue),
                    5 => worker.Age?.ToString() == searchValue,
                    6 => worker.City == searchValue,
                    7 => worker.Address == searchValue,
                    8 => worker.Cap == searchValue,
                    9 => worker.Phone == searchValue,
                    _ => false
                };
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return false;
            }
        }

        /// <summary>
        /// Gets a valid choice from the user for deletion operations.
        /// </summary>
        /// <returns>The parsed choice as an integer.</returns>
        private static int GetParsedChoice()
        {
            int parsedChoice = 0;
            GetValidInput("",
                input => !string.IsNullOrEmpty(input) &&
                         input.Length == 1 &&
                         int.TryParse(input, out parsedChoice) &&
                         parsedChoice >= 1 &&
                         parsedChoice <= 3);
            return parsedChoice;
        }

        /// <summary>
        /// Displays colored message to the user.
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <param name="backgroundColor">The background color of the message.</param>
        /// <param name="foregroundColor">The foreground color of the message.</param>
        private static void NotifyUser(string message, ConsoleColor backgroundColor = ConsoleColor.White, ConsoleColor foregroundColor = ConsoleColor.Black)
        {
            Console.BackgroundColor = backgroundColor;
            Console.ForegroundColor = foregroundColor;
            Console.WriteLine(message);
            Console.ResetColor();
            Console.ReadKey();
        }

        /// <summary>
        /// Handles exceptions by logging them using MenuException.
        /// </summary>
        /// <param name="ex">The exception to handle.</param>
        private static void HandleException(Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
            var menuException = new MenuException(ex.Message, ex.StackTrace);
            menuException.AddException(menuException);
        }

        #endregion
    }
}
