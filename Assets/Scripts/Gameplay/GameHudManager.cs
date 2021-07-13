using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHudManager : MonoBehaviour
{
    // the icons
    public GameObject p1Icon = null;
    public GameObject p2Icon = null;
    public GameObject p3Icon = null;
    public GameObject p4Icon = null;

    // use this to control the gameplay hud
    public Text p1Score = null;
    public Text p2Score = null;
    public Text p3Score = null;
    public Text p4Score = null;

    // Start is called before the first frame update
    void Start()
    {
    }

    //



    // sets the player score
    public void SetPlayerScore(int number)
    {
        // sets the text.
        switch (number)
        {
            case 0:
            case 1: // P1
                // score not set
                if (p1Score != null)
                    p1Score.text = number.ToString();
                break;
            
            case 2: // P2
                // score not set
                if (p2Score != null)
                    p2Score.text = number.ToString();
                break;
            
            case 3: // P3
                // score not set
                if (p3Score != null)
                    p3Score.text = number.ToString();
                break;
            
            case 4: // P4
                // score not set
                // score is not found
                if (p4Score != null)
                    p4Score.text = number.ToString();
                break;
        }

    }

    // player 1 score
    public void SetPlayer1Score()
    {
        SetPlayerScore(1);
    }

    // player 2 score
    public void SetPlayer2Score()
    {
        SetPlayerScore(2);
    }

    // player 3 score
    public void SetPlayer3Score()
    {
        SetPlayerScore(3);
    }

    // player 4 score
    public void SetPlayer4Score()
    {
        SetPlayerScore(4);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
