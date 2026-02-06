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

namespace TrainSimulatorWPF.View.RightPanel
{
    /// <summary>
    /// Logique d'interaction pour RightPan.xaml
    /// </summary>
    public partial class RightPan : UserControl
    {
        public event EventHandler SliderChange; 
        public RightPan()
        {
            InitializeComponent();
       
        }

        private void SliderControl_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            OnSliderChange(); 
        }

        protected virtual void OnSliderChange()
        {
            SliderChange?.Invoke(this, EventArgs.Empty); 
        }
    }
}
