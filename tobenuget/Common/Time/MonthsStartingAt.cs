using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DiiCommon.Time
{
    /// <summary>
    /// Represents a sequence of <see cref="Month"/>s, starting at a specified date.
    /// </summary>
    public class MonthsStartingAt : IReadOnlyDictionary<int, Month>
    {
        public MonthsStartingAt(DateTime specifiedDate, int count)
            : this (new Month(specifiedDate), count)
        {
        }

        public MonthsStartingAt(Month month, int count)
        {
            var months = new Dictionary<int, Month>();
            var iMonth = month;
            for (int i = 0; i < count; i++)
            {
                months[i] = iMonth;
                iMonth = iMonth.Plus(1);
            }
            Months = months;
        }

        public Month this[int key] => ((IReadOnlyDictionary<int, Month>)Months)[key];

        public Dictionary<int, Month> Months { get; }

        public IEnumerable<int> Keys => ((IReadOnlyDictionary<int, Month>)Months).Keys;

        public IEnumerable<Month> Values => ((IReadOnlyDictionary<int, Month>)Months).Values;

        public int Count => ((IReadOnlyCollection<KeyValuePair<int, Month>>)Months).Count;

        public bool ContainsKey(int key)
        {
            return ((IReadOnlyDictionary<int, Month>)Months).ContainsKey(key);
        }

        public IEnumerator<KeyValuePair<int, Month>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<int, Month>>)Months).GetEnumerator();
        }

        public bool TryGetValue(int key, [MaybeNullWhen(false)] out Month value)
        {
            return ((IReadOnlyDictionary<int, Month>)Months).TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Months).GetEnumerator();
        }
    }
}
