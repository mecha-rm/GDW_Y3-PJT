using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;


public class GameplayManager : MonoBehaviour
{
    // the winning score
    public float winScore = 15.0F;

    // the object for a player to win. 
    public Text objectiveText = null;

    // the player count
    // TODO: use this to optimize.
    public int playerCount = 0;

    // the four players
    // TODO: have these get generated instead of just already existing.
    // the character select screen will generate these.
    public PlayerObject p1 = null;
    public PlayerObject p2 = null;
    public PlayerObject p3 = null;
    public PlayerObject p4 = null;

    // the list of players
    private List<PlayerObject> players = new List<PlayerObject>();

    // the death space attached to the gameplay manager
    public DeathSpace deathSpace;

    // Start is called before the first frame update
    void Start()
    {
        // shows player objective.
        if (objectiveText != null)
            objectiveText.text = "First player to " + winScore + " wins";

        string playerPrefabPath = "Assets/Prefabs/Player.prefab";

        // TODO: create players at runtime (?)
        // TODO: how will they get the models though?
        if(p1 == null || p2 == null || p3 == null || p4 == null)
        {
            for (int i = 1; i <= playerCount; i++)
            {
                GameObject px = null;

                // generates player
                if (i == 1 && p1 == null)
                {
                    px = (GameObject)AssetDatabase.LoadAssetAtPath(playerPrefabPath, typeof(Object));
                    p1 = px.GetComponent<PlayerObject>();
                    p1.playerNumber = 1;
                    p1.playerCamera.gameObject.GetComponent<Camera>().targetDisplay = 1;

                }
                else if (i == 2 && p2 == null)
                {
                    px = (GameObject)AssetDatabase.LoadAssetAtPath(playerPrefabPath, typeof(Object));
                    p2 = px.GetComponent<PlayerObject>();
                    p2.playerNumber = 2;
                    p2.playerCamera.gameObject.GetComponent<Camera>().targetDisplay = 2;
                }
                else if (i == 3 && p3 == null)
                {
                    px = (GameObject)AssetDatabase.LoadAssetAtPath(playerPrefabPath, typeof(Object));
                    p3 = px.GetComponent<PlayerObject>();
                    p3.playerNumber = 3;
                    p3.playerCamera.gameObject.GetComponent<Camera>().targetDisplay = 3;
                }
                else if (i == 4 && p4 == null)
                {
                    px = (GameObject)AssetDatabase.LoadAssetAtPath(playerPrefabPath, typeof(Object));
                    p4 = px.GetComponent<PlayerObject>();
                    p4.playerNumber = 4;
                    p4.playerCamera.gameObject.GetComponent<Camera>().targetDisplay = 4;
                }
            }
        }
        

        // adds players to the player list
        if (p1 != null)
            players.Add(p1);
        if (p2 != null)
            players.Add(p2);
        if (p3 != null)
            players.Add(p3);
        if (p4 != null)
            players.Add(p4);

        // adds a death space if one doesn't exist.
        if(deathSpace == null)
        {
            // gets the death space component.
            deathSpace = GetComponent<DeathSpace>();

            // component does not exist, so add one.
            if (deathSpace == null)
            {
                // adds a death space component
                deathSpace = gameObject.AddComponent<DeathSpace>();

            }
        }
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

        // goes through all players
        // TODO: maybe put this in object checks?
        foreach(PlayerObject px in players)
        {
            // entered death space
            if (deathSpace.InDeathSpace(px.gameObject.transform.position))
            {
                px.Respawn();
            }
        }
    }
}
