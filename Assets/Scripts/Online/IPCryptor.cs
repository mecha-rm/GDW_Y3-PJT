using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;

// the ip encryptor and decryptor
public class IPCryptor : MonoBehaviour
{
    // gets the system IP address.
    // provides 'address list index'
    public static string GetSystemIPAddress()
    {
        IPAddress ip = NetworkLibrary.Server.GetLocalIPv4Address();

        // no ipv4
        if(ip == null)
        {
            Debug.LogError("Ipv4 not found. Trying Ipv6");

            // search for ipv6
            ip = NetworkLibrary.Server.GetLocalIPv6Address();

            // ipv6 not found
            if(ip == null)
            {
                Debug.LogError("Ipv6 not found, returning empty string.");
                return "";
            }
        }

        return ip.ToString();
    }

    // generates a system ip address.
    public static IPAddress GenerateSystemIPAddressObject()
    {
        IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
        IPAddress ip = host.AddressList[1]; // get IP address from list
        return ip;
    }

    // checks to see if the p address is valid.
    public static bool ValidateIPAddress(string ipAddress)
    {
        // ip address not valid.
        IPAddress nullAddress = null;
        return IPAddress.TryParse(ipAddress, out nullAddress);
    }

    // HELPER FUNCTIONS
    public static string ReverseString(string str)
    {
        // reverses the string.
        string strX = "";

        for (int i = str.Length - 1; i >= 0; i--)
            strX += str[i];

        return strX;
    }

    // ENCRYPTION
    // encrypts the system ip address.
    public static string EncryptSystemIP()
    {
        string str = GetSystemIPAddress();
        return EncryptIP(str);
    }

    // encrpyts the ip address.
    public static string EncryptIP(IPAddress ip)
    {
        return EncryptIP(ip.ToString());
    }

    // encrypts the ip address to create a room code.
    public static string EncryptIP(string ipD)
    {
        // activate encryption
        bool encrypt = true;

        // ip encrypted.
        string ipE = ipD;

        // encrypt the ip
        if(encrypt)
        {
            // TODO: make the encryption algorithm more complex. 
            // TODO: this doesn't work.
            // ipE.Replace('1', 'Q');
            // ipE.Replace('2', 'X');
            // ipE.Replace('3', 'R');
            // ipE.Replace('4', 'W');
            // ipE.Replace('5', 'M');
            // ipE.Replace('6', 'T');
            // ipE.Replace('7', 'F');
            // ipE.Replace('8', 'A');
            // ipE.Replace('9', 'Z');
            // ipE.Replace('.', 'S');
            // 
            // // reverses the string.
            // ipE = ReverseString(ipE);

            // temporary string
            string encryptStr = "";

            // checks to add replacement character
            for (int i = 0; i < ipD.Length; i++)
            {
                // adds replacement character.
                if(ipD.Substring(i, 1) == "1")
                {
                    encryptStr += "Q";
                }
                else if (ipD.Substring(i, 1) == "2")
                {
                    encryptStr += "X";
                }
                else if (ipD.Substring(i, 1) == "3")
                {
                    encryptStr += "R";
                }
                else if (ipD.Substring(i, 1) == "4")
                {
                    encryptStr += "W";
                }
                else if (ipD.Substring(i, 1) == "5")
                {
                    encryptStr += "M";
                }
                else if (ipD.Substring(i, 1) == "6")
                {
                    encryptStr += "T";
                }
                else if (ipD.Substring(i, 1) == "7")
                {
                    encryptStr += "F";
                }
                else if (ipD.Substring(i, 1) == "8")
                {
                    encryptStr += "A";
                }
                else if (ipD.Substring(i, 1) == "9")
                {
                    encryptStr += "Z";
                }
                else if (ipD.Substring(i, 1) == ".")
                {
                    encryptStr += "S";
                }
            }
            
            // flips string around.
            ipE = ReverseString(encryptStr);  
        }

        return ipE;
    }


    // DECRYPTION
    // decrpyts the ip address.
    public static IPAddress DecryptIPToObject(string ipE)
    {
        // decrypts the IP address.
        string ipStr = DecryptIP(ipE);
        IPAddress ipObject = IPAddress.Parse(ipStr);

        // returns ip object.
        return ipObject;
    }

    // decrypts a room to create a room code.
    public static string DecryptIP(string ipE)
    {
        // activate decryption
        bool decrypt = true;

        // decrypted.
        string ipD = ipE;

        // decrypt the ip
        if (decrypt)
        {
            // ipD.Replace('Q', '1');
            // ipD.Replace('X', '2');
            // ipD.Replace('R', '3');
            // ipD.Replace('W', '4');
            // ipD.Replace('M', '5');
            // ipD.Replace('T', '6');
            // ipD.Replace('F', '7');
            // ipD.Replace('A', '8');
            // ipD.Replace('Z', '9');
            // ipD.Replace('S', '.');
            // 
            // // reverse the string.
            // ipD = ReverseString(ipD);


            // temporary string for decryption.
            string decryptStr = "";

            // checks to add replacement character
            for (int i = 0; i < ipE.Length; i++)
            {
                // adds replacement character.
                if (ipE.Substring(i, 1) == "Q")
                {
                    decryptStr += "1";
                }
                else if (ipE.Substring(i, 1) == "X")
                {
                    decryptStr += "2";
                }
                else if (ipE.Substring(i, 1) == "R")
                {
                    decryptStr += "3";
                }
                else if (ipE.Substring(i, 1) == "W")
                {
                    decryptStr += "4";
                }
                else if (ipE.Substring(i, 1) == "M")
                {
                    decryptStr += "5";
                }
                else if (ipE.Substring(i, 1) == "T")
                {
                    decryptStr += "6";
                }
                else if (ipE.Substring(i, 1) == "F")
                {
                    decryptStr += "7";
                }
                else if (ipE.Substring(i, 1) == "A")
                {
                    decryptStr += "8";
                }
                else if (ipE.Substring(i, 1) == "Z")
                {
                    decryptStr += "9";
                }
                else if (ipE.Substring(i, 1) == "S")
                {
                    decryptStr += ".";
                }
            }

            // flips string around.
            ipD = ReverseString(decryptStr);
        }

        return ipD;
    }
}
