using University.BLogic;

namespace University.DataModel
{
    internal abstract class People
    {
        public required Guid Id { get; set; } = Guid.NewGuid(); // id = Guid.New()

        public required string FullName { get; set; }
        public required string Gender { get; set; }

        public required string Address { get; set; }

        public required string Email { get; set; }

        public required string Phone { get; set; }

        public required DateTime BirthYear { get; set; }

        public required bool IsFullTime { get; set; }

        public required Status MaritalStatus { get; set; }
    }

    internal class Student : People
    {
        public required string Matricola { get; set; }
        public required DateTime RegistrationYear { get; set; }

        public required Degrees Degree { get; set; }

        public required decimal ISEE { get; set; }
        

        //da aggiungere oggetto Faculty

    }

    internal class Employee : People
    {
        public required Roles Role { get; set; }

        public required DateTime HiringYear { get; set; }
        
        public required Faculty Faculty { get; set; } //where the employee works
        
        public required decimal Salary { get; set; }

    }

    internal class Professor : Employee
    {
        
        public required List<Exam> Exams { get; set; }  = [];         
    }
}