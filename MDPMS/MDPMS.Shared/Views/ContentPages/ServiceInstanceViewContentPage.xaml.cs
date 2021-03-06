﻿using MDPMS.Shared.ViewModels.ContentPageModels;
using MDPMS.Shared.ViewModels.ContentViewModels;
using MDPMS.Shared.Views.ContentViews;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MDPMS.Shared.Views.ContentPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ServiceInstanceViewContentPage : ContentPage
    {
        public ServiceInstanceViewContentPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var viewModel = (ServiceInstanceViewContentPageModel)BindingContext;
            var childView = new ServiceInstanceViewContentView();
            var childViewModel = new ServiceInstanceViewContentViewModel(viewModel.ApplicationInstanceData, viewModel.ServiceInstance);
            childView.BindingContext = childViewModel;
            this.ScrollView.Content = childView;
            //childView.OnAppearing();
        }
    }
}
