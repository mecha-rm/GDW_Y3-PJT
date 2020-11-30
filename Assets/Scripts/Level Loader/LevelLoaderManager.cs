using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// the level laod manager
public class LevelLoaderManager : MonoBehaviour
{
    public LevelLoader loader = null;
    public List<string> levels = new List<string>();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // loads the level
    public void LoadLevel(int index)
    {
        // index is invalid.
        if (index < 0 || index >= levels.Count)
            return;

        // loader is not set.
        if (loader == null)
            return;

        loader.file = levels[index];
        loader.LoadFromFile();
    }

    // loads the level
    public void LoadLevel(string file)
    {
        // loader is not set.
        if (loader == null)
            return;

        // goes through content.
        for(int i = 0; i < levels.Count; i++)
        {
            // loads the content.
            if(levels[i] == file)
            {
                loader.file = levels[i];
                loader.LoadFromFile();
                return;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
