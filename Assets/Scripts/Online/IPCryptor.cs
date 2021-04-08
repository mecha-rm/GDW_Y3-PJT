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
            ipE.Replace('1', 'Q');
            ipE.Replace('2', 'X');
            ipE.Replace('3', 'R');
            ipE.Replace('4', 'W');
            ipE.Replace('5', 'M');
            ipE.Replace('6', 'T');
            ipE.Replace('7', 'F');
            ipE.Replace('8', 'A');
            ipE.Replace('9', 'Z');
            ipE.Replace('.', 'S');

            // reverses the string.
            ipE = ReverseString(ipE);

            // temporary string
            // string str;

            // for (int i = 0; i < ipE.Length; i++)
            // {
            //     ipE.Replace('1', 'Q');
            //     ipE.Replace('2', 'X');
            //     ipE.Replace('3', 'R');
            //     ipE.Replace('4', 'W');
            //     ipE.Replace('5', 'M');
            //     ipE.Replace('6', 'T');
            //     ipE.Replace('7', 'F');
            //     ipE.Replace('8', 'A');
            //     ipE.Replace('9', 'Z');
            //     ipE.Replace('.', 'S');
            // }
            // 
            // ipE = ReverseString(str);
            
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
            ipD.Replace('Q', '1');
            ipD.Replace('X', '2');
            ipD.Replace('R', '3');
            ipD.Replace('W', '4');
            ipD.Replace('M', '5');
            ipD.Replace('T', '6');
            ipD.Replace('F', '7');
            ipD.Replace('A', '8');
            ipD.Replace('Z', '9');
            ipD.Replace('S', '.');

            // reverse the string.
            ipD = ReverseString(ipD);
        }

        return ipD;
    }
}
