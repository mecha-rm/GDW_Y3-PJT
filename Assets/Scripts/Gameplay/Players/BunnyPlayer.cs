using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// bunny
public class BunnyPlayer : PlayerObject
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();

        speedMult = 1.0F;
        knockbackMult = 1.0F;
        jumpMult = 1.5F;
        defenseMult = 1.0F;

        // replacing sounds

        // idle sound
        if(sfx_Idle.clip == null)
        {
            // Destroy(sfx_Idle.clip);
            sfx_Idle.clip = (AudioClip)Resources.Load("Audio/Cat/SFX_BUNNY_NOISES_01");
        }

        // death sound
        if (sfx_Death.clip == null)
        {
            // Destroy(sfx_Idle.clip);
            sfx_Death.clip = (AudioClip)Resources.Load("Audio/Cat/SFX_BUNNY_DEATH");
        }

        // instantiates the game object.
        playerIconPrefab = "Prefabs/UI/Bunny Box";
        playerIcon = Instantiate(Resources.Load(playerIconPrefab)) as GameObject;
        
        // gets text object.
        if(playerIcon != null && playerScoreText == null)
        {
            playerScoreText = playerIcon.GetComponentInChildren<Text>();
        }


        //GameObject parentObject = GameObject.Find("Players");
        //int childCount = parentObject.transform.childCount;

        //for (int index = 0; index < childCount; index++)
        //{
        //    GameObject childObject = parentObject.transform.GetChild(index).gameObject;
        //    if (childObject.name == playername)
        //    {
        //        childObject.SetActive(true);
        //        playerIcon = childObject;
        //    }
  
        //}

    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }
}
