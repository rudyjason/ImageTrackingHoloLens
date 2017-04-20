using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;
using UnityEngine.VR.WSA.Input;
using Vuforia;
using UnityEngine.VR.WSA.Persistence;
using UnityEngine.VR.WSA;

public class PickerScript : MonoBehaviour
{
    struct Item_Location
    {
        public int item;
        public int location;

        public Item_Location(int v1, int v2) : this()
        {
            item = v1;
            location = v2;
        }
    }

    private const string ANCHOR_ID = "247IndicAnchor";

    private WorldAnchorStore store;
    private WorldAnchor anchorStart;
    private GameObject anchoredNode;
    public GameObject anchorPrefab;
    public GameObject all;

    public GameObject hololensCamera;
    public GameObject inputManager;
    public GameObject environment;
    public GameObject userPosition;

    public FloatingMenu menu;

    public GameObject cursor;
    public GameObject directionPointer;
    //private CustomDirectionIndicator directionIndicator;

    [Tooltip("Model to tap to reset the picklist.")]
    public GameObject restartObject;

    private GameObject focusedObject;
    private GazeManager gazeManager;
    private GestureRecognizer gestureRecognizer;
    private float environmentStartRotation;
    private Vector3 environmentStartPosition;

    [Tooltip("GameObjects of all the stands available.")]
    public List<GameObject> stands;

    [Tooltip("Pickable items.")]
    public List<GameObject> targets;

    [Tooltip("Stack Locations.")]
    public List<GameObject> locations;

    private List<Item_Location> pickList;
    private int itemIndex;
    private int standIndex;
    private bool stellingFound;
    private bool firstTap;


    // Use this for initialization
    void Start()
    {
        Debug.Log("DEBUG - STARTING PICKERSCRIPT");

        //Setting up Gaze
        gazeManager = inputManager.GetComponent<GazeManager>();
        gazeManager.enabled = true;

        initGestures();
        initValues();

        // Load anchor store
        //WorldAnchorStore.GetAsync(StoreLoaded);

        stands[standIndex].SetActive(false);
        restartObject.SetActive(false);

        directionPointer = GameObject.FindGameObjectWithTag("pointer");
    }

    // Update is called once per frame
    void Update()
    {


    }


    void FixedUpdate()
    {
        if (!stellingFound && Vector3.Distance(stands[standIndex].transform.position, hololensCamera.transform.position) < 1.5f)
        {
            Debug.Log("CLOSE TO STAND");
            //foreach (GameObject target in targets)
            //{
            //    target.SetActive(false);
            //}
            directionPointer.SetActive(false);
            stands[standIndex].SetActive(false);
            stellingFound = true;
            targets[pickList[itemIndex].item].SetActive(true);
            //directionIndicator.enabled = false;
        }

        if(Vector3.Distance(targets[pickList[itemIndex].item].transform.position, hololensCamera.transform.position) < 0)
        {
            locations[pickList[itemIndex].location].SetActive(true);
        } else if (Vector3.Distance(targets[pickList[itemIndex].item].transform.position, locations[pickList[itemIndex].location].transform.position) < 0)
        {
            targets[pickList[itemIndex].item].SetActive(false);
            locations[pickList[itemIndex].location].SetActive(false);
            itemIndex++;
            targets[pickList[itemIndex].item].SetActive(true);
        }

        //If looking at something, set it to focusedObject, otherwise null-
        if (gazeManager.IsGazingAtObject)
        {
            if (focusedObject != gazeManager.HitObject)
                focusedObject = gazeManager.HitObject;
        }
        else
        {
            focusedObject = null;
        }
    }


    void NextItem()
    {

        Debug.Log("DEBUG - currentIndex:" + itemIndex);
        itemIndex++;
        if (itemIndex < pickList.Count)
        {
            //Go to next item on picklist
            GameObject currentTarget = targets[pickList[itemIndex].item];
            Debug.Log("DEBUG - SWITCHING ACTIVE TARGET TO:" + currentTarget);
            targets[pickList[itemIndex - 1].item].SetActive(false);
            currentTarget.SetActive(true);
        }
        else
        {
            standIndex++;
            targets[pickList[itemIndex - 1].item].SetActive(false);
            if (standIndex < stands.Count)
            {
                itemIndex = 0;
                stellingFound = false;
                stands[standIndex].SetActive(true);
                /*directionIndicator = stands[standIndex].EnsureComponent<CustomDirectionIndicator>();
                directionIndicator.DirectionIndicatorObject = directionPointer;
                directionIndicator.Cursor = cursor;
                Debug.Log("direction indicator: " + directionIndicator.name);
                directionIndicator.enabled = true;*/
            }
            else
            {
                //Display message that you are done
                standIndex--;
                restartObject.SetActive(true);
                Debug.Log("DEBUG - YOU ARE DONE");
            }
        }
    }

