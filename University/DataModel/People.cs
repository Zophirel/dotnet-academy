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
        public Faculties Faculty { get; set; } //where the student is enrolled
    }

    public class Student : People
    {
        public string Matricola { get; set; }
        public DateTime RegistrationYear { get; set; }
        public Degrees Degree { get; set; }
        public decimal ISEE { get; set; }
       
    }

    public class Employee : People
    {
        public Roles Role { get; set; }
        public DateTime HiringYear { get; set; }
        public decimal Salary { get; set; }
    }

    public class Professor : Employee
    {
        public List<Exam> Exams { get; set; } = [];
        public List<Courses> Courses { get; set; } = [];         
    }
}