﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// switches lobby
public class LobbySwitcher : MonoBehaviour
{
    // the scene to exit to.
    private string exitScene = "Title";

    // online manager
    public OnlineLobbyManager lobbyManager;

    // host object
    public GameObject hostObject;

    // join object
    public GameObject joinObject;

    // Start is called before the first frame update
    void Start()
    {
        // finds online manager.
        if (lobbyManager == null)
            lobbyManager = FindObjectOfType<OnlineLobbyManager>();


        // looks for host object
        if (hostObject == null)
            GameObject.Find("Host");

        // looks for join object.
        if (joinObject == null)
            GameObject.Find("Host");
    }

    public void SetLobbyManager(bool makeIfNotExists = true)
    {
        // // if the game builder does not exist.
        // if (lobbyManager == null)
        // {
        //     // find the lobby manager.
        //     // this ignores the inactive version since it gets regenerated.
        //     lobbyManager = FindObjectOfType<OnlineLobbyManager>(false);
        // 
        //     // if the lobby manager was not found, it searches for an inactive.
        //     if(lobbyManager == null)
        //     {
        //         lobbyManager = FindObjectOfType<OnlineLobbyManager>(true);
        // 
        //         if (lobbyManager != null)
        //         {
        //         }
        //     }
        //     else
        //     {
        // 
        //     }
        // 
        //     // no lobby exists
        //     if (lobbyManager == null && makeIfNotExists)
        //     {
        //         GameObject newObject = Instantiate(Resources.Load("Prefabs/Title Game Builder") as GameObject);
        // 
        //         // gets component.
        //         if (newObject != null)
        //         {
        //             gameBuilder = newObject.GetComponent<GameBuilder>();
        //         }
        //         else // makes game builder.
        //         {
        //             Debug.LogError("Game Builder Prefab Not Found.");
        //             newObject = new GameObject();
        //             gameBuilder = newObject.AddComponent<GameBuilder>();
        //         }
        // 
        //     }
        //     else
        //     {
        //         Debug.LogError("Lobby Manager not Found.");
        //     }
        // }
        // 
        // // returns game builder
        // return lobbyManager;
    }

    // swaps host and join object activities.
    public void SwitchMode()
    {
        // host is active, the join is not.
        if(hostObject.activeSelf && !joinObject.activeSelf)
        {
            hostObject.SetActive(false);
            joinObject.SetActive(true);
            lobbyManager.isMaster = false;
        }
        // host is not active, but the join is.
        else if (!hostObject.activeSelf && joinObject.activeSelf)
        {
            hostObject.SetActive(true);
            joinObject.SetActive(false);
            lobbyManager.isMaster = true;
        }
        // both are active
        else if (hostObject.activeSelf && joinObject.activeSelf)
        {
            // deactivates join object.
            joinObject.SetActive(false);
            lobbyManager.isMaster = true;
        }
        // both are inactive
        else if (!hostObject.activeSelf && !joinObject.activeSelf)
        {
            // activates join object.
            hostObject.SetActive(true);
            lobbyManager.isMaster = true;
        }
    }

    // activates the host
    public void ActivateHost()
    {
        hostObject.SetActive(true);
        joinObject.SetActive(false);
    }

    // activates the join
    public void ActivateJoin()
    {
        hostObject.SetActive(false);
        joinObject.SetActive(true);
    }

    // starts the round.
    public void StartMatch()
    {
        // lobbyManager.PreMatchStart();

        // match should start.
        lobbyManager.startMatchOnUpdate = true;
    }

    // exists the lobby.
    public void ExitLobby()
    {
        // game builder to be deleted.
        GameBuilder gb = null;

        // if the lobby manager has not been set, try looking for it.
        if (lobbyManager == null)
            lobbyManager = FindObjectOfType<OnlineLobbyManager>();

        // destroys online manager upon exit.
        if (lobbyManager != null)
        {
            // gets the game builder from the lobby manager.
            gb = lobbyManager.gameBuilder;

            Destroy(lobbyManager.gameObject);
        }


        // searches for the game builder if the lobby manager didn't have it.
        if (gb == null)
            gb = FindObjectOfType<GameBuilder>();

        // if the game builder is not equal to null.
        if (gb != null)
            Destroy(gb.gameObject);


        // changes the scene.
        SceneChanger.ChangeScene(exitScene);
    }

    // on destroying the lobby switcher
    public void OnDestroy()
    {
    }
}
