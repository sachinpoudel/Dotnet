namespace ContosoUniversity.Models
{
    public class Student
    {
        public int ID {get;set;} // becomes pri key auto
        public string LastName {get;set;}
        public string FirstMidName {get;set;}
        public DateTime EnrollmentDate {get;set;}

        public ICollection<Enrollment> Enrollments {get;set;}
    }
}