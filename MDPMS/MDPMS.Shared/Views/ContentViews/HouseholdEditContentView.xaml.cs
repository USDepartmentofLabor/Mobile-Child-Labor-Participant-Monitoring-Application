using System;
using System.Collections.Generic;
using System.Linq;
using MDPMS.Database.Data.Models;
using MDPMS.Shared.ViewModels.ContentViewModels;
using MDPMS.Shared.ViewModels.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MDPMS.Shared.Views.ContentViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HouseholdEditContentView : ContentView
    {
        public HouseholdEditContentView()
        {
            InitializeComponent();
        }

        public void OnAppearing(bool loadValues)
        {
            // load custom field controls
            var viewModel = (HouseholdEditContentViewModel)BindingContext;
            CustomFieldContent.Children.Clear();
            foreach (var customFieldControl in viewModel.CustomFieldControls)
            {
                CustomFieldContent.Children.Add(customFieldControl);
            }

            if (!loadValues) return;

            // query values (optional)
            var values = new List<Tuple<CustomField, string>>();
            var query = viewModel.ApplicationInstanceData.Data.CustomHouseholdValues.Where(a => a.Household.InternalId == viewModel.Household.InternalId);
            foreach (var result in query) values.Add(new Tuple<CustomField, string>(result.CustomField, result.Value));
            CustomFieldInit.LoadCustomFieldValues(viewModel.CustomFields, viewModel.CustomFieldControls, values);
        }
    }
}
