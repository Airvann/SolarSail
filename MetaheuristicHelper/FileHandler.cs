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
            FileStream fs = new FileStream(path, FileMode.CreateNew, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);

            List<double> c = res.GetControl();
            List<double> h = res.GetH();

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
            return new AgentFrame(sectionsCount, splineCoeff, c, h);
        }
    }
}
