using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace MultiSender.Views
{

    public partial class MainWindow : Window
    {
        private ViewModels.LogicManager _logic;
        public MainWindow()
        {
            InitializeComponent();
            _logic = new ViewModels.LogicManager();
            this.DataContext = _logic.Bounded;
            _logic.AutorunIfHasCommandArg();
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            _logic.MassSend();
        }

        private void Config_Click(object sender, RoutedEventArgs e)
        {
            _logic.LaunchConfig();
        }
    }
}
