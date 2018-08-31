using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZA
{
    class Program
    {
        const int genCountDefault = 75;
        const int cellCountDefault = 150;
        const string printSign = "X";

        static void Main(string[] args)
        {
            string defaultPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "images");
            int cellCount = GetCellCount();
            int genCount = GetGenCount();
            for (int i = 0; i < 255; i++)
            {
                byte[] image = CreateImage(cellCount, genCount, i);
                ExportImage(image, defaultPath);
                Console.Clear();
            }
        }

        private static byte[] CreateImage(int cellCount, int genCount, int ruleNr)
        {
            string rule = GetBinaryRule(ruleNr);

            //Initialize Generation 0
            string[] gen = new string[cellCount];
            gen.Populate("0");
            gen[cellCount / 2] = "1";
            PrintGeneration(gen); //TODO in byte[] bzw. image speichern

            for (int i = 1; i <= genCount; i++)
            {
                gen = GenerateGeneration(gen, rule);
                PrintGeneration(gen); //TODO in byte[] bzw. image speichern
            }

            return new byte[0];
        }


        private static void ExportImage(byte[] image, string defaultPath)
        {
            //TODO
            throw new NotImplementedException();
        }
        private static string GetBinaryRule(int ruleNr)
        {
            string ruleBin = Convert.ToString(ruleNr, 2);
            string[] rule = new string[8] { "0", "0", "0", "0", "0", "0", "0", "0", };

            int offset = 8 - ruleBin.Length;
            for (int i = 0; i < ruleBin.Length; i++)
            {
                rule[offset + i] = ruleBin[i].ToString();
            }
            return string.Join("", rule);
        }

        private static string[] GenerateGeneration(string[] gen, string rule)
        {
            int cells = gen.Length;
            string[] newGen = new string[cells];
            for (int c = 0; c < cells; c++)
            {
                string pre1 = c > 0 ? gen[c - 1] : "0";
                string pre2 = gen[c];
                string pre3 = c < cells - 1 ? gen[c + 1] : "0";
                int ruleIndex = 8 - Convert.ToInt32(pre1 + pre2 + pre3, 2) - 1;
                newGen[c] = rule[ruleIndex].ToString();
            }
            return newGen;
        }

        private static void PrintGeneration(string[] gen)
        {
            foreach (string cell in gen)
            {
                if (cell == "0")
                {
                    Console.Write(" ");
                }
                else if (cell == "1")
                {
                    Console.Write(printSign);
                }
            }
            Console.WriteLine();
        }

        #region Input
        private static int GetCellCount()
        {
            int cellCount;
            while (true)
            {
                Console.Write($"Cells (default {cellCountDefault}): ");
                string input = Console.ReadLine();
                if (input == "")
                {
                    cellCount = cellCountDefault;
                }
                else
                {
                    try
                    {
                        cellCount = int.Parse(input);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Invalid input");
                        continue;
                    }
                }
                if (cellCount < 8)
                {
                    Console.WriteLine("Min. 8 cells.");
                }
                else
                {
                    break;
                }
            }

            return cellCount;
        }

        private static int GetGenCount()
        {
            int genCount;
            while (true)
            {
                Console.Write($"Generations (default {genCountDefault}): ");
                string input = Console.ReadLine();
                if (input == "")
                {
                    genCount = genCountDefault;
                }
                else
                {
                    try
                    {
                        genCount = int.Parse(input);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Invalid input");
                        continue;
                    }
                }
                if (genCount < 1)
                {
                    Console.WriteLine("Min. 1 generation.");
                }
                else
                {
                    break;
                }
            }

            return genCount;
        }

        private static int GetRuleNr()
        {
            int genCount;
            while (true)
            {
                Console.Write("Rule: ");
                string input = Console.ReadLine();
                try
                {
                    genCount = int.Parse(input);
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid input");
                    continue;
                }
                if (genCount > 255)
                {
                    Console.WriteLine("Max. rule is 255.");
                }
                else
                {
                    break;
                }
            }

            return genCount;
        }
        #endregion
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
