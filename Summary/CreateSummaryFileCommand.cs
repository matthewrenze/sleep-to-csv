using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SleepToCsv.Summary
{
    public class CreateSummaryFileCommand
    {
        private const int IdIndex = 0;
        private const int TimeZoneIndex = 1;
        private const int FromIndex = 2;
        private const int ToIndex = 3;
        private const int ScheduledIndex = 4;
        private const int HoursIndex = 5;
        private const int RatingIndex = 6;
        private const int CommentsIndex = 7;
        private const int FrameRateIndex = 8;
        private const int SnoreIndex = 9;
        private const int NoiseIndex = 10;
        private const int CyclesIndex = 11;
        private const int DeepSleepIndex = 12;
        private const int LengthAdjustIndex = 13;
        private const int GeoIndex = 14;

        public void Execute(string sourceFile, string targetFolder)
        {
            Console.WriteLine("Creating summary file.");

            var rows = ReadSource(sourceFile);

            rows.Reverse();

            WriteTarget(targetFolder, rows);

            Console.WriteLine("Summary file has been created.");
        }

        private static List<SummaryRow> ReadSource(string sourceFile)
        {
            var lines = File.ReadAllLines(sourceFile);

            var rows = new List<SummaryRow>();

            for (int i = 0; i < lines.Length; i++)
                ReadRow(lines, i, rows);

            return rows;
        }

        private static void ReadRow(string[] lines, int i, List<SummaryRow> rows)
        {
            try
            {
                var reader = new Reader();

                var line = lines[i];

                if (!line.StartsWith("Id,"))
                    return;

                var row = new SummaryRow();

                var rowIndex = i + 1;

                row.Id = reader.GetLong(lines, rowIndex, IdIndex);
                row.TimeZone = reader.GetString(lines, rowIndex, TimeZoneIndex);
                row.From = reader.GetDateTime(lines, rowIndex, FromIndex);
                row.To = reader.GetDateTime(lines, rowIndex, ToIndex);
                row.Scheduled = reader.GetDateTime(lines, rowIndex, ScheduledIndex);
                row.Hours = reader.GetFloat(lines, rowIndex, HoursIndex);
                row.Rating = reader.GetFloat(lines, rowIndex, RatingIndex);
                row.Comments = reader.GetString(lines, rowIndex, CommentsIndex);
                row.FrameRate = reader.GetInt(lines, rowIndex, FrameRateIndex);
                row.Snore = reader.GetInt(lines, rowIndex, SnoreIndex);
                row.Noise = reader.GetFloat(lines, rowIndex, NoiseIndex);
                row.Cycles = reader.GetInt(lines, rowIndex, CyclesIndex);
                row.DeepSleep = reader.GetFloat(lines, rowIndex, DeepSleepIndex);
                row.LengthAdjust = reader.GetInt(lines, rowIndex, LengthAdjustIndex);
                row.Geo = reader.GetString(lines, rowIndex, GeoIndex);

                rows.Add(row);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: Failed to load row {0} due to the following error: /r/n {1}", i, ex.Message);
            }
        }

        private static void WriteTarget(string targetFolder, List<SummaryRow> rows)
        {
            var targetText = new StringBuilder();

            WriteHeader(targetText);

            foreach (var row in rows)
                WriteRow(targetText, row);

            var targetFileName = Path.Combine(targetFolder, "Summary.csv");

            File.WriteAllText(targetFileName, targetText.ToString());
        }

        private static void WriteHeader(StringBuilder targetText)
        {
            targetText
                .Append("Id,")
                .Append("TimeZone,")
                .Append("From,")
                .Append("To,")
                .Append("Scheduled,")
                .Append("Hours,")
                .Append("Rating,")
                .Append("Comment,")
                .Append("FrameRate,")
                .Append("Snore,")
                .Append("Noise,")
                .Append("Cycles,")
                .Append("DeepSleep,")
                .Append("LengthAdjust,")
                .Append("Geo,")
                .AppendLine();
        }

        private static void WriteRow(StringBuilder targetText, SummaryRow row)
        {
            targetText
                .Append(row.Id + ",")
                .Append(row.TimeZone + ",")
                .Append(row.From + ",")
                .Append(row.To + ",")
                .Append(row.Scheduled + ",")
                .Append(row.Hours + ",")
                .Append(row.Rating + ",")
                .Append(row.Comments + ",")
                .Append(row.FrameRate + ",")
                .Append(row.Snore + ",")
                .Append(row.Noise + ",")
                .Append(row.Cycles + ",")
                .Append(row.DeepSleep + ",")
                .Append(row.LengthAdjust + ",")
                .Append(row.Geo)
                .AppendLine();
        }
    }
}
