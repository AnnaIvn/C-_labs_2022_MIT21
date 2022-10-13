using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Runtime.Intrinsics.Arm;
using System.Diagnostics;

namespace lab3
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
                Console.WriteLine("  | 1 - part 1, hashing with salt");
                Console.WriteLine("  | 2 - part 2, hashing with salt + iterations");
                Console.WriteLine("  | 3 - part 2, hashing with salt + iterations, acording to variant");   // 10 значень із кроком 50'000; перше значення = номер варіанта * 10'000; variant = 11
                Console.WriteLine("  | 4 - part 3, authentification (with salt + iteration)");         // Число ітерацій = номер варіанта * 10'000.
                Console.WriteLine("  | 0 - exit");
                Console.WriteLine();
                Console.Write("   Your option -> ");
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
                    Console.WriteLine(" Starting user registration ");
                    Console.WriteLine(" -----------------------------------------------------------------------------------------------------------------------");
                    Console.Write(" Enter your username ->  ");
                    username = Console.ReadLine();                          // getting username
                    Console.Write(" Enter your password ->  ");
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

                    Console.WriteLine(" Your hashed password using ...  ");       // output
                    Console.WriteLine("------------------------------------------------------------------------------------------------------------------------");
                    Console.WriteLine(" MD5: " + Convert.ToBase64String(HmacMD5));
                    Console.WriteLine(" SHA1: " + Convert.ToBase64String(HmacSHA1));                                                                                // for delay
                    Console.WriteLine(" SHA256: " + Convert.ToBase64String(HmacSHA256));
                    Console.WriteLine(" SHA384: " + Convert.ToBase64String(HmacSHA384));
                    Console.WriteLine(" SHA512:  " + Convert.ToBase64String(HmacSHA512));
                    Console.WriteLine();
                    Console.WriteLine("  | Press Enter to see more");
                    Console.ReadLine();
                    Console.WriteLine();


                    // starting user authentification
                    Console.WriteLine(" Starting user authentification ");
                    Console.WriteLine("------------------------------------------------------------------------------------------------------------------------");
                    Console.Write(" Enter your username ->  ");
                    usernameCheck = Console.ReadLine();               // getting username
                    Console.Write(" Enter your password ->  ");
                    passwordCheck = Console.ReadLine();               // getting password
                    Console.WriteLine();
                    Console.WriteLine();

                    var Hmac2MD5 = SaltedHash.ComputeHmacSHA1(passwordInArray, saltMD5);
                    var Hmac2SHA1 = SaltedHash.ComputeHmacSHA1(passwordInArray, saltSHA1);
                    var Hmac2SHA256 = SaltedHash.ComputeHmacSHA256(passwordInArray, saltSHA256);  // call functions
                    var Hmac2SHA384 = SaltedHash.ComputeHmacSHA384(passwordInArray, saltSHA384);
                    var Hmac2SHA512 = SaltedHash.ComputeHmacSHA512(passwordInArray, saltSHA512);

                    Console.WriteLine(" Your hashed password using ...");       // output
                    Console.WriteLine(" -----------------------------------------------------------------------------------------------------------------------");
                    Console.WriteLine(" MD5: " + Convert.ToBase64String(Hmac2MD5));
                    Console.WriteLine(" SHA1: " + Convert.ToBase64String(Hmac2SHA1));                                                                              // for delay
                    Console.WriteLine(" SHA256: " + Convert.ToBase64String(Hmac2SHA256));
                    Console.WriteLine(" SHA384: " + Convert.ToBase64String(Hmac2SHA384));
                    Console.WriteLine(" SHA512:  " + Convert.ToBase64String(Hmac2SHA512));
                    Console.WriteLine();
                    Console.WriteLine("  | Press Enter to see more");
                    Console.ReadLine();
                    Console.WriteLine();


                    // checking
                    Console.WriteLine(" Checking your hashed password  ...");
                    Console.WriteLine(" -----------------------------------------------------------------------------------------------------------------------");
                    Console.Write("  MD5 check: ");
                    Console.Write( CheckHashOnly(HmacMD5, Hmac2MD5));
                    Console.Write(" SHA1 check: ");
                    Console.Write(CheckHashOnly(HmacSHA1, Hmac2SHA1));
                    Console.Write(" SHA256 check: ");
                    Console.Write(CheckHashOnly(HmacSHA256, Hmac2SHA256));
                    Console.Write(" SHA384 check: ");
                    Console.Write(CheckHashOnly(HmacSHA384, Hmac2SHA384));
                    Console.Write(" SHA512 check: ");
                    Console.Write(CheckHashOnly(HmacSHA512, Hmac2SHA512));
                    Console.WriteLine();
                    Console.WriteLine();
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

                    int numOfRounds = 100, numOfRounds1 = 1000, numOfRounds10 = 10000,
                        numOfRounds100 = 100000, numOfRounds1000 = 1000000;


                    // user initialisation
                    Console.WriteLine();
                    Console.WriteLine(" Starting user registration ");
                    Console.WriteLine(" -----------------------------------------------------------------------------------------------------------------------");
                    Console.Write(" Enter your username ->  ");
                    username = Console.ReadLine();                          // getting username
                    Console.Write(" Enter your password ->  ");
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
                    // round 1 = 100
                    var RfcMD5 = PBKDF2.HashPassword(passwordInArray, saltMD5, numOfRounds, saltLengthMD5);
                    var RfcSHA1 = PBKDF2.HashPassword(passwordInArray, saltSHA1, numOfRounds, saltLengthSHA1);
                    var RfcSHA256 = PBKDF2.HashPassword(passwordInArray, saltSHA256, numOfRounds, saltLengthSHA256);
                    var RfcSHA384 = PBKDF2.HashPassword(passwordInArray, saltSHA384, numOfRounds, saltLengthSHA384);
                    var RfcSHA512 = PBKDF2.HashPassword(passwordInArray, saltSHA512, numOfRounds, saltLengthSHA512);

                    var timeMD5 = MeasureRunTime(() => {
                        PBKDF2.HashPassword(passwordInArray, saltMD5, numOfRounds, saltLengthMD5);
                    });
                    var timeSHA1 = MeasureRunTime(() => {
                        PBKDF2.HashPassword(passwordInArray, saltSHA1, numOfRounds, saltLengthSHA1);
                    });
                    var timeSHA256 = MeasureRunTime(() => {
                        PBKDF2.HashPassword(passwordInArray, saltSHA256, numOfRounds, saltLengthSHA256);
                    });
                    var timeSHA384 = MeasureRunTime(() => {
                        PBKDF2.HashPassword(passwordInArray, saltSHA384, numOfRounds, saltLengthSHA384);
                    });
                    var timeSHA512 = MeasureRunTime(() => {
                        PBKDF2.HashPassword(passwordInArray, saltSHA512, numOfRounds, saltLengthSHA512);
                    });

                    Console.WriteLine(" Your hashed (with salt + number of rounds 100) password using ...");       // output
                    Console.WriteLine(" -----------------------------------------------------------------------------------------------------------------------");
                    Console.WriteLine(" 100 MD5: " + Convert.ToBase64String(RfcMD5));
                    Console.WriteLine(" 100 SHA1: " + Convert.ToBase64String(RfcSHA1));
                    Console.WriteLine(" 100 SHA256: " + Convert.ToBase64String(RfcSHA256));
                    Console.WriteLine(" 100 SHA384: " + Convert.ToBase64String(RfcSHA384));
                    Console.WriteLine(" 100 SHA512: " + Convert.ToBase64String(RfcSHA512));
                    Console.WriteLine();
                    Console.WriteLine("  | Press Enter to see more");
                    Console.ReadLine(); 
                    Console.WriteLine(" Iterations <" + numOfRounds + ">, Method <MD5>,    Elapsed Time: " + timeMD5);
                    Console.WriteLine(" Iterations <" + numOfRounds + ">  Metod <SHA1>,    Elapsed Time: " + timeSHA1);
                    Console.WriteLine(" Iterations <" + numOfRounds + ">, Method <SHA256>, Elapsed Time: " + timeSHA256);
                    Console.WriteLine(" Iterations <" + numOfRounds + ">, Method <SHA384>, Elapsed Time: " + timeSHA384);
                    Console.WriteLine(" Iterations <" + numOfRounds + ">, Method <SHA512>, Elapsed Time: " + timeSHA512);
                    Console.WriteLine();
                    Console.WriteLine("  | Press Enter to see more");
                    Console.ReadLine();
                    Console.WriteLine();



                    // round 2 = 1000
                    var Rfc1MD5 = PBKDF2.HashPassword(passwordInArray, saltMD5, numOfRounds1, saltLengthMD5);
                    var Rfc1SHA1 = PBKDF2.HashPassword(passwordInArray, saltSHA1, numOfRounds1, saltLengthSHA1);
                    var Rfc1SHA256 = PBKDF2.HashPassword(passwordInArray, saltSHA256, numOfRounds1, saltLengthSHA256);
                    var Rfc1SHA384 = PBKDF2.HashPassword(passwordInArray, saltSHA384, numOfRounds1, saltLengthSHA384);
                    var Rfc1SHA512 = PBKDF2.HashPassword(passwordInArray, saltSHA512, numOfRounds1, saltLengthSHA512);

                    var time1MD5 = MeasureRunTime(() => {
                        PBKDF2.HashPassword(passwordInArray, saltMD5, numOfRounds1, saltLengthMD5);
                    });
                    var time1SHA1 = MeasureRunTime(() => {
                        PBKDF2.HashPassword(passwordInArray, saltSHA1, numOfRounds1, saltLengthSHA1);
                    });
                    var time1SHA256 = MeasureRunTime(() => {
                        PBKDF2.HashPassword(passwordInArray, saltSHA256, numOfRounds1, saltLengthSHA256);
                    });
                    var time1SHA384 = MeasureRunTime(() => {
                        PBKDF2.HashPassword(passwordInArray, saltSHA384, numOfRounds1, saltLengthSHA384);
                    });
                    var time1SHA512 = MeasureRunTime(() => {
                        PBKDF2.HashPassword(passwordInArray, saltSHA512, numOfRounds1, saltLengthSHA512);
                    });

                    Console.WriteLine(" Your hashed (with salt + number of rounds 1000) password using ...");       // output
                    Console.WriteLine(" -----------------------------------------------------------------------------------------------------------------------");
                    Console.WriteLine(" 1000 MD5: " + Convert.ToBase64String(Rfc1MD5));
                    Console.WriteLine(" 1000 SHA1: " + Convert.ToBase64String(Rfc1SHA1));
                    Console.WriteLine(" 1000 SHA256: " + Convert.ToBase64String(Rfc1SHA256));
                    Console.WriteLine(" 1000 SHA384: " + Convert.ToBase64String(Rfc1SHA384));
                    Console.WriteLine(" 1000 SHA512: " + Convert.ToBase64String(Rfc1SHA512));
                    Console.WriteLine();
                    Console.WriteLine("  | Press Enter to see more");
                    Console.ReadLine();
                    Console.WriteLine(" Iterations <" + numOfRounds1 + ">, Method <MD5>,    Elapsed Time: " + time1MD5);
                    Console.WriteLine(" Iterations <" + numOfRounds1 + ">  Metod <SHA1>,    Elapsed Time: " + time1SHA1);
                    Console.WriteLine(" Iterations <" + numOfRounds1 + ">, Method <SHA256>, Elapsed Time: " + time1SHA256);
                    Console.WriteLine(" Iterations <" + numOfRounds1 + ">, Method <SHA384>, Elapsed Time: " + time1SHA384);
                    Console.WriteLine(" Iterations <" + numOfRounds1 + ">, Method <SHA512>, Elapsed Time: " + time1SHA512);
                    Console.WriteLine();
                    Console.WriteLine("  | Press Enter to see more");
                    Console.ReadLine();
                    Console.WriteLine();

                    // round 3 = 10 000
                    var Rfc10MD5 = PBKDF2.HashPassword(passwordInArray, saltMD5, numOfRounds10, saltLengthMD5);
                    var Rfc10SHA1 = PBKDF2.HashPassword(passwordInArray, saltSHA1, numOfRounds10, saltLengthSHA1);
                    var Rfc10SHA256 = PBKDF2.HashPassword(passwordInArray, saltSHA256, numOfRounds10, saltLengthSHA256);
                    var Rfc10SHA384 = PBKDF2.HashPassword(passwordInArray, saltSHA384, numOfRounds10, saltLengthSHA384);
                    var Rfc10SHA512 = PBKDF2.HashPassword(passwordInArray, saltSHA512, numOfRounds10, saltLengthSHA512);

                    var time10MD5 = MeasureRunTime(() => {
                        PBKDF2.HashPassword(passwordInArray, saltMD5, numOfRounds10, saltLengthMD5);
                    });
                    var time10SHA1 = MeasureRunTime(() => {
                        PBKDF2.HashPassword(passwordInArray, saltSHA1, numOfRounds10, saltLengthSHA1);
                    });
                    var time10SHA256 = MeasureRunTime(() => {
                        PBKDF2.HashPassword(passwordInArray, saltSHA256, numOfRounds10, saltLengthSHA256);
                    });
                    var time10SHA384 = MeasureRunTime(() => {
                        PBKDF2.HashPassword(passwordInArray, saltSHA384, numOfRounds10, saltLengthSHA384);
                    });
                    var time10SHA512 = MeasureRunTime(() => {
                        PBKDF2.HashPassword(passwordInArray, saltSHA512, numOfRounds10, saltLengthSHA512);
                    });

                    Console.WriteLine(" Your hashed (with salt + number of rounds 10 000) password using ...");       // output
                    Console.WriteLine(" -----------------------------------------------------------------------------------------------------------------------");
                    Console.WriteLine(" 10 000 MD5: " + Convert.ToBase64String(Rfc10MD5));
                    Console.WriteLine(" 10 000 SHA1: " + Convert.ToBase64String(Rfc10SHA1));
                    Console.WriteLine(" 10 000 SHA256: " + Convert.ToBase64String(Rfc10SHA256));
                    Console.WriteLine(" 10 000 SHA384: " + Convert.ToBase64String(Rfc10SHA384));
                    Console.WriteLine(" 10 000 SHA512: " + Convert.ToBase64String(Rfc10SHA512));
                    Console.WriteLine();
                    Console.WriteLine("  | Press Enter to see more");
                    Console.ReadLine();
                    Console.WriteLine(" Iterations <" + numOfRounds10 + ">, Method <MD5>,    Elapsed Time: " + time10MD5);
                    Console.WriteLine(" Iterations <" + numOfRounds10 + ">  Metod <SHA1>,    Elapsed Time: " + time10SHA1);
                    Console.WriteLine(" Iterations <" + numOfRounds10 + ">, Method <SHA256>, Elapsed Time: " + time10SHA256);
                    Console.WriteLine(" Iterations <" + numOfRounds10 + ">, Method <SHA384>, Elapsed Time: " + time10SHA384);
                    Console.WriteLine(" Iterations <" + numOfRounds10 + ">, Method <SHA512>, Elapsed Time: " + time10SHA512);
                    Console.WriteLine();
                    Console.WriteLine("  | Press Enter to see more");
                    Console.ReadLine();
                    Console.WriteLine();

                    // round 4 = 100 000
                    var Rfc100MD5 = PBKDF2.HashPassword(passwordInArray, saltMD5, numOfRounds100, saltLengthMD5);
                    var Rfc100SHA1 = PBKDF2.HashPassword(passwordInArray, saltSHA1, numOfRounds100, saltLengthSHA1);
                    var Rfc100SHA256 = PBKDF2.HashPassword(passwordInArray, saltSHA256, numOfRounds100, saltLengthSHA256);
                    var Rfc100SHA384 = PBKDF2.HashPassword(passwordInArray, saltSHA384, numOfRounds100, saltLengthSHA384);
                    var Rfc100SHA512 = PBKDF2.HashPassword(passwordInArray, saltSHA512, numOfRounds100, saltLengthSHA512);

                    var time100MD5 = MeasureRunTime(() => {
                        PBKDF2.HashPassword(passwordInArray, saltMD5, numOfRounds100, saltLengthMD5);
                    });
                    var time100SHA1 = MeasureRunTime(() => {
                        PBKDF2.HashPassword(passwordInArray, saltSHA1, numOfRounds100, saltLengthSHA1);
                    });       
                    var time100SHA256 = MeasureRunTime(() => {
                        PBKDF2.HashPassword(passwordInArray, saltSHA256, numOfRounds100, saltLengthSHA256);
                    });       
                    var time100SHA384 = MeasureRunTime(() => {
                        PBKDF2.HashPassword(passwordInArray, saltSHA384, numOfRounds100, saltLengthSHA384);
                    });       
                    var time100SHA512 = MeasureRunTime(() => {
                        PBKDF2.HashPassword(passwordInArray, saltSHA512, numOfRounds100, saltLengthSHA512);
                    });

                    Console.WriteLine(" Your hashed (with salt + number of rounds 100 000) password using ...");       // output
                    Console.WriteLine(" -----------------------------------------------------------------------------------------------------------------------");
                    Console.WriteLine(" 100 000 MD5: " + Convert.ToBase64String(Rfc100MD5));
                    Console.WriteLine(" 100 000 SHA1: " + Convert.ToBase64String(Rfc100SHA1));
                    Console.WriteLine(" 100 000 SHA256: " + Convert.ToBase64String(Rfc100SHA256));
                    Console.WriteLine(" 100 000 SHA384: " + Convert.ToBase64String(Rfc100SHA384));
                    Console.WriteLine(" 100 000 SHA512: " + Convert.ToBase64String(Rfc100SHA512));
                    Console.WriteLine();
                    Console.WriteLine("  | Press Enter to see more");
                    Console.ReadLine();
                    Console.WriteLine(" Iterations <" + numOfRounds100 + ">, Method <MD5>,    Elapsed Time: " + time100MD5);
                    Console.WriteLine(" Iterations <" + numOfRounds100 + ">  Metod <SHA1>,    Elapsed Time: " + time100SHA1);
                    Console.WriteLine(" Iterations <" + numOfRounds100 + ">, Method <SHA256>, Elapsed Time: " + time100SHA256);
                    Console.WriteLine(" Iterations <" + numOfRounds100 + ">, Method <SHA384>, Elapsed Time: " + time100SHA384);
                    Console.WriteLine(" Iterations <" + numOfRounds100 + ">, Method <SHA512>, Elapsed Time: " + time100SHA512);
                    Console.WriteLine();
                    Console.WriteLine("  | Press Enter to see more. If nothing appears, you may need to wait a little bit longer");
                    Console.ReadLine();
                    Console.WriteLine();

                    // round 5 = 1 000 000
                    var Rfc1000MD5 = PBKDF2.HashPassword(passwordInArray, saltMD5, numOfRounds1000, saltLengthMD5);
                    var Rfc1000SHA1 = PBKDF2.HashPassword(passwordInArray, saltSHA1, numOfRounds1000, saltLengthSHA1);
                    var Rfc1000SHA256 = PBKDF2.HashPassword(passwordInArray, saltSHA256, numOfRounds1000, saltLengthSHA256);
                    var Rfc1000SHA384 = PBKDF2.HashPassword(passwordInArray, saltSHA384, numOfRounds1000, saltLengthSHA384);
                    var Rfc1000SHA512 = PBKDF2.HashPassword(passwordInArray, saltSHA512, numOfRounds1000, saltLengthSHA512);

                    var time1000MD5 = MeasureRunTime(() => {
                        PBKDF2.HashPassword(passwordInArray, saltMD5, numOfRounds1000, saltLengthMD5);
                    });
                    var time1000SHA1 = MeasureRunTime(() => {
                        PBKDF2.HashPassword(passwordInArray, saltSHA1, numOfRounds1000, saltLengthSHA1);
                    });
                    var time1000SHA256 = MeasureRunTime(() => {
                        PBKDF2.HashPassword(passwordInArray, saltSHA256, numOfRounds1000, saltLengthSHA256);
                    });
                    var time1000SHA384 = MeasureRunTime(() => {
                        PBKDF2.HashPassword(passwordInArray, saltSHA384, numOfRounds1000, saltLengthSHA384);
                    });
                    var time1000SHA512 = MeasureRunTime(() => {
                        PBKDF2.HashPassword(passwordInArray, saltSHA512, numOfRounds1000, saltLengthSHA512);
                    });

                    Console.WriteLine(" Your hashed (with salt + number of rounds 1 000 000) password using ...");       // output
                    Console.WriteLine(" -----------------------------------------------------------------------------------------------------------------------");
                    Console.WriteLine(" 1 000 000 MD5: " + Convert.ToBase64String(Rfc1000MD5));
                    Console.WriteLine(" 1 000 000 SHA1: " + Convert.ToBase64String(Rfc1000SHA1));
                    Console.WriteLine(" 1 000 000 SHA256: " + Convert.ToBase64String(Rfc1000SHA256));
                    Console.WriteLine(" 1 000 000 SHA384: " + Convert.ToBase64String(Rfc1000SHA384));
                    Console.WriteLine(" 1 000 000 SHA512: " + Convert.ToBase64String(Rfc1000SHA512));
                    Console.WriteLine();
                    Console.WriteLine("  | Press Enter to see more");
                    Console.ReadLine();
                    Console.WriteLine(" Iterations <" + numOfRounds1000 + ">, Method <MD5>,    Elapsed Time: " + time1000MD5);
                    Console.WriteLine(" Iterations <" + numOfRounds1000 + ">  Metod <SHA1>,    Elapsed Time: " + time1000SHA1);
                    Console.WriteLine(" Iterations <" + numOfRounds1000 + ">, Method <SHA256>, Elapsed Time: " + time1000SHA256);
                    Console.WriteLine(" Iterations <" + numOfRounds1000 + ">, Method <SHA384>, Elapsed Time: " + time1000SHA384);
                    Console.WriteLine(" Iterations <" + numOfRounds1000 + ">, Method <SHA512>, Elapsed Time: " + time1000SHA512);
                    Console.WriteLine();
                    Console.WriteLine("  | Press Enter to see more");
                    Console.ReadLine();
                    Console.WriteLine();


                }

                else if (option == "3")  
                {
                    // length of keys for each method of hashing
                    int saltLengthMD5 = 16, saltLengthSHA1 = 20, saltLengthSHA256 = 32,
                        saltLengthSHA384 = 48, saltLengthSHA512 = 64;

                    // initializing passwords, usernames
                    string username, password, usernameCheck, passwordCheck, variant;

                    // initializing salt(key) arrays
                    byte[] saltMD5, saltSHA1, saltSHA256, saltSHA384, saltSHA512, passwordInArray;

                    // 10 значень із кроком 50'000; перше значення = номер варіанта * 10'000;
                    int numOfRounds0 = 110000, numOfRounds1 = 160000, numOfRounds2 = 210000,
                        numOfRounds3 = 260000, numOfRounds4 = 310000, numOfRounds5 = 360000,
                        numOfRounds6 = 410000, numOfRounds7 = 460000, numOfRounds8 = 510000,
                        numOfRounds9 = 560000;

                    // user initialisation
                    Console.WriteLine();
                    Console.WriteLine(" Starting user registration ");
                    Console.WriteLine(" -----------------------------------------------------------------------------------------------------------------------");
                    Console.Write(" Enter your username ->  ");
                    username = Console.ReadLine();                          // getting username
                    Console.Write(" Enter your password ->  ");
                    password = Console.ReadLine();                          // getting password
                    Console.WriteLine();
                    Console.WriteLine();

                    passwordInArray = Encoding.Unicode.GetBytes(password);  // putting password in array

                    // making keys (salt)
                    saltMD5 = cryptoKey(saltLengthMD5);
                    saltSHA1 = cryptoKey(saltLengthSHA1);
                    saltSHA256 = cryptoKey(saltLengthSHA256);
                    saltSHA384 = cryptoKey(saltLengthSHA384);
                    saltSHA512 = cryptoKey(saltLengthSHA512);


                    do
                    {
                        Console.WriteLine();
                        Console.WriteLine("  | Chose hashing algorithm (all variants use salt + iteraions) :");
                        Console.WriteLine("  | 1 - MD5");
                        Console.WriteLine("  | 2 - SHA1");
                        Console.WriteLine("  | 3 - SHA256");
                        Console.WriteLine("  | 4 - SHA384");
                        Console.WriteLine("  | 5 - SHA512");
                        Console.WriteLine("  | 0 - exit");
                        Console.WriteLine();
                        Console.Write("   Your variant -> ");
                        variant = Console.ReadLine();
                        Console.WriteLine();

                        if (variant == "1")
                        {
                            Console.WriteLine("  | You may need to wait a little bit");
                            Console.WriteLine();
                            // for MD5 hashes using different round numbers
                            var Rfc0MD5 = PBKDF2.HashPassword(passwordInArray, saltMD5, numOfRounds0, saltLengthMD5);
                            var Rfc1MD5 = PBKDF2.HashPassword(passwordInArray, saltMD5, numOfRounds1, saltLengthMD5);
                            var Rfc2MD5 = PBKDF2.HashPassword(passwordInArray, saltMD5, numOfRounds2, saltLengthMD5);
                            var Rfc3MD5 = PBKDF2.HashPassword(passwordInArray, saltMD5, numOfRounds3, saltLengthMD5);
                            var Rfc4MD5 = PBKDF2.HashPassword(passwordInArray, saltMD5, numOfRounds4, saltLengthMD5);
                            var Rfc5MD5 = PBKDF2.HashPassword(passwordInArray, saltMD5, numOfRounds5, saltLengthMD5);
                            var Rfc6MD5 = PBKDF2.HashPassword(passwordInArray, saltMD5, numOfRounds6, saltLengthMD5);
                            var Rfc7MD5 = PBKDF2.HashPassword(passwordInArray, saltMD5, numOfRounds7, saltLengthMD5);
                            var Rfc8MD5 = PBKDF2.HashPassword(passwordInArray, saltMD5, numOfRounds8, saltLengthMD5);
                            var Rfc9MD5 = PBKDF2.HashPassword(passwordInArray, saltMD5, numOfRounds9, saltLengthMD5);

                            // to know time, needed for each round
                            var time0MD5 = MeasureRunTime(() => {
                                PBKDF2.HashPassword(passwordInArray, saltMD5, numOfRounds0, saltLengthMD5);
                            });
                            var time1MD5 = MeasureRunTime(() => {
                                PBKDF2.HashPassword(passwordInArray, saltMD5, numOfRounds1, saltLengthMD5);
                            });
                            var time2MD5 = MeasureRunTime(() => {
                                PBKDF2.HashPassword(passwordInArray, saltMD5, numOfRounds2, saltLengthMD5);
                            });
                            var time3MD5 = MeasureRunTime(() => {
                                PBKDF2.HashPassword(passwordInArray, saltMD5, numOfRounds3, saltLengthMD5);
                            });
                            var time4MD5 = MeasureRunTime(() => {
                                PBKDF2.HashPassword(passwordInArray, saltMD5, numOfRounds4, saltLengthMD5);
                            });
                            var time5MD5 = MeasureRunTime(() => {
                                PBKDF2.HashPassword(passwordInArray, saltMD5, numOfRounds5, saltLengthMD5);
                            });
                            var time6MD5 = MeasureRunTime(() => {
                                PBKDF2.HashPassword(passwordInArray, saltMD5, numOfRounds6, saltLengthMD5);
                            });
                            var time7MD5 = MeasureRunTime(() => {
                                PBKDF2.HashPassword(passwordInArray, saltMD5, numOfRounds7, saltLengthMD5);
                            });
                            var time8MD5 = MeasureRunTime(() => {
                                PBKDF2.HashPassword(passwordInArray, saltMD5, numOfRounds8, saltLengthMD5);
                            });
                            var time9MD5 = MeasureRunTime(() => {
                                PBKDF2.HashPassword(passwordInArray, saltMD5, numOfRounds9, saltLengthMD5);
                            });

                            // output hashes and time for rounds
                            Console.WriteLine(" Your hashed (using MD5 + different rounds) password using ...");       // output
                            Console.WriteLine(" -----------------------------------------------------------------------------------------------------------------------");
                            Console.WriteLine( " " + numOfRounds0 + " MD5: " + Convert.ToBase64String(Rfc0MD5));
                            Console.WriteLine( " " + numOfRounds1 + " MD5: " + Convert.ToBase64String(Rfc1MD5));
                            Console.WriteLine( " " + numOfRounds2 + " MD5: " + Convert.ToBase64String(Rfc2MD5));
                            Console.WriteLine( " " + numOfRounds3 + " MD5: " + Convert.ToBase64String(Rfc3MD5));
                            Console.WriteLine( " " + numOfRounds4 + " MD5: " + Convert.ToBase64String(Rfc4MD5));
                            Console.WriteLine( " " + numOfRounds5 + " MD5: " + Convert.ToBase64String(Rfc5MD5));
                            Console.WriteLine( " " + numOfRounds6 + " MD5: " + Convert.ToBase64String(Rfc6MD5));
                            Console.WriteLine( " " + numOfRounds7 + " MD5: " + Convert.ToBase64String(Rfc7MD5));
                            Console.WriteLine( " " + numOfRounds8 + " MD5: " + Convert.ToBase64String(Rfc8MD5));
                            Console.WriteLine( " " + numOfRounds9 + " MD5: " + Convert.ToBase64String(Rfc9MD5));
                            Console.WriteLine();
                            Console.WriteLine("  | Press Enter to see more");
                            Console.ReadLine();
                            Console.WriteLine(" Iterations <" + numOfRounds0 + ">, Elapsed Time: " + time0MD5);
                            Console.WriteLine(" Iterations <" + numOfRounds1 + ">, Elapsed Time: " + time1MD5);
                            Console.WriteLine(" Iterations <" + numOfRounds2 + ">, Elapsed Time: " + time2MD5);
                            Console.WriteLine(" Iterations <" + numOfRounds3 + ">, Elapsed Time: " + time3MD5);
                            Console.WriteLine(" Iterations <" + numOfRounds4 + ">, Elapsed Time: " + time4MD5);
                            Console.WriteLine(" Iterations <" + numOfRounds5 + ">, Elapsed Time: " + time5MD5);
                            Console.WriteLine(" Iterations <" + numOfRounds6 + ">, Elapsed Time: " + time6MD5);
                            Console.WriteLine(" Iterations <" + numOfRounds7 + ">, Elapsed Time: " + time7MD5);
                            Console.WriteLine(" Iterations <" + numOfRounds8 + ">, Elapsed Time: " + time8MD5);
                            Console.WriteLine(" Iterations <" + numOfRounds9 + ">, Elapsed Time: " + time9MD5);
                            Console.WriteLine();
                            Console.WriteLine("  | Press Enter to continue");
                            Console.ReadLine();
                            Console.WriteLine();

                        }

                        else if (variant == "2")
                        {
                            Console.WriteLine("  | You may need to wait a little bit");
                            Console.WriteLine();
                            // for SHA1 hashes using different round numbers
                            var Rfc0SHA1 = PBKDF2.HashPassword(passwordInArray, saltSHA1, numOfRounds0, saltLengthSHA1);
                            var Rfc1SHA1 = PBKDF2.HashPassword(passwordInArray, saltSHA1, numOfRounds1, saltLengthSHA1);
                            var Rfc2SHA1 = PBKDF2.HashPassword(passwordInArray, saltSHA1, numOfRounds2, saltLengthSHA1);
                            var Rfc3SHA1 = PBKDF2.HashPassword(passwordInArray, saltSHA1, numOfRounds3, saltLengthSHA1);
                            var Rfc4SHA1 = PBKDF2.HashPassword(passwordInArray, saltSHA1, numOfRounds4, saltLengthSHA1);
                            var Rfc5SHA1 = PBKDF2.HashPassword(passwordInArray, saltSHA1, numOfRounds5, saltLengthSHA1);
                            var Rfc6SHA1 = PBKDF2.HashPassword(passwordInArray, saltSHA1, numOfRounds6, saltLengthSHA1);
                            var Rfc7SHA1 = PBKDF2.HashPassword(passwordInArray, saltSHA1, numOfRounds7, saltLengthSHA1);
                            var Rfc8SHA1 = PBKDF2.HashPassword(passwordInArray, saltSHA1, numOfRounds8, saltLengthSHA1);
                            var Rfc9SHA1 = PBKDF2.HashPassword(passwordInArray, saltSHA1, numOfRounds9, saltLengthSHA1);

                            // to know time, needed for each round
                            var time0SHA1 = MeasureRunTime(() => {
                                PBKDF2.HashPassword(passwordInArray, saltSHA1, numOfRounds0, saltLengthSHA1);
                            });                                          
                            var time1SHA1 = MeasureRunTime(() => {       
                                PBKDF2.HashPassword(passwordInArray, saltSHA1, numOfRounds1, saltLengthSHA1);
                            });                                          
                            var time2SHA1 = MeasureRunTime(() => {       
                                PBKDF2.HashPassword(passwordInArray, saltSHA1, numOfRounds2, saltLengthSHA1);
                            });                                          
                            var time3SHA1 = MeasureRunTime(() => {       
                                PBKDF2.HashPassword(passwordInArray, saltSHA1, numOfRounds3, saltLengthSHA1);
                            });                                          
                            var time4SHA1 = MeasureRunTime(() => {       
                                PBKDF2.HashPassword(passwordInArray, saltSHA1, numOfRounds4, saltLengthSHA1);
                            });                                          
                            var time5SHA1 = MeasureRunTime(() => {       
                                PBKDF2.HashPassword(passwordInArray, saltSHA1, numOfRounds5, saltLengthSHA1);
                            });                                          
                            var time6SHA1 = MeasureRunTime(() => {       
                                PBKDF2.HashPassword(passwordInArray, saltSHA1, numOfRounds6, saltLengthSHA1);
                            });                                          
                            var time7SHA1 = MeasureRunTime(() => {       
                                PBKDF2.HashPassword(passwordInArray, saltSHA1, numOfRounds7, saltLengthSHA1);
                            });                                          
                            var time8SHA1 = MeasureRunTime(() => {       
                                PBKDF2.HashPassword(passwordInArray, saltSHA1, numOfRounds8, saltLengthSHA1);
                            });                                          
                            var time9SHA1 = MeasureRunTime(() => {       
                                PBKDF2.HashPassword(passwordInArray, saltSHA1, numOfRounds9, saltLengthSHA1);
                            });

                            // output hashes and time for rounds
                            Console.WriteLine(" Your hashed (using SHA1 + different rounds) password using ...");       // output
                            Console.WriteLine(" -----------------------------------------------------------------------------------------------------------------------");
                            Console.WriteLine( " " + numOfRounds0 + " SHA1: " + Convert.ToBase64String(Rfc0SHA1));
                            Console.WriteLine( " " + numOfRounds1 + " SHA1: " + Convert.ToBase64String(Rfc1SHA1));
                            Console.WriteLine( " " + numOfRounds2 + " SHA1: " + Convert.ToBase64String(Rfc2SHA1));
                            Console.WriteLine( " " + numOfRounds3 + " SHA1: " + Convert.ToBase64String(Rfc3SHA1));
                            Console.WriteLine( " " + numOfRounds4 + " SHA1: " + Convert.ToBase64String(Rfc4SHA1));
                            Console.WriteLine( " " + numOfRounds5 + " SHA1: " + Convert.ToBase64String(Rfc5SHA1));
                            Console.WriteLine( " " + numOfRounds6 + " SHA1: " + Convert.ToBase64String(Rfc6SHA1));
                            Console.WriteLine( " " + numOfRounds7 + " SHA1: " + Convert.ToBase64String(Rfc7SHA1));
                            Console.WriteLine( " " + numOfRounds8 + " SHA1: " + Convert.ToBase64String(Rfc8SHA1));
                            Console.WriteLine( " " + numOfRounds9 + " SHA1: " + Convert.ToBase64String(Rfc9SHA1));
                            Console.WriteLine();
                            Console.WriteLine("  | Press Enter to see more");
                            Console.ReadLine();
                            Console.WriteLine(" Iterations <" + numOfRounds0 + ">, Elapsed Time: " + time0SHA1);
                            Console.WriteLine(" Iterations <" + numOfRounds1 + ">, Elapsed Time: " + time1SHA1);
                            Console.WriteLine(" Iterations <" + numOfRounds2 + ">, Elapsed Time: " + time2SHA1);
                            Console.WriteLine(" Iterations <" + numOfRounds3 + ">, Elapsed Time: " + time3SHA1);
                            Console.WriteLine(" Iterations <" + numOfRounds4 + ">, Elapsed Time: " + time4SHA1);
                            Console.WriteLine(" Iterations <" + numOfRounds5 + ">, Elapsed Time: " + time5SHA1);
                            Console.WriteLine(" Iterations <" + numOfRounds6 + ">, Elapsed Time: " + time6SHA1);
                            Console.WriteLine(" Iterations <" + numOfRounds7 + ">, Elapsed Time: " + time7SHA1);
                            Console.WriteLine(" Iterations <" + numOfRounds8 + ">, Elapsed Time: " + time8SHA1);
                            Console.WriteLine(" Iterations <" + numOfRounds9 + ">, Elapsed Time: " + time9SHA1);
                            Console.WriteLine();
                            Console.WriteLine("  | Press Enter to continue");
                            Console.ReadLine();
                        }

                        else if (variant == "3")
                        {
                            Console.WriteLine("  | You may need to wait a little bit");
                            Console.WriteLine();
                            // for SHA256 hashes using different round numbers
                            var Rfc0SHA256 = PBKDF2.HashPassword(passwordInArray, saltSHA256, numOfRounds0, saltLengthSHA256);
                            var Rfc1SHA256 = PBKDF2.HashPassword(passwordInArray, saltSHA256, numOfRounds1, saltLengthSHA256);
                            var Rfc2SHA256 = PBKDF2.HashPassword(passwordInArray, saltSHA256, numOfRounds2, saltLengthSHA256);
                            var Rfc3SHA256 = PBKDF2.HashPassword(passwordInArray, saltSHA256, numOfRounds3, saltLengthSHA256);
                            var Rfc4SHA256 = PBKDF2.HashPassword(passwordInArray, saltSHA256, numOfRounds4, saltLengthSHA256);
                            var Rfc5SHA256 = PBKDF2.HashPassword(passwordInArray, saltSHA256, numOfRounds5, saltLengthSHA256);
                            var Rfc6SHA256 = PBKDF2.HashPassword(passwordInArray, saltSHA256, numOfRounds6, saltLengthSHA256);
                            var Rfc7SHA256 = PBKDF2.HashPassword(passwordInArray, saltSHA256, numOfRounds7, saltLengthSHA256);
                            var Rfc8SHA256 = PBKDF2.HashPassword(passwordInArray, saltSHA256, numOfRounds8, saltLengthSHA256);
                            var Rfc9SHA256 = PBKDF2.HashPassword(passwordInArray, saltSHA256, numOfRounds9, saltLengthSHA256);

                            // to know time, needed for each round
                            var time0SHA256 = MeasureRunTime(() => {
                                PBKDF2.HashPassword(passwordInArray, saltSHA256, numOfRounds0, saltLengthSHA256);
                            });
                            var time1SHA256 = MeasureRunTime(() => {
                                PBKDF2.HashPassword(passwordInArray, saltSHA256, numOfRounds1, saltLengthSHA256);
                            });
                            var time2SHA256 = MeasureRunTime(() => {
                                PBKDF2.HashPassword(passwordInArray, saltSHA256, numOfRounds2, saltLengthSHA256);
                            });
                            var time3SHA256 = MeasureRunTime(() => {
                                PBKDF2.HashPassword(passwordInArray, saltSHA256, numOfRounds3, saltLengthSHA256);
                            });
                            var time4SHA256 = MeasureRunTime(() => {
                                PBKDF2.HashPassword(passwordInArray, saltSHA256, numOfRounds4, saltLengthSHA256);
                            });
                            var time5SHA256 = MeasureRunTime(() => {
                                PBKDF2.HashPassword(passwordInArray, saltSHA256, numOfRounds5, saltLengthSHA256);
                            });
                            var time6SHA256 = MeasureRunTime(() => {
                                PBKDF2.HashPassword(passwordInArray, saltSHA256, numOfRounds6, saltLengthSHA256);
                            });
                            var time7SHA256 = MeasureRunTime(() => {
                                PBKDF2.HashPassword(passwordInArray, saltSHA256, numOfRounds7, saltLengthSHA256);
                            });
                            var time8SHA256 = MeasureRunTime(() => {
                                PBKDF2.HashPassword(passwordInArray, saltSHA256, numOfRounds8, saltLengthSHA256);
                            });
                            var time9SHA256 = MeasureRunTime(() => {
                                PBKDF2.HashPassword(passwordInArray, saltSHA256, numOfRounds9, saltLengthSHA256);
                            });

                            // output hashes and time for rounds
                            Console.WriteLine(" Your hashed (using SHA256 + different rounds) password using ...");       // output
                            Console.WriteLine(" -----------------------------------------------------------------------------------------------------------------------");
                            Console.WriteLine( " " + numOfRounds0 + " SHA256: " + Convert.ToBase64String(Rfc0SHA256));
                            Console.WriteLine( " " + numOfRounds1 + " SHA256: " + Convert.ToBase64String(Rfc1SHA256));
                            Console.WriteLine( " " + numOfRounds2 + " SHA256: " + Convert.ToBase64String(Rfc2SHA256));
                            Console.WriteLine( " " + numOfRounds3 + " SHA256: " + Convert.ToBase64String(Rfc3SHA256));
                            Console.WriteLine( " " + numOfRounds4 + " SHA256: " + Convert.ToBase64String(Rfc4SHA256));
                            Console.WriteLine( " " + numOfRounds5 + " SHA256: " + Convert.ToBase64String(Rfc5SHA256));
                            Console.WriteLine( " " + numOfRounds6 + " SHA256: " + Convert.ToBase64String(Rfc6SHA256));
                            Console.WriteLine( " " + numOfRounds7 + " SHA256: " + Convert.ToBase64String(Rfc7SHA256));
                            Console.WriteLine( " " + numOfRounds8 + " SHA256: " + Convert.ToBase64String(Rfc8SHA256));
                            Console.WriteLine( " " + numOfRounds9 + " SHA256: " + Convert.ToBase64String(Rfc9SHA256));
                            Console.WriteLine();
                            Console.WriteLine("  | Press Enter to see more");
                            Console.ReadLine();
                            Console.WriteLine(" Iterations <" + numOfRounds0 + ">, Elapsed Time: " + time0SHA256);
                            Console.WriteLine(" Iterations <" + numOfRounds1 + ">, Elapsed Time: " + time1SHA256);
                            Console.WriteLine(" Iterations <" + numOfRounds2 + ">, Elapsed Time: " + time2SHA256);
                            Console.WriteLine(" Iterations <" + numOfRounds3 + ">, Elapsed Time: " + time3SHA256);
                            Console.WriteLine(" Iterations <" + numOfRounds4 + ">, Elapsed Time: " + time4SHA256);
                            Console.WriteLine(" Iterations <" + numOfRounds5 + ">, Elapsed Time: " + time5SHA256);
                            Console.WriteLine(" Iterations <" + numOfRounds6 + ">, Elapsed Time: " + time6SHA256);
                            Console.WriteLine(" Iterations <" + numOfRounds7 + ">, Elapsed Time: " + time7SHA256);
                            Console.WriteLine(" Iterations <" + numOfRounds8 + ">, Elapsed Time: " + time8SHA256);
                            Console.WriteLine(" Iterations <" + numOfRounds9 + ">, Elapsed Time: " + time9SHA256);
                            Console.WriteLine();
                            Console.WriteLine("  | Press Enter to continue");
                            Console.ReadLine();
                            Console.WriteLine();

                        }

                        else if (variant == "4")
                        {
                            Console.WriteLine("  | You may need to wait a little bit longer");
                            Console.WriteLine();
                            // for SHA384 hashes using different round numbers
                            var Rfc0SHA384 = PBKDF2.HashPassword(passwordInArray, saltSHA384, numOfRounds0, saltLengthSHA384);
                            var Rfc1SHA384 = PBKDF2.HashPassword(passwordInArray, saltSHA384, numOfRounds1, saltLengthSHA384);
                            var Rfc2SHA384 = PBKDF2.HashPassword(passwordInArray, saltSHA384, numOfRounds2, saltLengthSHA384);
                            var Rfc3SHA384 = PBKDF2.HashPassword(passwordInArray, saltSHA384, numOfRounds3, saltLengthSHA384);
                            var Rfc4SHA384 = PBKDF2.HashPassword(passwordInArray, saltSHA384, numOfRounds4, saltLengthSHA384);
                            var Rfc5SHA384 = PBKDF2.HashPassword(passwordInArray, saltSHA384, numOfRounds5, saltLengthSHA384);
                            var Rfc6SHA384 = PBKDF2.HashPassword(passwordInArray, saltSHA384, numOfRounds6, saltLengthSHA384);
                            var Rfc7SHA384 = PBKDF2.HashPassword(passwordInArray, saltSHA384, numOfRounds7, saltLengthSHA384);
                            var Rfc8SHA384 = PBKDF2.HashPassword(passwordInArray, saltSHA384, numOfRounds8, saltLengthSHA384);
                            var Rfc9SHA384 = PBKDF2.HashPassword(passwordInArray, saltSHA384, numOfRounds9, saltLengthSHA384);

                            // to know time, needed for each round
                            var time0SHA384 = MeasureRunTime(() => {
                                PBKDF2.HashPassword(passwordInArray, saltSHA384, numOfRounds0, saltLengthSHA384);
                            });
                            var time1SHA384 = MeasureRunTime(() => {
                                PBKDF2.HashPassword(passwordInArray, saltSHA384, numOfRounds1, saltLengthSHA384);
                            });
                            var time2SHA384 = MeasureRunTime(() => {
                                PBKDF2.HashPassword(passwordInArray, saltSHA384, numOfRounds2, saltLengthSHA384);
                            });
                            var time3SHA384 = MeasureRunTime(() => {
                                PBKDF2.HashPassword(passwordInArray, saltSHA384, numOfRounds3, saltLengthSHA384);
                            });
                            var time4SHA384 = MeasureRunTime(() => {
                                PBKDF2.HashPassword(passwordInArray, saltSHA384, numOfRounds4, saltLengthSHA384);
                            });
                            var time5SHA384 = MeasureRunTime(() => {
                                PBKDF2.HashPassword(passwordInArray, saltSHA384, numOfRounds5, saltLengthSHA384);
                            });
                            var time6SHA384 = MeasureRunTime(() => {
                                PBKDF2.HashPassword(passwordInArray, saltSHA384, numOfRounds6, saltLengthSHA384);
                            });
                            var time7SHA384 = MeasureRunTime(() => {
                                PBKDF2.HashPassword(passwordInArray, saltSHA384, numOfRounds7, saltLengthSHA384);
                            });
                            var time8SHA384 = MeasureRunTime(() => {
                                PBKDF2.HashPassword(passwordInArray, saltSHA384, numOfRounds8, saltLengthSHA384);
                            });
                            var time9SHA384 = MeasureRunTime(() => {
                                PBKDF2.HashPassword(passwordInArray, saltSHA384, numOfRounds9, saltLengthSHA384);
                            });

                            // output hashes and time for rounds
                            Console.WriteLine(" Your hashed (using SHA384 + different rounds) password using ...");       // output
                            Console.WriteLine(" -----------------------------------------------------------------------------------------------------------------------");
                            Console.WriteLine( " " + numOfRounds0 + " SHA384: " + Convert.ToBase64String(Rfc0SHA384));
                            Console.WriteLine( " " + numOfRounds1 + " SHA384: " + Convert.ToBase64String(Rfc1SHA384));
                            Console.WriteLine( " " + numOfRounds2 + " SHA384: " + Convert.ToBase64String(Rfc2SHA384));
                            Console.WriteLine( " " + numOfRounds3 + " SHA384: " + Convert.ToBase64String(Rfc3SHA384));
                            Console.WriteLine( " " + numOfRounds4 + " SHA384: " + Convert.ToBase64String(Rfc4SHA384));
                            Console.WriteLine( " " + numOfRounds5 + " SHA384: " + Convert.ToBase64String(Rfc5SHA384));
                            Console.WriteLine( " " + numOfRounds6 + " SHA384: " + Convert.ToBase64String(Rfc6SHA384));
                            Console.WriteLine( " " + numOfRounds7 + " SHA384: " + Convert.ToBase64String(Rfc7SHA384));
                            Console.WriteLine( " " + numOfRounds8 + " SHA384: " + Convert.ToBase64String(Rfc8SHA384));
                            Console.WriteLine( " " + numOfRounds9 + " SHA384: " + Convert.ToBase64String(Rfc9SHA384));
                            Console.WriteLine();
                            Console.WriteLine("  | Press Enter to see more");
                            Console.ReadLine();
                            Console.WriteLine(" Iterations <" + numOfRounds0 + ">, Elapsed Time: " + time0SHA384);
                            Console.WriteLine(" Iterations <" + numOfRounds1 + ">, Elapsed Time: " + time1SHA384);
                            Console.WriteLine(" Iterations <" + numOfRounds2 + ">, Elapsed Time: " + time2SHA384);
                            Console.WriteLine(" Iterations <" + numOfRounds3 + ">, Elapsed Time: " + time3SHA384);
                            Console.WriteLine(" Iterations <" + numOfRounds4 + ">, Elapsed Time: " + time4SHA384);
                            Console.WriteLine(" Iterations <" + numOfRounds5 + ">, Elapsed Time: " + time5SHA384);
                            Console.WriteLine(" Iterations <" + numOfRounds6 + ">, Elapsed Time: " + time6SHA384);
                            Console.WriteLine(" Iterations <" + numOfRounds7 + ">, Elapsed Time: " + time7SHA384);
                            Console.WriteLine(" Iterations <" + numOfRounds8 + ">, Elapsed Time: " + time8SHA384);
                            Console.WriteLine(" Iterations <" + numOfRounds9 + ">, Elapsed Time: " + time9SHA384);
                            Console.WriteLine();
                            Console.WriteLine("  | Press Enter to continue");
                            Console.ReadLine();
                            Console.WriteLine();

                        }

                        else if (variant == "5")
                        {
                            Console.WriteLine("  | You may need to wait a little bit longer");
                            Console.WriteLine();
                            // for SHA384 hashes using different round numbers
                            var Rfc0SHA512 = PBKDF2.HashPassword(passwordInArray, saltSHA512, numOfRounds0, saltLengthSHA512);
                            var Rfc1SHA512 = PBKDF2.HashPassword(passwordInArray, saltSHA512, numOfRounds1, saltLengthSHA512);
                            var Rfc2SHA512 = PBKDF2.HashPassword(passwordInArray, saltSHA512, numOfRounds2, saltLengthSHA512);
                            var Rfc3SHA512 = PBKDF2.HashPassword(passwordInArray, saltSHA512, numOfRounds3, saltLengthSHA512);
                            var Rfc4SHA512 = PBKDF2.HashPassword(passwordInArray, saltSHA512, numOfRounds4, saltLengthSHA512);
                            var Rfc5SHA512 = PBKDF2.HashPassword(passwordInArray, saltSHA512, numOfRounds5, saltLengthSHA512);
                            var Rfc6SHA512 = PBKDF2.HashPassword(passwordInArray, saltSHA512, numOfRounds6, saltLengthSHA512);
                            var Rfc7SHA512 = PBKDF2.HashPassword(passwordInArray, saltSHA512, numOfRounds7, saltLengthSHA512);
                            var Rfc8SHA512 = PBKDF2.HashPassword(passwordInArray, saltSHA512, numOfRounds8, saltLengthSHA512);
                            var Rfc9SHA512 = PBKDF2.HashPassword(passwordInArray, saltSHA512, numOfRounds9, saltLengthSHA512);

                            // to know time, needed for each round
                            var time0SHA512 = MeasureRunTime(() => {
                                PBKDF2.HashPassword(passwordInArray, saltSHA512, numOfRounds0, saltLengthSHA512);
                            });
                            var time1SHA512 = MeasureRunTime(() => {
                                PBKDF2.HashPassword(passwordInArray, saltSHA512, numOfRounds1, saltLengthSHA512);
                            });
                            var time2SHA512 = MeasureRunTime(() => {
                                PBKDF2.HashPassword(passwordInArray, saltSHA512, numOfRounds2, saltLengthSHA512);
                            });
                            var time3SHA512 = MeasureRunTime(() => {
                                PBKDF2.HashPassword(passwordInArray, saltSHA512, numOfRounds3, saltLengthSHA512);
                            });
                            var time4SHA512 = MeasureRunTime(() => {
                                PBKDF2.HashPassword(passwordInArray, saltSHA512, numOfRounds4, saltLengthSHA512);
                            });
                            var time5SHA512 = MeasureRunTime(() => {
                                PBKDF2.HashPassword(passwordInArray, saltSHA512, numOfRounds5, saltLengthSHA512);
                            });
                            var time6SHA512 = MeasureRunTime(() => {
                                PBKDF2.HashPassword(passwordInArray, saltSHA512, numOfRounds6, saltLengthSHA512);
                            });
                            var time7SHA512 = MeasureRunTime(() => {
                                PBKDF2.HashPassword(passwordInArray, saltSHA512, numOfRounds7, saltLengthSHA512);
                            });
                            var time8SHA512 = MeasureRunTime(() => {
                                PBKDF2.HashPassword(passwordInArray, saltSHA512, numOfRounds8, saltLengthSHA512);
                            });
                            var time9SHA512 = MeasureRunTime(() => {
                                PBKDF2.HashPassword(passwordInArray, saltSHA512, numOfRounds9, saltLengthSHA512);
                            });

                            // output hashes and time for rounds
                            Console.WriteLine(" Your hashed (using SHA512 + different rounds) password using ...");       // output
                            Console.WriteLine(" -----------------------------------------------------------------------------------------------------------------------");
                            Console.WriteLine( " " + numOfRounds0 + " SHA512: " + Convert.ToBase64String(Rfc0SHA512));
                            Console.WriteLine( " " + numOfRounds1 + " SHA512: " + Convert.ToBase64String(Rfc1SHA512));
                            Console.WriteLine( " " + numOfRounds2 + " SHA512: " + Convert.ToBase64String(Rfc2SHA512));
                            Console.WriteLine( " " + numOfRounds3 + " SHA512: " + Convert.ToBase64String(Rfc3SHA512));
                            Console.WriteLine( " " + numOfRounds4 + " SHA512: " + Convert.ToBase64String(Rfc4SHA512));
                            Console.WriteLine( " " + numOfRounds5 + " SHA512: " + Convert.ToBase64String(Rfc5SHA512));
                            Console.WriteLine( " " + numOfRounds6 + " SHA512: " + Convert.ToBase64String(Rfc6SHA512));
                            Console.WriteLine( " " + numOfRounds7 + " SHA512: " + Convert.ToBase64String(Rfc7SHA512));
                            Console.WriteLine( " " + numOfRounds8 + " SHA512: " + Convert.ToBase64String(Rfc8SHA512));
                            Console.WriteLine( " " + numOfRounds9 + " SHA512: " + Convert.ToBase64String(Rfc9SHA512));
                            Console.WriteLine();
                            Console.WriteLine("  | Press Enter to see more");
                            Console.ReadLine();
                            Console.WriteLine(" Iterations <" + numOfRounds0 + ">, Elapsed Time: " + time0SHA512);
                            Console.WriteLine(" Iterations <" + numOfRounds1 + ">, Elapsed Time: " + time1SHA512);
                            Console.WriteLine(" Iterations <" + numOfRounds2 + ">, Elapsed Time: " + time2SHA512);
                            Console.WriteLine(" Iterations <" + numOfRounds3 + ">, Elapsed Time: " + time3SHA512);
                            Console.WriteLine(" Iterations <" + numOfRounds4 + ">, Elapsed Time: " + time4SHA512);
                            Console.WriteLine(" Iterations <" + numOfRounds5 + ">, Elapsed Time: " + time5SHA512);
                            Console.WriteLine(" Iterations <" + numOfRounds6 + ">, Elapsed Time: " + time6SHA512);
                            Console.WriteLine(" Iterations <" + numOfRounds7 + ">, Elapsed Time: " + time7SHA512);
                            Console.WriteLine(" Iterations <" + numOfRounds8 + ">, Elapsed Time: " + time8SHA512);
                            Console.WriteLine(" Iterations <" + numOfRounds9 + ">, Elapsed Time: " + time9SHA512);
                            Console.WriteLine();
                            Console.WriteLine("  | Press Enter to continue");
                            Console.ReadLine();
                            Console.WriteLine();

                        }


                    } while (variant != "0");

                }

                else if (option == "4")
                {
                    // length of keys for each method of hashing
                    int saltLengthMD5 = 16, saltLengthSHA1 = 20, saltLengthSHA256 = 32,
                        saltLengthSHA384 = 48, saltLengthSHA512 = 64;

                    // initializing passwords, usernames
                    string username, password, usernameCheck, passwordCheck, choose;

                    // initializing salt(key) arrays
                    byte[] saltMD5, saltSHA1, saltSHA256, saltSHA384, saltSHA512, passwordInArray, passwordCheckInArray;

                    // 10 значень із кроком 50'000; перше значення = номер варіанта * 10'000;
                    int numOfRounds0 = 110000;

                    // user initialisation
                    Console.WriteLine();
                    Console.WriteLine(" Starting user registration ");
                    Console.WriteLine(" -----------------------------------------------------------------------------------------------------------------------");
                    Console.Write(" Enter your username ->  ");
                    username = Console.ReadLine();                          // getting username
                    Console.Write(" Enter your password ->  ");
                    password = Console.ReadLine();                          // getting password
                    Console.WriteLine();

                    passwordInArray = Encoding.Unicode.GetBytes(password);  // putting password in array

                    // making keys (salt)
                    saltMD5 = cryptoKey(saltLengthMD5);
                    saltSHA1 = cryptoKey(saltLengthSHA1);
                    saltSHA256 = cryptoKey(saltLengthSHA256);
                    saltSHA384 = cryptoKey(saltLengthSHA384);
                    saltSHA512 = cryptoKey(saltLengthSHA512);


                    do
                    {
                        Console.WriteLine();
                        Console.WriteLine("  | Chose hashing algorithm (all variants use salt + iteraions) for authentification process: ");
                        Console.WriteLine("  | 1 - MD5");
                        Console.WriteLine("  | 2 - SHA1");
                        Console.WriteLine("  | 3 - SHA256");
                        Console.WriteLine("  | 4 - SHA384");
                        Console.WriteLine("  | 5 - SHA512");
                        Console.WriteLine("  | 0 - exit");
                        Console.WriteLine();
                        Console.Write("   Your variant -> ");
                        choose = Console.ReadLine();
                        Console.WriteLine();

                        if (choose == "1")
                        {
                            // for MD5 hashes using different round numbers
                            var Rfc0MD5 = PBKDF2.HashPassword(passwordInArray, saltMD5, numOfRounds0, saltLengthMD5);
                            
                            // to know time, needed for each round
                            var time0MD5 = MeasureRunTime(() => {
                                PBKDF2.HashPassword(passwordInArray, saltMD5, numOfRounds0, saltLengthMD5);
                            });
                            

                            // output hashes and time for rounds
                            Console.WriteLine(" Your hashed (using MD5 + different rounds) password using ...");       // output
                            Console.WriteLine(" -----------------------------------------------------------------------------------------------------------------------");
                            Console.WriteLine(" " + numOfRounds0 + " MD5: " + Convert.ToBase64String(Rfc0MD5));
                            Console.WriteLine(" Iterations <" + numOfRounds0 + ">, Elapsed Time: " + time0MD5);
                            Console.WriteLine();
                            Console.WriteLine("  | Press Enter to continue");
                            Console.ReadLine();
                            Console.WriteLine();


                            // authentification starts
                            Console.WriteLine(" Starting user authentification ");
                            Console.WriteLine(" -----------------------------------------------------------------------------------------------------------------------");
                            Console.Write(" Enter your username ->  ");
                            usernameCheck = Console.ReadLine();               // getting username
                            Console.Write(" Enter your password ->  ");
                            passwordCheck = Console.ReadLine();               // getting password
                            Console.WriteLine();

                            passwordCheckInArray = Encoding.Unicode.GetBytes(passwordCheck);   // passwordCheck in array

                            var Rfc1MD5 = PBKDF2.HashPassword(passwordCheckInArray, saltMD5, numOfRounds0, saltLengthMD5); // hashing
                            var time1MD5 = MeasureRunTime(() => {          // to know time, needed for each round
                                PBKDF2.HashPassword(passwordCheckInArray, saltMD5, numOfRounds0, saltLengthMD5);
                            });

                            Console.WriteLine(" Your hashed (using MD5 + different rounds) password using ...");       // output
                            Console.WriteLine(" -----------------------------------------------------------------------------------------------------------------------");
                            Console.WriteLine(" " + numOfRounds0 + " MD5: " + Convert.ToBase64String(Rfc1MD5));
                            Console.WriteLine(" Iterations <" + numOfRounds0 + ">, Elapsed Time: " + time1MD5);
                            Console.WriteLine();
                            Console.WriteLine("  | Press Enter to continue authentification");
                            Console.ReadLine();
                            Console.WriteLine();

                            Console.WriteLine(" Checking your hashed password  ...");
                            Console.WriteLine(" -----------------------------------------------------------------------------------------------------------------------");
                            Console.Write(CheckHash(Rfc0MD5, Rfc1MD5, username, usernameCheck));
                            Console.WriteLine();
                            Console.WriteLine("  | Press Enter to continue");
                            Console.ReadLine();
                            Console.WriteLine();

                        }

                        else if (choose == "2")
                        {
                            // for SHA1 hashes using different round numbers
                            var Rfc0SHA1 = PBKDF2.HashPassword(passwordInArray, saltSHA1, numOfRounds0, saltLengthSHA1);

                            // to know time, needed for each round
                            var time0SHA1 = MeasureRunTime(() => {
                                PBKDF2.HashPassword(passwordInArray, saltSHA1, numOfRounds0, saltLengthSHA1);
                            });

                            // output hashes and time for rounds
                            Console.WriteLine(" Your hashed (using SHA1 + different rounds) password using ...");       // output
                            Console.WriteLine(" -----------------------------------------------------------------------------------------------------------------------");
                            Console.WriteLine(" " + numOfRounds0 + " SHA1: " + Convert.ToBase64String(Rfc0SHA1));
                            Console.WriteLine(" Iterations <" + numOfRounds0 + ">, Elapsed Time: " + time0SHA1);
                            Console.WriteLine();
                            Console.WriteLine("  | Press Enter to continue");
                            Console.ReadLine();


                            // authentification starts
                            Console.WriteLine(" Starting user authentification ");
                            Console.WriteLine(" -----------------------------------------------------------------------------------------------------------------------");
                            Console.Write(" Enter your username ->  ");
                            usernameCheck = Console.ReadLine();               // getting username
                            Console.Write(" Enter your password ->  ");
                            passwordCheck = Console.ReadLine();               // getting password
                            Console.WriteLine();

                            passwordCheckInArray = Encoding.Unicode.GetBytes(passwordCheck);   // passwordCheck in array

                            var Rfc1SHA1 = PBKDF2.HashPassword(passwordCheckInArray, saltSHA1, numOfRounds0, saltLengthSHA1); // hashing
                            var time1SHA1 = MeasureRunTime(() => {          // to know time, needed for each round
                                PBKDF2.HashPassword(passwordCheckInArray, saltSHA1, numOfRounds0, saltLengthSHA1);
                            });

                            Console.WriteLine(" Your hashed (using SHA1 + different rounds) password using ...");       // output
                            Console.WriteLine(" -----------------------------------------------------------------------------------------------------------------------");
                            Console.WriteLine(" " + numOfRounds0 + " SHA1: " + Convert.ToBase64String(Rfc1SHA1));
                            Console.WriteLine(" Iterations <" + numOfRounds0 + ">, Elapsed Time: " + time1SHA1);
                            Console.WriteLine();
                            Console.WriteLine("  | Press Enter to continue authentification");
                            Console.ReadLine();
                            Console.WriteLine();

                            Console.WriteLine(" Checking your hashed password  ...");
                            Console.WriteLine(" -----------------------------------------------------------------------------------------------------------------------");
                            Console.Write(CheckHash(Rfc0SHA1, Rfc1SHA1, username, usernameCheck));
                            Console.WriteLine();
                            Console.WriteLine("  | Press Enter to continue");
                            Console.ReadLine();
                            Console.WriteLine();

                        }

                        else if (choose == "3")
                        {
                            // for SHA256 hashes using different round numbers
                            var Rfc0SHA256 = PBKDF2.HashPassword(passwordInArray, saltSHA256, numOfRounds0, saltLengthSHA256);

                            // to know time, needed for each round
                            var time0SHA256 = MeasureRunTime(() => {
                                PBKDF2.HashPassword(passwordInArray, saltSHA256, numOfRounds0, saltLengthSHA256);
                            });

                            // output hashes and time for rounds
                            Console.WriteLine(" Your hashed (using SHA256 + different rounds) password using ...");       // output
                            Console.WriteLine(" -----------------------------------------------------------------------------------------------------------------------");
                            Console.WriteLine(" " + numOfRounds0 + " SHA256: " + Convert.ToBase64String(Rfc0SHA256));
                            Console.WriteLine(" Iterations <" + numOfRounds0 + ">, Elapsed Time: " + time0SHA256);
                            Console.WriteLine();
                            Console.WriteLine("  | Press Enter to continue");
                            Console.ReadLine();
                            Console.WriteLine();


                            // authentification starts
                            Console.WriteLine(" Starting user authentification ");
                            Console.WriteLine(" -----------------------------------------------------------------------------------------------------------------------");
                            Console.Write(" Enter your username ->  ");
                            usernameCheck = Console.ReadLine();               // getting username
                            Console.Write(" Enter your password ->  ");
                            passwordCheck = Console.ReadLine();               // getting password
                            Console.WriteLine();

                            passwordCheckInArray = Encoding.Unicode.GetBytes(passwordCheck);   // passwordCheck in array

                            var Rfc1SHA256 = PBKDF2.HashPassword(passwordCheckInArray, saltSHA256, numOfRounds0, saltLengthSHA256); // hashing
                            var time1SHA256 = MeasureRunTime(() => {          // to know time, needed for each round
                                PBKDF2.HashPassword(passwordCheckInArray, saltSHA256, numOfRounds0, saltLengthSHA256);
                            });

                            Console.WriteLine(" Your hashed (using SHA256 + different rounds) password using ...");       // output
                            Console.WriteLine(" -----------------------------------------------------------------------------------------------------------------------");
                            Console.WriteLine(" " + numOfRounds0 + " SHA256: " + Convert.ToBase64String(Rfc1SHA256));
                            Console.WriteLine(" Iterations <" + numOfRounds0 + ">, Elapsed Time: " + time1SHA256);
                            Console.WriteLine();
                            Console.WriteLine("  | Press Enter to continue authentification");
                            Console.ReadLine();
                            Console.WriteLine();

                            Console.WriteLine(" Checking your hashed password  ...");
                            Console.WriteLine(" -----------------------------------------------------------------------------------------------------------------------");
                            Console.Write(CheckHash(Rfc0SHA256, Rfc1SHA256, username, usernameCheck));
                            Console.WriteLine();
                            Console.WriteLine("  | Press Enter to continue");
                            Console.ReadLine();
                            Console.WriteLine();

                        }

                        else if (choose == "4")
                        {
                            Console.WriteLine("  | You may need to wait a little bit longer");
                            Console.WriteLine();
                            // for SHA384 hashes using different round numbers
                            var Rfc0SHA384 = PBKDF2.HashPassword(passwordInArray, saltSHA384, numOfRounds0, saltLengthSHA384);

                            // to know time, needed for each round
                            var time0SHA384 = MeasureRunTime(() => {
                                PBKDF2.HashPassword(passwordInArray, saltSHA384, numOfRounds0, saltLengthSHA384);
                            });

                            // output hashes and time for rounds
                            Console.WriteLine(" Your hashed (using SHA384 + different rounds) password using ...");       // output
                            Console.WriteLine(" -----------------------------------------------------------------------------------------------------------------------");
                            Console.WriteLine(" " + numOfRounds0 + " SHA384: " + Convert.ToBase64String(Rfc0SHA384));
                            Console.WriteLine(" Iterations <" + numOfRounds0 + ">, Elapsed Time: " + time0SHA384);
                            Console.WriteLine();
                            Console.WriteLine("  | Press Enter to continue");
                            Console.ReadLine();
                            Console.WriteLine();

                            // authentification starts
                            Console.WriteLine(" Starting user authentification ");
                            Console.WriteLine(" -----------------------------------------------------------------------------------------------------------------------");
                            Console.Write(" Enter your username ->  ");
                            usernameCheck = Console.ReadLine();               // getting username
                            Console.Write(" Enter your password ->  ");
                            passwordCheck = Console.ReadLine();               // getting password
                            Console.WriteLine();

                            passwordCheckInArray = Encoding.Unicode.GetBytes(passwordCheck);   // passwordCheck in array

                            var Rfc1SHA384 = PBKDF2.HashPassword(passwordCheckInArray, saltSHA384, numOfRounds0, saltLengthSHA384); // hashing
                            var time1SHA384 = MeasureRunTime(() => {          // to know time, needed for each round
                                PBKDF2.HashPassword(passwordCheckInArray, saltSHA384, numOfRounds0, saltLengthSHA384);
                            });

                            Console.WriteLine(" Your hashed (using SHA384 + different rounds) password using ...");       // output
                            Console.WriteLine(" -----------------------------------------------------------------------------------------------------------------------");
                            Console.WriteLine(" " + numOfRounds0 + " SHA384: " + Convert.ToBase64String(Rfc1SHA384));
                            Console.WriteLine(" Iterations <" + numOfRounds0 + ">, Elapsed Time: " + time1SHA384);
                            Console.WriteLine();
                            Console.WriteLine("  | Press Enter to continue authentification");
                            Console.ReadLine();
                            Console.WriteLine();

                            Console.WriteLine(" Checking your hashed password  ...");
                            Console.WriteLine(" -----------------------------------------------------------------------------------------------------------------------");
                            Console.Write(CheckHash(Rfc0SHA384, Rfc1SHA384, username, usernameCheck));
                            Console.WriteLine();
                            Console.WriteLine("  | Press Enter to continue");
                            Console.ReadLine();
                            Console.WriteLine();

                        }

                        else if (choose == "5")
                        {
                            // for SHA384 hashes using different round numbers
                            var Rfc0SHA512 = PBKDF2.HashPassword(passwordInArray, saltSHA512, numOfRounds0, saltLengthSHA512);

                            // to know time, needed for each round
                            var time0SHA512 = MeasureRunTime(() => {
                                PBKDF2.HashPassword(passwordInArray, saltSHA512, numOfRounds0, saltLengthSHA512);
                            });

                            // output hashes and time for rounds
                            Console.WriteLine(" Your hashed (using SHA512 + different rounds) password using ...");       // output
                            Console.WriteLine(" -----------------------------------------------------------------------------------------------------------------------");
                            Console.WriteLine(" " + numOfRounds0 + " SHA512: " + Convert.ToBase64String(Rfc0SHA512));
                            Console.WriteLine(" Iterations <" + numOfRounds0 + ">, Elapsed Time: " + time0SHA512);
                            Console.WriteLine();
                            Console.WriteLine("  | Press Enter to continue");
                            Console.ReadLine();
                            Console.WriteLine();


                            // authentification starts
                            Console.WriteLine(" Starting user authentification ");
                            Console.WriteLine(" -----------------------------------------------------------------------------------------------------------------------");
                            Console.Write(" Enter your username ->  ");
                            usernameCheck = Console.ReadLine();               // getting username
                            Console.Write(" Enter your password ->  ");
                            passwordCheck = Console.ReadLine();               // getting password
                            Console.WriteLine();

                            passwordCheckInArray = Encoding.Unicode.GetBytes(passwordCheck);   // passwordCheck in array

                            var Rfc1SHA512 = PBKDF2.HashPassword(passwordCheckInArray, saltSHA512, numOfRounds0, saltLengthSHA512); // hashing
                            var time1SHA512 = MeasureRunTime(() => {          // to know time, needed for each round
                                PBKDF2.HashPassword(passwordCheckInArray, saltSHA512, numOfRounds0, saltLengthSHA512);
                            });

                            Console.WriteLine(" Your hashed (using SHA512 + different rounds) password using ...");       // output
                            Console.WriteLine(" -----------------------------------------------------------------------------------------------------------------------");
                            Console.WriteLine(" " + numOfRounds0 + " SHA512: " + Convert.ToBase64String(Rfc1SHA512));
                            Console.WriteLine(" Iterations <" + numOfRounds0 + ">, Elapsed Time: " + time1SHA512);
                            Console.WriteLine();
                            Console.WriteLine("  | Press Enter to continue authentification");
                            Console.ReadLine();
                            Console.WriteLine();

                            Console.WriteLine(" Checking your hashed password  ...");
                            Console.WriteLine(" -----------------------------------------------------------------------------------------------------------------------");
                            Console.Write(CheckHash(Rfc0SHA512, Rfc1SHA512, username, usernameCheck));
                            Console.WriteLine();
                            Console.WriteLine("  | Press Enter to continue");
                            Console.ReadLine();
                            Console.WriteLine();

                        }

                    } while (choose != "0");

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

        // checking hash and login for accuracy
        public static string CheckHash(byte[] hash1, byte[] hash2, string login1, string login2)
        {
            if (login1 == login2 && Convert.ToBase64String(hash1) == Convert.ToBase64String(hash2))
            {
                Console.WriteLine(" Login is accurate");
                Console.WriteLine(" Hash of password is accurate");
                Console.WriteLine();
                Console.WriteLine("  | Access permitted. Welcome to the system");

            }
            else if (login1 == login2 && Convert.ToBase64String(hash1) != Convert.ToBase64String(hash2))
            {
                Console.WriteLine(" Login is accurate");
                Console.WriteLine(" Hash isn`t accurate. Password is corrupted");
                Console.WriteLine();
                Console.WriteLine("  | Access denied. You were wrong");
            }
            else if (login1 != login2 && Convert.ToBase64String(hash1) == Convert.ToBase64String(hash2))
            {
                Console.WriteLine(" Login isn`t accurate");
                Console.WriteLine(" Hash is accurate. Password is correct");
                Console.WriteLine();
                Console.WriteLine("  | Access denied. You were wrong");
            }
            else
            {
                Console.WriteLine(" Login isn`t accurate");
                Console.WriteLine(" Hash isn`t accurate. Password is corrupted");
                Console.WriteLine();
                Console.WriteLine("  | Access denied. You were totally wrong");
            }
            return " ";
        }

        // generating salt
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

        // password based key deprivation function
        public class PBKDF2
        {
            public static byte[] HashPassword(byte[] toBeHashed, byte[] salt, int numberOfRounds, int bytes)
            {
                using (var rfc2898 = new Rfc2898DeriveBytes(toBeHashed, salt, numberOfRounds))
                {
                    return rfc2898.GetBytes(bytes);
                }
            }
        }

        // for time counting
        public static TimeSpan MeasureRunTime(Action codeToRun)
        {
            var watch = Stopwatch.StartNew();
            codeToRun();
            watch.Stop();
            return watch.Elapsed;
        }

    }
}

