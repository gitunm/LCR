using DevExpress.Mvvm.Native;
using DevExpress.Mvvm.POCO;
using LeftCenterRight.Command;
using LeftCenterRight.DataModel;
using LeftCenterRight.View;
using log4net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using static LeftCenterRight.ViewModel.ChartViewModel;
using static LeftCenterRight.ViewModel.MainViewModel;

namespace LeftCenterRight.ViewModel
{
    public class SimulatorInputViewModel : BaseViewModel, IDataErrorInfo
    {
        public SimulatorInputViewModel(BaseViewModel parent, ILog log)
            : base(parent)
        {
            Log = log;

            if (PresetGames != null)
            {
                PresetGames.Add(new PresetGame(3, 100));
                PresetGames.Add(new PresetGame(4, 100));
                PresetGames.Add(new PresetGame(5, 100));
                PresetGames.Add(new PresetGame(5, 1000));
                PresetGames.Add(new PresetGame(5, 10000));
                PresetGames.Add(new PresetGame(5, 100000));
                PresetGames.Add(new PresetGame(6, 100));
                PresetGames.Add(new PresetGame(7, 100));

                SelectedPresetGame = PresetGames.ElementAt(0);
            }
        }

        private PresetGame? _selectedPresetGame;
        public PresetGame? SelectedPresetGame
        {
            get => _selectedPresetGame;
            set
            {
                _selectedPresetGame = value;
                NumberOfPlayers = value?.NumberOfPlayers;
                NumberOfGames = value?.NumberOfGames;
            }
        }

        private ObservableCollection<PresetGame>? _presetGames;
        public ObservableCollection<PresetGame>? PresetGames => _presetGames ??= new ObservableCollection<PresetGame>();

        public int? _numberOfPlayers;
        [IntegerValidator(MinValue = 3, MaxValue = int.MaxValue)]
        public int? NumberOfPlayers
        {
            get => _numberOfPlayers;
            set
            {
                _numberOfPlayers = value;
                OnPropertyChanged();
            }
        }

        public int? _numberOfGames;
        [IntegerValidator(MinValue = 1, MaxValue = int.MaxValue)]
        public int? NumberOfGames
        {
            get => _numberOfGames;
            set
            {
                _numberOfGames = value;
                OnPropertyChanged();
            }
        }

        private ICommand? _playCommand;
        public ICommand? PlayCommand
        {
            get => _playCommand ??= new RelayCommand(p => (NumberOfPlayers >= 3 && NumberOfGames > 0), p => Play());
        }

        private CancellationTokenSource cancellationToken;
        private void Play()
        {
            Log.Info($"Play button has been pressed with parameters {NumberOfPlayers} Players x {NumberOfGames} Games");

            cancellationToken = new CancellationTokenSource();
            PlayHelper();
        }

        private async void PlayHelper()
        {
            LinkedList<Player> list;
            LinkedListNode<Player>? current;
            records?.Clear();


            await Task.Factory.StartNew(() =>
            {
                if (NumberOfPlayers != null)
                {
                    Players = new Player[NumberOfPlayers.Value];
                }


                for (int i = 0; i < NumberOfGames; i++)
                {
                    if(cancellationToken.IsCancellationRequested)
                    {
                        Log.Info("Play has been cancelled");
                        break; 
                    }

                    int turns = 0;

                    for (int x = 0; x < Players?.Length; x++)
                    {
                        Players[x] = new Player(x + 1, 3);
                    }

                    list = new LinkedList<Player>(Players);
                    current = list.First;

                    while (!GameOver())
                    {
                        turns++;
                        current = list?.First;

                        while (current != null && !GameOver())
                        {
                            RollDice(current);

                            current = current.Next;
                        }
                    }
                    Player? roundWinner = Players.Where(c => c.ChipCount > 0).ElementAt(0);
                    roundWinner.WinCount++;

                    App.Current.Dispatcher.Invoke(async () =>
                    {
                        records?.Add(new DataPoint(i + 1, turns));
                    });

                    //records?.Add(new DataPoint(i + 1, turns));
                }

                Player? finalWinner = Players?.MaxBy(c => c.WinCount);
                if (finalWinner != null)
                    finalWinner.IsWinner = true;

                ChartViewModel? chartView = ((MainViewModel)Parent).ChartVM;
                if (chartView != null && chartView.ChartDM != null)
                    chartView.ChartDM.Coordinates = records;

                PlayerViewModel? playerViewModel = ((MainViewModel)Parent).PlayerVM;
                if (playerViewModel != null)
                    playerViewModel.Players = Players.ToObservableCollection();
            }, cancellationToken.Token);
        }


        private Random _diceRoll;
        public Random DiceRoll => _diceRoll ??= new Random();

        ObservableCollection<DataPoint>? records = new ObservableCollection<DataPoint>();

        private Player[]? Players { get; set; }

        private bool GameOver()
        {
            return !(Players?.Where(c => c.ChipCount > 0).Count() > 1);
        }

        private void RollDice(LinkedListNode<Player> playerNode)
        {
            Player player = playerNode.Value;

            for (int i = 0; player.ChipCount > 0 && i < 3; i++)
            {
                int valRoll = DiceRoll.Next(1, 6);

                switch (valRoll)
                {
                    case (int)Die.Left:
                        player.ChipCount--;
                        LinkedListNode<Player>? previous = playerNode.Previous ?? playerNode.List?.Last;
                        if (previous != null)
                            previous.Value.ChipCount++;
                        break;
                    case (int)Die.Center:
                        player.ChipCount--;
                        break;
                    case (int)Die.Right:
                        player.ChipCount--;
                        LinkedListNode<Player>? next = playerNode.Next ?? playerNode.List?.First;
                        if (next != null)
                            next.Value.ChipCount++;
                        break;
                }
            }
        }


        private ICommand? _cancelCommand;
        public ICommand? CancelCommand
        {
            get => _cancelCommand ??= new RelayCommand(p => true, p => Cancel());
        }

        private void Cancel()
        {
            Log.Info("Cancel button has been pressed");

            if(cancellationToken != null)
                cancellationToken.Cancel();
        }



        public string Error { get => null; }

        public string this[string name]
        {
            get
            {
                string result = null;

                if (name == nameof(NumberOfPlayers))
                {
                    if (this._numberOfPlayers < 3)
                    {
                        result = "Number of Players must be > 3";
                    }
                }
                else if (name == nameof(NumberOfGames))
                {
                    result = "Number of Games must be > 0";
                }

                return result;
            }
        }
    }
}
