using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SleepToCsv
{
    public class Reader
    {
        public DateTime GetDateTime(string[] lines, int rowIndex, int columnIndex)
        {
            var value = GetString(lines, rowIndex, columnIndex);

            return DateTime.ParseExact(value, "dd. MM. yyyy H:mm", null);
        }

        public float GetFloat(string[] lines, int rowIndex, int columnIndex)
        {
            var value = GetString(lines, rowIndex, columnIndex);

            return Single.Parse(value);
        }

        public int GetInt(string[] lines, int rowIndex, int columnIndex)
        {
            var value = GetString(lines, rowIndex, columnIndex);

            return Int32.Parse(value);
        }

        public long GetLong(string[] lines, int rowIndex, int columnIndex)
        {
            var value = GetString(lines, rowIndex, columnIndex);

            return Int64.Parse(value);
        }

        public string GetString(string[] lines, int rowIndex, int columnIndex)
        {
            var line = lines[rowIndex];

            var fields = line.Split(',');

            var value = fields[columnIndex];

            var text = value.Replace("\"", "");

            return text;
        }
    }
}
