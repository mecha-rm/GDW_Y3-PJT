using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// used to update the HUD (ver. 2).
public class GameHudUpdater : MonoBehaviour
{
    public GameplayManager gameManager = null;

    public PlayerObject p1 = null, p2 = null, p3 = null, p4 = null;

    // Start is called before the first frame update
    void Start()
    {
        // finds the gameplay manager
        if (gameManager == null)
            gameManager = FindObjectOfType<GameplayManager>();

        // if there is a game manager
        if(gameManager != null)
        {
            // player 1
            if(p1 == null)
                p1 = gameManager.p1;

            // player 2
            if (p2 == null)
                p2 = gameManager.p2;

            // player 3
            if (p3 == null)
                p3 = gameManager.p3;

            // player 4
            if (p4 == null)
                p4 = gameManager.p4;

        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
