using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    // public List<Object> keepList;


    // Start is called before the first frame update
    void Start()
    {
        // for(int i = 0; i < keepList.Count; i++)
        // {
        //     if(keepList[i] == null)
        //     {
        //         keepList.RemoveAt(i);
        //         continue;
        //     }
        // 
        //     Object.DontDestroyOnLoad(keepList[i]);
        // }
    }

    // adds object to "Do Not Destroy on Load" list.
    // public void AddObjectToDontDestroyOnLoadList(Object entity)
    // {
    //     Object.DontDestroyOnLoad(entity);
    //     keepList.Add(entity);
    // }
    
    // checks if a scene exists in the build list.
    // NOTE: this doesn't seem to work.
    public static bool SceneExists(string sceneName)
    {
        // gets scene count
        int sceneCount = SceneManager.sceneCountInBuildSettings;

        // goes through each scene.
        for(int i = 0; i < sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneByBuildIndex(i);

            // scene found.
            if(scene != null)
            {
                // scene names match
                if (sceneName == scene.name)
                    return true;
            }
            
        }

        return false;
    }

    // gets the name of the active scene
    public static string GetActiveSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }

    // gets the scene name
    public static string GetSceneName(int index)
    {
        // gets the scene.
        Scene scene = SceneManager.GetSceneByBuildIndex(index);

        // if scene was found.
        if (scene != null)
            return scene.name;
        else
            return null;
    }
   
    // changes the scene using an index.
    // TODO: make static
    public static void ChangeScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }

    // changes the scene using a string
    public static void ChangeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void BackChangeCanvas(string canvas)
    {
        var temp = GameObject.Find(canvas);
        temp.GetComponent<Canvas>().enabled = false;

    }

    public void ChangeCanvas(string canvas)
    {
        var temp = GameObject.Find(canvas);
        temp.GetComponent<Canvas>().enabled = true;
     
    }

    // switches out an active game object for an inactive one.
    public void SwitchActiveGameObject(string current, string next)
    {
        GameObject g0 = GameObject.Find(current);
        GameObject g1 = GameObject.Find(next);

        g0.SetActive(false);
        g1.SetActive(true);
    }

    // returns the skybox
    public Material GetSkybox()
    {
        return RenderSettings.skybox;
    }

    public void SetSkybox(Material newSkybox)
    {
        RenderSettings.skybox = newSkybox;
    }

    // exits the game
    public void ExitApplication()
    {
        Application.Quit();
    }

}
