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


    public class ZXCw
    {
        public static StreamReader reader = new StreamReader(@"..\..\input.txt");
        public static StreamWriter writer = new StreamWriter("letet.txt");

        [STAThread]
        private static void Main()
        {
            double a, v;

            a = Convert.ToDouble(reader.ReadLine());
            v = Convert.ToDouble(reader.ReadLine());
            reader.Close();

            Ptichka p = new Ptichka(a, v);
            p.SetPath(a, v);
            //p.ShowPath();

            List<Point> rec = new List<Point>
          {
            new Point(30, 0),
            new Point(30, 15),
            new Point(35, 15),
            new Point(35, 0)
          };


            NGon rect = new NGon(rec);


            p.OnCollision += p.CollisionMessage;

            int res = p.IsCollision(rect);

            writer.Close();
        }//END OF MAIN
    }
    class Point
    {
        public double X;
        public double Y;

        public Point(double X, double Y)
        {
            this.X = X;
            this.Y = Y;
        }
    }

    

    ////Base for every obstacle in this programm
    abstract class Obstacle
    { 
        public abstract bool Belongs(Point P);
    }


    //NGon(polygon) as an obstacle
    class NGon : Obstacle
    {
        //List of all vertices of the n-gon
        readonly List<Point> Vertices;

        ///Constructors:
        public NGon(List<Point> vertices)
        {
            Vertices = vertices;
        }
        ///

        //Shift a whole n-gon by vertical line
        public void VerticalShift(double shift)
        {
            int sz = Vertices.Count;
            for (int i = 0; i < sz; ++i)
                Vertices[i].Y += shift;
        }

        //Shift a whole n-gon by horizontal line
        public void HorizontalShift(double shift)
        {
            int sz = Vertices.Count;
            for (int i = 0; i < sz; ++i)
                Vertices[i].X += shift;
        }


        //Checks if the point P belongs to this n-gon
        //Compl.: O(count of vertices of this n-gon)
        public override bool Belongs(Point P)
        {
            int sz = Vertices.Count;
            double px = P.X, py = P.Y, m1x = Vertices[0].X, m1y = Vertices[0].Y, m2x, m2y;
            for (int i = 1; i < sz; ++i)
            {
                m2x = Vertices[i].X;
                m2y = Vertices[i].Y;

                if ((m2x - m1x) * (py - m1y) - (m2y - m1y) * (px - m1x) > 0) return false;

                m1x = m2x;
                m1y = m2y;
            }
            return true;
        }

        //Show vertices of n-gon
        public void Show()
        {
            int sz = Vertices.Count;
            string res = "";
            res += "Points: \n{\n";
            for (int i = 0; i < sz; ++i)
                res += String.Format("\n  ({0}, {1}),", Vertices[i].X, Vertices[i].Y);
            res = res.Substring(0, res.Length - 1);
            res += "\n}";
            ZXCw.writer.WriteLine(res);
        }
    }

   
    //It represents all data about 'bird' flight
    class Ptichka
    {
        double Angle { get; set; }
        double Velosity { get; set; }
        List<Point> PosList = new List<Point>();

        protected static double delta = 0.01;

        public Ptichka()
        { }

        public Ptichka(double angle, double velosity)
        {
            Angle = angle;
            Velosity = velosity;
        }

        public void SetPath(double angle, double velosity)
        {
            const double g = 9.81;
            double x = 0, y = 0, vx = velosity * Math.Cos(angle), vy = velosity * Math.Sin(angle), t = 0;
            do
            {
                PosList.Add(new Point(x, y));
                x += vx * delta;
                y += vy * delta;
                //vx - const, if k(t) = 0
                vy -= g * delta;
                t += delta;
            } while (y > 0);
            Point last = PosList[PosList.Count - 1];
            PosList.Add(new Point(last.X + (x - last.X) * (-last.Y) / (y - last.Y), 0));
            //PosList.Add(new Point(x, y));
        }


        public void ShowPath()
        {
            int sz = PosList.Count;
            string res = "";
            var culture = new CultureInfo("en-US");
            culture.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = culture;

            for (int i = 0; i < sz; ++i)
                //res += "Time: " + delta * i + " x = " + Math.Round(PosList[i].X, 3) + " y = " + Math.Round(PosList[i].Y, 3);
                res += Math.Round(PosList[i].X, 3) + ", ";
            res = res.Substring(0, res.Length - 2);

            res += "\n\n\n";

            for (int i = 0; i < sz; ++i)
                //res += "Time: " + delta * i + " x = " + Math.Round(PosList[i].X, 3) + " y = " + Math.Round(PosList[i].Y, 3);
                res += Math.Round(PosList[i].Y, 3) + ", ";

            res = res.Substring(0, res.Length - 2);
            ZXCw.writer.WriteLine(res);
        }


        public List<Point> PathData()
        {
            return PosList;
        }

        //Объявление делегата на функции типа void(int)
        public delegate void CollisionHandlerDelegate(int i);

        //Объявление события, соответствующее делегату CollisionHandlerDelegate
        public event CollisionHandlerDelegate OnCollision;

        //"Обработчик" события OnCollision
        public void CollisionMessage(int i)
        {
            string collisionMessage = "Столконовение в моменте:\nx=" +
                              Math.Round(PosList[i].X, 3) + " y=" +
                              Math.Round(PosList[i].Y, 3);

            ZXCw.writer.WriteLine(collisionMessage);

            MessageBox.Show(collisionMessage, Application.Current.MainWindow.Title);
        }


        //Compl.: O(count of vecrtices of obs * sz) ~ O(n^2)
        public int IsCollision(Obstacle obs)
        {
            int sz = PosList.Count;
            for (int i = 0; i < sz; ++i)
                if (obs.Belongs(PosList[i]))
                {
                    OnCollision += CollisionMessage;
                    OnCollision(i);
                    return i;
                }
            return -1;
        }
    }

}
