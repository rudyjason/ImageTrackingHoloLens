using HoloToolkit.Unity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DestinationIndication : MonoBehaviour
{
    // struct for combining items to pick and pick locations into one easy to use format
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

    public GameObject cursor;
    public GameObject directionIndicator;

    [Tooltip("Pickable items.")]
    public List<GameObject> targets;

    [Tooltip("Stack Locations.")]
    public List<GameObject> locations;

    public HUD hud;
    public GameObject ui;

    private List<Item_Location> pickList;

    private int itemIndex;
    private AudioSource beep;
    private AudioSource boop;

    private float distance;
    private float distance2;
    public int step { get; set; }
    private float waiter;
    private bool testing;

    private TextToSpeechManager speech;
    private int hudTextCounter = 0;
    private int calibrationCount = 0;

    private bool calibrating;
    public bool stuckLocations = false;

    private const float scanDistance = 0.3f;
    private const float putDistance = 0.25f;
    private const float testScanTimer = 0.25f;

    // Use this for initialization
    void Start()
    {
        beep = GameObject.Find("Beep").GetComponent<AudioSource>();
        boop = GameObject.Find("Boop").GetComponent<AudioSource>();
        speech = ui.GetComponent<TextToSpeechManager>();

        calibrating = true;
        AudioListener.volume = 1.0f;

        hud.SetDelegate(AfterHUDText);

        Debug.Log("GOING TO STEP 1");
    }

    internal void ChangeLocationType(bool stuck)
    {
        stuckLocations = stuck;
    }

    // Update is called once per frame
    void Update()
    {
        if (calibrating)
        {
            hud.ShowText("Calibrating...\nPlease scan the QR codes of " + (stuckLocations ? "the destinations" : "your cart"), false);
            Calibrate();
            return;
        }

        if (pickList != null)
        {
            distance = Vector3.Distance(targets[pickList[itemIndex].item].transform.position, Camera.main.transform.position);
            distance2 = Vector3.Distance(targets[pickList[itemIndex].item].transform.position, locations[pickList[itemIndex].location].transform.position);
        }

        waiter += Time.deltaTime;
        if (waiter > testScanTimer && !testing)
        {
            waiter = 0;
            // Wait until item is scanned to show the destination
            if (step == 1 && distance < scanDistance && distance != 0)
            {
                if (stuckLocations)
                {
                    hud.ShowText("Bring the picked item to the designated destination.", false);
                    locations[pickList[itemIndex].location].SetActive(true);
                    locations[pickList[itemIndex].location].GetComponent<CustomDirectionIndicator>().enabled = true;
                }
                else
                {
                    hud.ShowText("Please look at your cart for directions.", false);
                    locations[pickList[itemIndex].location].SetActive(true);
                    locations[pickList[itemIndex].location].transform.position = new Vector3(0, 1000, 0);
                }
                step = 2;
                beep.Play();
            }
            // Wait until the item is at the destination to show the next location/item
            else if (step == 2 && distance2 < putDistance && distance2 != 0)
            {
                locations[pickList[itemIndex].location].GetComponent<CustomDirectionIndicator>().enabled = false;
                step = 1;
                targets[pickList[itemIndex].item].SetActive(false);
                targets[pickList[itemIndex].item].transform.position = new Vector3(0, 1000, 0);
                locations[pickList[itemIndex].location].SetActive(false);
                itemIndex++;
                if (itemIndex >= pickList.Count)
                    itemIndex = 0;
                targets[pickList[itemIndex].item].SetActive(true);
                hud.SetShowTime(1);
                hud.ShowText("Item placed correctly", true);
                SpeakText("Scan the next item");
                boop.Play();
            }
        }
    }

    private void Calibrate()
    {
        foreach (var location in locations)
        {
            if (location.activeInHierarchy && (location.transform.localPosition.x != 0 || location.transform.localPosition.y != 0 || location.transform.localPosition.z != 0))
            {
                calibrationCount++;
                location.SetActive(false);
                beep.Play();
            }
        }
        if (calibrationCount == locations.Count)
        {
            calibrating = false;
            SpeakText("Calibration complete");
            hud.SetDelegate(AfterHUDText);
            hud.ShowText("Calibration complete", true);
            RestartPicking();
        }
    }

    private void AfterHUDText()
    {
        hud.ShowText("Scan the next item: " + targets[pickList[itemIndex].item].name, false);
    }

    private void InitTargets()
    {
        foreach (GameObject target in targets)
        {
            target.SetActive(false);
        }
        foreach (GameObject location in locations)
        {
            var dirInd = location.AddComponent<CustomDirectionIndicator>();
            dirInd.Cursor = cursor;
            dirInd.DirectionIndicatorObject = directionIndicator;
            dirInd.MetersFromCursor = 0.25f;
            dirInd.enabled = false;
            //var rotation = location.transform.FindChild("target").transform.rotation;
            //rotation.x = stuckLocations ? 0 : 90;
            //location.transform.FindChild("target").transform.rotation = rotation;
            location.SetActive(calibrating);
        }
    }

    public void RestartPicking()
    {
        InitTargets();
        InitPickList();

        testing = false;
        waiter = 0;
        step = 1;
    }

    public void TestTracking(bool set)
    {
        testing = set;
        foreach (GameObject target in targets)
        {
            target.SetActive(set);
        }
        foreach (GameObject location in locations)
        {
            location.SetActive(set);
        }
    }

    public void SpeakText(string text)
    {
        speech.SpeakText(text);
    }

    // TODO: replace with actual picklist
    private void InitPickList()
    {
        pickList = new List<Item_Location>();
        itemIndex = 0;

        //TEST - creating mockup picklist
        pickList.Add(new Item_Location(0, 0));
        pickList.Add(new Item_Location(1, 1));
        pickList.Add(new Item_Location(0, 1));
        pickList.Add(new Item_Location(1, 0));
        pickList.Add(new Item_Location(0, 0));
        pickList.Add(new Item_Location(1, 1));
        pickList.Add(new Item_Location(0, 1));
        pickList.Add(new Item_Location(1, 0));

        targets[pickList[itemIndex].item].SetActive(true);
    }
}
