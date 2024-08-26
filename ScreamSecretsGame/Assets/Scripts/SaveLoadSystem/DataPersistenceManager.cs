using System.Collections.Generic;
using SaveLoadSystem;
using System.Linq;
using UnityEngine;


public class DataPersistenceManager : MonoBehaviour
{
    private GameData gameData;
    private List<IDataPersistence> persistenceList;
    [SerializeField] private string fileName;
    private FileDataHandler fileDataHandler;
    public static DataPersistenceManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Only one instance of DataPersistenceManager is allowed");
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        this.fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.persistenceList = FindAllPersistenceObjects();
        LoadGame();
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        this.gameData = fileDataHandler.LoadGameData();
        
        if (this.gameData == null)
        {
            Debug.Log("No game data found. Initializing new game...");
            NewGame();
        }
        
        foreach (IDataPersistence persistence in this.persistenceList)
        {
            persistence.LoadData(gameData);
        }
        Debug.Log("Player position loaded: " + gameData.playerPosition);
    }

    public void SaveGame()
    {
        foreach (IDataPersistence persistence in this.persistenceList)
        {
            persistence.SaveData(gameData);
        }
        Debug.Log("Player position saved: " + gameData.playerPosition);
        
        // Save that data to a file using the data handler
        fileDataHandler.SaveGameData(gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersistence> FindAllPersistenceObjects()
    {
        IEnumerable<IDataPersistence> persistence = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return new List<IDataPersistence>(persistence);
    }
}

