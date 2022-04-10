using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemperatureProgram
{
    internal class DayTemperature
    {
        public DateTime Date { get; private set; }

        /// <summary>
        /// Key: Hour
        /// Value: Temperature
        /// </summary>
        public Dictionary<int, double> Temperature { get; private set; } = new Dictionary<int, double>();

        public DayTemperature(DateTime date)
        {
            Date = date;
        }

        public double GetMaxTemperature()
        {
            if (Temperature.Count == 0) return 1;
            return Temperature.Values.Max();
        }

        public double GetMinTemperature()
        {
            if (Temperature.Count == 0) return 0;
            return Temperature.Values.Min();
        }

        public double GetDeviation()
        {
            return 0;
        }
    }
}
