using System;
using System.Text;
using System.Security.Cryptography;
using static System.Net.Mime.MediaTypeNames;

namespace lab7
{
    class Program
    {
        static void Main(string[] args)
        {
            // initializing string options, data
            string option, variant, saveOrNo, nameOrNo, encrYourOrNo, nameOfEncrFile,
                   yourEncrFileName, yourPrivateFileName, encrypted, data,
                   encrStr, decrStr;

            // paths
            string path = "D:/Lessons/2nd Year/Основи інформаційної безпеки/Labs/Lab 7/lab 7";
            string publicKeyPath = path + "/publicKey.txt";
            string privateKeyPath = path + "/privateKey.txt";
            string encrFilePath2 = path + "/encryptedText.txt";     // full path for XML case
            string nameOfEncrFile2 = "encryptedText.txt";           // just name of encrypted file
            
            // initializing arrays
            byte[] dataInArray, encrInArray;

            do
            {
                Console.WriteLine();
                Console.WriteLine("  | Options:");
                Console.WriteLine("  | 1 - part 1, asymetryc encryption (RSA - In-memory keys)");
                Console.WriteLine("  | 2 - part 2, asymetryc encryption (XML - Public key in file)");
                Console.WriteLine("  | 3 - get path");
                Console.WriteLine("  | 0 - exit");
                Console.WriteLine();
                Console.Write("   Your option -> ");
                option = Console.ReadLine();

                // RSA ENCRYPTION
                if (option == "1")
                {
                    var rsaParams = new InMemory();

                    Console.WriteLine();  // Starting data encryption
                    Console.WriteLine(" Starting data encryption (RSA - saving keys in Memory)");
                    Console.WriteLine(" -----------------------------------------------------------------------------------------------------------------------");
                    Console.Write(" Enter your data ->  ");
                    data = Console.ReadLine();                          // getting data
                    Console.WriteLine();
                    Console.WriteLine();

                    dataInArray = Encoding.Unicode.GetBytes(data);      // putting data in array

                    rsaParams.AssignNewKeyInMemory();                   // assigning new keys (public, private)
                    var encryptedInArray = rsaParams.EncryptData(dataInArray);
                    var decryptedInArray = rsaParams.DecryptData(encryptedInArray);                    

                    Console.WriteLine(" RSA Encryption");
                    otherFunctions.mainOutput(encryptedInArray, dataInArray, decryptedInArray);   // main output
                }

                // XML ENCRYPTION
                if (option == "2")
                {
                    do
                    {
                        Console.WriteLine();  // Starting data encryption
                        Console.WriteLine(" Starting data encryption (XML - keys in files)");
                        Console.WriteLine(" -----------------------------------------------------------------------------------------------------------------------");
                        Console.WriteLine();
                        Console.WriteLine("  | Choose variant: ");
                        Console.WriteLine("  | 1 - enter data from line ");
                        Console.WriteLine("  | 2 - choose file with data ");
                        Console.WriteLine("  | 0 - exit");
                        Console.WriteLine();
                        Console.Write("  Your variant ->  ");
                        variant = Console.ReadLine();
                        Console.WriteLine();

                        //  ENTER DATA FROM LINE
                        if (variant == "1")
                        {
                            var xmlParams = new InFile();

                            otherFunctions.keysInFileOutput();
                            Console.Write(" Enter your data ->  ");
                            data = Console.ReadLine();                          // getting data
                            Console.WriteLine();
                            Console.WriteLine();

                            dataInArray = Encoding.Unicode.GetBytes(data);      // putting data in array

                            xmlParams.AssignNewKeyInFile(publicKeyPath, privateKeyPath);  // making + writing keys in files
                            var encryptedInArray = xmlParams.EncryptDataInFile(publicKeyPath, dataInArray);  // encryption
                            var decryptedInArray = xmlParams.DecryptDataInFile(privateKeyPath, encryptedInArray); // decryption

                            Console.WriteLine(" XML Encryption (data entered from the line");
                            otherFunctions.mainOutput(encryptedInArray, dataInArray, decryptedInArray);   // main output

                            encrypted = Convert.ToBase64String(encryptedInArray); // encrypted staring

                            Console.Write(" Do you want to save your encrypted message in a file? (yes|no|default) -> ");
                            saveOrNo = Console.ReadLine();
                            Console.WriteLine();

                            
                            switch (saveOrNo)
                            {
                                case "yes":
                                    Console.Write(" Do you want to choose a name for this file? (yes|no) -> ");
                                    nameOrNo = Console.ReadLine();
                                    switch (nameOrNo)
                                    {
                                        case "yes":
                                            Console.Write(" Enter name for your file (WITHOUT .txt, only name) -> ");
                                            nameOfEncrFile = Console.ReadLine();
                                            string encrFilePath1 = path + "/" + nameOfEncrFile + ".txt";     // path to the file
                                            File.WriteAllText(encrFilePath1, encrypted);                     // encrypted data to the file
                                            string nameOfEncrFileFull = nameOfEncrFile + ".txt";
                                            otherFunctions.fileNameOutput(nameOfEncrFileFull);               // data into file output
                                            break;

                                        case "no":
                                            
                                            File.WriteAllText(encrFilePath2, encrypted);        // encrypted data to the file
                                            otherFunctions.fileNameOutput(nameOfEncrFile2);      // data into file output
                                            break;
                                    }
                                    break;

                                case "no":
                                    break;

                                case "default":
                                    File.WriteAllText(encrFilePath2, encrypted);
                                    otherFunctions.fileNameOutput(nameOfEncrFile2);  // data into file output
                                    break;
                            }


                        }  // end of variant 1
                        
                        // TAKE DATA FROM FILE
                        if (variant == "2")
                        {
                            var xmlParam = new InFile();

                            Console.Write(" Do you want to choose your own file NAMES to work with? (yes|no) -> ");
                            encrYourOrNo = Console.ReadLine();
                            Console.WriteLine();
                            switch (encrYourOrNo)
                            {
                                case "yes":
                                    Console.Write(" Enter name of encrypted txt file (without .txt; def - encryptedText) -> ");
                                    yourEncrFileName = Console.ReadLine();
                                    Console.Write(" Enter name of private key txt file (without .txt; def - privateKey) -> ");
                                    yourPrivateFileName = Console.ReadLine();
                                    Console.WriteLine();
                                    string encrPathYes = path + "/" + yourEncrFileName + ".txt";           // encr path
                                    string privatePathYes = path + "/" + yourPrivateFileName + ".txt";     // private path
                                    
                                    encrStr = File.ReadAllText(encrPathYes);            // encrypted string
                                    encrInArray = Encoding.Unicode.GetBytes(encrStr);     // encrypted byte array
                                    
                                    var decryptedInArray = xmlParam.DecryptDataInFile(privatePathYes, encrInArray);  // decrypted byte array
                                    decrStr = Encoding.Default.GetString(decryptedInArray);                           // decrypted string
                                    Console.WriteLine();
                                    Console.WriteLine(" Your decrepted message:  " + decrStr);
                                    otherFunctions.prettyOutput(2);
                                    
                                    break;

                                case "no":
                                    string encrPathNo = path + "/encryptedText.txt";     // encr path
                                    //string privatePathNo = path + "/privateKey.txt";     // private path

                                    encrStr = File.ReadAllText(encrPathNo);              // encrypted string
                                    Console.WriteLine(encrStr);
                                    encrInArray = Encoding.Unicode.GetBytes(encrStr);    // encrypted byte array

                                    Console.WriteLine(encrInArray);

                                    var decryptedInArrayNo = xmlParam.DecryptDataInFile(privateKeyPath, encrInArray);  // decrypted byte array
                                    
                                    decrStr = Encoding.Default.GetString(decryptedInArrayNo);
                                    Console.WriteLine();
                                    Console.WriteLine(" Your decrepted message:  " + decrStr);
                                    otherFunctions.prettyOutput(2);
                                    break;
                            }

                        }
                    }while(variant != "0");
                }

                //if (option == "3")
                //{
                //    var path = ".";
                //    var fullPath = Path.GetFullPath(path);
                //    Console.WriteLine(fullPath);
                //}

            } while (option != "0");
        }
    }


