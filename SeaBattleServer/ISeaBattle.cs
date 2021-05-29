using System.ServiceModel;

namespace SeaBattleServer
{
    [ServiceContract]
    public interface ISeaBattle
    {
        [OperationContract]
        string StartGame();

        [OperationContract]
        int JoinGame(string gameCode, string playerName, string[] field);

        [OperationContract]
        string Shot(string gameCode, int numPlayer, string code);

        [OperationContract]
        string[] GetBoard(string gameCode, int numPlayer);

        [OperationContract]
        int CheckLooser(string gameCode);

        [OperationContract]
        string Update(string gameCode, int numPlayer);
    }
}