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
using TrainSimulatorWPF.View.CenterPanel;

namespace TrainSimulatorWPF.View.LeftPanel
{
    /// <summary>
    /// Logique d'interaction pour LeftPan.xaml
    /// </summary>
    public partial class LeftPan : UserControl
    {
        public event EventHandler? EnterBtnClicked; //other components will subscribe to this event, ? because EventHandler can be null if no one subscribed
        public event EventHandler? UpBtnClicked;
        public event EventHandler? DownBtnClicked;
        public event EventHandler? StopBtnClicked;

        public LeftPan()
        {
            InitializeComponent();
        }

        private void UpBtn_Click(object sender, RoutedEventArgs e)
        {
            OnUpBtnClicked(); 
        }

        //Callback when enter button is clicked
        private void DwnBtn_Click(object sender, RoutedEventArgs e)
        {
            OnDownBtnClicked();
        }

        private void EnterBtn_Click(object sender, RoutedEventArgs e)
        {
            OnEnterBtnEvent(); 
        }

        private void StopBtn_Click(object sender, RoutedEventArgs e)
        {
            OnStopBtnClicked(); 
        }

        protected virtual void OnEnterBtnEvent()
        {
            EnterBtnClicked?.Invoke(this, EventArgs.Empty); // notify subscribers 
        }

        protected virtual void OnUpBtnClicked()
        {
            UpBtnClicked?.Invoke(this, EventArgs.Empty); // notify subscribers 
        }

        protected virtual void OnDownBtnClicked()
        {
            DownBtnClicked?.Invoke(this, EventArgs.Empty); // notify subscribers 
        }

        protected virtual void OnStopBtnClicked()
        {
            StopBtnClicked?.Invoke(this, EventArgs.Empty); // notify subscribers 
        }

    }
}
