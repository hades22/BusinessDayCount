using BusinessDayCount;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Tests
{
    public class BusinessDayCountTests
    {
        private IList<DateTime> _publicHolidays;
        private BusinessDayCounter _businessDayCounter;
        private IList<Func<DateTime, bool>> _publicHolidayRules;

        
        private void SetupBusinessDayCounter()
        {
            _businessDayCounter = new BusinessDayCounter();
        }

        private void SetupPublicHolidays()
        {
            _publicHolidays = new List<DateTime>();
            _publicHolidays.Add(new DateTime(2013, 12, 25));
            _publicHolidays.Add(new DateTime(2013, 12, 26));
            _publicHolidays.Add(new DateTime(2014, 1, 1));
        }

        private void SetupPublicHolidayRules()
        {
            _publicHolidayRules = new List<Func<DateTime, bool>>();
            _publicHolidayRules.Add(date =>
            {
                // New Year Day
                return date.Month == 1 && date.Day == 1;
            });
            _publicHolidayRules.Add(date =>
            {
                // Additional Day when New Year Day falls on Sunday
                return date.Month == 1 && date.Day == 2 && date.DayOfWeek == DayOfWeek.Monday;
            });
            _publicHolidayRules.Add(date =>
            {
                // Additional Day when New Year Day falls on Saturday
                return date.Month == 1 && date.Day == 3 && date.DayOfWeek == DayOfWeek.Monday;
            });
            _publicHolidayRules.Add(date =>
            {
                // Australia Day
                return date.Month == 1 && date.Day == 26;
            });
            _publicHolidayRules.Add(date =>
            {
                // Additional Day when Australia Day falls on Sunday
                return date.Month == 1 && date.Day == 27 && date.DayOfWeek == DayOfWeek.Monday;
            });
            _publicHolidayRules.Add(date =>
            {
                // Additional Day when Australia Day falls on Saturday
                return date.Month == 1 && date.Day == 28 && date.DayOfWeek == DayOfWeek.Monday;
            });
            _publicHolidayRules.Add(date =>
            {
                // Easter Friday
                return EasterHelper.IsEasterFriday(date);
            });
            _publicHolidayRules.Add(date =>
            {
                // Easter Monday
                return EasterHelper.IsEasterMonday(date);
            });
            _publicHolidayRules.Add(date =>
            {
                // Anzac Day
                return date.Month == 4 && date.Day == 25;
            });
            _publicHolidayRules.Add(date =>
            {
                // Queen's Birthday
                return date.Month == 6 && date.DayOfWeek == DayOfWeek.Monday && date.AddDays(-7).Month == date.Month && date.AddDays(-14).Month != date.Month;
            });
            _publicHolidayRules.Add(date =>
            {
                // Labour Day
                return date.Month == 10 && date.DayOfWeek == DayOfWeek.Monday && date.AddDays(-7).Month != date.Month;
            });
            _publicHolidayRules.Add(date =>
            {
                // Xmas Day
                return date.Month == 12 && date.Day == 25;
            });
            _publicHolidayRules.Add(date =>
            {
                // Boxing Day
                return date.Month == 12 && date.Day == 26;
            });
            _publicHolidayRules.Add(date =>
            {
                // Additional Day when Xmas Day falls on Saturday
                return date.Month == 12 && date.Day == 27 && date.DayOfWeek == DayOfWeek.Monday;
            });
            _publicHolidayRules.Add(date =>
            {
                // Additional Day when Xmas Day falls on Sunday
                return date.Month == 12 && date.Day == 27 && date.DayOfWeek == DayOfWeek.Tuesday;
            });
            _publicHolidayRules.Add(date =>
            {
                // Additional Day when Boxing Day falls on Saturday
                return date.Month == 12 && date.Day == 28 && date.DayOfWeek == DayOfWeek.Monday;
            });
            _publicHolidayRules.Add(date =>
            {
                // Additional Day when Boxing Day falls on Sunday
                return date.Month == 12 && date.Day == 28 && date.DayOfWeek == DayOfWeek.Tuesday;
            });
        }


        [SetUp]
        public void Setup()
        {
            SetupBusinessDayCounter();
            SetupPublicHolidays();
            SetupPublicHolidayRules();
        }

        [Test]
        public void WeekDayOneDayTest()
        {
            var date1 = new DateTime(2013, 10, 7);
            var date2 = new DateTime(2013, 10, 9);
            var numberOfWeekDays = _businessDayCounter.WeekdaysBetweenTwoDates(date1, date2);
            Assert.AreEqual(1, numberOfWeekDays);
        }

        [Test]
        public void WeekDayOneWeekTest()
        {
            var date1 = new DateTime(2013, 10, 5);
            var date2 = new DateTime(2013, 10, 14);
            var numberOfWeekDays = _businessDayCounter.WeekdaysBetweenTwoDates(date1, date2);
            Assert.AreEqual(5, numberOfWeekDays);
        }

        [Test]
        public void WeekDayMultipleWeeksTest()
        {
            var date1 = new DateTime(2013, 10, 7);
            var date2 = new DateTime(2014, 1, 1);
            var numberOfWeekDays = _businessDayCounter.WeekdaysBetweenTwoDates(date1, date2);
            Assert.AreEqual(61, numberOfWeekDays);
        }

        [Test]
        public void WeekDayInvalidDateTest()
        {
            var date1 = new DateTime(2013, 10, 7);
            var date2 = new DateTime(2013, 10, 5);
            var numberOfWeekDays = _businessDayCounter.WeekdaysBetweenTwoDates(date1, date2);
            Assert.AreEqual(0, numberOfWeekDays);
        }

        [Test]
        public void BusinessDayOneDayTest()
        {
            var date1 = new DateTime(2013, 10, 7);
            var date2 = new DateTime(2013, 10, 9);
            var numberOfBusinessDays = _businessDayCounter.BusinessDaysBetweenTwoDates(date1, date2, _publicHolidays);
            Assert.AreEqual(1, numberOfBusinessDays);
        }

        [Test]
        public void BusinessDayChristmasTest()
        {
            var date1 = new DateTime(2013, 12, 24);
            var date2 = new DateTime(2013, 12, 27);
            var numberOfBusinessDays = _businessDayCounter.BusinessDaysBetweenTwoDates(date1, date2, _publicHolidays);
            Assert.AreEqual(0, numberOfBusinessDays);
        }

        [Test]
        public void BusinessDayChristmasNewYearTest()
        {
            var date1 = new DateTime(2013, 10, 7);
            var date2 = new DateTime(2014, 1, 2);
            var numberOfBusinessDays = _businessDayCounter.BusinessDaysBetweenTwoDates(date1, date2, _publicHolidays);
            Assert.AreEqual(59, numberOfBusinessDays);
        }

        [Test]
        public void BusinessDayInvalidDateTest()
        {
            var date1 = new DateTime(2013, 10, 7);
            var date2 = new DateTime(2013, 10, 5);
            var numberOfBusinessDays = _businessDayCounter.BusinessDaysBetweenTwoDates(date1, date2, _publicHolidays);
            Assert.AreEqual(0, numberOfBusinessDays);
        }

        [Test]
        public void BusinessDayByRulesOneDayTest()
        {
            var date1 = new DateTime(2021, 3, 8);
            var date2 = new DateTime(2021, 3, 10);
            var numberOfBusinessDays = _businessDayCounter.BusinessDaysBetweenTwoDates(date1, date2, _publicHolidayRules);
            Assert.AreEqual(1, numberOfBusinessDays);
        }

        [Test]
        public void BusinessDayByRulesOneWeekTest()
        {
            var date1 = new DateTime(2021, 3, 7);
            var date2 = new DateTime(2021, 3, 14);
            var numberOfBusinessDays = _businessDayCounter.BusinessDaysBetweenTwoDates(date1, date2, _publicHolidayRules);
            Assert.AreEqual(5, numberOfBusinessDays);
        }

        [Test]
        public void BusinessDayByRulesXmasTest1()
        {
            // Test when Xmas and Boxing day fall on Sat and Sun
            var date1 = new DateTime(2021, 12, 24);
            var date2 = new DateTime(2021, 12, 29);
            var numberOfBusinessDays = _businessDayCounter.BusinessDaysBetweenTwoDates(date1, date2, _publicHolidayRules);
            Assert.AreEqual(0, numberOfBusinessDays);
        }

        [Test]
        public void BusinessDayByRulesXmasTest2()
        {
            // Test when Xmas day falls on Sun
            var date1 = new DateTime(2022, 12, 24);
            var date2 = new DateTime(2022, 12, 29);
            var numberOfBusinessDays = _businessDayCounter.BusinessDaysBetweenTwoDates(date1, date2, _publicHolidayRules);
            Assert.AreEqual(1, numberOfBusinessDays);
        }

        [Test]
        public void BusinessDayByRulesXmasTest3()
        {
            // Test when Xmas and Boxing day fall on weekdays
            var date1 = new DateTime(2023, 12, 24);
            var date2 = new DateTime(2023, 12, 29);
            var numberOfBusinessDays = _businessDayCounter.BusinessDaysBetweenTwoDates(date1, date2, _publicHolidayRules);
            Assert.AreEqual(2, numberOfBusinessDays);
        }

        [Test]
        public void BusinessDayByRulesXmasTest4()
        {
            // Test when Boxing day falls on Sat
            var date1 = new DateTime(2020, 12, 24);
            var date2 = new DateTime(2020, 12, 29);
            var numberOfBusinessDays = _businessDayCounter.BusinessDaysBetweenTwoDates(date1, date2, _publicHolidayRules);
            Assert.AreEqual(0, numberOfBusinessDays);
        }

        [Test]
        public void BusinessDayByRulesNewYearTest1()
        {
            // Test when New Year Day falls on weekday
            var date1 = new DateTime(2020, 12, 31);
            var date2 = new DateTime(2021, 1, 2);
            var numberOfBusinessDays = _businessDayCounter.BusinessDaysBetweenTwoDates(date1, date2, _publicHolidayRules);
            Assert.AreEqual(0, numberOfBusinessDays);
        }

        [Test]
        public void BusinessDayByRulesNewYearTest2()
        {
            // Test when New Year Day falls on Sat
            var date1 = new DateTime(2021, 12, 31);
            var date2 = new DateTime(2022, 1, 4);
            var numberOfBusinessDays = _businessDayCounter.BusinessDaysBetweenTwoDates(date1, date2, _publicHolidayRules);
            Assert.AreEqual(0, numberOfBusinessDays);
        }

        [Test]
        public void BusinessDayByRulesNewYearTest3()
        {
            // Test when New Year Day falls on Sun
            var date1 = new DateTime(2022, 12, 31);
            var date2 = new DateTime(2023, 1, 3);
            var numberOfBusinessDays = _businessDayCounter.BusinessDaysBetweenTwoDates(date1, date2, _publicHolidayRules);
            Assert.AreEqual(0, numberOfBusinessDays);
        }

        [Test]
        public void BusinessDayByRulesAuDayTest1()
        {
            // Test when Australia Day falls on weekday
            var date1 = new DateTime(2021, 1, 25);
            var date2 = new DateTime(2021, 1, 27);
            var numberOfBusinessDays = _businessDayCounter.BusinessDaysBetweenTwoDates(date1, date2, _publicHolidayRules);
            Assert.AreEqual(0, numberOfBusinessDays);
        }

        [Test]
        public void BusinessDayByRulesAuDayTest2()
        {
            // Test when Australia Day falls on Sat
            var date1 = new DateTime(2019, 1, 25);
            var date2 = new DateTime(2019, 1, 29);
            var numberOfBusinessDays = _businessDayCounter.BusinessDaysBetweenTwoDates(date1, date2, _publicHolidayRules);
            Assert.AreEqual(0, numberOfBusinessDays);
        }

        [Test]
        public void BusinessDayByRulesAuDayTest3()
        {
            // Test when Australia Day falls on Sun
            var date1 = new DateTime(2020, 1, 25);
            var date2 = new DateTime(2020, 1, 28);
            var numberOfBusinessDays = _businessDayCounter.BusinessDaysBetweenTwoDates(date1, date2, _publicHolidayRules);
            Assert.AreEqual(0, numberOfBusinessDays);
        }

        [Test]
        public void BusinessDayByRulesEasterTest()
        {
            var date1 = new DateTime(2021, 4, 1);
            var date2 = new DateTime(2021, 4, 10);
            var numberOfBusinessDays = _businessDayCounter.BusinessDaysBetweenTwoDates(date1, date2, _publicHolidayRules);
            Assert.AreEqual(4, numberOfBusinessDays);
        }


        [Test]
        public void BusinessDayByRulesAnzacDayTest()
        {
            var date1 = new DateTime(2019, 4, 24);
            var date2 = new DateTime(2019, 4, 26);
            var numberOfBusinessDays = _businessDayCounter.BusinessDaysBetweenTwoDates(date1, date2, _publicHolidayRules);
            Assert.AreEqual(0, numberOfBusinessDays);
        }

        [Test]
        public void BusinessDayByRulesQueenBirthdayTest()
        {
            var date1 = new DateTime(2021, 6, 13);
            var date2 = new DateTime(2021, 6, 19);
            var numberOfBusinessDays = _businessDayCounter.BusinessDaysBetweenTwoDates(date1, date2, _publicHolidayRules);
            Assert.AreEqual(4, numberOfBusinessDays);
        }

        [Test]
        public void BusinessDayByRulesLabourDayTest()
        {
            var date1 = new DateTime(2021, 10, 3);
            var date2 = new DateTime(2021, 10, 9);
            var numberOfBusinessDays = _businessDayCounter.BusinessDaysBetweenTwoDates(date1, date2, _publicHolidayRules);
            Assert.AreEqual(4, numberOfBusinessDays);
        }

        [Test]
        public void BusinessDayByRulesInvalidDateTest()
        {
            var date1 = new DateTime(2013, 10, 7);
            var date2 = new DateTime(2013, 10, 5);
            var numberOfBusinessDays = _businessDayCounter.BusinessDaysBetweenTwoDates(date1, date2, _publicHolidayRules);
            Assert.AreEqual(0, numberOfBusinessDays);
        }
    }
}