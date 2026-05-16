using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.Models
{
    public enum Grade
    {
        A , B ,C , D,F
    }

    public class Enrollment {
        public int EnrollmentID {get;set;} 
        public int CourseID {get;set;} // foreign key for course
        public  int StudentID {get;set;} // foreign key for student table or entity

        [DisplayFormat(NullDisplayText ="No grade")]

        public Grade? Grade {get;set;}

        public Course Course {get;set;}
        public Student Student {get;set;}
    }
}