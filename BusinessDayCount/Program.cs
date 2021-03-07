using System;
using System.Collections.Generic;

namespace BusinessDayCount
{
    class Program
    {
        static void Main(string[] args)
        {
            var businessDayCounter = new BusinessDayCounter();
            var date1 = new DateTime(2013, 10, 7);
            var date2 = new DateTime(2014, 1, 1);
            var publicHolidays = new List<DateTime>();
            publicHolidays.Add(new DateTime(2013, 12, 25));
            publicHolidays.Add(new DateTime(2013, 12, 26));
            publicHolidays.Add(new DateTime(2014, 1, 1));
            var weekdays = businessDayCounter.WeekdaysBetweenTwoDates(date1, date2);
            var businessDays = businessDayCounter.BusinessDaysBetweenTwoDates(date1, date2, publicHolidays);
            var businessDaysByRules = businessDayCounter.BusinessDaysBetweenTwoDatesAustralia(date1, date2);
            Console.WriteLine("Weekdays: {0}", weekdays);
            Console.WriteLine("Business Days: {0}", businessDays);
            Console.WriteLine("Business Days By Rules: {0}", businessDaysByRules);
            Console.ReadKey();
        }
    }
}
