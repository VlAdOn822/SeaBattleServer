using SeaBattleServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleInterface
{
    class Program
    {
        static void Main(string[] args)
        {
            Uri tcpUri = new Uri("http://localhost:8000/SeaBattleServer");
            // Создаём сетевой адрес, с которым клиент будет взаимодействовать
            EndpointAddress address = new EndpointAddress(tcpUri);
            BasicHttpBinding binding = new BasicHttpBinding();
            // Данный класс используется клиентами для отправки сообщений
            ChannelFactory<ISeaBattle> factory = new ChannelFactory<ISeaBattle>(binding, address);
            // Открываем канал для общения клиента с со службой
            ISeaBattle service = factory.CreateChannel();

            SeaBattleConsoleUI ui = new SeaBattleConsoleUI(new Game());
            ui.Start();
            Console.ReadLine();
        }
    }
}
