using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour
{
    // the winning score
    public float winScore = 15.0F;

    // the object for a player to win.
    public Text objectiveText = null;

    // the player count
    // TODO: use this to optimize.
    int playerCount = 0;

    // the four players
    // TODO: have these get generated instead of just already existing.
    public PlayerObject p1 = null;
    public PlayerObject p2 = null;
    public PlayerObject p3 = null;
    public PlayerObject p4 = null;

    // Start is called before the first frame update
    void Start()
    {
        if (objectiveText != null)
            objectiveText.text = "First player to " + winScore + " wins";
    }

    // Update is called once per frame
    void Update()
    {
        // player 1 has won
        if(p1 != null)
        {
            if (p1.playerScore >= winScore)
                SceneManager.LoadScene("EndScene");
        }

        // player 2 has won
        if (p2 != null)
        {
            if (p2.playerScore >= winScore)
                SceneManager.LoadScene("EndScene");
        }

        // player 3 has won
        if (p3 != null)
        {
            if (p3.playerScore >= winScore)
                SceneManager.LoadScene("EndScene");
        }

        // player 4 has won
        if (p4 != null)
        {
            if (p4.playerScore >= winScore)
                SceneManager.LoadScene("EndScene");
        }
    }
}
