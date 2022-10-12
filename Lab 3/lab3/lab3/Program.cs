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
                Console.WriteLine("3 - part 3, hashing with hmac");
                Console.WriteLine("4 - part 4, registrating a user + authentification");
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
                
                else if (option == "3")
                {
                    int amountSHA1 = 20;       // length of keys for each method on heshing; 20 bytes
                    int amountSHA256 = 32;     // 32 bytes
                    int amountSHA512 = 64;     // 64 bytes

                    // getting message that needs to be hashed
                    Console.Write("Enter message for which you want a hash codes to be calculated ->  ");
                    string message = Console.ReadLine();

                    Console.WriteLine("");
                    Console.WriteLine("Hashing your message . . .");
                    Console.WriteLine("------------------------------------------------------------------------------------------------------------------------");
                    byte[] messageInArray = Encoding.Unicode.GetBytes(message);        // put string in byte array (масив)

                    // making keys
                    byte[] keySHA1 = cryptoKey(amountSHA1);
                    byte[] keySHA256 = cryptoKey(amountSHA256);
                    byte[] keySHA512 = cryptoKey(amountSHA512);


                    // hashing
                    //var HmacSHA1 = ComputeHmacSHA1(Encoding.Unicode.GetBytes(message, keySHA1)); // or we can write like this
                    var HmacSHA1 = ComputeHmacSHA1(messageInArray, keySHA1);
                    var HmacSHA256 = ComputeHmacSHA256(messageInArray, keySHA256);  // call functions
                    var HmacSHA512 = ComputeHmacSHA512(messageInArray, keySHA512);

                    Console.WriteLine($"HMAC SHA1:{Convert.ToBase64String(HmacSHA1)}"); // convert to string (to display)
                    Console.ReadLine();
                    Console.WriteLine($"HMAC SHA256:{Convert.ToBase64String(HmacSHA256)}");
                    Console.ReadLine();
                    Console.WriteLine($"HMAC SHA512:{Convert.ToBase64String(HmacSHA512)}");
                    Console.WriteLine("");


                    // making other hashes
                    Console.WriteLine("Making anothed hash (for cheking)  . . .");
                    Console.WriteLine("------------------------------------------------------------------------------------------------------------------------");

                    var Hmac2SHA1 = ComputeHmacSHA1(messageInArray, keySHA1);
                    var Hmac2SHA256 = ComputeHmacSHA256(messageInArray, keySHA256);  // call functions
                    var Hmac2SHA512 = ComputeHmacSHA512(messageInArray, keySHA512);

                    Console.WriteLine($"HMAC SHA1 (second hashing):{Convert.ToBase64String(Hmac2SHA1)}"); // convert to string (to display)
                    Console.ReadLine();
                    Console.WriteLine($"HMAC SHA256 (second hashing):{Convert.ToBase64String(Hmac2SHA256)}");
                    Console.ReadLine();
                    Console.WriteLine($"HMAC SHA512 (second hashing):{Convert.ToBase64String(Hmac2SHA512)}");
                    Console.WriteLine("");


                    // checking
                    Console.WriteLine("Checking your hashed message for accuracy (comparing hashes)  . . .");
                    Console.WriteLine("------------------------------------------------------------------------------------------------------------------------");

                    Console.Write("SHA1 check: ");
                    CheckHash(HmacSHA1, Hmac2SHA1);
                    Console.ReadLine();
                    Console.Write("SHA256 check: ");
                    CheckHash(HmacSHA256, Hmac2SHA256);
                    Console.ReadLine();
                    Console.Write("SHA512 check: ");
                    CheckHash(HmacSHA512, Hmac2SHA512);
                    Console.ReadLine();
                    Console.WriteLine("");
                    Console.WriteLine("");

                }
                
                else if (option == "4")
                {
                    // length of keys for each method of hashing
                    int amountSHA1 = 20, amountSHA256 = 32, amountSHA512 = 64;

                    // initializing passwords, usernames
                    string username, password, usernameCheck, passwordCheck;

                    // initializing salt(key) arrays
                    byte[] keySHA1, keySHA256, keySHA512, passwordInArray;


                    // user initialisation
                    Console.WriteLine();
                    Console.WriteLine("Starting user registration ");
                    Console.WriteLine("------------------------------------------------------------------------------------------------------------------------");
                    Console.Write("Enter your username ->  ");
                    username = Console.ReadLine();                          // getting username
                    Console.Write("Enter your password ->  ");
                    password = Console.ReadLine();                          // getting password
                    Console.WriteLine();
                    Console.WriteLine();

                    passwordInArray = Encoding.Unicode.GetBytes(password);  // putting password in array

                    // making keys
                    keySHA1 = cryptoKey(amountSHA1);
                    keySHA256 = cryptoKey(amountSHA256);
                    keySHA512 = cryptoKey(amountSHA512);

                    // hashing
                    //var HmacSHA1 = ComputeHmacSHA1(Encoding.Unicode.GetBytes(message, keySHA1)); // or we can write like this
                    var HmacSHA1 = ComputeHmacSHA1(passwordInArray, keySHA1);
                    var HmacSHA256 = ComputeHmacSHA256(passwordInArray, keySHA256);  // call functions
                    var HmacSHA512 = ComputeHmacSHA512(passwordInArray, keySHA512);

                    Console.WriteLine("Your hashed using ...");       // output
                    Console.WriteLine("------------------------------------------------------------------------------------------------------------------------");
                    Console.WriteLine();
                    Console.WriteLine("SHA1: " + Convert.ToBase64String(HmacSHA1));
                    Console.ReadLine();                                                                                // for delay
                    Console.WriteLine("SHA256: " + Convert.ToBase64String(HmacSHA256));
                    Console.ReadLine();
                    Console.WriteLine("SHA512:  " + Convert.ToBase64String(HmacSHA512));
                    Console.ReadLine();
                    Console.WriteLine();


                    // starting user authentification
                    Console.WriteLine("Starting user authentification ");
                    Console.WriteLine("------------------------------------------------------------------------------------------------------------------------");
                    Console.Write("Enter your username ->  ");
                    usernameCheck = Console.ReadLine();               // getting username
                    Console.Write("Enter your password ->  ");
                    passwordCheck = Console.ReadLine();               // getting password

                    var Hmac2SHA1 = ComputeHmacSHA1(passwordInArray, keySHA1);
                    var Hmac2SHA256 = ComputeHmacSHA256(passwordInArray, keySHA256);  // call functions
                    var Hmac2SHA512 = ComputeHmacSHA512(passwordInArray, keySHA512);

                    Console.WriteLine("Your hashed password using ...");    // output
                    Console.WriteLine("------------------------------------------------------------------------------------------------------------------------");
                    Console.WriteLine();
                    Console.WriteLine("SHA1: " + Convert.ToBase64String(Hmac2SHA1));
                    Console.ReadLine();                                                                                // for delay
                    Console.WriteLine("SHA256: " + Convert.ToBase64String(Hmac2SHA256));
                    Console.ReadLine();
                    Console.WriteLine("SHA512:  " + Convert.ToBase64String(Hmac2SHA512));
                    Console.ReadLine();
                    Console.WriteLine();


                    // checking
                    Console.WriteLine("Checking your hashed password  ...");
                    Console.WriteLine("------------------------------------------------------------------------------------------------------------------------");
                    Console.ReadLine();

                    Console.Write("SHA1 check: ");
                    CheckHash(HmacSHA1, Hmac2SHA1);
                    Console.ReadLine();
                    Console.Write("SHA256 check: ");
                    CheckHash(HmacSHA256, Hmac2SHA256);
                    Console.ReadLine();
                    Console.Write("SHA512 check: ");
                    CheckHash(HmacSHA512, Hmac2SHA512);
                    Console.ReadLine();
                    Console.WriteLine("");
                    Console.WriteLine("");
                }

            } while (option != "0"); 
        }


        // checking hashes for accuracy
        public static int CheckHash(byte[] hash1, byte[] hash2)
        {
            if (Convert.ToBase64String(hash1) == Convert.ToBase64String(hash2))
            {
                Console.WriteLine("Hashes of message are accurate");
            }
            else
            {
                Console.WriteLine("Hashes aren`t accurate. Message is corrupted");
            }
            return 1;
        }


        // generating key
        public static byte[] cryptoKey(int amount)    
        {
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider()) // class, that we are using, об`явлення
            {
                var Key = new byte[amount];
                rng.GetBytes(Key);
                return Key;
            }
        }


        // hmac hash functions  (for options 3-4)
        public static byte[] ComputeHmacSHA1(byte[] toBeHashed, byte[] key)
        {
            using (var hmac = new HMACSHA1(key))
            {
                return hmac.ComputeHash(toBeHashed);
            }
        }

        public static byte[] ComputeHmacSHA256(byte[] toBeHashed, byte[] key)
        {
            using (var hmac = new HMACSHA256(key))
            {
                return hmac.ComputeHash(toBeHashed);
            }
        }

        public static byte[] ComputeHmacSHA512(byte[] toBeHashed, byte[] key)
        {
            using (var hmac = new HMACSHA512(key))
            {
                return hmac.ComputeHash(toBeHashed);
            }
        }


        // hash functions (for options 1-2)
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
