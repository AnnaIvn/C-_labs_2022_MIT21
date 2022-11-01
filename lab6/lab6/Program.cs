using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Runtime.Intrinsics.Arm;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Collections.Generic;
using System.Linq;

namespace lab6
{
    class Program
    {
        static void Main(string[] args)
        {
            // initializing string options, data
            string option, variant, data, choose, decryptedDataDES, 
                   decryptedDataDES2, decryptedDataDES3, decryptedDataAES, 
                   password, passwordCheck;

            // initializing arrays
            byte[] dataInArray, encryptedDES, decryptedDES, 
                   encryptedDES2, decryptedDES2, encryptedDES3, decryptedDES3, 
                   encryptedAES, decryptedAES;

            // for fast key length change (when using AES encryption)
            int b48 = 48, b32 = 32, b24 = 24, b16 = 16;

            // for rfc generatiobn
            byte[] saltInArray1, saltInArray2, saltInArray3, saltInArray4,        // for salt in option 2
                   passwordInArray,
                   fullArray1, fullArray2, fullArray3, fullArray4,                // for full array (password + salt) in option 2
                   fullCutArray1, fullCutArray2, fullCutArray3, fullCutArray4,    // for full cut array 
                   passwordCheckInArray;


            int iterations = 110000;

            do
            {
                Console.WriteLine();
                Console.WriteLine("  | Options:");
                Console.WriteLine("  | 1 - part 1, symetryc encryption (using RNGcryptoServiceProvider)");
                Console.WriteLine("  | 2 - part 2, symetryc encryption (using Rfc2898DeriveBytes)");
                Console.WriteLine("  | 0 - exit");
                Console.WriteLine();
                Console.Write("   Your option -> ");
                option = Console.ReadLine();

                // USING RNGCRYPTO
                if (option == "1")
                {
                    // Starting data encryption
                    Console.WriteLine();
                    Console.WriteLine(" Starting data encryption using RNGcryptoServiceProvider");
                    Console.WriteLine(" -----------------------------------------------------------------------------------------------------------------------");
                    Console.Write(" Enter your data ->  ");
                    data = Console.ReadLine();                          // getting data
                    Console.WriteLine();
                    Console.WriteLine();

                    dataInArray = Encoding.Unicode.GetBytes(data);      // putting password in array

                    // DES part
                    var des = new DES();
                    var keyDES = otherFunctions.generateRandomNum(8);    // 8 bytes = 64 bits (our key is longer, actual key = 56 bits)
                    var ivDES = otherFunctions.generateRandomNum(8);
                    encryptedDES = des.Encrypt(dataInArray, keyDES, ivDES);            // enctyption
                    decryptedDES = des.Decrypt(encryptedDES, keyDES, ivDES);           // decryption
                    decryptedDataDES = Encoding.UTF8.GetString(decryptedDES);          // byte array -> string
                    
                    // Triple DES (1-2-1) part
                    var tdes = new TripleDES();
                    var keyDES2 = otherFunctions.generateRandomNum(16);    // 16 bytes = 128 bits (for two keys)
                    var ivDES2 = otherFunctions.generateRandomNum(8);
                    encryptedDES2 = tdes.Encrypt(dataInArray, keyDES2, ivDES2);         // enctyption
                    decryptedDES2 = tdes.Decrypt(encryptedDES2, keyDES2, ivDES2);       // decryption
                    decryptedDataDES2 = Encoding.UTF8.GetString(decryptedDES2);         // byte array -> string

                    // Triple DES (1-2-3) part
                    var tdes3 = new TripleDES();
                    var keyDES3 = otherFunctions.generateRandomNum(24);    // 24 bytes = 192 bits (for three keys)
                    var ivDES3 = otherFunctions.generateRandomNum(8);
                    encryptedDES3 = tdes3.Encrypt(dataInArray, keyDES3, ivDES3);        // enctyption
                    decryptedDES3 = tdes3.Decrypt(encryptedDES3, keyDES3, ivDES3);      // decryption
                    decryptedDataDES3 = Encoding.UTF8.GetString(decryptedDES3);         // byte array -> string

                    // AES part
                    var aes = new AES();
                    var keyAES = otherFunctions.generateRandomNum(b32);    // 32 bytes = 256 bits
                    var ivAES = otherFunctions.generateRandomNum(16);      // 16 bytes = 128
                    encryptedAES = aes.Encrypt(dataInArray, keyAES, ivAES);             // enctyption
                    decryptedAES = aes.Decrypt(encryptedAES, keyAES, ivAES);            // decryption
                    decryptedDataAES = Encoding.UTF8.GetString(decryptedAES);           // byte array -> string

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
                        Console.WriteLine();

                        // DES
                        if (variant == "1")
                        {                            
                            Console.WriteLine(" Encryption with DES ...");
                            otherFunctions.mainOutput(encryptedDES, dataInArray, decryptedDES, decryptedDataDES); // all output in separate function
                        }

                        // Triple DES
                        else if (variant == "2") 
                        {
                            do
                            {
                                Console.WriteLine("  | Choose Triple-DES algorithm for symetric encryption: ");
                                Console.WriteLine("  | 1 - Triple-DES (using 1-2-1 keys)");
                                Console.WriteLine("  | 2 - Triple-DES (using 1-2-3 keys)");
                                Console.WriteLine("  | 0 - exit");
                                Console.WriteLine();
                                Console.Write("   Your option -> ");
                                choose = Console.ReadLine();
                                Console.WriteLine();

                                // Triple DES (using 1-2-1 keys scheme)
                                if (choose == "1")
                                {
                                    Console.WriteLine(" Encryption with Triple DES (using 1-2-1 key scheme) ...");
                                    otherFunctions.mainOutput(encryptedDES2, dataInArray, decryptedDES2, decryptedDataDES2); // all output in separate function
                                }

                                // Triple DES (using 1-2-3 keys scheme)
                                else if (choose == "2")
                                {
                                    Console.WriteLine(" Encryption with Triple DES (using 1-2-3 key scheme) ...");
                                    otherFunctions.mainOutput(encryptedDES3, dataInArray, decryptedDES3, decryptedDataDES3); // all output in separate function
                                }

                            } while (choose != "0");
                        }

                        // AES
                        else if (variant == "3")
                        {
                            Console.WriteLine(" Encryption with AES ...");
                            otherFunctions.mainOutput(encryptedAES, dataInArray, decryptedAES, decryptedDataAES); // all output in this separate function
                        }

                        // all options
                        else if (variant == "4")
                        {
                            Console.WriteLine(" Encryption with DES, Triple DES (1-2-1, 1-2-3), AES ...");
                            Console.WriteLine(" -----------------------------------------------------------------------------------------------------------------------");
                            otherFunctions.allOutput.allEncreptedOutput(encryptedDES, encryptedDES2, encryptedDES3, encryptedAES);  // output functions
                            otherFunctions.allOutput.allDecreptedOutput(decryptedDataDES, decryptedDataDES2, decryptedDataDES3, decryptedDataAES);
                            otherFunctions.allOutput.allCheckOutput(dataInArray, decryptedDES, decryptedDES2, decryptedDES3, decryptedAES); 
                        }

                    } while (variant != "0");
                }

                // USING RFC2898
                if (option == "2")
                {
                    // Starting data encryption
                    Console.WriteLine();
                    Console.WriteLine(" Starting data encryption using Rfc2898DeriveBytes");
                    Console.WriteLine(" -----------------------------------------------------------------------------------------------------------------------");
                    Console.Write(" Enter your data ->  ");
                    data = Console.ReadLine();                          // getting data
                    Console.Write(" Enter your password (for encryption) ->  ");
                    password = Console.ReadLine();                      // getting password
                    Console.WriteLine();
                    Console.WriteLine();

                    dataInArray = Encoding.Unicode.GetBytes(data);           // putting data in array
                    passwordInArray = Encoding.Unicode.GetBytes(password);   // putting password in array
                    
                    saltInArray1 = otherFunctions.generateRandomNum(16);      // salt array for DES
                    saltInArray2 = otherFunctions.generateRandomNum(24);      // salt array for Tripple DES 1-2-1
                    saltInArray3 = otherFunctions.generateRandomNum(32);      // salt array for Tripple DES 1-2-3
                    saltInArray4 = otherFunctions.generateRandomNum(48);      // salt array for AES

                    fullArray1 = otherFunctions.CombineArrays(passwordInArray, saltInArray1);   // merging password and salt arrays
                    fullArray2 = otherFunctions.CombineArrays(passwordInArray, saltInArray2);
                    fullArray3 = otherFunctions.CombineArrays(passwordInArray, saltInArray3);
                    fullArray4 = otherFunctions.CombineArrays(passwordInArray, saltInArray4);

                    fullCutArray1 = otherFunctions.SplitArrayKey(fullArray1, 16, 16);     // so that array wont be longer than needed
                    fullCutArray2 = otherFunctions.SplitArrayKey(fullArray2, 24, 24);
                    fullCutArray3 = otherFunctions.SplitArrayKey(fullArray3, 32, 32);
                    fullCutArray4 = otherFunctions.SplitArrayKey(fullArray4, 48, 48);


                    // DES part
                    var des = new DES();
                    byte[] keyIv1 = otherFunctions.generateNum(dataInArray, fullCutArray1, iterations, b16);
                    byte[] key1 = otherFunctions.SplitArrayKey(keyIv1, 8, 8);
                    byte[] iv1 = otherFunctions.SplitArrayIv(keyIv1, 8, 8);
                    encryptedDES = des.Encrypt(dataInArray, key1, iv1);            // enctyption
                    decryptedDES = des.Decrypt(encryptedDES, key1, iv1);           // decryption
                    decryptedDataDES = Encoding.UTF8.GetString(decryptedDES);      // byte array -> string

                    // Triple DES (1-2-1) part
                    var tdes = new TripleDES();
                    byte[] keyIv2 = otherFunctions.generateNum(dataInArray, fullCutArray2, iterations, b24);
                    byte[] key2 = otherFunctions.SplitArrayKey(keyIv2, 16, 8);
                    byte[] iv2 = otherFunctions.SplitArrayIv(keyIv2, 16, 8);
                    encryptedDES2 = tdes.Encrypt(dataInArray, key2, iv2);         // enctyption
                    decryptedDES2 = tdes.Decrypt(encryptedDES2, key2, iv2);       // decryption
                    decryptedDataDES2 = Encoding.UTF8.GetString(decryptedDES2);   // byte array -> string

                    // Triple DES (1-2-3) part
                    var tdes3 = new TripleDES();
                    byte[] keyIv3 = otherFunctions.generateNum(dataInArray, fullCutArray3, iterations, b32);
                    byte[] key3 = otherFunctions.SplitArrayKey(keyIv3, 24, 8);
                    byte[] iv3 = otherFunctions.SplitArrayIv(keyIv3, 24, 8);
                    encryptedDES3 = tdes3.Encrypt(dataInArray, key3, iv3);        // enctyption
                    decryptedDES3 = tdes3.Decrypt(encryptedDES3, key3, iv3);      // decryption
                    decryptedDataDES3 = Encoding.UTF8.GetString(decryptedDES3);   // byte array -> string

                    // AES part
                    var aes = new AES();
                    byte[] keyIv4 = otherFunctions.generateNum(dataInArray, fullCutArray4, iterations, b48);
                    byte[] key4 = otherFunctions.SplitArrayKey(keyIv4, 32, 16);
                    byte[] iv4 = otherFunctions.SplitArrayIv(keyIv4, 32, 16);
                    encryptedAES = aes.Encrypt(dataInArray, key4, iv4);             // enctyption
                    decryptedAES = aes.Decrypt(encryptedAES, key4, iv4);            // decryption
                    decryptedDataAES = Encoding.UTF8.GetString(decryptedAES);       // byte array -> string


                    do
                    {
                        Console.WriteLine();
                        Console.WriteLine("  | Choose algorithm for symetric encryption using Rfc2898DeriveBytes:");
                        Console.WriteLine("  | 1 - DES");
                        Console.WriteLine("  | 2 - Triple-DES");
                        Console.WriteLine("  | 3 - AES");
                        Console.WriteLine("  | 4 - all of them");
                        Console.WriteLine("  | 0 - exit");
                        Console.WriteLine();
                        Console.Write("   Your variant -> ");
                        variant = Console.ReadLine();
                        Console.WriteLine();
                        Console.WriteLine();

                        // DES
                        if (variant == "1")
                        {
                            Console.WriteLine(" Encryption with DES ...");
                            passwordCheck = otherFunctions.mainOutput2(encryptedDES); // all output in separate function

                            // decryption part
                            passwordCheckInArray = Encoding.Unicode.GetBytes(passwordCheck);               // putting password in array
                            fullArray1 = otherFunctions.CombineArrays(passwordCheckInArray, saltInArray1);
                            fullCutArray1 = otherFunctions.SplitArrayKey(fullArray1, 16, 16);              // so that array wont be longer than needed

                            var des0 = new DES();
                            byte[] keyIv10 = otherFunctions.generateNum(dataInArray, fullCutArray1, iterations, b16);
                            byte[] key10 = otherFunctions.SplitArrayKey(keyIv10, 8, 8);
                            byte[] iv10 = otherFunctions.SplitArrayIv(keyIv10, 8, 8);
                            encryptedDES = des0.Encrypt(dataInArray, key10, iv10);            // enctyption
                            decryptedDES = des0.Decrypt(encryptedDES, key10, iv10);           // decryption
                            decryptedDataDES = Encoding.UTF8.GetString(decryptedDES);      // byte array -> string

                            otherFunctions.mainOutput3(decryptedDataDES, dataInArray, decryptedDES);
                        }

                        // Triple DES
                        else if (variant == "2")
                        {
                            do
                            {
                                Console.WriteLine("  | Choose Triple-DES algorithm for symetric encryption (using Rfc2898DeriveBytes): ");
                                Console.WriteLine("  | 1 - Triple-DES (using 1-2-1 keys)");
                                Console.WriteLine("  | 2 - Triple-DES (using 1-2-3 keys)");
                                Console.WriteLine("  | 0 - exit");
                                Console.WriteLine();
                                Console.Write("   Your option -> ");
                                choose = Console.ReadLine();
                                Console.WriteLine();

                                //Triple DES(using 1 - 2 - 1 keys scheme)
                                if (choose == "1")
                                {
                                    Console.WriteLine(" Encryption with Triple DES (using 1-2-1 key scheme) ...");
                                    passwordCheck = otherFunctions.mainOutput2(encryptedDES2);               // all output in separate function

                                    // decryption part
                                    passwordCheckInArray = Encoding.Unicode.GetBytes(passwordCheck);               // putting password in array
                                    fullArray2 = otherFunctions.CombineArrays(passwordCheckInArray, saltInArray2);
                                    fullCutArray2 = otherFunctions.SplitArrayKey(fullArray2, 24, 24);

                                    var tdes0 = new TripleDES();        // Triple DES (1-2-1) part
                                    byte[] keyIv20 = otherFunctions.generateNum(dataInArray, fullCutArray2, iterations, b24);
                                    byte[] key20 = otherFunctions.SplitArrayKey(keyIv20, 16, 8);
                                    byte[] iv20 = otherFunctions.SplitArrayIv(keyIv20, 16, 8);
                                    encryptedDES2 = tdes0.Encrypt(dataInArray, key20, iv20);         // enctyption
                                    decryptedDES2 = tdes0.Decrypt(encryptedDES2, key20, iv20);       // decryption
                                    decryptedDataDES2 = Encoding.UTF8.GetString(decryptedDES2);   // byte array -> string

                                    otherFunctions.mainOutput3(decryptedDataDES2, dataInArray, decryptedDES2);
                                }

                                //Triple DES(using 1 - 2 - 3 keys scheme)
                                else if (choose == "2")
                                {
                                    Console.WriteLine(" Encryption with Triple DES (using 1-2-3 key scheme) ...");
                                    passwordCheck = otherFunctions.mainOutput2(encryptedDES3);               // all output in separate function

                                    // decryption part
                                    passwordCheckInArray = Encoding.Unicode.GetBytes(passwordCheck);               // putting password in array
                                    fullArray3 = otherFunctions.CombineArrays(passwordCheckInArray, saltInArray3);
                                    fullCutArray3 = otherFunctions.SplitArrayKey(fullArray3, 32, 32);

                                    var tdes30 = new TripleDES();      // Triple DES (1-2-3) part
                                    byte[] keyIv30 = otherFunctions.generateNum(dataInArray, fullCutArray3, iterations, b32);
                                    byte[] key30 = otherFunctions.SplitArrayKey(keyIv30, 24, 8);
                                    byte[] iv30 = otherFunctions.SplitArrayIv(keyIv30, 24, 8);
                                    encryptedDES3 = tdes30.Encrypt(dataInArray, key30, iv30);        // enctyption
                                    decryptedDES3 = tdes30.Decrypt(encryptedDES3, key30, iv30);      // decryption
                                    decryptedDataDES3 = Encoding.UTF8.GetString(decryptedDES3);   // byte array -> string

                                    otherFunctions.mainOutput3(decryptedDataDES3, dataInArray, decryptedDES3);
                                }

                            } while (choose != "0");
                        }

                        // AES
                        else if (variant == "3")
                        {
                            Console.WriteLine(" Encryption with AES ...");
                            passwordCheck = otherFunctions.mainOutput2(encryptedDES3);               // all output in separate function

                            // decryption part
                            passwordCheckInArray = Encoding.Unicode.GetBytes(passwordCheck);
                            fullArray4 = otherFunctions.CombineArrays(passwordInArray, saltInArray4);
                            fullCutArray4 = otherFunctions.SplitArrayKey(fullArray4, 48, 48);

                            var aes0 = new AES();    // AES part
                            byte[] keyIv40 = otherFunctions.generateNum(dataInArray, fullCutArray4, iterations, b48);
                            byte[] key40 = otherFunctions.SplitArrayKey(keyIv40, 32, 16);
                            byte[] iv40 = otherFunctions.SplitArrayIv(keyIv40, 32, 16);
                            encryptedAES = aes0.Encrypt(dataInArray, key40, iv40);             // enctyption
                            decryptedAES = aes0.Decrypt(encryptedAES, key40, iv40);            // decryption
                            decryptedDataAES = Encoding.UTF8.GetString(decryptedAES);       // byte array -> string

                            otherFunctions.mainOutput3(decryptedDataAES, dataInArray, decryptedAES);
                        }

                        // all options
                        else if (variant == "4")
                        {
                            Console.WriteLine(" Encryption with DES, Triple DES (1-2-1, 1-2-3), AES ...");
                            Console.WriteLine(" -----------------------------------------------------------------------------------------------------------------------");
                            otherFunctions.allOutput.allEncreptedOutput(encryptedDES, encryptedDES2, encryptedDES3, encryptedAES);  // output functions
                            otherFunctions.allOutput.allDecreptedOutput(decryptedDataDES, decryptedDataDES2, decryptedDataDES3, decryptedDataAES);
                            otherFunctions.allOutput.allCheckOutput(dataInArray, decryptedDES, decryptedDES2, decryptedDES3, decryptedAES);
                        }

                    } while (variant != "0");
                }
    
            } while (option != "0");
        }



