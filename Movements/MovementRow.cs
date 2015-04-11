using System;

namespace SleepToCsv.Movements
{
    public class MovementRow
    {
        public DateTime DateTime { get; set; }

        public float Movement { get; set; }

        public float? Noise { get; set; }
    }
}
