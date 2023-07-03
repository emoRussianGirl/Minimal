using System.Diagnostics;

class Program
{
    static void Main(string[] args)
    {
        SeveroZapad severoZapad = new SeveroZapad();

        Console.WriteLine("Введте кол-во потребностей, затем их значения:");
        int[] needs = Input(); //потребности
        Debug.WriteLine($"Кол-во потребностей: {needs.Length}");

        Console.WriteLine("Введте кол-во запасов, затем их значения:");
        int[] reserves = Input(); //запасы
        Debug.WriteLine($"Кол-во запасов: {reserves.Length}");

        Console.WriteLine("Введите матрицу A:");
        int[,] A = InputMatrix(needs.Length, reserves.Length);

        Console.WriteLine("Введите матрицу B:");
        int[,] B = InputMatrix(needs.Length, reserves.Length);

        severoZapad.PrintResult(A, B, needs, reserves);
    }

    //Ввод потребностей или запасов
    static int[] Input()
    {
        int length = Convert.ToInt32(Console.ReadLine());
        int[] array = new int[length];

        for (int i = 0; i < length; i++)
        {
            Console.WriteLine($"Введите {i + 1}: ");
            array[i] = Convert.ToInt32(Console.ReadLine());
        }

        Console.Write("N{ ");

        for (int i = 0; i < length; i++)
        {
            Console.Write("{0} ", array[i]);

        }

        Console.WriteLine("}");

        return array;
    }

    //Ввод матриц
    static int[,] InputMatrix(int lengthN, int lengthM)
    {
        int[,] array = new int[lengthN, lengthM];

        for (int i = 0; i < lengthN; i++)
        {
            for (int j = 0; j < lengthM; j++)
            {
                Console.Write($"a[{i + 1},{j + 1}] = ");

                array[i, j] = Convert.ToInt32(Console.ReadLine());
            }
        }

        return array;
    }
}
public class SeveroZapad
{
    //Решение с выводом
    public void PrintResult(int[,] A, int[,] B, int[] N, int[] M)
    {
        if (!CheckNM(N, M))
        {
            throw new Exception("суммы векторов N, M не равны");
        }

        int[,] C = GetC(A, B);
        Console.WriteLine("матрица цен:");
        PrintMatrix(C);

        //Распределение потребностей и запасов
        int[,] distribution = GetDistributionMatrix(N, M);
        Console.WriteLine("матрица распределения:");
        PrintMatrix(distribution);

        //Умножение распределенных потребностей и запасов на C
        int[,] result = MatricesMultiply(C, distribution);

        //Затраты
        int f = CalculateF(result);
        Console.WriteLine($"Целевая функция: {f}");
    }

    //Рассчитывает затраты денег 
    public int Calculate(int[,] A, int[,] B, int[] N, int[] M)
    {
        if (!CheckNM(N, M))
        {
            throw new Exception("суммы векторов N, M не равны");
        }

        int[,] C = GetC(A, B);

        //Распределение потребностей и запасов
        int[,] distribution = GetDistributionMatrix(N, M);

        //Умножение распределенных потребностей и запасов на C
        int[,] result = MatricesMultiply(C, distribution);

        return CalculateF(result);
    }

    //Сумма A и B, для получения C
    private int[,] GetC(int[,] A, int[,] B)
    {
        if (A.GetLength(0) != B.GetLength(0) || A.GetLength(1) != B.GetLength(1))
        {
            throw new Exception("матрицы A и B должны быть одинаковыми");
        }

        int[,] C = new int[A.GetLength(0), A.GetLength(1)];

        for (int i = 0; i < A.GetLength(0); i++)
        {
            for (int j = 0; j < A.GetLength(1); j++)
            {
                C[i, j] = A[i, j] + B[i, j];
            }
        }

        return C;
    }

    //Сравнение сумм N и M
    public bool CheckNM(int[] N, int[] M)
    {
        return N.Sum() == M.Sum();
    }

    //Распределение
    private int[,] GetDistributionMatrix(int[] N, int[] M)
    {
        int[,] matrix = new int[N.Length, M.Length];

        int i = 0,
            j = 0;

        while (i < N.Length && j < M.Length)
        {
            //Минимальный элемент
            matrix[i, j] = N[i] > M[j] ? M[j] : N[i];

            N[i] -= matrix[i, j];
            M[j] -= matrix[i, j];

            if (N[i] == 0)
            {
                i++;
            }

            if (M[j] == 0)
            {
                j++;
            }
        }

        return matrix;
    }

    //Умножение матриц. Нужно для получения результата после распределения
    //потребностей и ресурсов по C.
    private int[,] MatricesMultiply(int[,] C, int[,] ditribution)
    {
        int[,] result = new int[C.GetLength(0), C.GetLength(1)];

        for (int i = 0; i < C.GetLength(0); i++)
        {
            for (int j = 0; j < C.GetLength(1); j++)
            {

                result[i, j] = C[i, j] * ditribution[i, j];
            }
        }

        return result;
    }

    //Рассчитываем F сложением всех элементов в итоговой матрице
    private int CalculateF(int[,] resultMatrix)
    {
        int sum = 0;

        for (int i = 0; i < resultMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < resultMatrix.GetLength(1); j++)
            {
                sum += resultMatrix[i, j];
            }
        }

        return sum;
    }

    private void PrintMatrix(int[,] matrix)
    {
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                Console.Write($"{matrix[i, j]}\t");
            }

            Console.WriteLine();
        }
    }
}
