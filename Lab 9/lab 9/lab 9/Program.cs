using System;
using System.Text;
using System.Security.Cryptography;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;

namespace System.Security.Principal
{
    public class User
    {
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public byte[] Salt { get; set; }
        public string Roles { get; set; }
    }

    class Protector
    {
        // поле типу Словник для зберігання зареєстрованих користувачів
        private static Dictionary<string, User> _users = new Dictionary<string, User>();

        // user registratio programm
        public static User Register(string userName, string password, string role)
        {
            if (_users.Count == 0)
            {
                Console.WriteLine("  | There is no users. Login accepted");
                byte[] passwordInArray = Encoding.Unicode.GetBytes(password);                              // password in array
                byte[] saltInArray = Hashing.GenerateSalt();                                               // generating salt
                byte[] passwordHashInArray = Hashing.HashPasswordWithSalt(passwordInArray, saltInArray);   // hashing password

                string passwordHash = Convert.ToBase64String(passwordHashInArray);

                User newUser = new User();                 // creating  new user
                newUser.Login = userName;                  // putting in some user data
                newUser.PasswordHash = passwordHash;
                newUser.Salt = saltInArray;
                if (role == "yes")
                {
                    newUser.Roles = "Admin";
                }
                else
                {
                    newUser.Roles = "User";
                }
                _users.Add(userName, newUser);             // adding new user to the dictionary _users; userName - key of this user
                otherFunctions.prettyOutput();
                return newUser;
            }

            else for (int i = 0; i < _users.Count; i++)
            {
                Console.WriteLine("  | Such user does not exist. Login accepted");
                byte[] passwordInArray = Encoding.Unicode.GetBytes(password);                              // password in array
                byte[] saltInArray = Hashing.GenerateSalt();                                               // generating salt
                byte[] passwordHashInArray = Hashing.HashPasswordWithSalt(passwordInArray, saltInArray);   // hashing password

                string passwordHash = Convert.ToBase64String(passwordHashInArray);

                User newUser = new User();                 // creating  new user
                newUser.Login = userName;                  // putting in some user data
                newUser.PasswordHash = passwordHash;
                newUser.Salt = saltInArray;
                if (role == "yes")
                {
                    newUser.Roles = "Admin";
                }
                else
                {
                    newUser.Roles = "User";
                }
                _users.Add(userName, newUser);             // adding new user to the dictionary _users; userName - key of this user
                otherFunctions.prettyOutput();
                return newUser;
            }
            return null;
        }


        // display all users
        public static void DisplayAllUsers()
        {
            for (int i = 0; i < _users.Count; i++)
            {
                Console.WriteLine("    " + _users.ElementAt(i).Value.Login + "                  " + _users.ElementAt(i).Value.Roles);
            }
            Console.WriteLine("--------------------------------");
            Console.WriteLine();
            Console.WriteLine(" Total user amount: " + _users.Count);
            Console.WriteLine();
            otherFunctions.prettyOutput();
        }


        // checking password, returns true or false
        public static bool CheckPassword(string userName, string password)
        {
            if (_users.Count == 0)  // if there is no users
            {
                Console.WriteLine("  | There are no users. Authentification can't be done.");
                otherFunctions.prettyOutput();
            }

            else 
            {
                for (int i = 0; i < _users.Count; i++)   // going through all users
                {
                    if (_users.ElementAt(i).Value.Login == userName)  // if there is user with such username -> checking password
                    {
                        byte[] passwordInArray = Encoding.Unicode.GetBytes(password);              // password in array
                        byte[] saltInArray = _users.ElementAt(i).Value.Salt;                              // getting salt from dictionary
                        byte[] passwordHashInArray = Hashing.HashPasswordWithSalt(passwordInArray, saltInArray);   // hashing
                        string passwordHash = Convert.ToBase64String(passwordHashInArray);
                        string salt = Convert.ToBase64String(saltInArray);

                        // if admin
                        if (Protector.CheckHash(_users.ElementAt(i).Value.PasswordHash, passwordHash) && _users.ElementAt(i).Value.Roles == "Admin")
                        {
                            Console.WriteLine("  | Admin detected. Giving some additional info");
                            Console.WriteLine();
                            Console.WriteLine(" Some info just for admins ...");
                            otherFunctions.prettyOutput();
                            return true;
                        }
                        // if user
                        else if (Protector.CheckHash(passwordHash, passwordHash))
                        {
                            Console.WriteLine("  | Password is correct. Access granted");
                            Console.WriteLine();
                            Console.WriteLine(" Some info for users ...");
                            otherFunctions.prettyOutput();
                            return true;
                        }
                        // if password is incorrect
                        Console.WriteLine("  | Password isn't correct. Access denied.");
                        otherFunctions.prettyOutput();
                        return false;
                    }
                }
                // if there isn't user with such name
                Console.WriteLine("  | User with such name does not exist. Access denied.");
                otherFunctions.prettyOutput();
                return false;
            }
            return false;
        }


