using University.BLogic;

namespace University.DataModel
{
    public abstract class People
    {
        public Guid Id { get; set; } = Guid.NewGuid(); // id = Guid.New()

        public string FullName { get; set; }
        public string Gender { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public DateTime BirthYear { get; set; }

        public bool IsFullTime { get; set; }

        public Status MaritalStatus { get; set; }
    }

    public class Student : People
    {
        public string Matricola { get; set; }
        public DateTime RegistrationYear { get; set; }

        public Degrees Degree { get; set; }

        public decimal ISEE { get; set; }
        

        //da aggiungere oggetto Faculty

    }

    public class Employee : People
    {
        public Roles Role { get; set; }

        public DateTime HiringYear { get; set; }
        
        public Faculty? Faculty { get; set; } //where the employee works
        
        public decimal Salary { get; set; }

    }

    public class Professor : Employee
    {

        public List<Exam> Exams { get; set; } = [];
        public List<Courses> Courses { get; set; } = [];         
    }
}