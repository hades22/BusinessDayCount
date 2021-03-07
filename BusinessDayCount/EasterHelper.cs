using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Text;

namespace BusinessDayCount
{
    public class EasterHelper
    {
        public static DateTime EasterSunday(int year)
        {
            var cache = MemoryCache.Default;
            var key = string.Format("EasterSunday{0}", year);
            if(cache.Contains(key))
            {
                return (DateTime)cache[key];
            }
            int day = 0;
            int month = 0;

            int g = year % 19;
            int c = year / 100;
            int h = (c - (int)(c / 4) - (int)((8 * c + 13) / 25) + 19 * g + 15) % 30;
            int i = h - (int)(h / 28) * (1 - (int)(h / 28) * (int)(29 / (h + 1)) * (int)((21 - g) / 11));

            day = i - ((year + (int)(year / 4) + i + 2 - c + (int)(c / 4)) % 7) + 28;
            month = 3;

            if (day > 31)
            {
                month++;
                day -= 31;
            }
            var result = new DateTime(year, month, day);
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTimeOffset.Now.AddHours(1);
            cache.Set(key, result, policy);

            return result;
        }

        public static bool IsEasterFriday(DateTime date)
        {
            var easterSunday = EasterSunday(date.Year);
            if (date.AddDays(2).Equals(easterSunday))
                return true;
            return false;
        }

        public static bool IsEasterMonday(DateTime date)
        {
            var easterSunday = EasterSunday(date.Year);
            if (date.AddDays(-1).Equals(easterSunday))
                return true;
            return false;
        }
    }
}