        // other functions for proper work of programm
        public class otherFunctions
        {

            public static string CheckData(byte[] data1, byte[] data2)  // checking data (entered data + decrypted data) for accuracy
            {
                if (Convert.ToBase64String(data1) == Convert.ToBase64String(data2))
                {
                    Console.WriteLine(" Data is accurate");
                    
                }
                else
                {
                    Console.WriteLine(" Data is corrupted");
                }
                return "";
            }
            
            public static byte[] generateRandomNum(int length)  // to generate symmetric encryption keys and initialization vectors
            {
                using (var rng = new RNGCryptoServiceProvider())
                {
                    var Key = new byte[length];
                    rng.GetBytes(Key);
                    return Key;
                }
            }

            public static byte[] generateNum(byte[] data, byte[] salt, int iterations, int bytes)  // to generate symmetric encryption keys and initialization vectors using Rfc2898DeriveBytes
            {
                using (var rfc = new Rfc2898DeriveBytes(data, salt, iterations))
                {
                    return rfc.GetBytes(bytes);
                }
            }

            public static byte[] SplitArrayKey(byte[] data, int split1, int take) // splitting function
            {
                byte[] key = data.Take(split1).ToArray();
                //byte[] iv = data.Skip(split1).Take(take).ToArray();
                return key;
            }
            public static byte[] SplitArrayIv(byte[] data, int split1, int take) // splitting function
            {
                //byte[] key = data.Take(split1).ToArray();
                byte[] iv = data.Skip(split1).Take(take).ToArray();
                return iv;
            }

