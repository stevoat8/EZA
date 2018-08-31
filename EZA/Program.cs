using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZA
{
    class Program
    {
        const int cellCount = 120;
        const int genCount = 75;

        static void Main(string[] args)
        {
            for (int r = 0; r < 256; r++)
            {
                //Get current rule (0..255)
                string[] rule = GetRule(r);
                Console.WriteLine($"Rule #: {r} {string.Join("",rule)}");

                //Initialize Generation 0
                string[] gen = new string[cellCount];
                gen.Populate("0");
                gen[(cellCount / 2)] = "1";
                string[] newGen;
                PrintGeneration(0, gen);

                //Jede einzelne Generation berechnen und ausgeben
                for (int i = 1; i <= genCount; i++)
                {
                    newGen = new string[cellCount];
                    for (int c = 0; c < cellCount; c++)
                    {
                        string pre1 = c > 0 ? gen[c - 1] : "0";
                        string pre2 = gen[c];
                        string pre3 = c < cellCount - 1 ? gen[c + 1] : "0";
                        int ruleIndex = rule.Length - Convert.ToInt32(pre1 + pre2 + pre3, 2) - 1;
                        newGen[c] = rule[ruleIndex];
                    }
                    gen = newGen;
                    PrintGeneration(i, gen);
                }
                Console.ReadKey();
                Console.Clear();
            }
        }

        private static string[] GetRule(int ruleNr)
        {
            string ruleBin = Convert.ToString(ruleNr, 2);
            string[] rule = new string[8] { "0", "0", "0", "0", "0", "0", "0", "0", };

            int offset = 8 - ruleBin.Length;
            for (int i = 0; i < ruleBin.Length; i++)
            {
                rule[offset + i] = ruleBin[i].ToString();
            }
            return rule;
        }

        private static void PrintGeneration(int genCount, string[] gen)
        {
            //Console.Write("G{0:0#}: ", genCount);

            foreach (string cell in gen)
            {
                if (cell == "0")
                {
                    Console.Write(" ");
                }
                else if (cell == "1")
                {
                    Console.Write("X");
                }
            }
            Console.WriteLine();
        }
    }

    static class ArrayExtension
    {
        /// <summary>
        /// Füllt ein string-Array mit dem übergebenen Wert auf.
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="defaultValue"></param>
        public static void Populate(this string[] arr, string defaultValue)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = defaultValue;
            }
        }
    }
}
