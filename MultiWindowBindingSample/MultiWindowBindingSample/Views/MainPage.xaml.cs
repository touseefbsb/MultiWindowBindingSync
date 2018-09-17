using System;
using MultiWindowBindingSample.Helpers;
using MultiWindowBindingSample.Models;
using MultiWindowBindingSample.Services;
using MultiWindowBindingSample.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MultiWindowBindingSample.Views
{
    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel { get; } = new MainViewModel();

        public MainPage()
        {
            InitializeComponent();
        }

        private async void OpenInNewWindowButtonClick(object sender, RoutedEventArgs e)
        {
            var note = (Note)((Button)sender).DataContext;
            //open note in secondary window
            await WindowManagerService.Current
                .TryShowAsViewModeAsync("AppDisplayName".GetLocalized(), typeof(PipNotePage), note);

        }
    }
}
