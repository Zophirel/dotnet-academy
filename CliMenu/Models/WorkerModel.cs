using System.ComponentModel.DataAnnotations;
using CliMenu.Enum;

namespace CliMenu.Models
{
    public record class WorkerModel
    {
        [Key]
        [Required]
        [MinLength(4), MaxLength(4)]
        public required string Matricola { get; init; }

        [Required]
        public required string FullName { get; init; }

        [Required]
        public required Role? Role { get; init; }

        public Department? Department { get; init; }

        [Range(18, 70)]
        public int? Age { get; init; }

        public string? City { get; init; }
        public string? Province { get; init; }
        public string? Address { get; init; }
        public string? Cap { get; init; }
        public string? Phone { get; init; }
        public List<WorkDay> WorkedDays { get; init; } = [];
        public List<Machine> Machines { get; init; } = [];
        public List<Holiday> Holidays { get; init; } = [];
        public List<WeigthCheck> WeigthChecks { get; init; } = [];  
        
        // Necessario per XML de/serializer
        public WorkerModel() { }

        public string ToCSV() {
            string? department = Department.ToString();
            return $"{Matricola};{FullName};{Role};{department ?? ""};{(Age == null ? "" : Age.ToString())};{Address ?? ""};{City ?? ""};{Province ?? ""};{Cap ?? ""};{Phone ?? ""}";
        }
           
        internal string ToConsole(bool full = true)
        {
            WorkerManager.SetWorkerWorkDay(this);
            FolderSearcher.SetWorkerAdditionalInfo(PathConfiguration.Path["ExternalData"], this);
            
            List<string> messages = [];

            const string separator1 = "=====================WORKER=====================";
            const string separator2 = "-------------------WORKED DAY-------------------";
            const string separator3 = "-------------------WORKER INFO-------------------";

            string workerTemplate = $""" 
            Matricola: {Matricola}
            Nome Completo: {FullName}
            Ruolo: ({Role}, {(int?)Role}) 
            Dipartimento: ({Department}, {(int?)Department})
            Eta': {Age}
            Indirizzo: {Address}
            Citta': {City}
            Province: {Province}
            CAP: {Cap}
            Telefono: {Phone}
            """;

            messages.Add(separator1);
            messages.Add(workerTemplate);
            if (full)
            {
                if (WorkedDays.Count == 0)
                {
                    messages.Add(separator2);
                    messages.Add("Nessuna giornata lavorativa presente");
                } else {
                    messages.Add(separator2);
                    foreach (WorkDay workDay in WorkedDays)
                    {
                        messages.Add("\n");
                        messages.Add(workDay.ToConsole());
                    }
                }

                messages.Add(separator3);
                if(Holidays.Count == 0){ 
                    messages.Add("Nessuna vacanza registrata presente");
                }else {
                    foreach(Holiday holiday in Holidays){ 
                        messages.Add(holiday.ToConsole());
                    }
                    messages.Add("\n");
                }
                
                if(Machines.Count == 0){
                    messages.Add("Nessun veicolo registrato presente");
                }else {
                    foreach(Machine machine in Machines){
                        messages.Add(machine.ToConsole());
                    }
                    messages.Add("\n");
                }

                if(WeigthChecks.Count == 0){
                    messages.Add("Nessuna rilevazione di peso presente");
                }else {
                    foreach(WeigthCheck weigth in WeigthChecks){
                        messages.Add(weigth.ToConsole());
                    }
                    messages.Add("\n");
                }
            }
            messages.Add("\n");
            return string.Join("\n", messages);
        }
    }
}
