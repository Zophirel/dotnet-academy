namespace CliMenu.Models
{
    public record Machine
    {
        public required string Marca { get; init; }
        public required string Tipo { get; init; }
        public required string Cilindrata { get; init; }
        public required int Anno { get; init; }
        public required string Accidents { get; init; }
        public required string Matricola { get; init; }
        public string ToConsole()
        {
            return $"""
            Marca: {Marca}
            Tipo: {Tipo}
            Cilindrata: {Cilindrata}
            Anno: {Anno}
            Accidents: {Accidents}
            Matricola: {Matricola}
            """;
        }
    }

    
}