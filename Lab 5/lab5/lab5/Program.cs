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
                Console.WriteLine("1 - part 1, hashing with salt");
                Console.WriteLine("2 - part 2, hashing with salt + iterations");
                Console.WriteLine("0 - exit");
                Console.Write("Your option -> ");
                option = Console.ReadLine();

                if (option == "1")
                {
                    // length of keys for each method of hashing
                    int saltLengthMD5 = 16, saltLengthSHA1 = 20, saltLengthSHA256 = 32, 
                        saltLengthSHA384 = 48, saltLengthSHA512 = 64;

                    // initializing passwords, usernames
                    string username, password, usernameCheck, passwordCheck;

                    // initializing salt(key) arrays
                    byte[] saltMD5, saltSHA1, saltSHA256, saltSHA384, saltSHA512, passwordInArray;


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
                    saltMD5 = cryptoKey(saltLengthMD5);
                    saltSHA1 = cryptoKey(saltLengthSHA1);
                    saltSHA256 = cryptoKey(saltLengthSHA256);
                    saltSHA384 = cryptoKey(saltLengthSHA384);
                    saltSHA512 = cryptoKey(saltLengthSHA512);

                    // hashing
                    //var HmacSHA1 = ComputeHmacSHA1(Encoding.Unicode.GetBytes(message, keySHA1)); // or we can write like this
                    var HmacMD5 = SaltedHash.ComputeHmacSHA1(passwordInArray, saltMD5);
                    var HmacSHA1 = SaltedHash.ComputeHmacSHA1(passwordInArray, saltSHA1);
                    var HmacSHA256 = SaltedHash.ComputeHmacSHA256(passwordInArray, saltSHA256);  // call functions
                    var HmacSHA384 = SaltedHash.ComputeHmacSHA384(passwordInArray, saltSHA384);
                    var HmacSHA512 = SaltedHash.ComputeHmacSHA512(passwordInArray, saltSHA512);

                    Console.WriteLine("Your hashed password using ...");       // output
                    Console.WriteLine("------------------------------------------------------------------------------------------------------------------------");
                    Console.WriteLine();
                    Console.WriteLine("MD5: " + Convert.ToBase64String(HmacMD5));
                    Console.ReadLine();
                    Console.WriteLine("SHA1: " + Convert.ToBase64String(HmacSHA1));
                    Console.ReadLine();                                                                                // for delay
                    Console.WriteLine("SHA256: " + Convert.ToBase64String(HmacSHA256));
                    Console.ReadLine();
                    Console.WriteLine("SHA384: " + Convert.ToBase64String(HmacSHA384));
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

                    var Hmac2MD5 = SaltedHash.ComputeHmacSHA1(passwordInArray, saltMD5);
                    var Hmac2SHA1 = SaltedHash.ComputeHmacSHA1(passwordInArray, saltSHA1);
                    var Hmac2SHA256 = SaltedHash.ComputeHmacSHA256(passwordInArray, saltSHA256);  // call functions
                    var Hmac2SHA384 = SaltedHash.ComputeHmacSHA384(passwordInArray, saltSHA384);
                    var Hmac2SHA512 = SaltedHash.ComputeHmacSHA512(passwordInArray, saltSHA512);

                    Console.WriteLine("Your hashed password using ...");       // output
                    Console.WriteLine("------------------------------------------------------------------------------------------------------------------------");
                    Console.WriteLine();
                    Console.WriteLine("MD5: " + Convert.ToBase64String(Hmac2MD5));
                    Console.ReadLine();
                    Console.WriteLine("SHA1: " + Convert.ToBase64String(Hmac2SHA1));
                    Console.ReadLine();                                                                                // for delay
                    Console.WriteLine("SHA256: " + Convert.ToBase64String(Hmac2SHA256));
                    Console.ReadLine();
                    Console.WriteLine("SHA384: " + Convert.ToBase64String(Hmac2SHA384));
                    Console.ReadLine();
                    Console.WriteLine("SHA512:  " + Convert.ToBase64String(Hmac2SHA512));
                    Console.ReadLine();
                    Console.WriteLine();


                    // checking
                    Console.WriteLine("Checking your hashed password  ...");
                    Console.WriteLine("------------------------------------------------------------------------------------------------------------------------");
                    Console.ReadLine();

                    Console.Write("MD5 check: ");
                    CheckHash(HmacMD5, Hmac2MD5);
                    Console.ReadLine();
                    Console.Write("SHA1 check: ");
                    CheckHash(HmacSHA1, Hmac2SHA1);
                    Console.ReadLine();
                    Console.Write("SHA256 check: ");
                    CheckHash(HmacSHA256, Hmac2SHA256);
                    Console.ReadLine();
                    Console.Write("SHA384 check: ");
                    CheckHash(HmacSHA384, Hmac2SHA384);
                    Console.ReadLine();
                    Console.Write("SHA512 check: ");
                    CheckHash(HmacSHA512, Hmac2SHA512);
                    Console.ReadLine();
                    Console.WriteLine("");
                    Console.WriteLine("");
                }
                else if (option == "2")
                {
                    // length of keys for each method of hashing
                    int saltLengthMD5 = 16, saltLengthSHA1 = 20, saltLengthSHA256 = 32,
                        saltLengthSHA384 = 48, saltLengthSHA512 = 64;

                    // initializing passwords, usernames
                    string username, password, usernameCheck, passwordCheck;

                    // initializing salt(key) arrays
                    byte[] saltMD5, saltSHA1, saltSHA256, saltSHA384, saltSHA512, passwordInArray;


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
                    saltMD5 = cryptoKey(saltLengthMD5);
                    saltSHA1 = cryptoKey(saltLengthSHA1);
                    saltSHA256 = cryptoKey(saltLengthSHA256);
                    saltSHA384 = cryptoKey(saltLengthSHA384);
                    saltSHA512 = cryptoKey(saltLengthSHA512);

                    // hashing
                    //var HmacSHA1 = ComputeHmacSHA1(Encoding.Unicode.GetBytes(message, keySHA1)); // or we can write like this
                    var HmacMD5 = SaltedHash.ComputeHmacSHA1(passwordInArray, saltMD5);
                    var HmacSHA1 = SaltedHash.ComputeHmacSHA1(passwordInArray, saltSHA1);
                    var HmacSHA256 = SaltedHash.ComputeHmacSHA256(passwordInArray, saltSHA256);  // call functions
                    var HmacSHA384 = SaltedHash.ComputeHmacSHA384(passwordInArray, saltSHA384);
                    var HmacSHA512 = SaltedHash.ComputeHmacSHA512(passwordInArray, saltSHA512);

                    Console.WriteLine("Your hashed password using ...");       // output
                    Console.WriteLine("------------------------------------------------------------------------------------------------------------------------");
                    Console.WriteLine();
                    Console.WriteLine("MD5: " + Convert.ToBase64String(HmacMD5));
                    Console.ReadLine();
                    Console.WriteLine("SHA1: " + Convert.ToBase64String(HmacSHA1));
                    Console.ReadLine();                                                                                // for delay
                    Console.WriteLine("SHA256: " + Convert.ToBase64String(HmacSHA256));
                    Console.ReadLine();
                    Console.WriteLine("SHA384: " + Convert.ToBase64String(HmacSHA384));
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

                    var Hmac2MD5 = SaltedHash.ComputeHmacSHA1(passwordInArray, saltMD5);
                    var Hmac2SHA1 = SaltedHash.ComputeHmacSHA1(passwordInArray, saltSHA1);
                    var Hmac2SHA256 = SaltedHash.ComputeHmacSHA256(passwordInArray, saltSHA256);  // call functions
                    var Hmac2SHA384 = SaltedHash.ComputeHmacSHA384(passwordInArray, saltSHA384);
                    var Hmac2SHA512 = SaltedHash.ComputeHmacSHA512(passwordInArray, saltSHA512);

                    Console.WriteLine("Your hashed password using ...");       // output
                    Console.WriteLine("------------------------------------------------------------------------------------------------------------------------");
                    Console.WriteLine();
                    Console.WriteLine("MD5: " + Convert.ToBase64String(Hmac2MD5));
                    Console.ReadLine();
                    Console.WriteLine("SHA1: " + Convert.ToBase64String(Hmac2SHA1));
                    Console.ReadLine();                                                                                // for delay
                    Console.WriteLine("SHA256: " + Convert.ToBase64String(Hmac2SHA256));
                    Console.ReadLine();
                    Console.WriteLine("SHA384: " + Convert.ToBase64String(Hmac2SHA384));
                    Console.ReadLine();
                    Console.WriteLine("SHA512:  " + Convert.ToBase64String(Hmac2SHA512));
                    Console.ReadLine();
                    Console.WriteLine();


                    // checking
                    Console.WriteLine("Checking your hashed password  ...");
                    Console.WriteLine("------------------------------------------------------------------------------------------------------------------------");
                    Console.ReadLine();

                    Console.Write("MD5 check: ");
                    CheckHash(HmacMD5, Hmac2MD5);
                    Console.ReadLine();
                    Console.Write("SHA1 check: ");
                    CheckHash(HmacSHA1, Hmac2SHA1);
                    Console.ReadLine();
                    Console.Write("SHA256 check: ");
                    CheckHash(HmacSHA256, Hmac2SHA256);
                    Console.ReadLine();
                    Console.Write("SHA384 check: ");
                    CheckHash(HmacSHA384, Hmac2SHA384);
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
                Console.WriteLine("Hash of password is accurate");
            }
            else
            {
                Console.WriteLine("Hash isn`t accurate. Password is corrupted");
            }
            return 1;
        }


        // generating key
        public static byte[] cryptoKey(int seed)
        {
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider()) // class, that we are using, об`явлення
            {
                var Key = new byte[seed];
                rng.GetBytes(Key);
                return Key;
            }
        }



        // hash + salt functions for option 1
        public class SaltedHash    
        {
            private static byte[] Combine(byte[] first, byte[] second)
            {
                var ret = new byte[first.Length + second.Length];
                Buffer.BlockCopy(first, 0, ret, 0, first.Length);
                Buffer.BlockCopy(second, 0, ret, first.Length,
                second.Length);
                return ret;
            }

            public static byte[] ComputeHmacMD5(byte[] toBeHashed, byte[] salt)
            {
                using (var hmac = new HMACMD5(salt))
                {
                    return hmac.ComputeHash(Combine(toBeHashed,salt));
                }
            }

            public static byte[] ComputeHmacSHA1(byte[] toBeHashed, byte[] salt)
            {
                using (var hmac = new HMACSHA1(salt))
                {
                    return hmac.ComputeHash(Combine(toBeHashed,salt));
                }
            }

            public static byte[] ComputeHmacSHA256(byte[] toBeHashed, byte[] salt)
            {
                using (var hmac = new HMACSHA256(salt))
                {
                    return hmac.ComputeHash(Combine(toBeHashed, salt));
                }
            }

            public static byte[] ComputeHmacSHA384(byte[] toBeHashed, byte[] salt)
            {
                using (var hmac = new HMACSHA384(salt))
                {
                    return hmac.ComputeHash(Combine(toBeHashed, salt));
                }
            }

            public static byte[] ComputeHmacSHA512(byte[] toBeHashed, byte[] salt)
            {
                using (var hmac = new HMACSHA512(salt))
                {
                    return hmac.ComputeHash(Combine(toBeHashed, salt));
                }
            }
        }



        public class PBKDF2
        {

            public static byte[] HashPassword(byte[] toBeHashed, byte[] salt, int numberOfRounds)
            {
                using (var rfc2898 = new Rfc2898DeriveBytes(toBeHashed, salt, numberOfRounds))
                {
                    return rfc2898.GetBytes(20);
                }
            }

        }





    }
}

