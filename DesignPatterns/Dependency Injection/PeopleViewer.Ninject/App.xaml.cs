﻿using System.Windows;
using Ninject;
using PeopleViewer.Common;
using PersonDataReader.CSV;
using PersonDataReader.Decorators;
using PersonDataReader.Service;

namespace PeopleViewer.Ninject
{
    public partial class App : Application
    {
        private readonly IKernel Container = new StandardKernel();
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ConfigureContainer();
            ComposeObjects();
            
            Current.MainWindow.Title = "With Dependency Injection - Ninject";
            Current.MainWindow.Show();
        }

        private void ConfigureContainer()
        {
            // Container.Bind<IPersonReader>().To<ServiceReader>();
            Container.Bind<IPersonReader>()
                .To<CachingReader>()
                .InSingletonScope() // Life-time management
                .WithConstructorArgument<IPersonReader>(Container.Get<ServiceReader>()); // Decorator
        }

        private void ComposeObjects()
        {
            Current.MainWindow = Container.Get<PeopleViewerWindow>();
        }
    }
}
