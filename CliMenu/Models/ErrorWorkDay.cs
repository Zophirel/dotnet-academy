namespace CliMenu.Models
{
    internal class ErrorWorkDay(string ID, DateTime ActivityDate, string JobType, decimal TotalHours, string Matricola)
    {
        public string ID {  get; set; } = ID;
     
        public DateTime ActivityDate { get; set; } = ActivityDate; 
     
        public string JobType { get; set; } = JobType; 
     
        public decimal TotalHours { get; set; } = TotalHours;
     
        public string Matricola { get; set; } = Matricola;

        internal string ToCSV() => $"{ID};{Matricola};{ActivityDate};{JobType};{TotalHours}";
        
        internal string ToConsole(){
            return 
            $""" 
            ID: {ID}
            Matricola: {Matricola}
            Date: {ActivityDate}
            Tipo lavoro: {JobType},
            Ore totali: {TotalHours}
            """;
        }
    }
}
