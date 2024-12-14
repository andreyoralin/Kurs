
using System;
using System.Globalization;
using System.Text;

namespace ConsoleApp1
{
    class Input
    {
        private static int n; // Кол-во уравнений
        private static double[,] A;  // Матрица коэффициентов

        public static int SetAmount()
        {
            Console.WriteLine("Введите количество уравнений");
            int amount;
            while (true)
            {
                try
                {
                    amount = int.Parse(Console.ReadLine());
                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Ошибка ввода. Пожалуйста, введите целое число.");
                }
            }
            return amount;
        }

        public static double[,] SetCoefficients(int n)
        {
            A = new double[n, n];
            Console.WriteLine("\nВведите коэффициенты матрицы построчно (элементы разделяйте пробелами):\n");

            for (int i = 0; i < n; i++)
            {
                Console.Write($"Строка {i + 1}: ");
                while (true)
                {
                    try
                    {
                        string[] input = Console.ReadLine().Split(' ');
                        if (input.Length != n)
                        {
                            throw new FormatException("Количество введенных элементов не совпадает с размером матрицы.");
                        }
                        for (int j = 0; j < n; j++)
                        {
                            if (!double.TryParse(input[j], NumberStyles.Any, CultureInfo.InvariantCulture, out A[i, j]))
                            {
                                throw new FormatException($"Элемент '{input[j]}' не является корректным числом.");
                            }
                        }
                        break;
                    }
                    catch (FormatException e)
                    {
                        Console.WriteLine($"Ошибка при вводе: {e.Message}. Попробуйте снова.");
                    }
                }
            }
            return A;
        }

        public static double[] SetFreeCoefficients(int n)
        {
            double[] b = new double[n];
            Console.WriteLine("Введите вектор правой части (элементы разделяйте пробелами): ");
            while (true)
            {
                try
                {
                    string[] bInput = Console.ReadLine().Split(' ');
                    if (bInput.Length != n)
                    {
                        throw new FormatException("Количество введенных элементов не совпадает с размером вектора.");
                    }
                    for (int i = 0; i < n; i++)
                    {
                        if (!double.TryParse(bInput[i], NumberStyles.Any, CultureInfo.InvariantCulture, out b[i]))
                        {
                            throw new FormatException($"Элемент '{bInput[i]}' не является корректным числом.");
                        }
                    }
                    break;
                }
                catch (FormatException e)
                {
                    Console.WriteLine($"Ошибка при вводе: {e.Message}. Попробуйте снова.");
                }
            }
            return b;
        }



        // Метод для сериализации матрицы в строку
        public static string MatrixSerialisation(double[,] matrix)
        {
            int rows = matrix.GetLength(0);  // Получаем количество строк
            int cols = matrix.GetLength(1);  // Получаем количество столбцов
            StringBuilder sb = new StringBuilder();  // Создаем StringBuilder для создания строки

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    sb.Append(matrix[i, j].ToString("G17"));  // Добавляем элемент матрицы в строку
                    if (j < cols - 1)
                    {
                        sb.Append(",");  // Добавляем запятую после каждого элемента, кроме последнего в строке
                    }
                }
                if (i < rows - 1)
                {
                    sb.Append(";");  // Добавляем точку с запятой после каждой строки, кроме последней
                }
            }

            return sb.ToString();  // Возвращаем строковое представление матрицы
        }
 

        // Метод для сериализации вектора в строку
        public static string VectorSerialisation(double[] vector)
        {
            StringBuilder sb = new StringBuilder();  // Создаем StringBuilder для создания строки

            for (int i = 0; i < vector.Length; i++)
            {
                sb.Append(vector[i].ToString("G17"));  // Добавляем элемент вектора в строку
                if (i < vector.Length - 1)
                {
                    sb.Append(",");  // Добавляем запятую после каждого элемента, кроме последнего
                }
            }

            return sb.ToString();  // Возвращаем строковое представление вектора
        }
    }

}



