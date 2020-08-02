﻿using PeopleViewer.Presentation;
using System.Windows;

namespace PeopleViewer
{
    // ReSharper disable once RedundantExtendsListEntry
    public partial class PeopleViewerWindow : Window
    {
        private readonly PeopleViewModel viewModel;

        public PeopleViewerWindow(PeopleViewModel peopleViewModel)
        {
            InitializeComponent();
            viewModel = peopleViewModel;
            DataContext = viewModel;
        }

        private void FetchButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.RefreshPeople();
            ShowRepositoryType();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.ClearPeople();
            ClearRepositoryType();
        }

        private void ShowRepositoryType()
        {
            RepositoryTypeTextBlock.Text = viewModel.DataReaderType;
        }

        private void ClearRepositoryType()
        {
            RepositoryTypeTextBlock.Text = string.Empty;
        }
    }
}
