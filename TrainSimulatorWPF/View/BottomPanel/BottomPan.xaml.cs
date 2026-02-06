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

namespace TrainSimulatorWPF.View.BottomPanel
{
    /// <summary>
    /// Logic for BottomPan.xaml
    /// </summary>
    /// 

    public partial class BottomPan : UserControl
    {

        public event EventHandler? OnBtnCLicked;
        public event EventHandler? ForwardBtnClicked;
        public event EventHandler? PantBtnClicked;
        public event EventHandler? PwOnBtnClicked;
        public event EventHandler? BrakeBtnClicked;

        public BottomPan()
        {
            InitializeComponent();

        }

        private void AcqButton_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void AcqBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TurnOnBtn_Click(object sender, RoutedEventArgs e)
        {
            OnTurnOnBtnEvent();
        }

        private void ForwardBtn_Click(object sender, RoutedEventArgs e)
        {
            OnForwardBtnEvent();
        }

        private void PantBtn_Click(object sender, RoutedEventArgs e)
        {
            OnPantBtnEvent();
        }

        private void PowerOnBtn_Click(object sender, RoutedEventArgs e)
        {
            OnPwrOnBtnEvent();

        }

        private void BrakeBtn_Click(object sender, RoutedEventArgs e)
        {
            OnBrakeBtnEvent();
        }

        protected virtual void OnTurnOnBtnEvent()
        {
            OnBtnCLicked?.Invoke(this, EventArgs.Empty); 
        }

        protected virtual void OnForwardBtnEvent()
        {
            ForwardBtnClicked?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnPantBtnEvent()
        {
            PantBtnClicked?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnPwrOnBtnEvent()
        {
            PwOnBtnClicked?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnBrakeBtnEvent()
        {
            BrakeBtnClicked?.Invoke(this, EventArgs.Empty);
        }






    }
}
