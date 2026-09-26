using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;

    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;

    public static DataPersistenceManager instance { get; private set; }

    private bool isLoading;
    private bool isSaving;

    // Other scripts can listen to these
    public event Action SaveStarted;
    public event Action SaveFinished;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Data PersistenceManager in the scene!");
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void Start()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        dataPersistenceObjects = FindAllDataPersistenceObjects();

        LoadGame();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveGame();
        }
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    public void NewGame()
    {
        if (gameData == null)
        {
            gameData = new GameData();
        }
        else
        {
            gameData.ResetToNewGame();
        }

        //Game
        if(VillageNewGame.instance != null) VillageNewGame.instance.DeletePrevious();
        if(VillageNewGame.instance != null) VillageNewGame.instance.InitializeNewGame();

        //Main Menu
        if(MenuNewGame.instance != null) MenuNewGame.instance.InitializeNewGame();

        SaveGame();
    }

    public void LoadGame()
    {
        gameData = dataHandler.Load();

        if (gameData == null)
        {
            Debug.Log("Initializing data to defaults");
            NewGame();
            return;
        }

        isLoading = true;

        try
        {
            foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
            {
                dataPersistenceObj.LoadData(gameData);
            }
        }
        finally
        {
            isLoading = false;
        }
    }

    public void SaveGame()
    {
        if (gameData == null || isLoading || isSaving) return;

        isSaving = true;
        SaveStarted?.Invoke();

        try
        {
            foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
            {
                dataPersistenceObj.SaveData(gameData);
            }

            dataHandler.Save(gameData);
        }
        finally
        {
            isSaving = false;
            SaveFinished?.Invoke();
        }
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>(true).OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}