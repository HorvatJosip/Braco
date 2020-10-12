namespace Braco.Utilities.Wpf
{
    /// <summary>
    /// Actions that can be performed on a window.
    /// </summary>
    public enum WindowAction
    {
        /// <summary>
        /// Close the window.
        /// </summary>
        Close,

        /// <summary>
        /// Hide the window.
        /// </summary>
        Hide,

        /// <summary>
        /// Leave the window shown, but don't let it be interactable.
        /// </summary>
        LeaveShown,

        /// <summary>
        /// Leave the window shown and interactable.
        /// </summary>
        LeaveShownAndInteractable
    }
}
