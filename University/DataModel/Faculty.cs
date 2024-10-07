using University.BLogic;

namespace University.DataModel
{
    internal class University 
    {
        public required string Name { get; set; }
        public required string Address { get; set; }

        public required List<Faculty> Faculties { get; set; } = [];
    }
    internal class Faculty
    {
        public required Faculties Name { get; set; }
        public required string Address { get; set; }
        public required int StudentsNumber { get; set; }

        public required int LabsNumber { get; set; }
        public required bool HasLibrary { get; set; }
        public required bool HasCanteen { get; set; }
        public required List<Exam> Exams { get; set; }
        public required List<Courses> Courses { get; set; } 

        // lista di oggetto esami

    }

    internal class Exam
    {
        public required Faculty Faculty { get; set; }
        public required string Name { get; set; }
        public required int CFU { get; set; }
        public required DateTime Date { get; set; }
        public required bool IsOnline { get; set; }
        public required int Partecipants {  get; set; }
        public required ExamType Type { get; set; }
        public required bool IsProjectRequired { get; set; }

    }

    // TOGLIERE E AGGIUNGERE CORSI 
    internal class Courses
    {
        public required Faculty Faculty { get; set; }
        public required string Name { get; set; }
        public required int CFU { get; set; }
        public required bool IsOnline { get; set; }
        public required Classroom Classroom { get; set; }


    }
}
  