using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_8
{
    class Program
    {
        /// <summary>
        /// Генератор тестов
        /// </summary>
        /// <param name="GraphA">Граф A</param>
        /// <param name="GraphB">Граф B</param>
        /// <param name="size">Размер матриц</param>
        static void GeneratorTests(out int[,] GraphA, out int[,] GraphB, out int size)
        {
            Random a = new Random();
            // Задаем размер матриц
            size = a.Next(2, 11);

            // Генерируем граф A
            GraphA = new int[size, size];
            Console.WriteLine("Граф А:");
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    // Главная диагональ - нули
                    if (i == j)
                        GraphA[i, j] = 0;
                    // Выше главной диагонали - генерируем ребра
                    else if (i < j)
                        GraphA[i, j] = a.Next(2);
                    // Ниже главной диагонали - используем уже созданные ребра 
                    else GraphA[i, j] = GraphA[j, i];
                    Console.Write("{0} ", GraphA[i, j]);
                }
                Console.WriteLine();
            }

            // Генерируем изоморфный граф B
            Console.WriteLine("Граф B:");
            GraphB = new int[size, size];

            // Создаем случайное биективное соответствие между вершинами графов
            int[] peak = new int[size];
            for (int i = 0; i < size; i++)
                peak[i] = -1;
            for (int i = 0; i < size; i++)
                while (true)
                {
                    int place = a.Next(size);
                    if (peak[place] == -1)
                    {
                        peak[place] = i;
                        break;
                    }
                }

            // Составляем матрицу смежности для графа B в соответствии с матрицей графа A
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    GraphB[i, j] = GraphA[peak[i], peak[j]];
                    Console.Write("{0} ", GraphB[i, j]);
                }
                Console.WriteLine();
            }

        }

        /// <summary>
        /// Генерация перестановок
        /// </summary>
        /// <param name="peak">Массив для варианта соответствия вершин</param>
        /// <returns></returns>
        static bool NextPermutation(ref int[] peak)
        {
            int index = peak.Length - 1;
            while (true)
            {
                int n = index;
                index--;
                // Находим два идущих поряд числа по возрастанию, index - одно из чисел для перестановок
                if (peak[index] < peak[n])
                {
                    // Находим второе число для перестановки, число меньшее первого найденного
                    int m = peak.Length - 1;
                    while (peak[index] >= peak[m])
                        m--;
                    // Выполняем перестановку
                    int t = peak[index]; peak[index] = peak[m]; peak[m] = t;
                    // При необходимости, возвращаем числа после перестановки в исходный порядок
                    Array.Reverse(peak, n, peak.Length - n);
                    return true;
                }
                if (index == 0)
                {
                    Array.Reverse(peak);
                    return false;
                }
            }
        }


        static void Main(string[] args)
        {
            int[,] GraphA, GraphB; int n;                 // Матрицы смежности и кол-во вершин
            // Генерируем матрицы смежности
            GeneratorTests(out GraphA, out GraphB, out n);
            // Граф для проверки наличия биективного соответствия
            int[,] graph = new int[n, n];

            // Массив для варианта соответствия вершин 
            int[] a = new int[n];
            for (int i = 0; i < n; i++)
                a[i] = i;

            bool ok = false;
            // Генерируем перестановку и проверяем, совпадают ли матрицы смежности
            while (!ok)
            {
                ok = true;
                bool result = NextPermutation(ref a);   // Генерация новой перестановки вершин
                // Составляем матрицу смежности по новой последовательности вершин и сравниваем с матрицей графа A
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        graph[i, j] = GraphB[a[i], a[j]];
                        if (graph[i, j] != GraphA[i, j])
                        {
                            ok = false;
                            break;
                        }
                    }
                    if (!ok) break;
                }
            }

            // Результат
            Console.WriteLine("Биективное соответствие (вершина графа A =  вершина графа B):");
            for (int i = 0; i < n; i++)
                Console.WriteLine("{0} = {1} ", i + 1, a[i] + 1);           
            Console.ReadLine();
        }
    }
}
