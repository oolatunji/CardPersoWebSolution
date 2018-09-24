using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFWithOracleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //Console.ForegroundColor = ConsoleColor.Green;
                //Console.WriteLine("\nCard Perso Database and Default Data Set Up.\n");

                //Console.ForegroundColor = ConsoleColor.White;
                //Console.Write("Enter Oracle Database Host: ");
                //string host = Console.ReadLine();

                //Console.Write("Enter Oracle Database Port: ");
                //string port = Console.ReadLine();

                //Console.Write("Enter Oracle Database Service Name (e.g XE, ORCL): ");
                //string servicename = Console.ReadLine();

                //Console.Write("Enter Oracle Database UserId: ");
                //string userId = Console.ReadLine();

                //Console.Write("Enter Oracle Database Password: ");
                //string password = Console.ReadLine();

                //OracleDataAccess dac = new OracleDataAccess();
                //var conn = dac.connect(host, port, servicename, userId, password);
                //Console.WriteLine("Do you want to proceed with card perso database and data set up?(Y/N)");
                //string response = Console.ReadLine();
                //if(response.ToLower().Equals("y"))
                //{
                //    dac.setupdatabase(conn);
                //}
                //else
                //{
                //    Console.WriteLine("Card Perso Database and Default Data Set Up Cancelled");
                //}
                //var client = new AuthenticationService.AuthenticationService();
                //var response = client.ValidateUser("sysadmin", "pass");
                //Console.WriteLine($"{response.IsSuccessful} Response: {response.FailureReason}");
                var password = "password";
                var hashedPassword = CardPerso.Library.ModelLayer.Utility.PasswordHash.SHA256Hash(password);
                Console.Write(hashedPassword);
                Console.Read();
            }
            catch(Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.Read();
            }
        }
    }
}
