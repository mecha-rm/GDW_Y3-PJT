/*
 * References:
    * https://answers.unity.com/questions/1087029/how-to-change-camera-on-trigger-enter-unity-5.html
    * https://answers.unity.com/questions/1171111/hideunhide-game-object.html
    * https://docs.unity3d.com/ScriptReference/Component-gameObject.html
    * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GED
{
    public class UI_Manager : MonoBehaviour
    {
        // 3D objects used for instaniation
        public GameObject cube;
        public GameObject sphere;
        public GameObject capsule;
        public GameObject cylinder;
        public GameObject plane;
        public GameObject quad;
        public GameObject terrain;
        public GameObject tree;

        // 2D objects used for instaniation
        public GameObject sprite;
        public GameObject spriteMask;
        // public GameObject tilemap;
        public GameObject grid;

        // custom
        public GameObject spawner;

        // lights
        public GameObject direcLight;
        public GameObject pointLight;
        public GameObject spotLight;
        public GameObject areaLight;
        public GameObject reflecProbe;
        public GameObject lightProbe;

        // used to hide or show the UI
        public GameObject ui01;
        public GameObject instructionPanel;

        // main camera and secondary camera
        // the original plan was to allow for the switching of displays, but instead we're using turning on and off the UI.
        // private bool swapCam = false; // unused
        public Camera cam1;
        public Camera cam2;

        // sets whether the recently instantiated object is a player or not.
        private bool addPlayerScript = false;
        // adds a rigid body to the entity, which allows for gravity to take effect.
        private bool addRigidBody = false;

        // the currently selected object
        private GameObject selectedObject;

        // the file that will be read from or written to.
        public string fileName = "";

        // list of entities to be saved to and loaded from files
        // this feature was not completed.
        private List<GameObject> fileObjectList = new List<GameObject>();

        // Start is called before the first frame update
        void Start()
        {

        }

        // mouse has clicked on the UI
        private void OnMouseDown()
        {
            selectedObject = null;
        }

        // SPAWNERS //
        // 3D OBJECTS //
        // Spawns a cube
        public void SpawnCube()
        {
            // instantiates a new object at the world origin
            GameObject newObject = Instantiate(cube, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 1));

            // adds the spawn components.
            AddSpawnComponents(newObject);
        }

        // Spawns a sphere
        public void SpawnSphere()
        {
            // instatiates a new object at the world origin
            GameObject newObject = Instantiate(sphere, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 1));

            // adds the spawn components.
            AddSpawnComponents(newObject);

            // newObject.GetComponent<SphereCollider>().isTrigger = true;
        }

        // Spawns a capsule
        public void SpawnCapsule()
        {
            // instantiates a new object at the world origin
            GameObject newObject = Instantiate(capsule, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 1));

            // adds the spawn components.
            AddSpawnComponents(newObject);
        }

        // Spawns a cylinder
        public void SpawnCylinder()
        {
            // instantiates a new object at the world origin
            GameObject newObject = Instantiate(cylinder, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 1));

            // adds the spawn components.
            AddSpawnComponents(newObject);
        }

        // Spawns a cylinder
        public void SpawnPlane()
        {
            // instantiates a new object at the world origin
            GameObject newObject = Instantiate(plane, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 1));

            // adds the spawn components.
            AddSpawnComponents(newObject);
        }

        // Spawns a quad
        public void SpawnQuad()
        {
            // instantiates a new object at the world origin
            GameObject newObject = Instantiate(quad, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 1));

            // adds the spawn components.
            AddSpawnComponents(newObject);
        }

        // Spawns a terrain object
        public void SpawnTerrain()
        {
            // instantiates a new object at the world origin
            GameObject newObject = Instantiate(terrain, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 1));

            // adds the spawn components.
            AddSpawnComponents(newObject);
        }

        // Spawns a tree
        public void SpawnTree()
        {
            // instantiates a new object at the world origin
            GameObject newObject = Instantiate(tree, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 1));

            // adds the spawn components.
            AddSpawnComponents(newObject);
        }

        // 2D OBJECTS //
        // spawns a sprite
        public void SpawnSprite()
        {
            // instantiates a new object at the world origin
            GameObject newObject = Instantiate(sprite, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 1));

            // adds the spawn components.
            AddSpawnComponents(newObject);
        }

        // spawns a sprite mask
        public void SpawnSpriteMask()
        {
            // instantiates a new object at the world origin
            GameObject newObject = Instantiate(spriteMask, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 1));

            // adds the spawn components.
            AddSpawnComponents(newObject);
        }

        // spawns a tilemap. Change the settings of the grid to get the other tilemap types.
        //public void SpawnTilemap()
        //{
        //    // instantiates a new object at the world origin
        //    GameObject newObject = Instantiate(tilemap, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 1));

        //    // adds the spawn components.
        //    AddSpawnComponents(newObject);
        //}

        // spawns a grid, which  point top tilemap
        public void SpawnGrid()
        {
            // instantiates a new object at the world origin
            GameObject newObject = Instantiate(grid, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 1));

            // adds the spawn components.
            AddSpawnComponents(newObject);
        }

        // CUSTOM
        // spawns a custom spawner
        public void SpawnSpawner()
        {
            // instantiates a new object at the world origin
            GameObject newObject = Instantiate(spawner, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 1));

            // adds the spawn components.
            // AddSpawnComponents(newObject);
        }

        // LIGHTS //
        // Spawn directional light
        public void SpawnDirectionalLight()
        {
            // instantiates a new cube at the world origin
            GameObject newLight = Instantiate(direcLight, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 1));

            // adds the spawn components.
            AddSpawnComponents(newLight);
        }

        // Spawn point light
        public void SpawnPointLight()
        {
            // instantiates a new cube at the world origin
            GameObject newLight = Instantiate(pointLight, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 1));

            // adds the spawn components.
            AddSpawnComponents(newLight);
        }

        // Spawn spotlight
        public void SpawnSpotlight()
        {
            // instantiates a new cube at the world origin
            GameObject newLight = Instantiate(spotLight, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 1));

            // adds the spawn components.
            AddSpawnComponents(newLight);
        }

        // Spawn area light
        public void SpawnAreaLight()
        {
            // instantiates a new cube at the world origin
            GameObject newLight = Instantiate(areaLight, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 1));

            // adds the spawn components.
            AddSpawnComponents(newLight);
        }

        // Spawn reflection probe
        public void SpawnReflectionProbe()
        {
            // instantiates a new cube at the world origin
            GameObject newLight = Instantiate(reflecProbe, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 1));

            // adds the spawn components.
            AddSpawnComponents(newLight);
        }

        // Spawn light probe group
        public void SpawnLightProbeGroup()
        {
            // instantiates a new cube at the world origin
            GameObject newLight = Instantiate(lightProbe, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 1));

            // adds the spawn components.
            AddSpawnComponents(newLight);
        }

        // adds components to spawned objects
        private void AddSpawnComponents(GameObject newObject)
        {
            // gives the new object the user interface manager so that they can be selected.
            newObject.GetComponent<ObjectScript>().uiManager = gameObject;

            // adds a player script
            if (addPlayerScript)
                newObject.AddComponent<PlayerController>();

            // adds a rigid body
            if (addRigidBody)
                newObject.AddComponent<Rigidbody>();

            newObject.GetComponent<ObjectScript>().camera = cam1;

            // adds the object to the file list.
            fileObjectList.Add(newObject);
        }

        // sets the selected object.
        public void SetSelectedObject(GameObject entity)
        {
            // TODO: set up option to move object via buttons.
            selectedObject = entity;
            gameObject.GetComponent<StateMachine>().SetState(1);
        }

        // gets the selected object
        public GameObject GetSelectedObject()
        {
            return selectedObject;
        }

        // TOGGLES //
        // it is a player
        public void IsPlayer()
        {
            addPlayerScript = !addPlayerScript;
        }

        // it has a rigid body
        public void HasRigidBody()
        {
            addRigidBody = !addRigidBody;
        }

        // OTHER
        // adds an objecto the list of items to be saved.
        public void AddObjectToSaveList(GameObject entity)
        {
            // the list contains the files
            if(fileObjectList.Contains(entity))
                fileObjectList.Add(entity);
        }

        // removes object from list of items to be saved
        public void RemoveObjectFromSaveList(GameObject entity)
        {
            fileObjectList.Remove(entity);
        }

        // saves the data from the file
        public void SaveDataToFile()
        {
            // this was not completed in time for the submission.
            Debug.Log("Data saving system was not completed.");
        }

        // loads the data from the file
        public void LoadDataFromFile()
        {
            // this was not completed in time for the submission.
            Debug.Log("Data loading system was not completed.");
        }

        // Update is called once per frame
        void Update()
        {
            // sets ui to be active
            if (Input.GetKeyDown(KeyCode.U))
                ui01.SetActive(!ui01.active);

            // shows and hides instruction panel
            if (Input.GetKeyDown(KeyCode.I))
                instructionPanel.SetActive(!instructionPanel.active);

            // switches the camera (unused)
            // if(Input.GetKeyDown(KeyCode.C))
            // {
            // 
            //     // TODO: this isn't very intuitive. This is just to see if I can get it to work.
            //     swapCam = !swapCam;
            //     switch(swapCam)
            //     {
            //         case true:
            //             // cam1.enabled = false;
            //             //cam1.targetDisplay = 2;
            // 
            //             // cam2.enabled = true;
            //             //cam2.targetDisplay = 1;
            //             int temp1 = cam1.targetDisplay;
            //             int temp2 = cam2.targetDisplay;
            // 
            //             Display.displays[cam2.targetDisplay].Activate();
            //             // gameObject.SetActive(false);
            //             break;
            //         case false:
            //             // cam2.enabled = false;
            //             //cam2.targetDisplay = 2;
            // 
            //             // cam1.enabled = true;
            //             //cam1.targetDisplay = 1;
            // 
            //             // gameObject.SetActive(true);
            //             Display.displays[cam1.targetDisplay].Activate();
            //             break;
            //     }
            // }
        }
    }
}