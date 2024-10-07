using CliMenu.Menu;
using CliMenu.Models;
namespace CliMenu
{
    internal class Program
    {      
        static void Main(string[] args)
        {
            // init external parameters from appsettings.json
            _ = new PathConfiguration();    
            WorkerManager.DetectSeparator();    
            //MenuStart.Show();
        }
    }
}