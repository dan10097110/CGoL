using System;
using System.Diagnostics;
using System.Threading;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace CGoL
{
    class Program
    {
        const int size = 60;

        static void Main(string[] args)
        {
            Simulate(Next);
            int[] world = new int[size * size];
            Random(world, 0.5);
            Console.WriteLine(Bench(Next, world, 100));
            Console.WriteLine(Bench(Next, world, 100));
            Console.WriteLine(Bench(Next, world, 100));
            Console.WriteLine(Bench(Next, world, 100));
            Console.ReadKey();
        }

        static void Next(int[] world)
        {
            for (int i = 0; i < world.Length; i++)
                if (world[i] > 8)
                {
                    if (i % size > 0)
                    {
                        if (i - 1 - size >= 0)
                            world[i - 1 - size]++;
                        if (i - 1 >= 0)
                            world[i - 1]++;
                        if (i - 1 + size < world.Length)
                            world[i - 1 + size]++;
                    }
                    if (i % size < size - 1)
                    {
                        if (i + 1 - size >= 0)
                            world[i + 1 - size]++;
                        if (i + 1 < world.Length)
                            world[i + 1]++;
                        if (i + 1 + size < world.Length)
                            world[i + 1 + size]++;
                    }
                    if (i - size >= 0)
                        world[i - size]++;
                    if (i + size < world.Length)
                        world[i + size]++;
                }
            for (int i = 0; i < world.Length; i++)
                if (world[i] > 8)
                    world[i] = Neighbours(i, 2, 3) ? 9 : 0;
                else
                    world[i] = Neighbours(i, 3) ? 9 : 0;

            bool Neighbours(int i, params int[] c)
            {
                foreach (int j in c)
                    if (world[i] % 9 == j)
                        return true;
                return false;
            }
        }

        static void Simulate(Action<int[]> Next)
        {
            int[] world = new int[size * size];
            Random(world, 0.3);
            for (; ; )
            {
                Print(world);
                Thread.Sleep(250);
                Next(world);
            }
        }

        static long Bench(Action<int[]> Next, int[] world, int generations)
        {
            int[] copy = new int[size * size];
            Array.Copy(world, copy, size * size);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            NextN(copy, Next, generations);
            return sw.ElapsedMilliseconds;
        }

        static void NextN(int[] world, Action<int[]> Next, int n)
        {
            for (int i = 0; i < n; i++)
                Next(world);
        }

        static StringBuilder s = new StringBuilder(size * size + 2 * size, size * size + 2 * size);

        static void Print(int[] world)
        {
            s.Clear();
            for (int i = 0; i < world.Length; i++)
            {
                s.Append(world[i] == 9 ? 'x' : ' ');
                if(i % size == size - 1)
                    s.AppendLine();
            }
            Console.Clear();
            Console.WriteLine(s);
        }

        static Random random = new Random();

        static void Random(int[] world, double density)
        {
            double d = density * 1000;
            for (int i = 0; i < world.Length; i++)
                world[i] = random.Next(0, 1000) < d ? 9 : 0;
        }
    }
}
