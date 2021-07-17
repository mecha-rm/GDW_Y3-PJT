using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//  the game hud multiplayer manager
public class GameHudMpManager : MonoBehaviour
{
    // the gameplay manager
    public GameplayManager gameManager = null;

    // the time object
    public TimerObject timer = null;

    // timer text
    public Text timerText;

    [Header("Player Boxes")]
    // the icons to be enabled/disabled
    public GameObject p1Box = null;
    public GameObject p2Box = null;
    public GameObject p3Box = null;
    public GameObject p4Box = null;


    [Header("Images")]
    // the player icon images
    public RawImage p1Image = null;
    public RawImage p2Image = null;
    public RawImage p3Image = null;
    public RawImage p4Image = null;

    // the animal icons for the HUD
    public Texture2D dogIcon = null;
    public Texture2D catIcon = null;
    public Texture2D bunnyIcon = null;
    public Texture2D turtleIcon = null;


    [Header("Scores")]
    // use this to control the gameplay hud
    public Text p1Score = null;
    public Text p2Score = null;
    public Text p3Score = null;
    public Text p4Score = null;

    // used to avoid updating scores that haven't changed.
    private float p1Points = -100, p2Points = -100, p3Points = -100, p4Points = -100;

    [Header("Players")]
    // player object
    public PlayerObject p1 = null;
    public PlayerObject p2 = null;
    public PlayerObject p3 = null;
    public PlayerObject p4 = null;

    // the local player that the item pane is connected to.
    // public PlayerObject localPlayer = null;

    // Start is called before the first frame update
    void Start()
    {
        // finds the gameplay manager
        if (gameManager == null)
            gameManager = FindObjectOfType<GameplayManager>();

        // if there is a game manager
        if (gameManager != null)
        {
            // player 1
            if (p1 == null)
                p1 = gameManager.p1;

            // player 2
            if (p2 == null)
                p2 = gameManager.p2;

            // player 3
            if (p3 == null)
                p3 = gameManager.p3;

            // player 4
            if (p4 == null)
                p4 = gameManager.p4;

        }

        // timer object
        {
            // finds the time object
            if (timer == null)
                timer = FindObjectOfType<TimerObject>();
        }

        // loads up the hud icons
        {
            if (dogIcon == null) // dog
                dogIcon = (Texture2D)Resources.Load("Images/Icons/dog_icon");

            if (catIcon == null) // cat
                catIcon = (Texture2D)Resources.Load("Images/Icons/cat_icon");

            if (bunnyIcon == null) // bunny
                bunnyIcon = (Texture2D)Resources.Load("Images/Icons/bunny_icon");

            if (turtleIcon == null) // turtle
                turtleIcon = (Texture2D)Resources.Load("Images/Icons/turtle_icon");
        }

        // set values
        UpdateTime();
        UpdateBoxes();
        UpdateIcons();
        UpdateScores();
    }

    // updates the time value
    public void UpdateTime()
    {
        // if the time is not set
        if(timer != null && timerText != null)
        {
            // updates the timer text.
            timerText.text = timer.GetCurrentTimeValue().ToString("F2");
        }
    }

    // updates the player boxes to enable/disable visibility.
    public void UpdateBoxes()
    {
        // p1
        if (p1 != null && p1Box != null)
            p1Box.SetActive(true);
        else if(p1 == null && p1Box != null)
            p1Box.SetActive(false);


        // p2
        if (p2 != null && p2Box != null)
            p2Box.SetActive(true);
        else if (p2 == null && p2Box != null)
            p2Box.SetActive(false);


        // p3
        if (p3 != null && p3Box != null)
            p3Box.SetActive(true);
        else if (p3 == null  && p3Box != null)
            p3Box.SetActive(false);

        // p4
        if (p4 != null && p4Box != null)
            p4Box.SetActive(true);
        else if (p4 == null && p4Box != null)
            p4Box.SetActive(false);
    }

    // updates player icons
    public void UpdateIcons()
    {
        // player list
        List<PlayerObject> players = new List<PlayerObject>();

        // adds the four players
        players.Add(p1);
        players.Add(p2);
        players.Add(p3);
        players.Add(p4);

        // goes through all players
        for(int i = 0; i < players.Count; i++)
        {
            // no player available
            if (players[i] == null)
                continue;

            // the texture image
            Texture2D img = null;

            
            // the players
            GameBuilder.playables type = players[i].GetPlayerType();

            // TODO: check and see which one is less intensive.
            // ver 1. - check type
            switch(type)
            {
                case GameBuilder.playables.none: // none (use default)
                    break;
                case GameBuilder.playables.dog:
                    img = dogIcon;
                    break;
                case GameBuilder.playables.cat:
                    img = catIcon;
                    break;
                case GameBuilder.playables.bunny:
                    img = bunnyIcon;
                    break;
                case GameBuilder.playables.turtle:
                    img = turtleIcon;
                    break;
            }

            // ver 2. - downcast check
            // if (players[i] is DogPlayer) // dog
            // {
            //     img = dogIcon;
            // }
            // else if (players[i] is CatPlayer) // cat
            // {
            //     img = catIcon;
            // }
            // else if (players[i] is BunnyPlayer) // bunny
            // {
            //     img = bunnyIcon;
            // }
            // else if (players[i] is TurtlePlayer) // turtle
            // {
            //     img = turtleIcon;
            // }

            // image was found
            if(img != null)
            {
                switch(i)
                {
                    case 0: // p1
                        if (p1Image != null)
                            p1Image.texture = img;
                        break;

                    case 1: // p2
                        if (p2Image != null)
                            p2Image.texture = img;
                        break;

                    case 2: // p3
                        if (p3Image != null)
                            p3Image.texture = img;
                        break;

                    case 3: // p4
                        if (p4Image != null)
                            p4Image.texture = img;
                        break;
                }
            }
        }
        
    }

    // updates the player scores
    public void UpdateScores()
    {
        // p1
        if (p1Score != null && p1 != null)
        {
            // internal tracker doesn't match
            if (p1Points != p1.playerScore)
            {
                int temp = Mathf.RoundToInt(p1.playerScore); // convert to int
                p1Score.text = temp.ToString("d3"); // update text
                p1Points = p1.playerScore;
            }
                
        }
            
        // p2
        if (p2Score != null && p2 != null)
        {
            // internal tracker doesn't match
            if (p2Points != p2.playerScore)
            {
                int temp = Mathf.RoundToInt(p2.playerScore); // convert to int
                p2Score.text = temp.ToString("d3"); // update text
                p2Points = p2.playerScore;
            }
                
        }
            
        // p3
        if (p3Score != null && p3 != null)
        {
            // internal tracker doesn't match
            if (p3Points != p3.playerScore)
            {
                int temp = Mathf.RoundToInt(p3.playerScore); // convert to int
                p3Score.text = temp.ToString("d3"); // update text
                p3Points = p3.playerScore;
            }
                
        }
            
        // p4
        if (p4Score != null && p4 != null)
        {
            // internal tracker doesn't match
            if (p4Points != p4.playerScore)
            {
                int temp = Mathf.RoundToInt(p4.playerScore); // convert to int
                p4Score.text = temp.ToString("d3"); // update text
                p4Points = p4.playerScore;
            }
                
        }
            

    }

    // Update is called once per frame
    void Update()
    {
        // updates the time
        UpdateTime();

        // updates the scores
        UpdateScores();
    }
}
