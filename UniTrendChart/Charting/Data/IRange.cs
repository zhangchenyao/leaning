using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace UniTrendChart.Charting.Data
{
    public interface IRange : ICloneable, INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the Min value of this range
        /// </summary>
        IComparable Min { get; set; }

        /// <summary>
        /// Gets or sets the Max value of this range
        /// </summary>
        IComparable Max { get; set; }

        /// <summary>
        /// Gets whether this Range is defined
        /// </summary>
        /// <example>Min and Max are not equal to double.NaN, or DateTime.MinValue or DateTime.MaxValue</example>
        bool IsDefined { get; }

        /// <summary>
        /// Gets the difference (Max - Min) of this range
        /// </summary>
        IComparable Diff { get; }

        /// <summary>
        /// Gets whether the range is Zero, where Max equals Min
        /// </summary>
        bool IsZero { get; }

        /// <summary>
        /// Converts this range to a doubleRange, which are used internally for calculations
        /// </summary>
        /// <example>For numeric ranges, the conversion is simple. For DateRange instances, returns a new DoubleRange with the Min and Max Ticks</example>
        /// <returns></returns>
        DoubleRange AsDoubleRange();

        /// <summary>
        /// Grows the current by the min and max fraction, returning this instance after modification
        /// </summary>
        /// <param name="minFraction">The Min fraction to grow by. For example, Min = -10 and minFraction = 0.1 will result in the new Min = -11</param>
        /// <param name="maxFraction">The Max fraction to grow by. For example, Max = 10 and maxFraction = 0.2 will result in the new Max = 12</param>
        /// <returns>This instance, after the operation</returns>
        IRange GrowBy(double minFraction, double maxFraction);

        /// <summary>
        /// Sets the Min, Max values on the <see cref="T:SciChart.Data.Model.IRange" />, returning this instance after modification
        /// </summary>
        /// <param name="min">The new Min value.</param>
        /// <param name="max">The new Max value.</param>
        /// <returns>This instance, after the operation</returns>
        /// <remarks></remarks>
        IRange SetMinMax(double min, double max);

        /// <summary>
        /// Sets the Min, Max values on the <see cref="T:SciChart.Data.Model.IRange" /> with a max range to clip values to, returning this instance after modification
        /// </summary>
        /// <param name="min">The new Min value.</param>
        /// <param name="max">The new Max value.</param>
        /// <param name="maxRange">The max range, which is used to clip values.</param>
        /// <returns>This instance, after the operation</returns>
        /// <remarks></remarks>
        IRange SetMinMaxWithLimit(double min, double max, IRange maxRange);

        /// <summary>
        /// Clips the current <see cref="T:SciChart.Data.Model.IRange" /> to a maxmimum range with <see cref="F:SciChart.Data.Model.RangeClipMode.MinMax" /> mode
        /// </summary>
        /// <param name="maximumRange">The Maximum Range</param>
        /// <returns>This instance, after the operation</returns>
        IRange ClipTo(IRange maximumRange);

        /// <summary>
        /// Clips the current <see cref="T:SciChart.Data.Model.IRange" /> to a maximum according to clip mode
        /// </summary>
        /// <param name="maximumRange">The maximum range</param>
        /// <param name="clipMode">clip mode which defines how to clip range</param>
        /// <returns>This instance, after the operation</returns>
        IRange ClipTo(IRange maximumRange, RangeClipMode clipMode);

        /// <summary>
        /// Performs the Union of two <see cref="T:SciChart.Data.Model.IRange" /> instances, returning a new <see cref="T:SciChart.Data.Model.IRange" /></summary>
        IRange Union(IRange range);

        /// <summary>
        /// Returns True if the value is within the Min and Max of the Range
        /// </summary>
        /// <param name="value">The value to test</param>
        /// <returns>True if the value is within the Min and Max of the Range</returns>
        bool IsValueWithinRange(IComparable value);
    }
}
