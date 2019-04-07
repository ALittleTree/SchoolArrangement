using System;

namespace ContosoUniversity.Services
{
    public static class CommonHelper
    {
        public static int ConvertDateTimeToChinese(DayOfWeek dayOfWeek)
        {
            var dayOfWeekNum = (int)dayOfWeek;
            return dayOfWeek == 0 ? 6 : dayOfWeekNum - 1;
        }

        public static DateTime GetTheStartDayOfThisWeek()
        {
            return DateTime.Now.Date.AddDays(-ConvertDateTimeToChinese(DateTime.Now.DayOfWeek));
        }

        public static DateTime GetTheEndDayOfThisWeek()
        {
            return DateTime.Now.Date.AddDays(6 - ConvertDateTimeToChinese(DateTime.Now.DayOfWeek));
        }
    }
}
