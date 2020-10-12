using AutoMapper;
using Braco.Services;
using Braco.Services.Abstractions;
using Microsoft.Extensions.Configuration;
using System;

namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// Base view model for content view models (like window or page).
	/// </summary>
	[PropertyChanged.AddINotifyPropertyChangedInterface]
	public abstract class ContentViewModel
	{
		/// <summary>
		/// Suffix that should be used for view models.
		/// </summary>
		public const string ViewModelSuffix = "ViewModel";

		#region Fields

		/// <summary>
		/// Localizer.
		/// </summary>
		protected readonly ILocalizer _localizer;
		/// <summary>
		/// Window service.
		/// </summary>
		protected readonly IWindowService _windowService;
		/// <summary>
		/// Configuration.
		/// </summary>
		protected readonly IConfigurationService _config;
		/// <summary>
		/// Read-only configuration.
		/// </summary>
		protected readonly IConfiguration _readOnlyConfig;
		/// <summary>
		/// Validator.
		/// </summary>
		protected readonly Validator _validator;
		/// <summary>
		/// Mapper.
		/// </summary>
		protected readonly IMapper _mapper;

		#endregion

		/// <summary>
		/// Event that is fired after a validation has been performed.
		/// </summary>
		public event EventHandler<ValidationPerformedEventArgs> ValidationPerformed;

		#region Constructor

		/// <summary>
		/// Creates an instance of the view model.
		/// </summary>
		protected ContentViewModel()
		{
			// Make sure we have a localizer
			_localizer = DI.Localizer ?? throw new Exception("Localizer isn't defined.");

			// Subscribe to language changes
			_localizer.LanguageChanged += (sender, e) => OnLanguageChanged(e.Culture);

			// Initialize the validator
			_validator = new Validator(this, _localizer);

			// Expose its validation performed event
			_validator.ValidationPerformed += (sender, e) => ValidationPerformed?.Invoke(sender, e);

			// Assign the config
			_config = DI.Configuration;

			// If a configuration is defined...
			if (_config != null)
				// Subscribe  to setting changes
				_config.SettingChanged += (sender, e) => OnSettingChanged(e.Setting, e.OldValue, e.NewValue);

			// Assign other services
			_windowService = DI.Get<IWindowService>();
			_readOnlyConfig = DI.ReadOnlyConfiguration;
			_mapper = DI.Mapper;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Fired when the <see cref="ILocalizer.LanguageChanged"/> fires.
		/// </summary>
		/// <param name="culture">Culture that was set.</param>
		protected virtual void OnLanguageChanged(string culture) { }

		/// <summary>
		/// Fired when the <see cref="IConfigurationService.SettingChanged"/> fires.
		/// </summary>
		/// <param name="setting">Setting that changed.</param>
		/// <param name="oldValue">Old value of the setting.</param>
		/// <param name="newValue">New value of the setting.</param>
		protected virtual void OnSettingChanged(string setting, object oldValue, object newValue) { }

		/// <summary>
		/// Shows the info box.
		/// </summary>
		/// <param name="type">Type of info box.</param>
		/// <param name="title">Title for the info box.</param>
		/// <param name="message">Message for the info box.</param>
		/// <param name="duration">Duration in seconds until it dismisses.</param>
		public void ShowInfoBox(InfoBoxType type, string title, string message, int? duration = null)
			=> _windowService.ShowInfoBox(type, title, message, duration);

		/// <summary>
		/// Shows the info box.
		/// </summary>
		/// <param name="type">Type of info box.</param>
		/// <param name="message">Message for the info box.</param>
		/// <param name="duration">Duration in seconds until it dismisses.</param>
		public void ShowInfoBox(InfoBoxType type, string message, int? duration = null)
			=> _windowService.ShowInfoBox(type, message, duration);

		/// <summary>
		/// Shows an error in the info box.
		/// </summary>
		/// <param name="title">Title for the info box.</param>
		/// <param name="message">Message for the info box.</param>
		/// <param name="duration">Duration in seconds until it dismisses.</param>
		public void ShowErrorInInfoBox(string title, string message, int? duration = null)
			=> _windowService.ShowErrorInInfoBox(title, message, duration);

		/// <summary>
		/// Shows an error in the info box.
		/// </summary>
		/// <param name="message">Message for the info box.</param>
		/// <param name="duration">Duration in seconds until it dismisses.</param>
		public void ShowErrorInInfoBox(string message, int? duration = null)
			=> _windowService.ShowErrorInInfoBox(message, duration);

		#endregion
	}
}