using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using TrainSimulator;
using TrainSimulatorWPF.View.BottomPanel;
using TrainSimulatorWPF.View.LeftPanel;

namespace TrainSimulatorWPF.View.CenterPanel
{
    /// <summary>
    /// Interaction logic for MainScreenxaml.xaml
    /// </summary>
    /// 

    // Custom EventArgs
    public class TrainSelectedEventArgs : EventArgs
    {
        public Train SelectedTrain { get; }
        public TrainSelectedEventArgs(Train train) => SelectedTrain = train;
    }


    public partial class MainScreen : UserControl
    {


        public event EventHandler<TrainSelectedEventArgs> FirstStepDoneEvent;
        public ObservableCollection<string> ItemsList { get; set; }

        public string SelectedItem { get; set; }

        bool StationChoiceDone;
        bool ScheduleChoiceDone;
        bool TrainChoiceDone;

        List<Station> StationsList;
        public int UserStationChoiceIndex { get; private set; }
        List<Train> TrainsList;
        public int UserTrainChoiceIndex { get; private set; }
        List<Station> ScheduleList;
        public int UserScheduleChoiceIndex { get; private set; }

        bool TurnOn, PantOn, PwrOn, BrakeOff, ForwardOn, FirstStepDone;

        public Train SelectedTrain { get; set; } 
        public MainScreen()
        {
            InitializeComponent();
            ItemsList = new ObservableCollection<string>();
            DisplayChoiceTB.Text = "";
            DataContext = this;

            ResetUserChoiceData();

            StationsList = new List<Station>();
            TrainsList = new List<Train>();
          
      
            // subscribe to selection changed
            ListLB.SelectionChanged += ListLB_SelectionChanged;

            MenuTB.Text = "***Bienvenue dans cette simulation***\n";

            feedData();
            displayStationChoiceLogic();

        }

        public void ResetUserChoiceData()
        {
            StationChoiceDone = false;
            ScheduleChoiceDone = false;
            TrainChoiceDone = false;

            UserStationChoiceIndex = -1;
            UserTrainChoiceIndex = -1;
            UserScheduleChoiceIndex = -1;

            TurnOn = PantOn = PwrOn = BrakeOff = ForwardOn = FirstStepDone = false;
        }

        public void SubscribeToLeftPan(LeftPan leftPan)
        {
            leftPan.EnterBtnClicked += OnEnterBtnEvent;
            leftPan.UpBtnClicked += OnUpBtnEvent;
            leftPan.DownBtnClicked += OnDownBtnEvent;
            leftPan.StopBtnClicked += OnStopBtnEvent;
        }

        public void SubscribeToBottomPan(BottomPan bottomPanel)
        {

            bottomPanel.OnBtnCLicked += OnTurnOnBtnEvent;
            bottomPanel.ForwardBtnClicked += OnForwardBtnEvent;
            bottomPanel.PantBtnClicked += OnPantBtnEvent;
            bottomPanel.PwOnBtnClicked += OnPwrOnBtnEvent;
            bottomPanel.BrakeBtnClicked += OnBrakeBtnEvent;
        }

        //Left panel button event handler
        private void OnEnterBtnEvent(object? sender, EventArgs e)
        {
            if (!StationChoiceDone && UserStationChoiceIndex >= 0)
            {
                StationChoiceDone = true;
                displayScheduleChoiceLogic();
            }
            else if (!ScheduleChoiceDone && UserScheduleChoiceIndex >= 0)
            {
                ScheduleChoiceDone = true;
                displayTrainChoiceLogic();
            }
            else if (!TrainChoiceDone && UserTrainChoiceIndex >= 0)
            {

                TrainChoiceDone = true;
                // display the final summary
                printRecap(StationsList[UserStationChoiceIndex],
                          StationsList[UserStationChoiceIndex].ScheduleList[UserScheduleChoiceIndex],
                          TrainsList[UserTrainChoiceIndex]);

                
                UserTrainChoiceIndex = -2;
            }
            else if (TrainChoiceDone && UserTrainChoiceIndex == -2)
            {
                FirstStepDone = true; 
                displayStartUpProc();

                if (TurnOn && ForwardOn && PantOn && PwrOn && BrakeOff)
                {
                    OnMainScreenEvent();
                }
            }
        }

