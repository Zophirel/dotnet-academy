using System.Globalization;

namespace CliMenu.Models{
	public class FolderSearcher
	{
		private static List<string> EnumerateFiles(string rootDirectory)
		{
			List<string> files = [];

			try
			{
				// Ensure that the root directory exists
				if (!Directory.Exists(rootDirectory))
				{
					Console.WriteLine($"Directory {rootDirectory} does not exist.");
					return files;
				}

				// Get all files in the root directory
				string[] fileEntries = Directory.GetFiles(rootDirectory);
				files.AddRange(fileEntries);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred while enumerating files: {ex.Message}");
				Console.WriteLine(ex.StackTrace);
			}

			return files;
		}

		// Check wether the an info folder exists for the worker, if so check if there are any holiday files and add them to the worker
		public static void SetWorkerAdditionalInfo(string rootDirectory, WorkerModel worker)
		{
			List<List<Holiday>> dataFound = [];

			try
			{
				// Ensure that the root directory exists
				if (!Directory.Exists(rootDirectory))
				{
					Console.WriteLine($"Directory {rootDirectory} does not exist.");
					return;
				}

				// Use a stack to manage directories to be processed
				Stack<string> directoriesToProcess = [];
				directoriesToProcess.Push(rootDirectory);

				while (directoriesToProcess.Count > 0)
				{
					string currentDirectory = directoriesToProcess.Pop();

					try
					{
						// Get all subdirectories in the current directory
						string[] subdirectories = Directory.GetDirectories(currentDirectory);
						foreach (string subdirectory in subdirectories)
						{
							if (Path.GetFileName(subdirectory).StartsWith("Info"))
							{
								string workerFullName = string.Join(" ", Path.GetFileName(subdirectory).Split(" ")[1..]);
								
								if(worker.FullName == workerFullName){
									var folderFiles = EnumerateFiles(subdirectory);
									// Get all Worker holiday
									List<string> vacanzeCsv = folderFiles.FindAll(file => file.Split('\\').Last().StartsWith("Vacanze", StringComparison.CurrentCultureIgnoreCase));
									foreach(string vacanza in vacanzeCsv){
										worker.Holidays.AddRange(FetchHolidaysFromFile(vacanza, worker.Matricola));
									}

									// Get all Worker machines
									List<string> MachineCsv = folderFiles.FindAll(file => file.Split('\\').Last().StartsWith("Mezzi", StringComparison.CurrentCultureIgnoreCase));
									foreach(string machine in MachineCsv){
										worker.Machines.AddRange(FetchFromFile(machine, worker.Matricola, parseLine: ParseMachine));
									}
									
									// Get all Worker Weigth mesuraments
									List<string> WeigthCsv = folderFiles.FindAll(file => file.Split('\\').Last().StartsWith("RilevazionePeso", StringComparison.CurrentCultureIgnoreCase));
									
									foreach(string weigth in WeigthCsv){
										worker.WeigthChecks.AddRange(FetchFromFile(weigth, worker.Matricola, parseLine: ParseWeigthCheck));
									}
											
								} 
							}
							directoriesToProcess.Push(subdirectory);
						}
					}
					catch (Exception ex)
					{
						Console.WriteLine($"An error occurred while processing directory {currentDirectory}: {ex.Message}");
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred: {ex.Message}");
			}
		}

		private static List<Holiday> FetchHolidaysFromFile(string csvFilePath, string matricola)
		{
			List<Holiday> holidays = [];

			try
			{
				if (!File.Exists(csvFilePath))
				{
					Console.WriteLine($"File {csvFilePath} does not exist.");
					return holidays;
				}

				string[] lines = File.ReadAllLines(csvFilePath);
				Holiday? currentHoliday = null;

				foreach (string line in lines)
				{
					string[] values = line.Split(';');
					if (values.Length > 2)
					{
						// Create a new Holiday object
						currentHoliday = new Holiday
						{
							Year = values[0],
							Region = values[1],
							Location = values[2],
							DateRange = values[3],
							AccommodationType = values[4],
							Children = values[5],
							BeachAccess = values[6],
							Matricola = matricola,
						};
						holidays.Add(currentHoliday);
					}
					else if (values.Length == 2 && currentHoliday != null)
					{
						// Add HolidayExtra to the current Holiday object
						currentHoliday.Extras.Add(new HolidayExtra
						{
							Service = values[0],
							Cost = values[1]
						});
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred while reading the CSV file: {ex.Message}");
			}

			return holidays;
		}
		public static List<T> FetchFromFile<T>(string csvFilePath, string matricola, Func<string[], string, T> parseLine)
		{
			List<T> records = [];

			try
			{
				if (!File.Exists(csvFilePath))
				{
					Console.WriteLine($"File {csvFilePath} does not exist.");
					return records;
				}

				string[] lines = File.ReadAllLines(csvFilePath);
				foreach (string line in lines)
				{
					string[] values = line.Split(';');
					T record = parseLine(values, matricola);
					records.Add(record);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred while reading the CSV file: {ex.Message}");
				Console.WriteLine(ex.StackTrace);
			}

			return records;
		}

		static Machine ParseMachine(string[] values, string matricola)
		{
			return new Machine
			{
				Marca = values[0],
				Tipo = values[1],
				Cilindrata = values[2],
				Anno = int.Parse(values[3]),
				Accidents = values[4],
				Matricola = matricola
			};
		}

		static WeigthCheck ParseWeigthCheck(string[] values, string matricola)
		{
			return new WeigthCheck
			{
				Matricola = matricola,
				DateOfMesurament = DateTime.ParseExact(values[0], "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None),
				Weigth = int.Parse(values[1])
			};
		}
	}	
}