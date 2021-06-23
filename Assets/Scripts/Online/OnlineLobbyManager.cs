using System;
using System.Net;
using System.Net.NetworkInformation;
using UnityEngine;
using System.Text;
using System.Collections.Generic;

// manages online lobby
public class OnlineLobbyManager : MonoBehaviour
{
    // TODO: randomize stage each time the data is sent to the client instead?

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

    // TODO: going to change up the setup to use RecPlayer for everything.
    // recieved character
    public struct LobbyPlayer
    {
        public string name;
        public GameBuilder.playables character;
        public GameBuilder.stages stage;
        public int wins;
    }


    // checks to see if this is the one hosting or not.
    public bool isMaster = true;

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
    public GameBuilder.stages recStage = GameBuilder.stages.halloween;

    // the saved name of the player
    private const int NAME_CHAR_LIMIT = 16; //
    private const string NO_NAME_CHAR = "-";
    private string p1Name = "", p2Name = "", p3Name = "", p4Name = "";

    // players
    public GameBuilder.playables p1Char = GameBuilder.playables.dog;
    public GameBuilder.playables p2Char, p3Char, p4Char;

    // room size
    public int roomSize = 2;

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


    // if 'true', the match starts on the next update.
    public bool startMatchOnUpdate = false;

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

            // room size increase.
            roomSize = serverEndpoints + 1;
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
    public bool RunHost()
    {
        // ip address is not set.
        if(ipAddress == "")
        {
            Debug.LogError("No IP Address Set.");
            return false;
        }

        if(isMaster && server != null) // run server
        {
            // server is already running
            if (server.server.IsRunning())
            {
                Debug.LogAssertion("Server is already running");
                return false;
            }

            // checks for valid ip address.
            if (!ValidIPAddress(ipAddress))
            {
                Debug.LogAssertion("Invalid room code.");
                return false;
            }


            // sets the ip address and runs the server.
            server.SetIPAddress(ipAddress);
            server.SetBlockingSockets(blocking);
            server.RunServer();

            // message
            Debug.Log("Server is now running");

            return server.server.IsRunning();
        }
        else if (!isMaster && client != null) // run client.
        {
            // server is already running
            if (client.client.IsRunning())
            {
                Debug.LogAssertion("Server is already running");
                return false;
            }

            // checks for valid ip address.
            if (!ValidIPAddress(ipAddress))
            {
                Debug.LogAssertion("Invalid room code.");
                return false;
            }

            // host not available
            if (!CheckHostAvailability())
            {
                Debug.LogAssertion("Host not available");
                return false;
            }


            // set client
            client.SetIPAddress(ipAddress);
            client.SetBlockingSockets(blocking);
            client.RunClient();

            // message
            Debug.Log("Client is now running");

            return client.client.IsRunning();
        }

        return false;
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
                {
                    // Debug.Log("Zero Status");
                    continue;
                }
                    
            }
            
            // name (conversion is broken)
            {
                string recName = Encoding.UTF8.GetString(data, index, NAME_CHAR_LIMIT);

                // received name setting
                switch (i)
                {
                    case 0:
                        p2Name = recName;
                        break;
                    case 1:
                        p3Name = recName;
                        break;
                    case 2:
                        p4Name = recName;
                        break;
                }

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
                        p2Char = (GameBuilder.playables)(charValue);
                        break;
                    case 1:
                        p3Char = (GameBuilder.playables)(charValue);
                        break;
                    case 2:
                        p4Char = (GameBuilder.playables)(charValue);
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
            int status = 0; // TODO: have parameter for 2

            // changes status based on if the match is starting or not.
            if (startMatchOnUpdate)
                status = 2;
            else
                status = 1;

            byte[] data = BitConverter.GetBytes(status);
            Buffer.BlockCopy(data, 0, sendData, index, data.Length);
            index += data.Length;
        }

