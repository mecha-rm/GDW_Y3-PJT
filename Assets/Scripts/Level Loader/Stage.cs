using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class for stage information.
public class Stage : MonoBehaviour
{
    // NOTE: for some reason, if the position is too high or too low, it may not save properly.
    // I encountered that problem when it came to saving and loading the stages for some reason.
    // If you encounter another issue with saving and loading stages, maybe that's why.
    // since all stages are saved as prefabs, the parent object should always be at (0, 0, 0).
    // I don't know why this happens, but it is an issue. Maybe it has to do with how things are saved in the file or something.

    // gameplay manager
    public GameplayManager gameManager;

    // the bgm for the stage.
    public AudioClip bgm;
    public float bgmClipStart;
    public float bgmClipEnd;

    // the skybox for the stage
    public Material skybox;

    // flag spawn position
    public GameObject flag = null;
    public string flagPrefab = "Prefabs/Flag";
    public Vector3 flagSpawn = new Vector3();
    public bool useFlagPosAsSpawn = true; // use flag's position as its spawn point.
    // public List<Vector3> flagSpawns = new List<Vector3>();
    // public bool randomizeFlagSpawn = false;

    // possible player spawn positions
    public List<Vector3> playerSpawns = new List<Vector3>();
    public bool randomPlayerPos = false; // randomizes player position

    // the scene's item spawner
    public ItemSpawner itemSpawner;

    // the minimum and maximum of the spawn area
    // this is set at the start of the round and overrides the existing value.
    public Vector3 itemSpawnAreaMin = new Vector3(-50.0F, -50.0F, -50.0F);
    public Vector3 itemSpawnAreaMax = new Vector3(50.0F, 50.0F, 50.0F);

    // Start is called before the first frame update
    void Start()
    {
        // finds the game manager if it hasn't been set. 
        if(gameManager == null)
        {
            gameManager = FindObjectOfType<GameplayManager>();
        }

        

        // sets the skybox
        if (skybox != null)
            RenderSettings.skybox = skybox;

        // flag not set
        if (flag == null)
        {
            flag = GameObject.Find("Flag");

            if(flag == null) // if the flag doesn't exist, make one.
            {
                // loads up the flag prefab
                object prefab = Resources.Load(flagPrefab);

                // loads default flag
                if (prefab == null)
                    prefab = Resources.Load("Prefabs/Flag");

                // instantiates the prefab.
                flag = Instantiate((GameObject)prefab);
            }
        }

        // if the flag exists, change its position
        if(flag != null)
        {
            if (useFlagPosAsSpawn)
                flagSpawn = flag.transform.position;
            else
                flag.transform.position = flagSpawn;
        }


        // searches for the item spawner if it hasn't been set
        if (itemSpawner == null)
            itemSpawner = FindObjectOfType<ItemSpawner>();

        // if an item spawner exists, set its min and max spawn areas.
        if (itemSpawner != null)
        {
            itemSpawner.spawnAreaMin = itemSpawnAreaMin;
            itemSpawner.spawnAreaMax = itemSpawnAreaMax;
        }
            

        // no spawns added
        if (playerSpawns.Count == 0)
            playerSpawns.Add(new Vector3(0, 0, 0));
    }

    // randomizes the player positions
    public void RandomizePlayerPositions()
    {
        // if the game manager is set to null
        if (gameManager == null)
            return;

        // copies spawn positions
        List<Vector3> positions = playerSpawns;

        // gives random positions
        for(int i = 1; i <= gameManager.playerCount; i++)
        {
            // it's a "up to but not including" randomizer
            int index = Random.Range(0, gameManager.playerCount);

            // TODO: don't limit to the provided positions?
            switch(i)
            {
                case 1:
                    gameManager.p1.transform.position = positions[index];
                    break;
                case 2:
                    gameManager.p2.transform.position = positions[index];
                    break;
                case 3:
                    gameManager.p3.transform.position = positions[index];
                    break;
                case 4:
                    gameManager.p4.transform.position = positions[index];
                    break;
            }

            // removes position
            positions.RemoveAt(index);

        }
    }

    // sets manager and randomizes player positions
    public void RandomizePlayerPositions(GameplayManager manager)
    {
        this.gameManager = manager;
        RandomizePlayerPositions();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
