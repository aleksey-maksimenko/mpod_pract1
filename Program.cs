using System;

namespace Pract1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const int size = 10_000_000;
            const int threads = 4;

            Console.WriteLine("Генерация данных...");
            decimal[] data = new decimal[size];
            Random rnd = new Random(42);
            for (int i = 0; i < size; i++)
            {
                double v = 1.0 + rnd.NextDouble() * 999.0;
                data[i] = (decimal)v;
            }

            // создаем обработчик данных
            DataProcessor proc = new DataProcessor();
            Console.WriteLine("Запуск последовательной обработки...");
            long seqTime;
            decimal[] seqRes;
            // измеряем время последовательной обработки
            (seqTime, seqRes) = PerformanceMeter.MeasureExecutionTime(
                delegate { return proc.ProcessDataSequential(data); },
                "Последовательная обработка"
            );
            Console.WriteLine("Запуск параллельной обработки...");
            long parTime;
            decimal[] parRes;
            // измеряем время параллельной обработки
            (parTime, parRes) = PerformanceMeter.MeasureExecutionTime(
                delegate { return proc.ProcessDataParallel(data, threads); },
                "Параллельная обработка"
            );
            // сравниваем результаты
            bool same = PerformanceMeter.CompareResults(seqRes, parRes);
            double speedup = (double)seqTime / parTime; // считаем ускорение

            Console.WriteLine();
            Console.WriteLine("=== Результаты обработки ===");
            Console.WriteLine($"Размер данных: {size} элементов");
            Console.WriteLine($"Последовательная обработка: {seqTime} мс");
            Console.WriteLine($"Параллельная обработка (4 потока): {parTime} мс");
            Console.WriteLine($"Ускорение: {speedup:F2}x");
            Console.WriteLine($"Результаты совпадают: {(same ? "Да" : "Нет")}");
            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}
