using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// remote player script for online component
public class RemotePlayer : MonoBehaviour
{
    // FORMAT:
    // [0 - 10] - P1 Transform (PosX, PosY, P

    /// <summary>
    /// Format for writing data to the server.
    /// 
    /// </summary>

    /// <summary>
    /// Format for reading data from the server.
    /// Keep in mind that a float is 4 bytes long.
    /// * [0 - 47] - Player
    ///     - [0 - 3] - Player Number (4 bytes per int)
    ///     - [4 - 15] - Position (x, y, z) (4 bytes per float, 12 total)
    ///     - [16 - 27] - Scale (x, y, z) (4 bytes per float, 12 total)
    ///     - [28 - 43] - Rotation (x, y, z, w) (4 bytes per float, 16 total)
    ///     - [44 - 47] - Score (4 bytes per int, 4 bytes total)
    /// </summary>

    // player the remote player is attached to.
    PlayerObject player;

    // Start is called before the first frame update
    void Start()
    {
        // player not set
        if (player == null)
            GetComponent<PlayerObject>();
    }

    // Update is called once per frame
    void Update()
    {
        // player object is set, and the server is running
        if(player != null && NetworkLibrary.UdpServerXInterface.IsRunning())
        {
            // gets the player number
            int p = player.playerNumber;

            byte[] data = NetworkLibrary.UdpServerXInterface.GetReceiveBufferData(p - 1);
        }
    }
}
