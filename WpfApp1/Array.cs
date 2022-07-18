using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    internal class Array
    {
        private int[,] array;
        private int arraySize;

        public Array(int[,] array, int arraySize)
        {
            this.array = array;
            this.arraySize = arraySize;
        }
        
        public int SumMainDiagonal()
        {
            int sum = 0;
            for(int i=0; i<arraySize; i++)
            {
                sum += array[i, i];
            }
            return sum;
        }

        public int SumIndicatedRow(int row)
        {
            int sum = 0;
            for(int i=0; i<arraySize; i++)
            {
                sum += array[row - 1, i];
            }
            return sum;
        }

        public int SumIndicatedColumn(int column)
        {
            int sum = 0;
            for(int i=0; i<arraySize; i++)
            {
                sum += array[i, column - 1];
            }
            return sum;
        }

        public void SaveToFile()
        {
            string fileName = "array.txt";
            using (StreamWriter sw = File.AppendText(fileName))
            {
                for (int row = 0; row < arraySize; row++)
                {
                    for (int col = 0; col < arraySize; col++)
                    {
                        sw.Write(array[row, col] + " ");
                    }
                    sw.WriteLine();
                }
            }
        }
    }
}
