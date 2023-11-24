using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using static Structs;

public class LevelMaker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int deleteIdAt = -1;

        FileData fileData = LevelMakerDev();

        DeleteLevelData(deleteIdAt);
        SaveLevelToFile(fileData, true);
    }

    public FileData LevelMakerDev()
    {
        FileData fileData = new FileData();

        fileData.name = "Test Level";
        fileData.id = 0;

        //[ TEST ]
        /* 
        [Level Layer]
        floor tile = 0
        wall tile = 1

        [Obstacle Layer]
        nothing = 0
        spikes = 1
        shooter = 2
        player spawn = 3

        [Properties]
        default = 0
        shooter facing direction = 0,1,2,3 (North, East, South, West)
        spikes = 0,1 (Normal Timing, Alt Timing)
        */
        fileData.intMap = new int[,,]
        {
        {
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                {1, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 1},
                {1, 0, 0, 1, 0, 0, 0, 1, 1, 0, 0, 1},
                {1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                {1, 1, 0, 1, 1, 0, 1, 1, 1, 1, 0, 1},
                {1, 1, 0, 1, 1, 0, 0, 0, 0, 1, 1, 1},
                {1, 1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 1},
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},

        },
        {
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 3, 0, 1, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        },
        {
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        }
        };
        fileData.scale = 1;

        fileData.spikesMaxLevel = 2;
        fileData.spikesStartingLevel = 1;

        fileData.shooterMaxLevel = 1;
        fileData.shooterStartingLevel = 1;

        fileData.isLavaOn = true;
        fileData.lavaMaxLevel = 1;
        fileData.lavaStartingLevel = 1;


        return fileData;
    }

    public void DeleteLevelData(int id)
    {
        if (File.Exists(Application.persistentDataPath + "/Level" + id + ".dat"))
            File.Delete(Application.persistentDataPath + "/Level" + id + ".dat");
    }

    private void SaveLevelToFile(FileData fileData, bool createFile)
    {
        if (createFile)
        {
            int id = fileData.id;

            if (!File.Exists(Application.persistentDataPath + "/Level" + id + ".dat"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Create(Application.persistentDataPath + "/Level" + id + ".dat");

                FileData data = fileData;

                bf.Serialize(file, data);
                file.Close();
            }
        }
    }
}
