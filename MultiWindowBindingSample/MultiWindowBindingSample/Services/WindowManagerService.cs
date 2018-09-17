using MultiWindowBindingSample.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MultiWindowBindingSample.Services
{
    public delegate void ViewClosedHandler(ViewLifetimeControl viewControl, EventArgs e);

    // For instructions on testing this service see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/features/multiple-views.md
    // More details about showing multiple views at https://docs.microsoft.com/windows/uwp/design/layout/show-multiple-views
    public class WindowManagerService
    {
        private static WindowManagerService _current;

        public static WindowManagerService Current => _current ?? (_current = new WindowManagerService());

        // Contains all the opened secondary views.
        public ObservableCollection<ViewLifetimeControl> SecondaryViews { get; } = new ObservableCollection<ViewLifetimeControl>();

        public int MainViewId { get; private set; }

        public CoreDispatcher MainDispatcher { get; private set; }

        public void Initialize()
        {
            MainViewId = ApplicationView.GetForCurrentView().Id;
            MainDispatcher = Window.Current.Dispatcher;
        }

       

        // Displays a view in the specified view mode
        public async Task<ViewLifetimeControl> TryShowAsViewModeAsync(string windowTitle, Type pageType, Note note, ApplicationViewMode viewMode = ApplicationViewMode.Default)
        {
            ViewLifetimeControl viewControl = await CreateViewLifetimeControlAsync(windowTitle, note, pageType);
            SecondaryViews.Add(viewControl);
            viewControl.StartViewInUse();
            var viewShown = await ApplicationViewSwitcher.TryShowAsViewModeAsync(viewControl.Id, viewMode);
            viewControl.StopViewInUse();
            return viewControl;
        }

        private async Task<ViewLifetimeControl> CreateViewLifetimeControlAsync(string windowTitle, Note note, Type pageType)
        {
            ViewLifetimeControl viewControl = null;

            await CoreApplication.CreateNewView().Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                viewControl = ViewLifetimeControl.CreateForCurrentView();
                viewControl.Title = windowTitle;
                viewControl.StartViewInUse();
                var frame = new Frame();
                frame.RequestedTheme = ThemeSelectorService.Theme;
                var t = Tuple.Create<Note, ViewLifetimeControl>(note, viewControl);
                frame.Navigate(pageType, t);
                Window.Current.Content = frame;
                Window.Current.Activate();
                ApplicationView.GetForCurrentView().Title = viewControl.Title;
            });

            return viewControl;
        }

        public bool IsWindowOpen(string windowTitle) => SecondaryViews.Any(v => v.Title == windowTitle);
    }
}
