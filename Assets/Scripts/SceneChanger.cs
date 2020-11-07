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


    // changes the scene using an index.
    public void ChangeScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }

    // changes the scene using a string
    public void ChangeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
