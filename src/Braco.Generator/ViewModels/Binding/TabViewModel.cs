using Braco.Utilities;
using System;
using System.Windows.Input;

namespace Braco.Generator
{
	[PropertyChanged.AddINotifyPropertyChangedInterface]
	public class TabViewModel
	{
		public string Icon { get; set; }
		public string Header { get; set; }
		public int ColumnIndex { get; set; }
		public ICommand Command { get; set; }
		public string BorderBrush { get; set; }

		public void Setup(Action commandAction, int columnIndex, string borderBrush)
		{
			Command = new RelayCommand(commandAction);
			ColumnIndex = columnIndex;
			BorderBrush = borderBrush;
		}
	}
}
