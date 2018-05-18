using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using WpfAnimatedGif;

namespace Car_Race
{
    public partial class MainWindow : Window
    {
        CarRaceViewModel viewModel = new CarRaceViewModel();

        double roadOffsetY, road2OffsetY, playerOffsetX, aiOffsetY, ai2OffsetY;
        Image player, ai, ai2;
        Random rnd = new Random();
        DispatcherTimer timer;
        List<Key> keys;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = viewModel;

            keys = new List<Key>();

            player = viewModel.Player._Image;
            ai = viewModel.Ai._Image;
            ai2 = viewModel.Ai2._Image;
            
            RoadCanvas.Children.Add(player);
            RoadCanvas.Children.Add(ai);
            RoadCanvas.Children.Add(ai2);

            player.Visibility = Visibility.Hidden;
            ai.Visibility = Visibility.Hidden;
            ai2.Visibility = Visibility.Hidden;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BtnStart.IsEnabled = false;
            BtnAccelerate.IsEnabled = true;
            BtnBrake.IsEnabled = true;
            Explosion.Visibility = Visibility.Hidden;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1);
            timer.Tick += new EventHandler(Timer_Tick);
            timer.Start();

            viewModel.gameStart();
            keys.Clear();

            Canvas.SetLeft(player, (RoadCanvas.ActualWidth - player.ActualWidth) / 2);
            Canvas.SetTop(player, RoadCanvas.ActualHeight - player.ActualHeight - player.ActualHeight/4);
            playerOffsetX = Canvas.GetLeft(player);

            resetAi1();
            resetAi2();

            player.Visibility = Visibility.Visible;
            ai.Visibility = Visibility.Visible;
            ai2.Visibility = Visibility.Visible;

            roadOffsetY = 0;
            road2OffsetY = -road2.Height;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            viewModel.Player.accelerate();
            viewModel.Ai.accelerate();
            viewModel.Ai2.accelerate();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            About about = new About();
            about.ShowDialog();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        { 
            Leaders leaders = new Leaders(viewModel);
            viewModel.LoadJson();
            leaders.ShowDialog();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            viewModel.Player.brake();
            viewModel.Ai.brake();
            viewModel.Ai2.brake();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Left || e.Key == Key.A || e.Key == Key.Right || e.Key == Key.D) && (!keys.Contains(e.Key)))
            {
                keys.Add(e.Key);
            }
            //immitation of 'Accelerate' button click
            if(e.Key == Key.Up || e.Key == Key.W)
            {
                Button_Click_1(sender, e);
            }
            //immitation of 'Brake' button click
            if(e.Key == Key.Down || e.Key == Key.S)
            {
                Button_Click_2(sender, e);
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Left || e.Key == Key.A || e.Key == Key.Right || e.Key == Key.D) && (keys.Contains(e.Key)))
            {  
                keys.Remove(e.Key);
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            viewModel.update();

            if ((keys.Contains(Key.Left) || keys.Contains(Key.A)) && Canvas.GetLeft(player) > viewModel.Player.Speed)
            {
                player.SetValue(Canvas.LeftProperty, playerOffsetX -= Car.SPEED_OFFSET);//= viewModel.Player.Speed);
            }
            if ((keys.Contains(Key.Right) || keys.Contains(Key.D)) && Canvas.GetLeft(player) < RoadCanvas.ActualWidth - player.ActualWidth - viewModel.Player.Speed)
            {
                player.SetValue(Canvas.LeftProperty, playerOffsetX += Car.SPEED_OFFSET);//= viewModel.Player.Speed);
            }
            //if interract with other car (game over)
            if ((Canvas.GetLeft(player) <= (Canvas.GetLeft(ai) + ai.ActualWidth) && 
                Canvas.GetTop(player) <= Canvas.GetTop(ai) + ai.ActualHeight && 
                Canvas.GetLeft(player) + player.ActualWidth >= Canvas.GetLeft(ai) &&
                Canvas.GetTop(player) + player.ActualHeight >= Canvas.GetTop(ai)) || 
                (Canvas.GetLeft(player) <= (Canvas.GetLeft(ai2) + ai2.ActualWidth) && 
                Canvas.GetTop(player) <= Canvas.GetTop(ai2) + ai2.ActualHeight && 
                Canvas.GetLeft(player) + player.ActualWidth >= Canvas.GetLeft(ai2) &&
                Canvas.GetTop(player) + player.ActualHeight >= Canvas.GetTop(ai2)))
            {
                timer.Stop();
                BtnStart.IsEnabled = true;
                BtnBrake.IsEnabled = false;
                BtnAccelerate.IsEnabled = false;

                viewModel.gameOver();

                if(viewModel.IsHihgestScore)
                {
                    HighScore highScore = new HighScore(viewModel);
                    highScore.ShowDialog();
                }
                //set exploison coordinate
                Canvas.SetLeft(Explosion, Canvas.GetLeft(player));

                //animated explosion
                var image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri("pack://application:,,,/Images/explosion.gif");
                image.EndInit();
                ImageBehavior.SetAnimatedSource(Explosion, image);

                //bring to front
                Panel.SetZIndex(Explosion, 1);
                Explosion.Visibility = Visibility.Visible;
            }

            road.SetValue(Canvas.TopProperty, roadOffsetY += viewModel.Player.Speed);
            road2.SetValue(Canvas.TopProperty, road2OffsetY += viewModel.Player.Speed);

            ai.SetValue(Canvas.TopProperty, aiOffsetY += viewModel.Ai.Speed);
            ai2.SetValue(Canvas.TopProperty, ai2OffsetY += viewModel.Ai2.Speed);

            //reset ai
            if (Canvas.GetTop(ai) - ai.ActualHeight > RoadCanvas.ActualWidth)
            {
                resetAi1();
            }

            //reset ai2
            if (Canvas.GetTop(ai2) - ai2.ActualHeight > RoadCanvas.ActualWidth)
            {
                resetAi2();
            }

            if (roadOffsetY > road.Height)
            {
                roadOffsetY = 0;
            }

            if (road2OffsetY > 0)
            {
                road2OffsetY = -road2.Height;
            }
        }

        void resetAi1()
        {
            Canvas.SetTop(ai, rnd.Next((int)ai.ActualHeight, (int)ai.ActualHeight * 2) * -1);
            Canvas.SetLeft(ai, rnd.Next(0, (int)RoadCanvas.ActualWidth / 2 - (int)ai.ActualWidth));
            aiOffsetY = Canvas.GetTop(ai);
            viewModel.updateAi(viewModel.Ai);
        }

        void resetAi2()
        {
            Canvas.SetTop(ai2, rnd.Next((int)ai.ActualHeight, (int)ai.ActualHeight * 2) * -1);
            Canvas.SetLeft(ai2, rnd.Next((int)RoadCanvas.ActualWidth / 2, (int)(RoadCanvas.ActualWidth - ai2.ActualWidth)));
            ai2OffsetY = Canvas.GetTop(ai2);
            viewModel.updateAi(viewModel.Ai2);
        }

    }
}
