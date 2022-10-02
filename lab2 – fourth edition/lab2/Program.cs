using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Runtime.Intrinsics.Arm;
using System.ComponentModel;

namespace lab2
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Console.WriteLine("Options:");
            Console.WriteLine("1 - encode file");
            Console.WriteLine("2 - decode file");
            Console.WriteLine("3 - part three of task");
            Console.Write("Your option -> ");

            string option = Console.ReadLine();
            string path = "D:/Lessons/2nd Year/Основи інформаційної безпеки/Labs/Lab 2/lab2 – fourth edition/lab2/";

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
                Console.WriteLine("");
                Console.WriteLine("Your file was encoded");
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
                Console.WriteLine("");
                Console.WriteLine("Your file was decoded");
            }
            else if (option == "3")
            {
                Console.WriteLine("Looking for correct message, having it`s encrypted version ...");

                // already in bits
                String correctEncr = "?\vg\tW2\ff\tW9,F\u0017FTH8b\u001b\u001b7K\u0005\u0011\u001b7\u0012p\u001c\u0006$[K\u0012Uee@W\u0003,^IW\u0003,\\\vW9,F\bEEe\u001f\u0005\u0003\u001c \u0012G\u0012\a1\u0013";
                String attemptDecr = "";

                int first = 0;
                int second = 0;
                int third = 0;
                int fourth = 0;
                int fifth = 0;
                int cracks = 0;

                // an array with numbers
                string[] array = new string[26];
                array[0] = "a";
                array[1] = "b";
                array[2] = "c";
                array[3] = "d";
                array[4] = "e";
                array[5] = "f";
                array[6] = "g";
                array[7] = "h";
                array[8] = "i";
                array[9] = "g";
                array[10] = "k";
                array[11] = "l";
                array[12] = "m";
                array[13] = "n";
                array[14] = "o";
                array[15] = "p";
                array[16] = "q";
                array[17] = "r";
                array[18] = "s";
                array[19] = "t";
                array[20] = "u";
                array[21] = "v";
                array[22] = "w";
                array[23] = "x";
                array[24] = "y";
                array[25] = "z";

                //int length = correctEncr.Length;                 // length
                //byte correctEncr = Convert.ToByte(length);     // do not need
                //byte[] attemptInArray = new byte[length];         // creating new array
                //attemptInArray = correctEncr.ToCharArray;         // into array 
                // ??????? how to put into array

                //start cracking
                while (!attemptDecr.Equals(correctEncr))
                {
                    if (first == array.Length)
                    {
                        second++;
                        first = 0;
                    }

                    if (second == array.Length)
                    {
                        third++;
                        second = 0;
                    }

                    if (third == array.Length)
                    {
                        fourth++;
                        third = 0;
                    }

                    if (fourth == array.Length)
                    {
                        fifth++;
                        fourth = 0;
                    }

                    if (fifth == array.Length)
                    {
                        break;
                    }

                    string attempt = array[fifth] + array[fourth] + array[third] + array[second] + array[first];

                    byte[] attemptInArray = Encoding.Unicode.GetBytes(attempt);
                    int length = attemptInArray.Length;

                    for (int i = 0; i < length; i++)
                    {
                        attemptInArray[i] = (byte)(correctEncr[i] ^ length);
                    }

                    attemptDecr = Convert.ToBase64String(attemptInArray);
                    //attemptDecr = Convert.ToUInt16(attemptInArray);        треба якось перевести до одного вигляду, щоб порівняти

                    Console.WriteLine(attempt + "         " + attemptDecr);
                    first++;
                    cracks++;
                }
                Console.WriteLine("> Attempts to crack: " + cracks);
                Console.ReadLine(); // to make it stay after finish






            }





        }
    }
}


