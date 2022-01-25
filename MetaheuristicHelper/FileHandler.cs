using System;
using System.Collections.Generic;
using System.IO;
using MetaheuristicHelper;

namespace Visualization
{
    public static class FileHandler
    {
        public static void Write(string path = @"file.txt")
        {
            Result res = Result.Get();
            Settings set = Settings.Get();

            FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);

            List<double> c = res.GetControl();
            List<double> h = res.GetH();

            sw.WriteLine("<targetOrbit>");
            sw.WriteLine(set.orbit.GetName());

            sw.WriteLine("<brightness>");
            sw.WriteLine(set.brightness);

            sw.WriteLine("<ODESolver>");
            sw.WriteLine(set.odeSolver.GetName());

            sw.WriteLine("<step>");
            sw.WriteLine(set.odeSolverStep);

            sw.WriteLine("<splineCoeff>");
            sw.WriteLine(set.splineCoeff);

            sw.WriteLine("<c>");
            for (int i = 0; i < c.Count; ++i)
                sw.WriteLine(c[i]);
            sw.WriteLine("</c>");

            sw.WriteLine("<h>");
            for (int i = 0; i < h.Count; ++i)
                sw.WriteLine(h[i]);
            sw.WriteLine("</h>");

            sw.Close();
            fs.Close();
        }

        public static void Read(string path = @"file.txt")
        {
            FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);

            Settings set = Settings.Get();
            Result res = Result.Get();

            res.Clear();
            set.Clear();

            string odeName = "Метод Эйлера";

            List<double> c = new List<double>();
            List<double> h = new List<double>();

            while (!sr.EndOfStream)
            {
                string nextLine = sr.ReadLine();

                if (nextLine == "<splineCoeff>")
                {
                    nextLine = sr.ReadLine();
                    set.splineCoeff = Convert.ToInt32(nextLine);
                    continue;
                }

                if (nextLine == "<targetOrbit>")
                {
                    nextLine = sr.ReadLine();
                    set.orbit = Orbit.ReturnOrbit(nextLine);
                    continue;
                }

                if (nextLine == "<ODESolver>")
                {
                    nextLine = sr.ReadLine();
                    odeName = nextLine;
                    continue;
                }

                if (nextLine == "<brightness>") 
                {
                    nextLine = sr.ReadLine();
                    set.brightness = Convert.ToDouble(nextLine);
                    continue;
                }

                if (nextLine == "<step>")
                {
                    nextLine = sr.ReadLine();
                    set.odeSolverStep = Convert.ToDouble(nextLine);
                    continue;
                }

                if (nextLine == "<h>")
                {
                    nextLine = sr.ReadLine();
                    do
                    {
                        h.Add(Convert.ToDouble(nextLine));
                        nextLine = sr.ReadLine();
                    } while (nextLine != "</h>");
                    res.Add("h", h);
                    set.sectionsCount = h.Count;
                }
                if (nextLine == "<c>")
                {
                    nextLine = sr.ReadLine();
                    do
                    {
                        c.Add(Convert.ToDouble(nextLine));
                        nextLine = sr.ReadLine();
                    } while (nextLine != "</c>");
                    res.Add("c", c);
                }
            }

            set.odeSolver = OdeSolver.OdeSolver.ReturnOdeSolver(odeName, set.splineCoeff, set.sectionsCount, set.brightness, set.odeSolverStep);
            fs.Close();
            sr.Close();
            if (res.GetControl().Count == 0 || res.GetH().Count == 0 || set.splineCoeff == 0)
                throw new FileLoadException();
        }
    }
}