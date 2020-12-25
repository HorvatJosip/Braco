using Braco.Services.Abstractions;
using Braco.Utilities.Wpf;
using Braco.Utilities.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Braco.Generator
{
	public class ProjectHomePageTabs : IHaveLocalizedCollection<TabViewModel>
	{
		private const string selectedBrush = ResourceKeys.PrimaryHoverBrush;
		private const string notSelectedBrush = ResourceKeys.PrimaryBrush;

		private readonly ObservableCollection<TabViewModel> _tabs = new ObservableCollection<TabViewModel>();
		private readonly Dictionary<int, Func<Action>> _actionIndexMap;

		public Action PagesTabClick { get; set; }
		public Action ImagesTabClick { get; set; }
		public Action LocalizationTabClick { get; set; }
		public Action FontsTabClick { get; set; }
		public Action ConstantsTabClick { get; set; }

		public IEnumerable<TabViewModel> Collection => _tabs;
		IEnumerable IHaveLocalizedCollection.Collection => Collection;

		public ProjectHomePageTabs()
		{
			_actionIndexMap = new Dictionary<int, Func<Action>>
			{
				{ 0, () => PagesTabClick },
				{ 1, () => ImagesTabClick },
				{ 2, () => LocalizationTabClick },
				{ 3, () => FontsTabClick },
				{ 4, () => ConstantsTabClick },
			};
		}

		public void Fill(ILocalizer localizer)
		{
			var tabs = new[]
			{
				new TabViewModel
				{
					Header = localizer[LocalizationKeys.LocalizedCollections_PagesTab],
					Icon = ResourceKeys.PagesIcon,
				},
				new TabViewModel
				{
					Header = localizer[LocalizationKeys.LocalizedCollections_ImagesTab],
					Icon = ResourceKeys.ImagesIcon,
				},
				new TabViewModel
				{
					Header = localizer[LocalizationKeys.LocalizedCollections_LocalizationTab],
					Icon = ResourceKeys.LocalizationIcon,
				},
				new TabViewModel
				{
					Header = localizer[LocalizationKeys.LocalizedCollections_FontsTab],
					Icon = ResourceKeys.FontsIcon,
				},
				new TabViewModel
				{
					Header = localizer[LocalizationKeys.LocalizedCollections_ConstantsTab],
					Icon = ResourceKeys.ConstantsIcon,
				}
			};

			tabs.ForEach((tab, i) => tab.Setup(() =>
			{
				_tabs.ForEach((currentTab, tabIndex) => currentTab.BorderBrush = GetBrush(tabIndex == i));

				_actionIndexMap[i]()?.Invoke();
			}, i, GetBrush(i == 0)));

			_tabs.RenewData(tabs);
		}

		private string GetBrush(bool isSelected)
			=> isSelected ? selectedBrush : notSelectedBrush;
	}
}
