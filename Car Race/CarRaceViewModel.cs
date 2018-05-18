using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;
/// ===============================
///  AUTHOR 
///  Mykyta Shvets
/// ===============================
namespace Car_Race
{
    public class CarRaceViewModel : INotifyPropertyChanged
    {
        public const int PLAYER_START_SPEED = 5;
        public const int AI_START_SPEED = 1;
        //public const int LEADERS_SIZE = 5;

        int indexOfHighest = -1;
        public Car Player { get; set; }
        public Car Ai { get; set; }
        public Car Ai2 { get; set; }
        public bool IsHihgestScore { get; set; }
        Random rnd = new Random();

        string name;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }

        ObservableCollection<Leader> leaders;
        public ObservableCollection<Leader> Leaders
        {
            get => leaders;
            set
            {
                leaders = value;
                OnPropertyChanged();
            }
        }

        int score;
        public int Score
        {
            get => score;
            set
            {
                score = value;
                OnPropertyChanged();
            }
        }

        public CarRaceViewModel()
        {
            Player = new Car("greenMcLaren", 1337);
            Ai = new Car("TruckBlue", 2049);
            Ai2 = new Car("TruckWhite", 1991);
        }

        public void gameStart()
        {
            score = 0;
            Ai.Speed = AI_START_SPEED;
            Ai2.Speed = AI_START_SPEED;
            Player.Speed = PLAYER_START_SPEED;
            LoadJson();
            IsHihgestScore = false;
        }

    
        public void gameOver()
        {
            getHighestIndex();
            if (indexOfHighest != -1)
            { 
                IsHihgestScore = true;
            }
        }

        //get position of element, where new result will be inserted. If score is not highest - position is -1.
        void getHighestIndex()
        {
            int tempScore = 0;
            indexOfHighest = -1;
            foreach (Leader leader in Leaders)
            {
                if (leader.Score < score && leader.Score >= tempScore)
                {
                    indexOfHighest = Leaders.IndexOf(leader);
                    tempScore = leader.Score;
                }
            }
        }

        public void updateLeaders()
        {
            //add new leader
            Leaders.Insert(indexOfHighest, new Leader(Name, score));
            //and remove the last in list
            if (Leaders.Count > 5) { 
                Leaders.RemoveAt(Leaders.Count-1);
            }
            //overwrite json file
            FileWriter<Leader> fileWriter = new FileWriter<Leader>();
            string fileName = Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.ApplicationData), "leaders.json");
            fileWriter.Write(fileName, Leaders);
        }

        public void LoadJson()
        {
            FileReader<Leader> fileReader = new FileReader<Leader>();
            string fileName = Path.Combine(Environment.GetFolderPath(
              Environment.SpecialFolder.ApplicationData), "leaders.json");
            try
            {
                Leaders = new ObservableCollection<Leader>(fileReader.readToList(fileName));
            } catch (ArgumentNullException e)
            {
                List<Leader> emptyLeaders = new List<Leader>();
                emptyLeaders.Add(new Leader(Name, 0));
                leaders = new ObservableCollection<Leader>(emptyLeaders);
            }
        }

        public void update()
        {
            Score += (Player.Speed / Car.SPEED_OFFSET);
        }

        public void updateAi(Car ai)
        {
            ai.Speed = rnd.Next(AI_START_SPEED + PLAYER_START_SPEED, ai.Speed + PLAYER_START_SPEED);
            int randomCar = rnd.Next(0, 9);
            switch (randomCar)
            {
                case 0:
                    ai.Make = "redFerrari";
                    break;
                case 1:
                    ai.Make = "orangeSubaru";
                    break;
                case 2:
                    ai.Make = "pinkDodge";
                    break;
                case 3:
                    ai.Make = "greenMcLaren";
                    break;
                case 4:
                    ai.Make = "grayLamborghini";
                    break;
                case 5:
                    ai.Make = "yellowPorsche";
                    break;
                case 6:
                    ai.Make = "ambulance";
                    break;
                case 7:
                    ai.Make = "TruckBlue";
                    break;
                case 8:
                    ai.Make = "TruckWhite";
                    break;
            }
            ai._Image.Source = new BitmapImage(new Uri("pack://application:,,,/Images/" + ai.Make + ".png"));
        }

        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        #endregion
    }

}
