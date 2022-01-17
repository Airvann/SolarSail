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
            Result res = Result.getInstance();
            FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);

            List<double> c = res.GetControl();
            List<double> h = res.GetH();

            sw.WriteLine("<date>");
            sw.WriteLine(DateTime.Now.ToString());

            sw.WriteLine("<targetOrbit>");
            sw.WriteLine(res.orbit.GetName());

            sw.WriteLine("<brightness>");
            sw.WriteLine(res.brightnessSolarSail);

            sw.WriteLine("<ODESolver>");
            sw.WriteLine(res.OdeSolver.GetName());

            sw.WriteLine("<step>");
            sw.WriteLine(res.brightnessSolarSail);
            sw.WriteLine("<sectionsCount>");
            sw.WriteLine(res.sectionsCount);;

            sw.WriteLine("<splineCoeff>");
            sw.WriteLine(res.splineCoeff);

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
        public static AgentFrame Read(string path = @"file.txt") 
        {
            FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);

            Orbit orbit = MetaheuristicHelper.Orbits.Mercury.Get();
            OdeSolver.OdeSolver odeSolver = new OdeSolver.EulerMethod(2,5,0.06,500, MetaheuristicHelper.Orbits.Mercury.Get());
            double brightness = 0.042;
            double step = -1;   //TODO: !!!!!!!!

            List<double> c = new List<double>();
            List<double> h = new List<double>();
            int sectionsCount = -1;
            int splineCoeff = -1;

            while (!sr.EndOfStream)
            {
                string nextLine = sr.ReadLine();

                if (nextLine == "<sectionsCount>")
                {
                    nextLine = sr.ReadLine();
                    sectionsCount = Convert.ToInt32(nextLine);
                    continue;
                }

                if (nextLine == "<splineCoeff>")
                {
                    nextLine = sr.ReadLine();
                    splineCoeff = Convert.ToInt32(nextLine);
                    continue;
                }

                if (nextLine == "<targetOrbit>")
                {
                    nextLine = sr.ReadLine();
                    orbit = Orbit.ReturnOrbit(nextLine);
                    continue;
                }

                if (nextLine == "<ODESolver>")
                {
                    nextLine = sr.ReadLine();
                    odeSolver = OdeSolver.OdeSolver.ReturnOdeSolver(nextLine, splineCoeff, sectionsCount, brightness, step, orbit);
                    continue;
                }

                if (nextLine == "<brightness>") 
                {
                    nextLine = sr.ReadLine();
                    brightness = Convert.ToDouble(nextLine);
                    continue;
                }

                if (nextLine == "<step>")
                {
                    nextLine = sr.ReadLine();
                    step = Convert.ToDouble(nextLine);
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
                }
                if (nextLine == "<c>")
                {
                    nextLine = sr.ReadLine();
                    do
                    {
                        c.Add(Convert.ToDouble(nextLine));
                        nextLine = sr.ReadLine();
                    } while (nextLine != "</c>");
                }
            }
            fs.Close();
            sr.Close();
            if (c.Count == 0 || h.Count == 0 || sectionsCount == -1 || splineCoeff == -1)
                throw new FileLoadException();
            return new AgentFrame(orbit, odeSolver, step, brightness, sectionsCount, splineCoeff, c, h);
        }
    }
}