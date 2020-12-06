using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public GameObject plane;
    public GameObject jumpPlatform;
    public GameObject flagPlane;
    public GameObject QuestLog;

    bool moveQuest;
    bool jumpQuest;
    bool pointQuest;

    bool toggledQuest;

    public Text Quests;


    // Start is called before the first frame update
    void Start()
    {
        moveQuest = false;
        jumpQuest = false;
        pointQuest = false;

        toggledQuest = false;

        Quests.text = "Learn to Move [ ]" +
            "\n\nMaster the Jump [ ]" +
            "\n\nCapture the Flag [ ] ";
    }

    // Update is called once per frame
    void Update()
    {
        if (moveQuest == true)
        {
            Quests.text = "Learn to Move [x]" +
                "\n\nMaster the Jump [ ]" +
                "\n\nCapture the Flag [ ] ";
            if (jumpQuest == true)
            {
                Quests.text = "Learn to Move [x]" +
               "\n\nMaster the Jump [x]" +
               "\n\nCapture the Flag [ ] ";
            }
            if (pointQuest == true)
            {
                Quests.text = "Learn to Move [x]" +
               "\n\nMaster the Jump [x]" +
               "\n\nCapture the Flag [x] ";
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {

        if (other.collider.name == "Movement")
        {
            moveQuest = true;
        }
        if (other.collider.name == "Jumping")
        {
            jumpQuest = true;
        }
        if (other.collider.name == "Point")
        {
            pointQuest = true;
        }
    }

    public void ToggleQuest()
    {
        if (toggledQuest == false)
        {
            QuestLog.SetActive(false); // hide object
        }
        if (toggledQuest == true)
        {
            QuestLog.SetActive(true);
        }
        toggledQuest = !toggledQuest;
    }
}
