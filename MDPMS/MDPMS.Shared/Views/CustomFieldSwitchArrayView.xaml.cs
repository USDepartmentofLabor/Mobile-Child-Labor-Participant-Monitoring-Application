﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDPMS.Shared.ViewModels;
using Xamarin.Forms;

namespace MDPMS.Shared.Views
{
    public partial class CustomFieldSwitchArrayView : ContentView
    {
        public CustomFieldSwitchArrayView()
        {
            InitializeComponent();
        }

        public List<Tuple<string, bool>> GetValues()
        {
            var rtn = new List<Tuple<string, bool>>();
            var vm = (CustomFieldSwitchArrayViewModel)BindingContext;
            for (var i = 0; i < ContentArea.Children.Count; i++)
            {
                var value = ((GenericSwitchTextViewModel)((GenericSwitchTextView)ContentArea.Children[i]).BindingContext).BoolValue;
                rtn.Add(new Tuple<string, bool>(vm.Content[i].Item2,value));
            }
            return rtn;
        }

        public string GetSelectedValues()
        {
            var rtn = new StringBuilder();
            var values = GetValues().Where(a => a.Item2).ToList();
            for (var i = 0; i < values.Count; i++)
            {
                if (i == (values.Count - 1)) rtn.Append(values[i].Item1);
                else rtn.AppendLine(values[i].Item1);
            }
            return rtn.ToString();
        }

        public void OnAppearing()
        {
            var vm = (CustomFieldSwitchArrayViewModel)BindingContext;
            ContentArea.Children.Clear();
            foreach (var content in vm.Content)
            {
                var switchControl = new GenericSwitchTextView
                {
                    BindingContext = new GenericSwitchTextViewModel(content.Item2)
                };
                ContentArea.Children.Add(switchControl);
            }
        }
    }
}