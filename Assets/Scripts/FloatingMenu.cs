using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FloatingMenu : MonoBehaviour
{

    [Tooltip("The prefab for the buttons.")]
    public GameObject buttonPrefab;

    [Tooltip("The prefab for the vertical buttons in case of a vertical menu.")]
    public GameObject buttonVertPrefab;
    public int amountOfButtons = 0;
    public int amountOfVertButtons = 0;
    public int buttonSize = 5;
    public int distanceFromCamera = 40;

    private List<GameObject> buttons;
    private List<GameObject> vertButtons;
    private MenuButtonHandler buttonHandler;
    private Canvas canvas;
    private GameObject detailMenu;

    // Use this for initialization
    void Start()
    {
        buttonHandler = GetComponent<MenuButtonHandler>();
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;

        if(amountOfVertButtons > 0)
        {
            detailMenu = Instantiate(new GameObject("DetailMenu"));
            detailMenu.transform.parent = transform;
            CloseDetailMenu();
        }

        CreateButtons();
        InitializeButtons();
    }

    private void InitializeButtons()
    {
        foreach (GameObject b in buttons)
        {
            b.GetComponent<Button>().onClick.AddListener(() =>
            {
                OnClick(buttons.IndexOf(b));
            });
        }

        foreach (GameObject vb in vertButtons)
        {
            vb.GetComponent<Button>().onClick.AddListener(() =>
            {
                OnVertClick(vertButtons.IndexOf(vb));
            });
        }
        //Initialize sprites for normal and highlighted state
        //Give the paths for the sprite and the highlighted sprite here
        LoadSprites(buttons[0], "UserInterface/toggle_off", "UserInterface/toggle_on");
        LoadSprites(buttons[1], "UserInterface/toggle_off", "UserInterface/toggle_on");
        LoadSprites(buttons[2], "UserInterface/toggle_off", "UserInterface/toggle_on");
        LoadSprites(buttons[3], "UserInterface/toggle_off", "UserInterface/toggle_on");

        LoadSprites(vertButtons[0], "UserInterface/toggle_off", "UserInterface/toggle_on");
        LoadSprites(vertButtons[1], "UserInterface/toggle_off", "UserInterface/toggle_on");
        LoadSprites(vertButtons[2], "UserInterface/toggle_off", "UserInterface/toggle_on");
        LoadSprites(vertButtons[3], "UserInterface/toggle_off", "UserInterface/toggle_on");
    }

    private void CreateButtons()
    {
        buttons = new List<GameObject>();
        vertButtons = new List<GameObject>();
        float startPosX = -(amountOfButtons - 1) * buttonSize / 2;
        for (int i = 0; i < amountOfButtons; i++)
        {
            buttons.Add(Instantiate(buttonPrefab));
            buttons[i].transform.SetParent(transform);
            buttons[i].transform.position += new Vector3(startPosX + (buttonSize * i), 0, 0);
        }
        for (int i = 0; i < amountOfVertButtons; i++)
        {
            vertButtons.Add(Instantiate(buttonVertPrefab));
            vertButtons[i].transform.position = buttons[amountOfButtons - 1].transform.position;
            vertButtons[i].transform.position += new Vector3(0, buttonSize + (buttonSize * i), 0);
            vertButtons[i].transform.SetParent(detailMenu.transform);
        }
    }


    private void OnClick(int index)
    {
        buttonHandler.ButtonClicked(index);
    }

    public void OnClick(GameObject button)
    {
        int index = buttons.IndexOf(button);
        if (index == -1)
        {
            index = vertButtons.IndexOf(button);
            OnVertClick(index);
        } else
        {
            OnClick(index);
        }
    }

    private void OnVertClick(int index)
    {
        buttonHandler.ButtonVertClicked(index);
    }

    private void LoadSprites(GameObject button, string path, string highlightedPath)
    {
        button.GetComponent<Image>().sprite = Resources.Load<Sprite>(path);
        SpriteState ss = new SpriteState();
        ss.highlightedSprite = Resources.Load<Sprite>(highlightedPath);
        button.GetComponent<Button>().spriteState = ss;
    }

    public void OpenMenu()
    {
        transform.position = Camera.main.transform.position + Camera.main.transform.forward * distanceFromCamera;
        transform.rotation = Camera.main.transform.rotation;
        canvas.enabled = true;
    }

    public void CloseMenu()
    {
        canvas.enabled = false;
        CloseDetailMenu();
    }

    public bool IsOpen()
    {
        return canvas.enabled;
    }

    public void OpenDetailMenu()
    {
        detailMenu.SetActive(true);
    }

    public void CloseDetailMenu()
    {
        detailMenu.SetActive(false);
    }

    public bool IsOpenDetail()
    {
        return detailMenu.activeInHierarchy;
    }
    

    public void OnPointerEnter(GameObject button)
    {
        //button.SetActive(false);
        Debug.Log("HOVER");
        Debug.Log(button.name);
    }
}