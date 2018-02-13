using System.Linq;
using MDPMS.Shared.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MDPMS.Shared.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HouseholdMemberFollowUpView : ContentPage
	{
		public HouseholdMemberFollowUpView ()
		{
			InitializeComponent ();
		}

	    protected override void OnAppearing()
	    {
	        base.OnAppearing();

	        // get view model
	        var viewModel = (HouseholdMemberFollowUpViewModel)BindingContext;

	        DynamicWorkActivities.Children.Clear();
	        DynamicHazardousConditions.Children.Clear();
	        DynamicHouseholdTasks.Children.Clear();

	        // set dynamic content from the data in the vm
	        foreach (var bindableWorkActivity in viewModel.BindableWorkActivities)
	        {
	            DynamicWorkActivities.Children.Add(new GenericSwitchTextView
	            {
	                BindingContext = bindableWorkActivity.Item3,
	                MinimumHeightRequest = 65
	            });
	        }
	        foreach (var bindableHazardousCondition in viewModel.BindableHazardousConditions)
	        {
	            DynamicHazardousConditions.Children.Add(new GenericSwitchTextView
	            {
	                BindingContext = bindableHazardousCondition.Item3,
	                MinimumHeightRequest = 65
	            });
	        }
	        foreach (var bindableHouseholdTask in viewModel.BindableHouseholdTasks)
	        {
	            DynamicHouseholdTasks.Children.Add(new GenericSwitchTextView
	            {
	                BindingContext = bindableHouseholdTask.Item3,
	                MinimumHeightRequest = 65
	            });
	        }
	    }
    }
}