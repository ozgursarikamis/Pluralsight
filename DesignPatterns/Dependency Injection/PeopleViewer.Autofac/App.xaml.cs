using System.Windows;
using Autofac;
using Autofac.Features.ResolveAnything;
using PeopleViewer.Common;
using PeopleViewer.Presentation;
using PersonDataReader.CSV;
using PersonDataReader.Decorators;
using PersonDataReader.Service;

namespace PeopleViewer.Autofac
{
    public partial class App : Application
    {
        private static IContainer Container;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ConfigureContainer();
            ComposeObjects();
            Application.Current.MainWindow.Title = "With Dependency Injection - Autofac";
            Application.Current.MainWindow.Show();
        }

        private static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<CSVReader>().As<IPersonReader>()
                .SingleInstance();

            // life-time notation:
            builder.RegisterType<PeopleViewerWindow>().InstancePerDependency();
            builder.RegisterType<PeopleViewModel>().InstancePerDependency();
            builder.RegisterType<ServiceReader>()
                .Named<IPersonReader>("reader") // defining decorator
                .As<IPersonReader>().SingleInstance();

            // builder is registering decorator:
            builder.RegisterDecorator<IPersonReader>((c, inner) =>
                new CachingReader(inner), fromKey: "reader");

            builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());

            Container = builder.Build();
        }

        private static void ComposeObjects()
        {
            Current.MainWindow = Container.Resolve<PeopleViewerWindow>();
        }
    }
}
