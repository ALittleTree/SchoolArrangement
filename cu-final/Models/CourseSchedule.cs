using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Models
{
    public class CourseSchedule
    {
        public int CourseScheduleID { get; set; }
        public Guid CourseGuid{ get; set; }
        [Required]
        public int CourseID { get; set; }
        [Required]
        public int InstructorID { get; set; }
        public int StudentID { get; set; }
        public bool IsAskForLeave { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "上课日期")]
        [Required]
        public DateTime ScheduleDate { get; set; }
    }
}
