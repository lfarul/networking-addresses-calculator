using System;
using System.IO;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Collections.Generic;


namespace Binary
{
    internal class Program
    {
        //Global variables
        //Maska
        private static string binaryOutputMaska = "";
        private static string dotMaska = "";
        private static string decimalMaska = "";
        //NotMaska
        private static string notMaska = "";
        private static string dotNotMaska = "";
        //IP
        private static string binaryIP = "";
        private static string binaryOutputIp = "";
        private static string dotIP = "";
        private static string decimalIP = "";
        //Network Address
        private static string networkAdress = "";
        private static string dotNetworkAdress = "";
        private static string decimalNetAdr = "";
        //Broadcasting address
        private static string broadcastingAddress = "";
        private static string dotBroadcast = "";
        private static string decimalBroadcast = "";
     
        public static bool ContainForbiddenSymbols(string input, string forbiddenSymbols)
        {
            //char[] chars = forbiddenSymbols.ToCharArray();

            foreach (char symbol in forbiddenSymbols)
            {
                if (input.Contains(symbol.ToString()))

                return true;
            }

            return false;
        }
        public static string ReplaceForbiddenSymbols(string input,string forbiddenSymbols)
        {

            //char[] chars = forbiddenSymbols.ToCharArray();

            foreach(char symbol in forbiddenSymbols)
            {
                input = input.Replace(symbol.ToString(), ".");
            }

            Console.WriteLine("Input after the santization: " + input);

            return input.ToString();
        }

        //IP validation - if 4 octets and within 0-255

        //Option 1: Parsing IP address
        //public static bool IpValidation(string ip)
        //{

        //    // Try parsing the IP address
        //    if (IPAddress.TryParse(ip, out IPAddress parsedIp))
        //    {
        //        // Check if it's a valid IPv4 address
        //        if (parsedIp.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
        //        {
        //            return true;
        //        }
        //    }

        //    return false;
        //}

        //Option 2: Parsing IP address
        public static bool IpValidation(string ip)
        {
            string[] ipOctets = ip.Split('.');
            int[] invalidOctets = new int[ipOctets.Length];

            if (ipOctets.Length == 4)
            {
                for (int i = 0; i < ipOctets.Length; i++)
                //foreach (string octet in ipOctets)
                {
                    bool parseSuccessfull = int.TryParse(ipOctets[i], out int value);

                    if (parseSuccessfull)
                    {
                        if (value >= 0 && value <= 255)
                        {
                            // Octet is valid, continue checking the next one
                            continue;
                        }
                        else
                        {
                            // Octet is not within the valid range
                            // Using LINQ to filter octets that out of 255
                            var invalidInput = from octet in ipOctets
                                                where Convert.ToInt32(octet) > 255
                                                select octet;
                                
                            string invalidInputConcat = String.Join(",", invalidInput);

                            Console.WriteLine($"Invalid octet[s]: {invalidInputConcat}. Please try again.");
                            WriteToFile($"Provided IP {ip} contains invalid octet[s]: {invalidInputConcat}.");

                            return false;
                        }
                    }
                    else
                    {
                        // Parsing failed, the octet is not a valid integer
                        Console.WriteLine($"Invalid octet[s] format: {ipOctets[i]}. Please try again.");
                        return false;
                    }
                }

                // All octets are valid
                return true;
            }

            // Incorrect number of octets
            string wrongIpFormat = String.Join(".", ipOctets);
            Console.WriteLine($"Provided IP: {wrongIpFormat} has only {ipOctets.Length} octets.  Please try again.");
            WriteToFile($"Provided IP: {wrongIpFormat} has only {ipOctets.Length} octet[s].");

            return false;
        }

