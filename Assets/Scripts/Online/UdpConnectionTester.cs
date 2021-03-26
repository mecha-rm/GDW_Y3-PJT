using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UdpConnectionTester : MonoBehaviour
{
    // online manager.
    public OnlineGameplayManager onlineManager;

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