        private void OnUpBtnEvent(object? sender, EventArgs e)
        {
            // go to next step
        }

        private void OnDownBtnEvent(object? sender, EventArgs e)
        {
            // go to next step
        }

        private void OnStopBtnEvent(object? sender, EventArgs e)
        {
            // return from train choice (before the summary)
            if (TrainChoiceDone)
            {
                TrainChoiceDone = false;
                UserTrainChoiceIndex = -1;
                DisplayChoiceTB.Text = "";
                displayTrainChoiceLogic();
            }
            // return from schedule choice
            else if (ScheduleChoiceDone)
            {
                ScheduleChoiceDone = false;
                UserScheduleChoiceIndex = -1;
                DisplayChoiceTB.Text = "";
                displayScheduleChoiceLogic();
            }
            // return from station choice
            else if (StationChoiceDone)
            {
                StationChoiceDone = false;
                UserStationChoiceIndex = -1;
                DisplayChoiceTB.Text = "";
                displayStationChoiceLogic();
            }
            else
            {
                DisplayChoiceTB.Text = "Vous êtes déjà au menu principal.";
            }
        }

        protected virtual void OnMainScreenEvent()
        {
            FirstStepDoneEvent?.Invoke(this, new TrainSelectedEventArgs(SelectedTrain)); // notify subscribers 
        }

        //bottom panel button event handler
        private void OnTurnOnBtnEvent(object? sender, EventArgs e)
        {
            ResetSequence();
            TurnOn = true;
            if (FirstStepDone)
            {
                ListLB.SelectedItems.Add(ItemsList[0]);
                DisplayChoiceTB.Text = "";
            }
                
        }

        private void OnPantBtnEvent(object? sender, EventArgs e)
        {
            if (FirstStepDone && TurnOn && !PantOn)
            {
                PantOn = true;
                ListLB.SelectedItems.Add(ItemsList[1]);
            }
            else
            {
                ResetSequence();
            }
        }

        private void OnPwrOnBtnEvent(object? sender, EventArgs e)
        {
            if (FirstStepDone &&  TurnOn && PantOn && !PwrOn)
            {
                PwrOn = true;
                ListLB.SelectedItems.Add(ItemsList[2]);
            }
            else
            {
                ResetSequence();
            }
        }

        private void OnBrakeBtnEvent(object? sender, EventArgs e)
        {
            if (FirstStepDone && TurnOn && PantOn && PwrOn && !BrakeOff)
            {
                BrakeOff = true;
                ListLB.SelectedItems.Add(ItemsList[3]);

            }
            else
            {
                ResetSequence();
            }
        }

        private void OnForwardBtnEvent(object? sender, EventArgs e)
        {
            if (FirstStepDone && TurnOn && PantOn && PwrOn && BrakeOff && !ForwardOn)
            {
                ForwardOn = true;
                ListLB.SelectedItems.Add(ItemsList[4]);
                DisplayChoiceTB.Text = "Séquence complète et correcte !\nAppuyer sur Entrer pour démarrer.";
            }
            else
            {
                ResetSequence();
            }
        }

        private void ResetSequence()
        {
            if (FirstStepDone)
            {
                if(ListLB.SelectionMode == SelectionMode.Single)
                {
                    ListLB.SelectionMode = SelectionMode.Multiple;
                }
                ListLB.SelectedItems.Clear();
                TurnOn = ForwardOn = PantOn = PwrOn = BrakeOff = false;
                DisplayChoiceTB.Text = "Ordre incorrect, \nVeuillez recommencer";
            }

            
        }




