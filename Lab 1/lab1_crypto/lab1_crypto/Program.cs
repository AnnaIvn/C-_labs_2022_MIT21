// lab1 part 2, using RNGCryptoServiceProvider
//Ivanytska Anna, MIT21
using System.Security.Cryptography;

namespace lab1_random_num
{
    class Program
    {
        static void Main(string[] args)
        {
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider()) // class? that we are using, об`явлення
            {
                byte[] data = new byte[4]; // buffer storage

                Console.WriteLine("");
                Console.WriteLine("   10 Random numbers using RNGCryptoServiceProvider were generated ...");
                Console.WriteLine("--------------------------------------------------------------------------");

                for (int i = 0; i < 10; i++)
                {
                    rng.GetBytes(data); // fill the buffer

                    int value = BitConverter.ToInt16(data, 0); // convert to int 16
                    Console.WriteLine(value); // вивід
                }
            }
        }
    }
}