        // Player Count
        {
            // becomes set to '2' when going onto another scene.
            // int pCount = 1; // TODO: have parameter for 2
            // 
            // // player 2 has joined.
            // if (p2Join)
            //     pCount++;
            // 
            // // player 3 has joined.
            // if (p3Join)
            //     pCount++;
            // 
            // // player 4 has joined.
            // if (p4Join)
            //     pCount++;
            
            // player count.
            int pCount = roomSize;

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
                byte[] data = Encoding.UTF8.GetBytes(nameStr);
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
                byte[] data = BitConverter.GetBytes((int)p1Char);
                Buffer.BlockCopy(data, 0, sendData, index, data.Length);
                index += data.Length;
            }

            // player 2 has joined.
            if (p2Join)
            {
                byte[] data = BitConverter.GetBytes((int)p2Char);
                Buffer.BlockCopy(data, 0, sendData, index, data.Length);
                index += data.Length;
            }

            // player 3 has joined.
            if (p3Join)
            {
                byte[] data = BitConverter.GetBytes((int)p3Char);
                Buffer.BlockCopy(data, 0, sendData, index, data.Length);
                index += data.Length;
            }

            // player 4 has joined.
            if(p4Join)
            {
                byte[] data = BitConverter.GetBytes((int)p4Char);
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
            byte[] data = System.Text.Encoding.UTF8.GetBytes(nameStr);
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
            byte[] data = BitConverter.GetBytes((int)p1Char);
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

        // list of players
        // these get applied to the objects after all data is received.
        // keep in mind that the player numbers on the server side and not the same as on the client side.
        // this will be fixed at the end when it comes to matching the data.
        // List<LobbyPlayer> plyrs = new List<LobbyPlayer>();
        
        // the up to four bits of player data that has been received.
        LobbyPlayer p1Rec = new LobbyPlayer();
        p1Rec.name = "";

        LobbyPlayer p2Rec = new LobbyPlayer();
        p2Rec.name = "";

        LobbyPlayer p3Rec = new LobbyPlayer();
        p3Rec.name = "";

        LobbyPlayer p4Rec = new LobbyPlayer();
        p4Rec.name = "";


        // values
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
            roomSize = plyrCount;
            index += sizeof(int);
        }

        // stage choice (from server)
        {
            stageInt = BitConverter.ToInt32(recData, index);
            recStage = (GameBuilder.stages)stageInt;
            index += sizeof(int);
        }

        // player names
        for(int i = 0; i < plyrCount; i++)
        {
            // gets the name
            string recName = System.Text.Encoding.UTF8.GetString(recData, index, NAME_CHAR_LIMIT);

            // saves name to right variable.
            switch(i)
            {
                case 1: // p1
                    p1Rec.name = recName;
                    break;
            
                case 2: // p2
                    p2Rec.name = recName;
                    break;
            
                case 3: // p3
                    p3Rec.name = recName;
                    break;
            
                case 4: // p4
                    p4Rec.name = recName;
                    break;
            
                default:
                    break;
            }

            // lenght of the name times size of chars.
            index += (recName.Length * sizeof(char));
        }


        // player characters
        for (int i = 0; i < plyrCount; i++)
        {
            // player character.
            int pChar = BitConverter.ToInt32(recData, index);

            // save character
            GameBuilder.playables px = (GameBuilder.playables)(pChar);

            // saves name to right variable.
            switch (i)
            {
                case 1: // p1
                    p1Rec.character = (GameBuilder.playables)(pChar);
                    break;
            
                case 2: // p2
                    p2Rec.character = (GameBuilder.playables)(pChar);
                    break;
            
                case 3: // p3
                    p3Rec.character = (GameBuilder.playables)(pChar);
                    break;
            
                case 4: // p4
                    p4Rec.character = (GameBuilder.playables)(pChar);
                    break;
            
                default:
                    break;
            }

            // lenght of the name times size of chars.
            index += sizeof(int);
        }

