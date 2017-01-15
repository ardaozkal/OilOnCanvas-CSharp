using System;
using System.Drawing;
using System.Text;
using System.IO;

namespace ooc //Oil on canvas
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("OilOnCanvas v1 - Copyright Ardaozkal 2017");
			if (args.Length == 0)
			{
				//Console.WriteLine("No arguments supplied.");
				//Console.WriteLine("To convert a PNG to OOC, run ooc.exe pngname.png");
				//Console.WriteLine("To convert an OOC to PNG, run ooc.exe oocname.ooc");
				while (true)
				{
					Console.WriteLine("Please specify the name of the file you want to convert (example: test.ooc or test.png):");
					var filename = Console.ReadLine();
					if (File.Exists(filename))
					{
						if (filename.EndsWith(".png"))
						{
							Console.WriteLine("Converting.");
							File.WriteAllText(filename.Replace(".png", ".ooc"), FromPNGToRaw(filename));
							Console.WriteLine("Conversion successful.");
						}
						else if (filename.EndsWith(".ooc"))
						{
							Console.WriteLine("Converting (this will take some time)");
							FromRawToPNG(File.ReadAllText(filename), filename.Replace(".ooc", ".png"));
							Console.WriteLine("Conversion successful.");
						}
						else
						{
							Console.WriteLine("Currently only png and ooc files are supported.");
						}
					}
					else
					{
						Console.WriteLine("File {0} not found", filename);
					}
				}
			}
			else if (args[0].EndsWith(".png"))
			{
				if (File.Exists(args[0]))
				{
					Console.WriteLine("Converting.");
					File.WriteAllText(args[0].Replace(".png",".ooc"),FromPNGToRaw(args[0]));
					Console.WriteLine("Conversion successful.");
				}
				else
				{
					Console.WriteLine("File {0} not found.", args[0]);
				}
			}
			else if (args[0].EndsWith(".ooc"))
			{
				if (File.Exists(args[0]))
				{
					Console.WriteLine("Converting (this will take some time)");
					FromRawToPNG(File.ReadAllText(args[0]), args[0].Replace(".ooc", ".png"));
					Console.WriteLine("Conversion successful.");
				}
				else
				{
					Console.WriteLine("File {0} not found.", args[0]);
				}
			}
			else
			{
				Console.WriteLine("Wrong arguments supplied.");
				Console.WriteLine("To convert a PNG to OOC, run ooc.exe pngname.png");
				Console.WriteLine("To convert an OOC to PNG, run ooc.exe oocname.ooc");
			}
		}

		public static void FromRawToPNG(string rawdata, string pngfilename)
		{
			string[] Ylines = rawdata.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
			var width = (Ylines[0].Length - Ylines[0].Replace(",", "").Length) / 4;
			var height = Ylines.Length;

			var output = new Bitmap(width, height);
			for (int y = 0; y < height; y++)
			{
				var currentline = Ylines[y];
				for (int x = 0; x < width; x++)
				{
					if (currentline.Contains(","))
					{
						var r = int.Parse(currentline.Substring(0, currentline.IndexOf(",")));
						currentline = currentline.Substring(currentline.IndexOf(",") + 1);
						var g = int.Parse(currentline.Substring(0, currentline.IndexOf(",")));
						currentline = currentline.Substring(currentline.IndexOf(",") + 1);
						var b = int.Parse(currentline.Substring(0, currentline.IndexOf(",")));
						currentline = currentline.Substring(currentline.IndexOf(",") + 1);
						var a = int.Parse(currentline.Substring(0, currentline.IndexOf(",")));
						currentline = currentline.Substring(currentline.IndexOf(",") + 1);
						var colortoput = Color.FromArgb(a, r, g, b);
						output.SetPixel(x, y, colortoput);
					}
				}
			}
			output.Save(pngfilename);
		}

		public static string FromPNGToRaw(string filename)
		{
			var sb = new StringBuilder();
			Bitmap asd = new Bitmap(filename);
			for (int y = 0; y < asd.Height; y++)
			{
				for (int x = 0; x < asd.Width; x++)
				{
					var curpix = asd.GetPixel(x,y);
					if (x != 0)
					{
						sb.Append(",");
					}
					sb.Append(curpix.R + "," + curpix.G + "," + curpix.B + "," + curpix.A);
				}
				sb.AppendLine();
			}
			return sb.ToString();
		}
	}
}
