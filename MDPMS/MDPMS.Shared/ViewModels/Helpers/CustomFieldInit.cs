using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MDPMS.Database.Data.Models;
using MDPMS.Shared.Models;
using MDPMS.Shared.Views;
using Xamarin.Forms;

namespace MDPMS.Shared.ViewModels.Helpers
{
    public static class CustomFieldInit
    {
        public static Tuple<List<CustomField>, List<ContentView>> InitCustomFields(string modelName, ApplicationInstanceData applicationInstanceData)
        {
            // Custom Fields
            var CustomFields = new List<CustomField>();
            var CustomFieldControls = new List<ContentView>();
            CustomFields = applicationInstanceData.Data.CustomFields.Where(a => a.ModelType.Equals(modelName)).OrderBy(b => b.SortOrder).ToList();
            foreach (var customField in CustomFields)
            {
                switch (customField.FieldType)
                {
                    case @"text":
                        CustomFieldControls.Add(new CustomFieldEntryView
                        {
                            BindingContext = new CustomFieldStringValueViewModel(customField.Name, customField.HelpText),
                            Margin = new Thickness(15, 5)
                        });
                        break;
                    case @"textarea":
                        CustomFieldControls.Add(new CustomFieldEditorView
                        {
                            BindingContext = new CustomFieldStringValueViewModel(customField.Name, customField.HelpText),
                            Margin = new Thickness(15, 5)
                        });
                        break;
                    case @"check_box":
                        var checkBoxViewModel = new CustomFieldSwitchArrayViewModel(customField.Name, customField.HelpText);
                        checkBoxViewModel.Content = new List<Tuple<bool, string>>();
                        var checkBoxValues = customField.GetOptions();
                        foreach (var option in checkBoxValues)
                        {
                            checkBoxViewModel.Content.Add(new Tuple<bool, string>(false, option));
                        }
                        var checkBoxView = new CustomFieldSwitchArrayView
                        {
                            BindingContext = checkBoxViewModel,
                            Margin = new Thickness(15, 5)
                        };
                        CustomFieldControls.Add(checkBoxView);
                        checkBoxView.OnAppearing();
                        break;
                    case @"radio_button":
                        var bindableOptions = new ObservableCollection<string>();
                        bindableOptions.Add(@"");
                        var pickerValues = customField.GetOptions();
                        foreach (var option in pickerValues)
                        {
                            bindableOptions.Add(option);
                        }
                        CustomFieldControls.Add(new CustomFieldPickerView
                        {
                            BindingContext = new CustomFieldPickerViewModel(customField.Name, customField.HelpText)
                            {
                                BindableOptions = bindableOptions,
                                SelectedBindableOption = bindableOptions.First()
                            },
                            Margin = new Thickness(15, 5)
                        });
                        break;
                    case @"select":
                        var bindableSelectOptions = new ObservableCollection<string>();
                        bindableSelectOptions.Add(@"");
                        var selectValues = customField.GetOptions();
                        foreach (var option in selectValues)
                        {
                            bindableSelectOptions.Add(option);
                        }
                        CustomFieldControls.Add(new CustomFieldPickerView
                        {
                            BindingContext = new CustomFieldPickerViewModel(customField.Name, customField.HelpText)
                            {
                                BindableOptions = bindableSelectOptions,
                                SelectedBindableOption = bindableSelectOptions.First()
                            },
                            Margin = new Thickness(15, 5)
                        });
                        break;
                    case @"number":
                        CustomFieldControls.Add(new CustomFieldNumericView
                        {
                            BindingContext = new CustomFieldDoubleValueViewModel(customField.Name, customField.HelpText),
                            Margin = new Thickness(15, 5)
                        });
                        break;
                    case @"date":
                        CustomFieldControls.Add(new CustomFieldDateTimeView
                        {
                            BindingContext = new CustomFieldDateTimeValueViewModel(customField.Name, customField.HelpText),
                            Margin = new Thickness(15, 5)
                        });
                        break;
                    case @"rank_list":
                        var customFieldRankListViewModel = new CustomFieldRankListViewModel(applicationInstanceData, customField.Name, customField.HelpText);
                        customFieldRankListViewModel.Entries = new ObservableCollection<Tuple<int, string>>();
                        var rankValues = customField.GetOptions();
                        for (var j = 0; j < rankValues.Count; j++)
                        {
                            customFieldRankListViewModel.Entries.Add(new Tuple<int, string>(j, rankValues[j]));
                        }
                        var rankListView = new CustomFieldRankListView
                        {
                            BindingContext = customFieldRankListViewModel,
                            Margin = new Thickness(15, 5)
                        };
                        customFieldRankListViewModel.View = rankListView;
                        CustomFieldControls.Add(rankListView);
                        rankListView.OnAppearing();
                        break;
                    default:
                        // TODO: error log but for now just fail silently
                        break;
                }
            }

            return new Tuple<List<CustomField>, List<ContentView>>(CustomFields, CustomFieldControls);
        }
    }
}
