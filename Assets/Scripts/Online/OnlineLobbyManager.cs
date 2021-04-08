using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.NetworkInformation;
using System;

// manages online lobby
public class OnlineLobbyManager : MonoBehaviour
{
    // if the size changes, or if the status is different, then the games load.

    /// FORMAT: SERVER TO CLIENT
    /// <summary>
    /// Total Bytes: ??? (Array size 256)
    /// Format: Sending Data to Clients
    ///     - [0 - 3] - Status
    ///         * 0 = none
    ///         * 1 = Friend Screen
    ///         * 2 = Enter Play
    ///     - [4 - 7] - Player Count
    ///     - [8 - 11] - Stage Choice
    ///     - [12 - 43] - Player 1 Name (char = 2 bytes, 16 chars total, which is 32 bytes)
    ///     - [44 - 75] - Player 2 Name (if applicable)
    ///     - [76 - 107] - Player 3 Name (if applicable)
    ///     - [108 - 139] - Player 4 Name (if applicable)
    ///     - [140 - 143] - Player 1 Character
    ///     - [144 - 147] - Player 2 Character (if applicable)
    ///     - [148 - 151] - Player 3 Character (if applicable)
    ///     - [152 - 155] - Player 4 Character (if applicable)
    ///     - [156 - 159] - Player 1 Win Count
    ///     - [160 - 163] - Player 2 Win Count (if applicable)
    ///     - [164 - 167] - Player 3 Win Count (if applicable)
    ///     - [168 - 171] - Player 4 Win Count (if applicable)
    /// </summary>

    /// FORMAT: CLIENT TO SERVER
    /// <summary>
    /// Total Bytes: ???
    /// Format:
    ///     - [0 - 3] - Status
    ///         - 0 for not connected
    ///         - 1 for connected
    ///     - [4 - 35] - Name (char = 2 bytes, 16 chars, which is 32 bytes)
    ///     - [36 - 39] - Stage Choice
    ///     - [40 - 43] - Player Choice
    ///     - [44 - 47] - Win Count
    /// </summary>

    // checks to see if this is the one hosting or not.
    public bool isMaster;

    // udp server and client
    public UdpServerX server;
    public UdpClient client;

    // the number of endpoints forr the server.
    public int serverEndpoints = 1;

    // sets whether to block on sockets or not for both the server and client.
    public bool blocking = true;

    // if 'true', data is communicated.
    public bool dataComm = true;

    // buffer size for clients and servers
    public int serverBufferSize = 512;
    public int clientBufferSize = 512;

    // ip address
    public string ipAddress;

    // selected stage (defaults to halloween)
    public GameBuilder.stages stage = GameBuilder.stages.halloween;

    // the saved name of the player
    private string p1Name = "", p2Name = "", p3Name = "", p4Name = "";

    // players
    public GameBuilder.playables p1 = GameBuilder.playables.dog, p2, p3, p4;

    // start time for timer
    public int startTime;

    // checks to see if the following connections are being used.
    private bool p2Join = false, p3Join = false, p4Join = false;

    // win count for all players
    private int p1Wins = 0, p2Wins = 0, p3Wins = 0, p4Wins = 0;

    // the online gameplay manager.
    public OnlineGameplayManager onlineGameManager;

    // TODO: add game builder.
    public GameBuilder gameBuilder;

    // states that the object shouldn't be destroyed on load.
    private void Awake()
    {
        // the lobby manager shouldn't be destroyed.
        DontDestroyOnLoad(gameObject);

        // TODO: enable online on scene change?
    }

    // Start is called before the first frame update
    void Start()
    {
        // sets player name
        p1Name = GameSettings.GetInstance().GetScreenName();

        // if server hasn't been set.
        if (server == null && isMaster)
        {
            server = FindObjectOfType<UdpServerX>();

            if (server == null)
                server = new UdpServerX();
        }

        // server is set, so add endpoints.
        if (server != null)
        {
            // set blocking sockets value
            server.SetBlockingSockets(blocking);

            // existing number of endpoints
            int defEpCount = server.server.GetEndPointCount();

            // adds remote clients
            for (int i = defEpCount; i < serverEndpoints; i++)
                server.AddEndPoint(serverBufferSize);


        }


        // finds online gameplay manager.
        if (onlineGameManager == null)
            onlineGameManager = FindObjectOfType<OnlineGameplayManager>(true);

        // sets the game builder
        SetGameBuilder(true);
    }


    // checks to see if an ip address is vali.
    public static bool ValidIPAddress(string ipString)
    {
        // checks for valid IP
        IPAddress result = null;

        // attempts to parse
        return IPAddress.TryParse(ipString, out result);
    }

