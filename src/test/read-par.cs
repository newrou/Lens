using System;
using System.IO;

class Test
{
    public static void Main(string[] args)
    {
	int counter=0;
	string line;
	string FileName, writePathOfFile="Lens.out";

        // Масштаб в программе 1 юнит = 1 мкм
        int scale = 1000;
        int scale_cm2mm = 10;
        double PixelScale = 1;
        double ScaleAngle = Math.PI / 180;
        // Lens
        double Cell = 0; // μm
        double Wall = 0; // μm
        double thikness = 0; // μm
        double density = 8.902; // g/cm3
        double WightToEdge = 0; // μm
        double HeightToEdge = 0; // μm
        double LensWidth = 0; // mm
        double LensHeight = 0; // mm
        int TypeMaterial = 0;
        // Screen
        int ScreenX = 0; // px
        int ScreenY = 0; // px
        double WidthScreen = 0; // mm
        double HeightScreen = 0; // mm
        // Settings
        double WidthSource = 0; // mm
        double HeightSource = 0; // mm
        double StartEnergy = 0; // keV
        double LengthSourceObject = 0; // mm
        double LengthObjectLens = 0; // mm
        double LengthLensScreen = 0; // mm
        double MaxAngleSource = 0; // rad
        double NStart = 0;
        // Steps
        int AmountOfScattering = 0;
        double MaxAngleScatering = 0; // rad
        double Step = 0; // μm
        double AngleStep = 0; // rad
        double AngleRadStep = 0; // rad
        // Object
        double ObjectWidth = 0; // mm
        double ObjectHeight = 0; // mm
        double ObjectThikness = 0; // mm
        // Нужные переменные
        // Solve settings
        int CountOfThreads = 1;

//        foreach (string arg in args) { Console.WriteLine(arg); }
	FileName=args[0];
        Console.WriteLine("Parameter file: {0}", FileName);

        try
        {
            using (StreamReader file = new StreamReader(FileName))
            {
		while((line = file.ReadLine()) != null)
		    {
//			System.Console.WriteLine(line);
			// Lens
			if(line.IndexOf("Cell:")==0) { Cell = Convert.ToDouble(line.Replace("Cell:", "").Replace('.', ',')); continue; }
			if(line.IndexOf("Wall:")==0) { Wall = Convert.ToDouble(line.Replace("Wall:", "").Replace('.', ',')); continue; }
			if(line.IndexOf("thikness:")==0) { thikness = Convert.ToDouble(line.Replace("thikness:", "").Replace('.', ',')); continue; }
			if(line.IndexOf("density:")==0) { density = Convert.ToDouble(line.Replace("density:", "").Replace('.', ',')); continue; }
			if(line.IndexOf("WightToEdge:")==0) { WightToEdge = Convert.ToDouble(line.Replace("WightToEdge:", "").Replace('.', ',')); continue; }
			if(line.IndexOf("HeightToEdge:")==0) { HeightToEdge = Convert.ToDouble(line.Replace("HeightToEdge:", "").Replace('.', ',')); continue; }
			if(line.IndexOf("LensWidth:")==0) { LensWidth = scale * Convert.ToDouble(line.Replace("LensWidth:", "").Replace('.', ',')); continue; }
			if(line.IndexOf("LensHeight:")==0) { LensHeight = scale * Convert.ToDouble(line.Replace("LensHeight:", "").Replace('.', ',')); continue; }
// Screen
			if(line.IndexOf("ScreenX:")==0) { ScreenX = (int)Convert.ToDouble(line.Replace("ScreenX:", "").Replace('.', ',')); continue; }
			if(line.IndexOf("ScreenY:")==0) { ScreenY = (int)Convert.ToDouble(line.Replace("ScreenY:", "").Replace('.', ',')); continue; }
			if(line.IndexOf("WidthScreen:")==0) { WidthScreen = scale * Convert.ToDouble(line.Replace("WidthScreen:", "").Replace('.', ',')); continue; }
			if(line.IndexOf("HeightScreen:")==0) { HeightScreen = scale * Convert.ToDouble(line.Replace("HeightScreen:", "").Replace('.', ',')); continue; }
// Settings
			if(line.IndexOf("WidthSource:")==0) { WidthSource = scale * Convert.ToDouble(line.Replace("WidthSource:", "").Replace('.', ',')); continue; }
			if(line.IndexOf("HeightSource:")==0) { HeightSource = scale * Convert.ToDouble(line.Replace("HeightSource:", "").Replace('.', ',')); continue; }
			if(line.IndexOf("StartEnergy:")==0) { StartEnergy = Convert.ToDouble(line.Replace("StartEnergy:", "").Replace('.', ',')); continue; }
			if(line.IndexOf("LengthSourceObject:")==0) { LengthSourceObject = scale * Convert.ToDouble(line.Replace("LengthSourceObject:", "").Replace('.', ',')); continue; }
			if(line.IndexOf("LengthObjectLens:")==0) { LengthObjectLens = scale * Convert.ToDouble(line.Replace("LengthObjectLens:", "").Replace('.', ',')); continue; }
			if(line.IndexOf("LengthLensScreen:")==0) { LengthLensScreen = scale * Convert.ToDouble(line.Replace("LengthLensScreen:", "").Replace('.', ',')); continue; }
			if(line.IndexOf("MaxAngleSource:")==0) { MaxAngleSource = ScaleAngle * Convert.ToDouble(line.Replace("MaxAngleSource:", "").Replace('.', ',')); continue; }
			if(line.IndexOf("NStart:")==0) { NStart = Convert.ToDouble(line.Replace("NStart:", "").Replace('.', ',')); continue; }
// Steps
			if(line.IndexOf("AmountOfScattering:")==0) { AmountOfScattering = (int)Convert.ToDouble(line.Replace("AmountOfScattering:", "").Replace('.', ',')); continue; }
			if(line.IndexOf("MaxAngleScatering:")==0) { MaxAngleScatering = ScaleAngle * Convert.ToDouble(line.Replace("MaxAngleScatering:", "").Replace('.', ',')); continue; }
			if(line.IndexOf("Step:")==0) { Step = Convert.ToDouble(line.Replace("Step: ", "").Replace('.', ',')); continue; }
			if(line.IndexOf("AngleStep:")==0) { AngleStep = ScaleAngle * Convert.ToDouble(line.Replace("AngleStep:", "").Replace('.', ',')); continue; }
			if(line.IndexOf("AngleRadStep:")==0) { AngleRadStep = ScaleAngle * Convert.ToDouble(line.Replace("AngleRadStep:", "").Replace('.', ',')); continue; }
// Object
			if(line.IndexOf("ObjectWidth:")==0) { ObjectWidth = scale * Convert.ToDouble(line.Replace("ObjectWidth:", "").Replace('.', ',')); continue; }
			if(line.IndexOf("ObjectHeight:")==0) { ObjectHeight = scale * Convert.ToDouble(line.Replace("ObjectHeight:", "").Replace('.', ',')); continue; }
			if(line.IndexOf("ObjectThikness:")==0) { ObjectThikness = scale * Convert.ToDouble(line.Replace("ObjectThikness:", "").Replace('.', ',')); continue; }
// Count Of Threads
			if(line.IndexOf("CountOfThreads:")==0) { CountOfThreads = (int)Convert.ToDouble(line.Replace("CountOfThreads:", "").Replace('.', ',')); continue; }
			if(line.IndexOf("writePathOfFile:")==0) { writePathOfFile = line.Replace("writePathOfFile: ", ""); continue; }
// Type Of Material
			if(line.IndexOf("TypeMaterial: Ni")==0) { TypeMaterial = 1; continue; }
			if(line.IndexOf("TypeMaterial: Co")==0) { TypeMaterial = 2; continue; }
			if(line.IndexOf("TypeMaterial: Cu")==0) { TypeMaterial = 3; continue; }
			if(line.IndexOf("TypeMaterial: Au")==0) { TypeMaterial = 4; continue; }
			if(line.IndexOf("TypeMaterial: Al")==0) { TypeMaterial = 5; continue; }

			counter++;
		    }
		file.Close();
// Test
		System.Console.WriteLine("There were {0} lines.", counter);

		System.Console.WriteLine("Cell={0}", Cell);
		System.Console.WriteLine("Wall={0}", Wall);
		System.Console.WriteLine("thikness={0}", thikness);
		System.Console.WriteLine("density={0}", density);
		System.Console.WriteLine("WightToEdge={0}", WightToEdge);
		System.Console.WriteLine("HeightToEdge={0}", HeightToEdge);
		System.Console.WriteLine("LensWidth={0}", LensWidth);
		System.Console.WriteLine("LensHeight={0}", LensHeight);
		System.Console.WriteLine("ScreenX={0}", ScreenX);
		System.Console.WriteLine("ScreenY={0}", ScreenY);
		System.Console.WriteLine("WidthScreen={0}", WidthScreen);
		System.Console.WriteLine("HeightScreen={0}", HeightScreen);
		System.Console.WriteLine("WidthSource={0}", WidthSource);
		System.Console.WriteLine("HeightSource={0}", HeightSource);
		System.Console.WriteLine("StartEnergy={0}", StartEnergy);
		System.Console.WriteLine("LengthSourceObject={0}", LengthSourceObject);
		System.Console.WriteLine("LengthObjectLens={0}", LengthObjectLens);
		System.Console.WriteLine("LengthLensScreen={0}", LengthLensScreen);
		System.Console.WriteLine("MaxAngleSource={0}", MaxAngleSource);
		System.Console.WriteLine("NStart={0}", NStart);
		System.Console.WriteLine("AmountOfScattering={0}", AmountOfScattering);
		System.Console.WriteLine("MaxAngleScatering={0}", MaxAngleScatering);
		System.Console.WriteLine("Step={0}", Step);
		System.Console.WriteLine("AngleStep={0}", AngleStep);
		System.Console.WriteLine("AngleRadStep={0}", AngleRadStep);
		System.Console.WriteLine("ObjectWidth={0}", ObjectWidth);
		System.Console.WriteLine("ObjectHeight={0}", ObjectHeight);
		System.Console.WriteLine("ObjectThikness={0}", ObjectThikness);
		System.Console.WriteLine("CountOfThreads={0}", CountOfThreads);
		System.Console.WriteLine("writePathOfFile={0}", writePathOfFile);
		System.Console.WriteLine("TypeMaterial={0}", TypeMaterial);
            }
        }
        catch (IOException e)
        {
            Console.WriteLine("The file could not be read:");
            Console.WriteLine(e.Message);
        }
    }
}
