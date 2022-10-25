using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Runtime.Intrinsics.Arm;
using System.Diagnostics;
using System.Reflection.Metadata;

namespace lab6
{
    class Program
    {
        static void Main(string[] args)
        {
            string option;

            do
            {
                Console.WriteLine();
                Console.WriteLine("  | Options:");
                Console.WriteLine("  | 1 - part 1, symetryc encryption (using cryptoServiceProvider)");
                Console.WriteLine("  | 0 - exit");
                Console.WriteLine();
                Console.Write("   Your option -> ");
                option = Console.ReadLine();

                if (option == "1")
                {
                    // initializing passwords, usernames
                    string data, dataCheck, variant;

                    // initializing arrays
                    byte[] dataInArray;

                    // Starting data encryption
                    Console.WriteLine();
                    Console.WriteLine(" Starting data encryption ");
                    Console.WriteLine(" -----------------------------------------------------------------------------------------------------------------------");
                    Console.Write(" Enter your data ->  ");
                    data = Console.ReadLine();                          // getting data
                    Console.WriteLine();
                    Console.WriteLine();

                    dataInArray = Encoding.Unicode.GetBytes(data);  // putting password in array

                    do
                    {
                        Console.WriteLine();
                        Console.WriteLine("  | Choose algorithm for symetric encryption:");
                        Console.WriteLine("  | 1 - DES");
                        Console.WriteLine("  | 2 - Triple-DES");
                        Console.WriteLine("  | 3 - AES");
                        Console.WriteLine("  | 4 - all of them");
                        Console.WriteLine("  | 0 - exit");
                        Console.WriteLine();
                        Console.Write("   Your variant -> ");
                        variant = Console.ReadLine();
                        Console.WriteLine();

                        if (variant == "1")
                        {
                            var des = new DES();
                            var key = generateRandomNum(8);    // 8 bytes = 64 bits (our key is longer? actual key = 56 bits)
                            var iv = generateRandomNum(8);

                            var encrypted = des.Encrypt(dataInArray, key, iv);               // enctyption
                            var decrypted = des.Decrypt(encrypted, key, iv);                 // decryption
                            var decryptedMessage = Encoding.UTF8.GetString(decrypted);       // byte array -> string
                            Console.WriteLine(" Encryption with DES ...");
                            Console.WriteLine(" -----------------------------------------------------------------------------------------------------------------------");
                            Console.WriteLine();
                            // encrypted part
                            Console.WriteLine(" Encrypted Text:  " + Convert.ToBase64String(encrypted));
                            Console.WriteLine();
                            Console.WriteLine("  | Press Enter to continue process");
                            Console.ReadLine();
                            Console.WriteLine();
                            // decrypted part
                            Console.WriteLine(" Decrypted Text:  " + decryptedMessage);
                            Console.WriteLine();
                            Console.WriteLine("  | Press Enter to continue");
                            Console.ReadLine();
                            Console.WriteLine();
                            // write checking here
                        }

                        else if (variant == "2")
                        {

                        }

                        else if (variant == "3")
                        {
                            //var key = aesChipher.GenerateRandomNumber(32);
                            //var iv = aesChipher.GenerateRandomNumber(16);
                            //const string original = "Text to encrypt";
                            //var encrypted = aesChipher.Encrypt(Encoding.UTF8.GetBytes(original), key, iv);
                            //var decrypted = aesChipher.Decrypt(encrypted, key, iv);
                            //var decryptedMessage = Encoding.UTF8.GetString(decrypted);
                            //Console.WriteLine("AES Encryption in .NET");
                            //Console.WriteLine("----------------------");
                            //Console.WriteLine();
                            //Console.WriteLine("Original Text = " + original);
                            //Console.WriteLine("Encrypted Text = " +
                            //Convert.ToBase64String(encrypted));
                            //Console.WriteLine("Decrypted Text = " + decryptedMessage);
                            //Console.ReadKey();
                        }

                        else if (variant == "4")
                        {

                        }

                    } while (variant != "0");
                }

            } while (option != "0");
        }

