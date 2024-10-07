using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using University.DataModel;

namespace University.BLogic
{
    public class UniversityManager
    {
        string filePath = Convert.ToString(ConfigurationManager.AppSettings["FileCourses"]);

        // prompt testo del sottomenu
        // lambda func - controllo all'interno del parametro del funzione quando la chiami
        // 
        private static string GetValidInput(string prompt, Func<string?, bool> validator)
        {
            try
            {
                string? input;
                do
                {
                    Console.WriteLine(prompt);
                    input = Console.ReadLine();
                    if (!validator(input))
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.WriteLine("Input non valido, riprova.");
                        Console.ResetColor();
                    }
                } while (!validator(input));
                return input!;

            }
            catch (Exception ex)
            {
                //var menuException = new MenuException(ex.Message, ex.StackTrace);
                //menuException.AddException(menuException);
                return string.Empty;
            }
        }


        #region Insert
        public void InsertUniversity()
        {
            try
            {
                Console.WriteLine("Insert University Name.");
                string? name = Console.ReadLine();

                Console.WriteLine("Insert University Address.");
                string? address = Console.ReadLine();

                UniModel university = new(name, address);

                string fileName = Convert.ToString(ConfigurationManager.AppSettings["FileUniversityJson"]);
                //ExportJson<UniModel>(fileName);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

        }

        public void InsertEmployee()
        {
            try
            {
                Console.Write("Enter Employee Full Name: ");
                string fullName = Console.ReadLine();

                Console.Write("Enter Gender (Male/Female): ");
                string gender = Console.ReadLine();

                Console.Write("Enter Address: ");
                string address = Console.ReadLine();

                Console.Write("Enter Email: ");
                string email = Console.ReadLine();

                Console.Write("Enter Phone Number: ");
                string phone = Console.ReadLine();

                Console.Write("Enter Birth Year (YYYY-MM-DD): ");
                DateTime birthYear = DateTime.Parse(Console.ReadLine());

                Console.Write("Is the Employee full-time? (true/false): ");
                bool isFullTime = bool.Parse(Console.ReadLine());

                string maritalStatus = GetValidInput("1. Single - 2.Married - 3. Divorced - 4. Widowed", input => (int.Parse(input!) > 0 && int.Parse(input!) < 5));

                string prompt = """
                    1 - TECHNICIAN,
                    2 - SECRETARY,
                    3 - CLEANING_STAFF,
                    4 - RECTOR,
                    """;

                string role = GetValidInput(prompt, input => (int.Parse(input!) > 0 && int.Parse(input!) < 5));

                Console.Write("Enter Faculty Name: ");
                string faculty = Console.ReadLine();

                Console.Write("Enter Hiring Year (YYYY-MM-DD): ");
                DateTime hiringYear = DateTime.Parse(Console.ReadLine());

                Console.Write("Enter Salary: ");
                decimal salary = decimal.Parse(Console.ReadLine());

                int roleint = int.Parse(role);
                int statusint = int.Parse(maritalStatus);

                Employee employee = new Employee
                {
                    FullName = fullName,
                    Gender = gender,
                    Address = address,
                    Email = email,
                    Phone = phone,
                    BirthYear = birthYear,
                    IsFullTime = isFullTime,
                    MaritalStatus = (Status)statusint,
                    Role = (Roles)roleint,
                    Faculty = null,
                    HiringYear = hiringYear,
                    Salary = salary
                };

                ExportJson<Employee>([employee]);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        // Function to create a Student object
        static void InsertStudent()
        {

            Console.Write("Enter Student Full Name: ");
            string fullName = Console.ReadLine();

            Console.Write("Enter Gender (Male/Female): ");
            string gender = Console.ReadLine();

            Console.Write("Enter Address: ");
            string address = Console.ReadLine();

            Console.Write("Enter Email: ");
            string email = Console.ReadLine();

            Console.Write("Enter Phone Number: ");
            string phone = Console.ReadLine();

            Console.Write("Enter Birth Year (YYYY-MM-DD): ");
            DateTime birthYear = DateTime.Parse(Console.ReadLine());

            Console.Write("Is the student full-time? (true/false): ");
            bool isFullTime = bool.Parse(Console.ReadLine());

            string maritalStatus = GetValidInput("1. Single - 2.Married - 3. Divorced - 4. Widowed", input => (int.Parse(input!) > 0 && int.Parse(input!) < 5));

            Console.Write("Enter Matricola (Student ID): ");
            string matricola = Console.ReadLine();

            Console.Write("Enter Registration Year (YYYY-MM-DD): ");
            DateTime registrationYear = DateTime.Parse(Console.ReadLine());

            string degree = GetValidInput("1. Bachelor - 2.Master - 3. PhD - 4. Five", input => (int.Parse(input!) > 0 && int.Parse(input!) < 5));

            Console.Write("Enter ISEE (Economic Indicator): ");
            decimal isee = decimal.Parse(Console.ReadLine());

            Student student = new Student
            {
                FullName = fullName,
                Gender = gender,
                Address = address,
                Email = email,
                Phone = phone,
                BirthYear = birthYear,
                IsFullTime = isFullTime,
                MaritalStatus = (Status)Enum.Parse(typeof(BLogic.Status), maritalStatus!.ToUpper()), // parse ritorna obj con il cast lo trasforma in enum status
                Matricola = matricola,
                RegistrationYear = registrationYear,
                Degree = (Degrees)Enum.Parse(typeof(BLogic.Degrees), degree!.ToUpper()),
                ISEE = isee
            };
        }

        // Function to create a Professor object
        static void InsertProfessor()
        {
            Console.Write("Enter Professor Full Name: ");
            string fullName = Console.ReadLine();

            Console.Write("Enter Gender (Male/Female): ");
            string gender = Console.ReadLine();

            Console.Write("Enter Address: ");
            string address = Console.ReadLine();

            Console.Write("Enter Email: ");
            string email = Console.ReadLine();

            Console.Write("Enter Phone Number: ");
            string phone = Console.ReadLine();

            Console.Write("Enter Birth Year (YYYY-MM-DD): ");
            DateTime birthYear = DateTime.Parse(Console.ReadLine());

            Console.Write("Is the professor full-time? (true/false): ");
            bool isFullTime = bool.Parse(Console.ReadLine());

            string maritalStatus = GetValidInput("1. Single - 2.Married - 3. Divorced - 4. Widowed", input => (int.Parse(input!) > 0 && int.Parse(input!) < 5));

            Console.Write("Enter Faculty Name: ");
            string faculty = Console.ReadLine();

            Console.Write("Enter Hiring Year (YYYY-MM-DD): ");
            DateTime hiringYear = DateTime.Parse(Console.ReadLine());

            Console.Write("Enter Salary: ");
            decimal salary = decimal.Parse(Console.ReadLine());

           Professor professore = new Professor
            {
                FullName = fullName,
                Gender = gender,
                Address = address,
                Email = email,
                Phone = phone,
                BirthYear = birthYear,
                IsFullTime = isFullTime,
                MaritalStatus = (Status)Enum.Parse(typeof(BLogic.Status), maritalStatus!.ToUpper()),
                Role = Roles.PROFESSOR,
                Faculty = null,
                HiringYear = hiringYear,
                Salary = salary
            };
        }

        public void InsertFaculty()
        {
            {
                string prompt =
                    """
                    1 - COMPUTER_SCIENCE
                    2 - BUSINESS_AND_MANAGEMENT,
                    3 - MATHEMATICS,
                    4 - PSYCHOLOGY,
                    5 - LAW,
                    6 - FASHION_DESIGN,
                    7 - NURSING,
                    8 - LANGUAGES,
                    9 - BIOLOGY
                    """;

                string name = GetValidInput(prompt, input => (int.Parse(input!) > 0 && int.Parse(input!) < 10));

                Console.Write("Enter Faculty Address: ");
                string address = Console.ReadLine();

                Console.Write("Enter Number of Students: ");
                int studentsNumber = int.Parse(Console.ReadLine());

                Console.Write("Enter Number of Labs: ");
                int labsNumber = int.Parse(Console.ReadLine());

                Console.Write("Does the Faculty have a library? (true/false): ");
                bool hasLibrary = bool.Parse(Console.ReadLine());

                Console.Write("Does the Faculty have a canteen? (true/false): ");
                bool hasCanteen = bool.Parse(Console.ReadLine());

                int nameint = int.Parse(name);

                Faculty faculty = new Faculty
                {
                    Name = (Faculties) nameint,
                    Address = address!,
                    StudentsNumber = studentsNumber,
                    LabsNumber = labsNumber,
                    HasLibrary = hasLibrary,
                    HasCanteen = hasCanteen
                };
            }
        }

        public void InsertExam()
        {
            Console.Write("Enter Exam Name: ");
            string name = Console.ReadLine();

            string faculty = Console.ReadLine();

            Console.Write("Enter CFU (Credits): ");
            int cfu = int.Parse(Console.ReadLine());

            Console.Write("Enter Exam Date (YYYY-MM-DD): ");
            DateTime date = DateTime.Parse(Console.ReadLine());

            Console.Write("Is the Exam Online? (true/false): ");
            bool isOnline = bool.Parse(Console.ReadLine());

            Console.Write("Enter Number of Participants: ");
            int participants = int.Parse(Console.ReadLine());


            string prompt = """
                1 - WRITTEN,
                2 - ORAL,
                3 - WRITTEN_AND_ORAL,
                """;

            string examType = GetValidInput(prompt, input => (int.Parse(input!) > 0 && int.Parse(input!) < 10));

            Console.Write("Is a Project Required? (true/false): ");
            bool isProjectRequired = bool.Parse(Console.ReadLine());

            int examint = int.Parse(examType);

            Exam exam = new Exam
            {
                Name = name!,
                Faculty = null,
                CFU = cfu,
                Date = date,
                IsOnline = isOnline,
                Participants = participants,
                ExamType = (ExamType ) examint,
                IsProjectRequired = isProjectRequired
            };
        }

        public void InsertCourse()
        {
            Console.Write("Enter Course Name: ");
            string name = Console.ReadLine();

            Console.Write("Enter CFU (Credits): ");
            int cfu = int.Parse(Console.ReadLine());

            Console.Write("Is the Course Online? (true/false): ");
            bool isOnline = bool.Parse(Console.ReadLine());

            string prompt = """

                1 - A,
                2 - B,
                3 - C,
                4 - D,
                5 - E,
                6 - F,
                7 - LAB_1,
                8 - LAB_2,
                9 - LAB_3
                """;

            string classroom = GetValidInput(prompt, input => (int.Parse(input!) > 0 && int.Parse(input!) < 10));

            int classroomint = int.Parse(classroom);

            Courses course = new Courses
            {
                Name = name,
                Faculty = null,
                CFU = cfu,
                IsOnline = isOnline,
                Classroom = (Classroom) classroomint,
            };
        }

        #endregion 

        internal static List<T> ImportFromJson<T>()
        {
            try
            {
                string Json = string.Empty;

                if (typeof(T) == typeof(Student))
                {
                     Json = File.ReadAllText (ConfigurationManager.AppSettings["FileStudentsJson"]);
                }
                else if (typeof(T) == typeof(Professor))
                {
                     Json = File.ReadAllText (ConfigurationManager.AppSettings["FileProfessorsJson"]);
                }
                else if (typeof(T) == typeof(Exam))
                {
                     Json = File.ReadAllText (ConfigurationManager.AppSettings["FileExamsJson"]);
                }
                else if (typeof(T) == typeof(Courses))
                {
                     Json = File.ReadAllText (ConfigurationManager.AppSettings["FileCoursesJson"]);
                }
                else if (typeof(T) == typeof(Faculty))
                {
                     Json = File.ReadAllText (ConfigurationManager.AppSettings["FileFacultiesJson"]);
                }

                return JsonSerializer.Deserialize<List<T>>(Json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return [];
            }
        }

        internal static bool ExportJson<T>(List<T> list)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
        
                if(typeof(T) == typeof(Student)){
                    List<Student> studentList = ImportFromJson<Student>();
                    List<Student> studentList2 = list.OfType<Student>().ToList();
                    studentList.AddRange(studentList2);
                    string json = JsonSerializer.Serialize(studentList, options);
                    File.WriteAllText(ConfigurationManager.AppSettings["FileStudentsJson"], json);
                    
                } else if (typeof(T) == typeof(Professor))
                {
                    List<Professor> professorList = ImportFromJson<Professor>();
                    List<Professor> professorList2 = list.OfType<Professor>().ToList();
                    professorList.AddRange(professorList2);
                    string json = JsonSerializer.Serialize(professorList, options);
                    File.WriteAllText(ConfigurationManager.AppSettings["FileProfessorsJson"], json);
                }
                else if(typeof(T) == typeof(Exam))
                {
                    List<Exam> examList = ImportFromJson<Exam>();
                    List<Exam> examList2 = list.OfType<Exam>().ToList();
                    examList.AddRange(examList2);
                    string json = JsonSerializer.Serialize(examList, options);
                    File.WriteAllText(ConfigurationManager.AppSettings["FileExamsJson"], json);
                }
                else if (typeof(T) == typeof(Courses))
                {
                    List<Courses> coursesList = ImportFromJson<Courses>();
                    List<Courses> coursesList2 = list.OfType<Courses>().ToList();
                    coursesList.AddRange(coursesList2);
                    string json = JsonSerializer.Serialize(coursesList, options);
                    File.WriteAllText(ConfigurationManager.AppSettings["FileCoursesJson"], json);
                }
                else if (typeof(T) == typeof(Faculty))
                {
                    List<Faculty> facultyList = ImportFromJson<Faculty>();
                    List<Faculty> facultyList2 = list.OfType<Faculty>().ToList();
                    facultyList.AddRange(facultyList2);
                    string json = JsonSerializer.Serialize(facultyList, options);
                    File.WriteAllText(ConfigurationManager.AppSettings["FileFacultiesJson"], json);
                }

            return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

    }
}