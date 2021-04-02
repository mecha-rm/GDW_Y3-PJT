using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// cat
public class CatPlayer : PlayerObject
{
    // Start is called before the first frame update
    string playername = "catbox";

    void Start()
    {
        base.Start();

        // replacing sounds
        {
            // TODO: randomize sound for cat meow
            // Destroy(sfx_Idle.clip); // regular destroy didn't work.
            if (sfx_Idle.clip == null)
            {
                DestroyImmediate(sfx_Idle.clip, true);
                sfx_Idle.clip = (AudioClip)Resources.Load("Audio/Cat/SFX_CAT_MEOW_02");
            }

            // Destroy(sfx_Death.clip);
            if(sfx_Death.clip == null)
            {
                DestroyImmediate(sfx_Death.clip, true);
                sfx_Death.clip = (AudioClip)Resources.Load("Audio/Cat/SFX_CAT_DEATH");
            }
        }

        speedMult = 1.5F;
        knockbackMult = 1.0F;
        jumpMult = 1.0F;
        defenseMult = 1.0F;

        // instantiates the game object.
        playerIconPrefab = "Prefabs/UI/Cat Box";
        playerIcon = Instantiate(Resources.Load(playerIconPrefab)) as GameObject;

        // gets text object.
        if (playerIcon != null)
        {
            playerScoreText = playerIcon.GetComponentInChildren<Text>();
        }

        //icons
        GameObject parentObject = GameObject.Find("Players");
        int childCount = parentObject.transform.childCount;

        for (int index = 0; index < childCount; index++)
        {
            GameObject childObject = parentObject.transform.GetChild(index).gameObject;
            if (childObject.name == playername)
            {
                childObject.SetActive(true);
                playerIcon = childObject;
            }
 
        }

    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }
}