        public static bool MaskaValidation(string maska)
        {
            string[] maskaOctets = maska.Split('.');
            string subnet = "";

            if (maskaOctets.Length == 4)
            {
                foreach (string octet in maskaOctets)
                {
                    if (int.TryParse(octet, out int value))
                    {
                        if (value >= 0 && value <= 255)
                        {
                            // Octet is valid, continue checking the next one
                            continue;
                        }
                        else
                        {
                            // Octet is not within the valid range
                            // Using LINQ to filter octets that out of 255
                            var invalidInput = from octets in maskaOctets
                                               where Convert.ToInt32(octets) > 255
                                               select octets;

                            string invalidInputConcat = String.Join(",", invalidInput);

                            Console.WriteLine($"Invalid octet[s]: {invalidInputConcat}. Please try again.");
                            WriteToFile($"Provided subnet {maska} contains invalid octet[s]: {invalidInputConcat}.");

                            return false;
                        }
                    }
                    else
                    {
                        // Parsing failed, the octet is not a valid integer
                        Console.WriteLine($"Invalid octet format: {octet}. Please try again.");
                        return false;
                    }
                }

                // All octets are valid
                return true;
            }

            else if (maskaOctets.Length == 1)
            {
                subnet = maskaOctets[0];

                if (int.TryParse(subnet, out int value))
                {
                    if (value >= 0 && value <= 32)
                    {
                        Console.WriteLine($"Provided subnet {subnet} meets validation criteria.");
                        return true;
                    }
                    else
                    {
                        // Subnet is not within the valid range
                        Console.WriteLine($"Provided subnet: {value} is out of range. Please try again.");
                        return false;
                    }
                }

                // Parsing failed, the subnet is not a valid integer
                Console.WriteLine($"Cannot parse to int: {subnet}. Please try again.");
                return false;
            }

            else
            {
                string wrongMaskaFormat = String.Join(".", maskaOctets);
                Console.WriteLine($"Invalid subnet format: {wrongMaskaFormat}. Please try again.");
                return false;
            }
        }

        // Convert IP and Mask to binary
        public static string ConvertToBinary(string input, string type)
        {
            // Split the input address by dots
            string[] inputParts = input.Split('.');
            string binaryOutput = "";

            StringBuilder builder = new StringBuilder();

            if(type == "maska" && inputParts.Length == 1)
            {
                string binaryOutputlocal = "";
                int inputMask = int.Parse(input);

                for(int i = 0; i < inputMask; i++)
                {
                    builder.Append('1');
                    //binaryOutputlocal += '1';
                }
                
                binaryOutputlocal = builder.ToString();

                const int numberOfbits = 32;

                binaryOutput = binaryOutputlocal.PadRight(numberOfbits, '0');

                SeparateAdressByDots(binaryOutputMaska, "maska");
                NotMaska(binaryOutputMaska);
            }
            else
            
                foreach (string part in inputParts)
                {
                    int result = int.Parse(part);
                    binaryOutput += Convert.ToString(result, 2).PadLeft(8, '0');
                }

                if (type == "ip")
                {
                    binaryOutputIp = binaryOutput;

                    //Console.WriteLine("IP:          " + binaryOutputIp);
                    SeparateAdressByDots(binaryOutputIp, "ip");
                }

                else if (type == "maska")
                {
                    binaryOutputMaska = binaryOutput;
                    //Console.WriteLine("Maska:       " + binaryOutputMaska);
                    SeparateAdressByDots(binaryOutputMaska, "maska");
                    NotMaska(binaryOutputMaska);
                    //CalculateNumberOfHosts(binaryOutputMaska);
                }

                else
                {
                    Console.WriteLine("Unknown type");
                }

                //reset the variable to store data for ip and maska
                binaryOutput = "";

                return binaryOutput;
        }

