using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DiiCommon.Time
{
    /// <summary>
    /// Represents a sequence of <see cref="Days"/>s, starting at a specified date
    /// and concluding with the last day of that month.
    /// </summary>
    public class DaysRemainingInMonthStartingAt : IReadOnlyDictionary<int, Day>
    {
        public DaysRemainingInMonthStartingAt(DateTime specifiedDate)
            : this(new Day(specifiedDate))
        {
        }

        public DaysRemainingInMonthStartingAt(Day day)
        {
            var days = new Dictionary<int, Day>();
            var iDay = day;
            int specifiedMonthNumber = day.MonthNumber;
            for (int i = 0; iDay.MonthNumber == specifiedMonthNumber; i++)
            {
                days[i] = iDay;
                iDay = iDay.Plus(1);
            }
            Days = days;
        }

        public Day this[int key] => Days[key];

        public IReadOnlyDictionary<int, Day> Days { get; }

        public IEnumerable<int> Keys => Days.Keys;

        public IEnumerable<Day> Values => Days.Values;

        public int Count => Days.Count;

        public bool ContainsKey(int key)
        {
            return Days.ContainsKey(key);
        }

        public IEnumerator<KeyValuePair<int, Day>> GetEnumerator()
        {
            return Days.GetEnumerator();
        }

        public bool TryGetValue(int key, [MaybeNullWhen(false)] out Day value)
        {
            return Days.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Days).GetEnumerator();
        }
    }
}
