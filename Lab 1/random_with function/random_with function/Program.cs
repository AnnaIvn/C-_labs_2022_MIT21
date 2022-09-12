// like lab1_random_num but in other form
//Ivanytska Anna, MIT21
using System;

namespace lab1_random_num
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("");
            Console.WriteLine("       20 Random numbers from 0 to 20 were generated ...");
            Console.WriteLine("-------------------------------------------------------------------------");

            Random random = new Random(); // to create random numbers, we don`t write seed so it`ll be diferent

            for (int i = 0; i < 20; i++)
            {
                //Console.Write(random.Next(-100, 100));
                getRandom(random); // call for function
                Console.Write("  ");
            }
            Console.WriteLine(" ");

        }

        static void getRandom(Random random) // write (pass in) Random random here, not in the function
        {
            Console.Write(random.Next(21)); // from 0 to 20
        }
    }
}



