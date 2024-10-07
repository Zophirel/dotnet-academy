using System.ComponentModel.DataAnnotations;

namespace CliMenu.Models
{
    public record WorkDay
    {
        [Key]
        [Required]
        public Guid ID { get; init; } = Guid.NewGuid();

        [Required]
        public DateTime? ActivityDate { get; init; }

        [Required]
        public string? JobType { get; init; }

        [Required]
        public decimal? TotalHours { get; init; }

        [Required]
        public string? Matricola { get; init; }

        // Parameterless constructor
        public WorkDay() { }

        // Method to convert to CSV format
        internal string ToCSV() => $"{ID};{Matricola};{ActivityDate};{JobType};{TotalHours}";

        // Method to convert to console output format
        internal string ToConsole()
        {
            return 
            $""" 
            ID: {ID}
            Matricola: {Matricola}
            Date: {ActivityDate}
            Tipo lavoro: {JobType}
            Ore totali: {TotalHours}
            """;
        }
    }
}