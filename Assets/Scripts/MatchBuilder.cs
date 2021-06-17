using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NOTE: MatchBuilder is an optimized form of game builder.
// MatchBuilder is made for loading into a pre-existing scene and bringing in the player.
// this should eventually replace GameBuilder.
public class MatchBuilder : MonoBehaviour
{
    // ENUMS //
    // playable charactrs
    public enum playables { none, dog, cat, bunny, turtle };

    // the stages
    public enum stages { none, halloween, christmas, valentines };

    
    // SCENE-RELATED VARIABLES
    // if 'true', the match load is started.
    public bool buildMatchOnEntry = false;

    // if 'true', the audio settings are updated with what's specified in the game settings singleton.
    public bool adjustSettingsOnEntry = true;

    // next scene once game is started.
    public string sceneAfterMatch = "EndScene";

    // if set to 'true', the match builder destroys itself when a new level is loaded.
    public bool destroyOnLevelLoad = false;


    // MATCH BUILDER VARIABLES //
    // the type of the map that has been entered.
    public stages map = 0;

    // the amount of players
    // also saves what type each player is.
    public List<playables> playerList = new List<playables>();

    // the gameplay manager
    public GameplayManager manager;

    // the stage object - the stage
    Stage stage;

    // score goal (does not get set if left as -1)
    public float winScore = -1;

    // start of countdown timer (not set if left as -1)
    public float countdownStart = -1;

    // sets the game object not to destroy itself.
    private void Awake()
    {
        // the game builder shouldn't be destroyed.
        // call 'DestroyBuilder()' to destroy the match builder.
        DontDestroyOnLoad(gameObject);        
    }

    // Start is called before the first frame update
    void Start()
    {
        // checks to see if the match should be built by default.
        if (buildMatchOnEntry)
            BuildMatch();

        // updates the game volume settings.
        if (adjustSettingsOnEntry)
            UpdateSettings();
    }


    // builds the match.
    public void BuildMatch()
    {
        // destroys all debug objects in the scene.
        {
            // destroys the debug object and its children.
            GameObject debug = GameObject.Find("Debug");

            // destroys the parent and the children.
            if (debug != null)
                Destroy(debug.gameObject);
        }

        // finds gameplay manager
        manager = FindObjectOfType<GameplayManager>();

        // if the manager does not exist, then it creates a new one.
        if (manager == null)
        {
            // creates a new object as the gameplay manager.
            GameObject temp = Instantiate((GameObject)(Resources.Load("Prefabs/Gameplay Manager")));
            manager = temp.GetComponent<GameplayManager>(); // gets component
        }

        // sets this as the builder that made the manager
        if (manager != null)
            manager.matchBuilder = this;


        // this no longer creates the game assets, since each stage is its own scene.
        // the level loader also isn't really used, so all level creation functionality has been removed.

        stage = FindObjectOfType<Stage>(); // finds the stage object in the scene.

        // if not set, it searches for the stage object.
        if (stage == null)
        {
            Debug.LogError("No Stage Component Found.");
        }


        // DESTROY ALL EXISTING PLAYERS //
        {
            // create the game assets
            int count = Mathf.Clamp(playerList.Count, 0, 4);
            manager.DestroyAllPlayers(); // destroys all players

            // creates the player and puts it in the manager
            // if no players exist, a default player is given.
            if (count == 0) // no playes set, so test player is added.
            {
                manager.CreatePlayer(0, 0, true, true);
                playerList.Add(playables.none); // add player
            }
            else if (count == 1) // only one player, set them to main camera.
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
        }


        // randomizes the player positions
        if (stage != null)
        {
            stage.RandomizePlayerPositions(manager);
        }


        // OTHER //

        // NOTE:
        // the audio and skybox do not need to be loaded.
        // this is because the individual scenes handle this already.
        // feel free to add these functions back in if need be.


        // items - clears out all items.
        ItemManager.GetInstance().ClearAllItemsInPool();

        // sets winning score
        if (winScore != -1)
            manager.winScore = winScore;

        // sets match time
        if (countdownStart != -1)
        {
            // if countdown timer exists, give it the set time
            if (manager.countdownTimer != null)
                manager.countdownTimer.SetCountdownStartTime(countdownStart);
            
            // TODO: have option to make game timeless.
        }

        // sets next scene for when match ends
        if (sceneAfterMatch != "")
        {
            manager.nextScene = sceneAfterMatch;
        }

    }

    // checks to see if the match should be loaded on entry.
    public bool GetBuildMatchOnEntry()
    {
        return buildMatchOnEntry;
    }

    // sets to load the game or not.
    public void SetBuildMatchOnEntry(bool build)
    {
        buildMatchOnEntry = build;
    }

    // adds a player to the list
    public PlayerObject AddPlayer(int newPlayer, bool useMainCamera = false)
    {
        playerList.Add((playables)newPlayer);

        if (manager != null)
            return manager.CreatePlayer(playerList.Count, playerList[playerList.Count - 1], true, useMainCamera);
        else
            return null;
    }

    // adds a player to the game builder.
    public PlayerObject AddPlayer(MatchBuilder.playables newPlayer, bool useMainCamera = false)
    {
        playerList.Add(newPlayer);

        // creates and returns player
        if (manager != null)
            return manager.CreatePlayer(playerList.Count, newPlayer, true, useMainCamera);
        else
            return null;
    }

    // adds a player to the game builder.
    public PlayerObject AddPlayer(int number, MatchBuilder.playables newPlayer)
    {
        playerList.Add(newPlayer);

        if (manager != null)
            return manager.CreatePlayer(number, newPlayer, true, false, Mathf.Clamp(number, 1, 4));
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


    // updates the volume of all sound effects and BGMs.
    public void UpdateSettings()
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

    // called when a new level loads
    public void OnLevelWasLoaded(int level)
    {
        // if 'true', the builder is destroyed.
        if (destroyOnLevelLoad)
            DestroyBuilder();
    }

    // destroys the builder
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
