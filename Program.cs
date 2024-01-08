using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

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


        // Convert IP and Mask to binary
        public static string ConvertToBinary(string input, string type)
        {
            // Split the input address by dots
            string[] inputParts = input.Split('.');
            string binaryOutput = "";
            

            if(type == "maska" && inputParts.Length == 1)
            {
                //Console.WriteLine("Wpadłeś tutaj :): " + inputParts.Length);

                string binaryOutputlocal = "";
                int inputMask = int.Parse(input);

                for(int i = 0; i < inputMask; i++)
                {
                    binaryOutputlocal += '1';
                }

                const int numberOfbits = 32;

                binaryOutput = binaryOutputlocal.PadRight(numberOfbits, '0');

                SeparateAdressByDots(binaryOutputMaska, "maska");
                NotMaska(binaryOutputMaska);
                //Console.WriteLine("Wynik: " + binaryOutput);

            }
            else
            
                foreach (string part in inputParts)
                {
                    int result = int.Parse(part);
                    binaryOutput += Convert.ToString(result, 2).PadLeft(8, '0');
                }

                if (type.ToLower() == "ip")
                {
                    binaryOutputIp = binaryOutput;
                    //Console.WriteLine("IP:          " + binaryOutputIp);
                    SeparateAdressByDots(binaryOutputIp, "ip");
                }

                else if (type.ToLower() == "maska")
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
            
            //return binaryOutput;
        }

        // Calculate network address = IP x Maska
        public static string CalculateNetworkAdress(string ip, string maska)
        {
            
            for (int i = 0; i < binaryOutputIp.Length; i++)
            {
                if (binaryOutputIp[i] == '1' && binaryOutputMaska[i] == '1')
                {
                    networkAdress += '1';
                }
                else
                    networkAdress += '0';
            }

            //Console.WriteLine("NetAdres:    " + networkAdress);
            //Console.WriteLine();

            SeparateAdressByDots(networkAdress, "networkAddress");
            return networkAdress;
        }

        // Separate binary address by dots every 8 digits
        public static string SeparateAdressByDots(string input, string type)
        {
            int denominator = 8;

            StringBuilder sb = new StringBuilder(input.Length);

            for (int i = 0; i < input.Length; i++)
            {
                if (i!=0 && i % denominator == 0)
                {
                    sb.Append('.');
                }
                
                sb.Append(input[i]);
            }

            if(type == "networkAddress")
            {
                dotNetworkAdress = sb.ToString();
                //Console.WriteLine("dotNetAdres: " + dotNetworkAdress); 
            }

            else if(type == "ip")
            {
                dotIP = sb.ToString();
                //Console.WriteLine("dotIP:       " + dotIP);
            }

            else if (type == "maska")
            {
                dotMaska = sb.ToString();
                //Console.WriteLine("dotMaska:    " + dotMaska);
            }

            else if (type == "notMaska")
            {
                dotNotMaska = sb.ToString();
                //Console.WriteLine("dot!Maska:   " + dotNotMaska);
            }

            else if (type == "broadcastingAddress")
            {
                dotBroadcast = sb.ToString();
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
            StringBuilder sb = new StringBuilder(binaryOutputMaska.Length);

            for (int i = 0; i < binaryOutputMaska.Length; i++)
            {
                if (binaryOutputMaska[i] == '1')
                {
                    sb.Append('0');
                }
                else
                    sb.Append('1');
            }     

            notMaska = sb.ToString();

            //Console.WriteLine("!Maska:      " + notMaska);

            SeparateAdressByDots(notMaska, "notMaska");

            return notMaska;
        }

        //Broadcasting address = network address + not maska
        public static string CalculateBroadcastingAddress(string networkAddress, string notMaska)
        {
            for(int i = 0; i < networkAddress.Length; i++)
            {
                // 1+1=1; 1+0=1;0+1=1;0+0=0

                if (networkAddress[i] == '0' && notMaska[i] == '0')
                    
                    broadcastingAddress += '0';
                
                else

                    broadcastingAddress += '1';

            }
            //Console.WriteLine("BroadcastAd: " + broadcastingAddress);

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
                Console.WriteLine("Network Adress:    " + decimalNetAdr);
            }

            else if (type == "broadcastingAddress")
            {
                decimalBroadcast = decimalOutput;
                Console.WriteLine("Broadcast Address: " + decimalBroadcast);
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
            // Split the inout address by dots
            string[] octets = networkAddress.Split('.');

            if (octets.Length == 4)
            {
                int lastOctet = int.Parse(octets[3]);

                int estimatedFirstHost = lastOctet + 1;

                // Update the last octet in the array
                octets[3] = estimatedFirstHost.ToString();

                // Join the octets back together with dots
                 estimatedAddress = string.Join(".", octets);

                Console.WriteLine("First Host IP: " + estimatedAddress);
    
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

                Console.WriteLine("Last Host IP:  " + estimatedAddress);

            }

            else
            {
                Console.WriteLine("Invalid broadcast address format.");
            }
            return estimatedAddress;
        }

        static void Main(string[] args)
        {
            Console.Write("Proszę podaj adres IP: ");
            string ip = Console.ReadLine();

            Console.Write("Proszę podaj adres maski: ");
            string maska = Console.ReadLine();

            //string ip = "192.168.1.1";
            //string maska = "255.255.255.0";

            ConvertToBinary(ip, "ip");
            ConvertToBinary(maska, "maska");

            Console.WriteLine("IP: " + ip);
            Console.WriteLine("Maska: " + maska);
            Console.WriteLine("Number of hosts: " + CalculateNumberOfHosts(binaryOutputMaska));

            CalculateNetworkAdress(binaryOutputIp, binaryOutputMaska);
            CalculateBroadcastingAddress(networkAdress, notMaska);

            Console.WriteLine("----------------------------------------------");
            ConvertToDecimal(dotNetworkAdress, "networkAddress");
            ConvertToDecimal(dotBroadcast, "broadcastingAddress");
            EstimateFirstHostAddress(decimalNetAdr);
            EstimateLastHostAddress(decimalBroadcast);
        }
    }
}
