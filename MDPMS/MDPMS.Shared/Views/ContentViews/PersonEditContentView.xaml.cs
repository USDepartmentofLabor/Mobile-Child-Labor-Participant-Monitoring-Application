using System;
using System.Collections.Generic;
using System.Linq;
using MDPMS.Database.Data.Models;
using MDPMS.Shared.ViewModels.ContentViewModels;
using MDPMS.Shared.ViewModels.Helpers;
using Xamarin.Forms;

namespace MDPMS.Shared.Views.ContentViews
{
    public partial class PersonEditContentView : ContentView
    {
        public PersonEditContentView()
        {
            InitializeComponent();
        }

        public void OnAppearing(bool loadValues)
        {
            var viewModel = (PersonEditContentViewModel)BindingContext;

            // Work Activities
            DynamicWorkActivities.Children.Clear();
            var workActivitiesGrid = new Grid();
            var i = 0;
            foreach (var bindableWorkActivity in viewModel.BindableWorkActivities)
            {
                var newWorkActivitiesGridRow = new RowDefinition();
                newWorkActivitiesGridRow.Height = 80;
                var newWorkActivityContent = new CustomControls.NewGenericSwitchTextView { BindingContext = bindableWorkActivity.Item3 };
                newWorkActivityContent.SetValue(Grid.RowProperty, i);
                workActivitiesGrid.Children.Add(newWorkActivityContent);
                i++;
            }
            DynamicWorkActivities.Children.Add(workActivitiesGrid);

            // Hazardous Conditions
            DynamicHazardousConditions.Children.Clear();
            var hazardousConditionsGrid = new Grid();
            i = 0;
            foreach (var bindableHazardousCondition in viewModel.BindableHazardousConditions)
            {
                var newHazardousConditionsGridRow = new RowDefinition();
                newHazardousConditionsGridRow.Height = 80;
                var newHazardousConditionContent = new CustomControls.NewGenericSwitchTextView { BindingContext = bindableHazardousCondition.Item3 };
                newHazardousConditionContent.SetValue(Grid.RowProperty, i);
                hazardousConditionsGrid.Children.Add(newHazardousConditionContent);
                i++;
            }
            DynamicHazardousConditions.Children.Add(hazardousConditionsGrid);

            // Household Tasks
            DynamicHouseholdTasks.Children.Clear();
            var householdTasksGrid = new Grid();
            i = 0;
            foreach (var bindableHouseholdTask in viewModel.BindableHouseholdTasks)
            {
                var newHouseholdTasksGridRow = new RowDefinition();
                newHouseholdTasksGridRow.Height = 80;
                var newHouseholdTaskContent = new CustomControls.NewGenericSwitchTextView { BindingContext = bindableHouseholdTask.Item3 };
                newHouseholdTaskContent.SetValue(Grid.RowProperty, i);
                householdTasksGrid.Children.Add(newHouseholdTaskContent);
                i++;
            }
            DynamicHouseholdTasks.Children.Add(householdTasksGrid);

            // Custom Fields
            CustomFieldContent.Children.Clear();
            foreach (var customFieldControl in viewModel.CustomFieldControls)
            {
                CustomFieldContent.Children.Add(customFieldControl);
            }

            if (!loadValues) return;

            // query values (optional)
            var values = new List<Tuple<CustomField, string>>();
            var query = viewModel.ApplicationInstanceData.Data.CustomPersonValues.Where(a => a.Person.InternalId == viewModel.Person.InternalId);
            foreach (var result in query) values.Add(new Tuple<CustomField, string>(result.CustomField, result.Value));
            CustomFieldInit.LoadCustomFieldValues(viewModel.CustomFields, viewModel.CustomFieldControls, values);
        }
    }
}
