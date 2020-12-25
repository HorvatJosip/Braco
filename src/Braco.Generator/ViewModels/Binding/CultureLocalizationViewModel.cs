using Braco.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;

namespace Braco.Generator
{
	[PropertyChanged.AddINotifyPropertyChangedInterface]
	public class CultureLocalizationViewModel
	{
		public CultureInfo Culture { get; }
		public ObservableCollection<LocalizedTableViewModel> Sections { get; }

		public ICommand AddSectionCommand { get; }
		public ICommand RemoveSectionCommand { get; }

		public CultureLocalizationViewModel(CultureInfo culture) : this(culture, null) { }

		public CultureLocalizationViewModel(CultureInfo culture, IEnumerable<LocalizedTableViewModel> sections)
		{
			Culture = culture ?? throw new ArgumentNullException(nameof(culture));

			Sections = sections == null
				? new ObservableCollection<LocalizedTableViewModel>()
				: new ObservableCollection<LocalizedTableViewModel>(sections);

			AddSectionCommand = new RelayCommand(OnAddSection);
			RemoveSectionCommand = new RelayCommand<LocalizedTableViewModel>(OnRemoveSection);
		}

		private void OnAddSection()
		{
			Sections.Add(new LocalizedTableViewModel());
		}

		private void OnRemoveSection(LocalizedTableViewModel section)
		{
			Sections.Remove(section);
		}
	}
}
