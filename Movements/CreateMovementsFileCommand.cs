using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SleepToCsv.Movements
{
    public class CreateMovementsFileCommand
    {
        private const int FromIndex = 2;
        private const int RawDataIndex = 15;

        public void Execute(string sourceFile, string targetFolder)
        {
            Console.WriteLine("Creating movements file.");

            var rows = ReadSource(sourceFile);

            var orderedRows = rows
                .OrderBy(p => p.DateTime)
                .ToList();

            WriteTarget(targetFolder, orderedRows);

            Console.WriteLine("Movements file has been created.");
        }

        private static List<MovementRow> ReadSource(string sourceFile)
        {
            var lines = File.ReadAllLines(sourceFile);

            var rows = new List<MovementRow>();

            for (int i = 0; i < lines.Length; i++)
                ReadRow(lines, i, rows);

            return rows;
        }

        private static void ReadRow(string[] lines, int i, List<MovementRow> rows)
        {
            try
            {
                var reader = new Reader();

                var line = lines[i];

                if (!line.StartsWith("Id,"))
                    return;

                var date = reader.GetDateTime(lines, i + 1, FromIndex).Date;

                var fields = line.Split(',');

                var lastTime = new DateTime();

                for (var j = RawDataIndex; j < fields.Length; j++)
                {
                    var columnName = reader.GetString(lines, i, j);

                    if (columnName == "Event")
                        continue;

                    var time = DateTime.ParseExact(columnName, "H:mm", null);

                    if (time < lastTime)
                        date = date.AddDays(1);

                    lastTime = time;

                    var dateTime = date.Add(time.TimeOfDay);

                    var row = new MovementRow();

                    row.DateTime = dateTime;

                    row.Movement = reader.GetFloat(lines, i + 1, j);

                    if (HasNoiseData(lines, i + 2, j))
                        row.Noise = reader.GetFloat(lines, i + 2, j);

                    rows.Add(row);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: Failed to load row {0} due to the following error: /r/n {1}", i, ex.Message);
            }
        }

        private static bool HasNoiseData(string[] lines, int rowIndex, int columnIndex)
        {
            if (rowIndex >= lines.Length)
                return false;

            if (!lines[rowIndex].StartsWith(","))
                return false;

            if (columnIndex >= lines[rowIndex].Split(',').Length)
                return false;

            return true;
        }

        private static void WriteTarget(string targetFolder, List<MovementRow> rows)
        {
            var targetText = new StringBuilder();

            WriteHeader(targetText);

            foreach (var row in rows)
                WriteRow(targetText, row);

            var targetFileName = Path.Combine(targetFolder, "Movements.csv");

            File.WriteAllText(targetFileName, targetText.ToString());
        }

        private static void WriteHeader(StringBuilder targetText)
        {
            targetText
                .Append("DateTime,")
                .Append("Movement,")
                .Append("Noise,")
                .AppendLine();
        }

        private static void WriteRow(StringBuilder targetText, MovementRow row)
        {
            targetText
                .Append(row.DateTime + ",")
                .Append(row.Movement + ",")
                .Append(row.Noise + ",")
                .AppendLine();
        }
    }
}
