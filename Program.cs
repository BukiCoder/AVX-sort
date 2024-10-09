using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Security;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Configs;
using System.Security.Cryptography;


namespace BASM
{

    public class Test
    {
        static int GetNextInt32(RNGCryptoServiceProvider rnd)
        {
            byte[] randomInt = new byte[4];
          
            rnd.GetBytes(randomInt);
            int res = BitConverter.ToInt32(randomInt, 0);
            return ((res < 0)? ((-res)/2) : res);
        }
        List<int[]> pp1;
        List<int[]> pp2;
      
        [GlobalSetup]
        public void Setup()
        {
            List<int> pp = new List<int>();
            pp1 = new List<int[]>();
            pp2 = new List<int[]>();
            Random rand = new Random();
             static void Shuffle<T>(Random rng, List<T> array)
            {
                int n = array.Count;
                while (n > 1)
                {
                    int k = rng.Next(n--);
                    T temp = array[n];
                    array[n] = array[k];
                    array[k] = temp;
                }
            }
            for (int i = 10000; i < 70000; i += 5000)
            { 
                for (int j = 0; j < i; j++)
                {
                   
                   

                    pp.Add(j);

                 //   Console.WriteLine(pp[j]);
                }
                Shuffle<int>(rand, pp);
                Shuffle<int>(rand, pp);
             
                pp1.Add(pp.ToArray());
                pp2.Add(pp.ToArray());
                pp.Clear();
            }
        }

       
        [Benchmark(Description = "D"), BenchmarkDotNet.Attributes.GcForce(false), BenchmarkDotNet.Attributes.GcConcurrent(false)]
        public void Test100()
        {
            Stopwatch st = new Stopwatch();
            st.Start();
            for (int i = 0; i < pp1.Count; i++)
            {
                int[] t = pp1[i];
                InsertionSortAlgorithms.InsertionSortM(ref t, 0, t.Length-1);
            }
            st.Stop();
            Console.WriteLine(st.ElapsedMilliseconds);
        }

        [Benchmark(Description = "O"), BenchmarkDotNet.Attributes.GcForce(false), BenchmarkDotNet.Attributes.GcConcurrent(false)]
        public void Test200()
        {
            Stopwatch st = new Stopwatch();
            st.Start();
            for (int i = 0; i < pp1.Count; i++)
            {
                int[] t = pp2[i];
                QuickSortingAlgorithms.QuickSortM(ref t, 0, t.Length - 1);
            }
            st.Stop();
            Console.WriteLine(st.ElapsedMilliseconds);
        }
    }
        internal class Program
        {
           
            static void TestMethod(Action<int[], int, int> method)
            {
                List<int> pp = new List<int>();
                Random rand = new Random();
                for (int i = 10000; i < 40000; i += 5000)
                {
                    for (int j = 0; j < i; j++)
                    {
                        pp.Add(rand.Next(2, 200000));
                    }
                    var arr = pp.ToArray();
                    method.Invoke(arr, 0, arr.Length - 1);
                    pp.Sort();
                    for (int j = 0; j < i; j++)
                    {
                        if (pp[j] != arr[j])
                        {
                            int start = (j - 7) > 0 ? (j - 7) : 0;
                            Console.ForegroundColor = ConsoleColor.Green;
                            for (int p = start; p < j; p++)
                            {
                                Console.Write(arr[p] + " ");
                            }
                            int end = (j + 7) < arr.Length ? (j + 7) : arr.Length - 1;
                            for (int p = j; p < end; p++)
                            {
                                Console.ForegroundColor = pp[p] == arr[p] ? ConsoleColor.Green : ConsoleColor.Red;
                                Console.Write(arr[p] + " ");
                            }
                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.Green;
                            for (int p = start; p < end; p++)
                            {
                                // Console.ForegroundColor = pp[p] == arr[p] ? ConsoleColor.Green : ConsoleColor.Red;
                                Console.Write(pp[p] + " ");
                            }
                            Console.WriteLine();
                            Console.WriteLine("index: " + j);
                            break;
                        }
                    }
                }

            }
            static void TestMethod(Action<int[]> method)
            {
                List<int> pp = new List<int>();
                Random rand = new Random();
                for (int i = 10000; i < 40000; i += 5000)
                {
                    for (int j = 0; j < i; j++)
                    {
                        pp.Add(rand.Next(2, 200000));
                    }
                    var arr = pp.ToArray();
                    method.Invoke(arr);
                    pp.Sort();
                    for (int j = 0; j < i; j++)
                    {
                        if (pp[j] != arr[j])
                        {
                            int start = (j - 7) > 0 ? (j - 7) : 0;
                            Console.ForegroundColor = ConsoleColor.Green;
                            for (int p = start; p < j; p++)
                            {
                                Console.Write(arr[p] + " ");
                            }
                            int end = (j + 7) < arr.Length ? (j + 7) : arr.Length - 1;
                            for (int p = j; p < end; p++)
                            {
                                Console.ForegroundColor = pp[p] == arr[p] ? ConsoleColor.Green : ConsoleColor.Red;
                                Console.Write(arr[p] + " ");
                            }
                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.Green;
                            for (int p = start; p < end; p++)
                            {
                                // Console.ForegroundColor = pp[p] == arr[p] ? ConsoleColor.Green : ConsoleColor.Red;
                                Console.Write(pp[p] + " ");
                            }
                            Console.WriteLine();
                            Console.WriteLine("index: " + j);
                            break;
                        }
                    }
                    pp.Clear();
                }

            }

