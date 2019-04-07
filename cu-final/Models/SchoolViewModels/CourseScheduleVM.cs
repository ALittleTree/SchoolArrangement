using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.Models.SchoolViewModels
{
    public class CourseScheduleVM
    {
        public int CourseScheduleID { get; set; }
        public Guid CourseGuid { get; set; }
        public int CourseId { get; set; }
        [Display(Name = "课程名称")]
        public string CourseName { get; set; }
        public int InstructorId { get; set; }
        [Display(Name = "教师姓名")]
        public string InstructorName { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "上课日期")]
        public DateTime ScheduleDate { get; set; }
        public string DayOfWeek
        {
            get
            {
                return System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(ScheduleDate.DayOfWeek);
            }
        }
    }

    public class CourseScheduleVMCompare : IEqualityComparer<CourseScheduleVM>
    {
        public bool Equals(CourseScheduleVM x, CourseScheduleVM y)
        {
            return x.CourseGuid == y.CourseGuid;
        }

        public int GetHashCode(CourseScheduleVM obj)
        {
            return obj.CourseGuid.GetHashCode();
        }
    }
}
