using System.Linq;
using MDPMS.Shared.ViewModels.ContentViewModels;
using MDPMS.Shared.ViewModels.Helpers;
using Xamarin.Forms;

namespace MDPMS.Shared.Views.ContentViews
{
    public partial class PersonViewContentView : ContentView
    {
        public PersonViewContentView()
        {
            InitializeComponent();
        }

        public void OnAppearing()
        {
            var viewModel = (PersonViewContentViewModel)BindingContext;

            // Custom Fields
            CustomFieldContent.Children.Clear();

            var grid = new Grid();
            var labelColumnDefinition = new ColumnDefinition();
            labelColumnDefinition.Width = 200;
            grid.ColumnDefinitions.Add(labelColumnDefinition);
            var valueColumnDefinition = new ColumnDefinition();
            grid.ColumnDefinitions.Add(valueColumnDefinition);

            var customFields = viewModel.ApplicationInstanceData.Data.CustomFields
                                            .Where(a => a.ModelType.Equals(@"Person"))
                                            .OrderBy(b => b.SortOrder);
            var i = 0;
            foreach (var customField in customFields)
            {
                var rowDefinition = new RowDefinition();
                rowDefinition.Height = 30;
                var valueQuery = viewModel.ApplicationInstanceData.Data.CustomPersonValues
                                          .Where(a => (a.CustomField.InternalId == customField.InternalId))
                                          .Where(b => b.Person.InternalId == viewModel.Person.InternalId);
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
                            valueString = CustomValueConverter.GetValueFromJsonNumber(valueString).ToString();
                            break;
                        case @"date":
                            valueString = CustomValueConverter.GetValueFromJsonDate(valueString).ToShortDateString();
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
                labelLabel.Style = (Style)Resources["LabelDescription"];
                grid.Children.Add(labelLabel);

                var valueLabel = new Label();
                valueLabel.Text = valueString;
                valueLabel.SetValue(Grid.ColumnProperty, 1);
                valueLabel.SetValue(Grid.RowProperty, i);
                valueLabel.Style = (Style)Resources["LabelValue"];
                grid.Children.Add(valueLabel);

                i++;
            }
            CustomFieldContent.Children.Add(grid);

            // Person Follow Ups
            PersonFollowUpsContent.Children.Clear();

            var personFollowUpsViewContentView = new PersonFollowUpsViewContentView();
            var personFollowUpsViewContentViewModel = new PersonFollowUpsViewContentViewModel(viewModel.ApplicationInstanceData, viewModel.Person);
            personFollowUpsViewContentView.BindingContext = personFollowUpsViewContentViewModel;
            PersonFollowUpsContent.Children.Add(personFollowUpsViewContentView);
            personFollowUpsViewContentView.OnAppearing();

            // Service Instances
            ServiceInstancesContent.Children.Clear();
            var serviceInstancesViewContentView = new ServiceInstancesViewContentView();
            var serviceInstancesViewContentViewModel = new ServiceInstancesViewContentViewModel(viewModel.ApplicationInstanceData, viewModel.Person);
            serviceInstancesViewContentView.BindingContext = serviceInstancesViewContentViewModel;
            ServiceInstancesContent.Children.Add(serviceInstancesViewContentView);
            serviceInstancesViewContentView.OnAppearing();
        }
    }
}
