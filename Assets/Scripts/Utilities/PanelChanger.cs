using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// changes out panels
public class PanelChanger : MonoBehaviour
{
    // panel objects
    public List<GameObject> panels = new List<GameObject>();

    // the current panel being viewed.
    public int currentPanel = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // switches to new panel
    public void SwitchPanel(int newPanel)
    {
        // checks to see if a valid panel was provided.
        if(newPanel >= 0 && newPanel < panels.Count)
        {
            panels[currentPanel].SetActive(false);
            currentPanel = newPanel;
            panels[currentPanel].SetActive(true);
        }
    }

    // switches to new panel via name
    public void SwitchPanel(string newPanel)
    {
        // finds panel string
        for (int i = 0; i < panels.Count; i++)
        {
            // if the panel has been found
            if(panels[i].name == newPanel)
            {
                panels[currentPanel].SetActive(false);
                currentPanel = i;
                panels[currentPanel].SetActive(true);

                return;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
