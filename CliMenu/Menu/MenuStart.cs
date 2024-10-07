using CliMenu.Enum;
using CliMenu.Models; 
using System.Globalization;
namespace CliMenu.Menu
{
    internal static class MenuStart
    {
        public static void Show()
        {
            bool end = false;
            while (!end)
            {
                Console.Clear();
                Console.WriteLine(new string('#', 100));
                Console.WriteLine("Welcome to the Academy 6 Menu");
                Console.WriteLine(new string('#', 100));
                Console.WriteLine("1. Add worker.");
                Console.WriteLine("2. Add working week.");
                Console.WriteLine("3. Delete worker / working day.");
                Console.WriteLine("4. View list of workers.");
                Console.WriteLine("5. Search worker by: ID, name, etc.");
                Console.WriteLine("6. Search workers by working date.");
                Console.WriteLine("7. Save workers with additional info.");
                Console.WriteLine("8. Close and exit the program.");
                
                bool parsed = int.TryParse(Console.ReadKey().KeyChar.ToString(), out int result);

                while(!parsed || result < 1 || result > 8){
                    Console.WriteLine("Command not recognized");
                    Console.WriteLine("Enter a valid command");
                    parsed = int.TryParse(Console.ReadKey().KeyChar.ToString(), out result);
                }
                
                switch ((MenuEnum)result)
                {
                    case MenuEnum.AddWorker:
                        // Add worker.
                        Console.Clear();
                        WorkerManager.AddWorker();
                        break;
                    case MenuEnum.AddWeek:
                        // Add working week.
                        Console.Clear();
                        WorkerModel? worker = WorkerManager.SearchByMatricola();

                        if(worker == null){
                            Console.WriteLine("The searched worker does not exist");
                            Console.ReadKey();
                            break;
                        } else {
                            Console.WriteLine("Enter the date from which you want to start the working week in the format DD/MM/YYYY");
                            string? dateInput = Console.ReadLine();
                        
                            DateTime date;    
                            while (DateTime.TryParseExact(dateInput, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date) == false)
                            {
                                Console.WriteLine($"Date inserted {dateInput}");
                                Console.WriteLine("Invalid date, do not forget to separate the digits with '/'");
                                dateInput = Console.ReadLine();
                            }

                            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday){
                                Console.WriteLine("The working week does not include Saturday or Sunday");
                                Console.WriteLine(date.ToString());
                                Console.WriteLine(date.DayOfWeek);
                                Console.ReadKey();
                                break;
                            }
        
                            WorkDayManager workDayManager = new (worker);
                            

                            while(date.DayOfWeek != DayOfWeek.Saturday){
                               
                                // 1 The worker's ID is requested to insert the week
                                // 2 The date from which to start the working week is requested (in the format dd/MM/yyyy)
                                //   Saturdays and Sundays are not accepted
                                // 3 It is asked if you want to fill in the current day or not
                                //   3.1 If you answer no, [AddWorkedDay] returns null
                                //   3.2 If you answer yes, [AddWorkedDay] after taking the data, returns true 
                                //       3.2.1 Once a day is added, it will be asked if you want to continue with the next one (if it is not a Saturday)
                                //             3.2.1.1 If you answer no, [AddWorkedDay] returns false
                                //             3.2.1.2 If you answer yes, [AddWorkedDay] returns true and advances the date by 2 days 
                            
                                bool? isWorkedDayAdded = workDayManager.AddWorkedDay(date);
                             
                                if(isWorkedDayAdded == true || isWorkedDayAdded == null){
                                    date = date.AddDays(1);
                                } else if(isWorkedDayAdded == false){
                                    date = date.AddDays(2);
                                }
                            }
                        }                   
                        break;
                    case MenuEnum.DelWorkerOrDay:
                        // Delete worker / working day.
                        Console.Clear();
                        worker = WorkerManager.SearchByMatricola();
                        if(worker == null){
                            Console.WriteLine("The searched worker does not exist");
                            Console.ReadKey();
                            break;
                        }else{
                            WorkerManager.Delete(worker);  
                        }
                        break;
                    case MenuEnum.ShowWorkers:
                        // View list of workers
                        Console.Clear();
                        WorkerManager.ShowWorkers();
                        Console.ReadKey();
                        break;
                    case MenuEnum.FindWorker:
                        // Search worker by: ID, name, etc.
                        Console.Clear();
                        WorkerManager.SearchForWorker();
                        break;
                    case MenuEnum.FindWorkerByDate:
                        // Search workers by working date.    
                        Console.Clear();
                        Console.WriteLine("Enter the date you want to search for (in the format DD/MM/YYYY)");
                        string? input = Console.ReadLine();
                    
                        DateTime selectedDate;    
                        while (DateTime.TryParseExact(input, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out selectedDate) == false)
                        {
                            Console.WriteLine($"Date inserted {input}");
                            Console.WriteLine("Invalid date, do not forget to separate the digits with '/'");
                            input = Console.ReadLine();
                        }

                        List<WorkerModel> workers = WorkDayManager.GetWorkersByWorkerDaysDate(selectedDate);
                      

                        Console.WriteLine("6");
                        foreach(WorkerModel filteredWorkers in workers){
                            Console.WriteLine(filteredWorkers.ToConsole(false));
                        }
                        Console.WriteLine("------------------------------------------------");
                        
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine("Press any key to return to the menu");
                        Console.ReadLine();
                        Console.ResetColor();
                        break;

                    case MenuEnum.SaveWorkersWithAddedInfo:
                        // Save workers with added info.
                        Console.Clear();
                        foreach(WorkerModel fullWorker in WorkerManager.GetAllWorkers().Item1){
                            WorkerManager.SaveWorkerWithAddedInfo(fullWorker);
                        }
                        
                        break;
                    case MenuEnum.Exit:
                        // Close and exit the program
                        Console.Clear();
                        end = true;
                        break;   
                    default:
                        Console.WriteLine("Command not recognized");
                        break;
                }
            }
        }
    }
}