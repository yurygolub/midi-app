﻿using System;
using System.Windows;
using System.Windows.Input;
using MidiApp.ViewModels;

namespace MidiApp.Views
{
    /// <summary>
    /// Interaction logic for AboutView.xaml.
    /// </summary>
    public partial class AboutView : Window
    {
        public AboutView(AboutViewModel viewModel)
        {
            this.ViewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            this.InitializeComponent();
        }

        public AboutViewModel ViewModel { get; }

        private void CloseCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
    }
}
