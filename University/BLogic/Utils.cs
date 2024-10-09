using System.Configuration;
using System.Text.Json;
using University.DataModel;

namespace University.BLogic {
    internal class Utils {
        public static List<T> ImportFromCsv<T>(string path)
        {
            var list = new List<T>();
            var lines = File.ReadAllLines(path);
            var header = lines[0].Split(',');

            for (int i = 1; i < lines.Length; i++)
            {
                var values = lines[i].Split(',');
                var obj = Activator.CreateInstance<T>();

                for (int j = 0; j < header.Length; j++)
                {
                    var property = obj?.GetType().GetProperty(header[j]);
            
                    if (property != null)
                    {
                        object convertedValue = ConvertValue(values[j], property.PropertyType);
                        property.SetValue(obj, convertedValue);
                    }
                }
                list.Add(obj);
            }
                     
            UniversityManager.ExportJson(list);
            return list;
        }

        private static object ConvertValue(string value, Type targetType)
        {
            Console.WriteLine($"{value} {targetType}");
            if (targetType == typeof(Guid))
            {
            
                return Guid.Parse(value);
            }
            else if (targetType.IsEnum)
            {
                if(value.Split(" ").Length > 1)
                {
                    value = value.Replace(" ", "_"); 
                }
                
                value = value.ToUpper();
                return Enum.Parse(targetType, value);
            }
            else if(targetType == typeof(Faculty))
            {
                List<Faculty> faculties = UniversityManager.ImportFromJson<Faculty>()!;
                Faculty faculty = faculties?.Find(f => f.Name == (Faculties)Enum.Parse(typeof(Faculties), value))!;
                return faculty;
               
            }else
            {

                return Convert.ChangeType(value, targetType);
            }
        }

        public static void PopulateProfessors()
        {
            List<Professor> professors = UniversityManager.ImportFromJson<Professor>()!;
            List<Exam> exams = UniversityManager.ImportFromJson<Exam>()!;
            List<Courses> courses = UniversityManager.ImportFromJson<Courses>()!;
            Random rInt = new ();
            Random rInt2 = new ();
            Console.WriteLine("Professors: " + professors.Count);
           
            for(int i = 0; i < professors.Count; i++){
                Console.WriteLine(professors[i].Exams.Count);
                if(professors[i].Exams == null || professors[i].Exams.Count == 0)
                {
                    List<Exam> professorExams = exams.FindAll(e => e.Faculty == professors[i].Faculty);
                    
                    
                    int numberOfExams = rInt2.Next(1, 4);
                    for(int j = 0; j < numberOfExams; j++)
                    {
                        professors[i].Exams.Add(professorExams[rInt.Next(0, professorExams.Count)]);
                    }
                }

                
                if(professors[i].Courses == null || professors[i].Courses.Count == 0)
                {
                    List<Courses> course = courses.FindAll(c => c.Faculty == professors[i].Faculty);
                    foreach(Exam exam in professors[i].Exams)
                    {
                        int randCourse = rInt2.Next(0, course.Count);    
                        professors[i].Courses.Add(course[randCourse ]);
                    }
                }
            }
            Console.WriteLine(professors[0].Exams.Count);
            UniversityManager.ExportJson(professors);

        }
        public static void PopulateFaculties(){
            List<Faculty> faculties = UniversityManager.ImportFromJson<Faculty>()!;
            List<Exam> exams = UniversityManager.ImportFromJson<Exam>()!;
            List<Courses> courses = UniversityManager.ImportFromJson<Courses>()!;

            foreach(Faculty faculty in faculties)
            {
                if(faculty.Exams == null || faculty.Exams.Count == 0)
                {
                    Console.WriteLine(faculty.Name);
                    faculty.Exams = exams.FindAll(e => e.Faculty == faculty.Name);

                }

                if(faculty.Courses == null || faculty.Courses.Count == 0)
                {
                    faculty.Courses = courses.FindAll(c => c.Faculty == faculty.Name);
                }
            }

            Console.WriteLine(faculties[0].Exams.Count);

    
            var option = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(faculties, option);
            File.WriteAllText(ConfigurationManager.AppSettings["FileFacultiesJson"], json);
        }
    }    
}