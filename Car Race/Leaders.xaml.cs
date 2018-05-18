using System.Windows;

namespace Car_Race
{
    public partial class Leaders : Window
    {
        public Leaders(CarRaceViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
