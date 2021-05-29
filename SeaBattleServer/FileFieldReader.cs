using System.IO;

namespace SeaBattleServer
{
     public class FileFieldReader : IFieldReader
    {
        public string fileName;

        public FileFieldReader(string fileName)
        {
            this.fileName = fileName;
        }

        public string[] ReadField()
        {
            StreamReader sr = new StreamReader(fileName);
            string[] field = new string[10];
            for(int i = 0; i<10; i++)
            {
                field[i] = sr.ReadLine().Trim();
            }
            sr.Close();
            return field;
        }
    }
}