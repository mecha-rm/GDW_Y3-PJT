using System;
using System.Net;
using System.Net.NetworkInformation;
using UnityEngine;
using System.Text;

// manages online lobby
public class OnlineLobbyManager : MonoBehaviour
{
    // if the size changes, or if the status is different, then the games load.

    /// FORMAT: CLIENTS TO SERVER
    /// <summary>
    /// 1. Status
    /// 2. Names (1 - 3)
    /// 3. Stages (1 - 3)
    /// 4. Characters (1 -3)
    /// 5. Wins (1 - 3)
    /// </summary>


    /// FORMAT: SERVER TO CLIENTS
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
    public int serverBufferSize = 256;
    public int clientBufferSize = 256;

    // ip address
    public string ipAddress;

    // selected stage (defaults to halloween)
    public GameBuilder.stages p1Stage = GameBuilder.stages.halloween;
    // other stages
    public GameBuilder.stages p2Stage, p3Stage, p4Stage;

    // stage received from server
    public GameBuilder.stages recStage;

    // the saved name of the player
    private const int NAME_CHAR_LIMIT = 16; //
    private const string NO_NAME_CHAR = "-";
    private string p1Name = "", p2Name = "", p3Name = "", p4Name = "";

    // players
    public GameBuilder.playables p1 = GameBuilder.playables.dog, p2, p3, p4;

    // start time for timer
    public int startTime;

    // win minimum nad maximum.
    public int winScore; // the winning score
    public const int WIN_MIN = 5, WIN_MAX = 100;

    // checks to see if the following connections are being used.
    private bool p2Join = false, p3Join = false, p4Join = false;

    // win count for all players
    private int p1Wins = 0, p2Wins = 0, p3Wins = 0, p4Wins = 0;

    // the online gameplay manager.
    public OnlineGameplayManager onlineGameManager;

    // is 'true' if in the lobby, false if not.
    private bool inLobby = true;

    // TODO: add game builder.
    public GameBuilder gameBuilder;

    // number of instances of this class.
    private static int instances = 0;

