using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Factory Design Pattern - Spawner
public class SpawnerScript : MonoBehaviour
{
    public List<GameObject> entities = new List<GameObject>();

    // dropdown menu of options on the list
    public Dropdown options;

    // position input
    public InputField xPosInput;
    public InputField yPosInput;
    public InputField zPosInput;

    // rotation input
    public InputField xRotInput;
    public InputField yRotInput;
    public InputField zRotInput;

    // scale input
    public InputField xScaInput;
    public InputField yScaInput;
    public InputField zScaInput;



    // Start is called before the first frame update
    void Start()
    {
        xPosInput.text = yPosInput.text = zPosInput.text = "0";
        xRotInput.text = yRotInput.text = zRotInput.text = "0";
        xScaInput.text = yScaInput.text = zScaInput.text = "1";
    }

    // sets active of spawner
    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    // toggles the 'active' parameter on/off
    public void SetActive()
    {
        gameObject.SetActive(!gameObject.active);
    }

    // spawns an object tied to the current option
    public void Spawn()
    {
        // index number of option
        if(entities.Count != 0 && options.value <= entities.Count)
        {
            GameObject newObject = Instantiate(entities[options.value]);

            // sets position, rotation, and scale values.
            // position
            newObject.transform.position = new Vector3(
                (xPosInput.text != "") ? float.Parse(xPosInput.text) : 0.0F,
                (yPosInput.text != "") ? float.Parse(yPosInput.text) : 0.0F,
                (zPosInput.text != "") ? float.Parse(zPosInput.text) : 0.0F
            );

            // rotation
            newObject.transform.rotation = new Quaternion(
                (xRotInput.text != "") ? float.Parse(xRotInput.text) : 0.0F,
                (yRotInput.text != "") ? float.Parse(yRotInput.text) : 0.0F,
                (zRotInput.text != "") ? float.Parse(zRotInput.text) : 0.0F,
                1.0F
            );

            // scale
            newObject.transform.localScale = new Vector3(
                (xScaInput.text != "") ? float.Parse(xScaInput.text) : 0.0F,
                (yScaInput.text != "") ? float.Parse(yScaInput.text) : 0.0F,
                (zScaInput.text != "") ? float.Parse(zScaInput.text) : 0.0F
            );

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