    private void HandleTap(GameObject tappedObject)
    {
        if (tappedObject == null)
        {
            if (!menu.IsOpen())
                menu.OpenMenu();
            else
                menu.CloseMenu();
            return;
        }

        Debug.Log("tapped on: " + tappedObject.name + " - current object to pick: " + targets[pickList[itemIndex].item].transform.GetChild(0).name + " - equal: " + (tappedObject == targets[pickList[itemIndex].item].transform.GetChild(0)));

        if (tappedObject == restartObject)
        {
            Start();
        }
        else if (tappedObject.name == targets[pickList[itemIndex].item].transform.GetChild(0).name)
        {
            NextItem();
        }
        else
        {
            if (!menu.IsOpen())
                menu.OpenMenu();
            else 
                menu.OnClick(tappedObject);
        }
    }

    private void initGestures()
    {
        //Create Gesture Recognizer
        gestureRecognizer = new UnityEngine.VR.WSA.Input.GestureRecognizer();

        //Tap Event
        gestureRecognizer.TappedEvent += (source, tapCount, ray) =>
        {
            HandleTap(focusedObject);
        };

        //Hold Event
        gestureRecognizer.HoldCompletedEvent += (source, ray) =>
        {
            makeAnchor(transform.position + ray.direction * 5);
            //ResetPosition();
        };

        //Start capturing gestures
        gestureRecognizer.StartCapturingGestures();
    }

    private void initValues()
    {
        itemIndex = 0;
        standIndex = 0;
        stellingFound = false;
        firstTap = true;
        pickList = new List<Item_Location>();
        environmentStartRotation = environment.transform.eulerAngles.y;
        environmentStartPosition = environment.transform.position;

        //TEST - creating mockup picklist
        pickList.Add(new Item_Location(0, 0));
        pickList.Add(new Item_Location(1, 0));
        pickList.Add(new Item_Location(0, 1));
        pickList.Add(new Item_Location(1, 0));
        pickList.Add(new Item_Location(1, 1));
        pickList.Add(new Item_Location(1, 1));
        //pickList.Add(2);
        //pickList.Add(3);
    }

    private void makeAnchor(Vector3 pos)
    {
        // Make anchor
        if(anchoredNode == null)
            anchoredNode = Instantiate(anchorPrefab);
        anchoredNode.transform.position = pos;
        Debug.Log("position: " + pos);
        anchorStart = anchoredNode.GetComponent<WorldAnchor>();
        if(anchorStart == null)
        {
            anchorStart = anchoredNode.AddComponent<WorldAnchor>();
        }
        all.transform.SetParent(anchoredNode.transform);
        all.transform.position = Vector3.zero;
        all.transform.rotation = Quaternion.Euler(Vector3.zero);

        // Save
        store.Save(ANCHOR_ID, anchorStart);
    }

    private void StoreLoaded(WorldAnchorStore store)
    {
        this.store = store;

        // Get the anchor
        anchorStart = store.Load(ANCHOR_ID, anchoredNode);

        if (anchorStart != null)
        {
            all.transform.position = anchorStart.transform.position;
            firstTap = false;
            stands[standIndex].SetActive(true);
            RemoveGeneratedImageTargets();
        }
    }

    private void ResetPosition()
    {
        environment.transform.position = environmentStartPosition + new Vector3(hololensCamera.transform.position.x, 0, hololensCamera.transform.position.z);
        Debug.Log("environment: " + environment.transform.position + " - camera: " + hololensCamera.transform.position);
        environment.transform.localEulerAngles = new Vector3(0, environmentStartRotation + hololensCamera.transform.eulerAngles.y, 0);

        directionPointer.SetActive(true);
        //Debug.Log("FIRST TAP ACTIVATED, CAMERA POSITION: " + position + ", USER POSITION: " + userPosition.transform.position + ", CAMERA ROTATION OVER Y: " + hololensCamera.transform.eulerAngles.y);
    }

    private void RemoveGeneratedImageTargets()
    {
        // Get the Vuforia StateManager
        StateManager sm = TrackerManager.Instance.GetStateManager();

        // Query the StateManager to retrieve the list of
        // currently 'active' trackables 
        //(i.e. the ones currently being tracked by Vuforia)
        IEnumerable<TrackableBehaviour> activeTrackables = sm.GetTrackableBehaviours();

        // Iterate through the list of active trackables
        Debug.Log("List of trackables currently active (tracked): ");
        foreach (TrackableBehaviour tb in activeTrackables)
        {
            Debug.Log("Trackable: " + tb + " - Gameobject" + tb.gameObject);
            if (tb.gameObject != null && !tb.tag.Equals("trackable"))
            {
                //tb.enabled = false;
                tb.gameObject.SetActive(false);
            }
        }

        DisableTracking();
    }

    public void DisableTracking()
    {
        foreach (GameObject target in targets)
        {
            target.SetActive(false);
        }
    }

    public void EnableTracking()
    {
        targets[pickList[itemIndex].item].SetActive(true);
    }

    public void TestTracking()
    {
        foreach (GameObject target in targets)
        {
            target.SetActive(true);
        }
    }
}
