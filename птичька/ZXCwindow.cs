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
            Title = "Птицевое";

            Button CreatePtichkaButton = new Button
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Content = "_Create a Bird and launch it!"
            };
            CreatePtichkaButton.Click += CreatePtichkaButtonOnClick;

            Content = CreatePtichkaButton;


        }

        void CreatePtichkaButtonOnClick(object sender, RoutedEventArgs args)
        {
            try
            {
                Ptichka p = new Ptichka();
                List<Point> path = p.PathData();
                StreamReader reader = new StreamReader("input.txt");
                List<Point> pr = new List<Point>
                {
                new Point(30, 0),
                new Point(30, 15),
                new Point(35, 15),
                new Point(35, 0)
                };
                NGon prya = new NGon(pr);

                p.SetPath(Convert.ToDouble(reader.ReadLine()), Convert.ToDouble(reader.ReadLine()));

                int res = p.IsCollision(prya);
                if (res == -1)
                    MessageBox.Show("Хороший запуски птичьки", "Congratulations");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Crash");
            }
        }
    }
}