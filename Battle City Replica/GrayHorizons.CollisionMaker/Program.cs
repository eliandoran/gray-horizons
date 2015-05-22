using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using GrayHorizons.Extensions;
using GrayHorizons.Logic;
using GrayHorizons.StaticObjects;
using GrayHorizons.Windows.DirectX;

namespace GrayHorizons.CollisionMaker
{
    class MainClass
    {
        static readonly Color CollisionColor = Color.FromArgb(255, 0, 255);
        static readonly Color DummyColor = Color.FromArgb(255, 0, 0);
        static readonly List<CollisionBoundary> CollisionRectangles = new List<CollisionBoundary>();
        static readonly List<DummyTarget> Dummies = new List<DummyTarget>();
        static Bitmap bitmap;
        static Stopwatch stopwatch;

        static int CalculateHeight(int originX, int originY)
        {
            int height = 1;
            for (int y = originY + 1;; y++)
            {
                if (y < bitmap.Height && bitmap.GetPixel(originX, y) == CollisionColor)
                    height++;
                else
                    return height;
            }
        }

        public static void Main(string[] args)
        {
            var fileName = @"C:\Users\elinn\Desktop\Gray Horizons\Gray Horizons\GrayHorizons.Content\Maps\TutorialCollision.png";

            Console.WriteLine("Parsing {0}...".FormatWith(Path.GetFileName(fileName)));
            Console.Write("Loading bitmap file... ");

            using (bitmap = new Bitmap(fileName))
            {
                Console.WriteLine("[w={0}, h={1}]".FormatWith(bitmap.Width, bitmap.Height));

                #region Scanning for collision rectangles
                Console.WriteLine("Scanning... ");
                stopwatch = Stopwatch.StartNew();

                bool foundCorner = false;
                int width = 1;
                int height = 0;
                int collisionCount = 1, dummyCount = 0;
                int cornerX = 0;
                int widthDigits = width.ToString().Length;
                int heightDigits = height.ToString().Length;
                for (int y = 0; y < bitmap.Height; y++)
                {
                    for (int x = 0; x < bitmap.Width; x++)
                    {
                        if (bitmap.GetPixel(x, y) == CollisionColor &&
                            (y == 0 || bitmap.GetPixel(x, y - 1) != CollisionColor))
                        {
                            if (!foundCorner)
                            {
                                foundCorner = true;
                                cornerX = x;
                                height = CalculateHeight(x, y);
                            }
                            else
                                width++;
                        }
                        else
                        {
                            if (foundCorner)
                            {
                                Console.WriteLine(
                                    "[Collision] X: {0}\tY: {1}\tW: {2}\tH: {3}".FormatWith(
                                        cornerX, y,
                                        width, height));

                                CollisionRectangles.Add(new CollisionBoundary(cornerX, y, width, height));
                                width = 1;
                                collisionCount++;
                                foundCorner = false;
                            }
                        }

                        if (bitmap.GetPixel(x, y) == DummyColor)
                        {
                            if ((x == 0 || (bitmap.GetPixel(x - 1, y) != DummyColor) &&
                                (y == 0 || (bitmap.GetPixel(x, y - 1) != DummyColor))))
                            {
                                Dummies.Add(new DummyTarget()
                                    {
                                        Location = new Microsoft.Xna.Framework.Point(x, y)
                                    });

                                Console.WriteLine("[Dummy] ({0}, {1})).".FormatWith(x, y));
                                dummyCount++;
                            }
                        }
                    }
                }

                Console.WriteLine("{0} collision rectangles found.".FormatWith(collisionCount));
                Console.WriteLine("{0} dummies were found.".FormatWith(dummyCount));
            }
            #endregion

            #region Serializing the results
            var outputFile = Path.ChangeExtension(fileName, "collision.xml");
            Console.WriteLine("Serializing collision rectangles to {0}...".FormatWith(outputFile));
            CollisionBoundary.SerializeList(CollisionRectangles, new FileStreamInputOutputAgent(), outputFile);

            outputFile = Path.ChangeExtension(fileName, "dummies.xml");
            Console.WriteLine("Serializing dummies to {0}...".FormatWith(outputFile));
            using (Stream stream = new FileStream(outputFile, FileMode.Create))
            using (var writer = new StreamWriter(stream))
            {
                var serializer = new XmlSerializer(
                                     Dummies.GetType(),
                                     new XmlRootAttribute("Dummies"));
                serializer.Serialize(writer, Dummies);
            }

            #endregion

            Console.WriteLine("Done in {0}.".FormatWith(stopwatch.Elapsed.ToString()));

            #if DEBUG
            Console.ReadKey();
            #endif
        }
    }
}
