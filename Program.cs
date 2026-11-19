using System;
using System.Globalization;
using System.Text;

class Program
{
    const int Capacity = 100;
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        double[] costs = new double[Capacity];
        int count = 0;
        while (true)
        {
            Console.Clear();
            Console.WriteLine("АВТОСЕРВІС — ЛР9. Вартість ремонтних робіт");
            Console.WriteLine("1. Показати значення\n2. Додати значення\n3. Видалити за номером\n4. Показати статистику\n5. Дослід: значення та посилання\n6. Про програму\n0. Вихід");
            int choice = ReadInt("Вибір: ", 0, 6);
            if (choice == 0) break;
            switch (choice)
            {
                case 1: PrintValues(costs, count); break;
                case 2:
                    if (count == Capacity) Console.WriteLine("Сховище заповнене.");
                    else { double value = ReadDouble("Вартість робіт, грн: ", 0, 1000000); AddValue(costs, ref count, value); Console.WriteLine($"Значень у Main після виклику: {count}."); }
                    break;
                case 3: RemoveAt(costs, ref count, ReadInt("Номер рядка: ", 1, 101)); break;
                case 4:
                    Statistics(costs, count, out double sum, out double average);
                    Console.WriteLine($"Кількість: {count}; сума: {sum:F2} грн; середнє: {average:F2} грн.");
                    if (FindMaximum(costs, count, out double max, out int number)) Console.WriteLine($"Найбільше: {max:F2} грн, номер {number}.");
                    else Console.WriteLine("Набір порожній, максимуму немає.");
                    break;
                case 5: DemonstratePassing(); break;
                case 6: Console.WriteLine("Бушко Михайло, КН-21. Тема №4 — Автосервіс. Лабораторна №9."); break;
            }
            Console.Write("\nEnter — продовжити..."); Console.ReadLine();
        }
    }
    static void AddValue(double[] values, ref int count, double value)
    {
        if (count >= values.Length) { Console.WriteLine("Сховище заповнене."); return; }
        values[count] = value; count++;
    }
    static void PrintValues(double[] values, int count)
    {
        if (count == 0) { Console.WriteLine("Значень поки немає."); return; }
        Console.WriteLine($"{"№",5} {"Вартість робіт, грн",22}");
        for (int i = 0; i < count; i++) Console.WriteLine($"{i + 1,5} {values[i],22:F2}");
        Console.WriteLine($"Усього: {count}.");
    }
    static bool FindMaximum(double[] values, int count, out double maximum, out int number)
    {
        maximum = 0; number = 0;
        if (count == 0) return false;
        maximum = values[0]; number = 1;
        for (int i = 1; i < count; i++) if (values[i] > maximum) { maximum = values[i]; number = i + 1; }
        return true;
    }
    static void Statistics(double[] values, int count, out double sum, out double average)
    {
        sum = 0;
        for (int i = 0; i < count; i++) sum += values[i];
        average = count == 0 ? 0 : sum / count;
    }
    static void RemoveAt(double[] values, ref int count, int index)
    {
        if (index < 1 || index > count) { Console.WriteLine("Неправильний номер; масив не змінено."); return; }
        for (int i = index - 1; i < count - 1; i++) values[i] = values[i + 1];
        count--; values[count] = 0;
        Console.WriteLine($"Видалено рядок {index}; залишилося {count}.");
    }
    static void TryChangeNumber(int value) { value += 100; Console.WriteLine($"У методі int = {value}"); }
    static void ChangeFirstElement(int[] values) { values[0] += 100; Console.WriteLine($"У методі масив[0] = {values[0]}"); }
    static void DemonstratePassing()
    {
        int number = 50; int[] numbers = { 50 };
        Console.WriteLine($"До: int = {number}; масив[0] = {numbers[0]}");
        TryChangeNumber(number); ChangeFirstElement(numbers);
        Console.WriteLine($"Після: int = {number}; масив[0] = {numbers[0]}");
        Console.WriteLine("int передано копією; елемент масиву змінено в тому самому об'єкті.");
    }
    static int ReadInt(string prompt, int min, int max)
    {
        while (true) { Console.Write(prompt); if (int.TryParse(Console.ReadLine(), out int v) && v >= min && v <= max) return v; Console.WriteLine($"Введіть ціле число від {min} до {max}."); }
    }
    static double ReadDouble(string prompt, double min, double max)
    {
        while (true)
        {
            Console.Write(prompt); string input = (Console.ReadLine() ?? "").Trim().Replace(',', '.');
            if (double.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out double v) && !double.IsNaN(v) && !double.IsInfinity(v) && v >= min && v <= max) return v;
            Console.WriteLine($"Введіть число від {min} до {max}.");
        }
    }
}