            static void Bench(Action<int[]> method1, Action<int[]> method2)
            {
                List<int> pp = new List<int>();
                List<int[]> pp1 = new List<int[]>();
                Random rand = new Random();

                for (int i = 10000; i < 40000; i += 5000)
                {
                    for (int j = 0; j < i; j++)
                    {
                        pp.Add(rand.Next(2, 200000));
                    }
                    pp1.Add(pp.ToArray());
                    pp.Clear();
                }
                Stopwatch st = new Stopwatch();
                st.Start();
                foreach (var item in pp1)
                {
                    method1.Invoke(item);
                }
                st.Stop();
                Console.WriteLine("1: " + st.ElapsedMilliseconds);
                Stopwatch st1 = new Stopwatch();
                st1.Start();
                foreach (var item in pp1)
                {
                    method2.Invoke(item);
                }
                st1.Stop();
                Console.WriteLine("2: " + st1.ElapsedMilliseconds);
            }
            static void Main(string[] args)
            {

                var summary = BenchmarkRunner.Run<Test>( BenchmarkDotNet.Configs.ManualConfig.Create(DefaultConfig.Instance).WithOption(ConfigOptions.DisableOptimizationsValidator, true));
                void Swap(ref int[] arr, int i, int j)
                {
                    int temp = arr[i];
                    arr[i] = arr[j];
                    arr[j] = temp;
                }

                void StoogeSort(ref int[] arr, int left, int right) //блуждающая
                {
                    if (arr[left] > arr[right]) Swap(ref arr, left, right);
                    if ((left + 1) >= right) return;
                    int k = (int)((right - left + 1) / 3);
                    StoogeSort(ref arr, left, right - k);
                    StoogeSort(ref arr, left + k, right);
                    StoogeSort(ref arr, left, right - k);
                }
                void SlowSort(ref int[] arr, int left, int right) //вялая
                {
                    if (left >= right) return;
                    int p = (int)((right + left) / 2);
                    SlowSort(ref arr, left, p);
                    SlowSort(ref arr, p + 1, right);
                    if (arr[p] > arr[right]) Swap(ref arr, p, right);
                    SlowSort(ref arr, left, right - 1);

                }


                void ShellSortSimple(ref int[] arr)
                {
                    for (int step = arr.Length >> 1; step > 0; step = step / 2)
                    {
                        for (int i = step; i < arr.Length; i++)
                        {

                            for (int j = i - step; j >= 0 && arr[j] > arr[j + step]; j -= step)
                            {
                                Swap(ref arr, j, j + step);


                            }
                        }
                    }
                }
                //https://codelab.ru/task/shell_sort:optimization/
                unsafe void ShellSortSE(ref int[] arr)
                {
                    var incs = stackalloc int[50];
                    int s = Increment(arr.Length, incs);
                    while (s >= 0)
                    {
                        int step = incs[s];
                        s--;
                        for (int i = step; i < arr.Length; i++)
                        {

                            for (int j = i - step; (j >= 0) && (arr[j] > arr[j + step]); j -= step)
                            {
                                Swap(ref arr, j, j + step);
                            }

                        }
                    }
                    InsertionSortAlgorithms.InsertionSortM(ref arr, 0, arr.Length - 1);

                }
                unsafe int Increment(int size, int* inc)
                {
                    int p1, p2, p3, s;

                    p1 = p2 = p3 = 1;
                    s = -1;
                    do
                    {
                        if (++s % 2 == 0)
                        {
                            inc[s] = (8 * p1 - 6 * p2 + 1);
                        }
                        else
                        {
                            inc[s] = (9 * p1 - 9 * p3 + 1);
                            p2 *= 2;
                            p3 *= 2;
                        }
                        p1 *= 2;
                    } while (3 * inc[s] < size);

                    return s > 0 ? s - 1 : 0;
                }


                int[] mass = new int[] { 1, 2, 777, 8, 4, 8, 3004, 6666666, 800, 3339, 23, 52, 20, 399, 29, 2, 8887, 300, 15, 0, 433535, 67899, 686452, 67575 };
                // DualInsertionSort(ref mass);
                InsertionSortAlgorithms.InsertionSortU(ref mass);
                foreach (var item in mass)
                {
                    Console.WriteLine(item);
                }
                //Stopwatch st = new Stopwatch();
                List<int> pp = new List<int>();
                Random rand = new Random();
                for (int i = 0; i < 8100000; i++)
                {
                    pp.Add(rand.Next(2, 200000));
                }
                Console.WriteLine(Popcnt.PopCount(~((uint)(17))));
                var tt = pp.ToArray();
                var tt1 = pp.ToArray();
                var tt2 = pp.ToArray();
                var tt3 = pp.ToArray();
                var tt4 = pp.ToArray();
                var tt5 = pp.ToArray();
                Console.WriteLine(1000 + (3 >> 1));
                List<int> pp1 = new List<int>();

                Console.WriteLine();
                Console.WriteLine();
                foreach (var item in pp1)
                {
                    Console.WriteLine(item);
                }
                Stopwatch st = new Stopwatch();
                st.Start();
                ShellSortSE(ref tt);
                st.Stop();
                Console.WriteLine("bsort: " + st.ElapsedMilliseconds);
                st.Reset();
                st.Start();
                ShellSortSimple(ref tt2);
                st.Stop();
                Console.WriteLine("bsort: " + st.ElapsedMilliseconds);
                st.Reset();


                for (int i = 0; i < tt.Length; i++)
                {
                    if (tt2[i] != tt[i])
                    {
                         Console.WriteLine(i);
                    }
                }


            }
        }

    
}