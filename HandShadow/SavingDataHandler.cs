using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HandShadow
{
    static class SavingDataHandler
    {
        public static int[,] isLevelCompleted = new int[4, 9];

        public static void readFile()
        {
            StreamReader objReader = new StreamReader("Content//Data//LevelCompletedData.txt");
            for (int i = 0; i < 4; i++)
            {
                String thisLine = objReader.ReadLine();
                String[] levelStr = thisLine.Split(' ');
                for (int j = 0; j < 9; j++)
                {

                    isLevelCompleted[i, j] = int.Parse(levelStr[j]);
                }
            }
            objReader.Close();
        }

        public static void writeFile()
        {
            String SavingDataStr = "";
            for (int i = 0; i < 4; i++)
            {
                if (i != 0) SavingDataStr += '\n';
                for (int j = 0; j < 9; j++)
                {
                    SavingDataStr += isLevelCompleted[i, j].ToString() + " ";
                }
            }
            FileStream fs = new FileStream("Content//Data//LevelCompletedData.txt", FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(SavingDataStr);
            sw.Flush();
            sw.Close();
            fs.Close();
        }
    }
}
