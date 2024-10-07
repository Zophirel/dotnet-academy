using System.Globalization;
namespace CliMenu.Models
{
    internal class WorkDayManager
    {
        readonly static string workDayCsv = PathConfiguration.Path["WorkDayCsv"];
        readonly static string csv = PathConfiguration.Path["Csv"];

        WorkerModel Worker { get; set; }

        public WorkDayManager(WorkerModel worker)
        {
            Worker = worker;
        }

        internal bool DeleteWorkerWorkedDays()
        {
            try
            {
                if (Worker.WorkedDays.Count > 0)
                {
                    // 1 Transform the CSV into a list of strings
                    var lines = File.ReadAllLines(workDayCsv).ToList();
                
                    // 2 Split the strings and check the second field (Matricola)
                    // Ignore lines that contain the selected matricola
                    var filteredLines = lines.Where(line => !(line.Split(';')[1] == Worker.Matricola)).ToList();

                    // 3 Rewrite the file replacing it with the new list
                    File.WriteAllLines(workDayCsv, filteredLines);
                
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                var menuException = new MenuException(ex.Message, ex.StackTrace);
                menuException.AddException(menuException);
                return false;
            }
        }

        internal bool DeleteWorkerWorkedDay()
        {
            try
            {
                if (Worker.WorkedDays.Count > 0)
                {
                    Console.WriteLine(Worker.ToConsole());
                    string dayID = GetValidInput("Inserisci l'ID della giornata lavorativa da cancellare", input => Guid.TryParse(input, out _));

                    // 1 Transform the CSV into a list of strings
                    var lines = File.ReadAllLines(workDayCsv).ToList();
                
                    // 2 Split the strings and check the first field (ID)
                    // Ignore lines that contain the selected day's ID
                    var filteredLines = lines.Where(line => !(line.Split(';')[0] == dayID)).ToList();

                    // 3 Rewrite the file replacing it with the new list
                    File.WriteAllLines(workDayCsv, filteredLines);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                var menuException = new MenuException(ex.Message, ex.StackTrace);
                menuException.AddException(menuException);
                return false;
            }
        }

        internal bool? AddWorkedDay(DateTime day)
        {
            try
            {
                var workedDay = CollectWorkDayData(day);

                if (workedDay != null)
                {
                    Worker.WorkedDays.Add(workedDay);
                    WorkDayInsert(workedDay);
                    if (day.AddDays(1).DayOfWeek == DayOfWeek.Saturday)
                    {
                        return true;
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write("Giornata lavorativa aggiunta, proseguire con la prossima (s/n)");
                        Console.ResetColor();
                        string key = Console.ReadKey().KeyChar.ToString();
                        if (key == "s" || key == "S")
                        {
                            return true;
                        }
                        return false;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                var menuException = new MenuException(ex.Message, ex.StackTrace);
                menuException.AddException(menuException);
                return null;
            }
        }

        private WorkDay? CollectWorkDayData(DateTime activityDate)
        {
            try
            {
                Console.WriteLine($"\nVuoi aggiungere i dati per il giorno {activityDate:dd/MM/yyyy} (s/n)");
                string answer = Console.ReadKey().KeyChar.ToString();
                
                if (answer == "s" || answer == "S")
                {
                    string jobType;

                    Guid id = Guid.NewGuid();
                    string matricola = Worker.Matricola;

                    jobType = GetValidInput("Tipo di lavoro svolto: ", input => !string.IsNullOrEmpty(input));

                    decimal totalHours;
                    while (!decimal.TryParse(GetValidInput("Totale ore svolte: ", input => !string.IsNullOrEmpty(input)), out totalHours))
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.WriteLine("Input non valido, inserisci un numero valido per le ore svolte.");
                        Console.ResetColor();
                    }
                
                    return new WorkDay(){
                        ID = id, 
                        ActivityDate = activityDate, 
                        JobType = jobType, 
                        TotalHours = totalHours, 
                        Matricola = matricola
                    };
                }
                return null;
            }
            catch (Exception ex)
            {
                var menuException = new MenuException(ex.Message, ex.StackTrace);
                menuException.AddException(menuException);
                return null;
            }
        }

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
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.WriteLine("Input non valido, riprova.");
                        Console.ResetColor();
                    }
                } while (!validator(input));
                return input!;
            }
            catch (Exception ex)
            {
                var menuException = new MenuException(ex.Message, ex.StackTrace);
                menuException.AddException(menuException);
                return string.Empty;
            }
        }

