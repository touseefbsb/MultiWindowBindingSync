using System;
using Microsoft.Toolkit.Uwp.Helpers;
using MultiWindowBindingSample.Models;
using MultiWindowBindingSample.Services;
using MultiWindowBindingSample.ViewModels;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace MultiWindowBindingSample.Views
{
    public sealed partial class PipNotePage : Page
    {
        public PipNoteViewModel ViewModel { get; } = new PipNoteViewModel();

        public PipNotePage()
        {
            InitializeComponent();
        }

        private ViewLifetimeControl _viewLifetimeControl;
        private TextBox parentBox;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var parameter = e.Parameter as Tuple<Note, ViewLifetimeControl, TextBox>;
            ViewModel.FillMyNote(parameter.Item1);
            _viewLifetimeControl = parameter.Item2;
            parentBox = parameter.Item3;
            parentBox.TextChanged += ParentBox_TextChanged;
            PipBox.TextChanged += PipBox_TextChanged; 
            _viewLifetimeControl.Released += OnViewLifetimeControlReleased;
        }
        private async void PipBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = PipBox.Text;
            await CoreApplication.MainView.Dispatcher.AwaitableRunAsync(() =>
            {
                if (parentBox.Text != text)
                    parentBox.Text = text;
            });
        }
        private async void ParentBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = parentBox.Text;
            await _viewLifetimeControl.Dispatcher.AwaitableRunAsync(() =>
            {
                if (ViewModel.MyNote.Description != text)
                    ViewModel.MyNote.Description = text;
            });
        }
        private async void OnViewLifetimeControlReleased(object sender, EventArgs e)
        {
            parentBox.TextChanged -= ParentBox_TextChanged;
            PipBox.TextChanged -= PipBox_TextChanged;
            _viewLifetimeControl.Released -= OnViewLifetimeControlReleased;
            await WindowManagerService.Current.MainDispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                WindowManagerService.Current.SecondaryViews.Remove(_viewLifetimeControl);
            });
        }
    }
}