        // win count
        for (int i = 0; i < plyrCount; i++)
        {
            // get win count
            int winCount = BitConverter.ToInt32(recData, index);

            // saves name to right variable.
            switch (i)
            {
                case 1: // p1
                    p1Rec.wins = winCount;
                    break;
            
                case 2: // p2
                    p2Rec.wins = winCount;
                    break;
            
                case 3: // p3
                    p3Rec.wins = winCount;
                    break;
            
                case 4: // p4
                    p4Rec.wins = winCount;
                    break;
            
                default:
                    break;
            }

            // lenght of the name times size of chars.
            index += sizeof(int);
        }

        // match the data to the players
        List<LobbyPlayer> plyrs = new List<LobbyPlayer>();

        // index of the local player
        int localIndex = -1;

        // if a name is not listed, then data wasn't sent.
        if(p1Rec.name != "") // p1
            plyrs.Add(p1Rec);

        if (p2Rec.name != "") // p2
            plyrs.Add(p2Rec);
        
        if (p3Rec.name != "") // p3
            plyrs.Add(p3Rec);

        if (p4Rec.name != "") // p4
            plyrs.Add(p4Rec);

        // loops through
        for(int i = 0; i < plyrs.Count; i++)
        {
            // local player found.
            if (plyrs[i].name == p1Name)
            {
                localIndex = i; // gets the index
                break;
            }
        }

        // removes the index of the local player.
        plyrs.RemoveAt(localIndex);

        // sets all the join variables to false in case the status has changed.
        p2Join = false;
        p3Join = false;
        p4Join = false;

        // applies the data for the rest of the objects.
        for(int i = 0; i < plyrs.Count; i++)
        {
            // goes through each player object.
            // note that the server only sends over its stage choice.
            // the server does not send the stage choice from the other players.
            // as such, each player is just given the server's stage.
            switch(i)
            {
                case 0: // p2
                    p2Name = plyrs[i].name;
                    p2Char = plyrs[i].character;
                    p2Stage = recStage;
                    p2Wins = plyrs[i].wins;
                    p2Join = true;
                    
                    break;

                case 1: // p3
                    p3Name = plyrs[i].name;
                    p3Char = plyrs[i].character;
                    p3Stage = recStage;
                    p3Wins = plyrs[i].wins;
                    p3Join = true;
                    
                    break;

                case 2: // p4
                    p4Name = plyrs[i].name;
                    p4Char = plyrs[i].character;
                    p4Stage = recStage;
                    p4Wins = plyrs[i].wins;
                    p4Join = true;
                    
                    break;
            }
        }

        // // set indexes
        // Stack<int> usedIndexes = new Stack<int>();
        // 
        // // set players
        // bool p1Set = false, p2Set = false, p3Set = false, p4Set = false;


