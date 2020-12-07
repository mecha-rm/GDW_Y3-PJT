using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// builds the game
public class GameBuilder : MonoBehaviour
{
    public enum playables { none, dog, cat, bunny, turtle};

    // if 'true', the game is loaded.
    public bool loadGame = false;
    
    // the name of the map
    public int map = 0; // loads the map for the scene.

    // the amoutn of players
    public List<playables> playerList = new List<playables>();

    // the gameplay manager
    GameplayManager manager;

    // the stage parent object
    Stage stage;

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

        // getting the manager from the game - gets or creates the gameplay manager
        if (manager == null)
        {
            GameObject temp = GameObject.Find("Manager");
            
            // object doesn't exist
            if(temp == null)
            {
                // generate manager
                temp = Instantiate((GameObject)(Resources.Load("Prefabs/Gameplay Manager")));
                manager = temp.GetComponent<GameplayManager>();
            }
            else
            {
                manager = temp.GetComponent<GameplayManager>();

                // if the manager is null, add the component
                if(manager == null)
                    manager = gameObject.AddComponent<GameplayManager>();
            }
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
                case 0: // no map
                    loadGame = false;
                    UnityEngine.SceneManagement.SceneManager.LoadScene("GameplayScene"); // TODO: update when you rename scene.
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
        int count = playerList.Count;
        manager.playerCount = Mathf.Clamp(count, 0, 4);

        // creating players
        for(int i = 0; i < count; i++)
        {
            // player object
            GameObject newPlayer = null;
            PlayerObject playerComp = null;

            // goes through all the players
            switch(playerList[i])
            {
                default:
                case playables.none: // no character set
                    newPlayer = Instantiate((GameObject)Resources.Load("Prefabs/Characters/Player"));
                    playerComp = newPlayer.GetComponent<PlayerObject>();
                    break;

                case playables.dog: // dog
                    newPlayer = Instantiate((GameObject)Resources.Load("Prefabs/Characters/Dog Player"));
                    playerComp = newPlayer.GetComponent<DogPlayer>();
                    break;

                case playables.cat: // cat
                    newPlayer = Instantiate((GameObject)Resources.Load("Prefabs/Characters/Cat Player"));
                    playerComp = newPlayer.GetComponent<CatPlayer>();
                    break;

                case playables.bunny: // bunny
                    newPlayer = Instantiate((GameObject)Resources.Load("Prefabs/Characters/Player"));
                    playerComp = newPlayer.GetComponent<PlayerObject>();
                    break;

                case playables.turtle: // turtle
                    newPlayer = Instantiate((GameObject)Resources.Load("Prefabs/Characters/Player"));
                    playerComp = newPlayer.GetComponent<PlayerObject>();
                    break;
            }

            // adds player to manager
            // there can only be maximum of 4 players
            switch(i + 1)
            {
                case 1:
                    manager.p1 = playerComp;
                    break;
                case 2:
                    manager.p2 = playerComp;
                    break;
                case 3:
                    manager.p3 = playerComp;
                    break;
                case 4:
                    manager.p4 = playerComp;
                    break;
            }
        }

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
            if(audioSource != null)
            {
                // add in the clip
                audioSource.clip = clip;
                audioSource.Play();
            }

        }

        // creates canvas, or just have it in the scene automatically?
        {

            // GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");

        }


    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
