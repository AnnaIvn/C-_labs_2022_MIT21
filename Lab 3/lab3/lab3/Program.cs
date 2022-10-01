using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Runtime.Intrinsics.Arm;

namespace lab3
{
    class Program
    {
        static void Main(string[] args)
        {
            string option;

            do
            {
                Console.WriteLine("Options:");
                Console.WriteLine("1 - part 1, hashing using different algoritms");
                Console.WriteLine("2 - part 2, cracking using brute force");
                Console.WriteLine("0 - exit");
                Console.Write("Your option -> ");
                option = Console.ReadLine();

                if (option == "1")                                                   // part 1 of lab 3
                {
                    Console.Write("Enter message for which you want a hash codes to be calculated ->  ");
                    string message = Console.ReadLine();
                    Console.WriteLine("");
                    Console.WriteLine("Hashing your message (" + message + ") . . .");
                    Console.WriteLine("------------------------------------------------------------------------------------------------------------------------");

                    byte[] messageInArray = Encoding.Unicode.GetBytes(message);        // !!!! put string in byte array (масив)

                    var md5ForStr = ComputeHashMd5(messageInArray); // визиваємо функцію
                    var SHA1ForStr = ComputeHashSHA1(Encoding.Unicode.GetBytes(message)); // or we can write like this
                    var SHA256ForStr = ComputeHashSHA256(messageInArray);
                    var SHA384ForStr = ComputeHashSHA384(messageInArray);
                    var SHA512ForStr = ComputeHashSHA512(messageInArray);

                    Console.WriteLine($"Hash MD5:{Convert.ToBase64String(md5ForStr)}"); // convert to string (to display)
                    Console.WriteLine($"Hash SHA1:{Convert.ToBase64String(SHA1ForStr)}");
                    Console.WriteLine($"Hash SHA256:{Convert.ToBase64String(SHA256ForStr)}");
                    Console.WriteLine($"Hash SHA384:{Convert.ToBase64String(SHA384ForStr)}");
                    Console.WriteLine($"Hash SHA512:{Convert.ToBase64String(SHA512ForStr)}");
                    Console.WriteLine("");

                }
                else if (option == "2")                                                  // decription, the same
                {

                    String correctHash = "po1MVkAE7IjUUwu61XxgNg==";
                    String attemptHash = "";

                    int first = 0;
                    int second = 0;
                    int third = 0;
                    int fourth = 0;
                    int fifth = 0;
                    int sixth = 0;
                    int seventh = 0;
                    int eighth = 0;
                    int cracks = 0;

                    // an array with numbers
                    string[] array = new string[10];
                    array[0] = "0";
                    array[1] = "1";
                    array[2] = "2";
                    array[3] = "3";
                    array[4] = "4";
                    array[5] = "5";
                    array[6] = "6";
                    array[7] = "7";
                    array[8] = "8";
                    array[9] = "9";

                    //start cracking
                    while (!attemptHash.Equals(correctHash))
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
                            sixth++;
                            fifth = 0;
                        }

                        if (sixth == array.Length)
                        {
                            seventh++;
                            sixth = 0;
                        }

                        if (seventh == array.Length)
                        {
                            eighth++;
                            seventh = 0;
                        }

                        if (eighth == array.Length)
                        {
                            break; //finished
                        }

                        string attempt = array[eighth] + array[seventh] + array[sixth] + array[fifth]
                            + array[fourth] + array[third] + array[second] + array[first];

                        byte[] attemptInArray = Encoding.Unicode.GetBytes(attempt);
                        attemptInArray = ComputeHashMd5(attemptInArray);
                        attemptHash = Convert.ToBase64String(attemptInArray);

                        Console.WriteLine(attempt + "         " + attemptHash);
                        first++;
                        cracks++;
                    }
                    Console.WriteLine("> Attempts to crack: " + cracks);
                    Console.ReadLine(); // to make it stay after finish

                    // password is -> 20192020
                }

            } while (option != "0"); 

        }

        static byte[] ComputeHashMd5(byte[] messageInArray)
        {
            using (var md5 = MD5.Create())
            {
                return md5.ComputeHash(messageInArray);
            }
        }

        static byte[] ComputeHashSHA1(byte[] messageInArray)
        {
            using (var sha1 = SHA1.Create())
            {
                return sha1.ComputeHash(messageInArray);
            }
        }

        static byte[] ComputeHashSHA256(byte[] messageInArray)
        {
            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(messageInArray);
            }
        }

        static byte[] ComputeHashSHA384(byte[] messageInArray)
        {
            using (var sha384 = SHA384.Create())
            {
                return sha384.ComputeHash(messageInArray);
            }
        }

        static byte[] ComputeHashSHA512(byte[] messageInArray)
        {
            using (var sha512 = SHA512.Create())
            {
                return sha512.ComputeHash(messageInArray);
            }
        }
    }
}
