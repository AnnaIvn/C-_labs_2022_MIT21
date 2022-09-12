//lab1 part 1, generating random numbers
//Ivanytska Anna, MIT21
using System;

namespace lab1_random_num
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("");
            Console.WriteLine("                 20 Random numbers from -100 to 100 were generated ...");
            Console.WriteLine("----------------------------------------------------------------------------------------");

            Random random = new Random(); // to create random numbers, we don`t write seed so it`ll be diferent

            for (int i = 0; i < 20; i++)
            {
                Console.Write(random.Next(-100, 101)); // вивід + range of numbers to display: -100 - 100
                Console.Write("  ");
            }
            Console.WriteLine(" ");
            
        }
    }
}

