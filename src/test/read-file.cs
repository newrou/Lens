using System;
using System.IO;

class Test
{
    public static void Main(string[] args)
    {
	int counter=0;
	string line;
	string FileName;

        Console.WriteLine("Work with args directly:");
        foreach (string arg in args) {
                Console.WriteLine(arg);
            }
//	Console.ReadKey();
	FileName=args[0];

        try
        {   // Open the text file using a stream reader.
//            using (StreamReader file = new StreamReader("TestFile.txt"))
            using (StreamReader file = new StreamReader(FileName))
            {
            // Read the stream to a string, and write the string to the console.
		while((line = file.ReadLine()) != null)
		    {
			System.Console.WriteLine(line);
			counter++;
		    }
		file.Close();
		System.Console.WriteLine("There were {0} lines.", counter);
//                String line = sr.ReadToEnd();
//                Console.WriteLine(line);
            }
        }
        catch (IOException e)
        {
            Console.WriteLine("The file could not be read:");
            Console.WriteLine(e.Message);
        }
    }
}
