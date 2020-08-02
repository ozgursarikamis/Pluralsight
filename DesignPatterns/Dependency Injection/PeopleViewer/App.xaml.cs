using System.Windows;
using PeopleViewer.Presentation;
using PersonDataReader.CSV;
using PersonDataReader.Service;

namespace PeopleViewer
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ComposeObjects();
            if (Current.MainWindow == null) return;
            Current.MainWindow.Title = "With Dependency Injection";
            Current.MainWindow.Show();
        }

        private static void ComposeObjects()
        {
            var reader = new CSVReader();
            var viewModel = new PeopleViewModel(reader);
            Current.MainWindow = new PeopleViewerWindow(viewModel);
        }
    }
}