            public static byte[] CombineArrays(byte[] array1, byte[] array2)  // to combine two arrays (password + salt)
            {
                byte[] final = new byte[array1.Length + array2.Length]; // new array to combine password and salt arrays
                Array.Copy(array1, final, array1.Length);
                Array.Copy(array2, 0, final, array1.Length, array2.Length);
                return final;
            }

            public static void prettyOutput(int num)    // for pretty output 
            {
                if (num == 1)
                {
                    Console.WriteLine();
                    Console.WriteLine("  | Press Enter to continue process");
                    Console.ReadLine();
                    Console.WriteLine();
                }
                else if (num == 2)
                {
                    Console.WriteLine();
                    Console.WriteLine("  | Press Enter to continue");
                    Console.ReadLine();
                    Console.WriteLine();
                }
            }

            public static void mainOutput(byte[] encrypted, byte[] dataInArray, byte[] decrypted, string decryptedData)  // main output
            {
                Console.WriteLine(" -----------------------------------------------------------------------------------------------------------------------");
                Console.WriteLine();
                Console.WriteLine(" Encrypted Text:  " + Convert.ToBase64String(encrypted));
                otherFunctions.prettyOutput(1);                               // some output for better look
                Console.WriteLine(" Decrypted Text:  " + decryptedData);   
                otherFunctions.prettyOutput(1);                               
                otherFunctions.CheckData(dataInArray, decrypted);     
                otherFunctions.prettyOutput(2);                              
            }
            public static string mainOutput2(byte[] encrypted)  // main output for option 2 start (encryption + gettion some data)
            {
                Console.WriteLine(" -----------------------------------------------------------------------------------------------------------------------");
                Console.WriteLine();
                Console.WriteLine(" Encrypted Text:  " + Convert.ToBase64String(encrypted)); // encrypted part
                otherFunctions.prettyOutput(1);                               // some output for better look
                Console.WriteLine(" Starting decryption with Triple DES (using 1-2-1 key scheme) ...");
                Console.WriteLine(" -----------------------------------------------------------------------------------------------------------------------");
                Console.WriteLine();
                Console.Write(" Enter your password -> ");
                string passwordCheck = Console.ReadLine();
                Console.WriteLine();
                Console.WriteLine();
                return passwordCheck;
            }
            public static void mainOutput3(string decryptedDataDES, byte[] dataInArray, byte[] decryptedDES)  // main output for option 2 end (decryption)
            {
                Console.WriteLine(" Decrypted Text:  " + decryptedDataDES);   //decrypted part
                otherFunctions.prettyOutput(1);                               
                otherFunctions.CheckData(dataInArray, decryptedDES);     
                otherFunctions.prettyOutput(2);                             
            }



