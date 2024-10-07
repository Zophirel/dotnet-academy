using System.Globalization;
using CliMenu.Enum;


namespace CliMenu.Models
{
    // I've used this class for the excercise of 30/09/2024
    internal class DataImporter
    {

        internal static List<WorkerModel> GetWorkers(){
            var objList = ImportFromCsv();
            List<WorkerModel> result = [];
            foreach(var worker in objList){
                if(worker.GetType() == typeof(WorkerModel)){
                    var workerModel = (WorkerModel) worker;
                    foreach(var day in objList ){
                        if(day.GetType() == typeof(WorkDay)){
                            var workDay = (WorkDay) worker;
                            if(workerModel.Matricola == workDay.Matricola){
                                workerModel.WorkedDays.Add(workDay);
                            }
                        }
                    }
                    result.Add(workerModel);
                }
            }
            return result;
        }


        // Import [WorkModel] and [WorkDay] data from specific csv with mixed data
        internal static List<object> ImportFromCsv()
        {
            string path = $"{PathConfiguration.Path["Csv"]}emplyeeEx.csv";
            List<string> records;

            // Use a list of object to contains both models object 
            List<object> recordsModels = [];

            try
            {
                //get each record of the csv file 
                records = File.ReadLines(path).ToList();
            }
            catch (Exception ex)
            {
                var menuException = new MenuException(ex.Message, ex.StackTrace);
                menuException.AddException(menuException);
                return recordsModels;
            }

            try
            {
                foreach (string record in records)
                {
                    string[] splittedLine = record.Split(';');
                    
                    //from the data we can assume that WorkDay records are long 3 
                    if (splittedLine.Length == 3)
                    {
                        
                        var workDay = ExtractData<WorkDay>(splittedLine);
                        if (workDay != null)
                        {
                            recordsModels.Add(workDay);
                        }
                    }
                    //from the data we can assume that WorkModel records are long 5
                    else if (splittedLine.Length == 5)
                    {
                        var worker = ExtractData<WorkerModel>(splittedLine);
                        if (worker != null)
                        {
                            recordsModels.Add(worker);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Unsupported data format.");
                    }
                }
            }
            catch (Exception ex)
            {
                var menuException = new MenuException(ex.Message, ex.StackTrace);
                menuException.AddException(menuException);
            }

            return recordsModels;
        }

        private static bool InBounds(int index, string[] array) => (index >= 0) && (index < array.Length);

        //Generic function to extract data from splitted lines of the csv and return the referred object
        private static T? ExtractData<T>(string[] data) where T : class
        {
            if (typeof(T) == typeof(WorkDay))
            {
                if (data.Length < 3)
                {
                    var menuException = new MenuException("Insufficient data to create WorkDay object.", null);
                    menuException.AddException(menuException);
                    return null;
                }

                try
                {
                    if (!DateTime.TryParseExact(data[0], "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                    {
                        throw new FormatException($"Invalid date format: {data[0]}");
                    }

                    WorkDay workDay = new(){
                        ActivityDate = date,
                        JobType = data[1],
                        Matricola = data[2] 
                    };

                    return (T)(object)workDay; // Cast WorkDay to object, then to T
                }
                catch (Exception ex)
                {
                    var menuException = new MenuException("Failed to parse WorkDay data.", ex.StackTrace);
                    menuException.AddException(menuException);
                    return null;
                }
            }
            else if (typeof(T) == typeof(WorkerModel))
            {
                if (data.Length < 5)
                {
                    var menuException = new MenuException("Insufficient data to create WorkerModel object.", null);
                    menuException.AddException(menuException);
                    return null;
                }

                try
                {
                    int? age = null;
                    if (!string.IsNullOrEmpty(data[3]))
                    {
                        if (int.TryParse(data[3], out int parsedAge))
                            age = parsedAge;
                        else
                            throw new FormatException("Invalid age format.");
                    }

                    Role? role = null; 
                    if (!string.IsNullOrEmpty(data[1]))
                    {
                        if (System.Enum.TryParse(data[1].ToUpper(), out Role parsedRole))
                            role = parsedRole;
                    }

                    Department? department = null;
                    if (!string.IsNullOrEmpty(data[2]))
                    {
                        var depToParse = string.Join("_", data[2].Split(" ")).ToUpper();
                        if (System.Enum.TryParse(depToParse, out Department parsedDepartment))
                            department = parsedDepartment;
                    }
                    
                    WorkerModel worker = new(){
                        Matricola = data[4],
                        FullName = data[0],
                        Role = role, 
                        Department = department,
                        Age = age,
                        Address = InBounds(5, data) ? data[4] : null,
                        City = InBounds(6, data) ? data[5] : null,
                        Province = InBounds(7, data) ? data[6] : null,
                        Cap = InBounds(8, data) ? data[7] : null,
                        Phone = InBounds(9, data) ? data[8] : null
                    };


                    return (T)(object)worker; // Cast WorkerModel to object, then to T
                }
                catch (Exception ex)
                {
                    var menuException = new MenuException("Failed to parse WorkerModel data.", ex.StackTrace);
                    menuException.AddException(menuException);
                    return null;
                }
            }
            else
            {
                var menuException = new MenuException("Unsupported type", null);
                menuException.AddException(menuException);
                return null;
            }
        }
    }
}
