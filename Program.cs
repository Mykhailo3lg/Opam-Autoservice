using System;
using System.Globalization;
using System.Text;

namespace AvtoService;

internal class Program
{
    private const int MaxRows = 10;
    private const int MaxColumns = 12;

    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;
        bool running = true;
        while (running)
        {
            Console.WriteLine("=== Автосервіс: облік замовлень на ремонт ===");
            Console.WriteLine("1 — довідка про проєкт");
            Console.WriteLine("2 — таблиця вартості ремонту");
            Console.WriteLine("3 — таблиця сум за видами робіт і місяцями");
            Console.WriteLine("0 — вихід");
            Console.Write("Ваш вибір: ");
            string? choice = Console.ReadLine();
            Console.WriteLine();
            switch (choice)
            {
                case "1":
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
                    break;
                case "2":
                    Console.Write("Початкова кількість годин (0,5–8): ");
                    if (!double.TryParse(Console.ReadLine(), CultureInfo.CurrentCulture, out double from) || from < 0.5 || from > 8)
                    { Console.WriteLine("Помилка: некоректний початок."); break; }
                    Console.Write("Кінцева кількість годин (від початку до 12): ");
                    if (!double.TryParse(Console.ReadLine(), CultureInfo.CurrentCulture, out double to) || to < from || to > 12)
                    { Console.WriteLine("Помилка: некоректна кінцева межа."); break; }
                    Console.Write("Крок (0,1–2): ");
                    if (!double.TryParse(Console.ReadLine(), CultureInfo.CurrentCulture, out double step) || step <= 0 || step > 2)
                    { Console.WriteLine("Помилка: крок має бути додатним числом до 2."); break; }
                    Console.WriteLine($"{"Години",10}{"Роботи, грн",16}{"Разом, грн",16}");
                    // Вартість робіт: 650 грн за годину; запчастини: 900 грн.
                    for (double hours = from; hours <= to + 1e-9; hours += step)
                    {
                        double labor = hours * 650;
                        double total = labor + 900;
                        Console.WriteLine($"{hours,10:F2}{labor,16:F2}{total,16:F2}");
                    }
                    break;
                case "3":
                    Console.Write($"Кількість видів робіт (1–{MaxRows}): ");
                    if (!int.TryParse(Console.ReadLine(), out int rows) || rows < 1 || rows > MaxRows)
                    { Console.WriteLine("Помилка: кількість рядків має бути додатною."); break; }
                    Console.Write($"Кількість місяців (1–{MaxColumns}): ");
                    if (!int.TryParse(Console.ReadLine(), out int columns) || columns < 1 || columns > MaxColumns)
                    { Console.WriteLine("Помилка: кількість стовпців має бути додатною."); break; }
                    // Демонстраційна формула, а не справжні замовлення.
                    double[,] amounts = new double[MaxRows, MaxColumns];
                    double[] totals = new double[MaxColumns];
                    double grandTotal = 0;
                    Console.Write($"{"Вид робіт",14}");
                    for (int month = 0; month < columns; month++) Console.Write($"{month + 1,12}");
                    Console.WriteLine($"{"Разом",14}");
                    for (int row = 0; row < rows; row++)
                    {
                        double rowTotal = 0;
                        Console.Write($"{row + 1,14}");
                        for (int col = 0; col < columns; col++)
                        {
                            amounts[row, col] = (row + 1) * (col + 1) * 120.0;
                            rowTotal += amounts[row, col];
                            totals[col] += amounts[row, col];
                            Console.Write($"{amounts[row, col],12:F2}");
                        }
                        grandTotal += rowTotal;
                        Console.WriteLine($"{rowTotal,14:F2}");
                    }
                    Console.Write($"{"Усього",14}");
                    for (int col = 0; col < columns; col++) Console.Write($"{totals[col],12:F2}");
                    Console.WriteLine($"{grandTotal,14:F2}");
                    break;
                case "0": running = false; Console.WriteLine("До побачення!"); break;
                default: Console.WriteLine("Немає такого пункту. Спробуйте ще раз."); break;
            }
            if (running)
            {
                Console.WriteLine("\nНатисніть будь-яку клавішу...");
                Console.ReadKey(true);
                Console.Clear();
            }
        }
    }
}
