using System;

namespace SleepToCsv.Summary
{
    public class SummaryRow
    {
        public long Id { get; set; }

        public string TimeZone { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public DateTime Scheduled { get; set; }

        public float Hours { get; set; }

        public float Rating { get; set; }

        public string Comments { get; set; }

        public int FrameRate { get; set; }

        public int Snore { get; set; }

        public float Noise { get; set; }

        public int Cycles { get; set; }

        public float DeepSleep { get; set; }

        public int LengthAdjust { get; set; }

        public string Geo { get; set; }
    }
}
