namespace CliMenu.Models
{
    internal class ErrorWorker(string matricola, string fullName, string role, string? department, int? age,  string? address, string? city, string? province, string? cap, string? phone)
    {
        public string Matricola { get; set; } = matricola;
        public string FullName { get; set; } = fullName;
        public string Role { get; set; } = role;
        public string? Department { get; set; } = department;
        public int? Age { get; set; } = age;
        public string? Address { get; set; } = address;
        public string? City { get; set; } = city;
        public string? Province {get; set;} = province;
        public string? Cap { get; set; } = cap;
        public string? Phone { get; set; } = phone;
        
        internal string ToCSV() => $"{Matricola};{FullName};{Role};{Department ?? ""};{(Age == null ? "" : Age)};{City ?? ""};{Address ?? ""};{Cap ?? ""};{Phone ?? ""}";
        
        internal string ToConsole(){            
            return $""" 
            Matricola: {Matricola}
            Nome Completo: {FullName}
            Ruolo: {Role}
            Dipartimento: {Department}
            Eta': {Age}
            Indirizzo: {Address}
            Citta': {City}
            Provincia: {Province}
            CAP: {Cap}
            Telefono: {Phone}
            """;   
        }
    }
}
