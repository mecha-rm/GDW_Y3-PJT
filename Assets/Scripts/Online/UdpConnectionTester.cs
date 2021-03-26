using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UdpConnectionTester : MonoBehaviour
{
    // online manager.
    public OnlineGameplayManager onlineManager;

    // input fields for ip address and port
    public InputField ipAddressInput;
    public InputField portInput;

    // Start is called before the first frame update
    void Start()
    {
        // finds online manager
        if (onlineManager == null)
            onlineManager = FindObjectOfType<OnlineGameplayManager>();
    }

    // sets whether this is the master or not.
    public void SetMaster(bool value)
    {
        onlineManager.isMaster = value;
    }

    // runs the host
    public void RunHost()
    {
        // setting ip address
        if(ipAddressInput != null)
        {
            string str = ipAddressInput.text;
            onlineManager.SetIPAddress(str, true);
        }

        // setting port
        if (ipAddressInput != null)
        {
            int val = int.Parse(portInput.text);
            onlineManager.SetPort(val);
        }

        // onlineManager.isMaster = ;
        onlineManager.RunHost();
    }

    // shuts donw the host
    public void ShutdownHost()
    {
        onlineManager.ShutdownHost();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