        public void feedData()
        {
            // dataset 

            var trains = new List<Train> {
                new Train("TGV", "Thalys PBA", 300, 270, new DateTime(1996, 5, 1),
                                    377, 3, 200, 385000, 300000, 500000, new List<double>{ 500, 30, 5 }),

                                new Train("TGV", "Thalys PBKA", 300, 270, new DateTime(1997, 6, 15),
                                    377, 3, 200, 390000, 300000, 500000, new List<double>{ 500, 30, 5 }),

                                new Train("IC", "AM96", 160, 140, new DateTime(1996, 9, 1),
                                    280, 3, 75, 200000, 180000, 300000, new List<double>{ 400, 25, 4 }),

                                new Train("IC", "AM08 Desiro ML", 160, 140, new DateTime(2008, 3, 20),
                                    280, 3, 79, 210000, 200000, 320000, new List<double>{ 420, 28, 4 }),

                                new Train("SNCB", "AM75", 140, 120, new DateTime(1975, 10, 12),
                                    250, 3, 70, 180000, 160000, 280000, new List<double>{ 380, 22, 3 }),

                                new Train("SNCB", "HLE18 Siemens", 200, 160, new DateTime(2008, 11, 30),
                                    0, 3, 19, 90000, 250000, 400000, new List<double>{ 300, 18, 2 }),

                                new Train("Fret", "HLE13 Bombardier", 200, 160, new DateTime(1997, 1, 5),
                                    0, 3, 20, 95000, 260000, 420000, new List<double>{ 310, 20, 2 })
             };


            TrainsList = trains;

            // Schedules departing from Bruxelles-Midi
            var bruxellesMidiSchedules = new List<Schedule>
            {
                new Schedule("Bruxelles-Midi", "Bruxelles-Central", new DateTime(2023, 10, 1, 5, 15, 0), 4),
                new Schedule("Bruxelles-Midi", "Anvers-Central", new DateTime(2023, 10, 1, 7, 30, 0), 6),
                new Schedule("Bruxelles-Midi", "Gand-Saint-Pierre", new DateTime(2023, 10, 1, 9, 45, 0), 5),
                new Schedule("Bruxelles-Midi", "Liège-Guillemins", new DateTime(2023, 10, 1, 12, 0, 0), 7),
                new Schedule("Bruxelles-Midi", "Namur", new DateTime(2023, 10, 1, 14, 15, 0), 3)
            };

            // Schedules departing from Bruxelles-Central
            var bruxellesCentralSchedules = new List<Schedule>
            {
                new Schedule("Bruxelles-Central", "Mons", new DateTime(2023, 10, 1, 6, 0, 0), 2),
                new Schedule("Bruxelles-Central", "Ostende", new DateTime(2023, 10, 1, 8, 15, 0), 4),
                new Schedule("Bruxelles-Central", "Liège-Guillemins", new DateTime(2023, 10, 1, 10, 30, 0), 3),
                new Schedule("Bruxelles-Central", "Gand-Saint-Pierre", new DateTime(2023, 10, 1, 12, 45, 0), 5),
                new Schedule("Bruxelles-Central", "Anvers-Central", new DateTime(2023, 10, 1, 15, 0, 0), 6)
            };

            // Schedules departing from Anvers-Central
            var anversCentralSchedules = new List<Schedule>
            {
                new Schedule("Anvers-Central", "Bruxelles-Midi", new DateTime(2023, 10, 1, 6, 0, 0), 6),
                new Schedule("Anvers-Central", "Gand-Saint-Pierre", new DateTime(2023, 10, 1, 8, 15, 0), 5),
                new Schedule("Anvers-Central", "Liège-Guillemins", new DateTime(2023, 10, 1, 10, 30, 0), 3),
                new Schedule("Anvers-Central", "Ostende", new DateTime(2023, 10, 1, 12, 45, 0), 4),
                new Schedule("Anvers-Central", "Namur", new DateTime(2023, 10, 1, 15, 0, 0), 2)
            };

            // Schedules departing from Gand-Saint-Pierre
            var gandSchedules = new List<Schedule>
            {
                new Schedule("Gand-Saint-Pierre", "Bruxelles-Midi", new DateTime(2023, 10, 1, 6, 15, 0), 5),
                new Schedule("Gand-Saint-Pierre", "Bruxelles-Central", new DateTime(2023, 10, 1, 8, 30, 0), 4),
                new Schedule("Gand-Saint-Pierre", "Anvers-Central", new DateTime(2023, 10, 1, 10, 45, 0), 3),
                new Schedule("Gand-Saint-Pierre", "Liège-Guillemins", new DateTime(2023, 10, 1, 13, 0, 0), 6),
                new Schedule("Gand-Saint-Pierre", "Ostende", new DateTime(2023, 10, 1, 15, 15, 0), 4)
            };

            // Schedules departing from Liege-Guillemins
            var liegeSchedules = new List<Schedule>
            {
                new Schedule("Liège-Guillemins", "Namur", new DateTime(2023, 10, 1, 6, 45, 0), 5),
                new Schedule("Liège-Guillemins", "Bruxelles-Midi", new DateTime(2023, 10, 1, 9, 0, 0), 6),
                new Schedule("Liège-Guillemins", "Anvers-Central", new DateTime(2023, 10, 1, 11, 15, 0), 3),
                new Schedule("Liège-Guillemins", "Gand-Saint-Pierre", new DateTime(2023, 10, 1, 13, 30, 0), 4),
                new Schedule("Liège-Guillemins", "Ostende", new DateTime(2023, 10, 1, 15, 45, 0), 2)
            };

            // Schedules departing from Namur
            var namurSchedules = new List<Schedule>
            {
                new Schedule("Namur", "Bruxelles-Midi", new DateTime(2023, 10, 1, 7, 0, 0), 3),
                new Schedule("Namur", "Bruxelles-Central", new DateTime(2023, 10, 1, 9, 15, 0), 5),
                new Schedule("Namur", "Gembloux", new DateTime(2023, 10, 1, 11, 30, 0), 4),
                new Schedule("Namur", "Charleroi-Sud", new DateTime(2023, 10, 1, 13, 45, 0), 6),
                new Schedule("Namur", "Liège-Guillemins", new DateTime(2023, 10, 1, 16, 0, 0), 2)
            };

            // Schedules departing from Charleroi-Sud
            var charleroiSchedules = new List<Schedule>
            {
                new Schedule("Charleroi-Sud", "Bruxelles-Midi", new DateTime(2023, 10, 1, 7, 30, 0), 4),
                new Schedule("Charleroi-Sud", "Namur", new DateTime(2023, 10, 1, 9, 45, 0), 3),
                new Schedule("Charleroi-Sud", "Mons", new DateTime(2023, 10, 1, 12, 0, 0), 2),
                new Schedule("Charleroi-Sud", "Liège-Guillemins", new DateTime(2023, 10, 1, 14, 15, 0), 5),
                new Schedule("Charleroi-Sud", "Gand-Saint-Pierre", new DateTime(2023, 10, 1, 16, 30, 0), 4)
            };

            // Schedules departing from Ostende
            var gemblouxSchedules = new List<Schedule>
            {
                new Schedule("Gembloux", "Bruxelles-Midi", new DateTime(2023, 10, 1, 6, 0, 0), 3),
                new Schedule("Gembloux", "Namur", new DateTime(2023, 10, 1, 8, 15, 0), 2),
                new Schedule("Gembloux", "Ottignies", new DateTime(2023, 10, 1, 10, 30, 0), 4),
                new Schedule("Gembloux", "Louvain-La-Neuve", new DateTime(2023, 10, 1, 12, 45, 0), 5),
                new Schedule("OstGemblouxende", "Liège-Guillemins", new DateTime(2023, 10, 1, 15, 0, 0), 3)
            };

            // Schedules departing from Mons
            var monsSchedules = new List<Schedule>
            {
                new Schedule("Mons", "Bruxelles-Midi", new DateTime(2023, 10, 1, 6, 15, 0), 2),
                new Schedule("Mons", "Namur", new DateTime(2023, 10, 1, 8, 30, 0), 3),
                new Schedule("Mons", "Charleroi-Sud", new DateTime(2023, 10, 1, 10, 45, 0), 4),
                new Schedule("Mons", "Anvers-Central", new DateTime(2023, 10, 1, 13, 0, 0), 5),
                new Schedule("Mons", "Gand-Saint-Pierre", new DateTime(2023, 10, 1, 15, 15, 0), 2)
            };

            var stations = new List<Station>()
            {
                new Station("Bruxelles-Midi", "Bruxelles", 22, bruxellesMidiSchedules),
                new Station("Bruxelles-Central", "Bruxelles", 10, bruxellesCentralSchedules),
                new Station("Bruxelles-Nord", "Bruxelles", 12, anversCentralSchedules),
                new Station("Anvers-Central", "Anvers", 14, anversCentralSchedules), 
                new Station("Gembloux", "Gand", 12, gemblouxSchedules),
                new Station("Liège-Guillemins", "Liège", 9, liegeSchedules),
                new Station("Namur", "Namur", 8, namurSchedules),
                new Station("Charleroi-Sud", "Charleroi", 9, charleroiSchedules),
                new Station("Ostende", "Ostende", 8, gandSchedules),
                new Station("Mons", "Mons", 6, monsSchedules)
            };

            StationsList = stations;
        }


