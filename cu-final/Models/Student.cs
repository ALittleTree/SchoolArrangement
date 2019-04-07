using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Models
{
    public class Student : Person
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "入学时间")]
        public DateTime EnrollmentDate { get; set; }

        [Display(Name="课时")]
        [Required]
        public int EnrollmentCount { get; set; }

        [Display(Name = "所有课程")]
        public ICollection<Enrollment> Enrollments { get; set; }
    }
}