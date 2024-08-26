using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager menumanagerinstance;
    [System.Serializable]
    public class MenuElement
    {
        public string name;
        public GameObject menuObject;
    }

    [SerializeField] private MenuElement mainMenuObject;
    [SerializeField] private GameObject rootMenuObject;
    [SerializeField] private List<MenuElement> menuElements = new List<MenuElement>();
    private Dictionary<string, GameObject> menuDictionary = new Dictionary<string, GameObject>();

    private void Awake()
    {
        // initialise menus
        InitializeMenuElements();
    }

    private void Start()
    {
        if (menumanagerinstance != null)
        {
            Destroy(gameObject);
            Debug.LogWarning("Cannot have more than one MenuManager in the scene!");
        }

        if (SceneManager.GetActiveScene().name == "MainMenu" && !mainMenuObject.menuObject.activeSelf)
        {
            ShowMainMenu();
        }
        menumanagerinstance = this;
    }

    public void InitializeMenuElements()
    {
        foreach (MenuElement menuElement in menuElements)
        {
            if (menuElement.menuObject != null)
            {
                menuDictionary[menuElement.name] = menuElement.menuObject;
                menuElement.menuObject.SetActive(false);
            }
            else
            {
                Debug.Log(menuElement.name + "has no attached gameobject");
            }
        }
        DontDestroyOnLoad(rootMenuObject);
    }

    public void ShowMenu(string menuName)
    {
        if (menuDictionary.TryGetValue(menuName, out GameObject menuObject))
        {
            menuObject.SetActive(true);
        }
        else
        {
            Debug.Log(menuName + "has no attached gameobject or is missing");
        }
    }

    public void HideMenu(string menuName)
    {
        if (menuDictionary.TryGetValue(menuName, out GameObject menuObject))
        {
            menuObject.SetActive(false);
        }
        else
        {
            Debug.Log(menuName + "has no attached gameobject or is missing");
        }
    }

    public void ToggleMenu(string menuName)
    {
        if (menuDictionary.TryGetValue(menuName, out GameObject menuObject))
        {
            menuObject.SetActive(!menuObject.activeSelf);
        }
        else
        {
            Debug.Log(menuName + "has no attached gameobject or is missing");
        }
    }

    public void HideAllMenus()
    {
        foreach(var menuObject in menuDictionary.Values)
        {
            menuObject.SetActive(false);
        }
    }

    public void ShowMainMenu()
    {
        mainMenuObject.menuObject.SetActive(true);
    }

    public void HideMainMenu()
    {
        mainMenuObject.menuObject.SetActive(false);
    }
}