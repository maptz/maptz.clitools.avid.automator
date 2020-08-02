using Maptz;
using Maptz.Editing.Avid.Markers;
using Maptz.Editing.Avid.MarkerSections;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Maptz.Editing.Avid.Automator.WinForms.Tool
{
    class Win32
    {
		[DllImport("User32.dll")]
		static extern int SetForegroundWindow(IntPtr point);

		[DllImport("kernel32.dll")]
		static extern bool AttachConsole(int dwProcessId);
		private const int ATTACH_PARENT_PROCESS = -1;

		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool FreeConsole();


		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool GetCursorPos(ref Win32Point pt);

		[StructLayout(LayoutKind.Sequential)]
		internal struct Win32Point
		{
			public Int32 X;
			public Int32 Y;
		};

		public static Point GetMousePosition()
		{
			Win32Point w32Mouse = new Win32Point();
			GetCursorPos(ref w32Mouse);
			return new Point(w32Mouse.X, w32Mouse.Y);
		}

	}
}