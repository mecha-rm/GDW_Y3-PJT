using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyHostManager : MonoBehaviour
{
    // room code text
    public InputField roomCodeInputField;

    // room size content
    public Text roomSizeText;
    public Slider roomSizeSlider;
    private int roomSize;

    // objects for host
    // public GameObject hostPane;
    // public GameObject join1Pane;
    // public GameObject join2Pane;
    // public GameObject join3Pane;

    // Start is called before the first frame update
    void Start()
    {
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
