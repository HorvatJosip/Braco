using System.Windows.Controls;

namespace Braco.Utilities.Wpf.Controls
{
	/// <inheritdoc/>
	public class ResourceKeys : Wpf.ResourceKeys
	{
		#region Numbers

		/// <summary>
		/// Smaller font size.
		/// </summary>
		public const string SmallerFontSize = "fontSizeSmaller";
		/// <summary>
		/// Small font size.
		/// </summary>
		public const string SmallFontSize = "fontSizeSmall";
		/// <summary>
		/// Medium font size.
		/// </summary>
		public const string MediumFontSize = "fontSizeMedium";
		/// <summary>
		/// Large font size.
		/// </summary>
		public const string LargeFontSize = "fontSizeLarge";
		/// <summary>
		/// Very large font size.
		/// </summary>
		public const string VeryLargeFontSize = "fontSizeVeryLarge";
		/// <summary>
		/// Extremely large font size.
		/// </summary>
		public const string ExtremelyLargeFontSize = "fontSizeExtremelyLarge";

		#endregion

		#region Colors

		/// <summary>
		/// Success color.
		/// </summary>
		public const string SuccessColor = "success";
		/// <summary>
		/// Information color.
		/// </summary>
		public const string InformationColor = "information";
		/// <summary>
		/// Warning color.
		/// </summary>
		public const string WarningColor = "warning";
		/// <summary>
		/// Error color.
		/// </summary>
		public const string ErrorColor = "error";
		/// <summary>
		/// Primary color.
		/// </summary>
		public const string PrimaryColor = "primary";
		/// <summary>
		/// Primary hover color.
		/// </summary>
		public const string PrimaryHoverColor = "primaryHover";
		/// <summary>
		/// Primary light color.
		/// </summary>
		public const string PrimaryLightColor = "primaryLight";
		/// <summary>
		/// Secondary color.
		/// </summary>
		public const string SecondaryColor = "secondary";
		/// <summary>
		/// Secondary hover color.
		/// </summary>
		public const string SecondaryHoverColor = "secondaryHover";
		/// <summary>
		/// Tertiary color.
		/// </summary>
		public const string TertiaryColor = "tertiary";
		/// <summary>
		/// Tertiary hover color.
		/// </summary>
		public const string TertiaryHoverColor = "tertiaryHover";
		/// <summary>
		/// Scroll color.
		/// </summary>
		public const string ScrollColor = "scroll";
		/// <summary>
		/// Placeholder color.
		/// </summary>
		public const string PlaceholderColor = "placeholder";

		#endregion

		#region Brushes

		/// <summary>
		/// Success brush.
		/// </summary>
		public const string SuccessBrush = "InfoBoxType_Success_Brush";
		/// <summary>
		/// Information brush.
		/// </summary>
		public const string InformationBrush = "InfoBoxType_Information_Brush";
		/// <summary>
		/// Warning brush.
		/// </summary>
		public const string WarningBrush = "InfoBoxType_Warning_Brush";
		/// <summary>
		/// Error brush.
		/// </summary>
		public const string ErrorBrush = "InfoBoxType_Error_Brush";
		/// <summary>
		/// Primary brush.
		/// </summary>
		public const string PrimaryBrush = "brushPrimary";
		/// <summary>
		/// Primary hover brush.
		/// </summary>
		public const string PrimaryHoverBrush = "brushPrimaryHover";
		/// <summary>
		/// Primary light brush.
		/// </summary>
		public const string PrimaryLightBrush = "brushPrimaryLight";
		/// <summary>
		/// Secondary brush.
		/// </summary>
		public const string SecondaryBrush = "brushSecondary";
		/// <summary>
		/// Secondary hover brush.
		/// </summary>
		public const string SecondaryHoverBrush = "brushSecondaryHover";
		/// <summary>
		/// Tertiary brush.
		/// </summary>
		public const string TertiaryBrush = "brushTertiary";
		/// <summary>
		/// Tertiary hover brush.
		/// </summary>
		public const string TertiaryHoverBrush = "brushTertiaryHover";
		/// <summary>
		/// Scroll brush.
		/// </summary>
		public const string ScrollBrush = "brushScroll";
		/// <summary>
		/// Placeholder brush.
		/// </summary>
		public const string PlaceholderBrush = "brushPlaceholder";

