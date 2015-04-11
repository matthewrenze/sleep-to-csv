using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SleepToCsv.Events;
using SleepToCsv.Movements;
using SleepToCsv.Summary;

namespace SleepToCsv
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (!args.Any())
                    DisplayHelp();

                var sourceFile = args[0];

                var targetFolder = args[1];

                new CreateSummaryFileCommand()
                    .Execute(sourceFile, targetFolder);

                new CreateMovementsFileCommand()
                    .Execute(sourceFile, targetFolder);

                new CreateEventsFileCommand()
                    .Execute(sourceFile, targetFolder);

                Exit();
            }
            catch (Exception ex)
            {
                Console.WriteLine("FATAL: {0}", ex.Message);

                Exit();
            }
        }

        private static void DisplayHelp()
        {
            var help = new StringBuilder()
               .AppendLine("Converts Sleep as Android .csv files to summary, movements, and events .csv files")
               .AppendLine("Usage: SleepToCsv.exe source-file target-folder")
               .AppendLine()
               .AppendLine("The parameters are:")
               .AppendLine("  source-file - the file containing the Sleep as Android .csv file")
               .AppendLine("  target-folder - the folder to write the real .csv files to")
               .AppendLine()
               .AppendLine("Example: SleepAsAndroid.exe \"C:\\Source\\sleep-export.csv\" \"C:\\Target\"");

            Console.WriteLine(help.ToString());

            Exit();
        }

        private static void Exit()
        {
            Console.WriteLine();

            Console.WriteLine("Press any key to exit.");

            Console.ReadKey();

            Environment.Exit(0);
        }
    }
}
