using System.Configuration;
using System.Globalization;
using System.Text.Json;
using University.DataModel;
namespace University.BLogic
{
    public class UniversityManager
    {
        readonly static string separator = Convert.ToString(ConfigurationManager.AppSettings["SeparationLine"])!; 
        readonly string filePath = Convert.ToString(ConfigurationManager.AppSettings["FileCourses"])!;

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
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"{ex.StackTrace}");
                return string.Empty;
            }
        }

        private static string GetFaculty(){
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

                return GetValidInput(prompt, input => int.Parse(input!) > 0 && int.Parse(input!) < 10);
        }
       
        private static string GetRole(){
            string prompt = """
                Enter Role:
                1 - TECHNICIAN,
                2 - SECRETARY,
                3 - CLEANING_STAFF,
                4 - RECTOR,
                """;

            return GetValidInput(prompt, input => int.Parse(input!) > 0 && int.Parse(input!) < 5);
        }
    
        private static string GetMaritialStatus(){
            string prompt = """
                Enter Marital Status:
                1 - SINGLE,
                2 - MARRIED,
                3 - DIVORCED,
                4 - WIDOWED,
                """;
            return GetValidInput(prompt, input => int.Parse(input!) > 0 && int.Parse(input!) < 5);
        }

        private static string GetDegree(){
            string prompt = """
                Enter Degree:
                1 - BACHELOR,
                2 - MASTER,
                3 - PHD,
                4 - FIVE,
                """;
            return GetValidInput(prompt, input => int.Parse(input!) > 0 && int.Parse(input!) < 5);
        }

        private static string GetExamType(){
            string prompt = """
                Enter Exam Type:
                1 - WRITTEN,
                2 - ORAL,
                3 - WRITTEN_AND_ORAL,
                """;
            return GetValidInput(prompt, input => int.Parse(input!) > 0 && int.Parse(input!) < 4);
        }   

        private static string GetClassroom(){
            string prompt = """
                Select Classroom:
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

            return GetValidInput(prompt, input => int.Parse(input!) > 0 && int.Parse(input!) < 10);
        }

        #region Create
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

                string maritalStatus = GetMaritialStatus();
                string role = GetRole();
                string faculty = GetFaculty();
                
                Console.Write("Enter Hiring Year (YYYY-MM-DD): ");
                DateTime hiringYear = DateTime.Parse(Console.ReadLine());

                Console.Write("Enter Salary: ");
                decimal salary = decimal.Parse(Console.ReadLine());

                int roleint = int.Parse(role);
                int statusint = int.Parse(maritalStatus);

                Employee employee = new()
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
                    Faculty = (Faculties) int.Parse(faculty),
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

            string maritalStatus = GetMaritialStatus();

            Console.Write("Enter Matricola (Student ID): ");
            string matricola = Console.ReadLine();

            Console.Write("Enter Registration Year (YYYY-MM-DD): ");
            DateTime registrationYear = DateTime.Parse(Console.ReadLine());

            string degree = GetDegree();

            Console.Write("Enter ISEE (Economic Indicator): ");
            decimal isee = decimal.Parse(Console.ReadLine());

            Student student = new()
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

            string maritalStatus = GetMaritialStatus();
            string faculty = GetFaculty();
           
            Console.Write("Enter Hiring Year (YYYY-MM-DD): ");
            DateTime hiringYear = DateTime.Parse(Console.ReadLine());

            Console.Write("Enter Salary: ");
            decimal salary = decimal.Parse(Console.ReadLine());

           Professor professore = new()
           {
                FullName = fullName,
                Gender = gender,
                Address = address,
                Email = email,
                Phone = phone,
                BirthYear = birthYear,
                IsFullTime = isFullTime,
                MaritalStatus = (Status)Enum.Parse(typeof(Status), maritalStatus!.ToUpper()),
                Role = Roles.PROFESSOR,
                Faculty = (Faculties)int.Parse(faculty),
                HiringYear = hiringYear,
                Salary = salary
            };
        }

        public static void InsertFaculty()
        {
            {
                string name = GetFaculty();

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

                Faculty faculty = new()
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

        public static void InsertExam()
        {
            Console.Write("Enter Exam Name: ");
            string name = Console.ReadLine();

            string faculty = GetFaculty();

            Console.Write("Enter CFU (Credits): ");
            int cfu = int.Parse(Console.ReadLine());

            Console.Write("Enter Exam Date (YYYY-MM-DD): ");
            DateTime date = DateTime.Parse(Console.ReadLine());

            Console.Write("Is the Exam Online? (true/false): ");
            bool isOnline = bool.Parse(Console.ReadLine());

            Console.Write("Enter Number of Participants: ");
            int participants = int.Parse(Console.ReadLine());
            
            string examType = GetExamType();
            
            Console.Write("Is a Project Required? (true/false): ");
            bool isProjectRequired = bool.Parse(Console.ReadLine());

            int examint = int.Parse(examType);

            Exam exam = new()
            {
                Name = name!,
                Faculty = (Faculties) int.Parse(faculty),
                CFU = cfu,
                Date = date,
                IsOnline = isOnline,
                Participants = participants,
                ExamType = (ExamType) examint,
                IsProjectRequired = isProjectRequired
            };
        }

        public static void InsertCourse()
        {
            
            string faculty = GetFaculty();
            Console.Write("Enter Course Name: ");
            string name = Console.ReadLine();

            Console.Write("Enter CFU (Credits): ");
            int cfu = int.Parse(Console.ReadLine());

            Console.Write("Is the Course Online? (true/false): ");
            bool isOnline = bool.Parse(Console.ReadLine());

            string classroom = GetClassroom();

            int classroomint = int.Parse(classroom);

            Courses course = new()
            {
                Name = name,
                Faculty = (Faculties) int.Parse(faculty),
                CFU = cfu,
                IsOnline = isOnline,
                Classroom = (Classroom) classroomint,
            };
        }

        #endregion 

        #region Read

        public static void ReadEmployees()
        {
            // Assuming you have a method to get the list of employees
            List<Employee> employees = ImportFromJson<Employee>();

            if (employees == null || employees.Count == 0)
            {
                Console.WriteLine("No employees found.");
                return;
            }

            foreach (var employee in employees)
            {
                Console.WriteLine("Employee Details:");
                Console.WriteLine($"ID: {employee.Id}");
                Console.WriteLine($"Full Name: {employee.FullName}");
                Console.WriteLine($"Gender: {employee.Gender}");
                Console.WriteLine($"Address: {employee.Address}");
                Console.WriteLine($"Email: {employee.Email}");
                Console.WriteLine($"Phone Number: {employee.Phone}");
                Console.WriteLine($"Birth Year: {employee.BirthYear}");
                Console.WriteLine($"Is Full Time: {employee.IsFullTime}");
                Console.WriteLine($"Marital Status: {employee.MaritalStatus}");
                Console.WriteLine($"Role: {employee.Role}");
                Console.WriteLine($"Faculty: {employee.Faculty}");
                Console.WriteLine($"Hiring Year: {employee.HiringYear}");
                Console.WriteLine($"Salary: {employee.Salary}");
                Console.WriteLine(separator);
            }
        }

        public static void ReadProfessors()
        {
            // Deserialize the list of professors from JSON
            List<Professor> professors = ImportFromJson<Professor>();

            // Check if the list is null or empty
            if (professors == null || professors.Count == 0)
            {
                Console.WriteLine("No professors found.");
                return;
            }

            // Iterate through each professor and print their details
            foreach (var professor in professors)
            {
                Console.WriteLine("Professor Details:");
                Console.WriteLine($"ID: {professor.Id}");
                Console.WriteLine($"Full Name: {professor.FullName}");
                Console.WriteLine($"Gender: {professor.Gender}");
                Console.WriteLine($"Address: {professor.Address}");
                Console.WriteLine($"Email: {professor.Email}");
                Console.WriteLine($"Phone Number: {professor.Phone}");
                Console.WriteLine($"Birth Year: {professor.BirthYear.ToShortDateString()}");
                Console.WriteLine($"Is Full Time: {professor.IsFullTime}");
                Console.WriteLine($"Marital Status: {professor.MaritalStatus}");
                Console.WriteLine($"Role: {professor.Role}");
                Console.WriteLine($"Faculty: {professor.Faculty}");
                Console.WriteLine($"Hiring Year: {professor.HiringYear.ToShortDateString()}");
                Console.WriteLine($"Salary: {professor.Salary:C}");

                // Print Exams
                Console.WriteLine("Exams:");
                if (professor.Exams != null && professor.Exams.Count > 0)
                {
                    foreach (var exam in professor.Exams)
                    {
                        Console.WriteLine($" - {exam.Name} on {exam.Date.ToShortDateString()}");
                    }
                }
                else
                {
                    Console.WriteLine(" No exams assigned.");
                }

                // Print Courses
                Console.WriteLine("Courses:");
                if (professor.Courses != null && professor.Courses.Count > 0)
                {
                    foreach (var course in professor.Courses)
                    {
                        Console.WriteLine($" - {course.Name}");
                    }
                }
                else
                {
                    Console.WriteLine(" No courses assigned.");
                }

                // Print separator
                Console.WriteLine(separator);
            }
        }

        public static void ReadStudents()
        {
            try
            {
                List<Student> students = ImportFromJson<Student>();
        
                if (students?.Count == 0)
                {
                    Console.WriteLine("There are no students to show.\n");
                }
                else
                {
                    foreach (Student? student in students)
                    {
                        Console.WriteLine("Personal Details");
                        Console.WriteLine($"Fullname: {student.FullName}");
                        Console.WriteLine($"Gender: {student.Gender}");
                        Console.WriteLine($"Address: {student.Address}");
                        Console.WriteLine($"Email: {student.Email}");
                        Console.WriteLine($"Phone: {student.Phone}");
                        Console.WriteLine($"Birth Date: {student.BirthYear}");
                        Console.WriteLine($"Status: {student.MaritalStatus}");
                        Console.WriteLine(separator);
        
                        Console.WriteLine();
        
                        Console.WriteLine("University Data");
                        Console.WriteLine($"ID: {student.Matricola}");
                        Console.WriteLine($"Registration Year: {student.RegistrationYear}");
                        Console.WriteLine($"Faculty: {student.Faculty}");
                        Console.WriteLine($"Degree: {student.Degree}");
                        Console.WriteLine($"Full Time: {student.IsFullTime}");
                        Console.WriteLine(separator);
        
                        Console.WriteLine("Financial Data");
                        Console.WriteLine($"ISEE: {student.ISEE}");
                        Console.WriteLine(separator);
                    }
                }
            }
            catch (Exception  ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        
        }

        public static void ReadExams()
        {
            try
            {
                List<Exam> exams = ImportFromJson<Exam>();

                Console.Clear();

                if (exams.Count == 0)
                {
                    Console.WriteLine("There are no exams to show.\n");
                }
                else
                {
                    foreach (Exam exam in exams)
                    {
                        Console.WriteLine("Id: " + exam.Id + "\n"); 
                        Console.WriteLine("Faculty: " + exam.Faculty + "\n");
                        Console.WriteLine("Exam Name: " + exam.Name + "\n");
                        Console.WriteLine("CFU: " + exam.CFU + "\n");
                        Console.WriteLine("Exam Date: " + exam.Date + "\n");

                        if (exam.IsOnline)
                        {
                            Console.WriteLine("The exam is online");
                        }
                        else
                        {
                            Console.WriteLine("The exam is takes place on site");
                        }

                        Console.WriteLine("Participants: " + exam.Participants + "\n");

                        Console.WriteLine("Exam Type:");
                        switch (exam.ExamType)
                        {
                            case ExamType.WRITTEN:
                                Console.WriteLine("The exam is written.\n");
                                break;
                            case ExamType.WRITTEN_AND_ORAL:
                                Console.WriteLine("The exam is written and oral.\n");
                                break;
                            case ExamType.ORAL:
                                Console.WriteLine("The exam is oral.\n");
                                break;
                            default:
                                break;
                        }

                        if (exam.IsProjectRequired)
                        {
                            Console.WriteLine("The exam requires a project\n");
                        }
                        else
                        {
                            Console.WriteLine("The exam doesn't require a project\n");
                        }
                        Console.WriteLine(ConfigurationManager.AppSettings["SeparationLine"]);
                    }                    
                }               

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        #endregion

        #region Update

        public static void UpdateProfessors()
        {
            try
            {
                List<Professor> professors = ImportFromJson<Professor>(); // Import existing data

                if (professors == null || professors.Count == 0)
                {
                    Console.WriteLine("There are no professors saved.\n");
                    return;
                }

                Console.WriteLine("Insert the ID of the professor (UUID format: xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx)");
                string id = Console.ReadLine();

                // Validate UUID and check length
                while (!Guid.TryParse(id, out Guid guid) || id.Length != 36)
                {
                    Console.WriteLine("Invalid input - the ID must be in UUID format (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx) and 36 characters long.");
                    Console.WriteLine("Insert the ID of the professor again:");
                    id = Console.ReadLine();
                }

                Professor? prof = professors.Find(prof => prof.Id == Guid.Parse(id));
                if (prof == null)
                {
                    Console.WriteLine("Professor not found.");
                    return;
                }

                bool doWhile = true;
                string n;
                string prompt =
                    """
                    1.  Change Full Name
                    2.  Change Gender
                    3.  Change Address
                    4.  Change Email
                    5.  Change Phone
                    6.  Change Birth Year
                    7.  Change Marital Status
                    8.  Change Full-Time Status
                    9. Change Hiring Year
                    10. Change Faculty
                    11. Change Salary
                    """;

                do
                {
                    Console.WriteLine($"Changing professor {prof.Id} information");
                    Console.WriteLine(prompt);
                    n = GetValidInput("Which info do you want to change? (press a number)", input => int.Parse(input!) > 0 && int.Parse(input!) < 12);

                    switch (n)
                    {
                        case "1":
                            Console.WriteLine("Insert new name");
                            prof.FullName = Console.ReadLine();
                            break;

                        case "2":
                            
                            prof.Gender = GetValidInput("Insert new gender (Male / Female / X)", input => input?.ToUpper() == "MALE" || input?.ToUpper() == "FEMALE" || input?.ToUpper() == "X");
                            break;

                        case "3":
                            Console.WriteLine("Insert new address");
                            prof.Address = Console.ReadLine();
                            break;

                        case "4":
                            Console.WriteLine("Insert new email");
                            prof.Email = Console.ReadLine(); // Assuming email needs to be updated
                            break;

                        case "5":
                            Console.WriteLine("Insert new phone (10 digits)");
                            string phone = Console.ReadLine();
                            while (phone.Length != 10 || !long.TryParse(phone, out _))
                            {
                                Console.WriteLine("Invalid input - the phone number must be 10 digits.");
                                phone = Console.ReadLine();
                            }
                            prof.Phone = phone;
                            break;

                        case "6":
                            Console.WriteLine("Insert new birth year (YYYY-MM-DD)");
                            prof.BirthYear = DateTime.Parse(Console.ReadLine());
                            break;

                        case "7":
                            Console.WriteLine($"Change Full-Time Status: from {prof.IsFullTime} to {!prof.IsFullTime}? (yes / no)");
                            string answer = Console.ReadLine().ToLower();
                            if (answer.Equals("yes"))
                            {
                                prof.IsFullTime = !prof.IsFullTime;
                            }
                            break;

                        case "8":
                            Console.WriteLine("Insert new hiring year (YYYY-MM-DD)");
                            prof.HiringYear = DateTime.Parse(Console.ReadLine());
                            break;

                        case "9":
                            string faculty = GetFaculty();
                            prof.Faculty = (Faculties) int.Parse(faculty); 
                            
                            break;

                        case "10":
                            Console.WriteLine("Insert new salary");
                            decimal salary = Convert.ToDecimal(Console.ReadLine());
                            while (salary <= 0)
                            {
                                Console.WriteLine("Invalid input - the salary must be a positive number.");
                                salary = Convert.ToDecimal(Console.ReadLine());
                            }
                            prof.Salary = salary;
                            break;

                        default:
                            Console.WriteLine("Invalid input.");
                            break;
                    }

                    Console.WriteLine("Do you want to change something else about this professor? (yes / no)");
                    string answerFinal = Console.ReadLine().ToLower();

                    doWhile = answerFinal.Equals("yes");

                } while (doWhile);

                // Save changes
                ExportJson(professors);
                Console.WriteLine("Professor information updated and saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public static void UpdateStudents()
        {
            try
            {
                List<Student> students = ImportFromJson<Student>();
                bool loop = true;
                bool doWhile = true;
                string n;
                string prompt =
                    """
                    1.  Change Full Name
                    2.  Change Gender
                    3.  Change Address
                    4.  Change Email
                    5.  Change Phone
                    6.  Change Birth Year
                    7.  Change Status
                    8.  Change Full Time
                    9.  Change ID
                    10. Change Registration Year
                    11. Change Faculty
                    12. Change Degree
                    13. Change ISEE
                    """;

                string prompt2 =
                    """
                    1. COMPUTER_SCIENCE,
                    2. BUSINESS_AND_MANAGEMENT,
                    3. MATHEMATICS,
                    4. PSYCHOLOGY,
                    5. LAW,
                    6. FASHION_DESIGN,
                    7. NURSING,
                    8. LANGUAGES,
                    9. BIOLOGY
                    """;

                if (students?.Count == 0)
                {
                    Console.WriteLine("There are no students saved.\n");
                }
                else
                {

                    Console.WriteLine("Insert the ID Matricola of the student");
                    string matricola = Console.ReadLine();

                    while (loop)
                    {
                        foreach (char s in matricola)
                            if (!char.IsDigit(s))
                                loop = false;

                        if (matricola.Length == 7 && loop)
                        {
                            loop = false;
                        }
                        else
                        {
                            Console.WriteLine("Invalid input - the ID must be 7 numbers"); //not sure
                            Console.Clear();
                            Console.WriteLine("Insert the ID Matricola of the student again");
                            matricola = Console.ReadLine();
                            loop = true;
                        }
                    }

                    do
                    {
                        loop = true;
                        Student stud = students!.First(stud => stud.Matricola == matricola);
                        Console.WriteLine($"Changing student {stud?.Matricola} infos");
                        Console.WriteLine(separator);
                        Console.WriteLine(prompt);
                        Console.WriteLine("Which info do you want to change? (press a number)");
                      
                        n = Console.ReadKey().KeyChar.ToString();

                        switch (n)
                        {
                            case "1":

                                Console.WriteLine("Insert new name ");
                                stud.FullName = Console.ReadLine();
                                while (stud.FullName == null)
                                {
                                    Console.WriteLine("Invalid input - the name must be a string");
                                    stud.FullName = Console.ReadLine();
                                }
                                break;

                            case "2":
                                
                                stud.Gender = GetValidInput("Insert new gender (Male / Female / X)", input => input?.ToUpper() == "MALE" || input?.ToUpper() == "FEMALE" || input?.ToUpper() == "X");
                                break;

                            case "3":
                                Console.WriteLine("Insert new address ");
                                stud.Address = Console.ReadLine();
                                break;

                            case "4":
                                Console.WriteLine("Insert new email ");
                                stud.Email = Console.ReadLine();
                                break;

                            case "5":
                                while (loop)
                                {
                                    Console.WriteLine("Insert new phone ");
                                    string phone = Console.ReadLine();

                                    for (int i = 0; i < phone?.Length; i++)
                                    {
                                        if (!Char.IsDigit(phone[i]))
                                        {   
                                            if( !(i == 0 && phone[i]=='+') )
                                                loop = false;
                                        }
                                    }

                                    if (phone?.Length == 11 && loop)
                                    {
                                        stud.Phone = phone;
                                        loop = false;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid input - the phone number must be + and 10 numbers");
                                        loop = true;
                                    }
                                }

                                break;

                            case "6":
                                Console.WriteLine("Insert new birth year ");
                                stud.BirthYear = DateTime.Parse(Console.ReadLine()); // not sure, devo specificare il formato della data???
                                break;

                            case "7":
                                Console.WriteLine("Insert new status ");
                                string maritalStatus = GetMaritialStatus();
                                stud.MaritalStatus = (Status)int.Parse(maritalStatus);
                                break;

                            case "8":
                               
                                Console.WriteLine($"Change Full Time: from {stud.IsFullTime} to {!stud.IsFullTime} ? (y / n)");
                                string answer = Console.ReadKey().KeyChar.ToString();
                                if (answer.Equals("y"))
                                {
                                    stud.IsFullTime = !(stud.IsFullTime); //dovrebbe andare ma ho i miei dubbi
                                }
                                break;

                            case "9":
                                Console.WriteLine("Insert new registration year ");
                                stud.RegistrationYear = DateTime.Parse(Console.ReadLine());
                                break;

                            case "10":
                                string faculty = GetFaculty();
                                stud.Faculty = (Faculties)int.Parse(faculty); // new faculty

                                break;

                            case "11":
                                string degree = GetDegree();
                                stud.Degree = (Degrees)int.Parse(degree);
                                break;

                            case "12":
                                while (loop)
                                {
                                    Console.WriteLine("Insert new ISEE ");
                                    decimal isee = Convert.ToDecimal(Console.ReadLine());

                                    if (isee > 0)
                                    {
                                        stud.ISEE = isee;
                                        loop = false;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid input - the ISEE must be a positive number.");
                                    }
                                }
                                break;

                            default:
                                Console.WriteLine("Invalid input.");
                                break;
                        }

                        Console.WriteLine(separator);
                        Console.WriteLine("Do you want to change something else about this student? (yes / no)");
                        char answerFinal = Console.ReadKey().KeyChar;

                        if (answerFinal  == 'n')
                        {
                            ExportJson(students);
                            Console.WriteLine("Update saved successfully");
                            doWhile = false;
                        }
                    } while (doWhile);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine();
            }
        }

        public static void UpdateEmployee()
        {
            try
            {
                List<Employee> employees = ImportFromJson<Employee>();
                bool loop = true;
                bool doWhile = true;
                string n;
                string prompt =
                    """
                    1.  Change Full Name
                    2.  Change Gender
                    3.  Change Address
                    4.  Change Email
                    5.  Change Phone
                    6.  Change Birth Year
                    7.  Change Status
                    8.  Change Full Time
                    9.  Change Faculty
                    10. Change Role
                    11. Change HiringYear
                    12. Change Salary
                    """;

                string prompt2 =
                    """
                    1. COMPUTER_SCIENCE,
                    2. BUSINESS_AND_MANAGEMENT,
                    3. MATHEMATICS,
                    4. PSYCHOLOGY,
                    5. LAW,
                    6. FASHION_DESIGN,
                    7. NURSING,
                    8. LANGUAGES,
                    9. BIOLOGY
                    """;

                if (employees.Count == 0)
                {
                    Console.WriteLine("There are no employees saved.\n");
                }
                else
                {

                    Console.WriteLine("Insert the ID of the employee");
                    string? id = Console.ReadLine();

                    while (loop)
                    {
                        foreach (char s in id)
                            
                        if (id?.Length == 36)
                        {
                            loop = false;
                        }
                        else
                        {
                            Console.WriteLine("Invalid input - the ID must be 36 alphanumeric characters."); //not sure
                            Console.Clear();
                            Console.WriteLine("Insert the ID of the employee again");
                            id = Console.ReadLine();
                            loop = true;
                        }
                    }

                    do
                    {
                        loop = true;
                        Employee? emp = employees.First(emp => emp.Id == Guid.Parse(id!));
                        Console.WriteLine($"Changing employee {emp.FullName} infos");
                        Console.WriteLine(separator);
                        Console.WriteLine(prompt);
                        Console.WriteLine("Which info do you want to change? (press a number)");
                        n = Console.ReadKey().KeyChar.ToString();

                        switch (n)
                        {
                            case "1":
                                Console.WriteLine("Insert new name ");
                                emp.FullName = Console.ReadLine();

                                break;

                            case "2":
                                Console.WriteLine("Insert new gender ");
                                emp.Gender = Console.ReadLine();
                                break;

                            case "3":
                                Console.WriteLine("Insert new address ");
                                emp.Address = Console.ReadLine();
                                break;

                            case "4":
                                Console.WriteLine("Insert new email ");
                                emp.Email = Console.ReadLine();
                                break;

                            case "5":
                                while (loop)
                                {
                                    Console.WriteLine("Insert new phone ");
                                    string phone = Console.ReadLine();

                                    for (int i = 0; i < phone.Length; i++)
                                    {
                                        if (!Char.IsDigit(phone[i]))
                                        {
                                            if (!(i == 0 && phone[i] == '+'))
                                                loop = false;
                                        }
                                    }

                                    if (phone.Length == 10 && loop)
                                    {
                                        emp.Phone = phone;
                                        loop = false;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid input - the phone number must be + 10 numbers");
                                        loop = true;
                                    }
                                }

                                break;

                            case "6":
                                Console.WriteLine("Insert new birth year ");
                                emp.BirthYear = DateTime.Parse(Console.ReadLine()); // not sure, devo specificare il formato della data???
                                break;

                            case "7":
                                string maritalStatus = GetMaritialStatus();
                                emp.MaritalStatus = (Status)int.Parse(maritalStatus);
                                break;

                            case "8":
                                Console.WriteLine($"Change Full Time: from {emp.IsFullTime} to {!emp.IsFullTime} ? (y / n)");
                                char answer = Console.ReadKey().KeyChar;
                                if (answer == 'y')
                                {
                                    emp.IsFullTime = !(emp.IsFullTime); //dovrebbe andare ma ho i miei dubbi
                                }
                                break;

                            case "9":
                                Console.WriteLine("Insert new Faculty (insert number)"); //hope
                                string faculty1 = GetValidInput(prompt2, input => int.Parse(input!) > 0 && int.Parse(input!) < 10);
                                var newFacultyVar = (Faculties)Enum.Parse(typeof(BLogic.Faculties), faculty1!.ToUpper()); // new faculty
                                emp.Faculty = newFacultyVar; //change faculty
                                break;

                            case "10":
                                Console.WriteLine("Insert new degree (insert number)");
                                string role = GetRole();
                                emp.Role = (Roles)int.Parse(role);

                                break;

                            case "11":
                                Console.WriteLine("Insert new hiring year ");
                                emp.HiringYear = DateTime.Parse(Console.ReadLine());
                                break;

                            case "12":
                                while (loop)
                                {
                                    Console.WriteLine("Insert new salary ");
                                    decimal salary = Convert.ToDecimal(Console.ReadLine());

                                    if (salary > 0)
                                    {
                                        emp.Salary = salary;
                                        loop = false;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid input - the ISEE must be a positive number.");
                                    }
                                }
                                break;

                            default:
                                Console.WriteLine("Invalid input.");
                                break;
                        }

                        Console.WriteLine(separator);
                        Console.WriteLine("Do you want to change something else about this employee? (y / n)");
                        string answerFinal = Console.ReadKey().KeyChar.ToString();

                        if (answerFinal.Equals("n"))
                        {
                            ExportJson<Employee>(employees);
                            Console.WriteLine("Update saved successfully");
                            doWhile = false;
                        }
                    } while (doWhile);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine();
            }
        }
        
        public static void UpdateExams()
        {
            try
            {
                List<Exam>? exams = ImportFromJson<Exam>();

                Console.Clear();

                if (exams.Count == 0)
                {
                    Console.WriteLine("There are no exams.\n");
                    return;
                }
                else
                {

                    Console.WriteLine("Insert the ID of the professor (UUID format: xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx)");
                    string id = Console.ReadLine();

                    // Validate UUID and check length
                    while (!Guid.TryParse(id, out Guid guid) || id.Length != 36)
                    {
                        Console.WriteLine("Invalid input - the ID must be in UUID format (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx) and 36 characters long.");
                        Console.WriteLine("Insert the ID of the professor again:");
                        id = Console.ReadLine();
                    }

                    if (!string.IsNullOrWhiteSpace(id)) //se è null, vuota o ha solo spazi bianchi!
                    {
                        Exam? exam = exams.Find(exam => exam.Id == Guid.Parse(id));

                        if (exam != null)
                        {
                            Console.WriteLine("Exam found! " +
                                $"What do you want to change about the exam {exam}?\n");
                            Console.WriteLine("Change the:\n");

                            Console.WriteLine("" +
                                "1 - Faculty\n" +
                                "2 - Name\n" +
                                "3 - CFU\n" +
                                "4 - Date\n" +
                                "5 - Modality (Online/On-site)\n" +
                                "6 - Number of participants\n" +
                                "7 - Exam type (Written, Oral, Written and Oral)\n" +
                                "8 - Project (is required or not)\n");

                            string? preChoice = Console.ReadLine();
                            int choice = int.Parse(preChoice);

                            switch (choice)
                            {
                                case 1:
                                    exam.Faculty = (Faculties) int.Parse(GetFaculty());
                                    break;

                                case 2:
                                    Console.WriteLine("You have chosen to upgrade the name of the exam\n");
                                    Console.WriteLine($"Currently, the exam name is: {exam.Name},\n" +
                                        "Enter the new name: ");
                                    string? newExamName = Console.ReadLine();
                                    if (newExamName != null)
                                    {
                                        exam.Name = newExamName;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid name!");
                                        return;
                                    }
                                    break;

                                case 3:
                                    Console.WriteLine("You have chosen to upgrade the number of CFU!\n");
                                    Console.WriteLine($"Currently, the exam number of CFU is: {exam.CFU},\n" +
                                        "Enter the new number of CFU: ");

                                    string? inputCfu = Console.ReadLine();
                                    int newExamCFU = int.Parse(inputCfu);

                                    if (newExamCFU > 0)
                                    {
                                        exam.CFU = newExamCFU;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid number of CFU!");
                                        return;
                                    }

                                    break;
                                case 4:
                                    Console.WriteLine("You have chosen to upgrade the date of the exam!\n");
                                    Console.WriteLine($"Currently, the exam date is: {exam.Date},\n" +
                                        "Enter the new date (with the format: YYYY/MM/DD ");

                                    string? inputExamDate = Console.ReadLine();

                                    if (DateTime.TryParseExact(inputExamDate, "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime newExamDate))
                                    {
                                        exam.Date = newExamDate;
                                        Console.WriteLine($"The exam date has been successfully updated to: {exam.Date}");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid date format!");
                                        return; //or loop until the correct date?
                                    }
                                    break;

                                case 5:
                                    Console.WriteLine("You have chosen to upgrade the modality of the exam!\n");

                                    Console.WriteLine($"Currently, the modality of the exam is: {(exam.IsOnline ? "Online" : "On-Site")}\n" +
                                        "Please enter 1 if you want to change the exam online flag,\n" +
                                        "Enter 0 otherwise!");
                                    string? inputModality = Console.ReadLine();
                                    bool changeModalityExam = bool.Parse(inputModality);

                                    if (changeModalityExam)
                                    {
                                        exam.IsOnline = !exam.IsOnline;
                                    }
                                    else
                                    {
                                        return;
                                    }

                                    break;

                                case 6:
                                    Console.WriteLine("You have chosen to upgrade the number of participants of the exam!\n");
                                    Console.WriteLine($"Currently, the number of participants is: {exam.Participants}\n" +
                                            "Please enter the new number of participants!");
                                    string? inputParticipants = Console.ReadLine();
                                    int newParticipants = int.Parse(inputParticipants);

                                    exam.Participants = newParticipants;
                                    break;

                                case 7:
                                    exam.ExamType = (ExamType) int.Parse(GetExamType());
                                    break;
                                case 8:
                                    Console.WriteLine("You have chosen to upgrade the project request for the exam!\n");

                                    Console.WriteLine($"Currently, the project is {(exam.IsProjectRequired ? "required" : "not required")}!\n" +
                                        "Please enter 1 if you want to change the required flag,\n " +
                                        "Enter 0 otherwise!");
                                    string? inputRequired = Console.ReadLine();
                                    bool changeModalityProject = bool.Parse(inputRequired);

                                    if (changeModalityProject)
                                    {
                                        exam.IsProjectRequired = !exam.IsProjectRequired;
                                    }
                                    else
                                    {
                                        return;
                                    }

                                    break;
                                default:
                                    Console.WriteLine("Invalid choice!");
                                    break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Exam not found.");
                            return;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid input.");
                    }
                }

                Console.WriteLine(ConfigurationManager.AppSettings["SeparationLine"]);
                ExportJson(exams);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
       
        #endregion

        #region Delete
        public static void DeleteProfessor()
        {
            try
            {
                List<Professor> professors = ImportFromJson<Professor>(); // Importa i dati esistenti

                if (professors.Count == 0)
                {
                    Console.WriteLine("There are no professors saved.\n");
                    return;
                }

                Console.WriteLine("Insert the ID of the professor you want to delete (UUID format: xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx):");
                string id = Console.ReadLine();

                // Verifica che l'ID inserito sia un UUID valido
                while (!Guid.TryParse(id, out Guid guid))
                {
                    Console.WriteLine("Invalid input - the ID must be in UUID format (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx).");
                    Console.WriteLine("Insert the ID of the professor again:");
                    id = Console.ReadLine();
                }

                // Trova il professore con l'ID specificato
                Professor? prof = professors.Find(p => p.Id ==  Guid.Parse(id));
                if (prof == null)
                {
                    Console.WriteLine("Professor not found.");
                    return;
                }

                // Rimuovi il professore dalla lista
                professors.Remove(prof);
                Console.WriteLine($"Professor {prof.FullName} has been removed successfully.");
                Console.WriteLine(professors.FirstOrDefault(p => p.Id == Guid.Parse(id)) == null);
                // Salvataggio delle modifiche nel file JSON usando ExportJson
                ExportJson(professors); // Usa ExportJson per salvare

                Console.WriteLine("Changes saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public static void DeleteStudents()
        {
            bool loop = true;
            try
            {
                while (loop)
                {
                    List<Student> students = ImportFromJson<Student>();
                    if (students.Count == 0)
                    {
                        Console.WriteLine("There are no students saved");
                    }
                    else
                    {
                        Console.WriteLine("Insert the ID Matricola of the student you want to delete");
                        string matricola = Console.ReadLine();

                        foreach (char s in matricola)
                            if (!Char.IsDigit(s))
                                loop = false;

                        if (matricola.Length == 7 && loop)
                        {
                            loop = false;
                        }
                        else
                        {
                            Console.WriteLine("Invalid input - the ID must be 8 numbers"); //not sure
                            Console.Clear();
                            Console.WriteLine("Insert the ID Matricola of the student you want to delete again");
                            matricola = Console.ReadLine();
                            loop = true;
                        }

                        Student? stud = students.Find(stud => stud.Matricola == matricola);
                        Console.WriteLine($"Are you sure you want to delete the student: {stud.FullName} ? ( y / n )");
                        char answer = Console.ReadKey().KeyChar;

                        if (answer == 'y')
                        {
                            students.Remove(stud);
                            ExportJson(students);
                            Console.WriteLine("Student removed successfully");
                            loop = false;
                        }
                    }
                }
            }
            catch ( Exception ex ) 
            { 
                    Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public static void DeleteEmployee()
        {
            bool loop = true;
            try
            {
                while (loop)
                {
                    List<Employee> employees = ImportFromJson<Employee>();
                    if (employees.Count == 0)
                    {
                        Console.WriteLine("There are no employee saved");
                    }
                    else
                    {
                        Console.WriteLine("Insert the ID of the employee you want to delete");
                        string id = Console.ReadLine();

                        foreach (char s in id)
                        {
                            if (id.Length == 36)
                            {
                                loop = false;
                            }
                            else
                            {
                                Console.WriteLine("Invalid input - the ID must be 36 alphanumeric characters."); //not sure
                                Console.Clear();
                                Console.WriteLine("Insert the ID of the employee again");
                                id = Console.ReadLine();
                                loop = true;
                            }
                        }

                        Employee? emp = employees.Find(emp => emp.Id.ToString() == id);
                        Console.WriteLine($"Are you sure you want to delete the employee: {emp.FullName} ? (yes/no)");
                        string answer = Console.ReadLine().ToLower();

                        if (answer.Equals("yes"))
                        {
                            employees.Remove(emp);
                            ExportJson(employees);
                            Console.WriteLine("Employee removed successfully");
                            loop = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public static void DeleteExams()
        {
            try
            {
                List<Exam> exams = ImportFromJson<Exam>();
                List<Professor> professors = ImportFromJson<Professor>();
        
                Console.Clear();
        
                if (exams.Count == 0)
                {
                    Console.WriteLine("There are no exams.\n");
                    return;
                }
                else
                {
                    Console.WriteLine("Insert the ID of the professor (UUID format: xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx)");
                    string id = Console.ReadLine();

                    // Validate UUID and check length
                    while (!Guid.TryParse(id, out Guid guid) || id.Length != 36)
                    {
                        Console.WriteLine("Invalid input - the ID must be in UUID format (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx) and 36 characters long.");
                        Console.WriteLine("Insert the ID of the professor again:");
                        id = Console.ReadLine();
                    }
                    if (!string.IsNullOrWhiteSpace(id))
                    {
                        Exam? exam = exams.First(e => e.Id == Guid.Parse(id));
        
                        if (exam != null)
                        {
                            exams.Remove(exam); //Deletes the exam from the exam list
                            ExportJson(exams);
        
                            foreach (Professor professor in professors)
                            {
                                professor.Exams.RemoveAll(e => e.Id == exam.Id); //Deletes the exam from the professor's exam list
                            }
        
                            ExportJson(professors);
        
                            Console.WriteLine($"The exam '{exam.Name}' has been successfully deleted.");
        
                            exam = null;
                        }
                        else
                        {
                            Console.WriteLine("The exam was not found.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        #endregion

        internal static List<T> ImportFromJson<T>()
        {
            try
            {
                string Json = string.Empty;

                if (typeof(T) == typeof(Student))
                {
                     Json = File.ReadAllText(ConfigurationManager.AppSettings["FileStudentsJson"]);
                }
                else if (typeof(T) == typeof(Professor))
                {
                     Json = File.ReadAllText(ConfigurationManager.AppSettings["FileProfessorsJson"]);
                }
                else if (typeof(T) == typeof(Exam))
                {
                     Json = File.ReadAllText(ConfigurationManager.AppSettings["FileExamsJson"]);
                }
                else if (typeof(T) == typeof(Courses))
                {
                     Json = File.ReadAllText(ConfigurationManager.AppSettings["FileCoursesJson"]);
                }
                else if (typeof(T) == typeof(Faculty))
                {
                     Json = File.ReadAllText(ConfigurationManager.AppSettings["FileFacultiesJson"]);
                } 
                else if(typeof(T) == typeof(Employee))
                {
                    Json = File.ReadAllText(ConfigurationManager.AppSettings["FileEmployeeJson"]);
                }

                return JsonSerializer.Deserialize<List<T>>(Json) ?? [];
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
    
                List<T> dataToAdd = list.OfType<T>().ToList();
                string json = JsonSerializer.Serialize(dataToAdd, options);

                if(typeof(T) == typeof(Student)){
                    File.WriteAllText(ConfigurationManager.AppSettings["FileStudentsJson"], json);
                    
                } else if (typeof(T) == typeof(Professor))
                {
                    File.WriteAllText(ConfigurationManager.AppSettings["FileProfessorsJson"], json);
                }
                else if(typeof(T) == typeof(Exam))
                {
                    File.WriteAllText(ConfigurationManager.AppSettings["FileExamsJson"], json);
                }
                else if (typeof(T) == typeof(Courses))
                {
                    File.WriteAllText(ConfigurationManager.AppSettings["FileCoursesJson"], json);
                }
                else if (typeof(T) == typeof(Faculty))
                {
                    File.WriteAllText(ConfigurationManager.AppSettings["FileFacultiesJson"], json);
                } 
                else if (typeof(T) == typeof(Employee))
                {
                    File.WriteAllText(ConfigurationManager.AppSettings["FileEmployeeJson"], json);
                }

            return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Error: {ex.StackTrace}");
                return false;
            }
        }

    }
}