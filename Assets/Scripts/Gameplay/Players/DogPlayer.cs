using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// dog
public class DogPlayer : PlayerObject
{
    // Start is called before the first frame update
    string playername = "Dogbox";

    void Start()
    {
        base.Start();

        speedMult = 1.0F;
        knockbackMult = 1.5F;
        jumpMult = 1.0F;
        defenseMult = 1.0F;

        // death sound
        if (sfx_Death.clip == null)
        {
            // Destroy(sfx_Idle.clip);
            sfx_Death.clip = (AudioClip)Resources.Load("Audio/Cat/SFX_DOG_DEATH");
        }

        // instantiates the game object.
        playerIconPrefab = "Prefabs/UI/Dog Box";
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