    // decrypts the ip address and sets it.
    public void DecryptAndSetIPAddress(string encryptIP)
    {
        ipAddress = IPCryptor.DecryptIP(encryptIP);
    }

    // checks for host availability using saved ip.
    public bool CheckHostAvailability()
    {
        // checks availability of host.
        return CheckHostAvailability(ipAddress);
    }

    // checks host availability
    public static bool CheckHostAvailability(string ipAddress)
    {
        try
        {
            // gets ip address
            IPAddress ip = IPAddress.Parse(ipAddress);

            // ip not null
            if (ip != null)
                return CheckHostAvailability(ip);
            else
                return false;
        }
        catch(Exception e)
        {
            Debug.LogError("Exception: " + e.ToString());
        }

        return false;

    }

    // checks to see if a host is available.
    public static bool CheckHostAvailability(IPAddress ip)
    {
        try
        {
            // ping objects.
            System.Net.NetworkInformation.Ping pingSender = new System.Net.NetworkInformation.Ping();
            PingReply pingReply;

            // checks for ping
            pingReply = pingSender.Send(ip);

            // if ping was successful, then host is available.
            if (pingReply.Status == IPStatus.Success)
            {
                return true;
            }

            // ping failure
            return false;

        }
        catch(Exception e)
        {
            Debug.LogError("Exception: " + e.ToString());
        }

        return false;
    }

    // used for searching for a hosted game to join.
    public void RunHost()
    {
        if(isMaster) // run server
        {
            // server is already running
            if (server.server.IsRunning())
            {
                Debug.LogAssertion("Server is already running");
                return;
            }

            // checks for valid ip address.
            if (!ValidIPAddress(ipAddress))
            {
                Debug.LogAssertion("Invalid room code.");
                return;
            }


            // sets the ip address and runs the server.
            server.SetIPAddress(ipAddress);
            server.RunServer();
        }
        else // run client.
        {
            // server is already running
            if (client.client.IsRunning())
            {
                Debug.LogAssertion("Server is already running");
                return;
            }

            // checks for valid ip address.
            if (!ValidIPAddress(ipAddress))
            {
                Debug.LogAssertion("Invalid room code.");
                return;
            }

            // host not available
            if (!CheckHostAvailability())
            {
                Debug.LogAssertion("Host not available");
                return;
            }

            client.SetIPAddress(ipAddress);
            client.RunClient();
        }

    }


    // COMMUNICATION //
    // SERVER -> CLIENT (MASTER) //

    // gets the data from client
    void ReceiveDataFromClients()
    {

        // gets the amount of endpoints
        int epAmount = server.server.GetEndPointCount();

        // TODO: maybe check something with player count?

        // goes through each endpoint to get the data. There is a max of 3.
        for (int i = 0; i < epAmount; i++)
        {

        }


    }

    // sends the data to the clients
    void SendDataToClients()
    {

        // data to be sent out to clients.
        byte[] sendData = new byte[serverBufferSize];
        int index = 0;

        // sets the send buffer data
        server.server.SetSendBufferData(sendData);
    }


    // CLIENT -> SERVER (NOT MASTER) //
    // sends the client's data to the server.
    void SendDataToServer()
    {
        // gets player position
        byte[] sendData = new byte[clientBufferSize];
        int index = 0;



    }

    // receives data from the server.
    void ReceiveDataFromServer()
    {
        // Time (float) - 4 bytes
        // Number of Players (Int) - 4 Bytes
        // Number of Active Item Boxes (Int) - 4 bytes
        // Players
        // Items

        // data to be sent out to clients.
        byte[] recData = client.client.GetReceiveBufferData();
        int index = 0;


        // Players - getting player data
        // for (int i = 0; i < plyrCount; i++)
        // {
        // 
        // }
    }

    
    // adds endpoint
    public void AddEndpoint()
    {
        server.server.AddEndPoint();
    }

    // removes the endpoint.
    public void RemoveEndpoint()
    {
        RemoveEndpoint(server.server.GetEndPointCount() - 1);
    }

    // removes endpoint.
    public void RemoveEndpoint(int index)
    {
        server.server.RemoveEndPoint(index);
    }


    // value setters
    // on stage selection
    public void SetStage(GameBuilder.stages stageEnum)
    {
        stage = stageEnum;
    }

    // on stage selection
    public void SetStage(int stageNum)
    {
        stage = (GameBuilder.stages)stageNum;
    }

    // sets the local player
    public void SetLocalPlayer(GameBuilder.playables plyr)
    {
        p1 = plyr;
    }

    // sets the local palyer
    public void SetLocalPlayer(int plyr)
    {
        p1 = (GameBuilder.playables)plyr;
    }