    // RSA ENCRYPTION
    public class InMemory
    {
        private RSAParameters _publicKey, _privateKey;        // creating public, private key + keeping them in objects in memory

        // FUNCTION FOR ASSIGNING KEYS (IN MEMORY)
        public void AssignNewKeyInMemory()
        {
            using (var rsa = new RSACryptoServiceProvider(2048))  // () - length for key size
            {
                rsa.PersistKeyInCsp = false;                    // To export the key material, you call the ExportParameters method
                _publicKey = rsa.ExportParameters(false);          // to export public key - pass FALSE into ExportParameters
                _privateKey = rsa.ExportParameters(true);          // to export private key - pass TRUE
            }
        }

        // TO ENCRYPT DATA 
        public byte[] EncryptData(byte[] dataToEncrypt)
        {
            byte[] cipherbytes;
            using (var rsa = new RSACryptoServiceProvider())      // createa new instance of RSACryptoServiceProvider
            {
                rsa.PersistKeyInCsp = false;                      // make sure, that this is set to false
                rsa.ImportParameters(_publicKey);                 // getting public key to encrypt
                cipherbytes = rsa.Encrypt(dataToEncrypt, true);   // encryption - call the Encrypt method on the RSACryptoServiceProvider object. true here = optimal asymmetric encryption padding(OEAP) - for randomisation
            }
            return cipherbytes;
        }
        // TO DECRYPT DATA
        public byte[] DecryptData(byte[] dataToEncrypt)
        {
            byte[] plain;
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.PersistKeyInCsp = false;
                rsa.ImportParameters(_privateKey);                 // getting private key for decryption
                plain = rsa.Decrypt(dataToEncrypt, true);          // call Decrypt
            }
            return plain;
        }
    }


    // XML ENCRYPTION
    public class InFile
    {
        // FOR ASSIGNING NEW KEYS IN FILES
        public void AssignNewKeyInFile(string publicKeyPath, string privateKeyPath)  // FUNCTION FOR ASSIGNING KEYS (IN FILES)
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;
                File.WriteAllText(publicKeyPath, rsa.ToXmlString(false));     // WriteAllText - to save out the XML text of the public and private keys to disk
                File.WriteAllText(privateKeyPath, rsa.ToXmlString(true));     // ToXmlString - to export the actual key material
            }
        }

        // TO ENCRYPT SOME DATA IN FILE
        public byte[] EncryptDataInFile(string publicKeyPath, byte[] dataToEncrypt)
        {
            byte[] cipherbytes;
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;
                rsa.FromXmlString(File.ReadAllText(publicKeyPath));   // we call FromXmlString and pass in the results of loading the key from disk
                cipherbytes = rsa.Encrypt(dataToEncrypt, false);      // encryption
            }
            return cipherbytes;
        }
        // TO DECRYPT SOME DATA IN FILE
        public byte[] DecryptDataInFile(string privateKeyPath, byte[] dataToDecrypt)
        {
            byte[] plain;
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;
                rsa.FromXmlString(File.ReadAllText(privateKeyPath));
                plain = rsa.Decrypt(dataToDecrypt, false);            // decryption
            }
            return plain;
        }
    }



    // SOME OUTPUT FUNCTIONS + CHECK FUNCTION
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

        public static void mainOutput(byte[] encrypted, byte[] dataInArray, byte[] decrypted)  // main output
        {
            Console.WriteLine(" -----------------------------------------------------------------------------------------------------------------------");
            //Console.WriteLine();
            Console.WriteLine(" Encrypted Text:  " + Convert.ToBase64String(encrypted));
            otherFunctions.prettyOutput(1);                                                     // some output for better look
            Console.WriteLine(" Decrypted Text:  " + Encoding.Default.GetString(decrypted));
            otherFunctions.prettyOutput(1);
            otherFunctions.CheckData(dataInArray, decrypted);
            otherFunctions.prettyOutput(2);
        }
        public static void keysInFileOutput()   // for XML encryption
        {
            Console.WriteLine("Your keys were generated and put into files");
            otherFunctions.prettyOutput(1);
        }
        public static void fileNameOutput(string nameOfEncrFile)
        {
            Console.WriteLine();
            Console.WriteLine("Your " + nameOfEncrFile + " file (with encrypted data) was successfully made");
            otherFunctions.prettyOutput(2);
        }
    }


}



