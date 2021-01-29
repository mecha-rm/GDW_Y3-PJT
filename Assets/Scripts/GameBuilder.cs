﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// builds the game
public class GameBuilder : MonoBehaviour
{
    public enum playables { none, dog, cat, bunny, turtle};

    // if 'true', the game is loaded.
    // TODO: rename function.
    public bool loadGame = false;
    
    // the name of the map
    public int map = 0; // loads the map for the scene.

    // the amount of players
    public List<playables> playerList = new List<playables>();

    // the gameplay manager
    GameplayManager manager;

    // the stage parent object
    Stage stage;

    // the stage file directory
    // string stageFileDirectory;

    // states that the object shouldn't be destroyed on load.
    private void Awake()
    {
        // the game builder shouldn't be destroyed.
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        // game shouldn't be loaded.
        if (!loadGame)
            return;

        LoadGame();
    }

    // loads the game
    public void LoadGame()
    {
        // game shouldn't be loaded.
        if(!loadGame)
        {
            Debug.Log("Game loading is disabled. Set 'loadGame' to true, and call LoadGame() again.");
            return;
        }

        // getting the manager from the game - gets or creates the gameplay manager
        if (manager == null)
        {
            manager = GetComponent<GameplayManager>();

            // if the manager component was not found...
            if (manager == null)
            {
                // searches for an object with the manager.
                GameObject temp = GameObject.Find("Manager");

                // object doesn't exist, so a new gameplay manager is made.
                if (temp == null)
                {
                    // generate manager
                    temp = Instantiate((GameObject)(Resources.Load("Prefabs/Gameplay Manager")));
                    manager = temp.GetComponent<GameplayManager>();
                }
                else
                {
                    manager = temp.GetComponent<GameplayManager>();

                    // if the manager is null, add the component
                    if (manager == null)
                        manager = gameObject.AddComponent<GameplayManager>();
                }
            }
        }
        else
        {
            // creates a new manager
            // Destroy(manager); // destroys the current manager
            // GameObject temp = Instantiate((GameObject)(Resources.Load("Prefabs/Gameplay Manager")));
            // manager = temp.GetComponent<GameplayManager>();
        }

        // create game assets

        // LOAD MAP
        {
            GameObject loadedObjects = new GameObject("Loaded Objects");
            LevelLoader levelLoader = loadedObjects.AddComponent<LevelLoader>();

            // LevelLoader levelLoader = new LevelLoader();
            // levelLoader.parent = new GameObject("Loaded Objects");

            levelLoader.parent = loadedObjects;
            // levelLoader.transform = levelLoader.parent.transform;
            // Stage stageComp = null;

            // if(levelLoader.parent == null || levelLoader.parent.gameObject == null) // no parent set
            // {
            //     stage = new GameObject();
            //     levelLoader.parent = stage;
            // }
            // else // parent set
            // {
            //     stage = levelLoader.parent;
            // }


            levelLoader.loadAsChildren = true;

            // TODO: move stage files into folder
            switch (map)
            {
                default:
                case 0: // no map, so load the debug scene instead.
                    loadGame = false;
                    // NOTE: do NOT try to jump to another scene when processing a switch to an exiting scene.
                    // UnityEngine.SceneManagement.SceneManager.LoadScene("DebugScene"); // TODO: update when you rename scene.
                    levelLoader.file = "unnamed.dat";
                    break;

                case 1: // halloween stage
                    levelLoader.file = "halloween_stage.dat";
                    break;

                case 2: // christmas stage
                    levelLoader.file = "christmas_stage.dat";
                    break;
            }

            // load contents
            if (levelLoader.file != "")
                levelLoader.LoadFromFile();

            stage = levelLoader.GetComponent<Stage>();

            // gets component from children if stage comp wasn't found.
            if (stage == null)
                stage = levelLoader.GetComponentInChildren<Stage>();
        }

        // LOAD CHARACTER ASSETS

        // create the game assets
        int count = Mathf.Clamp(playerList.Count, 0, 4);
        manager.DestroyAllPlayers(); // destroys all players

        // creates the player and puts it in the manager
        if(count == 0) // no playes set, so test player is added.
        {
            manager.CreatePlayer(0, 0, true, true);
        }
        else if(count == 1) // only one player, so use main camera
        {
            manager.CreatePlayer(0, playerList[0], false, true);
        }
        else
        {
            for (int i = 0; i < count; i++)
            {
                manager.CreatePlayer(i + 1, playerList[i], false, false);
            }
        }

        manager.playerCount = count;

        // creating players - now happens in the for loop above.
        // for (int i = 0; i < count; i++)
        // {
        //     // player object
        //     GameObject newPlayer = null;
        //     PlayerObject playerComp = null;
        // 
        //     // goes through all the players
        //     switch (playerList[i])
        //     {
        //         default:
        //         case playables.none: // no character set
        //             newPlayer = Instantiate((GameObject)Resources.Load("Prefabs/Characters/Player"));
        //             playerComp = newPlayer.GetComponent<PlayerObject>();
        //             break;
        // 
        //         case playables.dog: // dog
        //             newPlayer = Instantiate((GameObject)Resources.Load("Prefabs/Characters/Dog Player"));
        //             playerComp = newPlayer.GetComponent<DogPlayer>();
        //             break;
        // 
        //         case playables.cat: // cat
        //             newPlayer = Instantiate((GameObject)Resources.Load("Prefabs/Characters/Cat Player"));
        //             playerComp = newPlayer.GetComponent<CatPlayer>();
        //             break;
        // 
        //         case playables.bunny: // bunny
        //             newPlayer = Instantiate((GameObject)Resources.Load("Prefabs/Characters/Bunny Player"));
        //             playerComp = newPlayer.GetComponent<PlayerObject>();
        //             break;
        // 
        //         case playables.turtle: // turtle
        //             newPlayer = Instantiate((GameObject)Resources.Load("Prefabs/Characters/Turtle Player"));
        //             playerComp = newPlayer.GetComponent<PlayerObject>();
        //             break;
        //     }
        // 
        //     // adds player to manager
        //     // there can only be maximum of 4 players
        //     switch (i + 1)
        //     {
        //         case 1:
        //             manager.p1 = playerComp;
        //             break;
        //         case 2:
        //             manager.p2 = playerComp;
        //             break;
        //         case 3:
        //             manager.p3 = playerComp;
        //             break;
        //         case 4:
        //             manager.p4 = playerComp;
        //             break;
        //     }
        // }

        // TODO: check and see if the stage Start() needs to be called.

        // if the stage is not null, randomize the positions of the players
        if (stage != null)
        {
            stage.RandomizePlayerPositions(manager);
        }


        // LOAD AUDIO
        {
            AudioClip clip = stage.bgm;

            if (clip == null)
                clip = (AudioClip)(Resources.Load("Audio/BGMs/BGM_MAP_THEME_01"));

            // gets the audio source
            AudioSource audioSource = manager.GetComponentInChildren<AudioSource>();

            // if there is an audio source, play the audio.
            if (audioSource != null)
            {
                // add in the clip
                audioSource.clip = clip;
                audioSource.mute = false;
                audioSource.Play();
            }

            // gets the audio loop component.
            // TODO: maybe save this somewhere to save load time?
            AudioLoop audioLoop = GetComponentInChildren<AudioLoop>();
            
            // looping audio
            if(audioLoop == null)
            {
                audioLoop = gameObject.AddComponent<AudioLoop>();
            }

            // enables audio loop
            // TODO: should you check if it's already enabled first?
            audioLoop.enabled = true;
            audioLoop.clipStart = stage.bgmClipStart;
            audioLoop.clipEnd = stage.bgmClipEnd;

            // TODO: safety check?

        }

        // skybox
        {
            if (stage.skybox != null)
                RenderSettings.skybox = stage.skybox;
        }

        // game has been loaded. Turn back on before going to a new scene to load game assets.
        loadGame = false;
    }

    // checks to see if the game should be loaded.
    public bool GetLoadGame()
    {
        return loadGame;
    }

    // sets to load the game or not.
    public void SetLoadGame(bool load)
    {
        loadGame = load;
    }

    // adds a player to the list
    public void AddPlayer(int newPlayer)
    {
        playerList.Add((playables)newPlayer);
    }

    // gets the stage
    public int GetStage()
    {
        return map;
    }

    // sets the stage
    public void SetStage(int newMap)
    {
        map = newMap;
    }

    // called when the scene changes
    public void OnLevelWasLoaded(int level)
    {
        // TODO: maybe just have this do it on loadGame being true?.
        // picks from list of loaded scenes
        // string sceneName = UnityEngine.SceneManagement.SceneManager.GetSceneAt(level).name;

        // TODO: this will cause an infinite loop for some reason. Fix this.
        // string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        // 
        // // if the gameplay scene has been entered, or if the game should be loaded.
        // if(sceneName == "GameplayScene" || loadGame)
        // {
        //     LoadGame();
        // }   

        LoadGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
