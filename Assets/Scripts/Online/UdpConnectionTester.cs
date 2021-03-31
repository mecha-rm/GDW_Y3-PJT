using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// this should only be used in the debug scene.
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

        // if the online manager isn't the master, disable the item spawner.
        if(!onlineManager.isMaster) // this is a client, not a server.
        {
            // finds the item spanwer
            ItemSpawner spawner = FindObjectOfType<ItemSpawner>();
            
            // if the item spawner exists.
            if(spawner != null)
            {
                // disable spawning operations.
                // this makes it so that new items are given by the server onyl.
                spawner.spawnerEnabled = false;
            }
        }

        // if the timer has not been set.
        if(onlineManager.timer == null)
        {
            // finds timer text
            TimerText timerText = FindObjectOfType<TimerText>();

            // timer text found.
            if(timerText != null)
            {
                TimerObject timerObject = timerText.GetActiveTimer();

                // checks to see if the timer object is set.
                if (timerObject != null)
                {
                    // sets timer for online manager.
                    onlineManager.timer = timerObject;
                }

                // if this isn't the master, disable the timer.
                if (!onlineManager.isMaster)
                {
                    // pauses the active timer
                    timerText.PauseActiveTimer();
                }
            }
            
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
