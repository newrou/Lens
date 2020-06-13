using System;
using System.Collections.Generic;
using System.ComponentModel;
//using System.Data;
//using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
//using System.Windows.Forms;
using System.Collections;
using System.IO;

public struct Vector2
{
    public double X;
    public double Y;

    public Vector2(double x, double y)
    {
        this.X = x;
        this.Y = y;
    }

    public static Vector2 operator +(Vector2 v1, Vector2 v2)
    {
        return new Vector2(v1.X + v2.X, v1.Y + v2.Y);
    }

    public static Vector2 operator -(Vector2 v1, Vector2 v2)
    {
        return new Vector2(v1.X - v2.X, v1.Y - v2.Y);
    }

    public static Vector2 operator *(Vector2 v1, double m)
    {
        return new Vector2(v1.X * m, v1.Y * m);
    }

    public static double operator *(Vector2 v1, Vector2 v2)
    {
        return v1.X * v2.X + v1.Y * v2.Y;
    }

    public static Vector2 operator /(Vector2 v1, double m)
    {
        return new Vector2(v1.X / m, v1.Y / m);
    }

    public static double Distance(Vector2 v1, Vector2 v2)
    {
        return (double)Math.Sqrt(Math.Pow(v1.X - v2.X, 2) + Math.Pow(v1.Y - v2.Y, 2));
    }

    public double Length()
    {
        return (double)Math.Sqrt(X * X + Y * Y);
    }
}

public struct Vector3
{
    public double X;
    public double Y;
    public double Z;

    public Vector3(double x, double y, double z)
    {
        this.X = x;
        this.Y = y;
        this.Z = z;
    }

