using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessDayCount
{
    public class BusinessDayCounter
    {
        private IList<Func<DateTime, bool>> _publicHolidayRulesAu;

        public BusinessDayCounter()
        {
            InitialiseAuPublicHolidayRules();
        }

        public int WeekdaysBetweenTwoDates(DateTime firstDate, DateTime secondDate)
        {
            if (secondDate <= firstDate)
                return 0;

            var currentDay = firstDate.AddDays(1);
            int weekdayCount = 0;
            while (currentDay < secondDate)
            {
                if (IsWeekDay(currentDay))
                    weekdayCount++;
                currentDay = currentDay.AddDays(1);
            }
            return weekdayCount;
            
        }

        public int BusinessDaysBetweenTwoDates(DateTime firstDate, DateTime secondDate, IList<DateTime> publicHolidays)
        {
            if (secondDate <= firstDate)
                return 0;

            var businessDayCount = WeekdaysBetweenTwoDates(firstDate, secondDate);
            foreach(var publicHoliday in publicHolidays)
            {
                if(publicHoliday > firstDate && publicHoliday < secondDate)
                {
                    if (IsWeekDay(publicHoliday))
                        businessDayCount--;
                }
            }
            return businessDayCount;
        }

        public int BusinessDaysBetweenTwoDatesAustralia(DateTime firstDate, DateTime secondDate)
        {
            return BusinessDaysBetweenTwoDates(firstDate, secondDate, _publicHolidayRulesAu);
        }

        public int BusinessDaysBetweenTwoDates(DateTime firstDate, DateTime secondDate, IList<Func<DateTime, bool>> publicHolidayRules)
        {
            if (secondDate <= firstDate)
                return 0;

            var currentDay = firstDate.AddDays(1);
            int weekdayCount = 0;
            while (currentDay < secondDate)
            {
                if (IsWeekDay(currentDay) && !IsPublicHoliday(currentDay, publicHolidayRules))
                    weekdayCount++;
                currentDay = currentDay.AddDays(1);
            }
            return weekdayCount;
        }

        private bool IsWeekDay(DateTime date)
        {
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                return false;
            return true;
        }

        private bool IsPublicHoliday(DateTime date, IList<Func<DateTime, bool>> publicHolidayRules)
        {
            if (publicHolidayRules.Any())
            {
                foreach (var rule in publicHolidayRules)
                {
                    if (rule(date))
                        return true;
                }
            }

            return false;
        }

        private void InitialiseAuPublicHolidayRules()
        {
            _publicHolidayRulesAu = new List<Func<DateTime, bool>>();
            _publicHolidayRulesAu.Add(date =>
            {
                // New Year Day
                return date.Month == 1 && date.Day == 1;
            });
            _publicHolidayRulesAu.Add(date =>
            {
                // Additional Day when New Year Day falls on Sunday
                return date.Month == 1 && date.Day == 2 && date.DayOfWeek == DayOfWeek.Monday;
            });
            _publicHolidayRulesAu.Add(date =>
            {
                // Additional Day when New Year Day falls on Saturday
                return date.Month == 1 && date.Day == 3 && date.DayOfWeek == DayOfWeek.Monday;
            });
            _publicHolidayRulesAu.Add(date =>
            {
                // Australia Day
                return date.Month == 1 && date.Day == 26;
            });
            _publicHolidayRulesAu.Add(date =>
            {
                // Additional Day when Australia Day falls on Sunday
                return date.Month == 1 && date.Day == 27 && date.DayOfWeek == DayOfWeek.Monday;
            });
            _publicHolidayRulesAu.Add(date =>
            {
                // Additional Day when Australia Day falls on Saturday
                return date.Month == 1 && date.Day == 28 && date.DayOfWeek == DayOfWeek.Monday;
            });
            _publicHolidayRulesAu.Add(date =>
            {
                // Easter Friday
                return EasterHelper.IsEasterFriday(date);
            });
            _publicHolidayRulesAu.Add(date =>
            {
                // Easter Monday
                return EasterHelper.IsEasterMonday(date);
            });
            _publicHolidayRulesAu.Add(date =>
            {
                // Anzac Day
                return date.Month == 4 && date.Day == 25;
            });
            _publicHolidayRulesAu.Add(date =>
            {
                // Queen's Birthday
                return date.Month == 6 && date.DayOfWeek == DayOfWeek.Monday && date.AddDays(-7).Month == date.Month && date.AddDays(-14).Month != date.Month;
            });
            _publicHolidayRulesAu.Add(date =>
            {
                // Labour Day
                return date.Month == 10 && date.DayOfWeek == DayOfWeek.Monday && date.AddDays(-7).Month != date.Month;
            });
            _publicHolidayRulesAu.Add(date =>
            {
                // Xmas Day
                return date.Month == 12 && date.Day == 25;
            });
            _publicHolidayRulesAu.Add(date =>
            {
                // Boxing Day
                return date.Month == 12 && date.Day == 26;
            });
            _publicHolidayRulesAu.Add(date =>
            {
                // Additional Day when Xmas Day falls on Saturday
                return date.Month == 12 && date.Day == 27 && date.DayOfWeek == DayOfWeek.Monday;
            });
            _publicHolidayRulesAu.Add(date =>
            {
                // Additional Day when Xmas Day falls on Sunday
                return date.Month == 12 && date.Day == 27 && date.DayOfWeek == DayOfWeek.Tuesday;
            });
            _publicHolidayRulesAu.Add(date =>
            {
                // Additional Day when Boxing Day falls on Saturday
                return date.Month == 12 && date.Day == 28 && date.DayOfWeek == DayOfWeek.Monday;
            });
            _publicHolidayRulesAu.Add(date =>
            {
                // Additional Day when Boxing Day falls on Sunday
                return date.Month == 12 && date.Day == 28 && date.DayOfWeek == DayOfWeek.Tuesday;
            });
        }
    }
}
