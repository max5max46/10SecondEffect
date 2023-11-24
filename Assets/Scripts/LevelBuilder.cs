using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Structs;
using UnityEngine.Tilemaps;
using UnityEngine.WSA;
using UnityEditor.Experimental.GraphView;
using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.UIElements;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public class LevelBuilder : MonoBehaviour
{
    [SerializeField] private GameObject floorTile;
    [SerializeField] private GameObject wallTileNoSides;
    [SerializeField] private GameObject wallTileOneSide;
    [SerializeField] private GameObject wallTileTwoSidesCorner;
    [SerializeField] private GameObject wallTileTwoSidesStraight;
    [SerializeField] private GameObject wallTileThreeSides;
    [SerializeField] private GameObject wallTileFourSides;
    [SerializeField] private GameObject shooter;
    [SerializeField] private GameObject spikes;
    [SerializeField] private GameObject lava;

    private GameObject player;


    public LevelData LoadAndBuildLevel(int id)
    {
        player = GameObject.FindWithTag("Player");

        //Loads File Data
        FileData fileData = LoadFileData(id);

        //Converts to Level Data
        LevelData lvlData = FileDataToLevelData(fileData);

        //Builds From Level Data
        LevelData levelData = BuildLevel(lvlData);

        //Pass Back to Manager
        return levelData;
    }

    public FileData LoadFileData(int id)
    {

        if (File.Exists(UnityEngine.Application.persistentDataPath + "/Level" + id + ".dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(UnityEngine.Application.persistentDataPath + "/Level" + id + ".dat", FileMode.Open);
            FileData fileData = (FileData)bf.Deserialize(file);
            file.Close();

            return fileData;
        }
        else
        {
            Debug.LogError("No File Data Found");
        }

        return new FileData();
    }

    private LevelData FileDataToLevelData(FileData fileData)
    {
        LevelData levelData = new LevelData();

        // Fills in levelData using fileData
        levelData.name = fileData.name;
        levelData.id = fileData.id;
        levelData.tileMap = IntMapToTileMap(fileData.intMap);
        levelData.scale = fileData.scale;
        levelData.spikesMaxLevel = fileData.spikesMaxLevel;
        levelData.spikesStartingLevel = fileData.spikesStartingLevel;
        levelData.shooterMaxLevel = fileData.shooterMaxLevel;
        levelData.shooterStartingLevel = fileData.shooterStartingLevel;
        levelData.isLavaOn = fileData.isLavaOn;
        levelData.lavaMaxLevel = fileData.lavaMaxLevel;
        levelData.lavaStartingLevel = fileData.lavaStartingLevel;
        levelData.obstacleTypesInLevel = FillObstacleTypeList(levelData);

        return levelData;

        // Converts the intMap data into tileMap data
        Structs.Tile[,,] IntMapToTileMap(int[,,] intMap)
        {
            int layer = intMap.GetLength(0);
            int depth = intMap.GetLength(1);
            int width = intMap.GetLength(2);

            Structs.Tile[,,] tileMap = new Structs.Tile[layer, depth, width];

            //looks through all tiles and there surroundings to find what kind of tile they should be (ex: a one sided wall facing North)
            for (int y = 0; y < depth; y++)
                for (int x = 0; x < width; x++)
                {
                    string tileInfoString = "";
                    /*this checks all map tiles around the one we're looking and simplifies it into a string of 5 digits
                     "01101" looks like:

                        0     100
                       110 or 110 (*corners are not inculed in the string)
                        1     111

                     on the map */
                    for (int i = -1; i < 2; i++)
                        for (int j = -1; j < 2; j++)
                        {
                            //stops corners from being put in the string (removes the origin tile [i,j])
                            if (i != j && i + j != 0)
                            {
                                //checks if the tile looked at is outside the map data, if it is, default to a wall
                                if (y + i < 0 || x + j < 0 || y + i > depth - 1 || x + j > width - 1)
                                    tileInfoString += 1;
                                else
                                    tileInfoString += intMap[0, y + i, x + j];
                            }
                            //puts the origin tile [i,j] back in the string
                            if (i == 0 && j == 0)
                                tileInfoString += intMap[0, y, x];
                        }

                    // Checks the tileInfoString against all possible wall configurations to find out what asset and rotation each should be
                    if (tileInfoString[2] == '0' && intMap[1, y, x] != 1)
                        tileMap[0, y, x] = new Structs.Tile("Floor", floorTile, 0, 0);
                    else
                        switch (tileInfoString)
                        {
                            case "1" +
                                "111" +
                                 "1":
                                tileMap[0, y, x] = new Structs.Tile("WallTileNoSides", wallTileNoSides, 0, 1);
                                break;

                            case "0" +
                                "111" +
                                 "1":
                                tileMap[0, y, x] = new Structs.Tile("WallTileOneSideNorth", wallTileOneSide, 0, 1);
                                break;

                            case "1" +
                                "110" +
                                 "1":
                                tileMap[0, y, x] = new Structs.Tile("WallTileOneSideEast", wallTileOneSide, 90, 1);
                                break;

                            case "1" +
                                 "111" +
                                  "0":
                                tileMap[0, y, x] = new Structs.Tile("WallTileOneSideSouth", wallTileOneSide, 180, 1);
                                break;

                            case "1" +
                                 "011" +
                                  "1":
                                tileMap[0, y, x] = new Structs.Tile("WallTileOneSideWest", wallTileOneSide, 270, 1);
                                break;

                            case "0" +
                                 "110" +
                                  "1":
                                tileMap[0, y, x] = new Structs.Tile("WallTileTwoSidesCornerNorth", wallTileTwoSidesCorner, 0, 1);
                                break;

                            case "1" +
                                 "110" +
                                  "0":
                                tileMap[0, y, x] = new Structs.Tile("WallTileTwoSidesCornerEast", wallTileTwoSidesCorner, 90, 1);
                                break;

                            case "1" +
                                 "011" +
                                  "0":
                                tileMap[0, y, x] = new Structs.Tile("WallTileTwoSidesCornerSouth", wallTileTwoSidesCorner, 180, 1);
                                break;

                            case "0" +
                                 "011" +
                                  "1":
                                tileMap[0, y, x] = new Structs.Tile("WallTileTwoSidesCornerWest", wallTileTwoSidesCorner, 270, 1);
                                break;

                            case "0" +
                                 "111" +
                                  "0":
                                tileMap[0, y, x] = new Structs.Tile("WallTileTwoSidesStraightHorizontal", wallTileTwoSidesStraight, 0, 1);
                                break;

                            case "1" +
                                 "010" +
                                  "1":
                                tileMap[0, y, x] = new Structs.Tile("WallTileTwoSidesStraightVertical", wallTileTwoSidesStraight, 90, 1);
                                break;

                            case "0" +
                                 "010" +
                                  "1":
                                tileMap[0, y, x] = new Structs.Tile("WallTileThreeSidesNorth", wallTileThreeSides, 0, 1);
                                break;

                            case "0" +
                                 "110" +
                                  "0":
                                tileMap[0, y, x] = new Structs.Tile("WallTileThreeSidesEast", wallTileThreeSides, 90, 1);
                                break;

                            case "1" +
                                 "010" +
                                  "0":
                                tileMap[0, y, x] = new Structs.Tile("WallTileThreeSidesSouth", wallTileThreeSides, 180, 1);
                                break;

                            case "0" +
                                 "011" +
                                  "0":
                                tileMap[0, y, x] = new Structs.Tile("WallTileThreeSidesWest", wallTileThreeSides, 270, 1);
                                break;

                            case "0" +
                                 "010" +
                                  "0":
                                tileMap[0, y, x] = new Structs.Tile("WallTileFourSides", wallTileFourSides, 0, 1);
                                break;

                        }
                }

            // Checks the obstacle and properties layer and adds said obstacles and there properties to the tile map
            for (int y = 0; y < depth; y++)
                for (int x = 0; x < width; x++)
                    switch (intMap[1, y, x])
                    {
                        // Adds spikes to tile map
                        case 1:
                            if (intMap[0, y, x] != 1)
                            {
                                tileMap[1, y, x] = new Structs.Tile(Global.SPIKES_NAME, spikes, 0, 0);
                                // Checks the properties layer to see if the spikes are using alt timing
                                if (intMap[2, y, x] == 1)
                                    tileMap[1, y, x].spikeAltTiming = true;
                            }
                            break;

                        // Adds shooters to tile map
                        case 2:
                            if (intMap[0, y, x] != 1)
                            {
                                tileMap[1, y, x] = new Structs.Tile(Global.SHOOTER_NAME, shooter, 0, 1);
                                // Checks the properties layer to see which direction the shooter being added should point
                                switch (intMap[2, y, x])
                                {
                                    case 1:
                                        tileMap[1, y, x].yRotation = 90;
                                        break;
                                    case 2:
                                        tileMap[1, y, x].yRotation = 180;
                                        break;
                                    case 3:
                                        tileMap[1, y, x].yRotation = 270;
                                        break;
                                }
                            }
                            break;

                        // Adds the player spawn to the tile map
                        case 3:
                            if (intMap[0, y, x] != 1)
                                tileMap[1, y, x] = new Structs.Tile("PlayerSpawn", null, 0, 1);
                            else
                                Debug.LogError("Player spawn in invaild location");
                            break;

                    }

            return tileMap;
        }

        // Fills the Obstacle List to be used later
        List<string> FillObstacleTypeList(LevelData levelData)
        {
            int depth = levelData.tileMap.GetLength(1);
            int width = levelData.tileMap.GetLength(2);

            List<string> obstacleTypesInLevel = new List<string>();

            // Populates a list (obstacleTypesInLevel) with the names of each obstacle type currently in the level data
            for (int y = 0; y < depth; y++)
                for (int x = 0; x < width; x++)
                    // Skips all obstacles (tiles on the obstacle layer) that are not assigned
                    if (levelData.tileMap[1, y, x].name != null)
                    {
                        // Checks obstacle names and adds said name if it is defined in the switch statment and has yet to be added to the list (obstacleTypesInLevel), then adds that name to the list (obstacleTypesInLevel)
                        Debug.Log(levelData.tileMap[1, y, x].name);
                        switch (levelData.tileMap[1, y, x].name)
                        {
                            case var _ when levelData.tileMap[1, y, x].name == Global.SPIKES_NAME && !obstacleTypesInLevel.Contains(Global.SPIKES_NAME):
                                obstacleTypesInLevel.Add(Global.SPIKES_NAME);
                                break;

                            case var _ when levelData.tileMap[1, y, x].name == Global.SHOOTER_NAME && !obstacleTypesInLevel.Contains(Global.SHOOTER_NAME):
                                obstacleTypesInLevel.Add(Global.SHOOTER_NAME);
                                break;
                        }
                    }

            if (levelData.isLavaOn)
                obstacleTypesInLevel.Add(Global.LAVA_NAME);

            return obstacleTypesInLevel;
        }
    }

    private LevelData BuildLevel(LevelData lvlData)
    {
        LevelData levelData = lvlData;

        int depth = levelData.tileMap.GetLength(1);
        int width = levelData.tileMap.GetLength(2);

        // Adds a offset to everything depending on whether or not there is a even amount of tiles in either the x or y in tileMap to keep things centered
        float yEvenOffset = 0;
        float xEvenOffset = 0;
        if (depth % 2 == 0) yEvenOffset = levelData.scale / 2;
        if (width % 2 == 0) xEvenOffset = levelData.scale / 2;

        // Adds all game objects to scene using tileMap (k = layer, i = y, j = x)
        for (int layer = 0; layer < 2; layer++)
            for (int y = 0; y < depth; y++)
                for (int x = 0; x < width; x++)
                {
                    //Unpacking some variables
                    Structs.Tile tile = levelData.tileMap[layer, y, x];
                    float xPos = ((x - (width / 2)) * levelData.scale) + xEvenOffset;
                    float yPos = tile.height * levelData.scale;
                    float zPos = ((-y + (depth / 2)) * levelData.scale) - yEvenOffset;
                    Vector3 tilePosition = new Vector3(xPos, yPos, zPos);

                    // Checks if the tile attempting to be instantiated has its asset (prefab) defined
                    if (tile.asset != null)
                    {
                        // Makes a instantance of an object based off the asset in tile, then sets the new object to the gameObject variable in the same tile
                        GameObject gObject = Instantiate(tile.asset, tilePosition, Quaternion.Euler(new Vector3(0, tile.yRotation, 0)));
                        tile.gameObject = gObject;

                        // All gameObjects change scale accordingly
                        tile.gameObject.transform.localScale = new Vector3(levelData.scale, levelData.scale, levelData.scale);

                        // All obstacles are given their unique properties
                        switch (tile.name)
                        {
                            case Global.SPIKES_NAME:
                                tile.gameObject.GetComponent<Obstacle>().SetStartingLevel(levelData.spikesStartingLevel);
                                tile.gameObject.GetComponent<Obstacle>().SetMaxLevel(levelData.spikesMaxLevel);
                                if (tile.spikeAltTiming)
                                    tile.gameObject.GetComponent<Spikes>().altTiming = true;
                                break;
                            case Global.SHOOTER_NAME:
                                tile.gameObject.GetComponent<Obstacle>().SetStartingLevel(levelData.shooterStartingLevel);
                                tile.gameObject.GetComponent<Obstacle>().SetMaxLevel(levelData.shooterMaxLevel);
                                break;
                        }

                        //Gives tile back to the tile map
                        levelData.tileMap[layer, y, x] = tile;
                    }
                    else
                    {
                        // Sets Player spawn at the player spawn tile in the tile map
                        if (tile.name == "PlayerSpawn")
                            player.transform.position = new Vector3(tilePosition.x, (tilePosition.y / 2) + 0.5f, tilePosition.z);
                    }
                }

        // Add all game objects not on the tile map
        if (levelData.isLavaOn)
        {
            // Makes a instantance of the prefab lava and give it to levelData
            GameObject lavaGO = Instantiate(lava, Vector3.zero, Quaternion.Euler(Vector3.zero));
            levelData.lavaGameObject = lavaGO;

            // Lava is given its unique properties and certain levelData values
            levelData.lavaGameObject.GetComponent<Obstacle>().SetStartingLevel(levelData.lavaStartingLevel);
            levelData.lavaGameObject.GetComponent<Obstacle>().SetMaxLevel(levelData.lavaMaxLevel);
            levelData.lavaGameObject.GetComponent<Lava>().scale = levelData.scale;
            levelData.lavaGameObject.GetComponent<Lava>().mapWidth = width;
            levelData.lavaGameObject.GetComponent<Lava>().mapDepth = depth;
        }

        return levelData;
    }
}