            // output for variant 4 (all types of encryption)
            public class allOutput
            {

                public static void allEncreptedOutput(byte[] encryptedDES, byte[] encryptedDES2, byte[] encryptedDES3, byte[] encryptedAES) // encrepted output for variant 4 (all encryptions)
                {
                    Console.WriteLine(" Encrypted data using DES:                 " + Convert.ToBase64String(encryptedDES));
                    Console.WriteLine(" Encrypted data using Triple DES (1-2-1):  " + Convert.ToBase64String(encryptedDES2));
                    Console.WriteLine(" Encrypted data using Triple DES (1-2-3):  " + Convert.ToBase64String(encryptedDES3));
                    Console.WriteLine(" Encrypted data using AES:                 " + Convert.ToBase64String(encryptedAES));
                    otherFunctions.prettyOutput(1);                  // some output for better look
                }

                public static void allDecreptedOutput(string decryptedDataDES, string decryptedDataDES2, string decryptedDataDES3, string decryptedDataAES) // decrepted output for variant 4 (all encryptions)
                {
                    Console.WriteLine(" Decryption ... ");
                    Console.WriteLine(" -----------------------------------------------------");
                    Console.WriteLine(" Decrypted data using DES:                 " + decryptedDataDES);
                    Console.WriteLine(" Decrypted data using Triple DES (1-2-1):  " + decryptedDataDES2);
                    Console.WriteLine(" Decrypted data using Triple DES (1-2-3):  " + decryptedDataDES3);
                    Console.WriteLine(" Decrypted data using AES:                 " + decryptedDataAES);
                    otherFunctions.prettyOutput(1);
                }

