using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;

namespace EZA
{
    class Program
    {
        const int genCountDefault = 104;
        const int cellCountDefault = genCountDefault * 2 + 1;
        const int ruleLength = 8;
        const int pixelSize = 10;

        static void Main(string[] args)
        {
#if DEBUG
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
#endif
            string defaultPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                "images");

            if (Directory.Exists(defaultPath) == false)
            {
                Directory.CreateDirectory(defaultPath);
            }

            for (int ruleNr = 0; ruleNr < 256; ruleNr++)
            {
                try
                {
                    Bitmap image = CreateImage(cellCountDefault, genCountDefault, ruleNr);
                    image.Save(Path.Combine(defaultPath, "rule" + ruleNr + ".png"));
                    Console.WriteLine($"Rule #{ruleNr}: created successfully");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Rule #{ruleNr}: Error (Original error message: \"{ex.Message}\")");
                }
            }
#if DEBUG
            stopWatch.Stop();
            Console.WriteLine("Stopwatch: " + stopWatch.ElapsedMilliseconds + " ms");
            Console.Write("\nPress any key to quit...");
            Console.ReadKey(true);
#endif
        }

        /// <summary>
        /// Erzeugt eine Grafik für die Evolution der übergebenen Regel.
        /// </summary>
        private static Bitmap CreateImage(int cellCount, int genCount, int ruleNr)
        {
            string rule = GetBinaryRule(ruleNr);

            SolidBrush pixelBrush = new SolidBrush(Color.Black);
            Bitmap image = new Bitmap(cellCount * pixelSize, (genCount + 1) * pixelSize);
            using (Graphics graphic = Graphics.FromImage(image))
            {
                graphic.Clear(Color.White);

                // Generation 0 initialisieren
                string[] gen = new string[cellCount];
                for (int i = 0; i < cellCount; i++)
                {
                    gen[i] = "0";
                }
                gen[cellCount / 2] = "1";

                for (int y = 0; y <= genCount; y++)
                {
                    for (int x = 0; x < cellCount; x++)
                    {
                        if (gen[x] == "1")
                        {
                            graphic.FillRectangle(pixelBrush, x * pixelSize, y * pixelSize, pixelSize, pixelSize);
                        }
                    }
                    //TODO: GenerateNextGeneration wird noch einmal zu oft aufgerufen (immer für genCount+1).
                    gen = GenerateNextGeneration(gen, rule);
                }
            }

            return image;
        }

        /// <summary>
        /// Generiert zu einer Dezimalmal die passende Regel als Binärzahl mit führenden Nullen.
        /// </summary>
        private static string GetBinaryRule(int ruleNr)
        {
            //Dezimalzahl in Binärzahl konvertieren...
            string binRule = Convert.ToString(ruleNr, 2);

            //...und mit führenden Nullen auffüllen
            string leadingZeros = "";
            for (int i = 0; i < ruleLength - binRule.Length; i++)
            {
                leadingZeros += "0";
            }
            return leadingZeros + binRule;
        }

        /// <summary>
        /// Erzeugt eine Generation an Zeichen/Pixel, basierend auf der vorhergehenden 
        /// Generation und der anzuwendenen Regel.
        /// </summary>
        private static string[] GenerateNextGeneration(string[] gen, string rule)
        {
            int cells = gen.Length;
            string[] nextGen = new string[cells];
            for (int c = 0; c < cells; c++)
            {
                string pre1 = c > 0 ? gen[c - 1] : "0";
                string pre2 = gen[c];
                string pre3 = c < cells - 1 ? gen[c + 1] : "0";
                int ruleIndex = ruleLength - Convert.ToInt32(pre1 + pre2 + pre3, 2) - 1;
                nextGen[c] = rule[ruleIndex].ToString();
            }
            return nextGen;
        }
    }
}
