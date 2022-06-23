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
        static void ClientProcessing(object client_obj)
        {//Буфер для принимаемых данных
            Byte[] bytes = new Byte[256];
            String data = null;
            TcpClient client = client_obj as TcpClient;//??
            data = null;
            //Получаем информацию от клиента
            NetworkStream stream = client.GetStream();
            int i;
            //Принимаем данные от клиента в цикле пока не дойдём до конца
            while((i=stream.Read(bytes,0,bytes.Length))!=0)
            {//Преобразуем данные в строку ASCII
                data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                //Преобразуем строку к верхнему регистру
                data = data.ToUpper();
                //Преобразуем полученную строку в массив байт
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);
                //Отправляем данные обратно клиенту(ответ)
                stream.Write(msg, 0, msg.Length);
            }

        }
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
                Console.WriteLine("SocketException: {0}", ex);
            }
            finally
            {
                //Останавливаем сервер
                server.Stop();
                Console.WriteLine("\nНажмите Enter...");
                Console.Read();
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
