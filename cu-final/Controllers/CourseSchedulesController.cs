using ContosoUniversity.Data;
using ContosoUniversity.Models;
using ContosoUniversity.Models.SchoolViewModels;
using ContosoUniversity.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoUniversity.Controllers
{
    public class CourseSchedulesController : Controller
    {
        private readonly SchoolContext _context;

        public CourseSchedulesController(SchoolContext context)
        {
            _context = context;
        }

        // GET: CourseSchedules
        public async Task<IActionResult> Index()
        {
            var endDate = CommonHelper.GetTheEndDayOfThisWeek();
            var starDate = CommonHelper.GetTheStartDayOfThisWeek();
            ViewData["starDate"] = starDate.Date.ToString();
            ViewData["endDate"] = endDate.Date.ToString();
            var allSchedule = await _context.CourseSchedule.ToListAsync();
            var allScheduleVM = (from s in allSchedule
                                 from i in _context.Instructors
                                 from c in _context.Courses
                                 where s.CourseID == c.CourseID && s.InstructorID == i.ID
                                 && s.ScheduleDate >= starDate.Date && s.ScheduleDate <= endDate.Date
                                 select new CourseScheduleVM
                                 {
                                     CourseScheduleID = s.CourseScheduleID,
                                     CourseGuid = s.CourseGuid,
                                     CourseId = c.CourseID,
                                     CourseName = c.Title,
                                     InstructorId = i.ID,
                                     InstructorName = i.Name,
                                     ScheduleDate = s.ScheduleDate
                                 }).ToList().Distinct(new CourseScheduleVMCompare()).OrderBy(c => c.ScheduleDate).ToList();
            return View(allScheduleVM);
        }

        // GET: CourseSchedules/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseSchedule = await _context.CourseSchedule
                .SingleOrDefaultAsync(m => m.CourseScheduleID == id);
            if (courseSchedule == null)
            {
                return NotFound();
            }

            return View(courseSchedule);
        }

        // GET: CourseSchedules/Create
        public IActionResult Create()
        {
            var endDate = CommonHelper.GetTheEndDayOfThisWeek();
            var starDate = CommonHelper.GetTheStartDayOfThisWeek();
            ViewData["starDate"] = starDate.Date.ToString("yyyy/MM/dd");
            ViewData["endDate"] = endDate.Date.ToString("yyyy/MM/dd");
            return View();
        }
        [HttpPost]
        public JsonResult GetCourseDropDownList()
        {
            var allCourses = _context.Courses.ToList();
            var courseDropDown = allCourses.Select(a => new DropDownListVM
            {
                Key = a.CourseID.ToString(),
                Value = a.Title
            }).ToList();
            return Json(courseDropDown);
        }

        [HttpPost]
        public JsonResult GetInstructorByCourseID(int CourseID)
        {
            var allInstructor = _context.CourseAssignments.Where(c => c.CourseID == CourseID).Include(c => c.Instructor).Select(c => new DropDownListVM
            {
                Key = c.InstructorID.ToString(),
                Value = c.Instructor.Name
            }).ToList().Distinct(new DropDownListVMComparer()).ToList();
            var allStudent = _context.Enrollments.Where(c => c.CourseID == CourseID).Include(c => c.Student).Select(c => new DropDownListVM
            {
                Key = c.StudentID.ToString(),
                Value = c.Student.Name
            }).ToList().Distinct(new DropDownListVMComparer()).ToList();
            return Json(new
            {
                allInstructor,
                allStudent
            });
        }

        [HttpPost]
        public JsonResult GetInstructorAndStudentByCourseID(string courseGuidString, int courseID)
        {
            var courseGuid = new Guid(courseGuidString);
            var courseIDByGuid = _context.CourseSchedule.FirstOrDefault(s => s.CourseGuid == courseGuid)?.CourseID;
            if (courseIDByGuid.HasValue && courseID == courseIDByGuid)
            {
                var allInstructor = _context.CourseAssignments.Where(c => c.CourseID == courseIDByGuid).Include(c => c.Instructor).Select(c => new DropDownListVM
                {
                    Key = c.InstructorID.ToString(),
                    Value = c.Instructor.Name
                }).ToList().Distinct(new DropDownListVMComparer()).ToList();
                var allStudent = (from c in _context.CourseSchedule
                                  from s in _context.Students
                                  where c.CourseGuid == courseGuid && c.StudentID == s.ID
                                  select new CheckBoxListVM
                                  {
                                      Key = s.ID.ToString(),
                                      Value = s.Name,
                                      IsChecked = c.IsAskForLeave
                                  }).ToList();
                return Json(new
                {
                    allInstructor,
                    allStudent
                });
            }
            else
            {
                var allInstructor = _context.CourseAssignments.Where(c => c.CourseID == courseID).Include(c => c.Instructor).Select(c => new DropDownListVM
                {
                    Key = c.InstructorID.ToString(),
                    Value = c.Instructor.Name
                }).ToList().Distinct(new DropDownListVMComparer()).ToList();
                var allStudent = _context.Enrollments.Where(c => c.CourseID == courseID).Include(c => c.Student).Select(c => new DropDownListVM
                {
                    Key = c.StudentID.ToString(),
                    Value = c.Student.Name
                }).ToList().Distinct(new DropDownListVMComparer()).ToList();
                return Json(new
                {
                    allInstructor,
                    allStudent
                });
            }
        }

        // POST: CourseSchedules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseScheduleID,CourseID,InstructorID,StudentID,IsAskForLeave,ScheduleDate")] CourseSchedule courseSchedule, string[] leaveStrudentId)
        {
            if (ModelState.IsValid)
            {
                var newGuid = Guid.NewGuid();
                var allCourseSchedule = _context.Enrollments.Where(c => c.CourseID == courseSchedule.CourseID).Select(c => new CourseSchedule
                {
                    CourseID = courseSchedule.CourseID,
                    CourseGuid = newGuid,
                    InstructorID = courseSchedule.InstructorID,
                    ScheduleDate = courseSchedule.ScheduleDate,
                    IsAskForLeave = leaveStrudentId.Contains(c.StudentID.ToString()),
                    StudentID = c.StudentID
                }).ToList();
                await _context.CourseSchedule.AddRangeAsync(allCourseSchedule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(courseSchedule);
        }

        // GET: CourseSchedules/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseSchedule = await _context.CourseSchedule.SingleOrDefaultAsync(m => m.CourseScheduleID == id);
            if (courseSchedule == null)
            {
                return NotFound();
            }
            return View(courseSchedule);
        }

        // POST: CourseSchedules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CourseScheduleID,CourseGuid,CourseID,InstructorID,StudentID,IsAskForLeave,ScheduleDate")] CourseSchedule courseSchedule, string[] leaveStrudentId)
        {
            if (id != courseSchedule.CourseScheduleID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var allCourseSchedule = _context.CourseSchedule.Where(c => c.CourseGuid == courseSchedule.CourseGuid).ToList();
                    var allStudent = _context.Enrollments.Include(e => e.Student).Where(e => e.CourseID == courseSchedule.CourseID).Select(e => e.Student).Select(s => new CourseSchedule
                    {
                        CourseID = courseSchedule.CourseID,
                        CourseGuid = courseSchedule.CourseGuid,
                        InstructorID = courseSchedule.InstructorID,
                        ScheduleDate = courseSchedule.ScheduleDate,
                        StudentID = s.ID,
                        IsAskForLeave = leaveStrudentId.Contains(s.ID.ToString())
                    });
                    _context.CourseSchedule.RemoveRange(allCourseSchedule);
                    await _context.CourseSchedule.AddRangeAsync(allStudent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseScheduleExists(courseSchedule.CourseScheduleID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(courseSchedule);
        }

        // GET: CourseSchedules/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseSchedule = await _context.CourseSchedule
                .SingleOrDefaultAsync(m => m.CourseScheduleID == id);
            if (courseSchedule == null)
            {
                return NotFound();
            }

            return View(courseSchedule);
        }

        // POST: CourseSchedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var courseSchedule = await _context.CourseSchedule.SingleOrDefaultAsync(m => m.CourseScheduleID == id);
            var allCourseSchedule = _context.CourseSchedule.Where(c => c.CourseGuid == courseSchedule.CourseGuid).ToList();
            _context.CourseSchedule.RemoveRange(allCourseSchedule);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> CopyfromLastWeek()
        {
            var endDate = CommonHelper.GetTheStartDayOfThisWeek().AddDays(-1);
            var starDate = endDate.AddDays(-6);
            var allSchedule = await _context.CourseSchedule.Where(s => s.ScheduleDate >= starDate.Date && s.ScheduleDate <= endDate.Date).OrderBy(s => s.CourseGuid).ToListAsync();
            var newSchedule = new List<CourseSchedule>();
            var newGuid = new Guid();
            var lastGuid = new Guid();
            foreach (var item in allSchedule)
            {
                if (item.CourseGuid != lastGuid)
                {
                    newGuid = Guid.NewGuid();
                    lastGuid = item.CourseGuid;
                }
                newSchedule.Add(new CourseSchedule
                {
                    CourseGuid = newGuid,
                    CourseID = item.CourseID,
                    InstructorID = item.InstructorID,
                    IsAskForLeave = false,
                    StudentID = item.StudentID,
                    ScheduleDate = item.ScheduleDate.AddDays(7)
                });
            }
            if (newSchedule.Count > 0)
            {
                await _context.CourseSchedule.AddRangeAsync(newSchedule);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public JsonResult GetCourseByStudentId(int studentID, DateTime startDate, DateTime endDate)
        {
            endDate = endDate == new DateTime() ? DateTime.MaxValue : endDate;
            var allCourse = (from cs in _context.CourseSchedule
                             from c in _context.Courses
                             where cs.StudentID == studentID && cs.CourseID == c.CourseID
                             && (cs.ScheduleDate >= startDate && cs.ScheduleDate <= endDate)
                             select new
                             {
                                 courseName = c.Title,
                                 scheduleDate = cs.ScheduleDate.ToString("yyyy-MM-dd"),
                                 isAskForLeave = cs.IsAskForLeave ? "是" : "否"
                             }).OrderBy(c => c.scheduleDate).ToList();
            return Json(allCourse);
        }

        [HttpPost]
        public JsonResult GetCourseByInstructorId(int InstructorID, DateTime startDate, DateTime endDate)
        {
            endDate = endDate == new DateTime() ? DateTime.MaxValue : endDate;
            var allScheduleVM = (from s in _context.CourseSchedule
                                 from i in _context.Instructors
                                 from c in _context.Courses
                                 where s.CourseID == c.CourseID && s.InstructorID == i.ID && s.InstructorID == InstructorID
                                 && s.ScheduleDate >= startDate && s.ScheduleDate <= endDate
                                 select new CourseScheduleVM
                                 {
                                     CourseScheduleID = s.CourseScheduleID,
                                     CourseGuid = s.CourseGuid,
                                     CourseId = c.CourseID,
                                     CourseName = c.Title,
                                     InstructorId = i.ID,
                                     InstructorName = i.Name,
                                     ScheduleDate = s.ScheduleDate.Date
                                 }).ToList().Distinct(new CourseScheduleVMCompare()).OrderBy(c => c.ScheduleDate).ToList();
            return Json(allScheduleVM);
        }

        private bool CourseScheduleExists(int id)
        {
            return _context.CourseSchedule.Any(e => e.CourseScheduleID == id);
        }
    }
}
