using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;

namespace travelling_thief_problem
{
    class Program
    {
        static void Main(string[] args)
        {
            CultureInfo ci = new CultureInfo("en-GB");
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;

            List<string> setup = ReadProblemSetup("medium_4");

            TravellingThiefProblem travellingThiefProblem = new TravellingThiefProblem(setup);

            Console.ReadKey();
        }

        static List<string> ReadProblemSetup(string relativePath)
        {
            string line = "";
            List<string> setup = new List<string>();
            
            try
            {
                StreamReader setupFile = new StreamReader($"student\\{relativePath}.ttp");

                while ((line = setupFile.ReadLine()) != null)
                {
                    //Console.WriteLine(line);
                    setup.Add(line);
                }
                setupFile.Close();
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(ex);
            }

            return setup;
        }
    }

    
}
