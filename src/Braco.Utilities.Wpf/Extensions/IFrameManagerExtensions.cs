using Braco.Services;
using Braco.Utilities.Extensions;
using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// Extensions used for <see cref="IFrameManager"/>s.
	/// </summary>
	public static class IFrameManagerExtensions
	{
		private const string frameManagerFieldName = "_frame";

		/// <summary>
		/// Sets the manager inside the field of the manager's page view model property.
		/// </summary>
		/// <param name="manager">Manager on which to set view model's manager to current instance.</param>
		public static void InitializeForPage(this IFrameManager manager)
		{
			if (manager == null) throw new NullReferenceException();

			var pageVM = manager.CurrentPageViewModel;

			var field = pageVM.GetType().GetField(frameManagerFieldName, BindingFlags.NonPublic | BindingFlags.Instance);

			var frameManager = FrameManagerDefinitions.Get(pageVM.GetType());

			field.SetValue(pageVM, frameManager);
		}

		/// <summary>
		/// Used for checking if the user is authorized to go to the current page.
		/// </summary>
		/// <param name="manager">Manager on which to check for authorization.</param>
		public static void Authorize(this IFrameManager manager)
		{
			if (manager == null) throw new NullReferenceException();

			if
			(
				// If the authorize attribute is defined and...
				Attribute.IsDefined(manager.CurrentPageViewModel.GetType(), typeof(AuthorizeAttribute)) &&
				// ... the user isn't authenticated...
				manager.AuthService?.IsAuthenticated != true
			)
			{
				// Get the defined page view model type for unauthenticated location
				var unauthenticatedPage = DI.Resources.Get<PageGetter, Type>(manager.AuthService.UnauthenticatedLocation);

				// If it was found and it is indeed a page view model...
				if (typeof(PageViewModel).IsAssignableFrom(unauthenticatedPage))
					// Redirect to it
					manager.ChangePage(unauthenticatedPage);

				else
					throw new Exception($"Couldn't find page to redirect to for unauthenticated user... Make sure to override {nameof(PageGetter.GetPageType)} method inside {nameof(PageGetter)} and return type that is a {nameof(PageViewModel)}");
			}
		}

		/// <summary>
		/// Used for loading the settings for the properties defined on the current page of the manager.
		/// </summary>
		/// <param name="manager">Manager on which to load the settings.</param>
		public static void LoadSettings(this IFrameManager manager)
		{
			if (manager == null) throw new NullReferenceException();

			if (manager.Configuration == null) return;

			var pageVM = manager.CurrentPageViewModel;

			// Get the properties that are declared as settings
			var properties = pageVM
				.GetType()
				.GetProperties()
				.Where(prop => Attribute.IsDefined(prop, typeof(SettingAttribute)));

			// Loop through settings properties
			properties.ForEach(prop =>
			{
				// Fetch the attribute
				var attr = prop.GetCustomAttribute<SettingAttribute>();

				// Get the key for configuration
				var configKey = attr.Key ?? prop.Name;

				// If it should be loaded from settings...
				if (attr.Load && prop.GetSetMethod()?.IsPublic == true)
				{
					// Extract the value from the configuration
					var configValue = manager.Configuration[configKey];

					// If there is a value for it...
					if (configValue != null)
					{
						// Set it
						prop.SetValue(pageVM, prop.PropertyType == typeof(string)
							? configValue
							: configValue.Convert(prop.PropertyType));
					}
				}

				// If the setting should be updated whenever the property changes...
				if (attr.UpdateOnValueChanged)
					// Subscribe to property changes and set the setting on change
					ReflectionUtilities.ListenForPropertyChanges(pageVM, prop.Name, _ => manager.Configuration.Set(configKey, prop.GetValue(pageVM)));
			});
		}
	}
}
