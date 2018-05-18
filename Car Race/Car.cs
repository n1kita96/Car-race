using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
/// ===============================
///  AUTHOR 
///  Mykyta Shvets
///  Goudamaraja Muruganandam
///  Charnpreet Kaur
///  Julene DiSanto
///  Sachin Ranga 
/// ===============================
namespace Car_Race
{
    public class Car : INotifyPropertyChanged
    {

        private int speed;
        public int Speed
        {
            get => speed;
            set
            {
                speed = value;
                OnPropertyChanged();
            }
        }

        public string Make { get; set; }
        public const int SPEED_OFFSET = 5;
        public const int MAX_SPEED = 50;
        //What do we need Year for? Think about it
        public int Year { get; set; }
        
        public Image _Image;

        public Car(string Make, int Year)
        {
            this.Make = Make;
            this.Year = Year;
            Speed = 0;
            _Image = new Image
            {
                Source = new BitmapImage(new Uri("pack://application:,,,/Images/" + Make + ".png"))
            };
        }

        public void accelerate()
        {
            if (Speed < MAX_SPEED)
            { 
                Speed += SPEED_OFFSET;
            }
        }

        public void brake()
        {
            if(Speed > SPEED_OFFSET)
            { 
                Speed -= SPEED_OFFSET;
            }
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
