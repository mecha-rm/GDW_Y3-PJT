using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// this is used to manage user interface functions on the join side.
// shared operations are put in the OnlineLobbyManager script.
public class LobbyJoinInterface : MonoBehaviour
{
    // the lobby manager
    public OnlineLobbyManager lobbyManager;

    // room code text
    public InputField roomCodeInputField;

    // room indicator
    public Image roomIndict;
    private Color roomIndictOff; // sets to room indicator colour
    private Color roomIndictOn = new Color(102.0F / 255.0F, 255.0F / 255.0F, 149.0F / 255.0F);


    // players
    // labels for player names
    public Text hostLabel, join1Label, join2Label, join3Label;

    // Start is called before the first frame update
    void Start()
    {
        // lobby manager
        if (lobbyManager == null)
            lobbyManager = FindObjectOfType<OnlineLobbyManager>();

        // Room Code InputField
        if (roomCodeInputField == null)
        {
            // searches for object.
            GameObject temp = GameObject.Find("Room Join Code InputField");

            // object found
            if (temp != null)
            {
                roomCodeInputField = temp.GetComponent<InputField>();
            }
        }

        // room indicator colour
        if (roomIndict == null)
        {
            GameObject temp = GameObject.Find("Room Join Status");
            roomIndict = temp.GetComponent<Image>();
        }

        // room indicator found.
        if (roomIndict != null)
        {
            // gets 'off' colour.
            roomIndictOff = roomIndict.color;

            // this not set.
            if (roomIndictOn == Color.black)
                roomIndictOn = roomIndictOff;
        }


        // player name text labels
        // player 1
        if (hostLabel == null)
        {
            GameObject temp = GameObject.Find("Host Name Text");

            // gets text
            if (temp != null)
                hostLabel = temp.GetComponent<Text>();
        }

        // player 2
        if (join1Label == null)
        {
            GameObject temp = GameObject.Find("Join 1 Name Text");

            // gets text
            if (temp != null)
                join1Label = temp.GetComponent<Text>();
        }

        // player 3
        if (join2Label == null)
        {
            GameObject temp = GameObject.Find("Join 2 Name Text");

            // gets text
            if (temp != null)
                join2Label = temp.GetComponent<Text>();
        }

        // player 4
        if (join3Label == null)
        {
            GameObject temp = GameObject.Find("Join 3 Name Text");

            // gets text
            if (temp != null)
                join3Label = temp.GetComponent<Text>();
        }
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

    // updates the player name text.
    public void UpdatePlayerNameText()
    {
        // player name text labels
        // player 1
        if (hostLabel == null)
        {
            GameObject temp = GameObject.Find("Host Name Text");

            // gets text
            if (temp != null)
                hostLabel = temp.GetComponent<Text>();
        }

        // player 2
        if (join1Label == null)
        {
            GameObject temp = GameObject.Find("Join 1 Name Text");

            // gets text
            if (temp != null)
                join1Label = temp.GetComponent<Text>();
        }

        // player 3
        if (join2Label == null)
        {
            GameObject temp = GameObject.Find("Join 2 Name Text");

            // gets text
            if (temp != null)
                join2Label = temp.GetComponent<Text>();
        }

        // player 4
        if (join3Label == null)
        {
            GameObject temp = GameObject.Find("Join 3 Name Text");

            // gets text
            if (temp != null)
                join3Label = temp.GetComponent<Text>();
        }

        // host (p2)
        if (hostLabel != null)
            hostLabel.text = lobbyManager.player2Name;

        // join 1 (p1)
        if (join1Label != null)
            join1Label.text = lobbyManager.player1Name;

        // join 2
        if (join2Label != null)
            join2Label.text = lobbyManager.player3Name;

        // join 3
        if (join3Label != null)
            join3Label.text = lobbyManager.player4Name;
    }


    // finds the room (runs client)
    public void FindRoom()
    {
        // search for room code input field.
        if(roomCodeInputField.text == "")
        {
            Debug.LogError("No Room Code Set.");
            return;
        }

        // checks to see if the host is already running.
        if(lobbyManager.IsHostRunning())
        {
            Debug.LogAssertion("Tried to run the host to find a room, but the host was already running.");
            return;
        }

        // checks to see if the room is open.
        bool searchStarted = false;

        // sets ip address.
        lobbyManager.isMaster = false;
        lobbyManager.ipAddress = IPCryptor.DecryptIP(roomCodeInputField.text);
       
        // ip address parse failed.
        if(lobbyManager.ipAddress == "")
        {
            // ip address not set
            Debug.LogError("IPAddress not set.");
            return;
        }
        
        // runs host
        searchStarted = lobbyManager.RunHost();

        // room has been opened.
        if (searchStarted)
        {
            roomIndict.color = roomIndictOn; // on colour
        }
        else
        {
            roomIndict.color = roomIndictOff; // off colour
        }
    }

    // used to activate the room indicator light when re-entering hte lobby
    public void ActivateRoomIndicatorLight(bool active)
    {
        // room has been opened.
        if (active)
        {
            roomIndict.color = roomIndictOn; // on colour
        }
        else
        {
            roomIndict.color = roomIndictOff; // off colour
        }
    }

    // Update is called once per frame
    void Update()
    {
        // find lobby manager
        if (lobbyManager == null)
            lobbyManager = FindObjectOfType<OnlineLobbyManager>();

        // updates text.
        UpdatePlayerNameText();
    }
}
