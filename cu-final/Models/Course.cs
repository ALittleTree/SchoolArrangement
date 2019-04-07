using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Models
{
    public class Course
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "课程号")]
        public int CourseID { get; set; }

        [Display(Name = "课程名称")]
        public string Title { get; set; }

        [Range(0, 5)]
        [Display(Name ="学分")]
        public int Credits { get; set; }

        public int DepartmentID { get; set; }
        [Display(Name ="部门")]
        //public Department Department { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
        public ICollection<CourseAssignment> CourseAssignments { get; set; }
    }
}