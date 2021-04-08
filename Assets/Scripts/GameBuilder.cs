using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// builds the game
public class GameBuilder : MonoBehaviour
{
    // the playables
    public enum playables { none, dog, cat, bunny, turtle};

    // the stages
    public enum stages { none, halloween, christmas, valentines};

    // if 'true', the game is loaded.
    // Note: this must be set to 'true' before entering a scene in order for it to work.
    public bool loadGame = false;

    // if 'true', the map objects are loaded on entry.
    // if 'false', nothing is loaded. This should be disabled if using preset levels.
    public bool loadMapOnEntry = true;

    // if 'true', the audio settings are updated with what's specified in the game settings singleton.
    public bool adjustAudioOnEntry = true;

    // the name of the map
    public stages map = 0; // loads the map for the scene.

    // the amount of players
    public List<playables> playerList = new List<playables>();

    // the gameplay manager
    GameplayManager manager;

    // the stage parent object
    Stage stage;

    // next scene once game is started.
    public string sceneAfterGame = "EndScene";

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
        // checks to see if the game should be loaded.
        if (loadGame)
            LoadGame();


        // updates volume of everything in the scene.
        // this is in the 'Start()' section because this is not dependent on game loading.
        // plus this doesn't load anything
        if(adjustAudioOnEntry)
            UpdateVolume();
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

        // destroys any debug objects in the scene.
        {
            // destroys the debug object and its children.
            GameObject debug = GameObject.Find("Debug");

            // destroys the parent and the children.
            if (debug != null)
                Destroy(debug.gameObject);
        }

        // searches for gameplay manager
        manager = FindObjectOfType<GameplayManager>();

        // if the manager does not exist, then it creates a new one.
        if (manager == null)
        {
            // searches for an object with the manager.
            GameObject temp = GameObject.Find("Manager");

            // if an object with the name "manager" was not found, then it searches for an object called "Gameplay Manager"
            if (temp == null)
                temp = GameObject.Find("Gameplay Manager");

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
        else
        {
            // creates a new manager
            // Destroy(manager); // destroys the current manager
            // GameObject temp = Instantiate((GameObject)(Resources.Load("Prefabs/Gameplay Manager")));
            // manager = temp.GetComponent<GameplayManager>();
        }

        // sets this as the builder that made the manager.
        if (manager != null)
            manager.gameBuilder = this;

        // create game assets

        // LOAD MAP
        if (loadMapOnEntry) // load map on entry.
        {
            GameObject loadedObjects = new GameObject("Loaded Objects");
            LevelLoader levelLoader = loadedObjects.AddComponent<LevelLoader>();

            // LevelLoader levelLoader = new LevelLoader();
            // levelLoader.parent = new GameObject("Loaded Objects");

            levelLoader.parent = loadedObjects;


            levelLoader.loadAsChildren = true;

            // TODO: move stage files into folder
            switch (map)
            {
                default:
                case stages.none: // no map, so load the debug scene instead.
                    loadGame = false;
                    // NOTE: do NOT try to jump to another scene when processing a switch to an exiting scene.
                    // UnityEngine.SceneManagement.SceneManager.LoadScene("DebugScene"); // TODO: update when you rename scene.
                    levelLoader.file = "unnamed.dat";
                    break;

                case stages.halloween: // halloween stage
                    levelLoader.file = "halloween_stage.dat";
                    break;

                case stages.christmas: // christmas stage
                    levelLoader.file = "christmas_stage.dat";
                    break;

                case stages.valentines: // valentine's day stage
                    levelLoader.file = "valentines_day_stage.dat";
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
        else if(!loadMapOnEntry) // map should not be loaded on entry.
        {
            stage = FindObjectOfType<Stage>(); // finds the stage object in the scene.

            // if not set, it searches for the stage object.
            if(stage == null)
            {
                Debug.LogError("No Stage Component Found.");

                // GameObject temp = GameObject.Find("Stage");
                // 
                // // if there is no parent object named "stage", then nothing happens
                // if(temp != null)
                // {
                //     // gets the component from the stage
                //     stage = temp.GetComponent<Stage>();
                // 
                //     // if there is no stage component, add one.
                //     if (stage == null)
                //         stage = temp.AddComponent<Stage>();
                // }
            }
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

            // TODO: safety check

        }

        // skybox
        {
            if (stage.skybox != null)
                RenderSettings.skybox = stage.skybox;
        }

        // next scene.
        if (sceneAfterGame != "")
        {
            // sets next scene if it exists. Leaves alone if not available.
            // if (SceneChanger.SceneExists(sceneAfterGame))
            
            manager.nextScene = sceneAfterGame;
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

    // gets whether to load the stage or not.
    public bool GetLoadStage()
    {
        return loadMapOnEntry;
    }

    // sets to load the stage.
    public void SetLoadStage(bool loadStage)
    {
        loadMapOnEntry = loadStage;
    }

    // adds a player to the list
    public PlayerObject AddPlayer(int newPlayer)
    {
        playerList.Add((playables)newPlayer);

        if (manager != null)
            return manager.CreatePlayer(playerList.Count, playerList[playerList.Count - 1], true, false);
        else
            return null;
    }

    // adds a player to the game builder.
    public PlayerObject AddPlayer(GameBuilder.playables newPlayer)
    {
        playerList.Add(newPlayer);

        if (manager != null)
            return manager.CreatePlayer(playerList.Count, newPlayer, true, false);
        else
            return null;
    }

    // adds a player to the game builder.
    public PlayerObject AddPlayer(int number, GameBuilder.playables newPlayer)
    {
        playerList.Add(newPlayer);

        if (manager != null)
            return manager.CreatePlayer(number, newPlayer, true, false);
        else
            return null;
    }

    // gets the stage.
    public stages GetStage()
    {
        return map;
    }

    // gets the stage
    public int GetStageAsInt()
    {
        return (int)map;
    }

    // sets the stage
    public void SetStage(int newMap)
    {
        map = (stages)newMap;
    }

    // sets the stage
    public void SetStage(stages newMap)
    {
        map = newMap;
    }

    // clears the player list
    public void ClearPlayerList()
    {
        playerList.Clear();
    }

    // updates the volume of all sound efects and BGMs.
    public void UpdateVolume()
    {
        // finds all audio sources
        AudioSource[] audios = FindObjectsOfType<AudioSource>();

        // gets the game settings
        GameSettings settings = GameSettings.GetInstance();

        // gets the volume for the bgm and the sfx.
        // the master volume does not need to be set since that's done by Unity.
        float bgmVolume = settings.GetBgmVolume();
        float sfxVolume = settings.GetSfxVolume();

        // adjusts the volume settings.
        foreach (AudioSource audio in audios)
        {
            // adjusts volume
            switch (audio.tag)
            {
                case "BGM":
                    audio.volume = bgmVolume;
                    break;

                case "SFX":
                    audio.volume = sfxVolume;
                    break;
            }
        }

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
    
    // if called, the game builder is deleted.
    // the title screen makes a new builder everytime, so when ending the game the builder should be destroyed.
    public void DestroyBuilder()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // public void OnDestroy()
    // {
    //     
    // }
}