        public void displayStationChoiceLogic()
        {
            QuestionTB.Text = "Veuillez choisir une gare de départ parmi la liste ci-dessous";
            if (!ListLB.IsHitTestVisible)
            {
                resetListSelection();
            }
            feedStationToItemsList(StationsList);
            ListLB.SelectedIndex = -1; // no selection at the start
            UserStationChoiceIndex = -1;
        }

        public void displayScheduleChoiceLogic()
        {
            QuestionTB.Text = "Veuillez choisir un trajet.";
            if (!ListLB.IsHitTestVisible)
            {
                resetListSelection();
            }
            feedSchedulesToItemsList(StationsList[UserStationChoiceIndex].ScheduleList);
            ListLB.SelectedIndex = -1; // no selection at the start
            UserScheduleChoiceIndex = -1;
        }

        public void displayTrainChoiceLogic()
        {
            QuestionTB.Text = "Veuillez choisir un train.";
            if (!ListLB.IsHitTestVisible)
            {
                resetListSelection();
            }
                feedTrainToItemsList(TrainsList);
            ListLB.SelectedIndex = -1;
            UserTrainChoiceIndex = -1;
        }
        public void resetListSelection()
        {
            
                ListLB.SelectionMode = SelectionMode.Single; // back to single selection
                ListLB.IsHitTestVisible = true; // allow selection
           
        }

