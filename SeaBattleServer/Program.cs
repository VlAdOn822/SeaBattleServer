using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattleServer
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost host = new ServiceHost(typeof(Game), new Uri("http://localhost:9000/SeaBattleServer"));
            // Добавляем конечную точку службы с заданным интерфейсом, привязкой (создаём новую) и адресом конечной точки
            host.AddServiceEndpoint(typeof(ISeaBattle), new BasicHttpBinding(), "");
            // Запускаем службу
            host.Open();
            Console.WriteLine("Сервер запущен");
            Console.ReadLine();
            // Закрываем службу
            host.Close();

        }
    }
}
