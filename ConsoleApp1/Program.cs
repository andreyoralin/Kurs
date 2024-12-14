using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Data.SqlClient;

namespace ConsoleApp1
{
    class Program
    {
        public Database Database
        {
            get => default;
            set
            {
            }
        }

        public Solver Solver
        {
            get => default;
            set
            {
            }
        }

        internal Input Input
        {
            get => default;
            set
            {
            }
        }

        public async static Task Main()
        {
            try
            {
                Database database = new Database();
                await database.ConnectDb();


                int amount = Input.SetAmount();

                double[,] matrix =  Input.SetCoefficients(amount);
                double[] free = Input.SetFreeCoefficients(amount);

                double[] result = null;
                int choice = Choose();

                switch (choice)
                {
                    case 1:
                        result = Solver.SolveZeidel(matrix, free);
                        break;
                    case 2:
                        result = Solver.SolveJacobi(matrix, free);
                        break;
                    case 3:
                        result = Solver.SolveSimpleIterations(matrix, free);
                        break;
                    default:
                        Console.WriteLine("Неверный номер, попробуйте еще раз");
                        await Main();
                        return;
                }
                string stringMatrix = Input.MatrixSerialisation(matrix);
                string vectorMatrix = Input.VectorSerialisation(result);
                string freeMatrix = Input.VectorSerialisation(free);
                
                database.UpdateData(stringMatrix, freeMatrix,vectorMatrix);

                foreach (double element in result)
                {
                    Console.WriteLine($"{element} ");
                }
           
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }
        }

        public static int Choose()
        {
            while (true)
            {
                try
                {
                    Console.Write("Выберите метод решения\n" +
                                  "1) Метод Зейделя\n" +
                                  "2) Метод Якоби\n" +
                                  "3) Метод простых итераций (Пикари)\n\n");
                    int choice = int.Parse(Console.ReadLine());
                    if (choice < 1 || choice > 3)
                    {
                        throw new ArgumentOutOfRangeException("choice", "Выбор должен быть между 1 и 3.");
                    }
                    return choice;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Ошибка ввода. Пожалуйста, введите число.");
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}