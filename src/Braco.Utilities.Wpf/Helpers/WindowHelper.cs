using WinInterop = System.Windows.Interop;
using System.Runtime.InteropServices;
using System.Windows;
using System;
using System.Windows.Markup;

namespace Braco.Utilities.Wpf
{
	// Maximizing fix: https://stackoverflow.com/a/8082816/6287403
	/// <summary>
	/// Helper for dealing with <see cref="System.Windows.Window"/>.
	/// <para>Handles maximization issues.</para>
	/// </summary>
	public static class WindowHelper
	{
		/// <summary>
		/// Initializes the given window and optionally performs
		/// helper methods.
		/// </summary>
		/// <param name="window">Window to initialize.</param>
		/// <param name="fixMaximization">Should the maximization bug be fixed?</param>
		public static void Initialize(System.Windows.Window window, bool fixMaximization = true)
		{
			var initMethod = window?.GetType().GetMethod(nameof(IComponentConnector.InitializeComponent));

			initMethod?.Invoke(window, null);

			if (fixMaximization)
				FixMaximization(window);
		}

		/// <summary>
		/// Used to fix maximization hiding taskbar.
		/// </summary>
		/// <param name="window">Window for which to fix the
		/// maximization issue.</param>
		public static void FixMaximization(System.Windows.Window window)
		{
			if (window == null)
				return;

			window.SourceInitialized += (sender, e) =>
			{
				var handle = new WinInterop.WindowInteropHelper(window).Handle;

				WinInterop.HwndSource.FromHwnd(handle).AddHook(new WinInterop.HwndSourceHook(WindowProc));
			};
		}

		private static IntPtr WindowProc
		(
			  IntPtr hwnd,
			  int msg,
			  IntPtr wParam,
			  IntPtr lParam,
			  ref bool handled
		)
		{
			switch (msg)
			{
				case 0x0024:
					WmGetMinMaxInfo(hwnd, lParam);
					handled = true;
					break;
			}

			return (IntPtr)0;
		}

		private static void WmGetMinMaxInfo(IntPtr hwnd, IntPtr lParam)
		{
			var mmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));

			// Adjust the maximized size and position to fit the work area of the correct monitor
			int MONITOR_DEFAULTTONEAREST = 0x00000002;
			IntPtr monitor = MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);

			if (monitor != IntPtr.Zero)
			{
				var monitorInfo = new MONITORINFO();
				GetMonitorInfo(monitor, monitorInfo);
				RECT rcWorkArea = monitorInfo.rcWork;
				RECT rcMonitorArea = monitorInfo.rcMonitor;
				mmi.ptMaxPosition.x = Math.Abs(rcWorkArea.left - rcMonitorArea.left);
				mmi.ptMaxPosition.y = Math.Abs(rcWorkArea.top - rcMonitorArea.top);
				mmi.ptMaxSize.x = Math.Abs(rcWorkArea.right - rcWorkArea.left);
				mmi.ptMaxSize.y = Math.Abs(rcWorkArea.bottom - rcWorkArea.top);
			}

			Marshal.StructureToPtr(mmi, lParam, true);
		}

		[DllImport("user32")]
		internal static extern bool GetMonitorInfo(IntPtr hMonitor, MONITORINFO lpmi);

		[DllImport("user32.dll")]
		static extern bool GetCursorPos(ref Point lpPoint);

		[DllImport("User32")]
		internal static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);
	}

	/// <summary>
	/// Point structure.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct POINT
	{
		/// <summary>
		/// x coordinate of point.
		/// </summary>
		public int x;
		/// <summary>
		/// y coordinate of point.
		/// </summary>
		public int y;

		/// <summary>
		/// Construct a point of coordinates (x,y).
		/// </summary>
		public POINT(int x, int y)
		{
			this.x = x;
			this.y = y;
		}
	}

	/// <summary>
	/// Min Max Info structure.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct MINMAXINFO
	{
		/// <summary>
		/// Reserved.
		/// </summary>
		public POINT ptReserved;
		/// <summary>
		/// MaxSize.
		/// </summary>
		public POINT ptMaxSize;
		/// <summary>
		/// MaxPosition.
		/// </summary>
		public POINT ptMaxPosition;
		/// <summary>
		/// MinTrackSize.
		/// </summary>
		public POINT ptMinTrackSize;
		/// <summary>
		/// MaxTrackSize.
		/// </summary>
		public POINT ptMaxTrackSize;
	};

	/// <summary>
	/// Monitor info class.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	public class MONITORINFO
	{
		/// <summary>
		/// </summary>            
		public int cbSize = Marshal.SizeOf(typeof(MONITORINFO));

		/// <summary>
		/// </summary>            
		public RECT rcMonitor = new RECT();

		/// <summary>
		/// </summary>            
		public RECT rcWork = new RECT();

		/// <summary>
		/// </summary>            
		public int dwFlags = 0;
	}

	/// <summary>
	/// Rect structure.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 0)]
	public struct RECT
	{
		/// <summary> Win32 </summary>
		public int left;
		/// <summary> Win32 </summary>
		public int top;
		/// <summary> Win32 </summary>
		public int right;
		/// <summary> Win32 </summary>
		public int bottom;

		/// <summary> Win32 </summary>
		public static readonly RECT Empty = new RECT();

		/// <summary> Win32 </summary>
		public int Width
		{
			get { return Math.Abs(right - left); }  // Abs needed for BIDI OS
		}
		/// <summary> Win32 </summary>
		public int Height
		{
			get { return bottom - top; }
		}

		/// <summary> Win32 </summary>
		public RECT(int left, int top, int right, int bottom)
		{
			this.left = left;
			this.top = top;
			this.right = right;
			this.bottom = bottom;
		}


		/// <summary> Win32 </summary>
		public RECT(RECT rcSrc)
		{
			this.left = rcSrc.left;
			this.top = rcSrc.top;
			this.right = rcSrc.right;
			this.bottom = rcSrc.bottom;
		}

		/// <summary> Win32 </summary>
		public bool IsEmpty
		{
			get
			{
				// BUGBUG : On Bidi OS (hebrew arabic) left > right
				return left >= right || top >= bottom;
			}
		}
		/// <summary> Return a user friendly representation of this struct </summary>
		public override string ToString()
		{
			if (this == RECT.Empty) { return "RECT {Empty}"; }
			return "RECT { left : " + left + " / top : " + top + " / right : " + right + " / bottom : " + bottom + " }";
		}

		/// <summary> Determine if 2 RECT are equal (deep compare) </summary>
		public override bool Equals(object obj)
		{
			if (obj is not Rect) return false;
			return this == (RECT)obj;
		}

		/// <summary>Return the HashCode for this struct (not garanteed to be unique)</summary>
		public override int GetHashCode()
		{
			return left.GetHashCode() + top.GetHashCode() + right.GetHashCode() + bottom.GetHashCode();
		}


		/// <summary> Determine if 2 RECT are equal (deep compare)</summary>
		public static bool operator ==(RECT rect1, RECT rect2)
		{
			return (rect1.left == rect2.left && rect1.top == rect2.top && rect1.right == rect2.right && rect1.bottom == rect2.bottom);
		}

		/// <summary> Determine if 2 RECT are different(deep compare)</summary>
		public static bool operator !=(RECT rect1, RECT rect2)
		{
			return !(rect1 == rect2);
		}
	}
}