        // Calculate network address = IP x Maska
        public static string CalculateNetworkAdress(string ip, string maska)
        {
            StringBuilder builder = new StringBuilder();
            
            for (int i = 0; i < binaryOutputIp.Length; i++)
            {
                if (binaryOutputIp[i] == '1' && binaryOutputMaska[i] == '1')
                {
                    //networkAdress += '1';
                    builder.Append('1');
                    networkAdress = builder.ToString();
                }
                else
                    //networkAdress += '0';
                    builder.Append('0');
                    networkAdress = builder.ToString();
            }

            SeparateAdressByDots(networkAdress, "networkAddress");
            return networkAdress;
        }

        // Separate binary address by dots every 8 digits
        public static string SeparateAdressByDots(string input, string type)
        {
            int denominator = 8;

            StringBuilder builder = new StringBuilder(input.Length);

            for (int i = 0; i < input.Length; i++)
            {
                if (i != 0 && i % denominator == 0)
                {
                    builder.Append('.');
                }
                
                builder.Append(input[i]);
            }

            if(type == "networkAddress")
            {
                dotNetworkAdress = builder.ToString();
                //Console.WriteLine("dotNetAdres: " + dotNetworkAdress); 
            }

            else if(type == "ip")
            {
                dotIP = builder.ToString();
                //Console.WriteLine("dotIP:       " + dotIP);
            }

            else if (type == "maska")
            {
                dotMaska = builder.ToString();
                //Console.WriteLine("dotMaska:    " + dotMaska);
            }

            else if (type == "notMaska")
            {
                dotNotMaska = builder.ToString();
                //Console.WriteLine("dot!Maska:   " + dotNotMaska);
            }

            else if (type == "broadcastingAddress")
            {
                dotBroadcast = builder.ToString();
                //Console.WriteLine("dotBroadcast:" + dotBroadcast);      
            }

            else
            {
                Console.WriteLine("Type not defined");
            }
            
            return input;
        }


        // Reverse Maska to Not Maska
        public static string NotMaska(string binaryOutputMaska)
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < binaryOutputMaska.Length; i++)
            {
                if (binaryOutputMaska[i] == '1')
                {
                    builder.Append('0');
                }
                else
                    builder.Append('1');
            }     

            notMaska = builder.ToString();

            SeparateAdressByDots(notMaska, "notMaska");

