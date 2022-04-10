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

        public double GetAvarage()
        {
            if (Temperature.Count == 0) return 0;
            return Temperature.Values.Average();
        }

        public double GetDeviation()
        {
            List<double> numbers = Temperature.Values.ToList();
            if (numbers.Count == 0) return 0;

            double avarage = GetAvarage();
            double summ = 0;
            foreach(int number in numbers)
            {
                summ += Math.Pow(number - avarage, 2);
            }

            return Math.Sqrt(summ / numbers.Count);
        }
    }
}
