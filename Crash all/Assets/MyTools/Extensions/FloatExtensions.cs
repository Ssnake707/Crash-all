using UnityEngine;

namespace MyTools.Extensions
{
    public static class FloatExtensions
    {
        private static readonly char[] Letter = new char[] { 'K', 'M', 'B', 'T', 'q', 'Q', 's', 'S', 'O', 'N', 'D', 'U' };

        /// <summary>
        /// Float to string and add symbol
        /// </summary>
        /// <param name="value">float value</param>
        /// <returns>converting float to string</returns>
        public static string ToStringLetter(this float value)
        {
            string result = "";
            int i = 0;
            while (value > 1000f)
            {
                value /= 1000f;
                i++;
            }

            /*if (i == 0)
            {
                result = value.ToString("f0");
                return result;
            }*/

            int x = Mathf.FloorToInt(value);
            int y = Mathf.FloorToInt((value - Mathf.FloorToInt(value)) * 10);
            result = x.ToString();
            if (y != 0) 
                result = result + "." + y;

            if ((i - 1) < 0) 
                return result;
            
            result = result + Letter[i - 1];
            return result;
        }
        
        /// <summary>
        /// Choose random with chance
        /// </summary>
        /// <param name="values">Sorted array</param>
        /// <returns>index</returns>
        public static int Choose(this float[] values)
        {
            float total = 0;

            foreach (float elem in values) 
                total += elem;

            float randomPoint = Random.value * total;

            for (int i= 0; i < values.Length; i++)
                if (randomPoint < values[i])
                    return i;
                else
                    randomPoint -= values[i];

            return values.Length - 1;
        }
    }
}