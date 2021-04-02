using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;

// the ip encryptor and decryptor
public class IPCryptor : MonoBehaviour
{
    // gets the system IP address.
    public static string GetSystemIPAddress()
    {
        return GetSystemIPAddress(1);
    }

    // gets the system IP address.
    // provides 'address list index'
    public static string GetSystemIPAddress(int alIndex)
    {
        IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
        IPAddress ip = host.AddressList[alIndex]; // get IP address from list ([1])
        return ip.ToString();
    }

    // generates a system ip address.
    public static IPAddress GenerateSystemIPAddress()
    {
        IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
        IPAddress ip = host.AddressList[1]; // get IP address from list
        return ip;
    }

    // encrypts the ip address to create a room code.
    public static string EncryptIP(string ipD)
    {
        string ipE = ipD;

        // TODO: encrypt ip address

        return ipE;
    }

    // decrypts a room to create a room code.
    public static string DecryptIP(string ipE)
    {
        string ipD = ipE;

        // TODO: decrypt ip address

        return ipD;
    }
}
