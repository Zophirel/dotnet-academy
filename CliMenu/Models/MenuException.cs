using System.Text.Json;
using System.Text.Json.Nodes;

namespace CliMenu.Models {

    internal class MenuException {
        public string? Message { get; }
        public string? StackTrace { get; }
        public DateTime? Time { get; }
        readonly static List<MenuException> Exceptions = [];
        readonly static string JsonPath = PathConfiguration.Path["Json"];
        public MenuException(){}

        public MenuException(string message, string? stackTrace){
            StackTrace = stackTrace;
            Message = message;
            Time = DateTime.Now;
        }

        public void AddException(MenuException exception){
            try{
                if(exception.Time == null){
                    Console.WriteLine("Exception non valida");
                } else {
                    DateTime today = DateTime.Now;
                    Exceptions.Add(exception);
                    var options = new JsonSerializerOptions { WriteIndented = true };
                    File.AppendAllText($"{JsonPath}{today:dMyyyy}exceptions_record.json", JsonSerializer.Serialize(Exceptions, options));
                }
            } catch(Exception e){
                Console.WriteLine(e);
            }          
        }

        public static void ReadExcepitonRecord(){
            try{
                DateTime today = DateTime.Now;
                string path = $"{JsonPath}{today:dMyyyy}exceptions_record.json";
                if(File.Exists(path)){
                    Console.WriteLine(File.ReadAllText(path));
                } else {
                    Console.WriteLine("Ancora nessun errore intercettato!");
                }
            }catch(Exception e){
                Console.WriteLine(e);
            }
        }

        public static List<MenuException>? GetExceptionRecord(){
            try{
                DateTime today = DateTime.Now;
                string path = $"{JsonPath}{today:dMyyyy}exceptions_record.json";
                if(File.Exists(path)){
                    string data = File.ReadAllText(path);
                    return JsonSerializer.Deserialize<List<MenuException>>(data);
                } else {
                    Console.WriteLine("Ancora nessun errore intercettato!");
                    return [];
                }
            }catch(Exception e){
                Console.WriteLine(e);
            }
            return [];
        }
    }
}