using System.Diagnostics;

namespace MinimalJopeMethod
{
    internal class Program
    {
        private static readonly MinimalMethod _minimalMethod = new MinimalMethod();

        static void Main()
        {
            Console.WriteLine("Нахождение опорного плана методом минимального элемента.\n");

            do
            {
                Console.WriteLine("Введите кол-во строк (поставщики) матрицы стоимостей:");
                if (!int.TryParse(Console.ReadLine(), out int yLength))
                {
                    Console.WriteLine("Число введено неправильно");
                    continue;
                }

                Console.WriteLine("Введите кол-во столбцов (магазины) матрицы стоимостей:");
                if (!int.TryParse(Console.ReadLine(), out int xLength))
                {
                    Console.WriteLine("Число введено неправильно");
                    continue;
                }

                double[,] costMatrix = new double[yLength, xLength];

                double[] N = new double[yLength];
                double[] M = new double[xLength];

                bool errorFlag = false;

                for (int i = 0; i < N.Length; i++)
                {
                    Console.WriteLine($"Введите N{i + 1}:");
                    if (!double.TryParse(Console.ReadLine(), out N[i]))
                    {
                        Console.WriteLine("Число введено неправильно");
                        errorFlag = true;
                        break;
                    }
                }

                if (errorFlag)
                    continue;

                for (int i = 0; i < M.Length; i++)
                {
                    Console.WriteLine($"Введите M{i + 1}:");
                    if (!double.TryParse(Console.ReadLine(), out M[i]))
                    {
                        Console.WriteLine("Число введено неправильно");
                        errorFlag = true;
                        break;
                    }
                }

                if (errorFlag)
                    continue;

                for (int y = 0; y < yLength; y++)
                {
                    for (int x = 0; x < xLength; x++)
                    {
                        Console.WriteLine($"Введите стоимость[{y}, {x}]:");
                        if (!double.TryParse(Console.ReadLine(), out costMatrix[y, x]))
                        {
                            Console.WriteLine("Число введено неправильно");
                            errorFlag = true;
                            break;
                        }
                    }
                }

                if (errorFlag)
                    continue;

                var result = _minimalMethod.FindBasePlan(costMatrix, N, M);
                double f = 0;

                Console.WriteLine("Результат:");

                Console.Write("  \t");
                for (int i = 0; i < M.Length; i++)
                {
                    Console.Write($"{M[i]}\t");
                }

                Console.WriteLine();

                for (int y = 0; y < result.GetLength(0); y++)
                {
                    Console.Write($"{N[y]}\t");

                    for (int x = 0; x < result.GetLength(1); x++)
                    {
                        var element = result[y, x];

                        if (element.Value > 0)
                        {
                            f += element.Cost * element.Value;
                        }

                        Console.Write($"{element.Cost}{(element.Value > 0 ? $"({element.Value})" : "")}\t");
                    }

                    Console.WriteLine();
                }

                Console.WriteLine($"F = {f} у.д.е.\n");

                Console.WriteLine("Для выхода нажмите Escape. Для продолжения любую другую клавишу.\n");
            } while (Console.ReadKey().Key != ConsoleKey.Escape);

            Console.WriteLine("Завершение..");
        }
    }

    public class MinimalMethod
    {
        public PlanElement[,] FindBasePlan(double[,] costMatrix, double[] N, double[] M)
        {
            if (N.Length != M.Length && N.Length != costMatrix.GetLength(0) && costMatrix.GetLength(0) <= 2 && costMatrix.GetLength(1) <= 2)
            {
                throw new ArgumentException("Невалидные данные задачи.");
            }

            Debug.WriteLine($"Размер матрицы: {costMatrix.GetLength(0)}x{costMatrix.GetLength(1)}");

            double[] n = new double[N.Length];
            double[] m = new double[M.Length];

            N.CopyTo(n, 0);
            M.CopyTo(m, 0);

            int iterationCount = 1;

            List<double> excludedYs = new List<double>(costMatrix.GetLength(0));
            List<double> excludedXs = new List<double>(costMatrix.GetLength(1));

            PlanElement[,] resultPlan = new PlanElement[costMatrix.GetLength(0), costMatrix.GetLength(1)];

            while (n.Sum() != 0 && m.Sum() != 0)
            {
                if (iterationCount > 1000)
                {
                    throw new ArgumentException("Произведено более 1000 итераций. Не удалось найти опорный план.");
                }

                FindMinCost(costMatrix, excludedYs, excludedXs, out int y, out int x);

                double value;

                if (n[y] < m[x])
                {
                    value = n[y];
                    excludedYs.Add(y);
                    n[y] -= n[y];
                    m[x] -= value;
                }
                else
                {
                    value = m[x];
                    excludedXs.Add(x);
                    m[x] -= m[x];
                    n[y] -= value;
                }

                resultPlan[y, x] = new PlanElement(costMatrix[y, x], value);

                iterationCount++;
            }

            for (int y = 0; y < costMatrix.GetLength(0); y++)
            {
                for (int x = 0; x < costMatrix.GetLength(1); x++)
                {
                    if (resultPlan[y, x] == null)
                    {
                        resultPlan[y, x] = new PlanElement(costMatrix[y, x], 0);
                    }
                }
            }

            return resultPlan;
        }

        private void FindMinCost(double[,] costMatrix, List<double> excludedYs, List<double> excludedXs, out int yPos, out int xPos)
        {
            double minCost = double.MaxValue;

            yPos = 0;
            xPos = 0;

            for (int y = 0; y < costMatrix.GetLength(0); y++)
            {
                for (int x = 0; x < costMatrix.GetLength(1); x++)
                {
                    if (!excludedYs.Contains(y) && !excludedXs.Contains(x) && costMatrix[y, x] < minCost)
                    {
                        minCost = costMatrix[y, x];

                        yPos = y;
                        xPos = x;
                    }
                }
            }
        }
    }

    public class PlanElement
    {
        public double Cost { get; set; }
        public double Value { get; set; }

        public PlanElement(double cost, double value)
        {
            Cost = cost;
            Value = value;
        }
    }
}
