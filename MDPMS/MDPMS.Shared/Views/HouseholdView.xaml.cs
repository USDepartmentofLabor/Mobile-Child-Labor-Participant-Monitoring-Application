using System.Linq;
using MDPMS.Shared.ViewModels;
using Xamarin.Forms;

namespace MDPMS.Shared.Views
{
    public partial class HouseholdView : ContentPage
    {
        public HouseholdView()
        {
            InitializeComponent();
        }

		protected override void OnAppearing()
		{
			base.OnAppearing();

            var viewModel = (HouseholdViewModel)BindingContext;

            CustomFieldContent.Children.Clear();

            var grid = new Grid();
            var labelColumnDefinition = new ColumnDefinition();
            labelColumnDefinition.Width = 200;
            grid.ColumnDefinitions.Add(labelColumnDefinition);
            var valueColumnDefinition = new ColumnDefinition();
            grid.ColumnDefinitions.Add(valueColumnDefinition);

            var householdCustomFields = viewModel.ApplicationInstanceData.Data.CustomFields
                                            .Where(a => a.ModelType.Equals(@"Household"))
                                            .OrderBy(b => b.SortOrder);
            var i = 0;
            foreach (var customField in householdCustomFields)
            {
                var rowDefinition = new RowDefinition();
                rowDefinition.Height = 30;
                var valueQuery = viewModel.ApplicationInstanceData.Data.CustomHouseholdValues
                                          .Where(a => (a.CustomField.InternalId == customField.InternalId))
                                          .Where(b => b.Household.InternalId == viewModel.Household.InternalId);
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
                            break;
                        case @"textarea":
                            rowDefinition.Height = GridLength.Auto;
                            break;
                        case @"check_box":
                            rowDefinition.Height = GridLength.Auto;
                            break;
                        case @"radio_button":
                            break;
                        case @"select":
                            break;
                        case @"number":
                            break;
                        case @"date":
                            break;
                        case @"rank_list":
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
		}
	}
}
