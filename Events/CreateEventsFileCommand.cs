using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SleepToCsv.Events
{
    public class CreateEventsFileCommand
    {
        private const int RawDataIndex = 15;
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        public void Execute(string sourceFile, string targetFolder)
        {
            Console.WriteLine("Creating events file.");

            var rows = ReadEventsSource(sourceFile);

            var orderedRows = rows
                .OrderBy(p => p.DateTime)
                .ToList();

            WriteEventsTarget(targetFolder, orderedRows);

            Console.WriteLine("Events file has been created.");
        }

        private static List<EventRow> ReadEventsSource(string sourceFile)
        {
            var lines = File.ReadAllLines(sourceFile);

            var rows = new List<EventRow>();

            for (var i = 0; i < lines.Length; i++)
                ReadEventsRow(lines, i, rows);

            return rows;
        }

        private static void ReadEventsRow(string[] lines, int i, List<EventRow> rows)
        {
            try
            {
                var reader = new Reader();

                var line = lines[i];

                if (!line.StartsWith("Id,"))
                    return;

                var fields = line.Split(',');

                for (var j = RawDataIndex; j < fields.Length; j++)
                {
                    var columnName = reader.GetString(lines, i, j);

                    if (columnName != "Event")
                        continue;

                    var field = reader.GetString(lines, i + 1, j);

                    var row = new EventRow();

                    row.DateTime = GetDateTime(field);

                    row.EventType = GetEventType(field);

                    rows.Add(row);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: Failed to load row {0} due to the following error: /r/n {1}", i, ex.Message);
            }
        }

        private static DateTime GetDateTime(string field)
        {
            var parts = field.Split('-');

            var millisecondsAfterUnixEpoch = long.Parse(parts[1]);

            var dateTimeUtc = UnixEpoch.AddMilliseconds(millisecondsAfterUnixEpoch);

            var dateTime = dateTimeUtc.ToLocalTime();

            return dateTime;
        }

        private static string GetEventType(string field)
        {
            var parts = field.Split('-');

            var eventName = parts[0];

            var friendlyName = GetFriendlyName(eventName);

            return friendlyName;
        }

        private static string GetFriendlyName(string eventName)
        {
            var friendlyChars = new char[eventName.Length];

            for (var i = 0; i < eventName.Length; i++)
                friendlyChars[i] = GetFriendlyCharacter(eventName, i);

            var friendlyName = new string(friendlyChars);

            friendlyName = friendlyName.Replace("By", "by");

            friendlyName = friendlyName.Replace("Rem", "REM");

            return friendlyName;
        }

        private static char GetFriendlyCharacter(string eventName, int i)
        {
            var character = eventName[i];

            if (i == 0)
                return Char.ToUpper(character);

            if (character == '_')
                return ' ';

            if (i > 0 && eventName[i - 1] == '_')
                return Char.ToUpper(character);

            return Char.ToLower(character);
        }

        private static void WriteEventsTarget(string targetFolder, List<EventRow> rows)
        {
            var targetText = new StringBuilder();

            WriteEventsHeader(targetText);

            foreach (var row in rows)
                WriteEventsRow(targetText, row);

            var targetFileName = Path.Combine(targetFolder, "Events.csv");

            File.WriteAllText(targetFileName, targetText.ToString());
        }

        private static void WriteEventsHeader(StringBuilder targetText)
        {
            targetText
                .Append("DateTime,")
                .Append("Event,")
                .AppendLine();
        }

        private static void WriteEventsRow(StringBuilder targetText, EventRow row)
        {
            targetText
                .Append(row.DateTime + ",")
                .Append(row.EventType + ",")
                .AppendLine();
        }
    }
}
