using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace ZXC
{
    public class ZXCWindow : Window
    {
        [STAThread]
        public static void Main()
        {
            Application app = new Application();
            app.Run(new ZXCWindow());
        }

        public ZXCWindow()
        {
            Title = "Птичьки";

            Button CreateBirdButton = new Button();
            CreateBirdButton.HorizontalAlignment = HorizontalAlignment.Center;
            CreateBirdButton.VerticalAlignment = VerticalAlignment.Center;
            CreateBirdButton.Content = "Птичька лететь";
            CreateBirdButton.Click += CreateBirdButtonOnClick;

            Content = CreateBirdButton;


        }

        void CreateBirdButtonOnClick(object sender, RoutedEventArgs args)
        {
            try
            {
                Ptichka b = new Ptichka();
                List<Point> path = b.PathData();

                StreamReader reader = new StreamReader("input.txt");
                List<Point> pr = new List<Point>
                {
                new Point(30, 0),
                new Point(30, 15),
                new Point(35, 15),
                new Point(35, 0)
                };
                NGon rect = new NGon(pr);

                b.SetPath(Convert.ToDouble(reader.ReadLine()), Convert.ToDouble(reader.ReadLine()));

                int res = b.IsCollision(rect);
                if (res == -1)
                    MessageBox.Show("Птичька лететь без столкновений", "Good fly");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Crash");
            }
        }
    }
}