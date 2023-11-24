using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structs : MonoBehaviour
{
    public struct Tile
    {
        public string name;
        public GameObject asset;
        public float yRotation;
        public int height;
        public bool spikeAltTiming;
        public GameObject gameObject;
        public Tile(string name = "Undefined", GameObject asset = null, float yRotation = 0, int height = 0)
        {
            this.name = name;
            this.asset = asset;
            this.yRotation = yRotation;
            this.height = height;
            this.spikeAltTiming = false;
            gameObject = null;
        }
    }

    [Serializable]
    public struct FileData
    {
        public string name;
        public int id;
        public int[,,] intMap;
        public float scale;

        public int spikesStartingLevel;
        public int spikesMaxLevel;

        public int shooterStartingLevel;
        public int shooterMaxLevel;

        public bool isLavaOn;
        public int lavaStartingLevel;
        public int lavaMaxLevel;

    }

    public struct LevelData
    {
        public string name;
        public int id;
        public Tile[,,] tileMap;
        public List<string> obstacleTypesInLevel;
        public float scale;

        public int spikesStartingLevel;
        public int spikesMaxLevel;

        public int shooterStartingLevel;
        public int shooterMaxLevel;

        public bool isLavaOn;
        public int lavaStartingLevel;
        public int lavaMaxLevel;
        public GameObject lavaGameObject;

        public LevelData(GameObject lavaGameObject) : this()
        {
            this.lavaGameObject = lavaGameObject;
        }
    }
}
