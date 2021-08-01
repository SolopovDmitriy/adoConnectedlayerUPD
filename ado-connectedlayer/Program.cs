using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ado_connectedlayer
{
    class Program
    {
        static void Main(string[] args)
        {
            string dpString = ConfigurationManager.AppSettings["provider"];
            string connectionString = ConfigurationManager.ConnectionStrings["SqlProvider"].ConnectionString;

            //получаем поставщика подключения
            DbProviderFactory df = DbProviderFactories.GetFactory(dpString);

            //получаем обьект подключения
            DbConnection conn = df.CreateConnection();
            conn.ConnectionString = connectionString;
            conn.Open();
            Console.WriteLine("Подключение установлено");
            Console.WriteLine($"Обьект подключения: {conn.GetType().Name}"); ;
            conn.Close();


            using (DbConnection connTwo = df.CreateConnection())
            {
                connTwo.ConnectionString = connectionString;
                connTwo.Open();

                DbCommand dbCommandGetUsers = df.CreateCommand();
                dbCommandGetUsers.Connection = connTwo;
                dbCommandGetUsers.CommandText = "SELECT * FROM users";

                //dbCommandGetUsers.ExecuteNonQuery();  //не возвр рез, а только кол затронутых строк
                //dbCommandGetUsers.ExecuteReader();      //возвр результ набор
                //dbCommandGetUsers.ExecuteScalar();      //выполняет запрос и возвращает значение первого столбца первой строки

                using (DbDataReader drAllUsers = dbCommandGetUsers.ExecuteReader())
                {
                    Console.WriteLine($"Обьект подключения: {drAllUsers.GetType().Name}"); ;
                    Console.WriteLine("Содержимое таблицы Users: ");
                    Console.ForegroundColor = ConsoleColor.Green;

                    while (drAllUsers.Read())
                    {
                        Console.WriteLine($"ID: {drAllUsers["Id"]}; Login: {drAllUsers["login"]}; Password: {drAllUsers["password"]}");
                    }
                    Console.ResetColor();
                }

            }


        }
    }
}
