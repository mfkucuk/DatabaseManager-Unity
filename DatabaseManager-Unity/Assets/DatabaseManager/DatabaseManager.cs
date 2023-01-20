using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DatabaseManager : SingletonnPersistent<DatabaseManager>
{
    private GameData _gameData;
    private List<IDataPersistence> _dataPersistenceObjects;

    private void Start()
    {
        _dataPersistenceObjects = FindAllDataPersistenceObjects();
    }

    /**
     * <summary>
     * This function initializes the game data which calls the
     * constructor of GameData class.
     */
    public void NewGame()
    {
        _gameData = new GameData();
    }

    public void SaveGame()
    {
        // Save data from all classes which implement data persistence interface.
        foreach (IDataPersistence dataPersistenceObject in _dataPersistenceObjects)
        {
            dataPersistenceObject.SaveData(ref _gameData);
        }

        // Convert game data into JSON and save it in a file.
    }

    /**
     * <summary>
     * This function fetches the data from the JSON file via JsonHandler
     * and converts it into data usable by Unity. If there is no data found,
     * it initialized the game by calling NewGame function.
     */
    public void LoadGame()
    {
        // Check if the game is initialized.
        if (_gameData == null)
        {
            NewGame();
        }

        // Load data from all classes which implement data persistence interface.
        foreach (IDataPersistence dataPersistenceObject in _dataPersistenceObjects)
        {
            dataPersistenceObject.LoadData(_gameData);
        }
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
            .OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}
