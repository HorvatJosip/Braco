using Braco.Utilities;
using Braco.Utilities.Wpf.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;

namespace Braco.Generator
{
	public class LocalizedTableViewModel : TableViewModel<LocalizedValueViewModel>
	{
		public string Name { get; set; }

		public ICommand AddLocalizedValueCommand { get; set; }
		public ICommand RemoveLocalizedValueCommand { get; set; }

		public LocalizedTableViewModel() 
		{
			AddLocalizedValueCommand = new RelayCommand(OnAddLocalizedValue);
			RemoveLocalizedValueCommand = new RelayCommand<LocalizedValueViewModel>(OnRemoveLocalizedValue);
		}

		public LocalizedTableViewModel(string name) : this()
		{
			Name = name;
		}

		public LocalizedTableViewModel(string name, IEnumerable<LocalizedValueViewModel> values) : this(name)
		{
			DataManager.SetDataSource(values);
		}

		private void OnAddLocalizedValue()
		{
			DataManager.AllItems.Add(new LocalizedValueViewModel());
			DataManager.UpdateAlterations();
		}

		private void OnRemoveLocalizedValue(LocalizedValueViewModel value)
		{
			DataManager.AllItems.Remove(value);
			DataManager.UpdateAlterations();
		}

		protected override IEnumerable<LocalizedValueViewModel> Load() => null;
	}
}