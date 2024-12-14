using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace ConsoleApp1
{
    public class Database
    {
        private static string connectionString = @"Server=DESKTOP-DPGAK4V;Database=system;Trusted_Connection=True;TrustServerCertificate=True;";


        private string query = @"INSERT INTO EquationSolutions (Matrix, Solution) VALUES (@Matrix, @Solution)";
        private SqlConnection connection = new SqlConnection(connectionString);

        public async Task ConnectDb()
        {
            try
            {
                await connection.OpenAsync();
                Console.WriteLine("Подключен к БД\n");
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task CloseConnect()
        {
            if (connection.State == ConnectionState.Open)
            {
                await connection.CloseAsync();
                Console.WriteLine("Подключение закрыто...");
            }
        }

        public async Task UpdateData(string matrixStr, string freeVectorStr, string solutionStr)
        {
            // Проверка состояния подключения и подключение к базе данных при необходимости
            if (connection.State != ConnectionState.Open)
            {
                await ConnectDb();
            }

            // SQL-запрос для вставки данных в таблицу
            string query = @"
        INSERT INTO LinearSystemSolutions (Matrix, Free, Solution, CreatedAt) 
        VALUES (@Matrix, @Free, @Solution, GETDATE())";

            // Создание SQL-команды с параметрами
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Matrix", matrixStr);
                command.Parameters.AddWithValue("@Free", freeVectorStr);
                command.Parameters.AddWithValue("@Solution", solutionStr);

                try
                {
                    // Выполнение команды асинхронно
                    await command.ExecuteNonQueryAsync();
                    Console.WriteLine("Результаты успешно вставлены в базу данных.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при вставке результатов: {ex.Message}");
                }
                finally
                {
                    // Закрытие подключения после выполнения операции
                    await CloseConnect();
                }
            }
        }

    }
}
