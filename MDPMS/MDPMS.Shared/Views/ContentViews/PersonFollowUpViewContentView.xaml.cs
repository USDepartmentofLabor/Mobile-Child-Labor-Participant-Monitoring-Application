using System;
using System.Linq;
using MDPMS.Shared.ViewModels.ContentViewModels;
using MDPMS.Shared.ViewModels.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MDPMS.Shared.Views.ContentViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PersonFollowUpViewContentView : ContentView
    {
        public PersonFollowUpViewContentView()
        {
            InitializeComponent();
        }

        public void OnAppearing()
        {
            var viewModel = (PersonFollowUpViewContentViewModel)BindingContext;

            // Custom Fields
            CustomFieldContent.Children.Clear();

            var grid = new Grid();
            var labelColumnDefinition = new ColumnDefinition();
            grid.ColumnDefinitions.Add(labelColumnDefinition);
            var valueColumnDefinition = new ColumnDefinition();
            valueColumnDefinition.Width = new GridLength(3, GridUnitType.Star);
            grid.ColumnDefinitions.Add(valueColumnDefinition);

            var customFields = viewModel.ApplicationInstanceData.Data.CustomFields
                                            .Where(a => a.ModelType.Equals(@"FollowUp"))
                                            .OrderBy(b => b.SortOrder);
            var i = 0;
            foreach (var customField in customFields)
            {
                var rowDefinition = new RowDefinition();
                rowDefinition.Height = 30;
                var valueQuery = viewModel.ApplicationInstanceData.Data.CustomPersonFollowUpValues
                                          .Where(a => (a.CustomField.InternalId == customField.InternalId))
                                          .Where(b => b.PersonFollowUp.InternalId == viewModel.PersonFollowUp.InternalId);
                var queryCount = valueQuery.Any() ? valueQuery.Count() : 0;
                var valueString = @"";
                if (queryCount > 1) { /* TODO: error log */ }
                if (queryCount.Equals(1))
                {
                    var value = valueQuery.First();
                    valueString = value.Value;
                    switch (customField.FieldType)
                    {
                        case @"text":
                            valueString = CustomValueConverter.GetValueFromJsonText(valueString);
                            break;
                        case @"textarea":
                            valueString = CustomValueConverter.GetValueFromJsonTextArea(valueString);
                            rowDefinition.Height = GridLength.Auto;
                            break;
                        case @"check_box":
                            valueString = CustomValueConverter.GetValueFromJsonCheckBox(valueString);
                            rowDefinition.Height = GridLength.Auto;
                            break;
                        case @"radio_button":
                            valueString = CustomValueConverter.GetValueFromJsonRadioButton(valueString);
                            break;
                        case @"select":
                            valueString = CustomValueConverter.GetValueFromJsonSelect(valueString);
                            break;
                        case @"number":
                            var numberConverted = CustomValueConverter.GetValueFromJsonNumber(valueString);
                            valueString = (numberConverted == null) ? @"" : numberConverted.ToString();
                            break;
                        case @"date":
                            var dateTimeConverted = CustomValueConverter.GetValueFromJsonDate(valueString);
                            valueString = (dateTimeConverted == null) ? @"" : ((DateTime)dateTimeConverted).ToShortDateString();
                            break;
                        case @"rank_list":
                            valueString = CustomValueConverter.GetDisplayValueFromJsonRankList(valueString);
                            rowDefinition.Height = GridLength.Auto;
                            break;
                    }
                }

                grid.RowDefinitions.Add(rowDefinition);

                var labelLabel = new Label();
                labelLabel.Text = customField.Name;
                labelLabel.SetValue(Grid.ColumnProperty, 0);
                labelLabel.SetValue(Grid.RowProperty, i);
                labelLabel.Style = (Style)Application.Current.Resources["LabelDescription"];
                grid.Children.Add(labelLabel);

                var valueLabel = new Label();
                valueLabel.Text = valueString;
                valueLabel.SetValue(Grid.ColumnProperty, 1);
                valueLabel.SetValue(Grid.RowProperty, i);
                valueLabel.Style = (Style)Application.Current.Resources["LabelValue"];
                grid.Children.Add(valueLabel);

                i++;
            }
            CustomFieldContent.Children.Add(grid);
        }
    }
}
