using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MDPMS.Database.Data.Models;
using MDPMS.Shared.Models;
using MDPMS.Shared.ViewModels.CustomControlViewModels;
using MDPMS.Shared.Views.CustomControls;
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

        public static void LoadCustomFieldValues(List<CustomField> customFields, List<ContentView> customControls, List<Tuple<CustomField, string>> values)
        {
            var i = 0;
            foreach (var customField in customFields)
            {
                // get value
                var query = values.Where(a => a.Item1.InternalId == customField.InternalId);
                if (query.Count() == 1)
                {
                    var value = query.First().Item2;
                    switch (customField.FieldType)
                    {
                        case "text":
                            var textViewModel = (CustomFieldStringValueViewModel)customControls[i].BindingContext;
                            textViewModel.EntryValue = CustomValueConverter.GetValueFromJsonText(value);
                            textViewModel.NotifyPropertyChange(nameof(textViewModel.EntryValue));
                            break;
                        case "textarea":
                            var textAreaViewModel = (CustomFieldStringValueViewModel)customControls[i].BindingContext;
                            textAreaViewModel.EntryValue = CustomValueConverter.GetValueFromJsonText(value);
                            textAreaViewModel.NotifyPropertyChange(nameof(textAreaViewModel.EntryValue));
                            break;
                        case "check_box":
                            var checkBoxViewModel = (CustomFieldSwitchArrayViewModel)customControls[i].BindingContext;
                            var checkBoxContent = new List<Tuple<bool, string>>();
                            var checkBoxSelectedValues = CustomValueConverter.GetValuesFromJsonCheckBox(value);
                            foreach (var checkBoxPossibleValue in customField.GetOptions())
                            {
                                checkBoxContent.Add(new Tuple<bool, string>(checkBoxSelectedValues.Contains(checkBoxPossibleValue), checkBoxPossibleValue));
                            }
                            checkBoxViewModel.Content = checkBoxContent;
                            checkBoxViewModel.NotifyPropertyChange(nameof(CustomFieldSwitchArrayViewModel.Content));
                            var checkBoxView = (CustomFieldSwitchArrayView)customControls[i];
                            checkBoxView.OnAppearing();
                            break;
                        case "radio_button":
                            var radioButtonViewModel = (CustomFieldPickerViewModel)customControls[i].BindingContext;
                            radioButtonViewModel.SelectedBindableOption = CustomValueConverter.GetValueFromJsonRadioButton(value);
                            radioButtonViewModel.NotifyPropertyChange(nameof(CustomFieldPickerViewModel.SelectedBindableOption));
                            break;
                        case "select":
                            var selectViewModel = (CustomFieldPickerViewModel)customControls[i].BindingContext;
                            selectViewModel.SelectedBindableOption = CustomValueConverter.GetValueFromJsonSelect(value);
                            selectViewModel.NotifyPropertyChange(nameof(CustomFieldPickerViewModel.SelectedBindableOption));
                            break;
                        case "number":
                            var numberViewModel = (CustomFieldDoubleValueViewModel)customControls[i].BindingContext;
                            var numberConverted = CustomValueConverter.GetValueFromJsonNumber(value);
                            numberViewModel.EntryValue = (numberConverted == null) ? @"" : numberConverted.ToString();
                            numberViewModel.NotifyPropertyChange(nameof(CustomFieldDoubleValueViewModel.EntryValue));
                            break;
                        case "date":
                            var dateViewModel = (CustomFieldDateTimeValueViewModel)customControls[i].BindingContext;
                            dateViewModel.DateValue = CustomValueConverter.GetValueFromJsonDate(value);
                            dateViewModel.NotifyPropertyChange(nameof(CustomFieldDateTimeValueViewModel.DateValueReadable));
                            break;
                        case "rank_list":
                            var rankListViewModel = (CustomFieldRankListViewModel)customControls[i].BindingContext;
                            var rankListValues = new ObservableCollection<Tuple<int, string>>();
                            foreach (var rankValue in CustomValueConverter.GetValueFromJsonRankList(value))
                            {
                                rankListValues.Add(rankValue);
                            }
                            rankListViewModel.Entries = rankListValues;
                            rankListViewModel.NotifyPropertyChange(nameof(CustomFieldRankListViewModel.Entries));
                            if (rankListValues.Any())
                            {
                                var rankListView = (CustomFieldRankListView)customControls[i];
                                rankListView.IsParticipating = true;
                                rankListView.ShowParticipatingViewContent();
                            }
                            break;
                    }
                }
                // TODO error log else if > 1, zero is ok as it inly indicated the absence of a value where > 1 is an error
                i++;
            }
        }
    }
}
