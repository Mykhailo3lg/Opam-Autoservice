using System;
using System.Globalization;
using System.Text;

class Program
{
    const int Capacity = 100;
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        int[] ids = new int[Capacity];
        string[] brands = new string[Capacity];
        string[] plates = new string[Capacity];
        int[] workCodes = new int[Capacity];
        DateTime[] accepted = new DateTime[Capacity];
        double[] workCosts = new double[Capacity];
        double[] partCosts = new double[Capacity];
        string[] statuses = new string[Capacity];
        int count = 0, nextId = 1;
        while (true)
        {
            Console.Clear();
            Console.WriteLine("АВТОСЕРВІС — ЛР10. Замовлення на ремонт");
            Console.WriteLine("1. Показати всі замовлення\n2. Додати замовлення\n3. Додати 6 тестових замовлень\n4. Підсумки за видами робіт за місяць\n5. Про програму\n0. Вихід");
            int choice = ReadInt("Вибір: ", 0, 5);
            if (choice == 0) break;
            switch (choice)
            {
                case 1: PrintOrders(ids, brands, plates, workCodes, accepted, workCosts, partCosts, statuses, count); break;
                case 2: AddOrder(ids, brands, plates, workCodes, accepted, workCosts, partCosts, statuses, ref count, ref nextId); break;
                case 3: SeedOrders(ids, brands, plates, workCodes, accepted, workCosts, partCosts, statuses, ref count, ref nextId); break;
                case 4:
                    if (count == 0) { Console.WriteLine("Замовлень поки немає."); break; }
                    int year = ReadInt("Рік: ", 2000, 2100);
                    int month = ReadInt("Місяць (1–12): ", 1, 12);
                    PrintMonthlyTotals(workCodes, accepted, workCosts, partCosts, count, year, month);
                    break;
                case 5: Console.WriteLine("Бушко Михайло, КН-21. Тема №4 — Автосервіс. Лабораторна №10."); break;
            }
            Console.Write("\nEnter — продовжити..."); Console.ReadLine();
        }
    }
    static void AddOrder(int[] ids, string[] brands, string[] plates, int[] codes, DateTime[] dates, double[] works, double[] parts, string[] statuses, ref int count, ref int nextId)
    {
        if (count >= ids.Length) { Console.WriteLine("Сховище заповнене (100 замовлень)."); return; }
        string brand = ReadText("Марка автомобіля: ");
        string plate = ReadText("Державний номер: ");
        int code = ReadInt("Код виду робіт (1–5): ", 1, 5);
        DateTime date = ReadDate("Дата прийому (дд.мм.рррр): ");
        double work = ReadDouble("Вартість робіт, грн: ", 0, 1000000);
        double part = ReadDouble("Вартість запчастин, грн: ", 0, 1000000);
        string status = ReadText("Статус: ");
        Store(ids, brands, plates, codes, dates, works, parts, statuses, ref count, ref nextId, brand, plate, code, date, work, part, status);
        Console.WriteLine($"Додано замовлення №{ids[count - 1]}.");
    }
    static void Store(int[] ids, string[] brands, string[] plates, int[] codes, DateTime[] dates, double[] works, double[] parts, string[] statuses, ref int count, ref int nextId, string brand, string plate, int code, DateTime date, double work, double part, string status)
    {
        if (count >= ids.Length) return;
        ids[count] = nextId; brands[count] = brand; plates[count] = plate; codes[count] = code;
        dates[count] = date; works[count] = work; parts[count] = part; statuses[count] = status;
        count++; nextId++;
    }
    static void SeedOrders(int[] ids, string[] brands, string[] plates, int[] codes, DateTime[] dates, double[] works, double[] parts, string[] statuses, ref int count, ref int nextId)
    {
        if (ids.Length - count < 6) { Console.WriteLine("Недостатньо місця для шести записів."); return; }
        DateTime today = DateTime.Today;
        Store(ids, brands, plates, codes, dates, works, parts, statuses, ref count, ref nextId, "Toyota", "AO1234AA", 1, today, 1800, 950, "Прийнято");
        Store(ids, brands, plates, codes, dates, works, parts, statuses, ref count, ref nextId, "Skoda", "AO5678BB", 2, today, 2500, 4200, "В роботі");
        Store(ids, brands, plates, codes, dates, works, parts, statuses, ref count, ref nextId, "Ford", "AO9012CC", 1, today, 1200, 800, "Готово");
        Store(ids, brands, plates, codes, dates, works, parts, statuses, ref count, ref nextId, "Renault", "AO3456DD", 3, today, 3200, 1700, "Прийнято");
        Store(ids, brands, plates, codes, dates, works, parts, statuses, ref count, ref nextId, "Honda", "AO7890EE", 2, today.AddDays(-2), 1600, 3000, "В роботі");
        Store(ids, brands, plates, codes, dates, works, parts, statuses, ref count, ref nextId, "Volkswagen", "AO2468FF", 4, today.AddDays(-1), 900, 200, "Готово");
        Console.WriteLine("Додано 6 тестових замовлень.");
    }
    static void PrintOrders(int[] ids, string[] brands, string[] plates, int[] codes, DateTime[] dates, double[] works, double[] parts, string[] statuses, int count)
    {
        if (count == 0) { Console.WriteLine("Замовлень поки немає."); return; }
        Console.WriteLine($"{"№",4} {"Марка",-15} {"Держномер",-12} {"Код",4} {"Дата",-12} {"Роботи",10} {"Запчастини",11} {"Статус",-15}");
        for (int i = 0; i < count; i++) Console.WriteLine($"{ids[i],4} {brands[i],-15} {plates[i],-12} {codes[i],4} {dates[i]:dd.MM.yyyy}   {works[i],10:F2} {parts[i],11:F2} {statuses[i],-15}");
        Console.WriteLine($"Замовлень: {count}.");
        DateTime current = DateTime.Today;
        Console.WriteLine($"Підсумки за {current:MM.yyyy}:");
        PrintMonthlyTotals(codes, dates, works, parts, count, current.Year, current.Month);
    }
    static double TotalForWorkType(int[] codes, DateTime[] dates, double[] works, double[] parts, int count, int year, int month, int code)
    {
        double total = 0;
        for (int i = 0; i < count; i++) if (codes[i] == code && dates[i].Year == year && dates[i].Month == month) total += works[i] + parts[i];
        return total;
    }
    static void PrintMonthlyTotals(int[] codes, DateTime[] dates, double[] works, double[] parts, int count, int year, int month)
    {
        double grand = 0;
        for (int code = 1; code <= 5; code++) { double total = TotalForWorkType(codes, dates, works, parts, count, year, month, code); Console.WriteLine($"Вид робіт {code}: {total:F2} грн"); grand += total; }
        Console.WriteLine($"Разом: {grand:F2} грн.");
    }
    static string ReadText(string prompt)
    {
        while (true) { Console.Write(prompt); string text = (Console.ReadLine() ?? "").Trim(); if (text.Length > 0 && text.Length <= 30) return text; Console.WriteLine("Введіть непорожній текст до 30 символів."); }
    }
    static DateTime ReadDate(string prompt)
    {
        while (true) { Console.Write(prompt); if (DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date) && date.Year >= 2000 && date.Date <= DateTime.Today) return date; Console.WriteLine("Введіть коректну дату не пізніше сьогоднішньої у форматі дд.мм.рррр."); }
    }
    static int ReadInt(string prompt, int min, int max)
    {
        while (true) { Console.Write(prompt); if (int.TryParse(Console.ReadLine(), out int v) && v >= min && v <= max) return v; Console.WriteLine($"Введіть ціле число від {min} до {max}."); }
    }
    static double ReadDouble(string prompt, double min, double max)
    {
        while (true) { Console.Write(prompt); string input = (Console.ReadLine() ?? "").Trim().Replace(',', '.'); if (double.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out double v) && !double.IsNaN(v) && !double.IsInfinity(v) && v >= min && v <= max) return v; Console.WriteLine($"Введіть число від {min} до {max}."); }
    }
}
