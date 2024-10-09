using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Security;

namespace BASM
{
    public static class InsertionSortAlgorithms
    {
        //worst:   O(n²)
        //avarage: O(n²)
        //best:    O(n)

        //memory: O(n)

        //TODO: unsafe dual sort
        //TODO: insertion sort every 32 elements and finally merge sort all array(which merge sort?) Name: merge-insertion
        [DllImport("msvcrt.dll", EntryPoint = "memmove", CallingConvention = CallingConvention.Cdecl, SetLastError = false), SuppressUnmanagedCodeSecurity]
        public static unsafe extern void* MoveMemory(void* dest, void* src, ulong count);
        public static void Swap(ref int[] arr, int i, int j)
        {
            int temp = arr[i];
            arr[i] = arr[j];
            arr[j] = temp;
        }

        /// <summary>         
        /// <tt>dual: ✔️</tt><br/>
        /// <tt>unsafe: ❌</tt><br/>
        /// </summary>

        public static void InsertionSortD(ref int[] arr, int left, int right)
        {
            for (int i = left; i < right; i += 2)
            {
                int t = i - 1;
                int a1 = arr[i];
                int a2 = arr[i + 1];
                if (a1 > a2)
                {
                    a1 = a2;
                    a2 = arr[i];
                }

                while (t > -1 && arr[t] > a2)
                {
                    arr[t + 2] = arr[t];
                    t--;
                }

                arr[t + 2] = a2;

                while (t > -1 && arr[t] > a1)
                {
                    arr[t + 1] = arr[t];
                    t--;
                }
                arr[t + 1] = a1;
            }

            int last = arr[right]; right--;
            while (right > -1 && last < arr[right])
            {
                arr[right + 1] = arr[right];
                right--;
            }
            arr[right + 1] = last;
        }

        /// <summary>         
        /// <tt>dual: ❌</tt><br/>
        /// <tt>unsafe: ✔️</tt><br/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static unsafe void InsertionSortU(ref int[] arr)
        {

            InsertionSortM(ref arr, 0, 7);
            fixed (int* ptr = arr)
            {
                for (int i = 8; i < arr.Length; i++)
                {
                    int curr = arr[i];
                    Vector256<int> v2 = Vector256.Create(curr);
                    int j = i - 8;
                    while (true)
                    {
                        if (j < 0)
                        {
                            var sv = Avx2.LoadVector256(ptr);
                            var res = Avx2.MoveMask((Avx2.CompareGreaterThan(v2, sv)).AsByte());
                            var co = Popcnt.PopCount(unchecked((uint)(res))) >> 2;
                            var nptr = ptr + co;
                            MoveMemory(nptr + 1, nptr, unchecked((ulong)((i - co)) << 2));
                            arr[co] = curr;
                            break;
                        }
                        else
                        {
                            var nptr = ptr + j;
                            var sv = Avx2.LoadVector256(nptr);
                            var res = Avx2.MoveMask((Avx2.CompareGreaterThan(v2, sv)).AsByte());
                            var co = Popcnt.PopCount(unchecked((uint)(res)));
                            if (co > 0)
                            {
                                co = co >> 2;
                                var jco = co + j;
                                nptr += co;
                                MoveMemory(nptr + 1, nptr, unchecked((ulong)((i - jco)) << 2));
                                // Buffer.MemoryCopy(ptr + jco, ptr + jco + 1, 32, 32);
                                //  Array.Copy(arr, jco, arr, jco + 1, i - jco);
                                arr[jco] = curr;
                                break;
                            }
                            j -= 8;
                        }
                    }


                }

            }


        }

        /// <summary>         
        /// <tt>dual: ❌</tt><br/>
        /// <tt>unsafe: ✔️</tt><br/>
        /// With Array.Copy
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static unsafe void InsertionSortUC(ref int[] arr)
        {
            InsertionSortM(ref arr, 0, 7);

            fixed (int* ptr = arr)
            {
                for (int i = 8; i < arr.Length; i++)
                {
                    int curr = arr[i];
                    Vector256<int> v2 = Vector256.Create(curr);
                    int j = i - 8;
                    while (true)
                    {
                        if (j < 0)
                        {
                            var sv = Avx2.LoadVector256(ptr);
                            var res = Avx2.MoveMask((Avx2.CompareGreaterThan(v2, sv)).AsByte());
                            var co = Popcnt.PopCount((uint)(res)) >> 2;
                            //   var nptr = ptr + co;
                            Array.Copy(arr, co, arr, co + 1, i - co);
                            arr[co] = curr;
                            break;
                        }
                        else
                        {
                            var sv = Avx2.LoadVector256(ptr + j);
                            var res = Avx2.MoveMask((Avx2.CompareGreaterThan(v2, sv)).AsByte());
                            var co = Popcnt.PopCount((uint)(res)) >> 2;
                            var jco = co + j;
                            // var nptr = ptr + jco;
                            if (co > 0)
                            {
                                //  MoveMemory(nptr + 1, nptr, (ulong)(4 * (i - jco)));
                                // Buffer.MemoryCopy(ptr + jco, ptr + jco + 1, 32, 32);
                                Array.Copy(arr, jco, arr, jco + 1, i - jco);
                                arr[jco] = curr;
                                break;
                            }
                            j -= 8;
                        }
                    }


                }

            }


        }
        /// <summary>         
        /// <tt>dual: ❌</tt><br/>
        /// <tt>unsafe: ❌</tt><br/>
        /// </summary>
        public static void InsertionSortM(ref int[] arr, int left, int right)
        {
            for (int i = left + 1; i < right + 1; i++)
            {
                int curr = arr[i];
                int j = i - 1;
                while (j > -1 && arr[j] > curr)
                {
                    arr[j + 1] = arr[j];
                    j--;
                }
                arr[j + 1] = curr;
            }
        }
        public static void InsertionSortMO(ref int[] arr, int left, int right)
        {
            if (arr[left] > arr[left + 1])
            {
                Swap(ref arr, left, left + 1);
            }
            for (int i = left + 2; i < right + 1; i++)
            {
                int curr = arr[i];
                int j = i - 1;
                if (curr < arr[left])
                {
                    while (j > -1 )
                    {
                        arr[j + 1] = arr[j];
                        j--;
                    }
                }
                else
                {
                    while (arr[j] > curr)
                    {
                        arr[j + 1] = arr[j];
                        j--;
                    }
                }
             
              
                arr[j + 1] = curr;
            }
        }
    }
}