    public static Vector3 operator +(Vector3 v1, Vector3 v2)
    {
        return new Vector3(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
    }

    public static Vector3 operator -(Vector3 v1, Vector3 v2)
    {
        return new Vector3(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
    }

    public static Vector3 operator *(Vector3 v1, double m)
    {
        return new Vector3(v1.X * m, v1.Y * m, v1.Z * m);
    }

    public static double operator *(Vector3 v1, Vector3 v2)
    {
        return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
    }

    public static Vector3 operator /(Vector3 v1, double m)
    {
        return new Vector3(v1.X / m, v1.Y / m, v1.Z / m);
    }

    public static double Distance(Vector3 v1, Vector3 v2)
    {
        return (double)Math.Sqrt(Math.Pow(v1.X - v2.X, 2) + Math.Pow(v1.Y - v2.Y, 2));
    }

    public double Length()
    {
        return (double)Math.Sqrt(X * X + Y * Y + Z * Z);
    }
}

//namespace Lens
class Lens
{
//    public partial class Form1 : Form
    public static void Main(string[] args)
    {
        // Масштаб в программе 1 юнит = 1 мкм
        int scale = 1000;
        int scale_cm2mm = 10;
        double PixelScale = 1;
        double ScaleAngle = Math.PI / 180;


        // Стандартные величины

        double r0 = 2.8179 * Math.Pow(10, -15);


        // Материалы
        // таблица поглощений

        double[,] PhotonInteractionCoefficients;

        double[,] PhotonInteractionCoefficientsNickel = new double[6, 90];
        double[,] PhotonInteractionCoefficientsGold = new double[6, 110];
        double[,] PhotonInteractionCoefficientsAluminium = new double[6, 84];

        // Молекулярные массы

        double m0;

        double m0Nickel = 58.6934;
        double m0Gold = 196.966569;
        double m0Aluminium = 26.9815386;


        // Плотности

        double densityNickel = 8.902;
        double densityGold = 19.32;
        double densityAluminium = 2.6989;

        //

        // Lens

        double Cell = 100; // μm
        double Wall = 30; // μm
        double thikness = 1000; // μm
        double density = 8.902; // g/cm3
        double WightToEdge = 20; // μm
        double HeightToEdge = 20; // μm
        double LensWidth = 20; // mm
        double LensHeight = 20; // mm

        // Screen

        int ScreenX = 10000; // px
        int ScreenY = 10000; // px
        double WidthScreen = 100; // mm
        double HeightScreen = 100; // mm

        // Settings

        double WidthSource = 10; // mm
        double HeightSource = 10; // mm
        double StartEnergy = 30; // keV
        double LengthSourceObject = 100; // mm
        double LengthObjectLens = 0; // mm
        double LengthLensScreen = 1000; // mm
        double MaxAngleSource = 10; // rad
        double NStart = 100;

        // Steps

        int AmountOfScattering = 0;
        double MaxAngleScatering = 5; // rad
        double Step = 100; // μm
        double AngleStep = 0.1; // rad
        double AngleRadStep = 10; // rad

        // Object

        double ObjectWidth = 10; // mm
        double ObjectHeight = 10; // mm
        double ObjectThikness = 10; // mm

	int TypeMaterial = 0;

        // Нужные переменные

        // Solve settings

        int CountOfThreads = 1;

        //int CountOfTable = 30;
        //Vector2[] ReducingMount;

        // Настройки для полей
        string[] ArrayOfNames = new string[]
        {"Cell, μm", "Wall, μm", "thikness, μm", "density g/cm3", "WightToEdge, μm", "HeightToEdge, μm", "LensWidth, mm", "LensHeight, mm",
            "ScreenX", "ScreenY", "WidthScreen, mm", "HeightScreen, mm",
            "WidthSource, mm", "HeightSource, mm", "StartEnergy, keV", "Source-Object, mm", "Object-Lens, mm", "Lens-Screen, mm", "MaxAngleSource", "N",
            "AmountOfScattering", "MaxAngleScatering", "Step, μm", "AngleStep","AngleRadStep",
            "ObjectWidth, mm", "ObjectHeight, mm", "ObjectThikness, mm",
            "CountOfThreads" };

        int[] Grupps = new int[] { 8, 4, 8, 5, 3, 1 };
        string[] NameOfGrupps = new string[] { "Lens", "Screen", "Settings of Source", "Steps", "Object", "Solve settings" };

        double[] Parameters = new double[]
        {100, 30, 1000, 8.902, 20, 20, 20, 20,
            10000, 10000, 100, 100,
            10, 10, 30, 100, 0, 1000, 10, 100,
            0, 10, 100, 0.1, 10,
            10, 10, 10,
            1 };

//        Label[] LabelBox;
//        TextBox[] ArrayTextBox;

        // Переменные для расчётов
        public double[,] ArrayPointScreen;
        public double[,] ArrayPointScreenOfOne;
        double r;
        double W1, W2, W3, W4;
        double H1, H2, H3, H4;

        public double[,] ArrayPoint1;
        public double[,] ArrayPoint2;
        public double[,] ArrayPoint3;
        public double[,] ArrayPoint4;

        //ComboBox comboBoxMaterial;

        TextBox NameTextBox = new TextBox();
        Label LabelNameTextBox = new Label();
        string writePathOfFolder = @"C:\Lens\";
        string writePathOfFile = "Test1";
        string writeOfExtension = ".txt";

        // Потоки и их 
        List<Thread> myThreads = new List<Thread>();
        List<int> ProgressTreads = new List<int>();

        // Инициализация Form
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Console.WriteLine(PhotonInteractionCoefficients.Length / 6);
            Initialization();
        }

        // Получение параметров задачи
        void Initialization()
        {
            SetTables();

            CreateUI();
            GetSettings();
            CreateArrayScreen();
        }

        // Запись таблиц уменьшения
        void SetTables()
        {

            PhotonInteractionCoefficientsNickel[0, 0] = 1.00 * Math.Pow(10, -01); PhotonInteractionCoefficientsNickel[1, 0] = 5.90 * Math.Pow(10, +04); PhotonInteractionCoefficientsNickel[2, 0] = 5.90 * Math.Pow(10, +04); PhotonInteractionCoefficientsNickel[3, 0] = 1.13 * Math.Pow(10, -04); PhotonInteractionCoefficientsNickel[4, 0] = 5.34 * Math.Pow(10, +00); PhotonInteractionCoefficientsNickel[5, 0] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsNickel[0, 1] = 1.11 * Math.Pow(10, -01); PhotonInteractionCoefficientsNickel[1, 1] = 5.35 * Math.Pow(10, +04); PhotonInteractionCoefficientsNickel[2, 1] = 5.35 * Math.Pow(10, +04); PhotonInteractionCoefficientsNickel[3, 1] = 1.36 * Math.Pow(10, -04); PhotonInteractionCoefficientsNickel[4, 1] = 5.34 * Math.Pow(10, +00); PhotonInteractionCoefficientsNickel[5, 1] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsNickel[0, 2] = 1.11 * Math.Pow(10, -01); PhotonInteractionCoefficientsNickel[1, 2] = 5.64 * Math.Pow(10, +04); PhotonInteractionCoefficientsNickel[2, 2] = 5.64 * Math.Pow(10, +04); PhotonInteractionCoefficientsNickel[3, 2] = 1.39 * Math.Pow(10, -04); PhotonInteractionCoefficientsNickel[4, 2] = 5.34 * Math.Pow(10, +00); PhotonInteractionCoefficientsNickel[5, 2] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsNickel[0, 3] = 1.50 * Math.Pow(10, -01); PhotonInteractionCoefficientsNickel[1, 3] = 4.29 * Math.Pow(10, +04); PhotonInteractionCoefficientsNickel[2, 3] = 4.29 * Math.Pow(10, +04); PhotonInteractionCoefficientsNickel[3, 3] = 2.37 * Math.Pow(10, -04); PhotonInteractionCoefficientsNickel[4, 3] = 5.34 * Math.Pow(10, +00); PhotonInteractionCoefficientsNickel[5, 3] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsNickel[0, 4] = 2.00 * Math.Pow(10, -01); PhotonInteractionCoefficientsNickel[1, 4] = 2.91 * Math.Pow(10, +04); PhotonInteractionCoefficientsNickel[2, 4] = 2.91 * Math.Pow(10, +04); PhotonInteractionCoefficientsNickel[3, 4] = 4.09 * Math.Pow(10, -04); PhotonInteractionCoefficientsNickel[4, 4] = 5.33 * Math.Pow(10, +00); PhotonInteractionCoefficientsNickel[5, 4] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsNickel[0, 5] = 3.00 * Math.Pow(10, -01); PhotonInteractionCoefficientsNickel[1, 5] = 1.52 * Math.Pow(10, +04); PhotonInteractionCoefficientsNickel[2, 5] = 1.52 * Math.Pow(10, +04); PhotonInteractionCoefficientsNickel[3, 5] = 8.94 * Math.Pow(10, -04); PhotonInteractionCoefficientsNickel[4, 5] = 5.33 * Math.Pow(10, +00); PhotonInteractionCoefficientsNickel[5, 5] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsNickel[0, 6] = 4.00 * Math.Pow(10, -01); PhotonInteractionCoefficientsNickel[1, 6] = 8.92 * Math.Pow(10, +03); PhotonInteractionCoefficientsNickel[2, 6] = 8.92 * Math.Pow(10, +03); PhotonInteractionCoefficientsNickel[3, 6] = 1.55 * Math.Pow(10, -03); PhotonInteractionCoefficientsNickel[4, 6] = 5.30 * Math.Pow(10, +00); PhotonInteractionCoefficientsNickel[5, 6] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsNickel[0, 7] = 5.00 * Math.Pow(10, -01); PhotonInteractionCoefficientsNickel[1, 7] = 5.84 * Math.Pow(10, +03); PhotonInteractionCoefficientsNickel[2, 7] = 5.84 * Math.Pow(10, +03); PhotonInteractionCoefficientsNickel[3, 7] = 2.36 * Math.Pow(10, -03); PhotonInteractionCoefficientsNickel[4, 7] = 5.27 * Math.Pow(10, +00); PhotonInteractionCoefficientsNickel[5, 7] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsNickel[0, 8] = 6.00 * Math.Pow(10, -01); PhotonInteractionCoefficientsNickel[1, 8] = 4.03 * Math.Pow(10, +03); PhotonInteractionCoefficientsNickel[2, 8] = 4.03 * Math.Pow(10, +03); PhotonInteractionCoefficientsNickel[3, 8] = 3.30 * Math.Pow(10, -03); PhotonInteractionCoefficientsNickel[4, 8] = 5.24 * Math.Pow(10, +00); PhotonInteractionCoefficientsNickel[5, 8] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsNickel[0, 9] = 8.00 * Math.Pow(10, -01); PhotonInteractionCoefficientsNickel[1, 9] = 2.17 * Math.Pow(10, +03); PhotonInteractionCoefficientsNickel[2, 9] = 2.17 * Math.Pow(10, +03); PhotonInteractionCoefficientsNickel[3, 9] = 5.45 * Math.Pow(10, -03); PhotonInteractionCoefficientsNickel[4, 9] = 5.15 * Math.Pow(10, +00); PhotonInteractionCoefficientsNickel[5, 9] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsNickel[0, 10] = 8.45 * Math.Pow(10, -01); PhotonInteractionCoefficientsNickel[1, 10] = 1.79 * Math.Pow(10, +03); PhotonInteractionCoefficientsNickel[2, 10] = 1.79 * Math.Pow(10, +03); PhotonInteractionCoefficientsNickel[3, 10] = 6.14 * Math.Pow(10, -03); PhotonInteractionCoefficientsNickel[4, 10] = 5.13 * Math.Pow(10, +00); PhotonInteractionCoefficientsNickel[5, 10] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsNickel[0, 11] = 8.45 * Math.Pow(10, -01); PhotonInteractionCoefficientsNickel[1, 11] = 6.47 * Math.Pow(10, +03); PhotonInteractionCoefficientsNickel[2, 11] = 6.47 * Math.Pow(10, +03); PhotonInteractionCoefficientsNickel[3, 11] = 6.16 * Math.Pow(10, -03); PhotonInteractionCoefficientsNickel[4, 11] = 5.13 * Math.Pow(10, +00); PhotonInteractionCoefficientsNickel[5, 11] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsNickel[0, 12] = 8.71 * Math.Pow(10, -01); PhotonInteractionCoefficientsNickel[1, 12] = 6.48 * Math.Pow(10, +03); PhotonInteractionCoefficientsNickel[2, 12] = 6.48 * Math.Pow(10, +03); PhotonInteractionCoefficientsNickel[3, 12] = 6.16 * Math.Pow(10, -03); PhotonInteractionCoefficientsNickel[4, 12] = 5.13 * Math.Pow(10, +00); PhotonInteractionCoefficientsNickel[5, 12] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsNickel[0, 13] = 8.71 * Math.Pow(10, -01); PhotonInteractionCoefficientsNickel[1, 13] = 1.26 * Math.Pow(10, +04); PhotonInteractionCoefficientsNickel[2, 13] = 1.26 * Math.Pow(10, +04); PhotonInteractionCoefficientsNickel[3, 13] = 6.62 * Math.Pow(10, -03); PhotonInteractionCoefficientsNickel[4, 13] = 5.11 * Math.Pow(10, +00); PhotonInteractionCoefficientsNickel[5, 13] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsNickel[0, 14] = 1.00 * Math.Pow(10, +00); PhotonInteractionCoefficientsNickel[1, 14] = 9.86 * Math.Pow(10, +03); PhotonInteractionCoefficientsNickel[2, 14] = 9.85 * Math.Pow(10, +03); PhotonInteractionCoefficientsNickel[3, 14] = 7.81 * Math.Pow(10, -03); PhotonInteractionCoefficientsNickel[4, 14] = 5.05 * Math.Pow(10, +00); PhotonInteractionCoefficientsNickel[5, 14] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsNickel[0, 15] = 1.01 * Math.Pow(10, +00); PhotonInteractionCoefficientsNickel[1, 15] = 9.66 * Math.Pow(10, +03); PhotonInteractionCoefficientsNickel[2, 15] = 9.65 * Math.Pow(10, +03); PhotonInteractionCoefficientsNickel[3, 15] = 7.91 * Math.Pow(10, -03); PhotonInteractionCoefficientsNickel[4, 15] = 5.05 * Math.Pow(10, +00); PhotonInteractionCoefficientsNickel[5, 15] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsNickel[0, 16] = 1.01 * Math.Pow(10, +00); PhotonInteractionCoefficientsNickel[1, 16] = 1.10 * Math.Pow(10, +04); PhotonInteractionCoefficientsNickel[2, 16] = 1.10 * Math.Pow(10, +04); PhotonInteractionCoefficientsNickel[3, 16] = 7.91 * Math.Pow(10, -03); PhotonInteractionCoefficientsNickel[4, 16] = 5.05 * Math.Pow(10, +00); PhotonInteractionCoefficientsNickel[5, 16] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsNickel[0, 17] = 1.50 * Math.Pow(10, +00); PhotonInteractionCoefficientsNickel[1, 17] = 4.23 * Math.Pow(10, +03); PhotonInteractionCoefficientsNickel[2, 17] = 4.23 * Math.Pow(10, +03); PhotonInteractionCoefficientsNickel[3, 17] = 1.39 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[4, 17] = 4.76 * Math.Pow(10, +00); PhotonInteractionCoefficientsNickel[5, 17] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsNickel[0, 18] = 2.00 * Math.Pow(10, +00); PhotonInteractionCoefficientsNickel[1, 18] = 2.05 * Math.Pow(10, +03); PhotonInteractionCoefficientsNickel[2, 18] = 2.04 * Math.Pow(10, +03); PhotonInteractionCoefficientsNickel[3, 18] = 1.96 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[4, 18] = 4.45 * Math.Pow(10, +00); PhotonInteractionCoefficientsNickel[5, 18] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsNickel[0, 19] = 3.00 * Math.Pow(10, +00); PhotonInteractionCoefficientsNickel[1, 19] = 7.10 * Math.Pow(10, +02); PhotonInteractionCoefficientsNickel[2, 19] = 7.06 * Math.Pow(10, +02); PhotonInteractionCoefficientsNickel[3, 19] = 3.00 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[4, 19] = 3.84 * Math.Pow(10, +00); PhotonInteractionCoefficientsNickel[5, 19] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsNickel[0, 20] = 4.00 * Math.Pow(10, +00); PhotonInteractionCoefficientsNickel[1, 20] = 3.28 * Math.Pow(10, +02); PhotonInteractionCoefficientsNickel[2, 20] = 3.25 * Math.Pow(10, +02); PhotonInteractionCoefficientsNickel[3, 20] = 3.98 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[4, 20] = 3.29 * Math.Pow(10, +00); PhotonInteractionCoefficientsNickel[5, 20] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsNickel[0, 21] = 5.00 * Math.Pow(10, +00); PhotonInteractionCoefficientsNickel[1, 21] = 1.79 * Math.Pow(10, +02); PhotonInteractionCoefficientsNickel[2, 21] = 1.76 * Math.Pow(10, +02); PhotonInteractionCoefficientsNickel[3, 21] = 4.90 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[4, 21] = 2.82 * Math.Pow(10, +00); PhotonInteractionCoefficientsNickel[5, 21] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsNickel[0, 22] = 6.00 * Math.Pow(10, +00); PhotonInteractionCoefficientsNickel[1, 22] = 1.09 * Math.Pow(10, +02); PhotonInteractionCoefficientsNickel[2, 22] = 1.07 * Math.Pow(10, +02); PhotonInteractionCoefficientsNickel[3, 22] = 5.73 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[4, 22] = 2.42 * Math.Pow(10, +00); PhotonInteractionCoefficientsNickel[5, 22] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsNickel[0, 23] = 8.00 * Math.Pow(10, +00); PhotonInteractionCoefficientsNickel[1, 23] = 4.95 * Math.Pow(10, +01); PhotonInteractionCoefficientsNickel[2, 23] = 4.76 * Math.Pow(10, +01); PhotonInteractionCoefficientsNickel[3, 23] = 7.17 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[4, 23] = 1.81 * Math.Pow(10, +00); PhotonInteractionCoefficientsNickel[5, 23] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsNickel[0, 24] = 8.33 * Math.Pow(10, +00); PhotonInteractionCoefficientsNickel[1, 24] = 4.43 * Math.Pow(10, +01); PhotonInteractionCoefficientsNickel[2, 24] = 4.25 * Math.Pow(10, +01); PhotonInteractionCoefficientsNickel[3, 24] = 7.39 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[4, 24] = 1.73 * Math.Pow(10, +00); PhotonInteractionCoefficientsNickel[5, 24] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsNickel[0, 25] = 8.33 * Math.Pow(10, +00); PhotonInteractionCoefficientsNickel[1, 25] = 3.29 * Math.Pow(10, +02); PhotonInteractionCoefficientsNickel[2, 25] = 3.28 * Math.Pow(10, +02); PhotonInteractionCoefficientsNickel[3, 25] = 7.39 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[4, 25] = 1.73 * Math.Pow(10, +00); PhotonInteractionCoefficientsNickel[5, 25] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsNickel[0, 26] = 1.00 * Math.Pow(10, +01); PhotonInteractionCoefficientsNickel[1, 26] = 2.09 * Math.Pow(10, +02); PhotonInteractionCoefficientsNickel[2, 26] = 2.07 * Math.Pow(10, +02); PhotonInteractionCoefficientsNickel[3, 26] = 8.36 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[4, 26] = 1.41 * Math.Pow(10, +00); PhotonInteractionCoefficientsNickel[5, 26] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsNickel[0, 27] = 1.50 * Math.Pow(10, +01); PhotonInteractionCoefficientsNickel[1, 27] = 7.08 * Math.Pow(10, +01); PhotonInteractionCoefficientsNickel[2, 27] = 6.98 * Math.Pow(10, +01); PhotonInteractionCoefficientsNickel[3, 27] = 1.04 * Math.Pow(10, -01); PhotonInteractionCoefficientsNickel[4, 27] = 8.60 * Math.Pow(10, -01); PhotonInteractionCoefficientsNickel[5, 27] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsNickel[0, 28] = 2.00 * Math.Pow(10, +01); PhotonInteractionCoefficientsNickel[1, 28] = 3.22 * Math.Pow(10, +01); PhotonInteractionCoefficientsNickel[2, 28] = 3.15 * Math.Pow(10, +01); PhotonInteractionCoefficientsNickel[3, 28] = 1.16 * Math.Pow(10, -01); PhotonInteractionCoefficientsNickel[4, 28] = 5.95 * Math.Pow(10, -01); PhotonInteractionCoefficientsNickel[5, 28] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsNickel[0, 29] = 3.00 * Math.Pow(10, +01); PhotonInteractionCoefficientsNickel[1, 29] = 1.03 * Math.Pow(10, +01); PhotonInteractionCoefficientsNickel[2, 29] = 9.88 * Math.Pow(10, +00); PhotonInteractionCoefficientsNickel[3, 29] = 1.30 * Math.Pow(10, -01); PhotonInteractionCoefficientsNickel[4, 29] = 3.30 * Math.Pow(10, -01); PhotonInteractionCoefficientsNickel[5, 29] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsNickel[0, 30] = 4.00 * Math.Pow(10, +01); PhotonInteractionCoefficientsNickel[1, 30] = 4.60 * Math.Pow(10, +00); PhotonInteractionCoefficientsNickel[2, 30] = 4.26 * Math.Pow(10, +00); PhotonInteractionCoefficientsNickel[3, 30] = 1.36 * Math.Pow(10, -01); PhotonInteractionCoefficientsNickel[4, 30] = 2.08 * Math.Pow(10, -01); PhotonInteractionCoefficientsNickel[5, 30] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsNickel[0, 31] = 5.00 * Math.Pow(10, +01); PhotonInteractionCoefficientsNickel[1, 31] = 2.47 * Math.Pow(10, +00); PhotonInteractionCoefficientsNickel[2, 31] = 2.19 * Math.Pow(10, +00); PhotonInteractionCoefficientsNickel[3, 31] = 1.38 * Math.Pow(10, -01); PhotonInteractionCoefficientsNickel[4, 31] = 1.44 * Math.Pow(10, -01); PhotonInteractionCoefficientsNickel[5, 31] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsNickel[0, 32] = 6.00 * Math.Pow(10, +01); PhotonInteractionCoefficientsNickel[1, 32] = 1.51 * Math.Pow(10, +00); PhotonInteractionCoefficientsNickel[2, 32] = 1.27 * Math.Pow(10, +00); PhotonInteractionCoefficientsNickel[3, 32] = 1.38 * Math.Pow(10, -01); PhotonInteractionCoefficientsNickel[4, 32] = 1.06 * Math.Pow(10, -01); PhotonInteractionCoefficientsNickel[5, 32] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsNickel[0, 33] = 8.00 * Math.Pow(10, +01); PhotonInteractionCoefficientsNickel[1, 33] = 7.31 * Math.Pow(10, -01); PhotonInteractionCoefficientsNickel[2, 33] = 5.30 * Math.Pow(10, -01); PhotonInteractionCoefficientsNickel[3, 33] = 1.36 * Math.Pow(10, -01); PhotonInteractionCoefficientsNickel[4, 33] = 6.48 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[5, 33] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsNickel[0, 34] = 1.00 * Math.Pow(10, +02); PhotonInteractionCoefficientsNickel[1, 34] = 4.44 * Math.Pow(10, -01); PhotonInteractionCoefficientsNickel[2, 34] = 2.68 * Math.Pow(10, -01); PhotonInteractionCoefficientsNickel[3, 34] = 1.32 * Math.Pow(10, -01); PhotonInteractionCoefficientsNickel[4, 34] = 4.37 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[5, 34] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsNickel[0, 35] = 1.50 * Math.Pow(10, +02); PhotonInteractionCoefficientsNickel[1, 35] = 2.21 * Math.Pow(10, -01); PhotonInteractionCoefficientsNickel[2, 35] = 7.75 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[3, 35] = 1.23 * Math.Pow(10, -01); PhotonInteractionCoefficientsNickel[4, 35] = 2.07 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[5, 35] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsNickel[0, 36] = 2.00 * Math.Pow(10, +02); PhotonInteractionCoefficientsNickel[1, 36] = 1.58 * Math.Pow(10, -01); PhotonInteractionCoefficientsNickel[2, 36] = 3.23 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[3, 36] = 1.14 * Math.Pow(10, -01); PhotonInteractionCoefficientsNickel[4, 36] = 1.20 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[5, 36] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsNickel[0, 37] = 3.00 * Math.Pow(10, +02); PhotonInteractionCoefficientsNickel[1, 37] = 1.15 * Math.Pow(10, -01); PhotonInteractionCoefficientsNickel[2, 37] = 9.70 * Math.Pow(10, -03); PhotonInteractionCoefficientsNickel[3, 37] = 1.00 * Math.Pow(10, -01); PhotonInteractionCoefficientsNickel[4, 37] = 5.51 * Math.Pow(10, -03); PhotonInteractionCoefficientsNickel[5, 37] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsNickel[0, 38] = 4.00 * Math.Pow(10, +02); PhotonInteractionCoefficientsNickel[1, 38] = 9.76 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[2, 38] = 4.30 * Math.Pow(10, -03); PhotonInteractionCoefficientsNickel[3, 38] = 9.02 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[4, 38] = 3.14 * Math.Pow(10, -03); PhotonInteractionCoefficientsNickel[5, 38] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsNickel[0, 39] = 5.00 * Math.Pow(10, +02); PhotonInteractionCoefficientsNickel[1, 39] = 8.70 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[2, 39] = 2.37 * Math.Pow(10, -03); PhotonInteractionCoefficientsNickel[3, 39] = 8.26 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[4, 39] = 2.03 * Math.Pow(10, -03); PhotonInteractionCoefficientsNickel[5, 39] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsNickel[0, 40] = 6.00 * Math.Pow(10, +02); PhotonInteractionCoefficientsNickel[1, 40] = 7.94 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[2, 40] = 1.49 * Math.Pow(10, -03); PhotonInteractionCoefficientsNickel[3, 40] = 7.65 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[4, 40] = 1.41 * Math.Pow(10, -03); PhotonInteractionCoefficientsNickel[5, 40] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsNickel[0, 41] = 8.00 * Math.Pow(10, +02); PhotonInteractionCoefficientsNickel[1, 41] = 6.89 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[2, 41] = 7.60 * Math.Pow(10, -04); PhotonInteractionCoefficientsNickel[3, 41] = 6.74 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[4, 41] = 7.98 * Math.Pow(10, -04); PhotonInteractionCoefficientsNickel[5, 41] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsNickel[0, 42] = 1.00 * Math.Pow(10, +03); PhotonInteractionCoefficientsNickel[1, 42] = 6.16 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[2, 42] = 4.73 * Math.Pow(10, -04); PhotonInteractionCoefficientsNickel[3, 42] = 6.06 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[4, 42] = 5.12 * Math.Pow(10, -04); PhotonInteractionCoefficientsNickel[5, 42] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsNickel[0, 43] = 1.02 * Math.Pow(10, +03); PhotonInteractionCoefficientsNickel[1, 43] = 6.09 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[2, 43] = 4.49 * Math.Pow(10, -04); PhotonInteractionCoefficientsNickel[3, 43] = 6.00 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[4, 43] = 4.90 * Math.Pow(10, -04); PhotonInteractionCoefficientsNickel[5, 43] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsNickel[0, 44] = 1.25 * Math.Pow(10, +03); PhotonInteractionCoefficientsNickel[1, 44] = 5.49 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[2, 44] = 3.04 * Math.Pow(10, -04); PhotonInteractionCoefficientsNickel[3, 44] = 5.42 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[4, 44] = 3.28 * Math.Pow(10, -04); PhotonInteractionCoefficientsNickel[5, 44] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsNickel[0, 45] = 1.50 * Math.Pow(10, +03); PhotonInteractionCoefficientsNickel[1, 45] = 5.02 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[2, 45] = 2.19 * Math.Pow(10, -04); PhotonInteractionCoefficientsNickel[3, 45] = 4.93 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[4, 45] = 2.28 * Math.Pow(10, -04); PhotonInteractionCoefficientsNickel[5, 45] = 4.02 * Math.Pow(10, -04);
            PhotonInteractionCoefficientsNickel[0, 46] = 2.00 * Math.Pow(10, +03); PhotonInteractionCoefficientsNickel[1, 46] = 4.39 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[2, 46] = 1.35 * Math.Pow(10, -04); PhotonInteractionCoefficientsNickel[3, 46] = 4.21 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[4, 46] = 1.28 * Math.Pow(10, -04); PhotonInteractionCoefficientsNickel[5, 46] = 1.52 * Math.Pow(10, -03);
            PhotonInteractionCoefficientsNickel[0, 47] = 3.00 * Math.Pow(10, +03); PhotonInteractionCoefficientsNickel[1, 47] = 3.75 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[2, 47] = 7.30 * Math.Pow(10, -05); PhotonInteractionCoefficientsNickel[3, 47] = 3.31 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[4, 47] = 5.71 * Math.Pow(10, -05); PhotonInteractionCoefficientsNickel[5, 47] = 4.20 * Math.Pow(10, -03);
            PhotonInteractionCoefficientsNickel[0, 48] = 4.00 * Math.Pow(10, +03); PhotonInteractionCoefficientsNickel[1, 48] = 3.44 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[2, 48] = 4.91 * Math.Pow(10, -05); PhotonInteractionCoefficientsNickel[3, 48] = 2.76 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[4, 48] = 3.21 * Math.Pow(10, -05); PhotonInteractionCoefficientsNickel[5, 48] = 6.72 * Math.Pow(10, -03);
            PhotonInteractionCoefficientsNickel[0, 49] = 5.00 * Math.Pow(10, +03); PhotonInteractionCoefficientsNickel[1, 49] = 3.29 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[2, 49] = 3.67 * Math.Pow(10, -05); PhotonInteractionCoefficientsNickel[3, 49] = 2.39 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[4, 49] = 2.06 * Math.Pow(10, -05); PhotonInteractionCoefficientsNickel[5, 49] = 8.96 * Math.Pow(10, -03);
            PhotonInteractionCoefficientsNickel[0, 50] = 6.00 * Math.Pow(10, +03); PhotonInteractionCoefficientsNickel[1, 50] = 3.21 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[2, 50] = 2.92 * Math.Pow(10, -05); PhotonInteractionCoefficientsNickel[3, 50] = 2.11 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[4, 50] = 1.43 * Math.Pow(10, -05); PhotonInteractionCoefficientsNickel[5, 50] = 1.10 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsNickel[0, 51] = 7.00 * Math.Pow(10, +03); PhotonInteractionCoefficientsNickel[1, 51] = 3.17 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[2, 51] = 2.42 * Math.Pow(10, -05); PhotonInteractionCoefficientsNickel[3, 51] = 1.90 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[4, 51] = 1.05 * Math.Pow(10, -05); PhotonInteractionCoefficientsNickel[5, 51] = 1.27 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsNickel[0, 52] = 8.00 * Math.Pow(10, +03); PhotonInteractionCoefficientsNickel[1, 52] = 3.16 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[2, 52] = 2.07 * Math.Pow(10, -05); PhotonInteractionCoefficientsNickel[3, 52] = 1.73 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[4, 52] = 8.03 * Math.Pow(10, -06); PhotonInteractionCoefficientsNickel[5, 52] = 1.44 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsNickel[0, 53] = 9.00 * Math.Pow(10, +03); PhotonInteractionCoefficientsNickel[1, 53] = 3.17 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[2, 53] = 1.80 * Math.Pow(10, -05); PhotonInteractionCoefficientsNickel[3, 53] = 1.59 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[4, 53] = 6.35 * Math.Pow(10, -06); PhotonInteractionCoefficientsNickel[5, 53] = 1.58 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsNickel[0, 54] = 1.00 * Math.Pow(10, +04); PhotonInteractionCoefficientsNickel[1, 54] = 3.18 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[2, 54] = 1.59 * Math.Pow(10, -05); PhotonInteractionCoefficientsNickel[3, 54] = 1.47 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[4, 54] = 5.14 * Math.Pow(10, -06); PhotonInteractionCoefficientsNickel[5, 54] = 1.71 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsNickel[0, 55] = 1.20 * Math.Pow(10, +04); PhotonInteractionCoefficientsNickel[1, 55] = 3.23 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[2, 55] = 1.30 * Math.Pow(10, -05); PhotonInteractionCoefficientsNickel[3, 55] = 1.29 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[4, 55] = 3.57 * Math.Pow(10, -06); PhotonInteractionCoefficientsNickel[5, 55] = 1.94 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsNickel[0, 56] = 1.50 * Math.Pow(10, +04); PhotonInteractionCoefficientsNickel[1, 56] = 3.32 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[2, 56] = 1.01 * Math.Pow(10, -05); PhotonInteractionCoefficientsNickel[3, 56] = 1.09 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[4, 56] = 2.28 * Math.Pow(10, -06); PhotonInteractionCoefficientsNickel[5, 56] = 2.23 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsNickel[0, 57] = 2.00 * Math.Pow(10, +04); PhotonInteractionCoefficientsNickel[1, 57] = 3.48 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[2, 57] = 7.39 * Math.Pow(10, -06); PhotonInteractionCoefficientsNickel[3, 57] = 8.73 * Math.Pow(10, -03); PhotonInteractionCoefficientsNickel[4, 57] = 1.28 * Math.Pow(10, -06); PhotonInteractionCoefficientsNickel[5, 57] = 2.60 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsNickel[0, 58] = 2.20 * Math.Pow(10, +04); PhotonInteractionCoefficientsNickel[1, 58] = 3.54 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[2, 58] = 6.67 * Math.Pow(10, -06); PhotonInteractionCoefficientsNickel[3, 58] = 8.11 * Math.Pow(10, -03); PhotonInteractionCoefficientsNickel[4, 58] = 1.06 * Math.Pow(10, -06); PhotonInteractionCoefficientsNickel[5, 58] = 2.73 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsNickel[0, 59] = 2.60 * Math.Pow(10, +04); PhotonInteractionCoefficientsNickel[1, 59] = 3.65 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[2, 59] = 5.58 * Math.Pow(10, -06); PhotonInteractionCoefficientsNickel[3, 59] = 7.12 * Math.Pow(10, -03); PhotonInteractionCoefficientsNickel[4, 59] = 7.60 * Math.Pow(10, -07); PhotonInteractionCoefficientsNickel[5, 59] = 2.94 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsNickel[0, 60] = 3.00 * Math.Pow(10, +04); PhotonInteractionCoefficientsNickel[1, 60] = 3.76 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[2, 60] = 4.80 * Math.Pow(10, -06); PhotonInteractionCoefficientsNickel[3, 60] = 6.36 * Math.Pow(10, -03); PhotonInteractionCoefficientsNickel[4, 60] = 5.71 * Math.Pow(10, -07); PhotonInteractionCoefficientsNickel[5, 60] = 3.12 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsNickel[0, 61] = 4.00 * Math.Pow(10, +04); PhotonInteractionCoefficientsNickel[1, 61] = 3.98 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[2, 61] = 3.55 * Math.Pow(10, -06); PhotonInteractionCoefficientsNickel[3, 61] = 5.05 * Math.Pow(10, -03); PhotonInteractionCoefficientsNickel[4, 61] = 3.21 * Math.Pow(10, -07); PhotonInteractionCoefficientsNickel[5, 61] = 3.48 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsNickel[0, 62] = 5.00 * Math.Pow(10, +04); PhotonInteractionCoefficientsNickel[1, 62] = 4.17 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[2, 62] = 2.82 * Math.Pow(10, -06); PhotonInteractionCoefficientsNickel[3, 62] = 4.22 * Math.Pow(10, -03); PhotonInteractionCoefficientsNickel[4, 62] = 2.06 * Math.Pow(10, -07); PhotonInteractionCoefficientsNickel[5, 62] = 3.75 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsNickel[0, 63] = 6.00 * Math.Pow(10, +04); PhotonInteractionCoefficientsNickel[1, 63] = 4.32 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[2, 63] = 2.34 * Math.Pow(10, -06); PhotonInteractionCoefficientsNickel[3, 63] = 3.63 * Math.Pow(10, -03); PhotonInteractionCoefficientsNickel[4, 63] = 1.43 * Math.Pow(10, -07); PhotonInteractionCoefficientsNickel[5, 63] = 3.95 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsNickel[0, 64] = 8.00 * Math.Pow(10, +04); PhotonInteractionCoefficientsNickel[1, 64] = 4.55 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[2, 64] = 1.74 * Math.Pow(10, -06); PhotonInteractionCoefficientsNickel[3, 64] = 2.86 * Math.Pow(10, -03); PhotonInteractionCoefficientsNickel[4, 64] = 8.03 * Math.Pow(10, -08); PhotonInteractionCoefficientsNickel[5, 64] = 4.27 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsNickel[0, 65] = 1.00 * Math.Pow(10, +05); PhotonInteractionCoefficientsNickel[1, 65] = 4.73 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[2, 65] = 1.39 * Math.Pow(10, -06); PhotonInteractionCoefficientsNickel[3, 65] = 2.38 * Math.Pow(10, -03); PhotonInteractionCoefficientsNickel[4, 65] = 5.14 * Math.Pow(10, -08); PhotonInteractionCoefficientsNickel[5, 65] = 4.49 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsNickel[0, 66] = 1.50 * Math.Pow(10, +05); PhotonInteractionCoefficientsNickel[1, 66] = 5.01 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[2, 66] = 9.20 * Math.Pow(10, -07); PhotonInteractionCoefficientsNickel[3, 66] = 1.69 * Math.Pow(10, -03); PhotonInteractionCoefficientsNickel[4, 66] = 2.28 * Math.Pow(10, -08); PhotonInteractionCoefficientsNickel[5, 66] = 4.85 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsNickel[0, 67] = 2.00 * Math.Pow(10, +05); PhotonInteractionCoefficientsNickel[1, 67] = 5.19 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[2, 67] = 6.88 * Math.Pow(10, -07); PhotonInteractionCoefficientsNickel[3, 67] = 1.33 * Math.Pow(10, -03); PhotonInteractionCoefficientsNickel[4, 67] = 1.28 * Math.Pow(10, -08); PhotonInteractionCoefficientsNickel[5, 67] = 5.06 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsNickel[0, 68] = 3.00 * Math.Pow(10, +05); PhotonInteractionCoefficientsNickel[1, 68] = 5.41 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[2, 68] = 4.58 * Math.Pow(10, -07); PhotonInteractionCoefficientsNickel[3, 68] = 9.41 * Math.Pow(10, -04); PhotonInteractionCoefficientsNickel[4, 68] = 5.71 * Math.Pow(10, -09); PhotonInteractionCoefficientsNickel[5, 68] = 5.31 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsNickel[0, 69] = 4.00 * Math.Pow(10, +05); PhotonInteractionCoefficientsNickel[1, 69] = 5.53 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[2, 69] = 3.43 * Math.Pow(10, -07); PhotonInteractionCoefficientsNickel[3, 69] = 7.37 * Math.Pow(10, -04); PhotonInteractionCoefficientsNickel[4, 69] = 3.21 * Math.Pow(10, -09); PhotonInteractionCoefficientsNickel[5, 69] = 5.46 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsNickel[0, 70] = 5.00 * Math.Pow(10, +05); PhotonInteractionCoefficientsNickel[1, 70] = 5.61 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[2, 70] = 2.74 * Math.Pow(10, -07); PhotonInteractionCoefficientsNickel[3, 70] = 6.10 * Math.Pow(10, -04); PhotonInteractionCoefficientsNickel[4, 70] = 2.06 * Math.Pow(10, -09); PhotonInteractionCoefficientsNickel[5, 70] = 5.55 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsNickel[0, 71] = 6.00 * Math.Pow(10, +05); PhotonInteractionCoefficientsNickel[1, 71] = 5.67 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[2, 71] = 2.28 * Math.Pow(10, -07); PhotonInteractionCoefficientsNickel[3, 71] = 5.22 * Math.Pow(10, -04); PhotonInteractionCoefficientsNickel[4, 71] = 1.43 * Math.Pow(10, -09); PhotonInteractionCoefficientsNickel[5, 71] = 5.62 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsNickel[0, 72] = 8.00 * Math.Pow(10, +05); PhotonInteractionCoefficientsNickel[1, 72] = 5.75 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[2, 72] = 1.71 * Math.Pow(10, -07); PhotonInteractionCoefficientsNickel[3, 72] = 4.08 * Math.Pow(10, -04); PhotonInteractionCoefficientsNickel[4, 72] = 8.03 * Math.Pow(10, -10); PhotonInteractionCoefficientsNickel[5, 72] = 5.71 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsNickel[0, 73] = 1.00 * Math.Pow(10, +06); PhotonInteractionCoefficientsNickel[1, 73] = 5.81 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[2, 73] = 1.37 * Math.Pow(10, -07); PhotonInteractionCoefficientsNickel[3, 73] = 3.36 * Math.Pow(10, -04); PhotonInteractionCoefficientsNickel[4, 73] = 5.14 * Math.Pow(10, -10); PhotonInteractionCoefficientsNickel[5, 73] = 5.77 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsNickel[0, 74] = 1.50 * Math.Pow(10, +06); PhotonInteractionCoefficientsNickel[1, 74] = 5.88 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[2, 74] = 9.11 * Math.Pow(10, -08); PhotonInteractionCoefficientsNickel[3, 74] = 2.34 * Math.Pow(10, -04); PhotonInteractionCoefficientsNickel[4, 74] = 2.28 * Math.Pow(10, -10); PhotonInteractionCoefficientsNickel[5, 74] = 5.86 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsNickel[0, 75] = 2.00 * Math.Pow(10, +06); PhotonInteractionCoefficientsNickel[1, 75] = 5.92 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[2, 75] = 6.83 * Math.Pow(10, -08); PhotonInteractionCoefficientsNickel[3, 75] = 1.81 * Math.Pow(10, -04); PhotonInteractionCoefficientsNickel[4, 75] = 1.28 * Math.Pow(10, -10); PhotonInteractionCoefficientsNickel[5, 75] = 5.91 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsNickel[0, 76] = 3.00 * Math.Pow(10, +06); PhotonInteractionCoefficientsNickel[1, 76] = 5.97 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[2, 76] = 4.55 * Math.Pow(10, -08); PhotonInteractionCoefficientsNickel[3, 76] = 1.26 * Math.Pow(10, -04); PhotonInteractionCoefficientsNickel[4, 76] = 5.71 * Math.Pow(10, -11); PhotonInteractionCoefficientsNickel[5, 76] = 5.96 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsNickel[0, 77] = 4.00 * Math.Pow(10, +06); PhotonInteractionCoefficientsNickel[1, 77] = 5.99 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[2, 77] = 3.41 * Math.Pow(10, -08); PhotonInteractionCoefficientsNickel[3, 77] = 9.74 * Math.Pow(10, -05); PhotonInteractionCoefficientsNickel[4, 77] = 3.21 * Math.Pow(10, -11); PhotonInteractionCoefficientsNickel[5, 77] = 5.98 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsNickel[0, 78] = 5.00 * Math.Pow(10, +06); PhotonInteractionCoefficientsNickel[1, 78] = 6.01 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[2, 78] = 2.73 * Math.Pow(10, -08); PhotonInteractionCoefficientsNickel[3, 78] = 7.96 * Math.Pow(10, -05); PhotonInteractionCoefficientsNickel[4, 78] = 2.06 * Math.Pow(10, -11); PhotonInteractionCoefficientsNickel[5, 78] = 6.00 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsNickel[0, 79] = 6.00 * Math.Pow(10, +06); PhotonInteractionCoefficientsNickel[1, 79] = 6.02 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[2, 79] = 2.28 * Math.Pow(10, -08); PhotonInteractionCoefficientsNickel[3, 79] = 6.75 * Math.Pow(10, -05); PhotonInteractionCoefficientsNickel[4, 79] = 1.43 * Math.Pow(10, -11); PhotonInteractionCoefficientsNickel[5, 79] = 6.01 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsNickel[0, 80] = 8.00 * Math.Pow(10, +06); PhotonInteractionCoefficientsNickel[1, 80] = 6.03 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[2, 80] = 1.71 * Math.Pow(10, -08); PhotonInteractionCoefficientsNickel[3, 80] = 5.20 * Math.Pow(10, -05); PhotonInteractionCoefficientsNickel[4, 80] = 8.03 * Math.Pow(10, -12); PhotonInteractionCoefficientsNickel[5, 80] = 6.03 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsNickel[0, 81] = 1.00 * Math.Pow(10, +07); PhotonInteractionCoefficientsNickel[1, 81] = 6.04 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[2, 81] = 1.37 * Math.Pow(10, -08); PhotonInteractionCoefficientsNickel[3, 81] = 4.25 * Math.Pow(10, -05); PhotonInteractionCoefficientsNickel[4, 81] = 5.14 * Math.Pow(10, -12); PhotonInteractionCoefficientsNickel[5, 81] = 6.04 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsNickel[0, 82] = 1.50 * Math.Pow(10, +07); PhotonInteractionCoefficientsNickel[1, 82] = 6.06 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[2, 82] = 9.10 * Math.Pow(10, -09); PhotonInteractionCoefficientsNickel[3, 82] = 2.93 * Math.Pow(10, -05); PhotonInteractionCoefficientsNickel[4, 82] = 2.28 * Math.Pow(10, -12); PhotonInteractionCoefficientsNickel[5, 82] = 6.05 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsNickel[0, 83] = 2.00 * Math.Pow(10, +07); PhotonInteractionCoefficientsNickel[1, 83] = 6.06 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[2, 83] = 6.83 * Math.Pow(10, -09); PhotonInteractionCoefficientsNickel[3, 83] = 2.26 * Math.Pow(10, -05); PhotonInteractionCoefficientsNickel[4, 83] = 1.28 * Math.Pow(10, -12); PhotonInteractionCoefficientsNickel[5, 83] = 6.06 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsNickel[0, 84] = 3.00 * Math.Pow(10, +07); PhotonInteractionCoefficientsNickel[1, 84] = 6.07 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[2, 84] = 4.55 * Math.Pow(10, -09); PhotonInteractionCoefficientsNickel[3, 84] = 1.56 * Math.Pow(10, -05); PhotonInteractionCoefficientsNickel[4, 84] = 5.71 * Math.Pow(10, -13); PhotonInteractionCoefficientsNickel[5, 84] = 6.07 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsNickel[0, 85] = 4.00 * Math.Pow(10, +07); PhotonInteractionCoefficientsNickel[1, 85] = 6.07 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[2, 85] = 3.41 * Math.Pow(10, -09); PhotonInteractionCoefficientsNickel[3, 85] = 1.19 * Math.Pow(10, -05); PhotonInteractionCoefficientsNickel[4, 85] = 3.21 * Math.Pow(10, -13); PhotonInteractionCoefficientsNickel[5, 85] = 6.07 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsNickel[0, 86] = 5.00 * Math.Pow(10, +07); PhotonInteractionCoefficientsNickel[1, 86] = 6.08 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[2, 86] = 2.73 * Math.Pow(10, -09); PhotonInteractionCoefficientsNickel[3, 86] = 9.73 * Math.Pow(10, -06); PhotonInteractionCoefficientsNickel[4, 86] = 2.06 * Math.Pow(10, -13); PhotonInteractionCoefficientsNickel[5, 86] = 6.08 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsNickel[0, 87] = 6.00 * Math.Pow(10, +07); PhotonInteractionCoefficientsNickel[1, 87] = 6.08 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[2, 87] = 2.28 * Math.Pow(10, -09); PhotonInteractionCoefficientsNickel[3, 87] = 8.22 * Math.Pow(10, -06); PhotonInteractionCoefficientsNickel[4, 87] = 1.43 * Math.Pow(10, -13); PhotonInteractionCoefficientsNickel[5, 87] = 6.08 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsNickel[0, 88] = 8.00 * Math.Pow(10, +07); PhotonInteractionCoefficientsNickel[1, 88] = 6.08 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[2, 88] = 1.71 * Math.Pow(10, -09); PhotonInteractionCoefficientsNickel[3, 88] = 6.31 * Math.Pow(10, -06); PhotonInteractionCoefficientsNickel[4, 88] = 8.03 * Math.Pow(10, -14); PhotonInteractionCoefficientsNickel[5, 88] = 6.08 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsNickel[0, 89] = 1.00 * Math.Pow(10, +08); PhotonInteractionCoefficientsNickel[1, 89] = 6.08 * Math.Pow(10, -02); PhotonInteractionCoefficientsNickel[2, 89] = 1.37 * Math.Pow(10, -09); PhotonInteractionCoefficientsNickel[3, 89] = 5.13 * Math.Pow(10, -06); PhotonInteractionCoefficientsNickel[4, 89] = 5.14 * Math.Pow(10, -14); PhotonInteractionCoefficientsNickel[5, 89] = 6.08 * Math.Pow(10, -02);



            PhotonInteractionCoefficientsGold[0, 0] = 1.00 * Math.Pow(10, -01); PhotonInteractionCoefficientsGold[1, 0] = 6.27 * Math.Pow(10, +03); PhotonInteractionCoefficientsGold[2, 0] = 6.26 * Math.Pow(10, +03); PhotonInteractionCoefficientsGold[3, 0] = 4.76 * Math.Pow(10, -05); PhotonInteractionCoefficientsGold[4, 0] = 1.27 * Math.Pow(10, +01); PhotonInteractionCoefficientsGold[5, 0] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 1] = 1.10 * Math.Pow(10, -01); PhotonInteractionCoefficientsGold[1, 1] = 5.19 * Math.Pow(10, +03); PhotonInteractionCoefficientsGold[2, 1] = 5.18 * Math.Pow(10, +03); PhotonInteractionCoefficientsGold[3, 1] = 5.35 * Math.Pow(10, -05); PhotonInteractionCoefficientsGold[4, 1] = 1.27 * Math.Pow(10, +01); PhotonInteractionCoefficientsGold[5, 1] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 2] = 1.10 * Math.Pow(10, -01); PhotonInteractionCoefficientsGold[1, 2] = 5.24 * Math.Pow(10, +03); PhotonInteractionCoefficientsGold[2, 2] = 5.23 * Math.Pow(10, +03); PhotonInteractionCoefficientsGold[3, 2] = 5.47 * Math.Pow(10, -05); PhotonInteractionCoefficientsGold[4, 2] = 1.27 * Math.Pow(10, +01); PhotonInteractionCoefficientsGold[5, 2] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 3] = 1.50 * Math.Pow(10, -01); PhotonInteractionCoefficientsGold[1, 3] = 5.31 * Math.Pow(10, +03); PhotonInteractionCoefficientsGold[2, 3] = 5.30 * Math.Pow(10, +03); PhotonInteractionCoefficientsGold[3, 3] = 9.74 * Math.Pow(10, -05); PhotonInteractionCoefficientsGold[4, 3] = 1.27 * Math.Pow(10, +01); PhotonInteractionCoefficientsGold[5, 3] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 4] = 2.00 * Math.Pow(10, -01); PhotonInteractionCoefficientsGold[1, 4] = 1.08 * Math.Pow(10, +04); PhotonInteractionCoefficientsGold[2, 4] = 1.08 * Math.Pow(10, +04); PhotonInteractionCoefficientsGold[3, 4] = 1.68 * Math.Pow(10, -04); PhotonInteractionCoefficientsGold[4, 4] = 1.27 * Math.Pow(10, +01); PhotonInteractionCoefficientsGold[5, 4] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 5] = 3.00 * Math.Pow(10, -01); PhotonInteractionCoefficientsGold[1, 5] = 1.56 * Math.Pow(10, +04); PhotonInteractionCoefficientsGold[2, 5] = 1.56 * Math.Pow(10, +04); PhotonInteractionCoefficientsGold[3, 5] = 3.65 * Math.Pow(10, -04); PhotonInteractionCoefficientsGold[4, 5] = 1.26 * Math.Pow(10, +01); PhotonInteractionCoefficientsGold[5, 5] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 6] = 3.34 * Math.Pow(10, -01); PhotonInteractionCoefficientsGold[1, 6] = 1.51 * Math.Pow(10, +04); PhotonInteractionCoefficientsGold[2, 6] = 1.51 * Math.Pow(10, +04); PhotonInteractionCoefficientsGold[3, 6] = 4.64 * Math.Pow(10, -04); PhotonInteractionCoefficientsGold[4, 6] = 1.26 * Math.Pow(10, +01); PhotonInteractionCoefficientsGold[5, 6] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 7] = 3.34 * Math.Pow(10, -01); PhotonInteractionCoefficientsGold[1, 7] = 1.57 * Math.Pow(10, +04); PhotonInteractionCoefficientsGold[2, 7] = 1.57 * Math.Pow(10, +04); PhotonInteractionCoefficientsGold[3, 7] = 4.68 * Math.Pow(10, -04); PhotonInteractionCoefficientsGold[4, 7] = 1.26 * Math.Pow(10, +01); PhotonInteractionCoefficientsGold[5, 7] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 8] = 3.53 * Math.Pow(10, -01); PhotonInteractionCoefficientsGold[1, 8] = 1.57 * Math.Pow(10, +04); PhotonInteractionCoefficientsGold[2, 8] = 1.57 * Math.Pow(10, +04); PhotonInteractionCoefficientsGold[3, 8] = 4.68 * Math.Pow(10, -04); PhotonInteractionCoefficientsGold[4, 8] = 1.26 * Math.Pow(10, +01); PhotonInteractionCoefficientsGold[5, 8] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 9] = 3.53 * Math.Pow(10, -01); PhotonInteractionCoefficientsGold[1, 9] = 1.54 * Math.Pow(10, +04); PhotonInteractionCoefficientsGold[2, 9] = 1.54 * Math.Pow(10, +04); PhotonInteractionCoefficientsGold[3, 9] = 4.93 * Math.Pow(10, -04); PhotonInteractionCoefficientsGold[4, 9] = 1.26 * Math.Pow(10, +01); PhotonInteractionCoefficientsGold[5, 9] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 10] = 4.00 * Math.Pow(10, -01); PhotonInteractionCoefficientsGold[1, 10] = 1.44 * Math.Pow(10, +04); PhotonInteractionCoefficientsGold[2, 10] = 1.44 * Math.Pow(10, +04); PhotonInteractionCoefficientsGold[3, 10] = 6.32 * Math.Pow(10, -04); PhotonInteractionCoefficientsGold[4, 10] = 1.26 * Math.Pow(10, +01); PhotonInteractionCoefficientsGold[5, 10] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 11] = 5.00 * Math.Pow(10, -01); PhotonInteractionCoefficientsGold[1, 11] = 1.17 * Math.Pow(10, +04); PhotonInteractionCoefficientsGold[2, 11] = 1.17 * Math.Pow(10, +04); PhotonInteractionCoefficientsGold[3, 11] = 9.66 * Math.Pow(10, -04); PhotonInteractionCoefficientsGold[4, 11] = 1.26 * Math.Pow(10, +01); PhotonInteractionCoefficientsGold[5, 11] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 12] = 5.46 * Math.Pow(10, -01); PhotonInteractionCoefficientsGold[1, 12] = 9.70 * Math.Pow(10, +03); PhotonInteractionCoefficientsGold[2, 12] = 9.70 * Math.Pow(10, +03); PhotonInteractionCoefficientsGold[3, 12] = 1.26 * Math.Pow(10, -03); PhotonInteractionCoefficientsGold[4, 12] = 1.27 * Math.Pow(10, +01); PhotonInteractionCoefficientsGold[5, 12] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 13] = 5.46 * Math.Pow(10, -01); PhotonInteractionCoefficientsGold[1, 13] = 1.12 * Math.Pow(10, +04); PhotonInteractionCoefficientsGold[2, 13] = 1.12 * Math.Pow(10, +04); PhotonInteractionCoefficientsGold[3, 13] = 1.26 * Math.Pow(10, -03); PhotonInteractionCoefficientsGold[4, 13] = 1.26 * Math.Pow(10, +01); PhotonInteractionCoefficientsGold[5, 13] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 14] = 6.00 * Math.Pow(10, -01); PhotonInteractionCoefficientsGold[1, 14] = 1.03 * Math.Pow(10, +04); PhotonInteractionCoefficientsGold[2, 14] = 1.03 * Math.Pow(10, +04); PhotonInteractionCoefficientsGold[3, 14] = 1.35 * Math.Pow(10, -03); PhotonInteractionCoefficientsGold[4, 14] = 1.26 * Math.Pow(10, +01); PhotonInteractionCoefficientsGold[5, 14] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 15] = 6.44 * Math.Pow(10, -01); PhotonInteractionCoefficientsGold[1, 15] = 1.03 * Math.Pow(10, +04); PhotonInteractionCoefficientsGold[2, 15] = 1.03 * Math.Pow(10, +04); PhotonInteractionCoefficientsGold[3, 15] = 1.35 * Math.Pow(10, -03); PhotonInteractionCoefficientsGold[4, 15] = 1.26 * Math.Pow(10, +01); PhotonInteractionCoefficientsGold[5, 15] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 16] = 6.44 * Math.Pow(10, -01); PhotonInteractionCoefficientsGold[1, 16] = 1.03 * Math.Pow(10, +04); PhotonInteractionCoefficientsGold[2, 16] = 1.03 * Math.Pow(10, +04); PhotonInteractionCoefficientsGold[3, 16] = 1.35 * Math.Pow(10, -03); PhotonInteractionCoefficientsGold[4, 16] = 1.26 * Math.Pow(10, +01); PhotonInteractionCoefficientsGold[5, 16] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 17] = 7.62 * Math.Pow(10, -01); PhotonInteractionCoefficientsGold[1, 17] = 7.13 * Math.Pow(10, +03); PhotonInteractionCoefficientsGold[2, 17] = 7.13 * Math.Pow(10, +03); PhotonInteractionCoefficientsGold[3, 17] = 2.05 * Math.Pow(10, -03); PhotonInteractionCoefficientsGold[4, 17] = 1.25 * Math.Pow(10, +01); PhotonInteractionCoefficientsGold[5, 17] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 18] = 7.62 * Math.Pow(10, -01); PhotonInteractionCoefficientsGold[1, 18] = 7.65 * Math.Pow(10, +03); PhotonInteractionCoefficientsGold[2, 18] = 7.63 * Math.Pow(10, +03); PhotonInteractionCoefficientsGold[3, 18] = 2.06 * Math.Pow(10, -03); PhotonInteractionCoefficientsGold[4, 18] = 1.25 * Math.Pow(10, +01); PhotonInteractionCoefficientsGold[5, 18] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 19] = 8.00 * Math.Pow(10, -01); PhotonInteractionCoefficientsGold[1, 19] = 6.96 * Math.Pow(10, +03); PhotonInteractionCoefficientsGold[2, 19] = 6.95 * Math.Pow(10, +03); PhotonInteractionCoefficientsGold[3, 19] = 2.26 * Math.Pow(10, -03); PhotonInteractionCoefficientsGold[4, 19] = 1.25 * Math.Pow(10, +01); PhotonInteractionCoefficientsGold[5, 19] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 20] = 1.00 * Math.Pow(10, +00); PhotonInteractionCoefficientsGold[1, 20] = 4.65 * Math.Pow(10, +03); PhotonInteractionCoefficientsGold[2, 20] = 4.64 * Math.Pow(10, +03); PhotonInteractionCoefficientsGold[3, 20] = 3.27 * Math.Pow(10, -03); PhotonInteractionCoefficientsGold[4, 20] = 1.23 * Math.Pow(10, +01); PhotonInteractionCoefficientsGold[5, 20] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 21] = 1.50 * Math.Pow(10, +00); PhotonInteractionCoefficientsGold[1, 21] = 2.09 * Math.Pow(10, +03); PhotonInteractionCoefficientsGold[2, 21] = 2.08 * Math.Pow(10, +03); PhotonInteractionCoefficientsGold[3, 21] = 6.07 * Math.Pow(10, -03); PhotonInteractionCoefficientsGold[4, 21] = 1.18 * Math.Pow(10, +01); PhotonInteractionCoefficientsGold[5, 21] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 22] = 2.00 * Math.Pow(10, +00); PhotonInteractionCoefficientsGold[1, 22] = 1.14 * Math.Pow(10, +03); PhotonInteractionCoefficientsGold[2, 22] = 1.13 * Math.Pow(10, +03); PhotonInteractionCoefficientsGold[3, 22] = 8.96 * Math.Pow(10, -03); PhotonInteractionCoefficientsGold[4, 22] = 1.13 * Math.Pow(10, +01); PhotonInteractionCoefficientsGold[5, 22] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 23] = 2.21 * Math.Pow(10, +00); PhotonInteractionCoefficientsGold[1, 23] = 9.19 * Math.Pow(10, +02); PhotonInteractionCoefficientsGold[2, 23] = 9.08 * Math.Pow(10, +02); PhotonInteractionCoefficientsGold[3, 23] = 1.01 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[4, 23] = 1.10 * Math.Pow(10, +01); PhotonInteractionCoefficientsGold[5, 23] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 24] = 2.21 * Math.Pow(10, +00); PhotonInteractionCoefficientsGold[1, 24] = 9.94 * Math.Pow(10, +02); PhotonInteractionCoefficientsGold[2, 24] = 9.83 * Math.Pow(10, +02); PhotonInteractionCoefficientsGold[3, 24] = 1.01 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[4, 24] = 1.10 * Math.Pow(10, +01); PhotonInteractionCoefficientsGold[5, 24] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 25] = 2.29 * Math.Pow(10, +00); PhotonInteractionCoefficientsGold[1, 25] = 2.22 * Math.Pow(10, +03); PhotonInteractionCoefficientsGold[2, 25] = 2.21 * Math.Pow(10, +03); PhotonInteractionCoefficientsGold[3, 25] = 1.06 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[4, 25] = 1.09 * Math.Pow(10, +01); PhotonInteractionCoefficientsGold[5, 25] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 26] = 2.29 * Math.Pow(10, +00); PhotonInteractionCoefficientsGold[1, 26] = 2.36 * Math.Pow(10, +03); PhotonInteractionCoefficientsGold[2, 26] = 2.35 * Math.Pow(10, +03); PhotonInteractionCoefficientsGold[3, 26] = 1.06 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[4, 26] = 1.09 * Math.Pow(10, +01); PhotonInteractionCoefficientsGold[5, 26] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 27] = 2.74 * Math.Pow(10, +00); PhotonInteractionCoefficientsGold[1, 27] = 2.20 * Math.Pow(10, +03); PhotonInteractionCoefficientsGold[2, 27] = 2.19 * Math.Pow(10, +03); PhotonInteractionCoefficientsGold[3, 27] = 1.32 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[4, 27] = 1.04 * Math.Pow(10, +01); PhotonInteractionCoefficientsGold[5, 27] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 28] = 2.74 * Math.Pow(10, +00); PhotonInteractionCoefficientsGold[1, 28] = 2.54 * Math.Pow(10, +03); PhotonInteractionCoefficientsGold[2, 28] = 2.53 * Math.Pow(10, +03); PhotonInteractionCoefficientsGold[3, 28] = 1.32 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[4, 28] = 1.04 * Math.Pow(10, +01); PhotonInteractionCoefficientsGold[5, 28] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 29] = 3.00 * Math.Pow(10, +00); PhotonInteractionCoefficientsGold[1, 29] = 2.05 * Math.Pow(10, +03); PhotonInteractionCoefficientsGold[2, 29] = 2.04 * Math.Pow(10, +03); PhotonInteractionCoefficientsGold[3, 29] = 1.46 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[4, 29] = 1.01 * Math.Pow(10, +01); PhotonInteractionCoefficientsGold[5, 29] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 30] = 3.15 * Math.Pow(10, +00); PhotonInteractionCoefficientsGold[1, 30] = 1.82 * Math.Pow(10, +03); PhotonInteractionCoefficientsGold[2, 30] = 1.81 * Math.Pow(10, +03); PhotonInteractionCoefficientsGold[3, 30] = 1.54 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[4, 30] = 9.94 * Math.Pow(10, +00); PhotonInteractionCoefficientsGold[5, 30] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 31] = 3.15 * Math.Pow(10, +00); PhotonInteractionCoefficientsGold[1, 31] = 1.93 * Math.Pow(10, +03); PhotonInteractionCoefficientsGold[2, 31] = 1.92 * Math.Pow(10, +03); PhotonInteractionCoefficientsGold[3, 31] = 1.54 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[4, 31] = 9.94 * Math.Pow(10, +00); PhotonInteractionCoefficientsGold[5, 31] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 32] = 3.42 * Math.Pow(10, +00); PhotonInteractionCoefficientsGold[1, 32] = 1.58 * Math.Pow(10, +03); PhotonInteractionCoefficientsGold[2, 32] = 1.58 * Math.Pow(10, +03); PhotonInteractionCoefficientsGold[3, 32] = 1.69 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[4, 32] = 9.63 * Math.Pow(10, +00); PhotonInteractionCoefficientsGold[5, 32] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 33] = 3.42 * Math.Pow(10, +00); PhotonInteractionCoefficientsGold[1, 33] = 1.65 * Math.Pow(10, +03); PhotonInteractionCoefficientsGold[2, 33] = 1.64 * Math.Pow(10, +03); PhotonInteractionCoefficientsGold[3, 33] = 1.69 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[4, 33] = 9.63 * Math.Pow(10, +00); PhotonInteractionCoefficientsGold[5, 33] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 34] = 4.00 * Math.Pow(10, +00); PhotonInteractionCoefficientsGold[1, 34] = 1.14 * Math.Pow(10, +03); PhotonInteractionCoefficientsGold[2, 34] = 1.13 * Math.Pow(10, +03); PhotonInteractionCoefficientsGold[3, 34] = 1.99 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[4, 34] = 9.01 * Math.Pow(10, +00); PhotonInteractionCoefficientsGold[5, 34] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 35] = 5.00 * Math.Pow(10, +00); PhotonInteractionCoefficientsGold[1, 35] = 6.66 * Math.Pow(10, +02); PhotonInteractionCoefficientsGold[2, 35] = 6.58 * Math.Pow(10, +02); PhotonInteractionCoefficientsGold[3, 35] = 2.50 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[4, 35] = 8.03 * Math.Pow(10, +00); PhotonInteractionCoefficientsGold[5, 35] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 36] = 6.00 * Math.Pow(10, +00); PhotonInteractionCoefficientsGold[1, 36] = 4.25 * Math.Pow(10, +02); PhotonInteractionCoefficientsGold[2, 36] = 4.18 * Math.Pow(10, +02); PhotonInteractionCoefficientsGold[3, 36] = 2.99 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[4, 36] = 7.19 * Math.Pow(10, +00); PhotonInteractionCoefficientsGold[5, 36] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 37] = 8.00 * Math.Pow(10, +00); PhotonInteractionCoefficientsGold[1, 37] = 2.07 * Math.Pow(10, +02); PhotonInteractionCoefficientsGold[2, 37] = 2.01 * Math.Pow(10, +02); PhotonInteractionCoefficientsGold[3, 37] = 3.88 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[4, 37] = 5.85 * Math.Pow(10, +00); PhotonInteractionCoefficientsGold[5, 37] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 38] = 1.00 * Math.Pow(10, +01); PhotonInteractionCoefficientsGold[1, 38] = 1.18 * Math.Pow(10, +02); PhotonInteractionCoefficientsGold[2, 38] = 1.13 * Math.Pow(10, +02); PhotonInteractionCoefficientsGold[3, 38] = 4.63 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[4, 38] = 4.84 * Math.Pow(10, +00); PhotonInteractionCoefficientsGold[5, 38] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 39] = 1.19 * Math.Pow(10, +01); PhotonInteractionCoefficientsGold[1, 39] = 7.58 * Math.Pow(10, +01); PhotonInteractionCoefficientsGold[2, 39] = 7.17 * Math.Pow(10, +01); PhotonInteractionCoefficientsGold[3, 39] = 5.24 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[4, 39] = 4.08 * Math.Pow(10, +00); PhotonInteractionCoefficientsGold[5, 39] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 40] = 1.19 * Math.Pow(10, +01); PhotonInteractionCoefficientsGold[1, 40] = 1.87 * Math.Pow(10, +02); PhotonInteractionCoefficientsGold[2, 40] = 1.83 * Math.Pow(10, +02); PhotonInteractionCoefficientsGold[3, 40] = 5.24 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[4, 40] = 4.08 * Math.Pow(10, +00); PhotonInteractionCoefficientsGold[5, 40] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 41] = 1.37 * Math.Pow(10, +01); PhotonInteractionCoefficientsGold[1, 41] = 1.28 * Math.Pow(10, +02); PhotonInteractionCoefficientsGold[2, 41] = 1.25 * Math.Pow(10, +02); PhotonInteractionCoefficientsGold[3, 41] = 5.72 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[4, 41] = 3.51 * Math.Pow(10, +00); PhotonInteractionCoefficientsGold[5, 41] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 42] = 1.37 * Math.Pow(10, +01); PhotonInteractionCoefficientsGold[1, 42] = 1.76 * Math.Pow(10, +02); PhotonInteractionCoefficientsGold[2, 42] = 1.73 * Math.Pow(10, +02); PhotonInteractionCoefficientsGold[3, 42] = 5.72 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[4, 42] = 3.51 * Math.Pow(10, +00); PhotonInteractionCoefficientsGold[5, 42] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 43] = 1.44 * Math.Pow(10, +01); PhotonInteractionCoefficientsGold[1, 43] = 1.59 * Math.Pow(10, +02); PhotonInteractionCoefficientsGold[2, 43] = 1.55 * Math.Pow(10, +02); PhotonInteractionCoefficientsGold[3, 43] = 5.88 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[4, 43] = 3.34 * Math.Pow(10, +00); PhotonInteractionCoefficientsGold[5, 43] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 44] = 1.44 * Math.Pow(10, +01); PhotonInteractionCoefficientsGold[1, 44] = 1.83 * Math.Pow(10, +02); PhotonInteractionCoefficientsGold[2, 44] = 1.80 * Math.Pow(10, +02); PhotonInteractionCoefficientsGold[3, 44] = 5.88 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[4, 44] = 3.34 * Math.Pow(10, +00); PhotonInteractionCoefficientsGold[5, 44] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 45] = 1.50 * Math.Pow(10, +01); PhotonInteractionCoefficientsGold[1, 45] = 1.64 * Math.Pow(10, +02); PhotonInteractionCoefficientsGold[2, 45] = 1.60 * Math.Pow(10, +02); PhotonInteractionCoefficientsGold[3, 45] = 6.04 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[4, 45] = 3.18 * Math.Pow(10, +00); PhotonInteractionCoefficientsGold[5, 45] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 46] = 2.00 * Math.Pow(10, +01); PhotonInteractionCoefficientsGold[1, 46] = 7.88 * Math.Pow(10, +01); PhotonInteractionCoefficientsGold[2, 46] = 7.65 * Math.Pow(10, +01); PhotonInteractionCoefficientsGold[3, 46] = 7.05 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[4, 46] = 2.24 * Math.Pow(10, +00); PhotonInteractionCoefficientsGold[5, 46] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 47] = 3.00 * Math.Pow(10, +01); PhotonInteractionCoefficientsGold[1, 47] = 2.75 * Math.Pow(10, +01); PhotonInteractionCoefficientsGold[2, 47] = 2.61 * Math.Pow(10, +01); PhotonInteractionCoefficientsGold[3, 47] = 8.43 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[4, 47] = 1.32 * Math.Pow(10, +00); PhotonInteractionCoefficientsGold[5, 47] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 48] = 4.00 * Math.Pow(10, +01); PhotonInteractionCoefficientsGold[1, 48] = 1.30 * Math.Pow(10, +01); PhotonInteractionCoefficientsGold[2, 48] = 1.20 * Math.Pow(10, +01); PhotonInteractionCoefficientsGold[3, 48] = 9.23 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[4, 48] = 8.80 * Math.Pow(10, -01); PhotonInteractionCoefficientsGold[5, 48] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 49] = 5.00 * Math.Pow(10, +01); PhotonInteractionCoefficientsGold[1, 49] = 7.26 * Math.Pow(10, +00); PhotonInteractionCoefficientsGold[2, 49] = 6.54 * Math.Pow(10, +00); PhotonInteractionCoefficientsGold[3, 49] = 9.68 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[4, 49] = 6.24 * Math.Pow(10, -01); PhotonInteractionCoefficientsGold[5, 49] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 50] = 6.00 * Math.Pow(10, +01); PhotonInteractionCoefficientsGold[1, 50] = 4.53 * Math.Pow(10, +00); PhotonInteractionCoefficientsGold[2, 50] = 3.96 * Math.Pow(10, +00); PhotonInteractionCoefficientsGold[3, 50] = 9.94 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[4, 50] = 4.67 * Math.Pow(10, -01); PhotonInteractionCoefficientsGold[5, 50] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 51] = 8.00 * Math.Pow(10, +01); PhotonInteractionCoefficientsGold[1, 51] = 2.19 * Math.Pow(10, +00); PhotonInteractionCoefficientsGold[2, 51] = 1.79 * Math.Pow(10, +00); PhotonInteractionCoefficientsGold[3, 51] = 1.01 * Math.Pow(10, -01); PhotonInteractionCoefficientsGold[4, 51] = 2.94 * Math.Pow(10, -01); PhotonInteractionCoefficientsGold[5, 51] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 52] = 8.07 * Math.Pow(10, +01); PhotonInteractionCoefficientsGold[1, 52] = 2.14 * Math.Pow(10, +00); PhotonInteractionCoefficientsGold[2, 52] = 1.75 * Math.Pow(10, +00); PhotonInteractionCoefficientsGold[3, 52] = 1.01 * Math.Pow(10, -01); PhotonInteractionCoefficientsGold[4, 52] = 2.89 * Math.Pow(10, -01); PhotonInteractionCoefficientsGold[5, 52] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 53] = 8.07 * Math.Pow(10, +01); PhotonInteractionCoefficientsGold[1, 53] = 8.90 * Math.Pow(10, +00); PhotonInteractionCoefficientsGold[2, 53] = 8.51 * Math.Pow(10, +00); PhotonInteractionCoefficientsGold[3, 53] = 1.01 * Math.Pow(10, -01); PhotonInteractionCoefficientsGold[4, 53] = 2.89 * Math.Pow(10, -01); PhotonInteractionCoefficientsGold[5, 53] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 54] = 1.00 * Math.Pow(10, +02); PhotonInteractionCoefficientsGold[1, 54] = 5.16 * Math.Pow(10, +00); PhotonInteractionCoefficientsGold[2, 54] = 4.86 * Math.Pow(10, +00); PhotonInteractionCoefficientsGold[3, 54] = 1.01 * Math.Pow(10, -01); PhotonInteractionCoefficientsGold[4, 54] = 2.02 * Math.Pow(10, -01); PhotonInteractionCoefficientsGold[5, 54] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 55] = 1.50 * Math.Pow(10, +02); PhotonInteractionCoefficientsGold[1, 55] = 1.86 * Math.Pow(10, +00); PhotonInteractionCoefficientsGold[2, 55] = 1.66 * Math.Pow(10, +00); PhotonInteractionCoefficientsGold[3, 55] = 9.65 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[4, 55] = 9.95 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[5, 55] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 56] = 2.00 * Math.Pow(10, +02); PhotonInteractionCoefficientsGold[1, 56] = 9.22 * Math.Pow(10, -01); PhotonInteractionCoefficientsGold[2, 56] = 7.71 * Math.Pow(10, -01); PhotonInteractionCoefficientsGold[3, 56] = 9.11 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[4, 56] = 5.93 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[5, 56] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 57] = 3.00 * Math.Pow(10, +02); PhotonInteractionCoefficientsGold[1, 57] = 3.74 * Math.Pow(10, -01); PhotonInteractionCoefficientsGold[2, 57] = 2.65 * Math.Pow(10, -01); PhotonInteractionCoefficientsGold[3, 57] = 8.16 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[4, 57] = 2.83 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[5, 57] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 58] = 4.00 * Math.Pow(10, +02); PhotonInteractionCoefficientsGold[1, 58] = 2.18 * Math.Pow(10, -01); PhotonInteractionCoefficientsGold[2, 58] = 1.27 * Math.Pow(10, -01); PhotonInteractionCoefficientsGold[3, 58] = 7.42 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[4, 58] = 1.65 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[5, 58] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 59] = 5.00 * Math.Pow(10, +02); PhotonInteractionCoefficientsGold[1, 59] = 1.53 * Math.Pow(10, -01); PhotonInteractionCoefficientsGold[2, 59] = 7.39 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[3, 59] = 6.83 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[4, 59] = 1.08 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[5, 59] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 60] = 6.00 * Math.Pow(10, +02); PhotonInteractionCoefficientsGold[1, 60] = 1.19 * Math.Pow(10, -01); PhotonInteractionCoefficientsGold[2, 60] = 4.83 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[3, 60] = 6.35 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[4, 60] = 7.59 * Math.Pow(10, -03); PhotonInteractionCoefficientsGold[5, 60] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 61] = 8.00 * Math.Pow(10, +02); PhotonInteractionCoefficientsGold[1, 61] = 8.60 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[2, 61] = 2.56 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[3, 61] = 5.61 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[4, 61] = 4.35 * Math.Pow(10, -03); PhotonInteractionCoefficientsGold[5, 61] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 62] = 1.00 * Math.Pow(10, +03); PhotonInteractionCoefficientsGold[1, 62] = 6.95 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[2, 62] = 1.61 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[3, 62] = 5.06 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[4, 62] = 2.81 * Math.Pow(10, -03); PhotonInteractionCoefficientsGold[5, 62] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 63] = 1.02 * Math.Pow(10, +03); PhotonInteractionCoefficientsGold[1, 63] = 6.82 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[2, 63] = 1.54 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[3, 63] = 5.01 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[4, 63] = 2.69 * Math.Pow(10, -03); PhotonInteractionCoefficientsGold[5, 63] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 64] = 1.25 * Math.Pow(10, +03); PhotonInteractionCoefficientsGold[1, 64] = 5.79 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[2, 64] = 1.04 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[3, 64] = 4.54 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[4, 64] = 1.81 * Math.Pow(10, -03); PhotonInteractionCoefficientsGold[5, 64] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsGold[0, 65] = 1.50 * Math.Pow(10, +03); PhotonInteractionCoefficientsGold[1, 65] = 5.17 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[2, 65] = 7.40 * Math.Pow(10, -03); PhotonInteractionCoefficientsGold[3, 65] = 4.13 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[4, 65] = 1.27 * Math.Pow(10, -03); PhotonInteractionCoefficientsGold[5, 65] = 1.70 * Math.Pow(10, -03);
            PhotonInteractionCoefficientsGold[0, 66] = 2.00 * Math.Pow(10, +03); PhotonInteractionCoefficientsGold[1, 66] = 4.57 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[2, 66] = 4.48 * Math.Pow(10, -03); PhotonInteractionCoefficientsGold[3, 66] = 3.53 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[4, 66] = 7.16 * Math.Pow(10, -04); PhotonInteractionCoefficientsGold[5, 66] = 5.19 * Math.Pow(10, -03);
            PhotonInteractionCoefficientsGold[0, 67] = 3.00 * Math.Pow(10, +03); PhotonInteractionCoefficientsGold[1, 67] = 4.20 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[2, 67] = 2.34 * Math.Pow(10, -03); PhotonInteractionCoefficientsGold[3, 67] = 2.78 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[4, 67] = 3.20 * Math.Pow(10, -04); PhotonInteractionCoefficientsGold[5, 67] = 1.15 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsGold[0, 68] = 4.00 * Math.Pow(10, +03); PhotonInteractionCoefficientsGold[1, 68] = 4.17 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[2, 68] = 1.54 * Math.Pow(10, -03); PhotonInteractionCoefficientsGold[3, 68] = 2.32 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[4, 68] = 1.80 * Math.Pow(10, -04); PhotonInteractionCoefficientsGold[5, 68] = 1.67 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsGold[0, 69] = 5.00 * Math.Pow(10, +03); PhotonInteractionCoefficientsGold[1, 69] = 4.24 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[2, 69] = 1.13 * Math.Pow(10, -03); PhotonInteractionCoefficientsGold[3, 69] = 2.01 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[4, 69] = 1.15 * Math.Pow(10, -04); PhotonInteractionCoefficientsGold[5, 69] = 2.11 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsGold[0, 70] = 6.00 * Math.Pow(10, +03); PhotonInteractionCoefficientsGold[1, 70] = 4.36 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[2, 70] = 8.83 * Math.Pow(10, -04); PhotonInteractionCoefficientsGold[3, 70] = 1.77 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[4, 70] = 8.01 * Math.Pow(10, -05); PhotonInteractionCoefficientsGold[5, 70] = 2.49 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsGold[0, 71] = 7.00 * Math.Pow(10, +03); PhotonInteractionCoefficientsGold[1, 71] = 4.49 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[2, 71] = 7.23 * Math.Pow(10, -04); PhotonInteractionCoefficientsGold[3, 71] = 1.59 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[4, 71] = 5.89 * Math.Pow(10, -05); PhotonInteractionCoefficientsGold[5, 71] = 2.82 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsGold[0, 72] = 8.00 * Math.Pow(10, +03); PhotonInteractionCoefficientsGold[1, 72] = 4.63 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[2, 72] = 6.11 * Math.Pow(10, -04); PhotonInteractionCoefficientsGold[3, 72] = 1.45 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[4, 72] = 4.51 * Math.Pow(10, -05); PhotonInteractionCoefficientsGold[5, 72] = 3.12 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsGold[0, 73] = 9.00 * Math.Pow(10, +03); PhotonInteractionCoefficientsGold[1, 73] = 4.78 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[2, 73] = 5.29 * Math.Pow(10, -04); PhotonInteractionCoefficientsGold[3, 73] = 1.33 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[4, 73] = 3.56 * Math.Pow(10, -05); PhotonInteractionCoefficientsGold[5, 73] = 3.39 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsGold[0, 74] = 1.00 * Math.Pow(10, +04); PhotonInteractionCoefficientsGold[1, 74] = 4.93 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[2, 74] = 4.65 * Math.Pow(10, -04); PhotonInteractionCoefficientsGold[3, 74] = 1.24 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[4, 74] = 2.89 * Math.Pow(10, -05); PhotonInteractionCoefficientsGold[5, 74] = 3.64 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsGold[0, 75] = 1.20 * Math.Pow(10, +04); PhotonInteractionCoefficientsGold[1, 75] = 5.21 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[2, 75] = 3.74 * Math.Pow(10, -04); PhotonInteractionCoefficientsGold[3, 75] = 1.08 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[4, 75] = 2.00 * Math.Pow(10, -05); PhotonInteractionCoefficientsGold[5, 75] = 4.09 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsGold[0, 76] = 1.50 * Math.Pow(10, +04); PhotonInteractionCoefficientsGold[1, 76] = 5.60 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[2, 76] = 2.89 * Math.Pow(10, -04); PhotonInteractionCoefficientsGold[3, 76] = 9.14 * Math.Pow(10, -03); PhotonInteractionCoefficientsGold[4, 76] = 1.28 * Math.Pow(10, -05); PhotonInteractionCoefficientsGold[5, 76] = 4.65 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsGold[0, 77] = 2.00 * Math.Pow(10, +04); PhotonInteractionCoefficientsGold[1, 77] = 6.14 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[2, 77] = 2.09 * Math.Pow(10, -04); PhotonInteractionCoefficientsGold[3, 77] = 7.34 * Math.Pow(10, -03); PhotonInteractionCoefficientsGold[4, 77] = 7.22 * Math.Pow(10, -06); PhotonInteractionCoefficientsGold[5, 77] = 5.38 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsGold[0, 78] = 2.20 * Math.Pow(10, +04); PhotonInteractionCoefficientsGold[1, 78] = 6.32 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[2, 78] = 1.88 * Math.Pow(10, -04); PhotonInteractionCoefficientsGold[3, 78] = 6.82 * Math.Pow(10, -03); PhotonInteractionCoefficientsGold[4, 78] = 5.96 * Math.Pow(10, -06); PhotonInteractionCoefficientsGold[5, 78] = 5.62 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsGold[0, 79] = 2.60 * Math.Pow(10, +04); PhotonInteractionCoefficientsGold[1, 79] = 6.65 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[2, 79] = 1.57 * Math.Pow(10, -04); PhotonInteractionCoefficientsGold[3, 79] = 5.98 * Math.Pow(10, -03); PhotonInteractionCoefficientsGold[4, 79] = 4.27 * Math.Pow(10, -06); PhotonInteractionCoefficientsGold[5, 79] = 6.04 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsGold[0, 80] = 3.00 * Math.Pow(10, +04); PhotonInteractionCoefficientsGold[1, 80] = 6.94 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[2, 80] = 1.34 * Math.Pow(10, -04); PhotonInteractionCoefficientsGold[3, 80] = 5.34 * Math.Pow(10, -03); PhotonInteractionCoefficientsGold[4, 80] = 3.21 * Math.Pow(10, -06); PhotonInteractionCoefficientsGold[5, 80] = 6.39 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsGold[0, 81] = 4.00 * Math.Pow(10, +04); PhotonInteractionCoefficientsGold[1, 81] = 7.52 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[2, 81] = 9.86 * Math.Pow(10, -05); PhotonInteractionCoefficientsGold[3, 81] = 4.25 * Math.Pow(10, -03); PhotonInteractionCoefficientsGold[4, 81] = 1.80 * Math.Pow(10, -06); PhotonInteractionCoefficientsGold[5, 81] = 7.08 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsGold[0, 82] = 5.00 * Math.Pow(10, +04); PhotonInteractionCoefficientsGold[1, 82] = 7.95 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[2, 82] = 7.80 * Math.Pow(10, -05); PhotonInteractionCoefficientsGold[3, 82] = 3.54 * Math.Pow(10, -03); PhotonInteractionCoefficientsGold[4, 82] = 1.15 * Math.Pow(10, -06); PhotonInteractionCoefficientsGold[5, 82] = 7.59 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsGold[0, 83] = 6.00 * Math.Pow(10, +04); PhotonInteractionCoefficientsGold[1, 83] = 8.30 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[2, 83] = 6.45 * Math.Pow(10, -05); PhotonInteractionCoefficientsGold[3, 83] = 3.05 * Math.Pow(10, -03); PhotonInteractionCoefficientsGold[4, 83] = 8.02 * Math.Pow(10, -07); PhotonInteractionCoefficientsGold[5, 83] = 7.99 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsGold[0, 84] = 8.00 * Math.Pow(10, +04); PhotonInteractionCoefficientsGold[1, 84] = 8.82 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[2, 84] = 4.78 * Math.Pow(10, -05); PhotonInteractionCoefficientsGold[3, 84] = 2.41 * Math.Pow(10, -03); PhotonInteractionCoefficientsGold[4, 84] = 4.51 * Math.Pow(10, -07); PhotonInteractionCoefficientsGold[5, 84] = 8.57 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsGold[0, 85] = 1.00 * Math.Pow(10, +05); PhotonInteractionCoefficientsGold[1, 85] = 9.19 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[2, 85] = 3.81 * Math.Pow(10, -05); PhotonInteractionCoefficientsGold[3, 85] = 2.00 * Math.Pow(10, -03); PhotonInteractionCoefficientsGold[4, 85] = 2.89 * Math.Pow(10, -07); PhotonInteractionCoefficientsGold[5, 85] = 8.99 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsGold[0, 86] = 1.50 * Math.Pow(10, +05); PhotonInteractionCoefficientsGold[1, 86] = 9.78 * Math.Pow(10, -02); PhotonInteractionCoefficientsGold[2, 86] = 2.52 * Math.Pow(10, -05); PhotonInteractionCoefficientsGold[3, 86] = 1.42 * Math.Pow(10, -03); PhotonInteractionCoefficientsGold[4, 86] = 1.28 * Math.Pow(10, -07); PhotonInteractionCoefficientsGold[5, 86] = 9.64 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsGold[0, 87] = 2.00 * Math.Pow(10, +05); PhotonInteractionCoefficientsGold[1, 87] = 1.01 * Math.Pow(10, -01); PhotonInteractionCoefficientsGold[2, 87] = 1.88 * Math.Pow(10, -05); PhotonInteractionCoefficientsGold[3, 87] = 1.12 * Math.Pow(10, -03); PhotonInteractionCoefficientsGold[4, 87] = 7.22 * Math.Pow(10, -08); PhotonInteractionCoefficientsGold[5, 87] = 1.00 * Math.Pow(10, -01);
            PhotonInteractionCoefficientsGold[0, 88] = 3.00 * Math.Pow(10, +05); PhotonInteractionCoefficientsGold[1, 88] = 1.06 * Math.Pow(10, -01); PhotonInteractionCoefficientsGold[2, 88] = 1.25 * Math.Pow(10, -05); PhotonInteractionCoefficientsGold[3, 88] = 7.91 * Math.Pow(10, -04); PhotonInteractionCoefficientsGold[4, 88] = 3.21 * Math.Pow(10, -08); PhotonInteractionCoefficientsGold[5, 88] = 1.05 * Math.Pow(10, -01);
            PhotonInteractionCoefficientsGold[0, 89] = 4.00 * Math.Pow(10, +05); PhotonInteractionCoefficientsGold[1, 89] = 1.08 * Math.Pow(10, -01); PhotonInteractionCoefficientsGold[2, 89] = 9.34 * Math.Pow(10, -06); PhotonInteractionCoefficientsGold[3, 89] = 6.20 * Math.Pow(10, -04); PhotonInteractionCoefficientsGold[4, 89] = 1.80 * Math.Pow(10, -08); PhotonInteractionCoefficientsGold[5, 89] = 1.07 * Math.Pow(10, -01);
            PhotonInteractionCoefficientsGold[0, 90] = 5.00 * Math.Pow(10, +05); PhotonInteractionCoefficientsGold[1, 90] = 1.09 * Math.Pow(10, -01); PhotonInteractionCoefficientsGold[2, 90] = 7.46 * Math.Pow(10, -06); PhotonInteractionCoefficientsGold[3, 90] = 5.13 * Math.Pow(10, -04); PhotonInteractionCoefficientsGold[4, 90] = 1.15 * Math.Pow(10, -08); PhotonInteractionCoefficientsGold[5, 90] = 1.09 * Math.Pow(10, -01);
            PhotonInteractionCoefficientsGold[0, 91] = 6.00 * Math.Pow(10, +05); PhotonInteractionCoefficientsGold[1, 91] = 1.11 * Math.Pow(10, -01); PhotonInteractionCoefficientsGold[2, 91] = 6.21 * Math.Pow(10, -06); PhotonInteractionCoefficientsGold[3, 91] = 4.39 * Math.Pow(10, -04); PhotonInteractionCoefficientsGold[4, 91] = 8.02 * Math.Pow(10, -09); PhotonInteractionCoefficientsGold[5, 91] = 1.10 * Math.Pow(10, -01);
            PhotonInteractionCoefficientsGold[0, 92] = 8.00 * Math.Pow(10, +05); PhotonInteractionCoefficientsGold[1, 92] = 1.12 * Math.Pow(10, -01); PhotonInteractionCoefficientsGold[2, 92] = 4.66 * Math.Pow(10, -06); PhotonInteractionCoefficientsGold[3, 92] = 3.43 * Math.Pow(10, -04); PhotonInteractionCoefficientsGold[4, 92] = 4.51 * Math.Pow(10, -09); PhotonInteractionCoefficientsGold[5, 92] = 1.12 * Math.Pow(10, -01);
            PhotonInteractionCoefficientsGold[0, 93] = 1.00 * Math.Pow(10, +06); PhotonInteractionCoefficientsGold[1, 93] = 1.13 * Math.Pow(10, -01); PhotonInteractionCoefficientsGold[2, 93] = 3.72 * Math.Pow(10, -06); PhotonInteractionCoefficientsGold[3, 93] = 2.82 * Math.Pow(10, -04); PhotonInteractionCoefficientsGold[4, 93] = 2.89 * Math.Pow(10, -09); PhotonInteractionCoefficientsGold[5, 93] = 1.13 * Math.Pow(10, -01);
            PhotonInteractionCoefficientsGold[0, 94] = 1.50 * Math.Pow(10, +06); PhotonInteractionCoefficientsGold[1, 94] = 1.14 * Math.Pow(10, -01); PhotonInteractionCoefficientsGold[2, 94] = 2.48 * Math.Pow(10, -06); PhotonInteractionCoefficientsGold[3, 94] = 1.97 * Math.Pow(10, -04); PhotonInteractionCoefficientsGold[4, 94] = 1.28 * Math.Pow(10, -09); PhotonInteractionCoefficientsGold[5, 94] = 1.14 * Math.Pow(10, -01);
            PhotonInteractionCoefficientsGold[0, 95] = 2.00 * Math.Pow(10, +06); PhotonInteractionCoefficientsGold[1, 95] = 1.15 * Math.Pow(10, -01); PhotonInteractionCoefficientsGold[2, 95] = 1.86 * Math.Pow(10, -06); PhotonInteractionCoefficientsGold[3, 95] = 1.53 * Math.Pow(10, -04); PhotonInteractionCoefficientsGold[4, 95] = 7.22 * Math.Pow(10, -10); PhotonInteractionCoefficientsGold[5, 95] = 1.15 * Math.Pow(10, -01);
            PhotonInteractionCoefficientsGold[0, 96] = 3.00 * Math.Pow(10, +06); PhotonInteractionCoefficientsGold[1, 96] = 1.16 * Math.Pow(10, -01); PhotonInteractionCoefficientsGold[2, 96] = 1.24 * Math.Pow(10, -06); PhotonInteractionCoefficientsGold[3, 96] = 1.06 * Math.Pow(10, -04); PhotonInteractionCoefficientsGold[4, 96] = 3.21 * Math.Pow(10, -10); PhotonInteractionCoefficientsGold[5, 96] = 1.16 * Math.Pow(10, -01);
            PhotonInteractionCoefficientsGold[0, 97] = 4.00 * Math.Pow(10, +06); PhotonInteractionCoefficientsGold[1, 97] = 1.17 * Math.Pow(10, -01); PhotonInteractionCoefficientsGold[2, 97] = 9.29 * Math.Pow(10, -07); PhotonInteractionCoefficientsGold[3, 97] = 8.18 * Math.Pow(10, -05); PhotonInteractionCoefficientsGold[4, 97] = 1.80 * Math.Pow(10, -10); PhotonInteractionCoefficientsGold[5, 97] = 1.16 * Math.Pow(10, -01);
            PhotonInteractionCoefficientsGold[0, 98] = 5.00 * Math.Pow(10, +06); PhotonInteractionCoefficientsGold[1, 98] = 1.17 * Math.Pow(10, -01); PhotonInteractionCoefficientsGold[2, 98] = 7.43 * Math.Pow(10, -07); PhotonInteractionCoefficientsGold[3, 98] = 6.69 * Math.Pow(10, -05); PhotonInteractionCoefficientsGold[4, 98] = 1.15 * Math.Pow(10, -10); PhotonInteractionCoefficientsGold[5, 98] = 1.17 * Math.Pow(10, -01);
            PhotonInteractionCoefficientsGold[0, 99] = 6.00 * Math.Pow(10, +06); PhotonInteractionCoefficientsGold[1, 99] = 1.17 * Math.Pow(10, -01); PhotonInteractionCoefficientsGold[2, 99] = 6.19 * Math.Pow(10, -07); PhotonInteractionCoefficientsGold[3, 99] = 5.67 * Math.Pow(10, -05); PhotonInteractionCoefficientsGold[4, 99] = 8.02 * Math.Pow(10, -11); PhotonInteractionCoefficientsGold[5, 99] = 1.17 * Math.Pow(10, -01);
            PhotonInteractionCoefficientsGold[0, 100] = 8.00 * Math.Pow(10, +06); PhotonInteractionCoefficientsGold[1, 100] = 1.17 * Math.Pow(10, -01); PhotonInteractionCoefficientsGold[2, 100] = 4.64 * Math.Pow(10, -07); PhotonInteractionCoefficientsGold[3, 100] = 4.37 * Math.Pow(10, -05); PhotonInteractionCoefficientsGold[4, 100] = 4.51 * Math.Pow(10, -11); PhotonInteractionCoefficientsGold[5, 100] = 1.17 * Math.Pow(10, -01);
            PhotonInteractionCoefficientsGold[0, 101] = 1.00 * Math.Pow(10, +07); PhotonInteractionCoefficientsGold[1, 101] = 1.17 * Math.Pow(10, -01); PhotonInteractionCoefficientsGold[2, 101] = 3.71 * Math.Pow(10, -07); PhotonInteractionCoefficientsGold[3, 101] = 3.57 * Math.Pow(10, -05); PhotonInteractionCoefficientsGold[4, 101] = 2.89 * Math.Pow(10, -11); PhotonInteractionCoefficientsGold[5, 101] = 1.17 * Math.Pow(10, -01);
            PhotonInteractionCoefficientsGold[0, 102] = 1.50 * Math.Pow(10, +07); PhotonInteractionCoefficientsGold[1, 102] = 1.18 * Math.Pow(10, -01); PhotonInteractionCoefficientsGold[2, 102] = 2.48 * Math.Pow(10, -07); PhotonInteractionCoefficientsGold[3, 102] = 2.47 * Math.Pow(10, -05); PhotonInteractionCoefficientsGold[4, 102] = 1.28 * Math.Pow(10, -11); PhotonInteractionCoefficientsGold[5, 102] = 1.18 * Math.Pow(10, -01);
            PhotonInteractionCoefficientsGold[0, 103] = 2.00 * Math.Pow(10, +07); PhotonInteractionCoefficientsGold[1, 103] = 1.18 * Math.Pow(10, -01); PhotonInteractionCoefficientsGold[2, 103] = 1.86 * Math.Pow(10, -07); PhotonInteractionCoefficientsGold[3, 103] = 1.90 * Math.Pow(10, -05); PhotonInteractionCoefficientsGold[4, 103] = 7.22 * Math.Pow(10, -12); PhotonInteractionCoefficientsGold[5, 103] = 1.18 * Math.Pow(10, -01);
            PhotonInteractionCoefficientsGold[0, 104] = 3.00 * Math.Pow(10, +07); PhotonInteractionCoefficientsGold[1, 104] = 1.18 * Math.Pow(10, -01); PhotonInteractionCoefficientsGold[2, 104] = 1.24 * Math.Pow(10, -07); PhotonInteractionCoefficientsGold[3, 104] = 1.31 * Math.Pow(10, -05); PhotonInteractionCoefficientsGold[4, 104] = 3.21 * Math.Pow(10, -12); PhotonInteractionCoefficientsGold[5, 104] = 1.18 * Math.Pow(10, -01);
            PhotonInteractionCoefficientsGold[0, 105] = 4.00 * Math.Pow(10, +07); PhotonInteractionCoefficientsGold[1, 105] = 1.18 * Math.Pow(10, -01); PhotonInteractionCoefficientsGold[2, 105] = 9.28 * Math.Pow(10, -08); PhotonInteractionCoefficientsGold[3, 105] = 1.00 * Math.Pow(10, -05); PhotonInteractionCoefficientsGold[4, 105] = 1.80 * Math.Pow(10, -12); PhotonInteractionCoefficientsGold[5, 105] = 1.18 * Math.Pow(10, -01);
            PhotonInteractionCoefficientsGold[0, 106] = 5.00 * Math.Pow(10, +07); PhotonInteractionCoefficientsGold[1, 106] = 1.18 * Math.Pow(10, -01); PhotonInteractionCoefficientsGold[2, 106] = 7.43 * Math.Pow(10, -08); PhotonInteractionCoefficientsGold[3, 106] = 8.18 * Math.Pow(10, -06); PhotonInteractionCoefficientsGold[4, 106] = 1.15 * Math.Pow(10, -12); PhotonInteractionCoefficientsGold[5, 106] = 1.18 * Math.Pow(10, -01);
            PhotonInteractionCoefficientsGold[0, 107] = 6.00 * Math.Pow(10, +07); PhotonInteractionCoefficientsGold[1, 107] = 1.18 * Math.Pow(10, -01); PhotonInteractionCoefficientsGold[2, 107] = 6.19 * Math.Pow(10, -08); PhotonInteractionCoefficientsGold[3, 107] = 6.91 * Math.Pow(10, -06); PhotonInteractionCoefficientsGold[4, 107] = 8.02 * Math.Pow(10, -13); PhotonInteractionCoefficientsGold[5, 107] = 1.18 * Math.Pow(10, -01);
            PhotonInteractionCoefficientsGold[0, 108] = 8.00 * Math.Pow(10, +07); PhotonInteractionCoefficientsGold[1, 108] = 1.18 * Math.Pow(10, -01); PhotonInteractionCoefficientsGold[2, 108] = 4.64 * Math.Pow(10, -08); PhotonInteractionCoefficientsGold[3, 108] = 5.30 * Math.Pow(10, -06); PhotonInteractionCoefficientsGold[4, 108] = 4.51 * Math.Pow(10, -13); PhotonInteractionCoefficientsGold[5, 108] = 1.18 * Math.Pow(10, -01);
            PhotonInteractionCoefficientsGold[0, 109] = 1.00 * Math.Pow(10, +08); PhotonInteractionCoefficientsGold[1, 109] = 1.18 * Math.Pow(10, -01); PhotonInteractionCoefficientsGold[2, 109] = 3.71 * Math.Pow(10, -08); PhotonInteractionCoefficientsGold[3, 109] = 4.31 * Math.Pow(10, -06); PhotonInteractionCoefficientsGold[4, 109] = 2.89 * Math.Pow(10, -13); PhotonInteractionCoefficientsGold[5, 109] = 1.18 * Math.Pow(10, -01);



            PhotonInteractionCoefficientsAluminium[0, 0] = 1.00 * Math.Pow(10, -01); PhotonInteractionCoefficientsAluminium[1, 0] = 1.20 * Math.Pow(10, +05); PhotonInteractionCoefficientsAluminium[2, 0] = 1.20 * Math.Pow(10, +05); PhotonInteractionCoefficientsAluminium[3, 0] = 2.18 * Math.Pow(10, -04); PhotonInteractionCoefficientsAluminium[4, 0] = 2.50 * Math.Pow(10, +00); PhotonInteractionCoefficientsAluminium[5, 0] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsAluminium[0, 1] = 1.18 * Math.Pow(10, -01); PhotonInteractionCoefficientsAluminium[1, 1] = 1.11 * Math.Pow(10, +05); PhotonInteractionCoefficientsAluminium[2, 1] = 1.11 * Math.Pow(10, +05); PhotonInteractionCoefficientsAluminium[3, 1] = 2.85 * Math.Pow(10, -04); PhotonInteractionCoefficientsAluminium[4, 1] = 2.50 * Math.Pow(10, +00); PhotonInteractionCoefficientsAluminium[5, 1] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsAluminium[0, 2] = 1.18 * Math.Pow(10, -01); PhotonInteractionCoefficientsAluminium[1, 2] = 1.22 * Math.Pow(10, +05); PhotonInteractionCoefficientsAluminium[2, 2] = 1.22 * Math.Pow(10, +05); PhotonInteractionCoefficientsAluminium[3, 2] = 2.91 * Math.Pow(10, -04); PhotonInteractionCoefficientsAluminium[4, 2] = 2.50 * Math.Pow(10, +00); PhotonInteractionCoefficientsAluminium[5, 2] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsAluminium[0, 3] = 1.50 * Math.Pow(10, -01); PhotonInteractionCoefficientsAluminium[1, 3] = 8.84 * Math.Pow(10, +04); PhotonInteractionCoefficientsAluminium[2, 3] = 8.84 * Math.Pow(10, +04); PhotonInteractionCoefficientsAluminium[3, 3] = 4.48 * Math.Pow(10, -04); PhotonInteractionCoefficientsAluminium[4, 3] = 2.50 * Math.Pow(10, +00); PhotonInteractionCoefficientsAluminium[5, 3] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsAluminium[0, 4] = 2.00 * Math.Pow(10, -01); PhotonInteractionCoefficientsAluminium[1, 4] = 5.27 * Math.Pow(10, +04); PhotonInteractionCoefficientsAluminium[2, 4] = 5.27 * Math.Pow(10, +04); PhotonInteractionCoefficientsAluminium[3, 4] = 7.71 * Math.Pow(10, -04); PhotonInteractionCoefficientsAluminium[4, 4] = 2.50 * Math.Pow(10, +00); PhotonInteractionCoefficientsAluminium[5, 4] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsAluminium[0, 5] = 3.00 * Math.Pow(10, -01); PhotonInteractionCoefficientsAluminium[1, 5] = 2.25 * Math.Pow(10, +04); PhotonInteractionCoefficientsAluminium[2, 5] = 2.25 * Math.Pow(10, +04); PhotonInteractionCoefficientsAluminium[3, 5] = 1.68 * Math.Pow(10, -03); PhotonInteractionCoefficientsAluminium[4, 5] = 2.48 * Math.Pow(10, +00); PhotonInteractionCoefficientsAluminium[5, 5] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsAluminium[0, 6] = 4.00 * Math.Pow(10, -01); PhotonInteractionCoefficientsAluminium[1, 6] = 1.17 * Math.Pow(10, +04); PhotonInteractionCoefficientsAluminium[2, 6] = 1.17 * Math.Pow(10, +04); PhotonInteractionCoefficientsAluminium[3, 6] = 2.91 * Math.Pow(10, -03); PhotonInteractionCoefficientsAluminium[4, 6] = 2.46 * Math.Pow(10, +00); PhotonInteractionCoefficientsAluminium[5, 6] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsAluminium[0, 7] = 5.00 * Math.Pow(10, -01); PhotonInteractionCoefficientsAluminium[1, 7] = 6.81 * Math.Pow(10, +03); PhotonInteractionCoefficientsAluminium[2, 7] = 6.81 * Math.Pow(10, +03); PhotonInteractionCoefficientsAluminium[3, 7] = 4.41 * Math.Pow(10, -03); PhotonInteractionCoefficientsAluminium[4, 7] = 2.44 * Math.Pow(10, +00); PhotonInteractionCoefficientsAluminium[5, 7] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsAluminium[0, 8] = 6.00 * Math.Pow(10, -01); PhotonInteractionCoefficientsAluminium[1, 8] = 4.36 * Math.Pow(10, +03); PhotonInteractionCoefficientsAluminium[2, 8] = 4.36 * Math.Pow(10, +03); PhotonInteractionCoefficientsAluminium[3, 8] = 6.13 * Math.Pow(10, -03); PhotonInteractionCoefficientsAluminium[4, 8] = 2.41 * Math.Pow(10, +00); PhotonInteractionCoefficientsAluminium[5, 8] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsAluminium[0, 9] = 8.00 * Math.Pow(10, -01); PhotonInteractionCoefficientsAluminium[1, 9] = 2.10 * Math.Pow(10, +03); PhotonInteractionCoefficientsAluminium[2, 9] = 2.10 * Math.Pow(10, +03); PhotonInteractionCoefficientsAluminium[3, 9] = 1.01 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[4, 9] = 2.34 * Math.Pow(10, +00); PhotonInteractionCoefficientsAluminium[5, 9] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsAluminium[0, 10] = 1.00 * Math.Pow(10, +00); PhotonInteractionCoefficientsAluminium[1, 10] = 1.19 * Math.Pow(10, +03); PhotonInteractionCoefficientsAluminium[2, 10] = 1.18 * Math.Pow(10, +03); PhotonInteractionCoefficientsAluminium[3, 10] = 1.43 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[4, 10] = 2.26 * Math.Pow(10, +00); PhotonInteractionCoefficientsAluminium[5, 10] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsAluminium[0, 11] = 1.50 * Math.Pow(10, +00); PhotonInteractionCoefficientsAluminium[1, 11] = 4.02 * Math.Pow(10, +02); PhotonInteractionCoefficientsAluminium[2, 11] = 4.00 * Math.Pow(10, +02); PhotonInteractionCoefficientsAluminium[3, 11] = 2.48 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[4, 11] = 2.04 * Math.Pow(10, +00); PhotonInteractionCoefficientsAluminium[5, 11] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsAluminium[0, 12] = 1.56 * Math.Pow(10, +00); PhotonInteractionCoefficientsAluminium[1, 12] = 3.62 * Math.Pow(10, +02); PhotonInteractionCoefficientsAluminium[2, 12] = 3.60 * Math.Pow(10, +02); PhotonInteractionCoefficientsAluminium[3, 12] = 2.59 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[4, 12] = 2.01 * Math.Pow(10, +00); PhotonInteractionCoefficientsAluminium[5, 12] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsAluminium[0, 13] = 1.56 * Math.Pow(10, +00); PhotonInteractionCoefficientsAluminium[1, 13] = 3.96 * Math.Pow(10, +03); PhotonInteractionCoefficientsAluminium[2, 13] = 3.96 * Math.Pow(10, +03); PhotonInteractionCoefficientsAluminium[3, 13] = 2.59 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[4, 13] = 2.01 * Math.Pow(10, +00); PhotonInteractionCoefficientsAluminium[5, 13] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsAluminium[0, 14] = 2.00 * Math.Pow(10, +00); PhotonInteractionCoefficientsAluminium[1, 14] = 2.26 * Math.Pow(10, +03); PhotonInteractionCoefficientsAluminium[2, 14] = 2.26 * Math.Pow(10, +03); PhotonInteractionCoefficientsAluminium[3, 14] = 3.37 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[4, 14] = 1.84 * Math.Pow(10, +00); PhotonInteractionCoefficientsAluminium[5, 14] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsAluminium[0, 15] = 3.00 * Math.Pow(10, +00); PhotonInteractionCoefficientsAluminium[1, 15] = 7.88 * Math.Pow(10, +02); PhotonInteractionCoefficientsAluminium[2, 15] = 7.87 * Math.Pow(10, +02); PhotonInteractionCoefficientsAluminium[3, 15] = 4.73 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[4, 15] = 1.52 * Math.Pow(10, +00); PhotonInteractionCoefficientsAluminium[5, 15] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsAluminium[0, 16] = 4.00 * Math.Pow(10, +00); PhotonInteractionCoefficientsAluminium[1, 16] = 3.60 * Math.Pow(10, +02); PhotonInteractionCoefficientsAluminium[2, 16] = 3.59 * Math.Pow(10, +02); PhotonInteractionCoefficientsAluminium[3, 16] = 5.81 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[4, 16] = 1.30 * Math.Pow(10, +00); PhotonInteractionCoefficientsAluminium[5, 16] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsAluminium[0, 17] = 5.00 * Math.Pow(10, +00); PhotonInteractionCoefficientsAluminium[1, 17] = 1.93 * Math.Pow(10, +02); PhotonInteractionCoefficientsAluminium[2, 17] = 1.92 * Math.Pow(10, +02); PhotonInteractionCoefficientsAluminium[3, 17] = 6.79 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[4, 17] = 1.12 * Math.Pow(10, +00); PhotonInteractionCoefficientsAluminium[5, 17] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsAluminium[0, 18] = 6.00 * Math.Pow(10, +00); PhotonInteractionCoefficientsAluminium[1, 18] = 1.15 * Math.Pow(10, +02); PhotonInteractionCoefficientsAluminium[2, 18] = 1.14 * Math.Pow(10, +02); PhotonInteractionCoefficientsAluminium[3, 18] = 7.70 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[4, 18] = 9.64 * Math.Pow(10, -01); PhotonInteractionCoefficientsAluminium[5, 18] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsAluminium[0, 19] = 8.00 * Math.Pow(10, +00); PhotonInteractionCoefficientsAluminium[1, 19] = 5.03 * Math.Pow(10, +01); PhotonInteractionCoefficientsAluminium[2, 19] = 4.95 * Math.Pow(10, +01); PhotonInteractionCoefficientsAluminium[3, 19] = 9.29 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[4, 19] = 7.23 * Math.Pow(10, -01); PhotonInteractionCoefficientsAluminium[5, 19] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsAluminium[0, 20] = 1.00 * Math.Pow(10, +01); PhotonInteractionCoefficientsAluminium[1, 20] = 2.62 * Math.Pow(10, +01); PhotonInteractionCoefficientsAluminium[2, 20] = 2.56 * Math.Pow(10, +01); PhotonInteractionCoefficientsAluminium[3, 20] = 1.06 * Math.Pow(10, -01); PhotonInteractionCoefficientsAluminium[4, 20] = 5.51 * Math.Pow(10, -01); PhotonInteractionCoefficientsAluminium[5, 20] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsAluminium[0, 21] = 1.50 * Math.Pow(10, +01); PhotonInteractionCoefficientsAluminium[1, 21] = 7.96 * Math.Pow(10, +00); PhotonInteractionCoefficientsAluminium[2, 21] = 7.52 * Math.Pow(10, +00); PhotonInteractionCoefficientsAluminium[3, 21] = 1.27 * Math.Pow(10, -01); PhotonInteractionCoefficientsAluminium[4, 21] = 3.14 * Math.Pow(10, -01); PhotonInteractionCoefficientsAluminium[5, 21] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsAluminium[0, 22] = 2.00 * Math.Pow(10, +01); PhotonInteractionCoefficientsAluminium[1, 22] = 3.44 * Math.Pow(10, +00); PhotonInteractionCoefficientsAluminium[2, 22] = 3.10 * Math.Pow(10, +00); PhotonInteractionCoefficientsAluminium[3, 22] = 1.37 * Math.Pow(10, -01); PhotonInteractionCoefficientsAluminium[4, 22] = 2.05 * Math.Pow(10, -01); PhotonInteractionCoefficientsAluminium[5, 22] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsAluminium[0, 23] = 3.00 * Math.Pow(10, +01); PhotonInteractionCoefficientsAluminium[1, 23] = 1.13 * Math.Pow(10, +00); PhotonInteractionCoefficientsAluminium[2, 23] = 8.72 * Math.Pow(10, -01); PhotonInteractionCoefficientsAluminium[3, 23] = 1.46 * Math.Pow(10, -01); PhotonInteractionCoefficientsAluminium[4, 23] = 1.10 * Math.Pow(10, -01); PhotonInteractionCoefficientsAluminium[5, 23] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsAluminium[0, 24] = 4.00 * Math.Pow(10, +01); PhotonInteractionCoefficientsAluminium[1, 24] = 5.68 * Math.Pow(10, -01); PhotonInteractionCoefficientsAluminium[2, 24] = 3.50 * Math.Pow(10, -01); PhotonInteractionCoefficientsAluminium[3, 24] = 1.49 * Math.Pow(10, -01); PhotonInteractionCoefficientsAluminium[4, 24] = 6.86 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[5, 24] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsAluminium[0, 25] = 5.00 * Math.Pow(10, +01); PhotonInteractionCoefficientsAluminium[1, 25] = 3.68 * Math.Pow(10, -01); PhotonInteractionCoefficientsAluminium[2, 25] = 1.72 * Math.Pow(10, -01); PhotonInteractionCoefficientsAluminium[3, 25] = 1.50 * Math.Pow(10, -01); PhotonInteractionCoefficientsAluminium[4, 25] = 4.68 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[5, 25] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsAluminium[0, 26] = 6.00 * Math.Pow(10, +01); PhotonInteractionCoefficientsAluminium[1, 26] = 2.78 * Math.Pow(10, -01); PhotonInteractionCoefficientsAluminium[2, 26] = 9.56 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[3, 26] = 1.48 * Math.Pow(10, -01); PhotonInteractionCoefficientsAluminium[4, 26] = 3.39 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[5, 26] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsAluminium[0, 27] = 8.00 * Math.Pow(10, +01); PhotonInteractionCoefficientsAluminium[1, 27] = 2.02 * Math.Pow(10, -01); PhotonInteractionCoefficientsAluminium[2, 27] = 3.78 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[3, 27] = 1.44 * Math.Pow(10, -01); PhotonInteractionCoefficientsAluminium[4, 27] = 2.00 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[5, 27] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsAluminium[0, 28] = 1.00 * Math.Pow(10, +02); PhotonInteractionCoefficientsAluminium[1, 28] = 1.70 * Math.Pow(10, -01); PhotonInteractionCoefficientsAluminium[2, 28] = 1.84 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[3, 28] = 1.39 * Math.Pow(10, -01); PhotonInteractionCoefficientsAluminium[4, 28] = 1.32 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[5, 28] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsAluminium[0, 29] = 1.50 * Math.Pow(10, +02); PhotonInteractionCoefficientsAluminium[1, 29] = 1.38 * Math.Pow(10, -01); PhotonInteractionCoefficientsAluminium[2, 29] = 4.99 * Math.Pow(10, -03); PhotonInteractionCoefficientsAluminium[3, 29] = 1.27 * Math.Pow(10, -01); PhotonInteractionCoefficientsAluminium[4, 29] = 6.12 * Math.Pow(10, -03); PhotonInteractionCoefficientsAluminium[5, 29] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsAluminium[0, 30] = 2.00 * Math.Pow(10, +02); PhotonInteractionCoefficientsAluminium[1, 30] = 1.22 * Math.Pow(10, -01); PhotonInteractionCoefficientsAluminium[2, 30] = 2.00 * Math.Pow(10, -03); PhotonInteractionCoefficientsAluminium[3, 30] = 1.17 * Math.Pow(10, -01); PhotonInteractionCoefficientsAluminium[4, 30] = 3.50 * Math.Pow(10, -03); PhotonInteractionCoefficientsAluminium[5, 30] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsAluminium[0, 31] = 3.00 * Math.Pow(10, +02); PhotonInteractionCoefficientsAluminium[1, 31] = 1.04 * Math.Pow(10, -01); PhotonInteractionCoefficientsAluminium[2, 31] = 5.74 * Math.Pow(10, -04); PhotonInteractionCoefficientsAluminium[3, 31] = 1.02 * Math.Pow(10, -01); PhotonInteractionCoefficientsAluminium[4, 31] = 1.58 * Math.Pow(10, -03); PhotonInteractionCoefficientsAluminium[5, 31] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsAluminium[0, 32] = 4.00 * Math.Pow(10, +02); PhotonInteractionCoefficientsAluminium[1, 32] = 9.28 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[2, 32] = 2.48 * Math.Pow(10, -04); PhotonInteractionCoefficientsAluminium[3, 32] = 9.16 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[4, 32] = 8.93 * Math.Pow(10, -04); PhotonInteractionCoefficientsAluminium[5, 32] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsAluminium[0, 33] = 5.00 * Math.Pow(10, +02); PhotonInteractionCoefficientsAluminium[1, 33] = 8.45 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[2, 33] = 1.34 * Math.Pow(10, -04); PhotonInteractionCoefficientsAluminium[3, 33] = 8.37 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[4, 33] = 5.73 * Math.Pow(10, -04); PhotonInteractionCoefficientsAluminium[5, 33] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsAluminium[0, 34] = 6.00 * Math.Pow(10, +02); PhotonInteractionCoefficientsAluminium[1, 34] = 7.80 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[2, 34] = 8.40 * Math.Pow(10, -05); PhotonInteractionCoefficientsAluminium[3, 34] = 7.75 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[4, 34] = 3.99 * Math.Pow(10, -04); PhotonInteractionCoefficientsAluminium[5, 34] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsAluminium[0, 35] = 8.00 * Math.Pow(10, +02); PhotonInteractionCoefficientsAluminium[1, 35] = 6.84 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[2, 35] = 4.25 * Math.Pow(10, -05); PhotonInteractionCoefficientsAluminium[3, 35] = 6.81 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[4, 35] = 2.25 * Math.Pow(10, -04); PhotonInteractionCoefficientsAluminium[5, 35] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsAluminium[0, 36] = 1.00 * Math.Pow(10, +03); PhotonInteractionCoefficientsAluminium[1, 36] = 6.15 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[2, 36] = 2.64 * Math.Pow(10, -05); PhotonInteractionCoefficientsAluminium[3, 36] = 6.13 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[4, 36] = 1.44 * Math.Pow(10, -04); PhotonInteractionCoefficientsAluminium[5, 36] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsAluminium[0, 37] = 1.02 * Math.Pow(10, +03); PhotonInteractionCoefficientsAluminium[1, 37] = 6.08 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[2, 37] = 2.49 * Math.Pow(10, -05); PhotonInteractionCoefficientsAluminium[3, 37] = 6.06 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[4, 37] = 1.38 * Math.Pow(10, -04); PhotonInteractionCoefficientsAluminium[5, 37] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsAluminium[0, 38] = 1.25 * Math.Pow(10, +03); PhotonInteractionCoefficientsAluminium[1, 38] = 5.50 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[2, 38] = 1.69 * Math.Pow(10, -05); PhotonInteractionCoefficientsAluminium[3, 38] = 5.48 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[4, 38] = 9.21 * Math.Pow(10, -05); PhotonInteractionCoefficientsAluminium[5, 38] = 0.00 * Math.Pow(10, +00);
            PhotonInteractionCoefficientsAluminium[0, 39] = 1.50 * Math.Pow(10, +03); PhotonInteractionCoefficientsAluminium[1, 39] = 5.01 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[2, 39] = 1.22 * Math.Pow(10, -05); PhotonInteractionCoefficientsAluminium[3, 39] = 4.98 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[4, 39] = 6.39 * Math.Pow(10, -05); PhotonInteractionCoefficientsAluminium[5, 39] = 1.71 * Math.Pow(10, -04);
            PhotonInteractionCoefficientsAluminium[0, 40] = 2.00 * Math.Pow(10, +03); PhotonInteractionCoefficientsAluminium[1, 40] = 4.32 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[2, 40] = 7.63 * Math.Pow(10, -06); PhotonInteractionCoefficientsAluminium[3, 40] = 4.25 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[4, 40] = 3.60 * Math.Pow(10, -05); PhotonInteractionCoefficientsAluminium[5, 40] = 6.75 * Math.Pow(10, -04);
            PhotonInteractionCoefficientsAluminium[0, 41] = 3.00 * Math.Pow(10, +03); PhotonInteractionCoefficientsAluminium[1, 41] = 3.54 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[2, 41] = 4.22 * Math.Pow(10, -06); PhotonInteractionCoefficientsAluminium[3, 41] = 3.35 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[4, 41] = 1.60 * Math.Pow(10, -05); PhotonInteractionCoefficientsAluminium[5, 41] = 1.93 * Math.Pow(10, -03);
            PhotonInteractionCoefficientsAluminium[0, 42] = 4.00 * Math.Pow(10, +03); PhotonInteractionCoefficientsAluminium[1, 42] = 3.11 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[2, 42] = 2.88 * Math.Pow(10, -06); PhotonInteractionCoefficientsAluminium[3, 42] = 2.79 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[4, 42] = 9.00 * Math.Pow(10, -06); PhotonInteractionCoefficientsAluminium[5, 42] = 3.15 * Math.Pow(10, -03);
            PhotonInteractionCoefficientsAluminium[0, 43] = 5.00 * Math.Pow(10, +03); PhotonInteractionCoefficientsAluminium[1, 43] = 2.84 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[2, 43] = 2.18 * Math.Pow(10, -06); PhotonInteractionCoefficientsAluminium[3, 43] = 2.41 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[4, 43] = 5.76 * Math.Pow(10, -06); PhotonInteractionCoefficientsAluminium[5, 43] = 4.25 * Math.Pow(10, -03);
            PhotonInteractionCoefficientsAluminium[0, 44] = 6.00 * Math.Pow(10, +03); PhotonInteractionCoefficientsAluminium[1, 44] = 2.66 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[2, 44] = 1.74 * Math.Pow(10, -06); PhotonInteractionCoefficientsAluminium[3, 44] = 2.13 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[4, 44] = 4.00 * Math.Pow(10, -06); PhotonInteractionCoefficientsAluminium[5, 44] = 5.24 * Math.Pow(10, -03);
            PhotonInteractionCoefficientsAluminium[0, 45] = 7.00 * Math.Pow(10, +03); PhotonInteractionCoefficientsAluminium[1, 45] = 2.53 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[2, 45] = 1.45 * Math.Pow(10, -06); PhotonInteractionCoefficientsAluminium[3, 45] = 1.91 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[4, 45] = 2.94 * Math.Pow(10, -06); PhotonInteractionCoefficientsAluminium[5, 45] = 6.13 * Math.Pow(10, -03);
            PhotonInteractionCoefficientsAluminium[0, 46] = 8.00 * Math.Pow(10, +03); PhotonInteractionCoefficientsAluminium[1, 46] = 2.44 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[2, 46] = 1.24 * Math.Pow(10, -06); PhotonInteractionCoefficientsAluminium[3, 46] = 1.74 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[4, 46] = 2.25 * Math.Pow(10, -06); PhotonInteractionCoefficientsAluminium[5, 46] = 6.94 * Math.Pow(10, -03);
            PhotonInteractionCoefficientsAluminium[0, 47] = 9.00 * Math.Pow(10, +03); PhotonInteractionCoefficientsAluminium[1, 47] = 2.37 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[2, 47] = 1.09 * Math.Pow(10, -06); PhotonInteractionCoefficientsAluminium[3, 47] = 1.60 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[4, 47] = 1.78 * Math.Pow(10, -06); PhotonInteractionCoefficientsAluminium[5, 47] = 7.67 * Math.Pow(10, -03);
            PhotonInteractionCoefficientsAluminium[0, 48] = 1.00 * Math.Pow(10, +04); PhotonInteractionCoefficientsAluminium[1, 48] = 2.32 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[2, 48] = 9.66 * Math.Pow(10, -07); PhotonInteractionCoefficientsAluminium[3, 48] = 1.48 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[4, 48] = 1.44 * Math.Pow(10, -06); PhotonInteractionCoefficientsAluminium[5, 48] = 8.34 * Math.Pow(10, -03);
            PhotonInteractionCoefficientsAluminium[0, 49] = 1.20 * Math.Pow(10, +04); PhotonInteractionCoefficientsAluminium[1, 49] = 2.25 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[2, 49] = 7.89 * Math.Pow(10, -07); PhotonInteractionCoefficientsAluminium[3, 49] = 1.30 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[4, 49] = 1.00 * Math.Pow(10, -06); PhotonInteractionCoefficientsAluminium[5, 49] = 9.50 * Math.Pow(10, -03);
            PhotonInteractionCoefficientsAluminium[0, 50] = 1.50 * Math.Pow(10, +04); PhotonInteractionCoefficientsAluminium[1, 50] = 2.19 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[2, 50] = 6.19 * Math.Pow(10, -07); PhotonInteractionCoefficientsAluminium[3, 50] = 1.10 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[4, 50] = 6.40 * Math.Pow(10, -07); PhotonInteractionCoefficientsAluminium[5, 50] = 1.10 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsAluminium[0, 51] = 2.00 * Math.Pow(10, +04); PhotonInteractionCoefficientsAluminium[1, 51] = 2.17 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[2, 51] = 4.55 * Math.Pow(10, -07); PhotonInteractionCoefficientsAluminium[3, 51] = 8.82 * Math.Pow(10, -03); PhotonInteractionCoefficientsAluminium[4, 51] = 3.60 * Math.Pow(10, -07); PhotonInteractionCoefficientsAluminium[5, 51] = 1.29 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsAluminium[0, 52] = 2.20 * Math.Pow(10, +04); PhotonInteractionCoefficientsAluminium[1, 52] = 2.17 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[2, 52] = 4.11 * Math.Pow(10, -07); PhotonInteractionCoefficientsAluminium[3, 52] = 8.19 * Math.Pow(10, -03); PhotonInteractionCoefficientsAluminium[4, 52] = 2.98 * Math.Pow(10, -07); PhotonInteractionCoefficientsAluminium[5, 52] = 1.35 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsAluminium[0, 53] = 2.60 * Math.Pow(10, +04); PhotonInteractionCoefficientsAluminium[1, 53] = 2.18 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[2, 53] = 3.45 * Math.Pow(10, -07); PhotonInteractionCoefficientsAluminium[3, 53] = 7.19 * Math.Pow(10, -03); PhotonInteractionCoefficientsAluminium[4, 53] = 2.13 * Math.Pow(10, -07); PhotonInteractionCoefficientsAluminium[5, 53] = 1.46 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsAluminium[0, 54] = 3.00 * Math.Pow(10, +04); PhotonInteractionCoefficientsAluminium[1, 54] = 2.20 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[2, 54] = 2.97 * Math.Pow(10, -07); PhotonInteractionCoefficientsAluminium[3, 54] = 6.42 * Math.Pow(10, -03); PhotonInteractionCoefficientsAluminium[4, 54] = 1.60 * Math.Pow(10, -07); PhotonInteractionCoefficientsAluminium[5, 54] = 1.55 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsAluminium[0, 55] = 4.00 * Math.Pow(10, +04); PhotonInteractionCoefficientsAluminium[1, 55] = 2.25 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[2, 55] = 2.20 * Math.Pow(10, -07); PhotonInteractionCoefficientsAluminium[3, 55] = 5.10 * Math.Pow(10, -03); PhotonInteractionCoefficientsAluminium[4, 55] = 9.00 * Math.Pow(10, -08); PhotonInteractionCoefficientsAluminium[5, 55] = 1.74 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsAluminium[0, 56] = 5.00 * Math.Pow(10, +04); PhotonInteractionCoefficientsAluminium[1, 56] = 2.31 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[2, 56] = 1.75 * Math.Pow(10, -07); PhotonInteractionCoefficientsAluminium[3, 56] = 4.26 * Math.Pow(10, -03); PhotonInteractionCoefficientsAluminium[4, 56] = 5.76 * Math.Pow(10, -08); PhotonInteractionCoefficientsAluminium[5, 56] = 1.88 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsAluminium[0, 57] = 6.00 * Math.Pow(10, +04); PhotonInteractionCoefficientsAluminium[1, 57] = 2.36 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[2, 57] = 1.45 * Math.Pow(10, -07); PhotonInteractionCoefficientsAluminium[3, 57] = 3.67 * Math.Pow(10, -03); PhotonInteractionCoefficientsAluminium[4, 57] = 4.00 * Math.Pow(10, -08); PhotonInteractionCoefficientsAluminium[5, 57] = 1.99 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsAluminium[0, 58] = 8.00 * Math.Pow(10, +04); PhotonInteractionCoefficientsAluminium[1, 58] = 2.45 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[2, 58] = 1.08 * Math.Pow(10, -07); PhotonInteractionCoefficientsAluminium[3, 58] = 2.89 * Math.Pow(10, -03); PhotonInteractionCoefficientsAluminium[4, 58] = 2.25 * Math.Pow(10, -08); PhotonInteractionCoefficientsAluminium[5, 58] = 2.16 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsAluminium[0, 59] = 1.00 * Math.Pow(10, +05); PhotonInteractionCoefficientsAluminium[1, 59] = 2.52 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[2, 59] = 8.64 * Math.Pow(10, -08); PhotonInteractionCoefficientsAluminium[3, 59] = 2.40 * Math.Pow(10, -03); PhotonInteractionCoefficientsAluminium[4, 59] = 1.44 * Math.Pow(10, -08); PhotonInteractionCoefficientsAluminium[5, 59] = 2.28 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsAluminium[0, 60] = 1.50 * Math.Pow(10, +05); PhotonInteractionCoefficientsAluminium[1, 60] = 2.64 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[2, 60] = 5.74 * Math.Pow(10, -08); PhotonInteractionCoefficientsAluminium[3, 60] = 1.71 * Math.Pow(10, -03); PhotonInteractionCoefficientsAluminium[4, 60] = 6.40 * Math.Pow(10, -09); PhotonInteractionCoefficientsAluminium[5, 60] = 2.47 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsAluminium[0, 61] = 2.00 * Math.Pow(10, +05); PhotonInteractionCoefficientsAluminium[1, 61] = 2.72 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[2, 61] = 4.29 * Math.Pow(10, -08); PhotonInteractionCoefficientsAluminium[3, 61] = 1.34 * Math.Pow(10, -03); PhotonInteractionCoefficientsAluminium[4, 61] = 3.60 * Math.Pow(10, -09); PhotonInteractionCoefficientsAluminium[5, 61] = 2.59 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsAluminium[0, 62] = 3.00 * Math.Pow(10, +05); PhotonInteractionCoefficientsAluminium[1, 62] = 2.83 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[2, 62] = 2.86 * Math.Pow(10, -08); PhotonInteractionCoefficientsAluminium[3, 62] = 9.50 * Math.Pow(10, -04); PhotonInteractionCoefficientsAluminium[4, 62] = 1.60 * Math.Pow(10, -09); PhotonInteractionCoefficientsAluminium[5, 62] = 2.73 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsAluminium[0, 63] = 4.00 * Math.Pow(10, +05); PhotonInteractionCoefficientsAluminium[1, 63] = 2.89 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[2, 63] = 2.14 * Math.Pow(10, -08); PhotonInteractionCoefficientsAluminium[3, 63] = 7.44 * Math.Pow(10, -04); PhotonInteractionCoefficientsAluminium[4, 63] = 9.00 * Math.Pow(10, -10); PhotonInteractionCoefficientsAluminium[5, 63] = 2.81 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsAluminium[0, 64] = 5.00 * Math.Pow(10, +05); PhotonInteractionCoefficientsAluminium[1, 64] = 2.93 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[2, 64] = 1.71 * Math.Pow(10, -08); PhotonInteractionCoefficientsAluminium[3, 64] = 6.16 * Math.Pow(10, -04); PhotonInteractionCoefficientsAluminium[4, 64] = 5.76 * Math.Pow(10, -10); PhotonInteractionCoefficientsAluminium[5, 64] = 2.87 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsAluminium[0, 65] = 6.00 * Math.Pow(10, +05); PhotonInteractionCoefficientsAluminium[1, 65] = 2.96 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[2, 65] = 1.43 * Math.Pow(10, -08); PhotonInteractionCoefficientsAluminium[3, 65] = 5.28 * Math.Pow(10, -04); PhotonInteractionCoefficientsAluminium[4, 65] = 4.00 * Math.Pow(10, -10); PhotonInteractionCoefficientsAluminium[5, 65] = 2.91 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsAluminium[0, 66] = 8.00 * Math.Pow(10, +05); PhotonInteractionCoefficientsAluminium[1, 66] = 3.00 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[2, 66] = 1.07 * Math.Pow(10, -08); PhotonInteractionCoefficientsAluminium[3, 66] = 4.12 * Math.Pow(10, -04); PhotonInteractionCoefficientsAluminium[4, 66] = 2.25 * Math.Pow(10, -10); PhotonInteractionCoefficientsAluminium[5, 66] = 2.96 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsAluminium[0, 67] = 1.00 * Math.Pow(10, +06); PhotonInteractionCoefficientsAluminium[1, 67] = 3.03 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[2, 67] = 8.54 * Math.Pow(10, -09); PhotonInteractionCoefficientsAluminium[3, 67] = 3.39 * Math.Pow(10, -04); PhotonInteractionCoefficientsAluminium[4, 67] = 1.44 * Math.Pow(10, -10); PhotonInteractionCoefficientsAluminium[5, 67] = 3.00 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsAluminium[0, 68] = 1.50 * Math.Pow(10, +06); PhotonInteractionCoefficientsAluminium[1, 68] = 3.07 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[2, 68] = 5.69 * Math.Pow(10, -09); PhotonInteractionCoefficientsAluminium[3, 68] = 2.37 * Math.Pow(10, -04); PhotonInteractionCoefficientsAluminium[4, 68] = 6.40 * Math.Pow(10, -11); PhotonInteractionCoefficientsAluminium[5, 68] = 3.05 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsAluminium[0, 69] = 2.00 * Math.Pow(10, +06); PhotonInteractionCoefficientsAluminium[1, 69] = 3.10 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[2, 69] = 4.27 * Math.Pow(10, -09); PhotonInteractionCoefficientsAluminium[3, 69] = 1.83 * Math.Pow(10, -04); PhotonInteractionCoefficientsAluminium[4, 69] = 3.60 * Math.Pow(10, -11); PhotonInteractionCoefficientsAluminium[5, 69] = 3.08 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsAluminium[0, 70] = 3.00 * Math.Pow(10, +06); PhotonInteractionCoefficientsAluminium[1, 70] = 3.12 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[2, 70] = 2.85 * Math.Pow(10, -09); PhotonInteractionCoefficientsAluminium[3, 70] = 1.27 * Math.Pow(10, -04); PhotonInteractionCoefficientsAluminium[4, 70] = 1.60 * Math.Pow(10, -11); PhotonInteractionCoefficientsAluminium[5, 70] = 3.11 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsAluminium[0, 71] = 4.00 * Math.Pow(10, +06); PhotonInteractionCoefficientsAluminium[1, 71] = 3.14 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[2, 71] = 2.13 * Math.Pow(10, -09); PhotonInteractionCoefficientsAluminium[3, 71] = 9.83 * Math.Pow(10, -05); PhotonInteractionCoefficientsAluminium[4, 71] = 9.00 * Math.Pow(10, -12); PhotonInteractionCoefficientsAluminium[5, 71] = 3.13 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsAluminium[0, 72] = 5.00 * Math.Pow(10, +06); PhotonInteractionCoefficientsAluminium[1, 72] = 3.15 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[2, 72] = 1.71 * Math.Pow(10, -09); PhotonInteractionCoefficientsAluminium[3, 72] = 8.04 * Math.Pow(10, -05); PhotonInteractionCoefficientsAluminium[4, 72] = 5.76 * Math.Pow(10, -12); PhotonInteractionCoefficientsAluminium[5, 72] = 3.14 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsAluminium[0, 73] = 6.00 * Math.Pow(10, +06); PhotonInteractionCoefficientsAluminium[1, 73] = 3.15 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[2, 73] = 1.42 * Math.Pow(10, -09); PhotonInteractionCoefficientsAluminium[3, 73] = 6.82 * Math.Pow(10, -05); PhotonInteractionCoefficientsAluminium[4, 73] = 4.00 * Math.Pow(10, -12); PhotonInteractionCoefficientsAluminium[5, 73] = 3.15 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsAluminium[0, 74] = 8.00 * Math.Pow(10, +06); PhotonInteractionCoefficientsAluminium[1, 74] = 3.16 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[2, 74] = 1.07 * Math.Pow(10, -09); PhotonInteractionCoefficientsAluminium[3, 74] = 5.25 * Math.Pow(10, -05); PhotonInteractionCoefficientsAluminium[4, 74] = 2.25 * Math.Pow(10, -12); PhotonInteractionCoefficientsAluminium[5, 74] = 3.16 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsAluminium[0, 75] = 1.00 * Math.Pow(10, +07); PhotonInteractionCoefficientsAluminium[1, 75] = 3.17 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[2, 75] = 8.54 * Math.Pow(10, -10); PhotonInteractionCoefficientsAluminium[3, 75] = 4.29 * Math.Pow(10, -05); PhotonInteractionCoefficientsAluminium[4, 75] = 1.44 * Math.Pow(10, -12); PhotonInteractionCoefficientsAluminium[5, 75] = 3.16 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsAluminium[0, 76] = 1.50 * Math.Pow(10, +07); PhotonInteractionCoefficientsAluminium[1, 76] = 3.17 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[2, 76] = 5.69 * Math.Pow(10, -10); PhotonInteractionCoefficientsAluminium[3, 76] = 2.96 * Math.Pow(10, -05); PhotonInteractionCoefficientsAluminium[4, 76] = 6.40 * Math.Pow(10, -13); PhotonInteractionCoefficientsAluminium[5, 76] = 3.17 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsAluminium[0, 77] = 2.00 * Math.Pow(10, +07); PhotonInteractionCoefficientsAluminium[1, 77] = 3.18 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[2, 77] = 4.27 * Math.Pow(10, -10); PhotonInteractionCoefficientsAluminium[3, 77] = 2.28 * Math.Pow(10, -05); PhotonInteractionCoefficientsAluminium[4, 77] = 3.60 * Math.Pow(10, -13); PhotonInteractionCoefficientsAluminium[5, 77] = 3.18 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsAluminium[0, 78] = 3.00 * Math.Pow(10, +07); PhotonInteractionCoefficientsAluminium[1, 78] = 3.18 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[2, 78] = 2.85 * Math.Pow(10, -10); PhotonInteractionCoefficientsAluminium[3, 78] = 1.57 * Math.Pow(10, -05); PhotonInteractionCoefficientsAluminium[4, 78] = 1.60 * Math.Pow(10, -13); PhotonInteractionCoefficientsAluminium[5, 78] = 3.18 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsAluminium[0, 79] = 4.00 * Math.Pow(10, +07); PhotonInteractionCoefficientsAluminium[1, 79] = 3.18 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[2, 79] = 2.13 * Math.Pow(10, -10); PhotonInteractionCoefficientsAluminium[3, 79] = 1.21 * Math.Pow(10, -05); PhotonInteractionCoefficientsAluminium[4, 79] = 9.00 * Math.Pow(10, -14); PhotonInteractionCoefficientsAluminium[5, 79] = 3.18 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsAluminium[0, 80] = 5.00 * Math.Pow(10, +07); PhotonInteractionCoefficientsAluminium[1, 80] = 3.19 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[2, 80] = 1.71 * Math.Pow(10, -10); PhotonInteractionCoefficientsAluminium[3, 80] = 9.83 * Math.Pow(10, -06); PhotonInteractionCoefficientsAluminium[4, 80] = 5.76 * Math.Pow(10, -14); PhotonInteractionCoefficientsAluminium[5, 80] = 3.19 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsAluminium[0, 81] = 6.00 * Math.Pow(10, +07); PhotonInteractionCoefficientsAluminium[1, 81] = 3.19 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[2, 81] = 1.42 * Math.Pow(10, -10); PhotonInteractionCoefficientsAluminium[3, 81] = 8.31 * Math.Pow(10, -06); PhotonInteractionCoefficientsAluminium[4, 81] = 4.00 * Math.Pow(10, -14); PhotonInteractionCoefficientsAluminium[5, 81] = 3.19 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsAluminium[0, 82] = 8.00 * Math.Pow(10, +07); PhotonInteractionCoefficientsAluminium[1, 82] = 3.19 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[2, 82] = 1.07 * Math.Pow(10, -10); PhotonInteractionCoefficientsAluminium[3, 82] = 6.37 * Math.Pow(10, -06); PhotonInteractionCoefficientsAluminium[4, 82] = 2.25 * Math.Pow(10, -14); PhotonInteractionCoefficientsAluminium[5, 82] = 3.19 * Math.Pow(10, -02);
            PhotonInteractionCoefficientsAluminium[0, 83] = 1.00 * Math.Pow(10, +08); PhotonInteractionCoefficientsAluminium[1, 83] = 3.19 * Math.Pow(10, -02); PhotonInteractionCoefficientsAluminium[2, 83] = 8.53 * Math.Pow(10, -11); PhotonInteractionCoefficientsAluminium[3, 83] = 5.18 * Math.Pow(10, -06); PhotonInteractionCoefficientsAluminium[4, 83] = 1.44 * Math.Pow(10, -14); PhotonInteractionCoefficientsAluminium[5, 83] = 3.19 * Math.Pow(10, -02);




        }

        // Размещение текстовых полей;
        void CreateUI()
        {
            ArrayTextBox = new TextBox[ArrayOfNames.Length];
            LabelBox = new Label[ArrayOfNames.Length];
            int k = 0;
            int dx = 220;
            int dy = 30;
            int x = 120;
            int y = 0;
            int yLabel = 5;


            for (int t = 0; t < Grupps.Length; t++)
            {

                y += dy;
                Label label = new Label();
                this.Controls.Add(label);
                label.Size = new Size(110, 25);
                label.Location = new Point(x - 110, yLabel);
                label.Text = NameOfGrupps[t];

                for (int i = 0; i < Grupps[t]; i++)
                {
                    ArrayTextBox[k] = new TextBox();
                    this.Controls.Add(ArrayTextBox[k]);
                    ArrayTextBox[k].Size = new Size(80, 25);
                    if (y > (Grupps[t]) * dy)
                    {
                        x += dx;
                        y = dy;
                    }
                    else
                        y += dy;

                    ArrayTextBox[k].Location = new Point(x, y);
                    ArrayTextBox[k].Text = Parameters[k].ToString();

                    LabelBox[k] = new Label();
                    this.Controls.Add(LabelBox[k]);
                    LabelBox[k].Size = new Size(110, 25);
                    LabelBox[k].Location = new Point(x - 110, y);
                    LabelBox[k].Text = ArrayOfNames[k].ToString();
                    k++;
                }
            }

            this.Controls.Add(NameTextBox);
            NameTextBox.Size = new Size(80, 25);
            y += dy;

            NameTextBox.Location = new Point(x, y);
            NameTextBox.Text = writePathOfFile;

            LabelNameTextBox = new Label();
            this.Controls.Add(NameTextBox);
            LabelNameTextBox.Size = new Size(110, 25);
            LabelNameTextBox.Location = new Point(x - 110, y);
            LabelNameTextBox.Text = "Name of file";


            comboBoxMaterial.SelectedIndex = 0;

        }

        // Запоминание настроек
        private void SaveButton_Click(object sender, EventArgs e)
        {
            GetSettings();
        }

        // Получение настроек для расчётов
        void GetSettings()
        {

// *** Begin of Read Parameters
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
			if(line.IndexOf("TypeMaterial: Au")==1) { TypeMaterial = 4; continue; }
			if(line.IndexOf("TypeMaterial: Al")==2) { TypeMaterial = 5; continue; }

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

// *** end of Read Parameters ***

            SetPropertiesOfMaterial();
            PixelScale = WidthScreen / ScreenX;

            r = Math.Sqrt(3) / 2 * Cell;
            CreateArrayScreen();
        }

        // Задание параметров от выюранного материала
        void SetPropertiesOfMaterial()
        {

//            if (comboBoxMaterial.SelectedIndex == 0)
            if (TypeMaterial == 0)
            {
                PhotonInteractionCoefficients = PhotonInteractionCoefficientsNickel;
                density = densityNickel;
                m0 = m0Nickel;

            }

//            if (comboBoxMaterial.SelectedIndex == 1)
            if (TypeMaterial == 1)
            {
                PhotonInteractionCoefficients = PhotonInteractionCoefficientsGold;
                density = densityGold;
                m0 = m0Gold;

            }

 //           if (comboBoxMaterial.SelectedIndex == 2)
            if (TypeMaterial == 2)
            {
                PhotonInteractionCoefficients = PhotonInteractionCoefficientsAluminium;
                density = densityAluminium;
                m0 = m0Aluminium;

            }

            ArrayTextBox[3].Text = density.ToString();
        }

        // Задание массива экрана
        void CreateArrayScreen()
        {
            ArrayPointScreen = new double[ScreenX, ScreenY];

            for (int x = 0; x < ScreenX; x++)
                for (int y = 0; y < ScreenY; y++)
                    ArrayPointScreen[x, y] = 0;

        }

        private void SolveButton_Click(object sender, EventArgs e)
        {
            CreateThreads(CountOfThreads);
        }

        // Создаём потоки
        void CreateThreads(int count)
        {
            for (int i = 0; i < count; i++)
            {
                ThreadAndProperties counter = new ThreadAndProperties(i, 0, 0, 1, 1, 0, Parameters);
                Thread myThread = new Thread(new ThreadStart(counter.Launch));
                myThreads.Insert(myThreads.Count, myThread);
                myThread.Start();

                ProgressTreads.Add(0);
            }
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        // Расчёт через элементарную ячейку

        public double X1;
        public double Y1;
        public double X2;
        public double Y2;
        public double Progress;
        public double StartZ;

        private void SolveButtonOneThread_Click(object sender, EventArgs e)
        {
            GetSettings();

            CreateArrayScreenOfOne();

            X1 = 0;
            Y1 = 0;

            X2 = Math.Truncate(Cell + Wall * Math.Sqrt(3) / 2);
            Y2 = Math.Truncate(Cell * Math.Sqrt(3) + Wall);


            //Console.WriteLine("X2 = " + X2.ToString());
            //Console.WriteLine("Y2 = " + Y2.ToString());


            StartZ = LengthSourceObject + ObjectThikness + LengthObjectLens;

            Launch();

            //WriteGradientFromPart();

            CalculateStepOnScreen();

            WriteGradientFromOneThread();
        }


        // Задание массива экрана для элементарной ячкейки
        void CreateArrayScreenOfOne()
        {
            ArrayPointScreenOfOne = new double[ScreenX, ScreenY];

            for (int x = 0; x < ScreenX; x++)
                for (int y = 0; y < ScreenY; y++)
                    ArrayPointScreenOfOne[x, y] = 0;

        }

        // поток пускает лучи из источника
        public void Launch()
        {
            int rays = 0;
            double x = X1;
            double y = Y1;

            for (x = X1; x <= X2; x++)
            {
                for (y = Y1; y <= Y2; y++)
                {
                    double N0 = NStart;
                    if (x == X1 || x == X2)
                        N0 = N0 / 2;
                    if (y == Y1 || y == Y2)
                        N0 = N0 / 2;

                    MoveRay(new Vector3(x, y, 0), StartEnergy, 0, N0, new Vector3(0, 0, Step));
                    rays++;


                    for (double a = AngleStep; a <= MaxAngleSource; a += AngleStep)
                    {
                        double Na = ReturnStartN(a, AngleRadStep, N0);
                        for (double f = 0; f <= Math.PI * 2; f += AngleRadStep)
                        {
                            MoveRay(new Vector3(x, y, StartZ), StartEnergy, 0, Na, new Vector3(Math.Sin(a) * Math.Sin(f) * Step, -Math.Sin(a) * Math.Cos(f) * Step, Math.Cos(a) * Step));
                        }
                    }


                }
            }
        }

        //
        // Функционал для расчётов
        //

        // Возвращает величину начального пучка под углом
        double ReturnStartN(double Angle, double RadAngle, double N)
        {

            //return N * Math.Cos(Angle) * (RadAngle / Math.PI);
            return N * (RadAngle / Math.PI);
        }

        // Возвращает данные о местонахождении луча (в материале или нет) в линзе
        bool InVoid(Vector3 Position)
        {
            double x = (Position.X) % (X2);
            double y = (Position.Y) % (Y2);

            //x = Math.Abs(x) - (3 * r + Math.Sqrt(3) * Wall) / 2;
            //y = Math.Abs(y) - (2 * Cell + Wall) / 2;

            x = Math.Abs(x);
            y = Math.Abs(y);

            //if (Position.Z >= LengthSourceObject + ObjectThikness + LengthObjectLens && Position.Z <= LengthSourceObject + ObjectThikness + LengthObjectLens + thikness)
            //{
            if (Math.Abs(Position.X) <= LensWidth / 2 && Math.Abs(Position.Y) <= LensHeight / 2)
            {
                if (y <= r / 2 && x <= Cell / 2)
                    return true;
                if (x >= Cell / 2 && x <= Cell && y <= -Math.Sqrt(3) * x + Math.Sqrt(3) * Cell)
                    return true;
                if (x >= X2 - Cell && x <= X2 - Cell / 2 && y >= -Math.Sqrt(3) * x + (Y2 - Math.Sqrt(3) * (X2 - Cell)))
                    return true;
                if (x >= X2 - Cell - X2 && y >= Y2 - r)
                    return true;
                else
                    return false;
            }
            else
                return false;
            //}
            //else
            //return false;
        }

        // Возвращает данные о местонахождении луча (в материале или нет) в теле
        bool InVoidBody(Vector3 Position)
        {
            //if (Math.Abs(Position.X) <= ObjectWidth / 2 && Math.Abs(Position.Y) <= ObjectHeight / 2)
            //{
            //        return true;
            //}
            //else
            return false;
        }

        // Максимум массива
        double Maximum(double[] Array)
        {
            double max = Array[0];

            for (int i = 0; i < Array.Length; i++)
            {
                if (max < Array[i])
                    max = Array[i];
            }

            return max;
        }

        // Перемещение луча
        void MoveRay(Vector3 Position, double Energy, int heir, double N, Vector3 direction)
        {

            if (Position.Z < LengthSourceObject)
            {
                Position += direction * ((LengthSourceObject - Position.Z) / direction.Z);
            }

            while (Position.Z >= LengthSourceObject && Position.Z < LengthSourceObject + ObjectThikness && Energy > 0)
            {
                if (InVoidBody(Position) == false)
                {

                    //N -= GetReduceN(Energy, direction, N);
                    //Energy -= ReduceOfEnergy(Energy, direction);
                    //if (heir < AmountOfScattering)
                    //ScaterringRay(Position, Energy, heir, N, direction);
                    //Debug.Log(N);

                }
                Position += direction;


            }

            if (Position.Z >= LengthSourceObject + ObjectThikness && Position.Z < LengthSourceObject + ObjectThikness + LengthObjectLens)
            {
                Position += direction * ((LengthSourceObject + LengthObjectLens + thikness - Position.Z) / direction.Z) + direction;
            }


            while (Position.Z >= LengthSourceObject + ObjectThikness + LengthObjectLens && Position.Z < LengthSourceObject + ObjectThikness + LengthObjectLens + thikness && Energy > 0)
            {

                //Console.WriteLine(" N = " + N);

                if (InVoid(Position) == false)
                {

                    N -= GetReduceN(Energy, direction, N);
                    if (heir < AmountOfScattering)
                    {
                        ScaterringRayCompton(Position, Energy, heir, N, direction);
                        ScaterringRayRayleigh(Position, Energy, heir, N, direction);
                    }
                    //Debug.Log(N);
                }
                Position += direction;

            }

            if (Position.Z >= LengthSourceObject + ObjectThikness + LengthObjectLens + thikness && Position.Z < LengthSourceObject + ObjectThikness + LengthObjectLens + thikness + LengthLensScreen)
            {
                Position += direction * ((LengthSourceObject + thikness + LengthObjectLens + ObjectThikness + LengthLensScreen - Position.Z) / direction.Z) + direction;
            }

            if (Position.Z >= LengthSourceObject + thikness + LengthObjectLens + ObjectThikness + LengthLensScreen)
                AddEnergyToArray(Position, Energy, N);


        }




        // Комптоновское рассеяние

        // Запуск рассеянного луча
        void ScaterringRayCompton(Vector3 Position, double CurrentEnergy, int heir, double N, Vector3 direction)
        {
            for (double a = AngleStep; a <= MaxAngleScatering; a += AngleStep)
            {
                double n = ScatteringNCompton(CurrentEnergy, a, N, direction);
                //Console.WriteLine(" n Compton = " + n);
                if (n > 0)
                {
                    for (double f = 0; f <= Math.PI * 2; f += AngleRadStep)
                    {
                        MoveRay(Position, CurrentEnergy, heir + 1, n / (AngleRadStep / Math.PI), new Vector3(direction.X * Math.Cos(f) - Math.Sin(f) * (Math.Cos(a) * direction.Y - Math.Sin(a) * direction.Z), direction.X * Math.Sin(f) + Math.Cos(f) * (Math.Cos(a) * direction.Y - Math.Sin(a) * direction.Z), Math.Sin(a) * direction.Y + Math.Cos(a) * direction.Z));
                    }
                }
            }
        }

        // Частиц рассеянного луча
        double ScatteringNCompton(double CurrentEnergy, double angle, double N, Vector3 direction)
        {
            //double k = (((Math.Sin(angle) - Math.Sin(angle - AngleStep) + AngleStep)) / 2 * GetScatterNCompton(CurrentEnergy, direction, N) / 2 * (AngleRadStep / Math.PI * 2));
            double n = 0;

            for (double a = angle - AngleStep; a <= angle; a += AngleStep / 20)
            {
                n += AngleStep / 20 * r0 * r0 / 2 * Math.Pow((1 / (1 + CurrentEnergy * (1 - Math.Cos(angle)))), 2) * (1 + Math.Cos(angle) * Math.Cos(angle) + (Math.Pow(CurrentEnergy, 2) * Math.Pow(1 - Math.Cos(angle), 2)) / (1 + CurrentEnergy * (1 - Math.Cos(angle))));
            }

            n = n * 2.37 * Math.Pow(10, 29);
            n = n * GetScatterNCompton(CurrentEnergy, direction, N);

            n = n / (m0 * GetScatteringNCompton(CurrentEnergy));

            return (n);
        }

        // Рассеяние луча
        double GetScatterNCompton(double CurrentEnergy, Vector3 direction, double N)
        {
            double nn = 0;
            nn = (N * (1 - Math.Exp(-GetScatteringNCompton(CurrentEnergy) * direction.Length() * density / (scale_cm2mm * scale))));
            //nn = nn * 6 * Math.Pow(10, 32);
            return (nn);
        }

        // Расчёт по таблице 
        double GetScatteringNCompton(double Energy)
        {
            double ScatteringN = 0;

            if (Energy < PhotonInteractionCoefficients[0, 0])
                ScatteringN = PhotonInteractionCoefficients[3, 0];
            else
            {
                if (Energy > PhotonInteractionCoefficients[0, PhotonInteractionCoefficients.Length / 6 - 1])
                {
                    int i = PhotonInteractionCoefficients.Length / 6 - 1;
                    ScatteringN = PhotonInteractionCoefficients[0, 1] - (PhotonInteractionCoefficients[0, i] - Energy) * (PhotonInteractionCoefficients[3, i] - PhotonInteractionCoefficients[3, i - 1]) / (PhotonInteractionCoefficients[0, i] - PhotonInteractionCoefficients[0, i - 1]);

                }
                else
                {
                    int i = 1;

                    while (Energy > PhotonInteractionCoefficients[0, i] && i < PhotonInteractionCoefficients.Length / 6)
                    {
                        i++;
                    }

                    ScatteringN = PhotonInteractionCoefficients[1, i] - (PhotonInteractionCoefficients[0, i] - Energy) * (PhotonInteractionCoefficients[3, i] - PhotonInteractionCoefficients[3, i - 1]) / (PhotonInteractionCoefficients[0, i] - PhotonInteractionCoefficients[0, i - 1]);

                }
            }


            return ScatteringN;
        }


        // Рэдеевское рассеяение

        // Запуск рассеянного луча
        void ScaterringRayRayleigh(Vector3 Position, double CurrentEnergy, int heir, double N, Vector3 direction)
        {
            for (double a = AngleStep; a <= MaxAngleScatering; a += AngleStep)
            {
                double n = ScatteringNRayleigh(CurrentEnergy, a, N, direction);
                //Console.WriteLine(" n Rayleigh = " + n);
                if (n > 0)
                {
                    for (double f = 0; f <= Math.PI * 2; f += AngleRadStep)
                    {
                        MoveRay(Position, CurrentEnergy, heir + 1, n / (AngleRadStep / Math.PI), new Vector3(direction.X * Math.Cos(f) - Math.Sin(f) * (Math.Cos(a) * direction.Y - Math.Sin(a) * direction.Z), direction.X * Math.Sin(f) + Math.Cos(f) * (Math.Cos(a) * direction.Y - Math.Sin(a) * direction.Z), Math.Sin(a) * direction.Y + Math.Cos(a) * direction.Z));
                    }
                }
            }
        }

        // Частиц рассеянного луча
        double ScatteringNRayleigh(double CurrentEnergy, double angle, double N, Vector3 direction)
        {
            //double k = (((Math.Sin(angle) - Math.Sin(angle - AngleStep) + AngleStep)) / 2 * GetScatterNCompton(CurrentEnergy, direction, N) / 2 * (AngleRadStep / Math.PI * 2));

            double n = 0;

            for (double a = angle - AngleStep; a <= angle; a += AngleStep / 20)
            {
                n += AngleStep / 20  * r0 * r0 / 2 * Math.Pow((1 / (1 + CurrentEnergy * (1 - Math.Cos(angle)))), 2) * (1 + Math.Cos(angle) * Math.Cos(angle) + (Math.Pow(CurrentEnergy, 2) * Math.Pow(1 - Math.Cos(angle), 2)) / (1 + CurrentEnergy * (1 - Math.Cos(angle))));
            }

            n = n * 2.37 * Math.Pow(10, 29);
            n = n * GetScatterNRayleigh(CurrentEnergy, direction, N);

            n = n / (m0 * GetScatteringNRayleigh(CurrentEnergy));

            return (n);
        }

        // Рассеяние луча
        double GetScatterNRayleigh(double CurrentEnergy, Vector3 direction, double N)
        {
            double nn = 0;
            nn = (N * (1 - Math.Exp(-GetScatteringNRayleigh(CurrentEnergy) * direction.Length() * density / (scale_cm2mm * scale))));
            //nn = nn * 6 * Math.Pow(10, 32);
            return (nn);
        }

        // Расчёт по таблице 
        double GetScatteringNRayleigh(double Energy)
        {
            double ScatteringN = 0;


            if (Energy < PhotonInteractionCoefficients[0, 0])
                ScatteringN += PhotonInteractionCoefficients[4, 0];
            else
            {
                if (Energy > PhotonInteractionCoefficients[0, PhotonInteractionCoefficients.Length / 6 - 1])
                {
                    int i = PhotonInteractionCoefficients.Length / 6 - 1;
                    ScatteringN += PhotonInteractionCoefficients[0, 1] - (PhotonInteractionCoefficients[0, i] - Energy) * (PhotonInteractionCoefficients[4, i] - PhotonInteractionCoefficients[4, i - 1]) / (PhotonInteractionCoefficients[0, i] - PhotonInteractionCoefficients[0, i - 1]);

                }
                else
                {
                    int i = 1;

                    while (Energy > PhotonInteractionCoefficients[0, i] && i < PhotonInteractionCoefficients.Length / 6)
                    {
                        i++;
                    }

                    ScatteringN += PhotonInteractionCoefficients[1, i] - (PhotonInteractionCoefficients[0, i] - Energy) * (PhotonInteractionCoefficients[4, i] - PhotonInteractionCoefficients[4, i - 1]) / (PhotonInteractionCoefficients[0, i] - PhotonInteractionCoefficients[0, i - 1]);

                }
            }

            return ScatteringN;
        }
        


        // Уменьшение частиц основного луча
        double GetReduceN(double CurrentEnergy, Vector3 direction, double N)
        {
            double n = 0;

            n = (N * (1 - Math.Exp(-GetReducingN(CurrentEnergy) * direction.Length() * density / (scale_cm2mm * scale))));

            //Console.WriteLine(" old N = " + N);
            //Console.WriteLine(" CurrentEnergy = " + CurrentEnergy);
            //Console.WriteLine(" GetReducingN = " + GetReducingN(CurrentEnergy));
            //Console.WriteLine(" step = " + direction.Length());
            //Console.WriteLine(" reduce n = " + n);
            //Console.WriteLine("exp = " + Math.Exp(-GetReducingN(CurrentEnergy) * direction.Length() * density / (scale_cm2mm * scale)));
            //Console.WriteLine(" new N = " + (N - n));
            return n;
        }

        // Расчёт уменьшения по таблице 
        double GetReducingN(double Energy)
        {
            double Reducing = 0;

            if (Energy < PhotonInteractionCoefficients[0, 0])
                return PhotonInteractionCoefficients[1, 0];
            else
            {
                if (Energy > PhotonInteractionCoefficients[0, PhotonInteractionCoefficients.Length / 6 - 1])
                {
                    int i = PhotonInteractionCoefficients.Length / 6 - 1;
                    Reducing = PhotonInteractionCoefficients[0, 1] - (PhotonInteractionCoefficients[0, i] - Energy) * (PhotonInteractionCoefficients[1, i] - PhotonInteractionCoefficients[1, i - 1]) / (PhotonInteractionCoefficients[0, i] - PhotonInteractionCoefficients[0, i - 1]);
                    return Reducing;
                }
                else
                {
                    int i = 1;

                    while (Energy > PhotonInteractionCoefficients[0, i] && i < PhotonInteractionCoefficients.Length / 6)
                    {
                        i++;
                    }

                    Reducing = PhotonInteractionCoefficients[1, i] - (PhotonInteractionCoefficients[0, i] - Energy) * (PhotonInteractionCoefficients[1, i] - PhotonInteractionCoefficients[1, i - 1]) / (PhotonInteractionCoefficients[0, i] - PhotonInteractionCoefficients[0, i - 1]);

                    return Reducing;
                }
            }
        }

        // Добавление энергии в ячейку экрана
        void AddEnergyToArray(Vector3 Position, double Energy, double N)
        {
            int SX, SY = 0;

            if (Math.Abs(Position.X) < Width / 2 && Math.Abs(Position.X) < Height / 2)
            {
                if (Position.X <= 0)
                    SX = (int)((-Width / 2 - Position.X) * (ScreenX / Width) + ScreenX / 2);
                else
                    SX = (int)(Position.X / (Width) * ScreenX + ScreenX / 2);

                if (Position.Y <= 0)
                    SY = (int)((-Height / 2 - Position.Y) * ScreenY / Height + ScreenY / 2);
                else
                    SY = (int)(Position.Y / (Height) * ScreenY + ScreenY / 2);

                if (SX >= 0 && SY >= 0 && SX < ScreenX && SY < ScreenY)
                {
                    if (ArrayPointScreenOfOne[SX, SY] == null || ArrayPointScreenOfOne[SX, SY] == 0)
                        ArrayPointScreenOfOne[SX, SY] = Energy * N;
                    ArrayPointScreenOfOne[SX, SY] += Energy * N;

                    //Console.WriteLine(" Sx = " + SX);
                    //Console.WriteLine(" Sy = " + SY);

                    //Console.WriteLine(" E = " + Energy);
                    //Console.WriteLine(" N = " + N);
                    //Console.WriteLine(" E * N = " + ArrayPointScreenOfOne[SX, SY]);
                }

            }

        }

        void WriteGradientFromPart()
        {
            double Xp = 0;
            double Yp = 0;


            string Path = writePathOfFolder + writePathOfFile + "Particle" + writeOfExtension;

            string TextLine = "размер экрана " + ScreenX.ToString() + " x " + ScreenY.ToString();

            try
            {
                using (StreamWriter sw = new StreamWriter(Path, false, System.Text.Encoding.Default))
                {
                    sw.WriteLine(TextLine + "\n");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            TextLine = "Pixel X\tPixel Y\tX, мм\tY, мм\tЭнергия, кэВ";

            try
            {
                using (StreamWriter sw = new StreamWriter(Path, false, System.Text.Encoding.Default))
                {
                    sw.WriteLine(TextLine + "\n");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            int x = ScreenX / 2;
            int y = 0;

            //for (y = 0; y < ScreenY; y++)
            {
                Xp = -WidthScreen / scale / 2 + x * PixelScale / scale;
                Yp = -HeightScreen / scale / 2 + y * PixelScale / scale;

                TextLine = x.ToString() + "\t" + y.ToString() + "\t" + Xp.ToString() + "\t" + Yp.ToString() + "\t" + ArrayPointScreen[x, y].ToString();

                try
                {
                    using (StreamWriter sw = new StreamWriter(Path, true, System.Text.Encoding.Default))
                    {
                        sw.WriteLine(TextLine);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            }


        }

        // Поиск следа на экране
        void CalculateStepOnScreen()
        {

            double[,] ArrayPointScreen1Reflection = new double[ScreenX, ScreenY];
            //double[,] ArrayPointScreen2Reflection = new double[ScreenX, ScreenY];


            for (int x = 0; x < ScreenX; x++)
                for (int y = 0; y < ScreenY; y++)
                    ArrayPointScreen1Reflection[x, y] = ArrayPointScreenOfOne[x, y] + ArrayPointScreenOfOne[ScreenX - 1 - x, y];



            for (int x = 0; x < ScreenX; x++)
                for (int y = 0; y < ScreenY; y++)
                    ArrayPointScreenOfOne[x, y] = ArrayPointScreen1Reflection[x, y] + ArrayPointScreen1Reflection[x, ScreenY - 1 - y];

            ArrayPointScreen1Reflection = null;

            //LensWidth = 20;
            //LensHeight = 20;
            //PixelScale

            /*
            double[,] ArrayPoint = new double[ScreenX, ScreenY];

            for (int x = 0; x < ScreenX; x++)
                for (int y = 0; y < ScreenY; y++)
                    ArrayPoint[x, y] = 0;


            for (int xBox = (int)-LensWidth / 2 * scale; xBox <= LensWidth / 2 * scale; xBox = +(int)X2 * 2)
                for (int yBox = (int)-LensHeight / 2 * scale; yBox <= LensHeight / 2 * scale; yBox = +(int)Y2 * 2)
                {

                    for (int x = 0; x < ScreenX; x++)
                        for (int y = 0; y < ScreenY; y++)
                        {
                            if (x + (int)((WidthScreen / 2 - LensWidth / 2 + xBox) / PixelScale) - ScreenX / 2 > 0 && x + (int)((WidthScreen / 2 - LensWidth / 2 + xBox) / PixelScale) - ScreenX / 2 < ScreenX)
                                if (y + (int)((HeightScreen / 2 - LensHeight / 2 + yBox) / PixelScale) - ScreenY / 2 > 0 && y + (int)((HeightScreen / 2 - LensHeight / 2 + yBox) / PixelScale) - ScreenY / 2 < ScreenY)
                                    ArrayPoint[x + (int)((WidthScreen / 2 - LensWidth / 2 + xBox) / PixelScale) - ScreenX / 2, y + (int)((HeightScreen / 2 - LensHeight / 2 + yBox) / PixelScale) - ScreenY / 2] += ArrayPointScreen2Reflection[x, y];
                        }

                }


            for (int xBox = (int)-ObjectWidth / 2 * scale; xBox <= ObjectWidth / 2 * scale; xBox = +(int)X2 * 2)
                for (int yBox = (int)-ObjectHeight / 2 * scale; yBox <= ObjectHeight / 2 * scale; yBox = +(int)Y2 * 2)
                {

                    for (int x = 0; x < ScreenX; x++)
                        for (int y = 0; y < ScreenY; y++)
                        {
                            if (x + (int)((WidthScreen / 2 - ObjectWidth / 2 + xBox) / PixelScale) - ScreenX / 2 > 0 && x + (int)((WidthScreen / 2 - ObjectWidth / 2 + xBox) / PixelScale) - ScreenX / 2 < ScreenX)
                                if (y + (int)((HeightScreen / 2 - ObjectHeight / 2 + yBox) / PixelScale) - ScreenY / 2 > 0 && y + (int)((HeightScreen / 2 - ObjectHeight / 2 + yBox) / PixelScale) - ScreenY / 2 < ScreenY)
                                    ArrayPoint[x + (int)((WidthScreen / 2 - ObjectWidth / 2 + xBox) / PixelScale) - ScreenX / 2, y + (int)((HeightScreen / 2 - ObjectHeight / 2 + yBox) / PixelScale) - ScreenY / 2] -= ArrayPointScreen2Reflection[x, y];
                        }

                }


            for (int x = 0; x < ScreenX; x++)
                for (int y = 0; y < ScreenY; y++)
                    ArrayPointScreen[x, y] = ArrayPoint[x, y];*/

            CopyArrayScreen();
            //CalculateScreen();
        }

        void CalculateScreen()
        {

            double[,] ArrayPointX = new double[ScreenX, ScreenY];

            for (int x = 0; x < ScreenX; x++)
                for (int y = 0; y < ScreenY; y++)
                    ArrayPointX[x, y] = 0;

            Console.WriteLine("X2 = " + X2.ToString());
            Console.WriteLine("Y2 = " + Y2.ToString());


            int xBox = 0;
            int yBox = 0;

            for (xBox = (int)-LensWidth / 2 * scale; xBox <= LensWidth / 2 * scale; xBox = +(int)X2 * 2)
            {

                for (int x = 0; x < ScreenX; x++)
                    for (int y = 0; y < ScreenY; y++)
                    {
                        if (x + (int)((WidthScreen / 2 - LensWidth / 2 + xBox) / PixelScale) - ScreenX / 2 > 0 && x + (int)((WidthScreen / 2 - LensWidth / 2 + xBox) / PixelScale) - ScreenX / 2 < ScreenX)
                            if (y + (int)((HeightScreen / 2 - LensHeight / 2 + yBox) / PixelScale) - ScreenY / 2 > 0 && y + (int)((HeightScreen / 2 - LensHeight / 2 + yBox) / PixelScale) - ScreenY / 2 < ScreenY)
                                ArrayPointX[x + (int)((WidthScreen / 2 - LensWidth / 2 + xBox) / PixelScale) - ScreenX / 2, y + (int)((HeightScreen / 2 - LensHeight / 2 + yBox) / PixelScale) - ScreenY / 2] += ArrayPointScreenOfOne[x, y];
                    }

            }


            for (yBox = (int)-LensHeight / 2 * scale; yBox <= -ObjectHeight / 2 * scale; yBox = +(int)Y2 * 2)
            {
                for (int x = 0; x < ScreenX; x++)
                    for (int y = 0; y < ScreenY; y++)
                        ArrayPointScreen[x + (int)((WidthScreen / 2 - LensWidth / 2 + xBox) / PixelScale) - ScreenX / 2, y + (int)((HeightScreen / 2 - LensHeight / 2 + yBox) / PixelScale) - ScreenY / 2] += ArrayPointX[x, y];
            }


            for (yBox = (int)ObjectHeight / 2 * scale; yBox <= LensHeight / 2 * scale; yBox = +(int)Y2 * 2)
            {
                for (int x = 0; x < ScreenX; x++)
                    for (int y = 0; y < ScreenY; y++)
                        ArrayPointScreen[x + (int)((WidthScreen / 2 - LensWidth / 2 + xBox) / PixelScale) - ScreenX / 2, y + (int)((HeightScreen / 2 - LensHeight / 2 + yBox) / PixelScale) - ScreenY / 2] += ArrayPointX[x, y];
            }



            ArrayPointX = null;

            double[,] ArrayPointY = new double[ScreenX, ScreenY];

            xBox = 0;
            for (yBox = (int)-ObjectHeight / 2 * scale; yBox <= ObjectHeight / 2 * scale; yBox = +(int)Y2 * 2)
            {

                for (int x = 0; x < ScreenX; x++)
                    for (int y = 0; y < ScreenY; y++)
                    {
                        if (x + (int)((WidthScreen / 2 - LensWidth / 2 + xBox) / PixelScale) - ScreenX / 2 > 0 && x + (int)((WidthScreen / 2 - LensWidth / 2 + xBox) / PixelScale) - ScreenX / 2 < ScreenX)
                            if (y + (int)((HeightScreen / 2 - LensHeight / 2 + yBox) / PixelScale) - ScreenY / 2 > 0 && y + (int)((HeightScreen / 2 - LensHeight / 2 + yBox) / PixelScale) - ScreenY / 2 < ScreenY)
                                ArrayPointY[x + (int)((WidthScreen / 2 - LensWidth / 2 + xBox) / PixelScale) - ScreenX / 2, y + (int)((HeightScreen / 2 - LensHeight / 2 + yBox) / PixelScale) - ScreenY / 2] += ArrayPointScreenOfOne[x, y];
                    }

            }


            for (xBox = (int)-LensWidth / 2 * scale; xBox <= -ObjectWidth / 2 * scale; xBox = +(int)X2 * 2)
            {
                for (int x = 0; x < ScreenX; x++)
                    for (int y = 0; y < ScreenY; y++)
                        ArrayPointScreen[x + (int)((WidthScreen / 2 - LensWidth / 2 + xBox) / PixelScale) - ScreenX / 2, y + (int)((HeightScreen / 2 - LensHeight / 2 + yBox) / PixelScale) - ScreenY / 2] += ArrayPointY[x, y];
            }


            for (xBox = (int)ObjectWidth / 2 * scale; xBox <= LensWidth / 2 * scale; xBox = +(int)X2 * 2)
            {
                for (int x = 0; x < ScreenX; x++)
                    for (int y = 0; y < ScreenY; y++)
                        ArrayPointScreen[x + (int)((WidthScreen / 2 - LensWidth / 2 + xBox) / PixelScale) - ScreenX / 2, y + (int)((HeightScreen / 2 - LensHeight / 2 + yBox) / PixelScale) - ScreenY / 2] += ArrayPointY[x, y];
            }


            //ObjectWidth

            /*
            for (int xBox = (int)-LensWidth / 2 * scale; xBox <= LensWidth / 2 * scale; xBox = +(int)X2 * 2)
                for (int yBox = (int)-LensHeight / 2 * scale; yBox <= LensHeight / 2 * scale; yBox = +(int)Y2 * 2)
                {

                    for (int x = 0; x < ScreenX; x++)
                        for (int y = 0; y < ScreenY; y++)
                        {
                            if (x + (int)((WidthScreen / 2 - LensWidth / 2 + xBox) / PixelScale) - ScreenX / 2 > 0 && x + (int)((WidthScreen / 2 - LensWidth / 2 + xBox) / PixelScale) - ScreenX / 2 < ScreenX)
                                if (y + (int)((HeightScreen / 2 - LensHeight / 2 + yBox) / PixelScale) - ScreenY / 2 > 0 && y + (int)((HeightScreen / 2 - LensHeight / 2 + yBox) / PixelScale) - ScreenY / 2 < ScreenY)
                                    ArrayPoint[x + (int)((WidthScreen / 2 - LensWidth / 2 + xBox) / PixelScale) - ScreenX / 2, y + (int)((HeightScreen / 2 - LensHeight / 2 + yBox) / PixelScale) - ScreenY / 2] += ArrayPointScreenOfOne[x, y];
                        }

                }


            for (int xBox = (int)-ObjectWidth / 2 * scale; xBox <= ObjectWidth / 2 * scale; xBox = +(int)X2 * 2)
                for (int yBox = (int)-ObjectHeight / 2 * scale; yBox <= ObjectHeight / 2 * scale; yBox = +(int)Y2 * 2)
                {

                    for (int x = 0; x < ScreenX; x++)
                        for (int y = 0; y < ScreenY; y++)
                        {
                            if (x + (int)((WidthScreen / 2 - ObjectWidth / 2 + xBox) / PixelScale) - ScreenX / 2 > 0 && x + (int)((WidthScreen / 2 - ObjectWidth / 2 + xBox) / PixelScale) - ScreenX / 2 < ScreenX)
                                if (y + (int)((HeightScreen / 2 - ObjectHeight / 2 + yBox) / PixelScale) - ScreenY / 2 > 0 && y + (int)((HeightScreen / 2 - ObjectHeight / 2 + yBox) / PixelScale) - ScreenY / 2 < ScreenY)
                                    ArrayPoint[x + (int)((WidthScreen / 2 - ObjectWidth / 2 + xBox) / PixelScale) - ScreenX / 2, y + (int)((HeightScreen / 2 - ObjectHeight / 2 + yBox) / PixelScale) - ScreenY / 2] -= ArrayPointScreenOfOne[x, y];
                        }

                }


            for (int x = 0; x < ScreenX; x++)
                for (int y = 0; y < ScreenY; y++)
                    ArrayPointScreen[x, y] = ArrayPoint[x, y];*/


        }

        void CopyArrayScreen()
        {

            double[,] ArrayPointX = new double[ScreenX, ScreenY];

            for (int x = 0; x < ScreenX; x++)
                for (int y = 0; y < ScreenY; y++)
                    ArrayPointX[x, y] = 0;

            //Console.WriteLine("X2 = " + X2.ToString());
            //Console.WriteLine("Y2 = " + Y2.ToString());
            

            int xBox = 0;
            int yBox = 0;

            int Xn = 0;
            int Yn = 0;

            for (xBox = (int)-LensWidth / 2; xBox < LensWidth / 2; xBox += (int)X2 * 2)
            {
                //Console.WriteLine("xBox = " + xBox.ToString());

                for (int x = 0; x < ScreenX; x++)
                    for (int y = 0; y < ScreenY; y++)
                    {
                        Xn = x + (int)((WidthScreen / 2 - LensWidth / 2 + xBox) / PixelScale) - ScreenX / 2;
                        Yn = y + (int)((HeightScreen / 2 - LensHeight / 2 + yBox) / PixelScale) - ScreenY / 2;

                        if (Xn > 0 && Xn < ScreenX)
                            if (Yn > 0 && Yn < ScreenY)
                                ArrayPointX[Xn , Yn] += ArrayPointScreenOfOne[x, y];
                    }

            }


            for (yBox = (int)-LensHeight / 2; yBox < -ObjectHeight / 2; yBox += (int)Y2 * 2)
            {
                for (int x = 0; x < ScreenX; x++)
                    for (int y = 0; y < ScreenY; y++)
                    {

                        Xn = x + (int)((WidthScreen / 2 - LensWidth / 2 + xBox) / PixelScale) - ScreenX / 2;
                        Yn = y + (int)((HeightScreen / 2 - LensHeight / 2 + yBox) / PixelScale) - ScreenY / 2;

                        if (Xn > 0 && Xn < ScreenX)
                            if (Yn > 0 && Yn < ScreenY)
                                ArrayPointScreen[Xn, Yn] += ArrayPointX[x, y];
                    }
            }


            for (yBox = (int)ObjectHeight / 2; yBox < LensHeight / 2; yBox += (int)Y2 * 2)
            {
                for (int x = 0; x < ScreenX; x++)
                    for (int y = 0; y < ScreenY; y++)
                    {

                        Xn = x + (int)((WidthScreen / 2 - LensWidth / 2 + xBox) / PixelScale) - ScreenX / 2;
                        Yn = y + (int)((HeightScreen / 2 - LensHeight / 2 + yBox) / PixelScale) - ScreenY / 2;

                        if (Xn > 0 && Xn < ScreenX)
                            if (Yn > 0 && Yn < ScreenY)
                                ArrayPointScreen[Xn, Yn] += ArrayPointX[x, y];
                    }
            }
            


            ArrayPointX = null;

            double[,] ArrayPointY = new double[ScreenX, ScreenY];

            xBox = 0;
            for (yBox = (int)-ObjectHeight / 2; yBox < ObjectHeight / 2; yBox += (int)Y2 * 2)
            {

                for (int x = 0; x < ScreenX; x++)
                    for (int y = 0; y < ScreenY; y++)
                    {
                        Xn = x + (int)((WidthScreen / 2 - LensWidth / 2 + xBox) / PixelScale) - ScreenX / 2;
                        Yn = y + (int)((HeightScreen / 2 - LensHeight / 2 + yBox) / PixelScale) - ScreenY / 2;

                        if (Xn > 0 && Xn < ScreenX)
                            if (Yn > 0 && Yn < ScreenY)
                                ArrayPointY[Xn, Yn] += ArrayPointScreenOfOne[x, y];
                    }

            }


            for (xBox = (int)-LensWidth / 2; xBox < -ObjectWidth / 2; xBox += (int)X2 * 2)
            {
                for (int x = 0; x < ScreenX; x++)
                    for (int y = 0; y < ScreenY; y++)
                    {

                        Xn = x + (int)((WidthScreen / 2 - LensWidth / 2 + xBox) / PixelScale) - ScreenX / 2;
                        Yn = y + (int)((HeightScreen / 2 - LensHeight / 2 + yBox) / PixelScale) - ScreenY / 2;

                        if (Xn > 0 && Xn < ScreenX)
                            if (Yn > 0 && Yn < ScreenY)
                                ArrayPointScreen[Xn, Yn] += ArrayPointY[x, y];
                    }
            }
            

            for (xBox = (int)ObjectWidth / 2; xBox < LensWidth / 2; xBox += (int)X2 * 2)
            {
                for (int x = 0; x < ScreenX; x++)
                    for (int y = 0; y < ScreenY; y++)
                    {

                        Xn = x + (int)((WidthScreen / 2 - LensWidth / 2 + xBox) / PixelScale) - ScreenX / 2;
                        Yn = y + (int)((HeightScreen / 2 - LensHeight / 2 + yBox) / PixelScale) - ScreenY / 2;

                        if (Xn > 0 && Xn < ScreenX)
                            if (Yn > 0 && Yn < ScreenY)
                                ArrayPointScreen[Xn, Yn] += ArrayPointY[x, y];
                    }
            }

            
        }



        // Вывод результатов

        private void buttonSaveScreen_Click(object sender, EventArgs e)
        {
            WriteResultFromOneThread();
        }

        void WriteResultFromOneThread()
        {
            double Xp = 0;
            double Yp = 0;

            string Path = writePathOfFolder + writePathOfFile + "Screen" + writeOfExtension;

            string TextLine = "размер экрана" + ScreenX.ToString() + " x " + ScreenY.ToString();


            try
            {
                using (StreamWriter sw = new StreamWriter(Path, false, System.Text.Encoding.Default))
                {
                    sw.WriteLine(TextLine + "\n");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            TextLine = "Pixel X\tPixel Y\tX, мм\tY, мм\tЭнергия, кэВ";

            try
            {
                using (StreamWriter sw = new StreamWriter(Path, false, System.Text.Encoding.Default))
                {
                    sw.WriteLine(TextLine + "\n");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


            for (int x = 0; x < ScreenX; x++)
            {
                for (int y = 0; y < ScreenY; y++)
                {
                    Xp = -WidthScreen / scale / 2 + x * PixelScale / scale;
                    Yp = -HeightScreen / scale / 2 + y * PixelScale / scale;

                    TextLine = x.ToString() + "\t" + y.ToString() + "\t" + Xp.ToString() + "\t" + Yp.ToString() + "\t" + ArrayPointScreen[x, y].ToString();

                    try
                    {
                        using (StreamWriter sw = new StreamWriter(Path, true, System.Text.Encoding.Default))
                        {
                            sw.WriteLine(TextLine);
                        }
                        //Console.WriteLine("Запись выполнена");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                }
            }
            
        }

        private void buttonSaveGradient_Click(object sender, EventArgs e)
        {
            WriteGradientFromOneThread();
        }

        void WriteGradientFromOneThread()
        {
            double Xp = 0;
            double Yp = 0;


            string Path = writePathOfFolder + writePathOfFile + "Gradient" + writeOfExtension;

            string TextLine = "размер экрана " + ScreenX.ToString() + " x " + ScreenY.ToString();
            
            try
            {
                using (StreamWriter sw = new StreamWriter(Path, false, System.Text.Encoding.Default))
                {
                    sw.WriteLine(TextLine + "\n");
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            TextLine = "Pixel X\tPixel Y\tX, мм\tY, мм\tЭнергия, кэВ";

            try
            {
                using (StreamWriter sw = new StreamWriter(Path, false, System.Text.Encoding.Default))
                {
                    sw.WriteLine(TextLine + "\n");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            int x = ScreenX / 2;

            for (int y = 0; y < ScreenY; y++)
            {
                Xp = -WidthScreen / scale / 2 + x * PixelScale / scale;
                Yp = -HeightScreen / scale / 2 + y * PixelScale / scale;

                TextLine = x.ToString() + "\t" + y.ToString() + "\t" + Xp.ToString() + "\t" + Yp.ToString() + "\t" + ArrayPointScreen[x, y].ToString();

                try
                {
                    using (StreamWriter sw = new StreamWriter(Path, true, System.Text.Encoding.Default))
                    {
                        sw.WriteLine(TextLine);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            }
            
            
        }


    }
}

public class ThreadAndProperties
{

    public int CountOfTread;
    public double X1;
    public double Y1;
    public double X2;
    public double Y2;
    public double Progress;

    public double[] Parameters;

    // Lens

    double Cell = 10;
    double Wall = 10;
    double thikness = 100;
    double density = 10;
    double WightToEdge = 20;
    double HeightToEdge = 20;
    double LensWidth = 100;
    double LensHeight = 100;

    // Screen

    int ScreenX = 10000;
    int ScreenY = 10000;
    double WidthScreen = 100;
    double HeightScreen = 100;

    // Settings

    double WidthSource = 10;
    double HeightSource = 10;
    double StartEnergy = 60;
    double LengthSourceLens = 100;
    double LengthLensObject = 0;
    double LengthObjectScreen = 1000;
    double MaxAngleSource = Math.PI / 180 * 2;
    double N = 1000000;

    // Steps

    int AmountOfScattering = 2;
    double MaxAngleScatering = Math.PI / 180 * 2;
    double Step = 5;
    double AngleStep = Math.PI / 180;
    double AngleRadStep = Math.PI / 18;

    // Object

    double ObjectWidth = 0;
    double ObjectHeight = 0;
    double ObjectThikness = 0;

    // Для расчёта

    public double[,] ArrayPointScreen;
    double R;

    int Width = 0;
    int Height = 0;

    int CountOfTable = 30;
    Vector2[] ReducingMount;


    public ThreadAndProperties(int countOfTread, double x1, double y1, double x2, double y2, double progress, double[] p)
    {
        this.CountOfTread = countOfTread;
        this.X1 = x1;
        this.Y1 = y1;
        this.X2 = x2;
        this.Y2 = y2;
        this.Progress = progress;
        this.Parameters = p;

        Initialiase();
        Launch();
    }

    public void Initialiase()
    {
        // Lens

        Cell = Parameters[0];
        Wall = Parameters[1];
        thikness = Parameters[2];
        density = Parameters[3];
        WightToEdge = Parameters[4];
        HeightToEdge = Parameters[5];
        LensWidth = Parameters[6];
        LensHeight = Parameters[7];

        // Screen

        WidthScreen = Parameters[8];
        HeightScreen = Parameters[9];
        ScreenX = (int)Parameters[10];
        ScreenY = (int)Parameters[11];

        // Settings

        WidthSource = Parameters[12];
        HeightSource = Parameters[13];
        StartEnergy = Parameters[14];
        LengthSourceLens = Parameters[15];
        LengthLensObject = Parameters[16];
        LengthObjectScreen = Parameters[17];
        MaxAngleSource = Parameters[18];
        N = Parameters[19];

        // Steps

        AmountOfScattering = (int)Parameters[20];
        MaxAngleScatering = Parameters[21];
        Step = Parameters[22];
        AngleStep = Parameters[23];
        AngleRadStep = Parameters[24];

        // Object

        ObjectWidth = Parameters[25];
        ObjectHeight = Parameters[26];
        ObjectThikness = Parameters[27];

        R = 2 / Math.Sqrt(3) * Cell;
        Width = 1;
        Height = 1;

        ReducingMount = new Vector2[CountOfTable];
        SetTable(CountOfTable, ReducingMount);



        CreateArrayScreen();
    }


    // Запись таблиц уменьшения
    void SetTable(int Length, Vector2[] Table)
    {
        double a = 0;
        double b = 0;

        for (int i = 0; i < Table.Length; i++)
        {
            a = 0.425f * Math.Exp(0.258f * i);
            b = 2074 * Math.Pow(i, -1.4f);
            Table[i] = new Vector2(a, b);

        }

    }

    // Задание массива экрана
    void CreateArrayScreen()
    {
        ArrayPointScreen = new double[ScreenX, ScreenY];

        for (int x = 0; x < ScreenX; x++)
            for (int y = 0; y < ScreenY; y++)
                ArrayPointScreen[x, y] = 0;

    }

    // поток пускает лучи из источника
    public void Launch()
    {
        int rays = 0;
        for (double i = X1; i <= X2; i++)
        {
            for (double t = Y1; t <= Y2; t++)
            {
                int N0 = (int)N;
                if (i == X1 || i == X2)
                    N0 = N0 / 2;
                if (t == Y1 || t == Y2)
                    N0 = N0 / 2;

                MoveRay(new Vector3(i, t, 0), StartEnergy, 0, N, new Vector3(0, 0, Step));
                rays++;


                for (double a = 0; a <= MaxAngleSource; a += AngleStep)
                {
                    for (double f = 0; f <= Math.PI * 2; f += AngleRadStep)
                    {
                        MoveRay(new Vector3(i, t, 0), StartEnergy, 0, ReturnStartN(a, AngleRadStep), new Vector3(Math.Sin(a) * Math.Sin(f) * Step, -Math.Sin(a) * Math.Cos(f) * Step, Math.Cos(a) * Step));
                    }
                }


            }
        }
    }

    //
    // Функционал для расчётов
    //

    // Возвращает величину начального пучка под углом
    double ReturnStartN(double Angle, double RadAngle)
    {

        return N * Math.Cos(Angle) * (RadAngle / Math.PI);
    }

    // Возвращает данные о местонахождении луча (в материале или нет) в линзе
    bool InVoid(Vector3 Position)
    {
        double x = Math.Abs(Position.X) % (3 * R + Math.Sqrt(3) * Wall);
        double y = Math.Abs(Position.Y) % (2 * Cell + Wall);

        x = Math.Abs(x) - (3 * R + Math.Sqrt(3) * Wall) / 2;
        y = Math.Abs(y) - (2 * Cell + Wall) / 2;

        x = Math.Abs(x);
        y = Math.Abs(y);

        if (Position.Z >= LengthSourceLens && Position.Z <= LengthSourceLens + thikness)
        {
            if (Math.Abs(Position.X) <= LensWidth / 2 && Math.Abs(Position.Y) <= LensHeight / 2)
            {
                if (x <= R / 2 && y <= Cell)
                    return true;
                if (x >= R / 2 && x <= R && y <= -Math.Sqrt(3) * x + 2 * Cell)
                    return true;
                if (x >= R / 2 + Math.Sqrt(3) / 2 * Wall && x <= R + Math.Sqrt(3) / 2 * Wall && y >= -Math.Sqrt(3) * x + 2 * Cell + 2 * Wall)
                    return true;
                if (x >= R + Math.Sqrt(3) / 2 * Wall && y >= Wall / 2)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
        else
            return false;
    }

    // Возвращает данные о местонахождении луча (в материале или нет) в теле
    bool InVoidBody(Vector3 Position)
    {

        return false;
    }

    // Максимум массива
    double Maximum(double[] Array)
    {
        double max = Array[0];

        for (int i = 0; i < Array.Length; i++)
        {
            if (max < Array[i])
                max = Array[i];
        }

        return max;
    }

    // Перемещение луча
    void MoveRay(Vector3 Position, double Energy, int heir, double N, Vector3 direction)
    {
        if (Position.Z < LengthSourceLens)
        {
            Position += direction * ((LengthSourceLens - Position.Z) / direction.Z);
        }

        while (Position.Z >= LengthSourceLens && Position.Z < LengthSourceLens + thikness && Energy > 0)
        {
            if (InVoid(Position) == true)
            {
                N -= GetReduceN(Energy, direction, N);
                //Energy -= ReduceOfEnergy(Energy, direction);
                if (heir < AmountOfScattering)
                    ScaterringRay(Position, Energy, heir, N, direction);
                //Debug.Log(N);
            }
            Position += direction;


        }

        if (Position.Z >= LengthSourceLens + thikness && Position.Z < LengthSourceLens + thikness + LengthLensObject)
        {
            Position += direction * ((LengthSourceLens + LengthLensObject + thikness - Position.Z) / direction.Z) + direction;
        }

        while (Position.Z >= LengthSourceLens + thikness + LengthLensObject && Position.Z < LengthSourceLens + thikness + LengthLensObject + ObjectThikness && Energy > 0)
        {
            if (InVoidBody(Position) == true)
            {
                N -= GetReduceN(Energy, direction, N);
                //Energy -= ReduceOfEnergy(Energy, direction);
                if (heir < AmountOfScattering)
                    ScaterringRay(Position, Energy, heir, N, direction);
                //Debug.Log(N);
            }
            Position += direction;


        }

        if (Position.Z >= LengthSourceLens + thikness + LengthLensObject + ObjectThikness && Position.Z < LengthSourceLens + thikness + LengthLensObject + ObjectThikness + LengthObjectScreen)
        {
            Position += direction * ((LengthSourceLens + thikness + LengthLensObject + ObjectThikness + LengthObjectScreen - Position.Z) / direction.Z) + direction;
        }

        if (Position.Z >= LengthSourceLens + thikness + LengthLensObject + ObjectThikness + LengthObjectScreen)
            AddEnergyToArray(Position, Energy, N);

    }

    // Запуск рассеянного луча
    void ScaterringRay(Vector3 Position, double CurrentEnergy, int heir, double N, Vector3 direction)
    {
        for (double a = AngleStep; a <= MaxAngleScatering; a += AngleStep)
        {
            int n = ScatteringN(CurrentEnergy, a, N, direction);
            if (n > 0)
            {
                for (double f = 0; f <= Math.PI * 2; f += AngleRadStep)
                {
                    MoveRay(Position, CurrentEnergy, heir + 1, n, new Vector3(direction.X * Math.Cos(f) - Math.Sin(f) * (Math.Cos(a) * direction.Y - Math.Sin(a) * direction.Z), direction.X * Math.Sin(f) + Math.Cos(f) * (Math.Cos(a) * direction.Y - Math.Sin(a) * direction.Z), Math.Sin(a) * direction.Y + Math.Cos(a) * direction.Z));
                }
            }
        }
    }

    // Частиц рассеянного луча
    int ScatteringN(double CurrentEnergy, double angle, double N, Vector3 direction)
    {
        double k = (((Math.Sin(angle) - Math.Sin(angle - AngleStep) + AngleStep)) / 2 * GetReduceN(CurrentEnergy, direction, N) / 2 * (AngleRadStep / Math.PI * 2));
        return (int)(k);
    }

    // Уменьшение частиц основного луча
    int GetReduceN(double CurrentEnergy, Vector3 direction, double N)
    {
        return (int)(Math.Exp(GetReducingN(CurrentEnergy, ReducingMount) * direction.Length() * density));
    }

    // Расчёт уменьшения по таблице 
    double GetReducingN(double x, Vector2[] Table)
    {
        double Reducing = 0;

        if (x < Table[0].X)
            return x;
        else
        {
            if (x > Table[Table.Length - 1].X)
            {
                int i = Table.Length - 1;
                Reducing = Table[i].Y - (Table[i].X - x) * (Table[i].Y - Table[i - 1].Y) / (Table[i].X - Table[i - 1].X);
                return 0;
            }
            else
            {
                int i = 1;

                while (x > Table[i].X && i < Table.Length)
                {
                    i++;
                }

                Reducing = Table[i].Y - (Table[i].X - x) * (Table[i].Y - Table[i - 1].Y) / (Table[i].X - Table[i - 1].X);

                return Reducing;
            }
        }
    }

    // Добавление энергии в ячейку экрана
    void AddEnergyToArray(Vector3 Position, double Energy, double N)
    {
        int SX, SY = 0;

        if (Math.Abs(Position.X) < Width / 2 && Math.Abs(Position.X) < Height / 2)
        {
            if (Position.X <= 0)
                SX = (int)((-Width / 2 - Position.X) * (ScreenX / Width) + ScreenX / 2);
            else
                SX = (int)(Position.X / (Width) * ScreenX + ScreenX / 2);

            if (Position.Y <= 0)
                SY = (int)((-Height / 2 - Position.Y) * ScreenY / Height + ScreenY / 2);
            else
                SY = (int)(Position.Y / (Height) * ScreenY + ScreenY / 2);

            if (ArrayPointScreen[SX, SY] == null || ArrayPointScreen[SX, SY] == 0)
                ArrayPointScreen[SX, SY] = Energy * N;
            ArrayPointScreen[SX, SY] += Energy * N;
            //Console.WriteLine(ArrayPointScreen[SX, SY]);
        }

    }
}
