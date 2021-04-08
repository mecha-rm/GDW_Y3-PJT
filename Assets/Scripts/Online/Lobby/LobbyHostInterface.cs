using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// this is used to manage user interface functions on the host side.
// shared operations are put in the OnlineLobbyManager script.
// ALL BUTTON OPERATONS NEED TO HAPPEN HERE.
public class LobbyHostInterface : MonoBehaviour
{
    // the lobby manager
    public OnlineLobbyManager lobbyManager;

    // room code text
    public InputField roomCodeInputField;

    // room size content
    public Text roomSizeText;
    public Slider roomSizeSlider;
    private int roomSize;

    // time selection
    public Dropdown timeSelect;

    // score select
    public Slider scoreSelect;
    public Text scoreText;

    // players
    // labels for player names
    public Text p1Label, p2Label, p3Label, p4Label;


    // public GameObject hostPane;
    // public GameObject join1Pane;
    // public GameObject join2Pane;
    // public GameObject join3Pane;

    // Start is called before the first frame update
    void Start()
    {
        // lobby manager
        if (lobbyManager == null)
            lobbyManager = FindObjectOfType<OnlineLobbyManager>();

        // Room Code InputField
        if(roomCodeInputField == null)
        {
            // searches for object.
            GameObject temp = GameObject.Find("Room Code InputField");

            // object found
            if(temp != null)
            {
                roomCodeInputField = temp.GetComponent<InputField>();
            }
        }

        // room size text not set.
        if (roomSizeText == null)
        {
            // search for object.
            GameObject temp = GameObject.Find("Room Size Text");

            // get text component.
            if (temp != null)
                roomSizeText = temp.GetComponent<Text>();
        }

        // room size slider
        if(roomSizeSlider == null)
        {
            // search for object.
            GameObject temp = GameObject.Find("Room Size Slider");

            // get text component.
            if (temp != null)
                roomSizeSlider = temp.GetComponent<Slider>();
        }

        // gets the room size in integer form.
        if (roomSizeText != null)
            roomSize = int.Parse(roomSizeText.text);


        // gets the time dropdown
        if(timeSelect == null)
        {
            // searches for object.
            GameObject temp = GameObject.Find("Time Dropdown");

            // gets component
            if (temp != null)
                timeSelect = temp.GetComponent<Dropdown>();

            // sets the start time
            if (timeSelect != null)
                SetStartTime();
        }

        // SCORE
        // score text
        if(scoreText == null)
        {
            GameObject newObject = GameObject.Find("Win Score Text");
            scoreText = newObject.GetComponent<Text>();
        }

        // gets the score value
        if (scoreSelect == null)
        {
            // searches for object.
            GameObject temp = GameObject.Find("Win Score Slider");

            // gets component
            if (temp != null)
                scoreSelect = temp.GetComponent<Slider>();

            // sets the start time
            if (scoreSelect != null)
                OnScoreChange();
        }

        // player name text labels
        // player 1
        if (p1Label == null)
        {
            GameObject temp = GameObject.Find("Player 1 Name Text");

            // gets text
            if (temp != null)
                p1Label = temp.GetComponent<Text>();
        }

        // player 2
        if (p2Label == null)
        {
            GameObject temp = GameObject.Find("Player 2 Name Text");

            // gets text
            if (temp != null)
                p2Label = temp.GetComponent<Text>();
        }

        // player 3
        if (p3Label == null)
        {
            GameObject temp = GameObject.Find("Player 3 Name Text");

            // gets text
            if (temp != null)
                p3Label = temp.GetComponent<Text>();
        }

        // player 4
        if (p4Label == null)
        {
            GameObject temp = GameObject.Find("Player 4 Name Text");

            // gets text
            if (temp != null)
                p4Label = temp.GetComponent<Text>();
        }

    }

    // gets the room size.
    public int GetRoomSize()
    {
        return roomSize;
    }

    // on the room size change
    public void OnRoomSizeChange()
    {
        // gets the new size
        int newSize = (int)roomSizeSlider.value;

        // sets text and room size.
        roomSizeText.text = newSize.ToString();
        roomSize = newSize;
    }

    // generates the room code from the ip address
    public string GenerateRoomCode()
    {
        // gets the system ip
        string roomCode = IPCryptor.EncryptSystemIP();

        // TODO: randomize the room code so that the ip addres sisn't set.

        return roomCode;
    }

    // generates the room code for the text object.
    public void GenerateAndSetRoomCode()
    {
        roomCodeInputField.text = GenerateRoomCode();
    }

    // sets the start time using the dialogue box value
    public void SetStartTime()
    {
        string str = timeSelect.options[timeSelect.value].text; // get current option
        int val = int.Parse(str); // get value
        lobbyManager.startTime = val; // set value
    }

    // sets the start time using the dialogue box value
    public void SetStartTime(int st)
    {
        lobbyManager.startTime = st;
    }

    // called when the score is changed.
    public void OnScoreChange()
    {
        // gets the new value
        lobbyManager.winScore = Mathf.RoundToInt(scoreSelect.value);

        // set text value.
        string str = lobbyManager.winScore.ToString();

        // adding zeroes.
        while (str.Length < 3)
            str = "0" + str;

        scoreText.text = str;
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
        // player 1
        if (p1Label != null)
            p1Label.text = lobbyManager.player1Name;

        // player 2
        if (p2Label != null)
            p2Label.text = lobbyManager.player2Name;

        // player 3
        if (p3Label != null)
            p3Label.text = lobbyManager.player3Name;

        // player 4
        if (p4Label != null)
            p4Label.text = lobbyManager.player4Name;
    }

    // starts the game
    public void OnStartGame()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        // find lobby manager
        if (lobbyManager == null)
            lobbyManager = FindObjectOfType<OnlineLobbyManager>();

        UpdatePlayerNameText();
    }
}
