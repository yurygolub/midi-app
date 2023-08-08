using System;
using System.Windows.Controls;
using MidiApp.ViewModels;

namespace MidiApp.Views
{
    /// <summary>
    /// Interaction logic for MidiInView.xaml.
    /// </summary>
    public partial class MidiInView : UserControl
    {
        public MidiInView(MidiInViewModel viewModel)
        {
            this.ViewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            this.InitializeComponent();
        }

        public MidiInViewModel ViewModel { get; }
    }
}
