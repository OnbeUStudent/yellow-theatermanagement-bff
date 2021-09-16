using System;

namespace DiiCommon.Time
{
    public class Month
    {
        protected int _year;
        protected int _month;

        public Month(int monthId)
        {
            _year = monthId / 100;
            _month = ((monthId - 1) % 100) + 1;
        }

        public Month(DateTime now)
            : this(now.Year, now.Month)
        {
        }

        public Month(int year, int month)
        {
            // Normalize, even if the month is greater than 12. So 2000 and 13 yields 200101.
            _year = year + (month - 1) / 12;
            _month = ((month - 1) % 12) + 1;
        }

        /// <summary>
        /// The month in YYYYMM format.
        /// </summary>
        public int MonthId
        {
            get
            {
                return GetMonthId(_year, _month);
            }
        }

        public int MonthNumber => _month;

        public int YearNumber => _year;

        public string DisplayValue => GetDisplayValue(MonthId);

        public Month Plus(int offset)
        {
            return new Month(this._year, this._month + offset);
        }

        public static int GetMonthId(int year, int month)
        {
            return (year * 100 + month);
        }

        public static string GetDisplayValue(int monthId)
        {
            int year = monthId / 100;
            int month = ((monthId - 1) % 100) + 1;

            return $"{month:##}/{year:####}";
        }

        public Day FirstDay()
        {
            return new Day(_year, _month, 1);
        }
    }
}
