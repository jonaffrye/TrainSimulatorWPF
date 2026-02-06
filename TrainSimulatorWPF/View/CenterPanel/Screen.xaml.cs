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
using System.Windows.Threading;
using TrainSimulator;
using TrainSimulatorWPF.View.LeftPanel;
using TrainSimulatorWPF.View.RightPanel;

namespace TrainSimulatorWPF.View.CenterPanel
{
    /// <summary>
    /// Logique d'interaction pour Screen.xaml
    /// </summary>
    public partial class Screen : UserControl
    {
        MainScreen MainScreen;
        Train SelectedTrain;
        DispatcherTimer timer;
        double currentThrottleValue = 0.0; 
        DateTime simulationStartTime; // pour calculer le temps écoulé
        private const double DELTA_TIME = 0.05; // pour le pas de temps

        public Screen()
        {
            InitializeComponent();
            InitializeScreen();
            InitializeView();
        }

        public void InitializeView()
        {
            TB_Brake.Text = "0%";
            TB_Traction.Text = "0%";
            TB_Clock.Text = "00:00";
            TB_SpeedValue.Text = "0";
            TB_PowerValue.Text = "0";
            TB_DistanceValue.Text = "0";
        }

        public void InitializeTimer()
        {
            // timer pour la simulation (50 ms ≈ 20 Hz)
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(50);
            timer.Tick += Timer_Tick; // abonnement à l'événement Tick
            simulationStartTime = DateTime.Now;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (SelectedTrain != null)
            {
                // mettre à jour la vitesse et la position en continu, même si le slider n'a pas bougé
                SelectedTrain.UpdateVelocity(DELTA_TIME, currentThrottleValue);
                SelectedTrain.UpdatePosition(DELTA_TIME);

                UpdateDisplay();
            }
        }

        private void UpdateDisplay()
        {
            if (SelectedTrain == null) return;

          
            if (currentThrottleValue > 0)
            {
                // mode traction (accélération)
                TB_Traction.Text = "Traction: " + ((currentThrottleValue / 3.0) * 100).ToString("F0") + "%";
                TB_Brake.Text = "Brake: 0%";
            }
            else if (currentThrottleValue < 0)
            {
                // mode freinage (décélération)
                TB_Brake.Text = "Brake: " + ((Math.Abs(currentThrottleValue) / 3.0) * 100).ToString("F0") + "%";
                TB_Traction.Text = "Traction: 0%";
            }
            else
            {
                // mode neutre
                TB_Traction.Text = "Traction: 0%";
                TB_Brake.Text = "Brake: 0%";
            }

         
            TimeSpan elapsed = DateTime.Now - simulationStartTime;
            TB_Clock.Text = elapsed.ToString(@"mm\:ss");

            
            double speedKmh = SelectedTrain.Velocity * 3.6; // conversion m/s vers km/h
            
            speedKmh = Math.Max(0, speedKmh); // eviter une vitesse négative
            TB_SpeedValue.Text = speedKmh.ToString("F1");

            // calcul de la puissance (P = F * v)
            // force approximative basée sur la valeur du throttle
            double approximateForce = currentThrottleValue * 10000; 
            double power = Math.Abs(approximateForce * SelectedTrain.Velocity) / 1000;
            TB_PowerValue.Text = power.ToString("F0");


            TB_DistanceValue.Text = SelectedTrain.Position.ToString("F0");

   
        }

        public void SubscribeToLeftPan(LeftPan leftPan)
        {
            leftPan.EnterBtnClicked += OnEnterBtnEvent;
            leftPan.StopBtnClicked += OnStopBtnEvent;
        }

        public void SubscribeToMainScreen(MainScreen mainScreen)
        {
            mainScreen.FirstStepDoneEvent += OnMainScreenEvent;
        }

        public void SubscribeToRightPan(RightPan rightPanel)
        {
            rightPanel.SliderChange += OnSliderChange;
        }

        private void OnSliderChange(object? sender, EventArgs e)
        {
            if (sender is RightPan rightPan && SelectedTrain != null)
            {
            
                currentThrottleValue = rightPan.SliderControl.Value;

      
            }
        }

        private void OnMainScreenEvent(object? sender, EventArgs e)
        {
            if (sender is MainScreen mainScreen)
            {
                MainScreen = mainScreen;
                SelectedTrain = mainScreen.SelectedTrain; 

    
                if (SelectedTrain != null)
                {
                    InitializeTimer();
                }
                

                mainScreen.Visibility = Visibility.Collapsed;
                SndScreen.Visibility = Visibility.Visible;
            }
        }

