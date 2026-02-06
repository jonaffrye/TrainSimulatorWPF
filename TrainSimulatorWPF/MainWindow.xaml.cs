using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TrainSimulatorWPF.View.BottomPanel;
using TrainSimulatorWPF.View.CenterPanel;
using TrainSimulatorWPF.View.LeftPanel;

namespace TrainSimulatorWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SubscribePanels();
            SndScreen.Visibility = Visibility.Collapsed;
            
        }

        public void SubscribePanels()
        {
            SndScreen.SubscribeToLeftPan(LeftPanel);
            MainScreen.SubscribeToLeftPan(LeftPanel);
            SndScreen.SubscribeToMainScreen(MainScreen);
            MainScreen.SubscribeToBottomPan(BottomPanel);
            SndScreen.SubscribeToRightPan(RightPanel); 
        }
        private void Screen_Loaded(object sender, RoutedEventArgs e)
        {

        }

       
    }
}