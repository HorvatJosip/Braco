using System;

namespace Braco.Services.Media.Abstractions
{
	/// <summary>
	/// Holds two <see cref="TimeSpan"/>s which form a time range.
	/// </summary>
	public class TimeRange
	{
		/// <summary>
		/// Starting time.
		/// </summary>
		public TimeSpan Start { get; set; }

		/// <summary>
		/// Ending time.
		/// </summary>
		public TimeSpan End { get; set; }

		/// <summary>
		/// Constructs a time range from given times.
		/// <para>They don't have to be in correct order, they will
		/// be fixed automatically.</para>
		/// </summary>
		/// <param name="start">Start of the time range.</param>
		/// <param name="end">End of the time range.</param>
		public TimeRange(TimeSpan start, TimeSpan end)
		{
			if (start < end)
			{
				Start = start;
				End = end;
			}
			else
			{
				End = start;
				Start = end;
			}
		}

		/// <summary>
		/// Checks if this time range contains specific time point.
		/// </summary>
		/// <param name="timePoint">Time point to check if it is within this time range.</param>
		/// <returns>If the given time point is within this time range.</returns>
		public bool Contains(TimeSpan timePoint)
			=> timePoint >= Start && timePoint <= End;
	}
}
