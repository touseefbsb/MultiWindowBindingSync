using System;
using MultiWindowBindingSample.Models;
using MultiWindowBindingSample.Services;
using MultiWindowBindingSample.ViewModels;
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var parameter = e.Parameter as Tuple<Note, ViewLifetimeControl>;
            ViewModel.FillMyNote(parameter.Item1);
            _viewLifetimeControl = parameter.Item2;
            _viewLifetimeControl.Released += OnViewLifetimeControlReleased;
        }

        private async void OnViewLifetimeControlReleased(object sender, EventArgs e)
        {
            _viewLifetimeControl.Released -= OnViewLifetimeControlReleased;
            await WindowManagerService.Current.MainDispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                WindowManagerService.Current.SecondaryViews.Remove(_viewLifetimeControl);
            });
        }
    }
}