        // // match data with players.
        // for(int i = 0; i < plyrCount; i++)
        // {
        //     if(p1Name == plyrs[i].name) // p1 (local player)
        //     {
        //         // skip if set to local player.
        //         usedIndexes.Push(i);
        // 
        //         // p1 has been set.
        //         p1Set = true;
        // 
        //         // p1Name = plyrs[i].name;
        //         // p1Stage = plyrs[i].stage;
        //         // p1Wins = plyrs[i].wins;
        //     }
        //     else if(p2Name == plyrs[i].name) // p2
        //     {
        //         usedIndexes.Push(i);
        //         p2Stage = plyrs[i].stage;
        //         p2Wins = plyrs[i].wins;
        // 
        //         // p2 has been set.
        //         p2Set = true;
        //     }
        //     else if (p3Name == plyrs[i].name) // p3
        //     {
        //         usedIndexes.Push(i);
        //         p3Stage = plyrs[i].stage;
        //         p3Wins = plyrs[i].wins;
        // 
        //         // p3 has been set.
        //         p3Set = true;
        //     }
        //     else if (p4Name == plyrs[i].name) // p4
        //     {
        //         usedIndexes.Push(i);
        //         p4Stage = plyrs[i].stage;
        //         p4Wins = plyrs[i].wins;
        // 
        //         // p4 has been set.
        //         p4Set = true;
        //     }
        // }
        // 
        // 
        // // the used indexes
        // while(usedIndexes.Count != 0)
        // {
        //     int idx = usedIndexes.Pop(); // remove index
        //     plyrs.RemoveAt(idx); // remove value
        // }
        // 
        // 
        // // fit remaining players
        // for(int i = 0; i < plyrs.Count; i++)
        // {
        //     // checks what items are available.
        //     // p1 is ignored since that's local.
        //     // if(p1Set == false)
        //     // {
        //     //     //
        //     // }
        // 
        //     // p2 not set.
        //     if(p2Set == false)
        //     {
        //         p2Name = plyrs[i].name;
        //         p2Char = plyrs[i].character;
        //         p2Stage = plyrs[i].stage;
        //         p2Wins = plyrs[i].wins;
        //         p2Set = true;
        //     }
        //     else if(p3Set == false) // p3 not set.
        //     {
        //         p3Name = plyrs[i].name;
        //         p3Char = plyrs[i].character;
        //         p3Stage = plyrs[i].stage;
        //         p3Wins = plyrs[i].wins;
        //         p3Set = true;
        //     }
        //     else if(p4Set == false) // p4 not set.
        //     {
        //         p4Name = plyrs[i].name;
        //         p4Char = plyrs[i].character;
        //         p4Stage = plyrs[i].stage;
        //         p4Wins = plyrs[i].wins;
        //         p4Set = true;
        //     }
        // }


