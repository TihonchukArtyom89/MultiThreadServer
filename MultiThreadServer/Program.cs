using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace MultiThreadServer
{
    class ExampleTcpListener
    {
        static void Main(string[] args)
        {
            TcpListener server = null;
            try
            {//Установим максимальное количество потоков
                int MaxThreadsCount = Environment.ProcessorCount * 4;
                ThreadPool.SetMaxThreads(MaxThreadsCount, MaxThreadsCount);
                //Установим минимальное количество потоков
                ThreadPool.SetMinThreads(2, 2);
                //Установим порт для TcpListener
                Int32 port = 9595;
                
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");
                int counter = 0;
                server = new TcpListener(localAddr, port);
                Console.WriteLine("Конфигурация многопоточного сервера:\nIP-адрес: "
                    + localAddr.ToString() + "\nПорт: " + port.ToString() + "\nПотоки: " + MaxThreadsCount.ToString() +
                    "\nСервер запущен!\n");
                //Запускаем сервер и начинаем слушать клиентов
                server.Start();
                //Принимаем  клиентов в бесконечном цикле
                while(true)
                {
                    Console.WriteLine("Ожидание соединения...");
                    ThreadPool.QueueUserWorkItem(ClientProcessing, server.AcceptTcpClient());
                    //Выводим информацию о подключении
                    counter++;
                    Console.Write("\nСоединение № "+ counter.ToString() + " !");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