    // finds gmae builder, generating a new one if it doesn't exist.
    private GameBuilder SetGameBuilder(bool makeIfNotExists = true)
    {
        // if the game builder does not exist.
        if (gameBuilder == null)
        {
            // find a game builder
            gameBuilder = FindObjectOfType<GameBuilder>(true);

            // no game builder exists.
            if (gameBuilder == null && makeIfNotExists)
            {
                GameObject newObject = Instantiate(Resources.Load("Prefabs/Title Game Builder") as GameObject);

                // gets component.
                if (newObject != null)
                {
                    gameBuilder = newObject.GetComponent<GameBuilder>();
                }
                else // makes game builder.
                {
                    Debug.LogError("Game Builder Prefab Not Found.");
                    newObject = new GameObject();
                    gameBuilder = newObject.AddComponent<GameBuilder>();
                }

            }
            else
            {
                Debug.LogError("No Game Builder Found.");
            }
        }

        // returns game builder
        return gameBuilder;
    }

    // called when the match is about to start (changes scene at the end of the function).
    public void PreMatchStart()
    {
        // the scene name
        string sceneName = "";

        // stage (checks to see if stage exists)
        switch (stage)
        {
            case GameBuilder.stages.halloween: // halloween stage
                sceneName = "HalloweenMap";
                break;

            case GameBuilder.stages.christmas: // christmas stage
                sceneName = "ChristmasMap";
                break;

            case GameBuilder.stages.valentines: // valentine's stage
                sceneName = "ValentinesMap";
                break;

            default: // nothing set.
                sceneName = "";
                break;
        }

        // no round to start
        if (sceneName == "")
        {
            Debug.LogError("No match to start. Start failure.");
            return;
        }


        // finds game builder if not set.
        if (gameBuilder == null)
            SetGameBuilder();

        // if no players have joined.
        // if((p2Join || p3Join || p4Join) == false)
        // {
        //     Debug.LogAssertion("No players have joined.");
        //     return;
        // }

        // players
        int plyrCount = 0;

        // goes through each player
        for(int i = 1; i <= serverEndpoints + 1; i++)
        {
            GameBuilder.playables p = GameBuilder.playables.none;
            bool joined = false;

            // gets player
            switch(i)
            {
                case 1:
                    p = p1;
                    joined = true;
                    break;
                case 2:
                    p = p2;
                    joined = p2Join;
                    break;
                case 3:
                    p = p3;
                    joined = p3Join;
                    break;
                case 4:
                    p = p4;
                    joined = p4Join;
                    break;
            }

            // if the player is set to none, it means it wasn't set.
            // TODO: change to use joined.
            if (p == GameBuilder.playables.none)
                continue;
            else
                plyrCount++;

            // adds player to game builder.
            gameBuilder.AddPlayer(i, p);
        }

        // set to load the game.
        gameBuilder.SetLoadGame(true);
        gameBuilder.SetLoadStage(false);
        gameBuilder.sceneAfterGame = "LobbyScene";

        // change the scene.
        SceneChanger.ChangeScene(sceneName);
    }

    // called when level is loaded.
    private void OnLevelWasLoaded(int level)
    {
        // name of the level.
        string levelName = SceneChanger.GetSceneName(level);
       
        // gameplay scene loaded
        if(levelName == "HalloweenMap" || levelName == "ChristmasMap" || levelName == "ValentinesMap")
        {
            OnMatchStart();
        }
        else if(levelName == "LobbyScene") // lobby loaded.
        {
            // finds online game manager if not set.
            if (onlineGameManager == null)
                onlineGameManager = FindObjectOfType<OnlineGameplayManager>();

            // turn off online gameplay manager
            // TODO: maybe delete and readd component?
            if (onlineGameManager != null)
                onlineGameManager.enabled = false;
        }
    }

    // called when the match is started.
    private void OnMatchStart()
    {
        // finds online game manager if not set.
        if (onlineGameManager == null)
            onlineGameManager = FindObjectOfType<OnlineGameplayManager>();

        // activates gameplay manager.
        if (onlineGameManager != null)
            onlineGameManager.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        // if(isMaster) // acting as server
        // {
        //     ReceiveDataFromClients();
        //     SendDataToClients();
        // }
        // else // acting as client
        // {
        //     SendDataToServer();
        //     ReceiveDataFromServer();
        // }
        
    }

    // OnDestroy is called when an object is being destroyed.
    private void OnDestroy()
    {
        // shuts down server and client.
        if(server != null)
            server.ShutdownServer();
        
        if(client != null)
            client.ShutdownClient();

    }
}
