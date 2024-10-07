namespace CliMenu.Models
{
    public record Person() {
        public required string FirstName { get; init; }
        public required string LastName { get; init; }
        public required int Age { get; init; }
    } 
}