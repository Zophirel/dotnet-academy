using University.BLogic;

namespace University.DataModel
{
    public class UniModel
    {
        public string Name { get; set; }
        public string Address { get; set; }

        public List<Faculty> Faculties { get; set; } = [];

        public UniModel(string name, string address)
        {
            Name = name;
            Address = address;
        }
        
    }
    public class Faculty
    {
        public Faculties Name { get; set; }
        public string Address { get; set; }
        public int StudentsNumber { get; set; }

        public int LabsNumber { get; set; }
        public bool HasLibrary { get; set; }
        public bool HasCanteen { get; set; }
        public List<Exam> Exams { get; set; }
        public List<Courses> Courses { get; set; } 

        // lista di oggetto esami

    }

    public class Exam
    {
        public Faculty Faculty { get; set; }
        public string Name { get; set; }
        public int CFU { get; set; }
        public DateTime Date { get; set; }
        public bool IsOnline { get; set; }
        public int Partecipants {  get; set; }
        public ExamType Type { get; set; }
        public bool IsProject { get; set; }

    }

    // TOGLIERE E AGGIUNGERE CORSI 
    public class Courses
    {
        public Faculty Faculty { get; set; }
        public string Name { get; set; }
        public int CFU { get; set; }
        public bool IsOnline { get; set; }
        public Classroom Classroom { get; set; }


    }
}
  