using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class SaveFileManager : MonoBehaviour
{
    SaveData currentData;

    void Start()
    {
        currentData = Load();
    }

    // Checks the boolean at the index (id) in levelsUnlocked under currentData then returns it
    public bool CheckIfLevelUnlocked(int id)
    {
        return currentData.levelsUnlocked[id];
    }

    // Changes the boolean at the index (id) to true in levelsUnlocked under currentData then saves that to file
    public void UnlockLevel(int id)
    {
        currentData.levelsUnlocked[id] = true;
        SaveToFile(currentData);
    }

    // Deletes file SaveData and creates a new instance for currentData
    public void DeleteSaveData()
    {
        if (File.Exists(Application.persistentDataPath + "/save.dat"))
            File.Delete(Application.persistentDataPath + "/save.dat");

        currentData = new SaveData();
    }

    // Takes currentData and saves it to file
    private void SaveToFile(SaveData currentData)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/save.dat");

        SaveData data = currentData;

        bf.Serialize(file, data);
        file.Close();
    }

    // Loads the file SaveData then returns it
    private SaveData Load()
    {
        if (File.Exists(Application.persistentDataPath + "/save.dat"))
        {

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/save.dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();

            return data;
        }

        return new SaveData();
    }
}

[Serializable]
class SaveData
{
    public bool[] levelsUnlocked = new bool[] { false, false, false, false, false, false, false, false, false, false };
}
