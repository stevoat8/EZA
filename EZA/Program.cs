using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace EZA
{
    /// <summary>
    /// The program works as an elementary cellular automaton which can be configured by 256 different binary rules.
    /// The program applies all 256 rules to generate evolution images.
    /// </summary>
    internal class Program
    {
        private const int genCountDefault = 104;
        private const int cellCountDefault = genCountDefault * 2 + 1;
        private const int ruleLength = 8;
        private const int pixelSize = 10;

        /// <summary>
        /// Generates evolution images and saves them in seperate files.
        /// </summary>
        private static void Main(string[] args)
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
        /// Creates an image for the evolution of the passed Rule.
        /// </summary>
        private static Bitmap CreateImage(int cellCount, int genCount, int ruleNr)
        {
            string rule = GetBinaryRule(ruleNr);

            SolidBrush pixelBrush = new SolidBrush(Color.Black);
            Bitmap image = new Bitmap(cellCount * pixelSize, (genCount + 1) * pixelSize);
            using (Graphics graphic = Graphics.FromImage(image))
            {
                graphic.Clear(Color.White);

                // Initialize generation 0
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
                    //TODO: Method is called once too often (for genCount + 1).
                    gen = GenerateNextGeneration(gen, rule);
                }
            }

            return image;
        }

        /// <summary>
        /// Generates the matching binary rule for the passed decimal digit.
        /// </summary>
        private static string GetBinaryRule(int ruleNr)
        {
            //Convert decimal to binary...
            string binRule = Convert.ToString(ruleNr, 2);

            //...and filling up with leading zeros
            string leadingZeros = "";
            for (int i = 0; i < ruleLength - binRule.Length; i++)
            {
                leadingZeros += "0";
            }
            return leadingZeros + binRule;
        }

        /// <summary>
        /// Generates a generation based on the previous one and the given binary rule
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