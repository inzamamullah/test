﻿using System;
using PDS.Framework;

namespace PDS.Witsml
{
    /// <summary>
    /// Provides helper methods for common range operations.
    /// </summary>
    public static class Range
    {
        /// <summary>
        /// Parses the specified start and end range values.
        /// </summary>
        /// <param name="start">The range start value.</param>
        /// <param name="end">The range end value.</param>
        /// <param name="isTime">if set to <c>true</c> the range values are date/time.</param>
        /// <returns></returns>
        public static Range<double?> Parse(object start, object end, bool isTime)
        {
            double? rangeStart = null, rangeEnd = null;

            if (isTime)
            {
                DateTimeOffset time;

                if (start != null && DateTimeOffset.TryParse(start.ToString(), out time))
                    rangeStart = time.ToUnixTimeSeconds();

                if (end != null && DateTimeOffset.TryParse(end.ToString(), out time))
                    rangeEnd = time.ToUnixTimeSeconds();
            }
            else
            {
                double depth;

                if (start != null && double.TryParse(start.ToString(), out depth))
                    rangeStart = depth;

                if (end != null && double.TryParse(end.ToString(), out depth))
                    rangeEnd = depth;
            }

            return new Range<double?>(rangeStart, rangeEnd);
        }

        /// <summary>
        /// Determines whether a range starts after the specified value.
        /// </summary>
        /// <param name="range">The range.</param>
        /// <param name="value">The value.</param>
        /// <param name="increasing">if set to <c>true</c> the range is increasing.</param>
        /// <returns><c>true</c> if the range starts after the specified value; otherwise, <c>false</c>.</returns>
        public static bool StartsAfter(this Range<double?> range, double value, bool increasing = true)
        {
            if (!range.Start.HasValue)
                return false;

            return increasing
                ? value < range.Start.Value
                : value > range.Start.Value;
        }

        /// <summary>
        /// Determines whether a range ends before the specified value.
        /// </summary>
        /// <param name="range">The range.</param>
        /// <param name="value">The value.</param>
        /// <param name="increasing">if set to <c>true</c> the range is increasing.</param>
        /// <returns><c>true</c> if the range ends before the specified value; otherwise, <c>false</c>.</returns>
        public static bool EndsBefore(this Range<double?> range, double value, bool increasing = true)
        {
            if (!range.End.HasValue)
                return false;

            return increasing
                ? value > range.End.Value
                : value < range.End.Value;
        }

        /// <summary>
        /// Determines whether a range contains the specified value.
        /// </summary>
        /// <param name="range">The range.</param>
        /// <param name="value">The value.</param>
        /// <param name="increasing">if set to <c>true</c> the range is increasing.</param>
        /// <returns><c>true</c> if the range contains the specified value; otherwise, <c>false</c>.</returns>
        public static bool Contains(this Range<double?> range, double value, bool increasing = true)
        {
            if (!range.Start.HasValue || !range.End.HasValue)
                return false;

            return increasing
                ? (value >= range.Start.Value && value <= range.End.Value)
                : (value <= range.Start.Value && value >= range.End.Value);
        }

        /// <summary>
        /// Computes the range of a data chunk that contains the given index.
        /// </summary>
        /// <param name="index">The index contained within the computed range.</param>
        /// <param name="rangeSize">The range size of one chunk.</param>
        /// <param name="increasing">if set to <c>true</c> [increasing].</param>
        /// <returns>The range.</returns>
        public static Range<int> ComputeRange(double index, int rangeSize, bool increasing = true)
        {
            var rangeIndex = increasing ? (int)(Math.Floor(index / rangeSize)) : (int)(Math.Ceiling(index / rangeSize));
            return new Range<int>(rangeIndex * rangeSize, rangeIndex * rangeSize + (increasing ? rangeSize : -rangeSize));
        }
    }
}