using Microsoft.Extensions.Configuration;

namespace CliMenu.Models {
    #region Fields
    internal class PathConfiguration {
        private readonly static IConfigurationRoot config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();

        internal readonly static Dictionary<string, string> Path = [];
    #endregion

    #region Constructor
        public PathConfiguration(){
            SetPath();
        }
    #endregion

    #region Methods
        private static void SetPath(){
            if(Path.Count == 0){
                List<IConfigurationSection> configList = config.GetRequiredSection("Path").GetChildren().ToList();
                foreach(var item in configList){
                    Path.Add(item.Key, item.Value!);
                }
            }
        }
    #endregion
    }
}