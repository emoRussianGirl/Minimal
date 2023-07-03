using System.Diagnostics;
using System.Text;
 
namespace MathModelingForExam
{
    internal class Program
    {
        static void Main()
        {
            TaskJonhson.StartTask("jskflj");
        }
    }

    public class TaskJonhson
    {
        public static void StartTask(string path)
        {
            int[,] enterSt = InitStank(path);
            GetAnswer(enterSt, path);
            int[,] answerArray = Redistribution(enterSt);
            answerArray = SortArray(answerArray);
            GetAnswer(answerArray, path);
        }
        
        private static int[,] InitStank(string path)
        {
            var lines = File.ReadAllLines(path);
            var nums = new int[lines.Length, 2];

            for (int i = 0; i < lines.Length; i++)
            {
                var lineNums = lines[i].Split(' ');

                int num1 = int.Parse(lineNums[0]);
                int num2 = int.Parse(lineNums[1]);

                nums[i, 0] = num1;
                nums[i, 1] = num2;
            }

            Debug.WriteLine($"Количество считанных строк в столбце: {nums.GetLength(0)}");
            return nums;
        }
        private static void GetAnswer(int[,] allSt, string path) //поиск ответа
        {
            int sum = allSt[0, 0];
            List<int> allResults = new List<int>();
            allResults.Add(sum); //первое значение добавляем по-умолчанию
            for (int i = 1; i < allSt.GetLength(0); i++)
            {
                sum = sum + allSt[i, 0] - allSt[i - 1, 1];
                allResults.Add(sum);
            }
            
            StringBuilder sb = new();
            sb.AppendLine();

            foreach (int i in allResults)
            {
                sb.Append($"{i} ");
            }
            int answer = allResults.Max();
            sb.AppendLine($"Ответ: {answer}");

            File.AppendAllText(path, sb.ToString());
        }
        private static int[,] Redistribution(int[,] allSt) //алгоритм перераспределения
        {
            int[,] newSt = new int[allSt.GetLength(0), allSt.GetLength(1)];
            List<int> infAboutMin = new List<int>();
            List<int> firstColumn = new List<int>();
            List<int> secondColumn = new List<int>();
            int indStr = 0;
            int indSt = 0;
            for (int i = 0; i < allSt.GetLength(0); i++) //само распределение (два листа)
            {
                infAboutMin = FindMinElInArr(allSt); //лист с общей информацией о мин. эл-те
                indStr = infAboutMin[1];
                indSt = infAboutMin[2];
                if (indSt == 0)
                {
                    firstColumn.Add(allSt[indStr, indSt]);
                    firstColumn.Add(allSt[indStr, indSt + 1]);
                    allSt[indStr, indSt] = 0;
                    allSt[indStr, indSt + 1] = 0;
                }
                else if (indSt == 1)
                {
                    secondColumn.Add(allSt[indStr, indSt - 1]);
                    secondColumn.Add(allSt[indStr, indSt]);
                    allSt[indStr, indSt - 1] = 0;
                    allSt[indStr, indSt] = 0;
                }
            }
            //добавление в первый лист элементов из второго (переворачиваем)
            for (int i = secondColumn.Count - 1; i > 0; i -= 2) //цикл идет с конца
            {
                firstColumn.Add(secondColumn[i - 1]);
                firstColumn.Add(secondColumn[i]);
            }
            //в массив двумерный переделываем
            int k = 1;
            for (int i = 0; i < newSt.GetLength(0); i++)
            {
                newSt[i, 0] = firstColumn[k - 1];
                newSt[i, 1] = firstColumn[k];
                k += 2;
            }
            return newSt;
        }
        private static int[,] SortArray(int[,] allSt) //сортируем на случай если минимальные элементы одинаковые
        {
            int temp;
            for (int i = 1; i < allSt.GetLength(0); i++)
            {
                if (allSt[i - 1, 0] == allSt[i, 0])
                {
                    if (allSt[i - 1, 1] < allSt[i, 1])
                    {
                        temp = allSt[i - 1, 1];
                        allSt[i - 1, 1] = allSt[i, 1];
                        allSt[i, 1] = temp;
                    }
                }
                if (allSt[i - 1, 1] == allSt[i, 1])
                {
                    if (allSt[i - 1, 0] > allSt[i, 0])
                    {
                        temp = allSt[i - 1, 0];
                        allSt[i - 1, 0] = allSt[i, 0];
                        allSt[i, 0] = temp;
                    }
                }
            }
            return allSt;
        }
        private static List<int> FindMinElInArr(int[,] arr) //найти минимальный элемент и его индексы
        {
            int min = int.MaxValue;
            int indStr = 0;
            int indSt = 0;
            List<int> infAboutMin = new List<int>();
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    if (arr[i, j] != 0 && min > arr[i, j])
                    {
                        min = arr[i, j];
                        indStr = i;
                        indSt = j;
                    }
                }
            }
            infAboutMin.Add(min);
            infAboutMin.Add(indStr);
            infAboutMin.Add(indSt);
            return infAboutMin;
        }
    }
}
