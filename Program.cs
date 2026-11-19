using System;
using System.Globalization;
using System.Text;

namespace AvtoService;

internal class Program
{
    private const int MaxRows = 10;
    private const int MaxColumns = 12;
    private const double HourlyRate = 650;
    private const double PartsCost = 900;

    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;
        bool running = true;
        while (running)
        {
            ShowMenu();
            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1": ShowAbout(); Pause(); break;
                case "2": ShowCostTable(); Pause(); break;
                case "3": ShowMonthlyTable(); Pause(); break;
                case "0": running = false; Console.WriteLine("До побачення!"); break;
                default: Console.WriteLine("Немає такого пункту."); Pause(); break;
            }
        }
    }

    static void ShowMenu()
    {
        Console.WriteLine("=== Автосервіс: облік замовлень на ремонт ===");
        Console.WriteLine("1 — довідка про проєкт");
        Console.WriteLine("2 — таблиця вартості ремонту");
        Console.WriteLine("3 — таблиця сум за видами робіт і місяцями");
        Console.WriteLine("0 — вихід");
        Console.Write("Ваш вибір: ");
    }

    static void Pause()
    {
        Console.WriteLine("\nНатисніть будь-яку клавішу...");
        Console.ReadKey(true);
        Console.Clear();
    }

    static void ShowAbout()
    {
        Console.WriteLine("Тема №4: Автосервіс.");
        Console.WriteLine("Сутності: Користувач; Замовлення на ремонт; Вид робіт (довідник).");
        Console.WriteLine("Поля замовлення:");
        Console.WriteLine("int — номер замовлення");
        Console.WriteLine("string — марка авто");
        Console.WriteLine("string — держномер");
        Console.WriteLine("int — код виду робіт");
        Console.WriteLine("DateTime — дата прийому");
        Console.WriteLine("double — вартість робіт");
        Console.WriteLine("double — вартість запчастин");
        Console.WriteLine("string — статус");
        Console.WriteLine("Підсумок: сума за кожним видом робіт за місяць.");
    }

    // Чистий розрахунок: метод не читає і не друкує дані.
    static double CalculateCost(double hours, double hourlyRate, double parts)
    {
        return hours * hourlyRate + parts;
    }

    static void ShowCostTable()
    {
        double from = ReadDouble("Початкова кількість годин (0,5–8): ", 0.5, 8);
        double to = ReadDouble("Кінцева кількість годин: ", from, 12);
        double step = ReadDouble("Крок (0,1–2): ", 0.1, 2);
        TabulateCost(from, to, step);
    }

    // Табулювання приймає рівно три параметри: початок, кінець і крок.
    static void TabulateCost(double from, double to, double step)
    {
        Console.WriteLine($"{"Години",10}{"Роботи, грн",16}{"Разом, грн",16}");
        for (double hours = from; hours <= to + 1e-9; hours += step)
        {
            double labor = CalculateCost(hours, HourlyRate, 0);
            double total = CalculateCost(hours, HourlyRate, PartsCost);
            Console.WriteLine($"{hours,10:F2}{labor,16:F2}{total,16:F2}");
        }
    }

    static void ShowMonthlyTable()
    {
        int rows = ReadInt($"Кількість видів робіт (1–{MaxRows}): ", 1, MaxRows);
        int columns = ReadInt($"Кількість місяців (1–{MaxColumns}): ", 1, MaxColumns);
        PrintMonthlyTable(rows, columns);
    }

    // Значення умовні: це приклад двовимірної таблиці до появи реальних записів у ЛР10.
    static double DemoAmount(int workType, int month)
    {
        return workType * month * 120.0;
    }

    static void PrintMonthlyTable(int rows, int columns)
    {
        double[,] amounts = new double[MaxRows, MaxColumns];
        double[] columnTotals = new double[MaxColumns];
        double grandTotal = 0;
        Console.Write($"{"Вид робіт",14}");
        for (int col = 0; col < columns; col++) Console.Write($"{col + 1,12}");
        Console.WriteLine($"{"Разом",14}");
        for (int row = 0; row < rows; row++)
        {
            double rowTotal = 0;
            Console.Write($"{row + 1,14}");
            for (int col = 0; col < columns; col++)
            {
                amounts[row, col] = DemoAmount(row + 1, col + 1);
                rowTotal += amounts[row, col];
                columnTotals[col] += amounts[row, col];
                Console.Write($"{amounts[row, col],12:F2}");
            }
            grandTotal += rowTotal;
            Console.WriteLine($"{rowTotal,14:F2}");
        }
        Console.Write($"{"Усього",14}");
        for (int col = 0; col < columns; col++) Console.Write($"{columnTotals[col],12:F2}");
        Console.WriteLine($"{grandTotal,14:F2}");
    }

    static double ReadDouble(string prompt, double min, double max)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            if (!double.TryParse(input, NumberStyles.Float, CultureInfo.CurrentCulture, out double value)
                && !double.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out value))
            { Console.WriteLine("Помилка: введіть число."); continue; }
            if (double.IsNaN(value) || double.IsInfinity(value) || value < min || value > max)
            { Console.WriteLine($"Помилка: число має бути від {min} до {max}."); continue; }
            return value;
        }
    }

    static int ReadInt(string prompt, int min, int max)
    {
        while (true)
        {
            Console.Write(prompt);
            if (!int.TryParse(Console.ReadLine(), out int value))
            { Console.WriteLine("Помилка: введіть ціле число."); continue; }
            if (value < min || value > max)
            { Console.WriteLine($"Помилка: число має бути від {min} до {max}."); continue; }
            return value;
        }
    }
}
