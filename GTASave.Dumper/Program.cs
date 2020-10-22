using Newtonsoft.Json;
using System;
using System.IO;

namespace GTASave.Dumper
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileName = "GTASAsf1.b";
            if (args.Length >= 1)
            {
                fileName = args[0];
            }

            using (FileStream stream = System.IO.File.OpenRead(fileName))
            {
                SaveReader reader = new SaveReader(stream);
                Save save = reader.ReadSave();

                System.Console.WriteLine(JsonConvert.SerializeObject(save, Formatting.Indented));
            }
        }
    }
}
