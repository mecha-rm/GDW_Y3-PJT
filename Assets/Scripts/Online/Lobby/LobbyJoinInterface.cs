using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this is used to manage user interface functions on the join side.
// shared operations are put in the OnlineLobbyManager script.
public class LobbyJoinInterface : MonoBehaviour
{
    // the lobby manager
    public OnlineLobbyManager lobbyManager;

    // Start is called before the first frame update
    void Start()
    {
        // lobby manager
        if (lobbyManager == null)
            lobbyManager = FindObjectOfType<OnlineLobbyManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
