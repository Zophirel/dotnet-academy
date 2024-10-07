namespace CliMenu.Models
{

    internal class FirstWeekResume {
        public Guid Id { get; } = Guid.NewGuid();
        public string? FullName { get; set; }
        public string? Mail { get; set; }

        readonly List<FirstWeekResume> FirstWeeks = [];

        internal FirstWeekResume(){

        }

        internal FirstWeekResume(string fullName, string email){
            FullName = fullName;
            Mail = email;
        }

        internal List<FirstWeekResume> AddNames(FirstWeekResume resume){
            try
            {
                FirstWeeks.Add(resume);    
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Attenzione si è verificato un errore! {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
            return FirstWeeks;   
        }
    }
}
