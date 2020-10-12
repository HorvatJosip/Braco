using System;

namespace Braco.Utilities
{
	/// <summary>
	/// Progress manager that manages progress in percentages - the
	/// <see cref="Value"/> property can range from 0 to 100. This can be
	/// useful for report progress on things you know the maximum of in advance.
	/// <para>There is one requirement for this report progress correctly -
	/// either use constructor that takes in a maximum or set it yourself
	/// using the <see cref="Initialize(double)"/> method.</para>
	/// </summary>
	[PropertyChanged.AddINotifyPropertyChangedInterface]
	public class PercentageProgress : IProgress<double>
	{
		/// <summary>
		/// Maximum possible percentage value (100%).
		/// </summary>
		public const double MaximumPercentage = 100;

		private double _multiplier;

		/// <summary>
		/// Current percentage value.
		/// </summary>
		public double Value { get; set; }

		/// <summary>
		/// Flag indicating if the progress is being tracked.
		/// </summary>
		public bool Active { get; set; }

		/// <summary>
		/// Creates an instance without initializing. Make sure to
		/// use <see cref="Initialize(double)"/> method on your own
		/// when you determine the maximum.
		/// </summary>
		public PercentageProgress() { }

		/// <summary>
		/// Creates an instance and initializes the progress
		/// reporting value with the given maximum.
		/// </summary>
		/// <param name="maximum">Value that means the task is done (e.g.
		/// end of file has been reached at 2 GB - 2 GB is maximum).</param>
		public PercentageProgress(double maximum) => Initialize(maximum);

		/// <summary>
		/// Initializes the progress reporting value.
		/// </summary>
		/// <param name="maximum">Value that means the task is done (e.g.
		/// end of file has been reached at 2 GB - 2 GB is maximum).</param>
		public void Initialize(double maximum)
		{
			if (maximum <= 0) throw new ArgumentOutOfRangeException(nameof(maximum), "You must provide a value over 0.");

			_multiplier = MaximumPercentage / maximum;
		}

		/// <inheritdoc/>
		public void Report(double value)
		{
			Active = true;
			Value = value * _multiplier;

			if (Value >= 100)
			{
				Value = 100;
				Active = false;
			}
		}

		/// <summary>
		/// Resets the progress reporter to starting phase.
		/// </summary>
		public void Reset()
		{
			Value = 0;
			Active = false;
		}
	}
}