            return notMaska;
        }

        //Broadcasting address = network address + not maska
        public static string CalculateBroadcastingAddress(string networkAddress, string notMaska)
        {
            StringBuilder builder = new StringBuilder();

            for(int i = 0; i < networkAddress.Length; i++)
            {
                if (networkAddress[i] == '0' && notMaska[i] == '0')
                {
                    //broadcastingAddress += '0';
                    builder.Append('0');
                    broadcastingAddress = builder.ToString();
                }

                else
                {
                    //broadcastingAddress += '1';
                    builder.Append('1');
                    broadcastingAddress = builder.ToString();
                }
            }

            SeparateAdressByDots(broadcastingAddress, "broadcastingAddress");

            return broadcastingAddress;
        }

        public static string ConvertToDecimal(string input, string type)
        {
            // Split the input address by dots
            string[] inputParts = input.Split('.');
            string decimalOutput = "";

            foreach (string part in inputParts)
            {
                decimalOutput += Convert.ToInt32(part,2)+".";
            }
            // Remove the dot at the end of the address
            decimalOutput = decimalOutput.TrimEnd('.');

            if (type == "networkAddress")
            {
                decimalNetAdr = decimalOutput;
                Console.WriteLine($"Network Address\t\t {decimalNetAdr}");
            }

            else if (type == "broadcastingAddress")
            {
                decimalBroadcast = decimalOutput;
                Console.WriteLine($"Broadcast Address\t {decimalBroadcast}");
            }
            else
            {
                Console.WriteLine("Type not defined");
            }

            decimalOutput = "";

            return decimalOutput;
        }

        //calculate number of hosts based on given ip and maska
        public static int CalculateNumberOfHosts(string maska)
        {
            char[] maskaChar = maska.ToCharArray();
            int counter = 0;
            int podstawa = 2;
            int wykladnik;
            int numberOfHosts;

            for (int i = 0; i < maskaChar.Length; i++)
            {
                if (maskaChar[i] == '1')
                    counter++;
            }

            wykladnik = 32 - counter;

            numberOfHosts = (int)Math.Pow(podstawa, wykladnik);

            return numberOfHosts;
        }


        //estimate address of the first host 
        public static string EstimateFirstHostAddress(string networkAddress)
        {
            string estimatedAddress = "";
            // Split the input address by dots
            string[] octets = networkAddress.Split('.');

            if (octets.Length == 4)
            {
                int lastOctet = int.Parse(octets[3]);

                int estimatedFirstHost = lastOctet + 1;

                // Update the last octet in the array
                octets[3] = estimatedFirstHost.ToString();

                // Join the octets back together with dots
                 estimatedAddress = string.Join(".", octets);

                Console.WriteLine($"First Host IP\t\t {estimatedAddress}");
            }

            else
            {
                Console.WriteLine("Invalid network address format.");
            }
            return estimatedAddress;
        }

        //estimate address of the last host 
        public static string EstimateLastHostAddress(string broadcastAddress)
        {
            string estimatedAddress = "";
            // Split the inout address by dots
            string[] octets = broadcastAddress.Split('.');

            if (octets.Length == 4)
            {
                int lastOctet = int.Parse(octets[3]);

                int estimatedLastHost = lastOctet - 1;

                // Update the last octet in the array
                octets[3] = estimatedLastHost.ToString();

                // Join the octets back together with dots
                estimatedAddress = string.Join(".", octets);

                Console.WriteLine($"Last Host IP\t\t {estimatedAddress}");
                //WriteToFile(estimatedAddress);
            }

            else
            {
                Console.WriteLine("Invalid broadcast address format.");
            }
            return estimatedAddress;
        }

        // Display complementary information
        public static bool ComplementaryInformationRequired (string input)
        {
            bool result;
            input = input.ToLower().Trim();
            
            if (input == "no")
            {
                result = false;
                Console.WriteLine("----------------------------------------------");
                Console.WriteLine("No further details required");
            }

            else if (input == "yes")
            {
                result = true;
                Console.WriteLine("----------------------------------------------\n");

                Console.WriteLine("Binary output\n");

                Console.WriteLine($"IP\t\t\t {binaryOutputIp}");
                Console.WriteLine($"Maska\t\t\t {binaryOutputMaska}");
                Console.WriteLine($"!Maska\t\t\t {notMaska}");
                Console.WriteLine($"Broadcast Address\t {broadcastingAddress}");
                Console.WriteLine($"Network Address\t\t {networkAdress}\n");

                Console.WriteLine("Binary output in octets\n");

                Console.WriteLine($"IP\t\t\t {dotIP}");
                Console.WriteLine($"Maska\t\t\t {dotMaska}");
                Console.WriteLine($"!Maska\t\t\t {dotNotMaska}");
                Console.WriteLine($"Broadcast Address\t {dotBroadcast}");
                Console.WriteLine($"Network Address\t\t {dotNetworkAdress}");
            }

            else
            {
                result = false;
                Console.WriteLine("----------------------------------------------");
                Console.WriteLine("No input provided");
            }
            
            return result;
        }

        // Collecting logs
        public static void WriteToFile(string data)
        {
            string folder = @"C:\Users\a630281\";
            string fileName = "records.txt";

            string fullPath = folder + fileName;
            DateTime date = DateTime.Now;
 
            //DateTime d = DateTime.Today;

            using (StreamWriter writer = new StreamWriter(fullPath, true))
            {
                //writer.Write("-----------------LOGS-------------\n");
                //writer.Write($"{date.ToString("dd/MM/yyyy")}\t");
                writer.Write($"{date.ToString()}\t");
                //writer.Write($"{d.ToString()}\t");
                writer.Write($"{data}\n"); 
            }
        }

        static void Main(string[] args)
        {
            NewProgram newProgram = new NewProgram();
            newProgram.SayHello();

            string ip = "";
            string maska = "";
            bool ipValidationStatus;
            bool maskaValidationStatus;

            //define potential symbols that users may use when typing IP and Subnet
            string forbiddenSymbols = ",;/!@#$%^&*()_+";

            if (newProgram.CanWeStart())    
            {
                do
                {
                    Console.Write("\nPlease provide address IP: ");
                    ip = Console.ReadLine();

                    if (ContainForbiddenSymbols(ip, forbiddenSymbols))
                    {
                        Console.WriteLine("IP contains forbidden symbols.");
                        string ipSanitized = ReplaceForbiddenSymbols(ip, forbiddenSymbols);
                        ip = ipSanitized;
                        ipValidationStatus = IpValidation(ip);
                    }
                    
                    else
                    {
                        ipValidationStatus = IpValidation(ip);   
                    }

                    if (!ipValidationStatus)
                    {
                        continue;
                    }
                } while (!ipValidationStatus);

                do
                {
                    Console.Write("Please provide subnet mask: ");
                    maska = Console.ReadLine();

                    if (ContainForbiddenSymbols(maska, forbiddenSymbols))
                    {
                        Console.WriteLine("Maska contains forbidden symbols.");

                        string maskaSanitized = ReplaceForbiddenSymbols(maska, forbiddenSymbols);
                        maska = maskaSanitized;
                        maskaValidationStatus = MaskaValidation(maska);
                    }

                    else
                    {
                        maskaValidationStatus = MaskaValidation(maska);
                    }

                    if (!maskaValidationStatus)
                    {
                        continue;
                    }
                } while (!maskaValidationStatus);

                ConvertToBinary(ip, "ip");
                ConvertToBinary(maska, "maska");

                Console.WriteLine("----------------------------------------------\n");

                Console.WriteLine("Basic information\t Value\n");
                Console.WriteLine($"IP\t\t\t {ip}");
                Console.WriteLine($"Maska\t\t\t {maska}");
                Console.WriteLine($"Number of hosts\t\t {CalculateNumberOfHosts(binaryOutputMaska)}");

                CalculateNetworkAdress(binaryOutputIp, binaryOutputMaska);
                CalculateBroadcastingAddress(networkAdress, notMaska);

                Console.WriteLine("----------------------------------------------\n");

                Console.WriteLine("Calculated value\t Address\n");
                ConvertToDecimal(dotNetworkAdress, "networkAddress");
                ConvertToDecimal(dotBroadcast, "broadcastingAddress");
                EstimateFirstHostAddress(decimalNetAdr);
                EstimateLastHostAddress(decimalBroadcast);

                Console.WriteLine();

                Console.Write("Do you want to see complementary information (yes/no) ?: ");
                string input = Console.ReadLine();

                ComplementaryInformationRequired(input);

                //WriteToFile(input);
            }

            else
                Console.WriteLine("\nThank you. See you next time.");

            // required for running application in the console (keeps the Output windows open to view results)
            Console.WriteLine("\n\rPress the Enter key to continue");
            Console.ReadLine();
        }
    }

    public class NewProgram
    {
        public void SayHello()
        {
            Console.WriteLine("Hallo from NewClass.\n\nThis console application can help you with determining:\n- network address\n- broadcast address\n- first available host\n- last available host\n- number of hosts in a subnet.\n\n" +
                "The information can be used for:\n- identifying the range of IP addresses that are available for use within a network\n- configuring network devices such as routers and switches and more.\n");
        }

        public bool CanWeStart()
        {
  
            Console.Write("Can we start (yes/no) ?: ");
            string userInput = Console.ReadLine();

            userInput = userInput.ToLower().Trim();

            bool result = (userInput == "yes") ? true : false;

            return result;
        }
    }
}



