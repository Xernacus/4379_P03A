using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private static SaveManager _instance;
    public static SaveManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<SaveManager>();
                if (_instance == null)
                {
                    GameObject newGO = new GameObject();
                    _instance = newGO.AddComponent<SaveManager>();
                    newGO.name = "DataManager";
                    DontDestroyOnLoad(newGO);
                }
            }
            return _instance;
        }
    }

    public SaveData ActiveSaveData { get; private set; } = new SaveData();

    public void Save()
    {
        SaveSystem.SaveToFile(ActiveSaveData);
    }

    public void Load()
    {
        ActiveSaveData = SaveSystem.LoadFromFile();
    }

    public void ResetSave()
    {
        ActiveSaveData = SaveSystem.CreateNewSaveFile();
    }
}
