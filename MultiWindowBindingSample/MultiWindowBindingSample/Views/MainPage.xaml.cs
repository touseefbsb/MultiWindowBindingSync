using System;
using MultiWindowBindingSample.Helpers;
using MultiWindowBindingSample.Models;
using MultiWindowBindingSample.Services;
using MultiWindowBindingSample.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Toolkit.Uwp.UI.Extensions;
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
            var button = sender as Button;
            var note = button.DataContext as Note;
            var stack = button.FindParent<StackPanel>();
            var box = stack.FindChild<TextBox>();
            //open note in secondary window
            await WindowManagerService.Current
                .TryShowAsViewModeAsync("AppDisplayName".GetLocalized(), typeof(PipNotePage),
                new Note { Id = note.Id, Description = note.Description }, box);

        }
    }
}
