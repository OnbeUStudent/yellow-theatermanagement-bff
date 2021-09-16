using System;

namespace DiiCommon.Time
{
    public class Day
    {
        protected readonly DateTime _dateTime;
        private readonly Month _month;

        public Day(int dayId)
        {
            int year = dayId / 10000;
            int day = ((dayId - 1) % 100) + 1;
            int month = ((((dayId - day) - 1) % 10000) + 1) / 100;
            _dateTime = new DateTime(year, month, day);
            _month = new Month(year, month);
        }

        public Day(DateTime specifiedDate)
        {
            _dateTime = specifiedDate;
            _month = new Month(specifiedDate.Year, specifiedDate.Month);
        }

        public Day(int year, int month, int day)
        {
            _dateTime = new DateTime(year, month, day);
            _month = new Month(year, month);
        }

        /// <summary>
        /// The day in YYYYMMDD format.
        /// </summary>
        public int DayId
        {
            get
            {
                return (_dateTime.Year * 10000 + _dateTime.Month * 100 + _dateTime.Day);
            }
        }

        public int MonthNumber => _dateTime.Month;

        public int YearNumber => _dateTime.Year;

        public int DayNumber => _dateTime.Day;

        public Month Month => _month;

        public string DisplayValue => _dateTime.ToString("d");

        public int MonthId 
        { 
            get {
                return Month.GetMonthId(YearNumber, MonthNumber);
            } 
        }

        internal Day Plus(int offset)
        {
            return new Day(_dateTime.AddDays(offset));
        }
    }
}
