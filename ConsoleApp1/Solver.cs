using System;
using System.Diagnostics;

namespace ConsoleApp1
{
    public class Solver
    {
        public Solver Solver1
        {
            get => default;
            set
            {
            }
        }
        // Метод для проверки диагонального преобладания и попытки привести матрицу к этому виду
        // Метод для проверки диагонального преобладания и попытки привести матрицу к этому виду
        private static bool EnsureDiagonalDominance(double[,] A, double[] b)
        {
            int n = A.GetLength(0);
            bool[] usedRows = new bool[n];  // Массив для отслеживания использованных строк
            int[] rowOrder = new int[n];    // Массив для хранения нового порядка строк

            // Инициализация rowOrder значениями по умолчанию
            for (int i = 0; i < n; i++)
            {
                rowOrder[i] = -1;
            }

            for (int i = 0; i < n; i++)
            {
                bool found = false;
                for (int r = 0; r < n; r++)
                {
                    if (usedRows[r]) continue;

                    double sum = 0;
                    for (int j = 0; j < n; j++)
                    {
                        if (j != i)
                            sum += Math.Abs(A[r, j]);
                    }

                    if (Math.Abs(A[r, i]) > sum)
                    {
                        rowOrder[i] = r;
                        usedRows[r] = true;
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    return false;  // Невозможно привести матрицу к диагональному преобладанию
                }
            }

            // Создание новых массивов для переставленной матрицы и вектора b
            double[,] newA = new double[n, n];
            double[] newB = new double[n];

            for (int i = 0; i < n; i++)
            {
                int rowIndex = rowOrder[i];
                for (int j = 0; j < n; j++)
                {
                    newA[i, j] = A[rowIndex, j];
                }
                newB[i] = b[rowIndex];
            }

            // Копирование переставленной матрицы и вектора обратно в A и b
            Array.Copy(newA, A, A.Length);
            Array.Copy(newB, b, b.Length);

            return true;
        }



        // Метод для проверки диагонального преобладания
        private static bool HasDiagonalDominance(double[,] A)
        {
            // Получаем размер матрицы
            int n = A.GetLength(0);
            // Перебираем все строки матрицы
            for (int i = 0; i < n; i++)
            {
                double sum = 0;  // Переменная для хранения суммы элементов строки

                // Перебираем все элементы строки
                for (int j = 0; j < n; j++)
                {
                    if (i != j)
                        sum += Math.Abs(A[i, j]);  // Суммируем элементы строки, кроме диагонального элемента
                }

                // Проверяем условие диагонального преобладания
                if (Math.Abs(A[i, i]) <= sum)
                {
                    return false;  // Если условие не выполняется, возвращаем false
                }
            }
            return true;  // Если условие выполняется для всех строк, возвращаем true
        }

        // Метод для решения методом Якоби
        public static double[] SolveJacobi(double[,] A, double[] b, double tolerance = 1e-6, int maxIterations = 1000)
        {
            // Попытка привести матрицу к диагональному преобладанию
            if (!HasDiagonalDominance(A))
            {
                Console.WriteLine("Матрица не имеет диагонального преобладания. Попытка переставить строки...");
                if (!EnsureDiagonalDominance(A, b))
                {
                    throw new InvalidOperationException("Невозможно привести матрицу к диагональному преобладанию. Метод Якоби может не сойтись.");
                }
                Console.WriteLine("Матрица успешно приведена к диагональному преобладанию.");
            }

            int n = A.GetLength(0);
            double[] x = new double[n];
            double[] xNew = new double[n];

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            for (int k = 0; k < maxIterations; k++)
            {
                Parallel.For(0, n, i =>
                {
                    double sum = 0;
                    for (int j = 0; j < n; j++)
                    {
                        if (i != j)
                            sum += A[i, j] * x[j];
                    }
                    xNew[i] = (b[i] - sum) / A[i, i];
                });

                double maxDifference = 0;
                for (int i = 0; i < n; i++)
                {
                    maxDifference = Math.Max(maxDifference, Math.Abs(xNew[i] - x[i]));
                }

                if (maxDifference < tolerance)
                {
                    stopwatch.Stop();
                    Console.WriteLine($"Метод Якоби: Решение найдено за {k + 1} итераций. Время решения: {stopwatch.Elapsed}");
                    return xNew;
                }

                Array.Copy(xNew, x, n);
            }

            stopwatch.Stop();
            Console.WriteLine($"Метод Якоби: Не сошелся за {maxIterations} итераций. Время решения: {stopwatch.Elapsed}");
            return xNew;
        }

        // Метод для решения методом простой итерации
        public static double[] SolveSimpleIterations(double[,] A, double[] b, double tolerance = 1e-6, int maxIterations = 1000)
        {
            // Попытка привести матрицу к диагональному преобладанию
            if (!HasDiagonalDominance(A))
            {
                Console.WriteLine("Матрица не имеет диагонального преобладания. Попытка переставить строки...");
                if (!EnsureDiagonalDominance(A, b))
                {
                    throw new InvalidOperationException("Невозможно привести матрицу к диагональному преобладанию. Метод простой итерации может не сойтись.");
                }
                Console.WriteLine("Матрица успешно приведена к диагональному преобладанию.");
            }

            int n = A.GetLength(0);
            double[] x = new double[n];
            double[] xNew = new double[n];

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            for (int k = 0; k < maxIterations; k++)
            {
                Parallel.For(0, n, i =>
                {
                    double sum = 0;
                    for (int j = 0; j < n; j++)
                    {
                        if (i != j)
                            sum += A[i, j] * x[j];
                    }
                    xNew[i] = (b[i] - sum) / A[i, i];
                });

                double maxDifference = 0;
                for (int i = 0; i < n; i++)
                {
                    maxDifference = Math.Max(maxDifference, Math.Abs(xNew[i] - x[i]));
                }

                if (maxDifference < tolerance)
                {
                    stopwatch.Stop();
                    Console.WriteLine($"Метод простой итерации: Решение найдено за {k + 1} итераций. Время решения: {stopwatch.Elapsed}");
                    return xNew;
                }

                Array.Copy(xNew, x, n);
            }

            stopwatch.Stop();
            Console.WriteLine($"Метод простой итерации: Не сошелся за {maxIterations} итераций. Время решения: {stopwatch.Elapsed}");
            return xNew;
        }


        // Метод для решения методом Зейделя
        public static double[] SolveZeidel(double[,] A, double[] b, double tolerance = 1e-6, int maxIterations = 1000)
        {
            // Попытка привести матрицу к диагональному преобладанию
            if (!HasDiagonalDominance(A))
            {
                Console.WriteLine("Матрица не имеет диагонального преобладания. Попытка переставить строки...");
                if (!EnsureDiagonalDominance(A, b))
                {
                    throw new InvalidOperationException("Невозможно привести матрицу к диагональному преобладанию. Метод Зейделя может не сойтись.");
                }
                Console.WriteLine("Матрица успешно приведена к диагональному преобладанию.");
            }

            int n = A.GetLength(0);
            double[] x = new double[n];

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            for (int k = 0; k < maxIterations; k++)
            {
                double[] xOld = (double[])x.Clone();

                for (int i = 0; i < n; i++)
                {
                    double sum = 0;
                    for (int j = 0; j < n; j++)
                    {
                        if (i != j)
                            sum += A[i, j] * (j < i ? x[j] : xOld[j]);
                    }
                    x[i] = (b[i] - sum) / A[i, i];
                }

                double maxDifference = 0;
                for (int i = 0; i < n; i++)
                {
                    maxDifference = Math.Max(maxDifference, Math.Abs(x[i] - xOld[i]));
                }

                if (maxDifference < tolerance)
                {
                    stopwatch.Stop();
                    Console.WriteLine($"Метод Зейделя: Решение найдено за {k + 1} итераций. Время решения: {stopwatch.Elapsed}");
                    return x;
                }
            }

            stopwatch.Stop();
            Console.WriteLine($"Метод Зейделя: Не сошелся за {maxIterations} итераций. Время решения: {stopwatch.Elapsed}");
            return x;
        }

    }
}