        // check for the same username
        public static bool CheckForTheSameUsername(string userName)
        {
            for (int i = 0; i < _users.Count; i++)
            {
                if (_users.ElementAt(i).Value.Login == userName)
                {
                    Console.WriteLine();
                    Console.WriteLine("  | User with such userame already exists. Try another name");
                    otherFunctions.prettyOutput();
                    return false;
                }
            }
            return true;
        }


        // checking hashes for accuracy
        public static bool CheckHash(string hash1, string hash2)
        {
            if (hash1 == hash2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        // method for user authentification (вхід)
        public static void LogIn(string userName, string password)
        {
            if (CheckPassword(userName, password))           // Перевірка пароля
            {
                string[] roles = { "User", "Admin" };
                
                var identity = new GenericIdentity(userName, password);            // Створюється екземпляр автентифікованого користувача
                //var principal = new GenericPrincipal(identity, _users[userName].Roles);    // Виконується прив’язка до ролей, до яких належить користувач
                var principal = new GenericPrincipal(identity, roles);
                System.Threading.Thread.CurrentPrincipal = principal;  // Створений екземпляр автентифікованого користувача з відповідними ролями присвоюється потоку, в якому виконується програма
            }
        }

    }



    // Password Based Key Derivation Functions
    public class Hashing
    {
        // generating salt
        public static byte[] GenerateSalt()
        {
            const int saltLength = 32;
            using (var randomNumberGenerator = new RNGCryptoServiceProvider())
            {
                var randomNumber = new byte[saltLength];
                randomNumberGenerator.GetBytes(randomNumber);
                return randomNumber;
            }
        }

        // hashing using sha256
        public static byte[] HashPasswordWithSalt(byte[] toBeHashed, byte[] salt)
        {
            int numberOfRounds = 150000;
            using (var rfc2898 = new Rfc2898DeriveBytes(toBeHashed, salt, numberOfRounds))
            {
                return rfc2898.GetBytes(32);
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





namespace lab9
{
    class Program
    {
        static void Main(string[] args)
        {
            // initializing string option, data
            string option, username, password, role;

            // initializing array
            byte[] hashedDocument;

            do
            {
                Console.WriteLine();
                Console.WriteLine("  | Options:");
                Console.WriteLine("  | 1 - registration");
                Console.WriteLine("  | 2 - authentification");
                Console.WriteLine("  | 3 - display all users");
                Console.WriteLine("  | 0 - exit");
                Console.WriteLine();
                Console.Write("   Your option -> ");
                option = Console.ReadLine();
                Console.WriteLine();
                Console.WriteLine();

                // REGISTRATION
                if (option == "1")
                {
                    Console.WriteLine(" Starting user registration  ");
                    Console.WriteLine("------------------------------------------------------------------------------------------------------------------------");
                    Console.Write(" Enter your username ->  ");
                    username = Console.ReadLine();                      // getting username

                    if (Protector.CheckForTheSameUsername(username))    // if true ( if there is no users with such username)
                    {
                        Console.Write(" Enter your password ->  ");
                        password = Console.ReadLine();                      // gettind password
                        Console.Write(" Are you registrating Admin? (yes | no)  ->  ");
                        role = Console.ReadLine();                      // gettind password
                        Console.WriteLine();
                        Protector.Register(username, password, role);             // calling registration command
                    }
                    
                    

                }

                //AUTHENTIFICATION
                if (option == "2")
                {
                    Console.WriteLine(" Starting user authentification  ");
                    Console.WriteLine("------------------------------------------------------------------------------------------------------------------------");
                    Console.Write(" Enter your username ->  ");
                    username = Console.ReadLine();                      // getting username
                    Console.Write(" Enter your password ->  ");         
                    password = Console.ReadLine();                      // gettind password
                    Console.WriteLine();
                    Protector.LogIn(username, password);                // calling log in command (and check password command)
                }

                // DISPLAY ALL USERS
                if (option == "3")
                {
                    //Console.WriteLine("    Username    Roles        Hashed Password                                   Salt");
                    Console.WriteLine("    Username            Role");
                    Console.WriteLine("--------------------------------");
                    Protector.DisplayAllUsers();                       // calling function for displaying all users

                }

                
            } while (option != "0");
        }
    }







}


