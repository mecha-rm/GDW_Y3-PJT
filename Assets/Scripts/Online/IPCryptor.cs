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
    public static IPAddress GenerateSystemIPAddressObject()
    {
        IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
        IPAddress ip = host.AddressList[1]; // get IP address from list
        return ip;
    }


    // ENCRYPTION
    // encrypts the system ip address.
    public static string EncryptSystemIP()
    {
        return EncryptIP(GetSystemIPAddress());
    }

    // encrpyts the ip address.
    public static string EncryptIP(IPAddress ip)
    {
        return EncryptIP(ip.ToString());
    }

    // encrypts the ip address to create a room code.
    public static string EncryptIP(string ipD)
    {
        string ipE = ipD;

        // TODO: encrypt ip address

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
        string ipD = ipE;

        // TODO: decrypt ip address

        return ipD;
    }
}
