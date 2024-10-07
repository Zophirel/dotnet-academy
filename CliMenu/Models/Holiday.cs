namespace CliMenu.Models
{

    public record class Holiday {
        public required string Year { get; init; } // Vacanze 2023
        public required string Region { get; init; } // toscana
        public required string Location { get; init; } // lido Camaiore
        public required string DateRange { get; init; } // 01/07/2023-10/07/2023
        public required string AccommodationType { get; init; } // pensione completa
        public required string Children { get; init; } // due bambini
        public required string BeachAccess { get; init; } // spiaggia pagamento
        public required string Matricola { get; init; }
        public List<HolidayExtra> Extras { get; init; } = [];  // extra

        public static void GetWorkerHolidays(WorkerModel worker){
            FolderSearcher.SetWorkerAdditionalInfo(PathConfiguration.Path["ExternalData"], worker);
            string? a = worker.Holidays.Count > 0 ? worker.Holidays[0].Extras[0].Service : "0"; 
            Console.WriteLine($"{worker.FullName} {worker.Holidays.Count} {a}" );
        }

        public string ToConsole(){
            List<string> output = [];

            string holiday = $"""
            Year: {Year}
            Region: {Region} 
            Location: {Location} 
            Date Range: {DateRange} 
            Accomodation: {AccommodationType} 
            Children: {Children} 
            Beach Access: {BeachAccess}";
            Matricola: {Matricola}
            """;

            output.Add(holiday);
            string startExtra = "{";

            output.Add(startExtra);

            foreach (var extra in Extras){
                string holidayExtra = 
                $""" 
                    Service: {extra.Service}
                    Cost: {extra.Cost}
                """;

                output.Add(holidayExtra);
                
            }
            string endExtra = "}";
            output.Add(endExtra);

            return string.Join("\n", output);
        }
    }

    public record class HolidayExtra
    {
        public string? Service { get; init; }
        public string? Cost { get; init; }
    }



}