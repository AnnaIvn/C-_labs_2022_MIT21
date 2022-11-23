using System;
using System.Text;
using System.Security.Cryptography;
using System.ComponentModel;

namespace lab8
{
    class Program
    {
        static void Main(string[] args)
        {
            // initializing string option, data
            string option, data;

            // paths
            string path = "D:/Lessons/2nd Year/Основи інформаційної безпеки/Labs/Lab 8/lab8",
                   publicKeyPath = path + "/publicKey.xml",
                   privateKeyPath = path + "/privateKey.xml";


            // initializing array
            byte[] hashedDocument;

            do
            {
                Console.WriteLine();
                Console.WriteLine("  | Options:");
                Console.WriteLine("  | 1 - digital signature");
                Console.WriteLine("  | 0 - exit");
                Console.WriteLine();
                Console.Write("   Your option -> ");
                option = Console.ReadLine();
                Console.WriteLine();
                Console.WriteLine();

                // 
                if (option == "1")
                {
                    var cspParams = new DigitalSignature();
                    
                    Console.Write(" Enter your data ->  ");
                    data = Console.ReadLine();                          // getting data
                    Console.WriteLine();
                    Console.WriteLine();
                    var document = Encoding.UTF8.GetBytes(data);        // string data to bytes

                    using (var sha256 = SHA256.Create())
                    {
                        hashedDocument = sha256.ComputeHash(document);  // hashing data bytes + putting in byte array
                    }

                    DigitalSignature.AssignNewKey(publicKeyPath, privateKeyPath);
                    var signature = cspParams.SignData(hashedDocument, privateKeyPath);
                    var verified = cspParams.VerifySignature(hashedDocument, signature, publicKeyPath);

                    Console.WriteLine(" Digital Signature Demonstration");
                    Console.WriteLine(" ---------------------------------------");
                    Console.WriteLine(" Original Text = " + Encoding.Default.GetString(document));
                    Console.WriteLine();
                    Console.WriteLine(" Digital Signature = " + Convert.ToBase64String(signature));
                    Console.WriteLine();

                    Console.WriteLine(verified
                    ? " The digital signature has been correctly verified."
                    : " The digital signature has NOT been correctly verified.");

                    otherFunctions.prettyOutput();
                }


            } while (option != "0");
        }
    }

    public class DigitalSignature
    {

        const string ContainerName = "MyContainer";
        public static void AssignNewKey(string publicKeyPath, string privateKeyPath)
        {
            CspParameters cspParams = new CspParameters(1)
            {
                KeyContainerName = ContainerName,
                Flags = CspProviderFlags.UseMachineKeyStore,
                ProviderName = "Microsoft Strong Cryptographic Provider"
            };
            using (var rsa = new RSACryptoServiceProvider(2048))     // create a new instance of the RSACryptoServiceProvider
            {
                rsa.PersistKeyInCsp = true;                                         // pass the previously created CspParameters object to its constructor
                File.WriteAllText(privateKeyPath, rsa.ToXmlString(true));
                File.WriteAllText(publicKeyPath, rsa.ToXmlString(false));
            };
        }


        public byte[] SignData(byte[] hashOfDataToSign, string privateKeyPath)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.PersistKeyInCsp = false;
                rsa.FromXmlString(File.ReadAllText(privateKeyPath));
                var rsaFormatter = new RSAPKCS1SignatureFormatter(rsa);
                rsaFormatter.SetHashAlgorithm("SHA256");
                return rsaFormatter.CreateSignature(hashOfDataToSign);
            }
        }

        public bool VerifySignature(byte[] hashOfDataToSign, byte[] signature, string publicKeyPath)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(File.ReadAllText(publicKeyPath));
                var rsaDeformatter = new RSAPKCS1SignatureDeformatter(rsa);
                rsaDeformatter.SetHashAlgorithm("SHA256");
                return rsaDeformatter.VerifySignature(hashOfDataToSign, signature);
            }
        }

    }


    public class otherFunctions
    {
        public static void prettyOutput()    // for pretty output 
        {
            Console.WriteLine();
            Console.WriteLine("  | Press Enter to continue");
            Console.ReadLine();
            Console.WriteLine();
        }
    }


}