        public void displayStartUpProc()
        {
            ItemsList.Clear();
            DisplayChoiceTB.Text = "Veuillez actionner les boutons dans l'ordre\ndu processus de démarrage du train\n";

            QuestionTB.Text = "Processus de démarrage";
            ListLB.IsHitTestVisible = false;
            ItemsList.Add("1. Activer la cabine");
            ItemsList.Add("2. Lever le pantographe");
            ItemsList.Add("3. Mettre le train sous tension");
            ItemsList.Add("4. Desérrer le frein de stationnement");
            ItemsList.Add("5. Pousser le selectionneur de direction en avant");

            ListLB.SelectionMode = SelectionMode.Extended; // allow multiple selection

            /*
             ListLB.SelectedItems.Clear();
            ListLB.SelectedItems.Add(ItemsList[0]);
            ListLB.SelectedItems.Add(ItemsList[2]);
             */

        }

        private void printRecap(Station from, Schedule schedule, Train train)
        {

            DisplayChoiceTB.Text = $"Feuille de route:\n\n" +
                                  $"Gare: {from.StationName}\n" +
                                  $"Horaire: {schedule.ToString()}\n" +
                                  $"Train: {train.Name}";
        }


        private void ListLB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListLB.SelectedIndex < 0) return;

            string selected = ListLB.SelectedItem.ToString();

            if (!StationChoiceDone)
            {
                UserStationChoiceIndex = ListLB.SelectedIndex;
                DisplayChoiceTB.Text = $"Vous avez choisi la gare de {selected}";
            }
            else if (!ScheduleChoiceDone)
            {
                UserScheduleChoiceIndex = ListLB.SelectedIndex;
                DisplayChoiceTB.Text = $"Vous avez choisi l'horaire\n{selected}";
            }
            else if (!TrainChoiceDone)
            {
                UserTrainChoiceIndex = ListLB.SelectedIndex;
                SelectedTrain = TrainsList[UserTrainChoiceIndex];
                DisplayChoiceTB.Text = $"{TrainsList[UserTrainChoiceIndex].ToString()}";
            }
        }


  
        private void feedStationToItemsList(List<Station> list)
        {
            ItemsList.Clear();

            foreach (Station s in list)
            {
                ItemsList.Add(s.StationName);

            }
        }

        private void feedSchedulesToItemsList(List<Schedule> list)
        {
            ItemsList.Clear();

            foreach (Schedule s in list)
            {
                ItemsList.Add(s.ToString());

            }
        }

        private void feedTrainToItemsList(List<Train> list)
        {
            ItemsList.Clear();

            foreach (Train t in list)
            {
                ItemsList.Add(t.Name);

            }
        }

    }
}