        private static void WorkDayInsert(WorkDay workDay)
        {
            try
            {
                if (!Directory.Exists(csv))
                {
                    Directory.CreateDirectory(csv);
                }
                File.AppendAllText(workDayCsv, workDay.ToCSV() + "\n");
            }
            catch (Exception ex)
            {
                var menuException = new MenuException(ex.Message, ex.StackTrace);
                menuException.AddException(menuException);
            }
        }

        internal static Tuple<List<WorkDay>, List<ErrorWorkDay>> GetAllWorkDays()
        {
            try
            {
                List<string> lines = new(File.ReadAllLines(workDayCsv));
                
                List<WorkDay> workDays = [];
                List<ErrorWorkDay> errorWorkDays = [];
                foreach (string line in lines)
                {
                    string[] splittedLine = line.Split(';');
                    bool isDayIDValid = Guid.TryParse(splittedLine[0], out Guid id);
                    bool isDayDateValid = DateTime.TryParseExact(splittedLine[1], "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date);
                    bool isJobValid = !string.IsNullOrEmpty(splittedLine[2]);
                    bool isTotalHoursValid = decimal.TryParse(splittedLine[3], out decimal totalHours);
                    bool isWorkerMatricolaValid = !string.IsNullOrEmpty(splittedLine[4]) && splittedLine[4].Length == 4; 

                    if (isDayIDValid && isDayDateValid && isWorkerMatricolaValid && isJobValid && isTotalHoursValid)
                    {
                        workDays.Add(new WorkDay(){
                            ID = id, 
                            ActivityDate = date, 
                            JobType = splittedLine[2], 
                            TotalHours = totalHours, 
                            Matricola = splittedLine[4]
                        });
                    }
                    else
                    {
                        errorWorkDays.Add(new ErrorWorkDay(
                            id.ToString(), 
                            date, 
                            splittedLine.Length > 2 ? splittedLine[2] : string.Empty, 
                            totalHours, 
                            splittedLine.Length > 1 ? splittedLine[1] : string.Empty));
                    }
                }

                return Tuple.Create(workDays, errorWorkDays);
            }
            catch (Exception ex)
            {
                var menuException = new MenuException(ex.Message, ex.StackTrace);
                menuException.AddException(menuException);
                return Tuple.Create(new List<WorkDay>(), new List<ErrorWorkDay>());
            }
        }

        internal static List<WorkDay> GetWorkDaysByDate(DateTime date)
        {
            try
            {
                return GetAllWorkDays().Item1.FindAll(day => day.ActivityDate?.ToString("yyyy/MM/dd") == date.ToString("yyyy/MM/dd"));
            }
            catch (Exception ex)
            {
                var menuException = new MenuException(ex.Message, ex.StackTrace);
                menuException.AddException(menuException);
                return [];
            }
        }

        internal static List<WorkerModel> GetWorkersByWorkerDaysDate(DateTime date)
        {
            try
            {
                List<WorkDay> workDays = GetWorkDaysByDate(date);
                List<string?> matricole = workDays.Select(elem => elem.Matricola).ToList();
                List<WorkerModel> workers = [];
                foreach (string? matricola in matricole)
                {
                    WorkerModel? worker = WorkerManager.SearchByMatricola(matricola);
                    if (worker != null)
                    {
                        workers.Add(worker);
                    }
                }
                return workers;
            }
            catch (Exception ex)
            {
                var menuException = new MenuException(ex.Message, ex.StackTrace);
                menuException.AddException(menuException);
                return [];
            }
        }
    }
}