        private void OnEnterBtnEvent(object? sender, EventArgs e)
        {
            ShowAllElements();
        }

        private void OnStopBtnEvent(object? sender, EventArgs e)
        {
            HideAllElements();

            // arrêter la simulation
            if (timer != null)
            {
                timer.Stop();
            }

            // Reset des valeurs
            currentThrottleValue = 0.0;

          

            if (sender is LeftPan leftPan && MainScreen != null)
            {
                MainScreen.Visibility = Visibility.Visible;
                MainScreen.ResetUserChoiceData();
                MainScreen.displayStationChoiceLogic();
            }
        }

        public void InitializeScreen()
        {
            HideAllElements();
        }

        public void ShowAllElements()
        {
            Ellipse_SpeedDial.Visibility = Visibility.Visible;
            TB_SpeedValue.Visibility = Visibility.Visible;
            TB_SpeedUnit.Visibility = Visibility.Visible;
            TB_Status.Visibility = Visibility.Visible;
            TB_Param.Visibility = Visibility.Visible;
            TB_Spec.Visibility = Visibility.Visible;
            TB_Override.Visibility = Visibility.Visible;
            TB_Data.Visibility = Visibility.Visible;
            TB_Main.Visibility = Visibility.Visible;
            TB_PowerValue.Visibility = Visibility.Visible;
            TB_PowerUnit.Visibility = Visibility.Visible;
            TB_DistanceUnit.Visibility = Visibility.Visible;
            TB_DistanceValue.Visibility = Visibility.Visible;

            Rect_GaugeBackground.Visibility = Visibility.Visible;
            Rect_FuelLevel.Visibility = Visibility.Visible;
            Rect_WarningLine.Visibility = Visibility.Visible;
            TB_MaxGauge.Visibility = Visibility.Visible;
            TB_MinGauge.Visibility = Visibility.Visible;
            TB_MidGauge.Visibility = Visibility.Visible;
            TB_Traction.Visibility = Visibility.Visible;
            TB_Brake.Visibility = Visibility.Visible;
            TB_Regulator.Visibility = Visibility.Visible;
            TB_MaxSpeed.Visibility = Visibility.Visible;
            TB_MidSpeed.Visibility = Visibility.Visible;
            TB_UpArrow.Visibility = Visibility.Visible;
            TB_DownArrow.Visibility = Visibility.Visible;
            TB_Clock.Visibility = Visibility.Visible;
        }

   
        public void HideAllElements()
        {
            Ellipse_SpeedDial.Visibility = Visibility.Collapsed;
            TB_SpeedValue.Visibility = Visibility.Collapsed;
            TB_SpeedUnit.Visibility = Visibility.Collapsed;
            TB_Status.Visibility = Visibility.Collapsed;
            TB_Param.Visibility = Visibility.Collapsed;
            TB_Spec.Visibility = Visibility.Collapsed;
            TB_Override.Visibility = Visibility.Collapsed;
            TB_Data.Visibility = Visibility.Collapsed;
            TB_Main.Visibility = Visibility.Collapsed;
            TB_PowerValue.Visibility = Visibility.Collapsed;
            TB_PowerUnit.Visibility = Visibility.Collapsed;
            TB_DistanceUnit.Visibility = Visibility.Collapsed;
            TB_DistanceValue.Visibility = Visibility.Collapsed;

            Rect_GaugeBackground.Visibility = Visibility.Collapsed;
            Rect_FuelLevel.Visibility = Visibility.Collapsed;
            Rect_WarningLine.Visibility = Visibility.Collapsed;
            TB_MaxGauge.Visibility = Visibility.Collapsed;
            TB_MinGauge.Visibility = Visibility.Collapsed;
            TB_MidGauge.Visibility = Visibility.Collapsed;
            TB_Traction.Visibility = Visibility.Collapsed;
            TB_Brake.Visibility = Visibility.Collapsed;
            TB_Regulator.Visibility = Visibility.Collapsed;
            TB_MaxSpeed.Visibility = Visibility.Collapsed;
            TB_MidSpeed.Visibility = Visibility.Collapsed;
            TB_UpArrow.Visibility = Visibility.Collapsed;
            TB_DownArrow.Visibility = Visibility.Collapsed;
            TB_Clock.Visibility = Visibility.Collapsed;
        }
    }
}