                public static void allCheckOutput(byte[] dataInArray, byte[] decryptedDES, byte[] decryptedDES2, byte[] decryptedDES3, byte[] decryptedAES)
                {
                    Console.WriteLine(" Checking ... ");
                    Console.WriteLine(" -----------------------------------------------------");
                    Console.Write(" For DES:                ");
                    Console.Write(otherFunctions.CheckData(dataInArray, decryptedDES));
                    Console.Write(" For Triple DES (1-2-1): ");
                    Console.Write(otherFunctions.CheckData(dataInArray, decryptedDES2));
                    Console.Write(" For Triple DES (1-2-3): ");
                    Console.Write(otherFunctions.CheckData(dataInArray, decryptedDES3));
                    Console.Write(" For AES:                ");
                    Console.Write(otherFunctions.CheckData(dataInArray, decryptedAES));
                    otherFunctions.prettyOutput(2);
                }

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



        // encryption + decryption functions with Triple DES (Data Encryption Standard)
        public class TripleDES
        {

            public byte[] Encrypt(byte[] dataToEncrypt, byte[] key, byte[] iv)  // Triple DES encryption
            {
                using (var des = new TripleDESCryptoServiceProvider())    // creating an instance of the TripleDESCryptoServiceProvider object
                {
                    des.Mode = CipherMode.CBC;         // explicitly setting default encryption mode
                    des.Padding = PaddingMode.PKCS7;   // explicitly setting default padding
                    des.Key = key;                       // assigning key
                    des.IV = iv;                         // assigning initialisation vector
                    using (var memoryStream = new MemoryStream())     // creating (тут більше як об'явлення створення) MemoryStream instance
                    {
                        var cryptoStream = new CryptoStream(memoryStream, des.CreateEncryptor(), CryptoStreamMode.Write);
                        cryptoStream.Write(dataToEncrypt, 0, dataToEncrypt.Length);    // encryption
                        cryptoStream.FlushFinalBlock();
                        return memoryStream.ToArray();
                    }
                }
            }

