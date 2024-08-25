using UnityEngine;
using System;
using System.IO;
using System.Threading;

public class FileDataHandler
{
    // path to save data
    private string dataPath = "";
    // data file name
    private string dataFileName = "";

    // Initialisation
    public FileDataHandler(string dataPath, string dataFileName)
    {
        this.dataPath = dataPath;
        this.dataFileName = dataFileName;
    }

    // Load game data from file
    public GameData LoadGameData()
    {
        string fullPath = Path.Combine(this.dataPath, this.dataFileName);
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                // Load data with retry mechanism
                string dataToLoad = LoadFileWithRetry(fullPath);
                
                // Deserialize loaded data
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error loading game data: {e.Message}");
            }
        }
        return loadedData;
    }

    // Save game data to file
    public void SaveGameData(GameData gameData)
    {
        string fullPath = Path.Combine(this.dataPath, this.dataFileName);
        Debug.Log("Saving data to " + fullPath);
        try
        {
            // create dir if not exists
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            string dataToStore = JsonUtility.ToJson(gameData, true);

            // Save data with retry mechanism
            SaveFileWithRetry(fullPath, dataToStore);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error saving game data: {e.Message}");
        }
    }

    private string LoadFileWithRetry(string path, int maxRetries = 3, int delayMs = 100)
    {
        for (int i = 0; i < maxRetries; i++)
        {
            try
            {
                return File.ReadAllText(path);
            }
            catch (IOException)
            {
                if (i == maxRetries - 1) throw;
                Thread.Sleep(delayMs);
            }
        }
        return null; // This line should never be reached
    }

    private void SaveFileWithRetry(string path, string content, int maxRetries = 3, int delayMs = 100)
    {
        for (int i = 0; i < maxRetries; i++)
        {
            try
            {
                File.WriteAllText(path, content);
                return;
            }
            catch (IOException)
            {
                if (i == maxRetries - 1) throw;
                Thread.Sleep(delayMs);
            }
        }
    }
}