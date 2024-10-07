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
        


        public void ExportJson<T> (string fileName)
        {
                string saveUniversityJson = String.Empty;

                saveUniversityJson = JsonSerializer.Serialize(typeof(T), new JsonSerializerOptions { WriteIndented = true });
                File.AppendAllText(Path.Combine(filePath, fileName), saveUniversityJson.ToString());
        }

        // prompt testo del sottomenu
        // lambda func - controllo all'interno del parametro del funzione quando la chiami
        // 
        private static string GetValidInput(string prompt, Func< string?, bool> validator)
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
                } while (!validator(input)) ;
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
                ExportJson<UniModel>(fileName);

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
                Console.WriteLine("Insert Emplooye FullName.");
                string? name = Console.ReadLine();

                Console.WriteLine("Insert University Address.");
                string? address = Console.ReadLine();

                UniModel university = new(name, address);

                string fileName = Convert.ToString(ConfigurationManager.AppSettings["FileUniversityJson"]);
                ExportJson<UniModel>(fileName);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        // Function to create a Student object
        static Student InsertStudent()
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

            Console.Write("Enter Marital Status (Single/Married/Divorced/Widowed): ");
            string maritalStatus = GetValidInput("1. Single - 2.Married - 3. Divorced - 4. Widowed", input => (int.Parse(input!) > 0 && int.Parse(input!) < 5));

            Console.Write("Enter Matricola (Student ID): ");
            string matricola = Console.ReadLine();

            Console.Write("Enter Registration Year (YYYY-MM-DD): ");
            DateTime registrationYear = DateTime.Parse(Console.ReadLine());

            Console.Write("Enter Degree (Bachelor/Master/PhD/FiveYear): ");
            string degree = Console.ReadLine();

            Console.Write("Enter ISEE (Economic Indicator): ");
            decimal isee = decimal.Parse(Console.ReadLine());

            return new Student
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
        static Professor InsertProfessor()
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

            Console.Write("Enter Marital Status (Single/Married/Divorced/Widowed): ");
            string maritalStatus = Console.ReadLine();

            Console.Write("Enter Role (Professor/Technician/Secretary/etc.): ");
            string role = Console.ReadLine();

            Console.Write("Enter Faculty Name: ");
            string faculty = Console.ReadLine();

            Console.Write("Enter Hiring Year (YYYY-MM-DD): ");
            DateTime hiringYear = DateTime.Parse(Console.ReadLine());

            Console.Write("Enter Salary: ");
            decimal salary = decimal.Parse(Console.ReadLine());

            return new Professor
            {
                FullName = fullName,
                Gender = gender,
                Address = address,
                Email = email,
                Phone = phone,
                BirthYear = birthYear,
                IsFullTime = isFullTime,
                MaritalStatus = (Status)Enum.Parse(typeof(BLogic.Status), maritalStatus!.ToUpper()),
                Role = (Roles)Enum.Parse(typeof(BLogic.Roles), role!.ToUpper()),
                Faculty = null,
                HiringYear = hiringYear,
                Salary = salary
            };
        }

        public void InsertFaculty()
        {

        }

        public void InsertExam()
        {

        }

        public void InsertCourse()
        {

        }

        #endregion 

        public void ImportDataFromFile(string fileName)
        {
            string filePath = Convert.ToString(ConfigurationManager.AppSettings["FilePath"]);
            string filePathFinale = Path.Combine(filePath, fileName);

            try
            {
                string[] stringData = File.ReadAllLines(filePathFinale);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            

        }


    }
}