            public byte[] Decrypt(byte[] dataToDecrypt, byte[] key, byte[] iv)  // Triple DES decryption
            {
                using (var des = new TripleDESCryptoServiceProvider()) 
                {
                    des.Mode = CipherMode.CBC;         
                    des.Padding = PaddingMode.PKCS7;  
                    des.Key = key;                      
                    des.IV = iv;                       
                    using (var memoryStream = new MemoryStream())     // creating MemoryStream instance
                    {
                        var cryptoStream = new CryptoStream(memoryStream, des.CreateDecryptor(), CryptoStreamMode.Write);  // creating CryptoStream instance
                        cryptoStream.Write(dataToDecrypt, 0, dataToDecrypt.Length);  // decryption itself
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
                using (var aes = new AesCryptoServiceProvider())  // main difference; creating an instance of the AesCryptoServiceProvider object
                {
                    aes.Mode = CipherMode.CBC;                    // same as in the previous functions
                    aes.Padding = PaddingMode.PKCS7;
                    aes.Key = key;
                    aes.IV = iv;
                    using (var memoryStream = new MemoryStream())
                    {
                        var cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write); 
                        cryptoStream.Write(dataToEncrypt, 0, dataToEncrypt.Length);   // encryption
                        cryptoStream.FlushFinalBlock();
                        return memoryStream.ToArray();
                    }
                }
            }

            public byte[] Decrypt(byte[] dataToDecrypt, byte[] key, byte[] iv) // decription AES method
            {
                using (var des = new AesCryptoServiceProvider())   // creation an instance of this object
                {
                    des.Mode = CipherMode.CBC;
                    des.Padding = PaddingMode.PKCS7;
                    des.Key = key;
                    des.IV = iv;
                    using (var memoryStream = new MemoryStream())
                    {
                        var cryptoStream = new CryptoStream(memoryStream, des.CreateDecryptor(), CryptoStreamMode.Write);  // Create Decryptor
                        cryptoStream.Write(dataToDecrypt, 0, dataToDecrypt.Length);  // decryption
                        cryptoStream.FlushFinalBlock();
                        return memoryStream.ToArray();
                    }
                }
            }

        }



    }
}