		#endregion

		#region Styles

		/// <summary>
		/// EditableComboBoxToggleButton style.
		/// </summary>
		public const string EditableComboBoxToggleButtonStyle = "EditableComboBoxToggleButton";
		/// <summary>
		/// ReadOnlyComboBoxToggleButton style.
		/// </summary>
		public const string ReadOnlyComboBoxToggleButtonStyle = "ReadOnlyComboBoxToggleButton";
		/// <summary>
		/// Key for the style that defines default cell style for a <see cref="DataGrid"/>.
		/// </summary>
		public const string DefaultCellStyle = "DefaultCellStyle";
		/// <summary>
		/// Key for the style that defines default cell style for a row of a <see cref="DataGrid"/>.
		/// </summary>
		public const string DefaultRowStyle = "DefaultRowStyle";

		#endregion

		#region Images

		/// <summary>
		/// Logo image.
		/// </summary>
		public const string LogoImage = "Logo";

		#endregion

		#region Image File Name Getters

		public const string SortIconFileNameGetter = "ImageFileNameGetters.SortIcon";

		#endregion

		#region Icons

		/// <summary>
		/// Icon that is used for opening the <see cref="ComboBox"/>.
		/// </summary>
		public const string DownIcon = "Down";
		/// <summary>
		/// Icon that is used for settings button on the title bar.
		/// </summary>
		public const string SettingsIcon = "Settings";
		/// <summary>
		/// Icon that is used for minimize button on the title bar.
		/// </summary>
		public const string MinimizeIcon = "Minimize";
		/// <summary>
		/// Icon that is used for close button on the title bar.
		/// </summary>
		public const string CloseIcon = "Close";
		/// <summary>
		/// Icon that is used for cancel button on the info box.
		/// </summary>
		public const string CancelIcon = "Cancel";
		/// <summary>
		/// Icon that is used for info tooltip on input field.
		/// </summary>
		public const string QuestionMarkIcon = "QuestionMark";
		/// <summary>
		/// Icon that is used for previous page button.
		/// </summary>
		public const string PreviousPageIcon = "PreviousPage";

		#endregion

		#region Application

		/// <summary>
		/// Application name tooltip.
		/// </summary>
		public const string ApplicationName = "Application_Name";
		/// <summary>
		/// Tooltip for the close button on title bar of main window.
		/// </summary>
		public const string ApplicationCloseToolTip = "Application_Close_ToolTip";

		#endregion

		#region Window

		/// <summary>
		/// Tooltip for the close button on title bar of other windows.
		/// </summary>
		public const string WindowCloseToolTip = "Window_Close_ToolTip";
		/// <summary>
		/// Tooltip for the previous page button on the window.
		/// </summary>
		public const string WindowPreviousPageButtonToolTip = "Window_PreviousPageButton_ToolTip";

		#endregion

		#region TitleBar

		/// <summary>
		/// Tooltip for settings button on title bar.
		/// </summary>
		public const string TitleBarSettingsButtonToolTip = "TitleBar_Settings_ToolTip";
		/// <summary>
		/// Tooltip for settings button on title bar.
		/// </summary>
		public const string TitleBarMinimizeButtonToolTip = "TitleBar_Minimize_ToolTip";
		/// <summary>
		/// Tooltip for maximize button on title bar.
		/// </summary>
		public const string TitleBarMaximizeButtonToolTip = "TitleBar_Maximize_ToolTip";
		/// <summary>
		/// Tooltip for restore button on title bar.
		/// </summary>
		public const string TitleBarRestoreButtonToolTip = "TitleBar_Restore_ToolTip"; 

		#endregion
	}
}
