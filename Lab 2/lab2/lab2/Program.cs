using System;
using System.Text;
using System.Security.Cryptography;

namespace lab2
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Console.WriteLine("Options:");
            Console.WriteLine("1 - encode file");
            Console.WriteLine("2 - decode file");
            Console.WriteLine("");
            Console.Write("Your option -> ");

            string option = Console.ReadLine();
            string path = "D:/Lessons/2nd Year/Основи інформаційної безпеки/Labs/Lab 2/lab2/lab2/";

            if (option == "1")                                                       // encryption
            {
                byte[] data = File.ReadAllBytes(path + "file.txt").ToArray();        // read file and put all in array (масив)

                int length = data.Length;                                            // length of data in array
                byte key = Convert.ToByte(length);                                   // key = length, converted into byte

                byte[] encrLength = new byte[length];                                // encrLength = key in array
                
                for (int i = 0; i < length; i++)                                     // while length > 0
                {
                    encrLength[i] = (byte)(data[i] ^ length);                        // encryntion
                }
                File.WriteAllBytes(path + "file.dat", encrLength);                   // writing in new file
            }
            else if (option == "2")                                                  // decription, the same
            {
                byte[] data = File.ReadAllBytes(path + "file.dat").ToArray();

                int length = data.Length;
                byte key = Convert.ToByte(length);

                byte[] encrLength = new byte[length];
                for (int i = 0; i < length; i++)
                {
                    encrLength[i] = (byte)(data[i] ^ length);
                }
                File.WriteAllBytes(path + "file.txt", encrLength);

            }
        }
    }
}