    // states that the object shouldn't be destroyed on load.
    private void Awake()
    {
        instances++; // instance exists

        if (instances > 1) // instance already exists, so use that.
        {
            // destroys this object so it isn't used.
            Destroy(gameObject);
        }
        else // no instance exists
        {
            DontDestroyOnLoad(gameObject); // don't destroy this object.
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // sets player name
        p1Name = GameSettings.GetInstance().GetScreenName();
        p2Name = NO_NAME_CHAR;
        p3Name = NO_NAME_CHAR;
        p4Name = NO_NAME_CHAR;

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

    // player 1 name
    public string player1Name
    {
        get
        {
            return p1Name;
        }
    }

    // player 2 name
    public string player2Name
    {
        get
        {
            return p2Name;
        }
    }

    // player 3 name
    public string player3Name
    {
        get
        {
            return p3Name;
        }
    }

    // player 4 name
    public string player4Name
    {
        get
        {
            return p4Name;
        }
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

    // UTILITY - truncates or extends string if too large.
    public static string SetStringLength(string str, int charCount, string fillChar = " ")
    {
        if (str.Length == charCount) // no changes
        {
            return str;
        }
        else if (str.Length > charCount) // too large
        {
            return str.Substring(0, charCount);
        }
        else if (str.Length < charCount) // too small
        {
            // string 2
            string str2 = str;

            do
            {
                // extend string.
                str2 += fillChar;
            }
            while (str2.Length < charCount);

            return str2;
        }

        return str;
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
            // gets buffer data
            byte[] data = server.server.GetReceiveBufferData(i);
            int index = 0;

            // 1. Status
            // 2. Names
            // 3. Stages
            // 4. Characters
            // 5. Wins

            // status (0 = unconnected, 1 = connected, 2 = enter play)
            {
                int status = BitConverter.ToInt32(data, index);
                index += sizeof(int);

                // status
                switch (status)
                {
                    case 0: // not connected.
                        // TODO: change colour to show not connected.
                        switch (i) // change name to show there's no connection.
                        {
                            case 0:
                                p2Name = NO_NAME_CHAR;
                                p2Join = false;
                                break;
                            case 1:
                                p3Name = NO_NAME_CHAR;
                                p3Join = false;
                                break;
                            case 2:
                                p4Name = NO_NAME_CHAR;
                                p4Join = false;
                                break;
                        }

                        break;
                    case 1: // connected
                        switch (i) // change join values
                        {
                            case 0: // p2
                                p2Join = true;
                                break;
                            case 1: // p3
                                p3Join = true;
                                break;
                            case 2: // p4
                                p4Join = true;
                                break;
                        }
                        break;
                    case 2: // entered game (should not be used)
                        break;
                    default:
                        break;
                }

                // no data to get.
                if (status == 0)
                    continue;
            }
            
            // name 
            {
                string recName = BitConverter.ToString(data, index, NAME_CHAR_LIMIT * sizeof(char));

                // lenght of the name times size of chars.
                index += (recName.Length * sizeof(char));
                
            }

            // stage
            {
                // stage
                int stageInt = BitConverter.ToInt32(data, index);
                
                // sets stage information
                switch(i)
                {
                    case 0:
                        p2Stage = (GameBuilder.stages)(stageInt);
                        break;
                    case 1:
                        p3Stage = (GameBuilder.stages)(stageInt);
                        break;
                    case 2:
                        p4Stage = (GameBuilder.stages)(stageInt);
                        break;
                }

                // next
                index += sizeof(int);
            }

            // character
            {
                // character number
                int charValue = BitConverter.ToInt32(data, index);

                // sets stage information
                switch (i)
                {
                    case 0:
                        p2 = (GameBuilder.playables)(charValue);
                        break;
                    case 1:
                        p3 = (GameBuilder.playables)(charValue);
                        break;
                    case 2:
                        p4 = (GameBuilder.playables)(charValue);
                        break;
                }

                // next
                index += sizeof(int);
            }

            // win count
            {
                // win count
                int winCount = BitConverter.ToInt32(data, index);

                // sets win count information
                switch (i)
                {
                    case 0:
                        p2Wins = winCount;
                        break;
                    case 1:
                        p3Wins = winCount;
                        break;
                    case 2:
                        p4Wins = winCount;
                        break;
                }

                // next
                index += sizeof(int);
            }

        }


    }

    // sends the data to the clients
    void SendDataToClients()
    {
        // data to be sent out to clients.
        byte[] sendData = new byte[serverBufferSize];
        int index = 0;

        // 1. Status
        // 2. Player Count
        // 3. Stage Choice
        // 4. Player Name
        // 5. Player Character
        // 6. Player Win Count

        // status
        {
            // becomes set to '2' when going onto another scene.
            int status = 1; // TODO: have parameter for 2

            byte[] data = BitConverter.GetBytes(status);
            Buffer.BlockCopy(data, 0, sendData, index, data.Length);
            index += data.Length;
        }

        // Player Count
        {
            // becomes set to '2' when going onto another scene.
            int pCount = 1; // TODO: have parameter for 2

            // player 2 has joined.
            if (p2Join)
                pCount++;

            // player 3 has joined.
            if (p3Join)
                pCount++;

            // player 4 has joined.
            if (p4Join)
                pCount++;

            byte[] data = BitConverter.GetBytes(pCount);
            Buffer.BlockCopy(data, 0, sendData, index, data.Length);
            index += data.Length;
        }

        // stage
        {
            // TODO: randomize instead of using P1s.
            byte[] data = BitConverter.GetBytes((int)p1Stage);
            Buffer.BlockCopy(data, 0, sendData, index, data.Length);
            index += data.Length;
        }

        // name
        {
            // temporary string.
            string nameStr;

            // p1
            // if (p1Name != "")
            {
                nameStr = SetStringLength(p1Name, NAME_CHAR_LIMIT);
                byte[] data = Encoding.ASCII.GetBytes(nameStr);
                Buffer.BlockCopy(data, 0, sendData, index, data.Length);
                index += data.Length;
            }

            // p2
            if (p2Join)
            {
                nameStr = SetStringLength(p2Name, NAME_CHAR_LIMIT);
                byte[] data = Encoding.ASCII.GetBytes(nameStr);
                Buffer.BlockCopy(data, 0, sendData, index, data.Length);
                index += data.Length;
            }

            // p3
            if (p3Join)
            {
                nameStr = SetStringLength(p3Name, NAME_CHAR_LIMIT);
                byte[] data = Encoding.ASCII.GetBytes(nameStr);
                Buffer.BlockCopy(data, 0, sendData, index, data.Length);
                index += data.Length;
            }

            // p4
            if (p4Join)
            {
                nameStr = SetStringLength(p4Name, NAME_CHAR_LIMIT);
                byte[] data = Encoding.ASCII.GetBytes(nameStr);
                Buffer.BlockCopy(data, 0, sendData, index, data.Length);
                index += data.Length;
            }
        }

        // character
        {
            // player 1 has joined.
            // if (p1 != GameBuilder.playables.none)
            {   
                byte[] data = BitConverter.GetBytes((int)p1);
                Buffer.BlockCopy(data, 0, sendData, index, data.Length);
                index += data.Length;
            }

            // player 2 has joined.
            if (p2Join)
            {
                byte[] data = BitConverter.GetBytes((int)p2);
                Buffer.BlockCopy(data, 0, sendData, index, data.Length);
                index += data.Length;
            }

            // player 3 has joined.
            if (p3Join)
            {
                byte[] data = BitConverter.GetBytes((int)p3);
                Buffer.BlockCopy(data, 0, sendData, index, data.Length);
                index += data.Length;
            }

            // player 4 has joined.
            if(p4Join)
            {
                byte[] data = BitConverter.GetBytes((int)p4);
                Buffer.BlockCopy(data, 0, sendData, index, data.Length);
                index += data.Length;
            }
        }

        // wins
        {
            // player 1 has joined.
            // if (p1 != GameBuilder.playables.none)
            {
                byte[] data = BitConverter.GetBytes(p1Wins);
                Buffer.BlockCopy(data, 0, sendData, index, data.Length);
                index += data.Length;
            }

            // player 2 has joined.
            if (p2Join)
            {
                byte[] data = BitConverter.GetBytes(p2Wins);
                Buffer.BlockCopy(data, 0, sendData, index, data.Length);
                index += data.Length;
            }

            // player 3 has joined.
            if (p3Join)
            {
                byte[] data = BitConverter.GetBytes(p3Wins);
                Buffer.BlockCopy(data, 0, sendData, index, data.Length);
                index += data.Length;
            }

            // player 4 has joined.
            if (p4Join)
            {
                byte[] data = BitConverter.GetBytes(p4Wins);
                Buffer.BlockCopy(data, 0, sendData, index, data.Length);
                index += data.Length;
            }
        }

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

        // 1. Status
        // 2. Name
        // 3. Stages
        // 4. Characters
        // 5. Wins


        // Status
        {
            // becomes set to '2' when going onto another scene.
            int status = 1; // TODO: have parameter for 2

            byte[] data = BitConverter.GetBytes(status);
            Buffer.BlockCopy(data, 0, sendData, index, data.Length);
            index += data.Length;
        }

        // Name
        {
            string nameStr = SetStringLength(p1Name, NAME_CHAR_LIMIT);
            byte[] data = Encoding.ASCII.GetBytes(nameStr);
            Buffer.BlockCopy(data, 0, sendData, index, data.Length);
            index += data.Length;
        }

        // Stage
        {
            byte[] data = BitConverter.GetBytes((int)p1Stage);
            Buffer.BlockCopy(data, 0, sendData, index, data.Length);
            index += data.Length;
        }

        // Character
        {
            byte[] data = BitConverter.GetBytes((int)p1);
            Buffer.BlockCopy(data, 0, sendData, index, data.Length);
            index += data.Length;
        }

        // Wins
        {
            byte[] data = BitConverter.GetBytes(p1Wins);
            Buffer.BlockCopy(data, 0, sendData, index, data.Length);
            index += data.Length;
        }


        // sets data
        client.client.SetSendBufferData(sendData);
    }

    // receives data from the server.
    void ReceiveDataFromServer()
    {
        // 1. Status
        // 2. Player Count
        // 3. Stage Choice
        // 4. Player Names
        // 5. Player Character
        // 6. Player Win Count

        // data to be sent out to clients.
        byte[] recData = client.client.GetReceiveBufferData();
        int index = 0;

        int status = -1;
        int plyrCount = -1;
        int stageInt = -1;

        // status
        {
            status = BitConverter.ToInt32(recData, index);
            index += sizeof(int);
        }

        // player count
        {
            plyrCount = BitConverter.ToInt32(recData, index);
            index += sizeof(int);
        }

        // stage choice (from server)
        {
            stageInt = BitConverter.ToInt32(recData, index);
            recStage = (GameBuilder.stages)stageInt;
            index += sizeof(int);
        }

        // player names
        for(int i = 1; i <= plyrCount; i++)
        {
            string recName = BitConverter.ToString(recData, index, NAME_CHAR_LIMIT * sizeof(char));

            // saves name to right variable.
            // NOTE: need identifiers for this.
            switch(i)
            {
                case 1: // p1
                    p1Name = recName;
                    break;

                case 2: // p2 (on the local side p2 is player 1)
                    p2Name = recName;
                    break;

                case 3: // p3
                    p3Name = recName;
                    break;

                case 4: // p4
                    p4Name = recName;
                    break;

                default:
                    break;
            }

            // lenght of the name times size of chars.
            index += (recName.Length * sizeof(char));
        }


        // player characters
        for (int i = 1; i <= plyrCount; i++)
        {
            int pChar = BitConverter.ToInt32(recData, index);

            // saves name to right variable.
            // NOTE: need identifiers for this.
            switch (i)
            {
                case 1: // p1
                    p1 = (GameBuilder.playables)(pChar);
                    break;

                case 2: // p2 (on the local side p2 is player 1)
                    p2 = (GameBuilder.playables)(pChar);
                    break;

                case 3: // p3
                    p3 = (GameBuilder.playables)(pChar);
                    break;

                case 4: // p4
                    p4 = (GameBuilder.playables)(pChar);
                    break;

                default:
                    break;
            }

            // lenght of the name times size of chars.
            index += sizeof(int);
        }

        // win count
        for (int i = 1; i <= plyrCount; i++)
        {
            int winCount = BitConverter.ToInt32(recData, index);

            // saves name to right variable.
            // NOTE: need identifiers for this.
            switch (i)
            {
                case 1: // p1
                    p1Wins = winCount;
                    break;

                case 2: // p2 (on the local side p2 is player 1)
                    p2Wins = winCount;
                    break;

                case 3: // p3
                    p3Wins = winCount;
                    break;

                case 4: // p4
                    p4Wins = winCount;
                    break;

                default:
                    break;
            }

            // lenght of the name times size of chars.
            index += sizeof(int);
        }

        // Move Onto Gameplay Scene
        if (status == 2)
            PreMatchStart();
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
        p1Stage = stageEnum;
    }

    // on stage selection
    public void SetStage(int stageNum)
    {
        p1Stage = (GameBuilder.stages)stageNum;
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
                Debug.Log("Game Builder Found.");
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
        switch (p1Stage)
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
            OnReturnToLobby();
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
        {
            onlineGameManager.enabled = true;
            onlineGameManager.isMaster = isMaster;
        }

        // now in lobby
        inLobby = false;
    }

    // called when returning to the lobby.
    public void OnReturnToLobby()
    {
        // finds online game manager if not set.
        if (onlineGameManager == null)
            onlineGameManager = FindObjectOfType<OnlineGameplayManager>();

        // turn off online gameplay manager
        // TODO: maybe delete and re-add component?
        if (onlineGameManager != null)
            onlineGameManager.enabled = false;

        // game builder was deleted.
        // if(gameBuilder == null)
        //     SetGameBuilder();

        // recreates game builder since it will be deleted.
        // GameObject newObject = Instantiate(Resources.Load("Prefabs/Title Game Builder") as GameObject);

        // now in lobby
        inLobby = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameBuilder == null)
            SetGameBuilder();

        // TODO: add identifiers for the game.
        // if in the lobby, recieve data from clients and send data to them.
        if(inLobby)
        {
            // if(isMaster) // acting as server
            // {
            //     ReceiveDataFromClients();
            //     SendDataToClients();
            // }
            // else // acting as client
            // {
            //     // moved onto gameplay.
            //     if(client.client.GetReceiveBufferSize() != clientBufferSize)
            //     {
            //         // calls for prematch start.
            //         PreMatchStart();
            //     }
            //     else
            //     {
            //         SendDataToServer();
            //         ReceiveDataFromServer();
            //     }
            // 
            //     
            // }
        
        }
    }

    // OnDestroy is called when an object is being destroyed.
    private void OnDestroy()
    {
        // shuts down server and client.
        if(server != null)
            server.ShutdownServer();
        
        if(client != null)
            client.ShutdownClient();

        // this instance has been destroyed.
        instances--;
    }
}