        // checking hash for accuracy
        public static string CheckHashOnly(byte[] hash1, byte[] hash2)
        {
            if (Convert.ToBase64String(hash1) == Convert.ToBase64String(hash2))
            {
                Console.WriteLine("Hash of password is accurate");
            }
            else
            {
                Console.WriteLine("Hash isn`t accurate. Password is corrupted");
            }
            return " ";
        }


        // to generate symmetric encryption keys and initialization vectors
        public static byte[] generateRandomNum(int length)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var Key = new byte[length];
                rng.GetBytes(Key);
                return Key;
            }
        }


        // encryption + decryption functions with DES (Data Encryption Standard)
        public class DES
        {

            public byte[] Encrypt(byte[] dataToEncrypt, byte[] key, byte[] iv) // encryption using DES
            {
                using (var des = new DESCryptoServiceProvider())    // creating an instance of the DESCryptoServiceProvider object
                {
                    des.Mode = CipherMode.CBC;           // setting (explicitly - явно) encryption mode;  using default mode (so that encrypted data would be same if key wasn`t changed)
                    des.Padding = PaddingMode.PKCS7;     // setting padding; applies when the message block being encrypted is shorter than needed
                    des.Key = key;                         // key that was passed in as parameter is assigned
                    des.IV = iv;                           // initialization vector ...
                    using (var memoryStream = new MemoryStream())   // instances of MemoryStream and CryptoStream are created
                    {
                        var cryptoStream = new CryptoStream(memoryStream, des.CreateEncryptor(), CryptoStreamMode.Write);      // we pass in des.CreateEncryptor(), which sets up the crypto stream with our configured DESCryptoServiceProvider object.
                        cryptoStream.Write(dataToEncrypt, 0, dataToEncrypt.Length);   // following two lines - actual encryption
                        cryptoStream.FlushFinalBlock();
                        return memoryStream.ToArray();
                    }
                }
            }

            public byte[] Decrypt(byte[] dataToDecrypt, byte[] key, byte[] iv) // decryption using DES
            {
                using (var des = new DESCryptoServiceProvider())
                {
                    des.Mode = CipherMode.CBC;
                    des.Padding = PaddingMode.PKCS7;
                    des.Key = key;
                    des.IV = iv;
                    using (var memoryStream = new MemoryStream())
                    {
                        var cryptoStream = new CryptoStream(memoryStream, des.CreateDecryptor(), CryptoStreamMode.Write);  // des.CreateDecryptor()
                        cryptoStream.Write(dataToDecrypt, 0, dataToDecrypt.Length);   // decryption 
                        cryptoStream.FlushFinalBlock();
                        return memoryStream.ToArray();
                    }
                }
            }

        }


        // encryption + decryption functions with AES (Advanced Encryption Standard)
        public class AES
        {
            public byte[] Encrypt(byte[] dataToEncrypt, byte[] key, byte[] iv) // encryption using AES method
            {
                using (var aes = new AesCryptoServiceProvider())
                {
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;
                    aes.Key = key;
                    aes.IV = iv;
                    using (var memoryStream = new MemoryStream())
                    {
                        var cryptoStream = new CryptoStream(
                        memoryStream,
                        aes.CreateEncryptor(),
                        CryptoStreamMode.Write);
                        cryptoStream.Write(dataToEncrypt, 0,
                        dataToEncrypt.Length);
                        cryptoStream.FlushFinalBlock();
                        return memoryStream.ToArray();
                    }
                }
            }

            public byte[] Decrypt(byte[] dataToDecrypt, byte[] key, byte[] iv) // decription AES method
            {
                using (var des = new DESCryptoServiceProvider())
                {
                    des.Mode = CipherMode.CBC;
                    des.Padding = PaddingMode.PKCS7;
                    des.Key = key;
                    des.IV = iv;
                    using (var memoryStream = new MemoryStream())
                    {
                        var cryptoStream = new CryptoStream(memoryStream, des.CreateDecryptor(), CryptoStreamMode.Write);
                        cryptoStream.Write(dataToDecrypt, 0, dataToDecrypt.Length);
                        cryptoStream.FlushFinalBlock();
                        return memoryStream.ToArray();
                    }
                }
            }

        }
        


        







    }
}