        // Move Onto Gameplay Scene
        if (status == 2)
        {
            // PreMatchStart();
            startMatchOnUpdate = true; // start match.
        }
    }

    // get endpoint count.
    public int GetEndPointCount()
    {
        return server.server.GetEndPointCount();
    }

    // set endpoint amount
    public void SetEndPointCount(int newCount)
    {
        // server exists check.
        if(server == null)
        {
            Debug.LogError("Server does not exist.");
        }

        // existing endpoint count.
        int oldCount = server.server.GetEndPointCount();

        if(newCount > oldCount) // count has increased.
        {
            // updated endpoint count.
            int updatedCount;

            // adds endpoints
            do
            {
                server.server.AddEndPoint(); // add endpoint
                updatedCount = server.server.GetEndPointCount(); // get current count.
            }
            while (newCount > updatedCount);
        }
        else if(newCount < oldCount) // count has decreased.
        {
            int updatedCount;

            // adds endpoints
            do
            {
                updatedCount = server.server.GetEndPointCount() - 1;

                // no more endpoints.
                if (updatedCount < 0)
                    break;

                server.server.RemoveEndPoint(updatedCount);
            }
            while (newCount < updatedCount);
        }

        // server endpoint count updated.
        serverEndpoints = server.server.GetEndPointCount();
        roomSize = serverEndpoints + 1;
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
        p1Char = plyr;
    }

    // sets the local palyer
    public void SetLocalPlayer(int plyr)
    {
        p1Char = (GameBuilder.playables)plyr;
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

        // sets builder.
        if (gameBuilder.manager == null)
            gameBuilder.manager = FindObjectOfType<GameplayManager>(true);

        // returns game builder
        return gameBuilder;
    }

    // called when the match is about to start (changes scene at the end of the function).
    public void PreMatchStart()
    {
        // TODO: check joined bools to get player counts. Also put them into a list for the loop.
        List<LobbyPlayer> plyrs = new List<LobbyPlayer>();

        // the scene name
        string sceneName = "";

        // TODO: optimize this
        // add player 1
        {
            LobbyPlayer rec = new LobbyPlayer();
            rec.name = p1Name;
            rec.character = p1Char;
            rec.stage = p1Stage;
            rec.wins = p1Wins;

            plyrs.Add(rec);
        }

        // add player 2
        if(p2Join)
        {
            LobbyPlayer rec = new LobbyPlayer();
            rec.name = p2Name;
            rec.character = p2Char;
            rec.stage = p2Stage;
            rec.wins = p2Wins;

            plyrs.Add(rec);
        }

        // add player 3
        if(p3Join)
        {
            LobbyPlayer rec = new LobbyPlayer();
            rec.name = p3Name;
            rec.character = p3Char;
            rec.stage = p3Stage;
            rec.wins = p3Wins;

            plyrs.Add(rec);
        }

        // add player 4
        if(p4Join)
        {
            LobbyPlayer rec = new LobbyPlayer();
            rec.name = p4Name;
            rec.character = p4Char;
            rec.stage = p4Stage;
            rec.wins = p4Wins;

            plyrs.Add(rec);
        }



        // selecting the stage.
        {
            // selecting the stage.
            // sets the next stage. TODO: randomize stages
            // TODO: have player 1 choose the stage instead of randomize them. 

            // the host chooses the stage.
            // spawning still having problems.
            GameBuilder.stages chosenStage = (isMaster) ? p1Stage : recStage;

            // TODO: remove
            // original
            // if(plyrs.Count != 0)
            // {
            //     // new - gets random stage
            //     int index = UnityEngine.Random.Range(0, plyrs.Count);
            //     chosenStage = plyrs[index].stage;
            // }
            // else // list is empty
            // {
            //     chosenStage = (isMaster) ? p1Stage : recStage;
            // }
             
            // stage (checks to see if stage exists)
            switch (chosenStage)
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
        // int plyrCount = roomSize;
        // int joinedPlayers = 0;

        // goes through each player that's being let into the match.
        // for (int i = 1; i <= plyrs.Count; i++)
        for (int i = 0; i < plyrs.Count; i++)
        {
            // gets the cahracter
            GameBuilder.playables p = plyrs[i].character;

            // there is no character, so set it to the default.
            if (p == GameBuilder.playables.none)
                p = GameBuilder.playables.dog;

            // adds player to game builder.
            gameBuilder.AddPlayer(i, p);


            // GameBuilder.playables p = GameBuilder.playables.none;
            // bool isP1 = false;
            // bool joined = false;
            // 
            // // gets player
            // switch (i)
            // {
            //     case 1:
            //         p = p1Char;
            //         joined = true;
            //         isP1 = true;
            //         break;
            //     case 2:
            //         p = p2Char;
            //         joined = p2Join;
            //         break;
            //     case 3:
            //         p = p3Char;
            //         joined = p3Join;
            //         break;
            //     case 4:
            //         p = p4Char;
            //         joined = p4Join;
            //         break;
            // }
            // 
            // // if the player is set to none, it means it wasn't set.
            // // TODO: change to use joined.
            // // if (joined == false)
            // // {
            // //     continue;
            // // }
            // // else
            // // {
            // //     plyrCount++;
            // // }
            // 
            // plyrCount++;
            // 
            // // set game builder to dog if this is set to none.
            // // this should be commented out eventually.
            // if (p == GameBuilder.playables.none)
            //     p = GameBuilder.playables.dog;
            // 
            // // adds player to game builder.
            // if (joined == true)
            // {
            //     if (isP1)
            //         gameBuilder.AddPlayer(i, p);
            //     else
            //         gameBuilder.AddPlayer(i, p);
            // 
            //     joinedPlayers++;
            // }

        }

        // if no players joined, it adds a default.
        // if (joinedPlayers == 0)
        //     gameBuilder.AddPlayer(1, GameBuilder.playables.dog);

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
           if(isMaster) // acting as server
           {
               ReceiveDataFromClients();
               SendDataToClients();
           }
           else // acting as client
           {
                // moved onto gameplay
                // int recSize = client.client.GetReceiveBufferSize();
                // 
                // 
                // if (recSize != clientBufferSize && recSize != 0)
                // {
                //     // calls for prematch start.
                //     PreMatchStart();
                // }
                // else
                {
                    SendDataToServer();
                    ReceiveDataFromServer();
                }     
           }

           // starts the match.
            if (startMatchOnUpdate)
                PreMatchStart();
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
