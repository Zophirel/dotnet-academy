namespace CliMenu.Models
{
    public record WeigthCheck
    {
        public required DateTime DateOfMesurament { get; init; }
        public required int Weigth { get; init; }

        public required string Matricola { get; init; }
    
        public string ToConsole()
        {
            return $"""
            Date of Mesurament: {DateOfMesurament}
            Weigth: {Weigth}
            Matricola: {Matricola}
            """;
        }
    }
}