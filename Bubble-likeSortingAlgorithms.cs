using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BASM
{
    internal class Bubble_likeSorts
    {
        public static void Swap(ref int[] arr, int i, int j)
        {
            int temp = arr[i];
            arr[i] = arr[j];
            arr[j] = temp;
        }
        
        void BubbleSort(ref int[] arr)
        {
            bool sorted = true;
            for (int j = 1; (j < arr.Length - 1); j++)
            {
                sorted = true;
                int g = arr.Length - j;
                for (int i = 0; i < g; i++)
                {
                    if (arr[i] > arr[i + 1])
                    {
                        Swap(ref arr, i, i + 1);
                        sorted = false;
                    }
                }
                if (sorted) break;
            }
        }
        
        void ShakeSort(ref int[] arr)
        {
            int p1 = 0;
            int p2 = arr.Length - 1;
            while (p1 < p2)
            {
                for (int i = p1; i < p2; i++)
                {
                    if (arr[i] > arr[i + 1])
                    {
                        Swap(ref arr, i, i + 1);
                    }
                }
                p2--;
                for (int i = p2; i > p1; i--)
                {
                    if (arr[i] < arr[i - 1])
                    {
                        Swap(ref arr, i, i - 1);
                    }
                }
                p1++;
            }
        }
        
        void OddEvenSort(ref int[] arr)
        {
            bool sorted = false;

            while (!sorted)
            {
                sorted = true;
                for (int i = 0; i < arr.Length - 1; i += 2)
                {
                    if (arr[i] > arr[i + 1])
                    {
                        Swap(ref arr, i, i + 1);
                        sorted = false;
                    }
                }
                for (int i = 1; i < arr.Length - 1; i += 2)
                {
                    if (arr[i] > arr[i + 1])
                    {
                        Swap(ref arr, i, i + 1);
                        sorted = false;
                    }
                }
            }
        }

        void CombSort(ref int[] arr)
        {
            int step = arr.Length - 1;
            while (step >= 1)
            {
                for (int i = 0; i + step < arr.Length; i++)
                {
                    if (arr[i] > arr[i + step])
                    {
                        Swap(ref arr, i, i + step);
                    }
                }
                step = (int)(step / 1.2473309f);
            }
        }
        
        void GnomeSort(int[] arr)
        {
            int i = 1;
            int j = 2;
            while (i < arr.Length)
            {
                if (arr[i - 1] <= arr[i])
                {
                    i = j;
                    j++;
                }
                else
                {
                    Swap(ref arr, i - 1, i);
                    i--;
                    if (i == 0)
                    {
                        i = j;
                        j = j++;
                    }
                }
            }
        }
    }
}
