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
    public int playerCount = 0;

    // the four players
    // TODO: have these get generated instead of just already existing. Done elsewhere.
    // TODO: make this a listi nstead of dedicated varaibles?
    // the character select screen will generate these.
    public PlayerObject p1 = null;
    public PlayerObject p2 = null;
    public PlayerObject p3 = null;
    public PlayerObject p4 = null;

    // the list of players
    // private List<PlayerObject> players = new List<PlayerObject>();

    // the death space attached to the gameplay manager
    public DeathSpace deathSpace = null;

    // Start is called before the first frame update
    void Start()
    {
        // shows player objective.
        if (objectiveText == null)
        {
            GameObject temp = GameObject.Find("Objective Text");

            // if the objective text object exists
            if(temp != null)
                objectiveText = temp.GetComponent<Text>();
        }
            

        if (objectiveText != null)
            objectiveText.text = "First player to " + winScore + " wins";

        // now loads from resources folder so that an EXE can be built.
        // Object playerPrefab = Resources.Load<Object>("Prefabs/Player"); // Assets/Resources/Prefabs/Player.prefab

        // creates players at runtime
        // if (p1 == null || p2 == null || p3 == null || p4 == null)
        // {
        //     for (int i = 1; i <= playerCount; i++)
        //     {
        //         GameObject px = Instantiate((GameObject)playerPrefab);
        // 
        //         // generates players
        //         if (i == 1 && p1 == null)
        //         {
        //             p1 = px.GetComponent<PlayerObject>();
        //             p1.playerNumber = 1;
        //             p1.playerCamera.gameObject.GetComponent<Camera>().targetDisplay = 1;
        //         }
        //         else if (i == 2 && p2 == null)
        //         {
        //             p2 = px.GetComponent<PlayerObject>();
        //             p2.playerNumber = 2;
        //             p2.playerCamera.gameObject.GetComponent<Camera>().targetDisplay = 2;
        //         }
        //         else if (i == 3 && p3 == null)
        //         {
        //             p3 = px.GetComponent<PlayerObject>();
        //             p3.playerNumber = 3;
        //             p3.playerCamera.gameObject.GetComponent<Camera>().targetDisplay = 3;
        //         }
        //         else if (i == 4 && p4 == null)
        //         {
        //             p4 = px.GetComponent<PlayerObject>();
        //             p4.playerNumber = 4;
        //             p4.playerCamera.gameObject.GetComponent<Camera>().targetDisplay = 4;
        //         }
        //     }
        // }
        

        // adds players to the player list
        // if (p1 != null)
        //     players.Add(p1);
        // if (p2 != null)
        //     players.Add(p2);
        // if (p3 != null)
        //     players.Add(p3);
        // if (p4 != null)
        //     players.Add(p4);

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

    // creates the players
    public void CreatePlayer(int number, GameBuilder.playables type, bool destroySaved)
    {
        // new player
        GameObject newPlayer;

        // type
        switch (type)
        {
            case GameBuilder.playables.dog:
                newPlayer = (GameObject)Resources.Load("Prefabs/Dog");
                break;
            case GameBuilder.playables.cat:
                newPlayer = (GameObject)Resources.Load("Prefabs/Cat");
                break;
            case GameBuilder.playables.bunny:
                newPlayer = (GameObject)Resources.Load("Prefabs/Bunny");
                break;
            case GameBuilder.playables.turtle:
                newPlayer = (GameObject)Resources.Load("Prefabs/Turtle");
                break;
            case GameBuilder.playables.none:
            default:
                newPlayer = (GameObject)Resources.Load("Prefabs/Player");
                break;
        }


        // player object values
        PlayerObject px = newPlayer.GetComponent<PlayerObject>();
        number = Mathf.Clamp(number, 0, 4);

        px.playerNumber = number;
        px.playerCamera.gameObject.GetComponent<Camera>().targetDisplay = number;
        
        // saves the player object
        // also increases player count if no player object was assigned yet.
        switch(number)
        {
            case 1:
            default:
                if (p1 == null)
                    playerCount++;

                if (destroySaved)
                    Destroy(p1);

                p1 = px;
                break;

            case 2:
                if (p2 == null)
                    playerCount++;

                if (destroySaved)
                    Destroy(p2);

                p2 = px;
                break;

            case 3:
                if (p3 == null)
                    playerCount++;

                if (destroySaved)
                    Destroy(p3);

                p3 = px;
                break;

            case 4:
                if (p4 == null)
                    playerCount++;

                if (destroySaved)
                    Destroy(p4);

                p4 = px;
                break;

        }
    }

    // destroys the player mased on their number
    public void DestroyPlayer(int number)
    {
        // bool for destroying objects
        bool destroyed = false;

        switch(number)
        {
            case 1:
                Destroy(p1);
                destroyed = true;
                break;
            case 2:
                Destroy(p2);
                destroyed = true;
                break;
            case 3:
                Destroy(p3);
                destroyed = true;
                break;
            case 4:
                Destroy(p4);
                destroyed = true;
                break;
        }

        if (destroyed)
            playerCount--;
    }

    // destroys all players
    public void DestroyAllPlayers()
    {
        Destroy(p1);
        Destroy(p2);
        Destroy(p3);
        Destroy(p4);

        playerCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // player 1 has won
        if(p1 != null)
        {
            if (p1.playerScore >= winScore)
                SceneManager.LoadScene("EndScene");

            // death calculation.
            if (deathSpace.InDeathSpace(p1.gameObject.transform.position))
                p1.Respawn();
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
        // foreach(PlayerObject px in players)
        // {
        //     // entered death space
        //     if (deathSpace.InDeathSpace(px.gameObject.transform.position))
        //     {
        //         px.Respawn();
        //     }
        // }
    }
}
