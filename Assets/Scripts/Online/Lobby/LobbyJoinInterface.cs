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

    // on stage selection
    public void SetStage(int stageNum)
    {
        lobbyManager.SetStage(stageNum);
    }

    // sets the local palyer
    public void SetLocalPlayer(int plyr)
    {
        lobbyManager.SetLocalPlayer(plyr);
    }

    // Update is called once per frame
    void Update()
    {
        // find lobby manager
        if (lobbyManager == null)
            lobbyManager = FindObjectOfType<OnlineLobbyManager>();
    }
}
