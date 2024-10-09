using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace BASM
{
    public static class QuickSortingAlgorithms
    {
        //worst: O(n²)
        //avarage: O(n log n)
        //best: O(n log n) / O(n)(dual)

        //memory: O(log n)

        public static void Swap(ref int[] arr, int i, int j)
        {
            int temp = arr[i];
            arr[i] = arr[j];
            arr[j] = temp;
        }

        public static int Median3(ref int[] arr, int left, int right)
        {
            if (arr[right] < arr[left])
                Swap(ref arr, left, right);
            int mid = (left + right) >> 1;
            if (arr[mid] < arr[left])
                Swap(ref arr, left, mid);
            if (arr[mid] > arr[right])
                Swap(ref arr, mid, right);
            return arr[mid];
        }

        /// <summary>         
        /// <tt>dual: ✔️</tt><br/>
        /// <tt>median: ❌</tt><br/>
        /// <tt>unsafe: ❌</tt><br/>
        /// <tt>insertion: ❌</tt>
        /// /// https://www.geeksforgeeks.org/dual-pivot-quicksort/<br/>
        /// </summary>
        public static void QuickSortD(ref int[] arr, int left, int right)
        {
            if (left < right)
            {
                var piv = DualPartition(ref arr, left, right);
                QuickSortD(ref arr, left, piv.Item1 - 1);
                QuickSortD(ref arr, piv.Item1 + 1, piv.Item2 - 1);
                QuickSortD(ref arr, piv.Item2 + 1, right);
            }
        }

        //?
        public static (int, int) DualPartition(ref int[] arr, int left, int right)
        {
            if (arr[left] > arr[right])
                Swap(ref arr, left, right);
            int j = left + 1;
            int g = right - 1, k = left + 1,

            p = arr[left], q = arr[right];

            while (k <= g)
            {

                // If elements are less than the left pivot
                if (arr[k] < p)
                {
                    Swap(ref arr, k, j);
                    j++;
                }

                // If elements are greater than or equal
                // to the right pivot
                else if (arr[k] >= q)
                {
                    while (arr[g] > q && k < g)
                        g--;

                    Swap(ref arr, k, g);
                    g--;

                    if (arr[k] < p)
                    {
                        Swap(ref arr, k, j);
                        j++;
                    }
                }
                k++;
            }
            j--;
            g++;

            // Bring pivots to their appropriate positions.
            Swap(ref arr, left, j);
            Swap(ref arr, right, g);

            // Returning the indices of the pivots
            // because we cannot return two elements
            // from a function, we do that using an array.
            return (j, g);
        }

        /// <summary>         
        /// <tt>dual: ✔️</tt><br/>
        /// <tt>median: ✔️</tt><br/>
        /// <tt>unsafe: ❌</tt><br/>
        /// <tt>insertion: ✔️</tt>
        /// /// https://github.com/litwr2/research-of-sorting/blob/master/quick-dp.cpp с сортировкой вставками<br/>
        /// </summary>
        public static void QuicksortDMI(ref int[] arr, int left, int right)
        {
            int len = right - left;
            if (len < 17)
            {
                InsertionSortAlgorithms.InsertionSortM(ref arr, left, right);
                return;
            }
            // median indexes
            int sixth = len / 6;
            int m1 = left + sixth;
            int m2 = m1 + sixth;
            int m3 = m2 + sixth;
            int m4 = m3 + sixth;
            int m5 = m4 + sixth;
            // 5-element sorting network
            if (arr[m1] > arr[m2]) Swap(ref arr, m1, m2);
            if (arr[m4] > arr[m5]) Swap(ref arr, m4, m5);
            if (arr[m1] > arr[m3]) Swap(ref arr, m1, m3);
            if (arr[m2] > arr[m3]) Swap(ref arr, m2, m3);
            if (arr[m1] > arr[m4]) Swap(ref arr, m1, m4);
            if (arr[m3] > arr[m4]) Swap(ref arr, m3, m4);
            if (arr[m2] > arr[m5]) Swap(ref arr, m2, m5);
            if (arr[m2] > arr[m3]) Swap(ref arr, m2, m3);
            if (arr[m4] > arr[m5]) Swap(ref arr, m4, m5);
            // pivots: [ < pivot1 | pivot1 <= && <= pivot2 | > pivot2 ]
            int pivot1 = arr[m2];
            int pivot2 = arr[m4];
            bool diffPivots = pivot1 != pivot2;
            arr[m2] = arr[left];
            arr[m4] = arr[right];
            // center part pointers
            int less = left + 1;
            int great = right - 1;
            // sorting
            if (diffPivots)
                for (int k = less; k <= great; k++)
                {
                    if (arr[k] < pivot1)
                        Swap(ref arr, k, less + 1);
                    else if (arr[k] > pivot2)
                    {
                        while (arr[great] > pivot2 && k < great)
                            great--;
                        Swap(ref arr, k, great - 1);
                        if (arr[k] < pivot1)
                            Swap(ref arr, k, less + 1);
                    }
                }
            else
                for (int k = less; k <= great; k++)
                    if (arr[k] == pivot1)
                        continue;
                    else if (arr[k] < pivot1)
                        Swap(ref arr, k, less + 1);
                    else
                    {
                        while (arr[great] > pivot2 && k < great)
                            great--;
                        Swap(ref arr, k, great - 1);
                        if (arr[k] < pivot1)
                            Swap(ref arr, k, less + 1);
                    }
            // swap
            arr[left] = arr[less - 1];
            arr[less - 1] = pivot1;
            arr[right] = arr[great + 1];
            arr[great + 1] = pivot2;
            // left and right parts
            QuicksortDMI(ref arr, left, less - 2);
            QuicksortDMI(ref arr, great + 2, right);
            // equal elements
            if (great - less > len - 13 && diffPivots)
                for (int k = less; k <= great; k++)
                    if (arr[k] == pivot1)
                        Swap(ref arr, k, less + 1);
                    else if (arr[k] == pivot2)
                    {
                        Swap(ref arr, k, great - 1);
                        if (arr[k] == pivot1)
                            Swap(ref arr, k, less + 1);
                    }
            // center part
            if (diffPivots)
                QuicksortDMI(ref arr, less, great);
        }
        public static void QuicksortDMID(ref int[] arr, int left, int right)
        {
            int len = right - left;
            if (len < 47)
            {
                InsertionSortAlgorithms.InsertionSortD(ref arr, left, right);
                return;
            }
            // median indexes
            int sixth = len / 6;
            int m1 = left + sixth;
            int m2 = m1 + sixth;
            int m3 = m2 + sixth;
            int m4 = m3 + sixth;
            int m5 = m4 + sixth;
            // 5-element sorting network
            if (arr[m1] > arr[m2]) Swap(ref arr, m1, m2);
            if (arr[m4] > arr[m5]) Swap(ref arr, m4, m5);
            if (arr[m1] > arr[m3]) Swap(ref arr, m1, m3);
            if (arr[m2] > arr[m3]) Swap(ref arr, m2, m3);
            if (arr[m1] > arr[m4]) Swap(ref arr, m1, m4);
            if (arr[m3] > arr[m4]) Swap(ref arr, m3, m4);
            if (arr[m2] > arr[m5]) Swap(ref arr, m2, m5);
            if (arr[m2] > arr[m3]) Swap(ref arr, m2, m3);
            if (arr[m4] > arr[m5]) Swap(ref arr, m4, m5);
            // pivots: [ < pivot1 | pivot1 <= && <= pivot2 | > pivot2 ]
            int pivot1 = arr[m2];
            int pivot2 = arr[m4];
            bool diffPivots = pivot1 != pivot2;
            arr[m2] = arr[left];
            arr[m4] = arr[right];
            // center part pointers
            int less = left + 1;
            int great = right - 1;
            // sorting
            if (diffPivots)
                for (int k = less; k <= great; k++)
                {
                    if (arr[k] < pivot1)
                        Swap(ref arr, k, less + 1);
                    else if (arr[k] > pivot2)
                    {
                        while (arr[great] > pivot2 && k < great)
                            great--;
                        Swap(ref arr, k, great - 1);
                        if (arr[k] < pivot1)
                            Swap(ref arr, k, less + 1);
                    }
                }
            else
                for (int k = less; k <= great; k++)
                    if (arr[k] == pivot1)
                        continue;
                    else if (arr[k] < pivot1)
                        Swap(ref arr, k, less + 1);
                    else
                    {
                        while (arr[great] > pivot2 && k < great)
                            great--;
                        Swap(ref arr, k, great - 1);
                        if (arr[k] < pivot1)
                            Swap(ref arr, k, less + 1);
                    }
            // swap
            arr[left] = arr[less - 1];
            arr[less - 1] = pivot1;
            arr[right] = arr[great + 1];
            arr[great + 1] = pivot2;
            // left and right parts
            QuicksortDMI(ref arr, left, less - 2);
            QuicksortDMI(ref arr, great + 2, right);
            // equal elements
            if (great - less > len - 13 && diffPivots)
                for (int k = less; k <= great; k++)
                    if (arr[k] == pivot1)
                        Swap(ref arr, k, less + 1);
                    else if (arr[k] == pivot2)
                    {
                        Swap(ref arr, k, great - 1);
                        if (arr[k] == pivot1)
                            Swap(ref arr, k, less + 1);
                    }
            // center part
            if (diffPivots)
                QuicksortDMI(ref arr, less, great);
        }

        /// <summary>  
        /// <tt>dual: ❌</tt><br/>
        /// <tt>median: ✔️</tt><br/>
        /// <tt>unsafe: ❌</tt><br/>
        /// <tt>insertion: ❌</tt>
        /// </summary>
        public static void QuickSortM(ref int[] arr, int left, int right)
        {
            if (right - left == 1)
            {
                if (arr[right] < arr[left])
                    Swap(ref arr, left, right);
                return;
            }
            int pivot = Median3(ref arr, left, right);

            int i = left + 1;
            int j = right - 1;
            while (i <= j)
            {
                while (arr[i] < pivot)
                    i++;
                while (arr[j] > pivot)
                    j--;
                if (i <= j)
                {
                    Swap(ref arr, i, j);
                    i++;
                    j--;
                }
            }
            if (j > left)
                QuickSortM(ref arr, left, j);
            if (i < right)
                QuickSortM(ref arr, i, right);
        }

        /// <summary>  
        /// <tt>dual: ❌</tt><br/>
        /// <tt>median: ❌</tt><br/>
        /// <tt>unsafe: ❌</tt><br/>
        /// <tt>insertion: ❌</tt>
        /// </summary>
        public static void QuickSort(ref int[] arr, int left, int right)
        {
            int pivot = arr[(right + left) << 1];
            int i = left;
            int j = right;
            while (i <= j)
            {
                while (arr[i] < pivot)
                    i++;
                while (arr[j] > pivot)
                    j--;
                if (i <= j)
                {
                    Swap(ref arr, i, j);
                    i++;
                    j--;
                }
            }
            if (j > left)
                QuickSort(ref arr, left, j);
            if (i < right)
                QuickSort(ref arr, i, right);
        }

        /// <summary>  
        /// <tt>dual: ❌</tt><br/>
        /// <tt>median: ✔️</tt><br/>
        /// <tt>unsafe: ❌</tt><br/>
        /// <tt>insertion: ✔️</tt>
        /// </summary>
        public static void QuickSortMI(ref int[] arr, int left, int right)
        {
            if (right - left < 17)
            {
                InsertionSortAlgorithms.InsertionSortM(ref arr, left, right);
                return;
            }
            int pivot = Median3(ref arr, left, right);

            int i = 1 + left;
            int j = right - 1;
            while (i <= j)
            {
                while (arr[i] < pivot)
                    i++;
                while (arr[j] > pivot)
                    j--;
                if (i <= j)
                {
                    Swap(ref arr, i, j);
                    i++;
                    j--;
                }
            }
            if (j > left)
                QuickSortMI(ref arr, left, j);
            if (i < right)
                QuickSortMI(ref arr, i, right);
        }

        /// <summary>  
        /// <tt>dual: ❌</tt><br/>
        /// <tt>median: ❌</tt><br/>
        /// <tt>unsafe: ❌</tt><br/>
        /// <tt>insertion: ✔️</tt><br/>
        /// </summary>
        public static void QuickSortI(ref int[] arr, int left, int right)
        {
            if (right - left < 17)
            {
                InsertionSortAlgorithms.InsertionSortM(ref arr, left, right);
                return;
            }
            int pivot = arr[right];
            int i = left;
            int j = right;
            while (i <= j)
            {
                while (arr[i] < pivot)
                    i++;
                while (arr[j] > pivot)
                    j--;
                if (i <= j)
                {
                    Swap(ref arr, i, j);
                    i++;
                    j--;
                }
            }
            if (j > left)
                QuickSortI(ref arr, left, j);
            if (i < right)
                QuickSortI(ref arr, i, right);
        }

        /// <summary>  
        /// <tt>dual: ❌</tt><br/>
        /// <tt>median: ✔️</tt><br/>
        /// <tt>unsafe: ✔️</tt><br/>
        /// <tt>insertion: ✔️</tt>
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static unsafe void QuickSortMUI(ref int[] arr, int left, int right)
        {
            if (right - left < 17)
            {
                InsertionSortAlgorithms.InsertionSortM(ref arr, left, right);
                return;
            }
            int pivot = Median3(ref arr, left, right);
            Vector256<int> v2 = Vector256.Create(pivot);

            int i = left + 1;
            int j = right - 7;

            fixed (int* ptr = arr)
            {
                while (i <= j + 7) //i - указатель на элемент больше pivot
                                   //j - указатель на элемент меньше pivot, но -7
                {

                    while (true)
                    {
                        if (i < right - 7)
                        {
                            var sv = Avx2.LoadVector256(ptr + i);
                            var res = Avx2.MoveMask(Avx2.CompareGreaterThan(sv, v2).AsByte()); ;
                            var co = BitOperations.TrailingZeroCount(res);
                            if (co != 32)
                            {
                                i += co >> 2;
                                break;
                            }
                            i += 8;
                        }
                        else
                        {
                            while (arr[i] < pivot)
                            {
                                i++;
                            }

                            break;
                        }

                    }

                    while (true)
                    {
                        if (j > -1)
                        {
                            var sv = Avx2.LoadVector256(ptr + j);
                            var res = Avx2.MoveMask((Avx2.CompareGreaterThan(v2, sv)).AsByte());
                            var co = BitOperations.LeadingZeroCount(unchecked((uint)res));

                            if (co != 32)
                            {
                                j -= (co >> 2);
                                break;
                            }
                            j -= 8;
                        }
                        else
                        {
                            if (j < -7) break;
                            while (arr[j + 7] > pivot)
                            {

                                j--;
                                if (j < -7)
                                {
                                    j = -7;
                                    break;

                                }
                            }

                            break;
                        }
                    }

                    if (i <= j + 7)
                    {
                        Swap(ref arr, i, j + 7);
                        i++;
                        j--;
                    }
                }
                if (j + 7 > left)
                    QuickSortMUI(ref arr, left, j + 7);
                if (i < right)
                    QuickSortMUI(ref arr, i, right);
            }
        } 
    }
}
