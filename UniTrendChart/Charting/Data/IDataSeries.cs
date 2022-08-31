using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTrendChart.Charting.Data
{
    public interface IDataSeries : ISuspendable, IDataDistributionProvider
    {
        /// <summary>
        /// Gets or sets an arbitrary tag on the DataSeries
        /// </summary>
        object Tag { get; set; }

        /// <summary>
        /// Gets the Type of X-data points in this DataSeries. Used to check compatibility with Axis types.
        /// </summary>
        Type XType { get; }

        /// <summary>
        /// Gets the Type of Y-data points in this DataSeries. Used to check compatibility with Axis types.
        /// </summary>
        Type YType { get; }

        /// <summary>
        /// Gets or sets the parent <see cref="T:SciChart.Charting.Visuals.ISciChartSurface" /> which this <see cref="T:SciChart.Charting.Model.DataSeries.IDataSeries" /> instance is attached to.
        /// </summary>
        //ISciChartSurface ParentSurface { get; set; }

        /// <summary>
        /// Gets the total extents of the <see cref="T:SciChart.Charting.Model.DataSeries.IDataSeries" /> in the X direction.
        /// </summary>
        /// <remarks>Note: The performance implications of calling this is the DataSeries will perform a full recalculation on each get. It is recommended to get and cache if this property is needed more than once.</remarks>
        IRange XRange { get; }

        /// <summary>
        /// Gets the total extents of the <see cref="T:SciChart.Charting.Model.DataSeries.IDataSeries" /> in the Y direction.
        /// </summary>
        /// <remarks>Note: The performance implications of calling this is the DataSeries will perform a full recalculation on each get. It is recommended to get and cache if this property is needed more than once.</remarks>
        IRange YRange { get; }

        /// <summary>
        /// Gets the <see cref="P:SciChart.Charting.Model.DataSeries.IDataSeries.DataSeriesType" /> for this DataSeries.
        /// </summary>
        DataSeriesType DataSeriesType { get; }

        /// <summary>
        /// Gets the XValues of this dataseries.
        /// </summary>
        IList XValues { get; }

        /// <summary>
        /// Gets the YValues of this dataseries.        
        /// </summary>
        IList YValues { get; }

        /// <summary>
        /// Gets the metadata.
        /// </summary>
        IList<IPointMetadata> Metadata { get; }

        /// <summary>
        /// Gets the latest Y-Value of the DataSeries.
        /// </summary>
        IComparable LatestYValue { get; }

        /// <summary>
        /// Gets or sets the name of this series.
        /// </summary>
        string SeriesName { get; set; }

        /// <summary>
        /// Gets the computed Minimum value in Y for this series.
        /// </summary>
        IComparable YMin { get; }

        /// <summary>
        /// Gets the computed Maximum value in Y for this series.
        /// </summary>
        IComparable YMax { get; }

        /// <summary>
        /// Gets the computed Minimum value in X for this series.
        /// </summary>
        IComparable XMin { get; }

        /// <summary>
        /// Gets the computed Maximum value in X for this series.
        /// </summary>
        IComparable XMax { get; }

        /// <summary>
        /// Gets whether the dataseries behaves as a FIFO. 
        /// If True, when the FifoCapacity is reached, old points will be
        /// discarded in favour of new points, resulting in a scrolling chart.
        /// </summary>
        bool IsFifo { get; }

        /// <summary>
        /// Gets or sets the size of the FIFO buffer. 
        /// If null, then the dataseries is unlimited. 
        /// If a value is set, when the point count reaches this value, older points will be discarded.
        /// </summary>
        int? FifoCapacity { get; set; }

        /// <summary>
        /// Gets whether the DataSeries has values(is not empty).
        /// </summary>
        bool HasValues { get; }

        /// <summary>
        /// Gets whether the DataSeries contains metadata for any point(is not empty).
        /// </summary>
        bool HasMetadata { get; }

        /// <summary>
        /// Gets the number of points in this dataseries.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets whether this DataSeries contains Sorted data in the X-direction. 
        /// Note: Sorted data will result in far faster indexing operations. If at all possible, try to keep your data sorted in the X-direction.
        /// </summary>
        bool IsSorted { get; }

        /// <summary>
        /// Gets the minimal spacing between X Values
        /// </summary>
        double MinXSpacing { get; }

        /// <summary>
        /// Gets a synchronization object used to lock this data-series. Also locked on append, update, remove or clear.
        /// </summary>
        object SyncRoot { get; }

        /// <summary>
        /// New to v3.3: when AcceptsUnsortedData is false, the DataSeries with throw an InvalidOperationException if unsorted data is appended. Unintentional unsorted data can result in much slower performance. 
        /// To disable this check, set AcceptsUnsortedData = true. 
        /// </summary>
        bool AcceptsUnsortedData { get; set; }

        /// <summary>
        /// Gets the change count for this data series. Allows to indentify when data series was changed
        /// </summary>
        int ChangeCount { get; }

        /// <summary>
        /// Event raised whenever points are added to, removed or one or more DataSeries properties changes.
        /// </summary>
        event EventHandler<DataSeriesChangedEventArgs> DataSeriesChanged;

        /// <summary>
        /// Clears the series, resetting internal lists to zero size.
        /// </summary>
        void Clear();

        /// <summary>
        /// Gets the integer indices of the XValues array that are currently in the VisibleRange passed in,
        /// and an indefinite range otherwise.
        /// </summary>
        /// <example>If the input X-data is 0...99 in steps of 1, the VisibleRange is (10, 30) then the Indices Range will be 10, 30.</example>
        /// <param name="visibleRange">The VisibleRange to get the indices range.</param>
        /// <param name="downSearchMode">Specifies the search mode used to look for the index of <paramref name="visibleRange.Min.Min" />.</param>
        /// <param name="upSearchMode">Specifies the search mode used to look for the index of <paramref name="visibleRange.Max.Max" />.</param>
        /// <returns>The indices to the X-Data that are currently in range.</returns>
        IndexRange GetIndicesRange(IRange visibleRange, SearchMode downSearchMode = SearchMode.RoundDown, SearchMode upSearchMode = SearchMode.RoundUp);

        /// <summary>
        /// Converts the default <see cref="P:SciChart.Charting.Model.DataSeries.IDataSeries.YValues" /> to an <see cref="T:SciChart.Data.Model.IPointSeries" /> which is used to render XY series.
        /// </summary>
        /// <param name="resamplingParams">The resampling parameters</param>
        /// <param name="resamplingMode">The resampling mode to use</param>
        /// <param name="factory">The PointResamplerFactory which returns <see cref="!:IPointResampler" /> instances.</param>
        /// <param name="lastPointSeries">The last resampled point series</param>
        /// <returns>A <see cref="T:SciChart.Data.Model.IPointSeries" /> which is used to render XY series.</returns>
        IPointSeries ToPointSeries(ResamplingParams resamplingParams, ResamplingMode resamplingMode, IPointResamplerFactory factory, IPointSeries lastPointSeries);

        /// <summary>
        /// Gets the YRange of the data (min, max of the series) in the input visible range point range, where the input range is the <see cref="P:SciChart.Charting.Visuals.Axes.IAxisParams.VisibleRange" />.
        /// </summary>
        /// <param name="xRange">The X-Axis Range currently in view.</param>
        /// <returns>The YRange of the data in this window.</returns>
        IRange GetWindowedYRange(IRange xRange);

        /// <summary>
        /// Gets the YRange of the data (min, max of the series) in the input IndexRange, where indices are point-indices on the DataSeries columns.
        /// </summary>
        /// <param name="xIndexRange">The X-Axis Indices currently in view.</param>
        /// <returns>The YRange of the data in this window.</returns>
        IRange GetWindowedYRange(IndexRange xIndexRange);

        /// <summary>
        /// Gets the YRange of the data (min, max of the series) in the input IndexRange, where indices are point-indices on the DataSeries columns.
        /// </summary>
        /// <param name="xIndexRange">The X-Axis Indices currently in view.</param>
        /// <param name="getPositiveRange">If true, returns an <seealso cref="T:SciChart.Data.Model.IRange" /> which only has positive values, e.g, when viewing a Logarithmic chart this value might be set.</param>
        /// <returns>The YRange of the data in this window.</returns>
        IRange GetWindowedYRange(IndexRange xIndexRange, bool getPositiveRange);

        /// <summary>
        /// Gets the YRange of the data (min, max of the series) in the input visible range point range, where the input range is the <see cref="P:SciChart.Charting.Visuals.Axes.IAxisParams.VisibleRange" />.
        /// </summary>
        /// <param name="xRange">The X-Axis Range currently in view.</param>
        /// <param name="getPositiveRange">If true, returns an <seealso cref="T:SciChart.Data.Model.IRange" /> which only has positive values, e.g, when viewing a Logarithmic chart this value might be set.</param>
        /// <returns>The YRange of the data in this window.</returns>
        IRange GetWindowedYRange(IRange xRange, bool getPositiveRange);

        /// <summary>
        /// Finds the index to the DataSeries at the specified X-Value.
        /// </summary>
        /// <param name="x">The X-value to search for.</param>
        /// <param name="searchMode">The <see cref="T:SciChart.Charting.Common.Extensions.SearchMode" /> options to use. Default is exact, where -1 is returned if the index is not found.</param>
        /// <returns>The index of the found value.</returns>
        int FindIndex(IComparable x, SearchMode searchMode = SearchMode.Exact);

        /// <summary>
        /// When overridden in a derived class, returns a <see cref="T:SciChart.Charting.Visuals.RenderableSeries.HitTestInfo" /> struct containing data about the data-point at the specified index.
        /// </summary>
        /// <param name="index">The index to the DataSeries.</param>
        /// <returns>The HitTestInfo.</returns>
        HitTestInfo ToHitTestInfo(int index);

        /// <summary>
        /// May be called to trigger a redraw on the parent <see cref="T:SciChart.Charting.Visuals.SciChartSurface" />. This method is extremely useful
        /// when <see cref="T:SciChart.Charting.Model.DataSeries.IDataSeries" /> are in a ViewModel and bound via MVVM to <see cref="T:SciChart.Charting.Visuals.RenderableSeries.IRenderableSeries" />.
        /// Please see the <paramref name="rangeMode" /> parameter for invalidation options.
        /// </summary>
        /// <param name="rangeMode">Provides <see cref="T:SciChart.Charting.Model.DataSeries.RangeMode" /> invalidation options for the parent surface.</param>
        /// <param name="hasDataChanged">if set to <c>true</c> this tells the DataSeries that data has changed, and any cached values must be recreated.</param>
        void InvalidateParentSurface(RangeMode rangeMode, bool hasDataChanged = true);

        /// <summary>
        /// Finds the closest point to a point with given X and Y value. Search region is a vertical area with center in X and [maxXDistance] X units to left and right.
        /// </summary>
        /// <param name="x">The X-value of point [X data units, not pixels].</param>
        /// <param name="y">The Y-value of point [Y data units, not pixels].</param>
        /// <param name="xyScaleRatio">xUnitsPerPixel/yUnitsPerPixel.</param>
        /// <param name="maxXDistance">specifies search region in X units (ticks for DateTime or TimeSpan).</param>
        /// <returns>The index of the found value, -1 if not found (when count is zero).</returns>
        int FindClosestPoint(IComparable x, IComparable y, double xyScaleRatio, double maxXDistance);

        /// <summary>
        /// Finds the closest line to a point with given X and Y value. Search region is a vertical area with center in X and [maxXDistance] X units to left and right.
        /// </summary>
        /// <param name="x">The X-value of point [X data units, not pixels].</param>
        /// <param name="y">The Y-value of point [Y data units, not pixels].</param>
        /// <param name="xyScaleRatio">xUnitsPerPixel/yUnitsPerPixel.</param>
        /// <param name="xRadius">specifies search region in X units (ticks for DateTime or TimeSpan).</param>
        /// <param name="drawNanAs">specifies how to handle NAN elements.</param>
        /// <returns>The index of first point in line, -1 if not found (when count is zero).</returns>
        int FindClosestLine(IComparable x, IComparable y, double xyScaleRatio, double xRadius, LineDrawMode drawNanAs);
    }
}
