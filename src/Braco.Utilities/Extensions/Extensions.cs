using System;
using System.Linq.Expressions;

namespace Braco.Utilities.Extensions
{
	/// <summary>
	/// Extension that can't be specifically grouped.
	/// </summary>
	public static class Extensions
	{
		/// <summary>
		/// Used for raising property changed event for the given member.
		/// </summary>
		/// <typeparam name="T">Type of object on which the member changes.</typeparam>
		/// <typeparam name="TTarget">Target member that changes.</typeparam>
		/// <param name="obj">Object on which the member changes.</param>
		/// <param name="selector">Used for selecting the member that changes.</param>
		/// <returns>If the property changed event was raised.</returns>
		public static bool Change<T, TTarget>(this T obj, Expression<Func<T, TTarget>> selector)
			=> Change(obj, selector, null);

		/// <summary>
		/// Used for raising property changed event for the given member.
		/// </summary>
		/// <typeparam name="T">Type of object on which the member changes.</typeparam>
		/// <typeparam name="TTarget">Target member that changes.</typeparam>
		/// <param name="obj">Object on which the member changes.</param>
		/// <param name="selector">Used for selecting the member that changes.</param>
		/// <param name="action">Action to invoke using the given object.</param>
		/// <param name="invokeActionBeforeRaisingPropertyChangedEvent">If set to true, <paramref name="action"/>
		/// will be invoked before raising property changed event. Otherwise, it will be called after the property
		/// changed event was raised.
		/// </param>
		/// <returns>If the property changed event was raised.</returns>
		public static bool Change<T, TTarget>(this T obj, Expression<Func<T, TTarget>> selector, Action<TTarget> action, bool invokeActionBeforeRaisingPropertyChangedEvent = true)
		{
			if (selector.Body is not MemberExpression memberExpression) return false;

			var memberName = memberExpression.Member.Name;

			if(action != null)
			{
				var targetProperty = selector.Compile().Invoke(obj);

				if (invokeActionBeforeRaisingPropertyChangedEvent) action(targetProperty);

				var wasRaised = ReflectionUtilities.RaisePropertyChanged(obj, memberName);

				if (!invokeActionBeforeRaisingPropertyChangedEvent) action(targetProperty);

				return wasRaised;
			}

			return ReflectionUtilities.RaisePropertyChanged(obj, memberName);
		}
	}
}
