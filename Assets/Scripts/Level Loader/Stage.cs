using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
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
    public Vector3 flagSpawn = new Vector3();
    public bool useFlagPosAsSpawn = true; // use flag's position as its spawn point.
    // public List<Vector3> flagSpawns = new List<Vector3>();
    // public bool randomizeFlagSpawn = false;

    // possible player spawn positions
    public List<Vector3> playerSpawns = new List<Vector3>();
    public bool randomPlayerPos = false; // randomizes player position

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
                flag = Instantiate((GameObject)(Resources.Load("Prefabs/Flag")));
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
