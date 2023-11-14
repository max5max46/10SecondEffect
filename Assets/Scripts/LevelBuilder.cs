using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Structs;
using UnityEngine.Tilemaps;

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

    public LoadedLevelData TestDataFill()
    {
        LoadedLevelData fileData = new LoadedLevelData();

        fileData.name = "Test Level";

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

        fileData.spikesMaxLevel = 3;
        fileData.spikesStartingLevel = 1;

        fileData.shooterMaxLevel = 5;
        fileData.shooterStartingLevel = 1;

        fileData.isLavaOn = false;
        fileData.lavaMaxLevel = 1;
        fileData.lavaStartingLevel = 1;


        return fileData;
    }

    public LevelData LoadAndBuildLevel()
    {
        player = GameObject.FindWithTag("Player");

        LevelData levelData = FileDataToLevelData();

        float xEvenOffset = 0;
        float yEvenOffset = 0;

        player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + (levelData.scale / 2) + (transform.localScale.y / 2), player.transform.position.z);

        if (levelData.tileMap.GetLength(2) % 2 == 0) xEvenOffset = levelData.scale / 2;
        if (levelData.tileMap.GetLength(1) % 2 == 0) yEvenOffset = levelData.scale / 2;


        // Adds all game objects to scene using tileMap (k = layer, i = y, j = x)
        for (int k = 0; k < 2; k++)
            for (int i = 0; i < levelData.tileMap.GetLength(1); i++)
                for (int j = 0; j < levelData.tileMap.GetLength(2); j++)
                    // Checks if the tile attempting to be instantiated is defined
                    if (levelData.tileMap[k, i, j].asset != null)
                    {
                        // Makes a instantance of an object based off the asset in tile, then sets the new object to the gameObject variable in the same tile
                        GameObject gObject = Instantiate(levelData.tileMap[k, i, j].asset, new Vector3(((-(levelData.tileMap.GetLength(1) / 2) + j) * levelData.scale) + xEvenOffset, levelData.tileMap[k, i, j].height * levelData.scale, (((levelData.tileMap.GetLength(0) / 2) - i) * levelData.scale) - yEvenOffset), Quaternion.Euler(new Vector3(0, levelData.tileMap[k, i, j].yRotation, 0)));
                        levelData.tileMap[k, i, j].gameObject = gObject;

                        // All gameObjects change scale accordingly
                        levelData.tileMap[k, i, j].gameObject.transform.localScale = new Vector3(levelData.scale, levelData.scale, levelData.scale);

                        // All obstacle
                        switch (levelData.tileMap[k, i, j].name)
                        {
                            case Global.SPIKES_NAME:
                                levelData.tileMap[k, i, j].gameObject.GetComponent<Obstacle>().SetStartingLevel(levelData.spikesStartingLevel);
                                levelData.tileMap[k, i, j].gameObject.GetComponent<Obstacle>().SetMaxLevel(levelData.spikesMaxLevel);
                                if (levelData.tileMap[k, i, j].spikeAltTiming)
                                    levelData.tileMap[k, i, j].gameObject.GetComponent<Spikes>().altTiming = true;
                                break;
                            case Global.SHOOTER_NAME:
                                levelData.tileMap[k, i, j].gameObject.GetComponent<Obstacle>().SetStartingLevel(levelData.shooterStartingLevel);
                                levelData.tileMap[k, i, j].gameObject.GetComponent<Obstacle>().SetMaxLevel(levelData.shooterMaxLevel);
                                break;
                        }
                    }
                    else
                    {
                        // Sets Player spawn at the player spawn tile in the tile map
                        if (levelData.tileMap[k, i, j].name == "PlayerSpawn")
                            player.transform.position = new Vector3(((-(levelData.tileMap.GetLength(1) / 2) + j) * levelData.scale) + xEvenOffset, levelData.tileMap[k, i, j].height * levelData.scale, (((levelData.tileMap.GetLength(0) / 2) - i) * levelData.scale) - yEvenOffset);
                    }

        //Add all game objects not on the tile map
        if (levelData.isLavaOn)
        {

        }


        return levelData;
    }

    private LevelData FileDataToLevelData()
    {
        LevelData levelData = new LevelData();
        LoadedLevelData fileData = TestDataFill();

        levelData.name = fileData.name;
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
    }

    private Structs.Tile[,,] IntMapToTileMap(int[,,] intMap)
    {
        Structs.Tile[,,] tileMap = new Structs.Tile[intMap.GetLength(0), intMap.GetLength(1), intMap.GetLength(2)];

        //looks through all tiles and there surroundings to find what kind of tile they should be (ex: a one sided wall facing North)
        for (int i = 0; i < intMap.GetLength(1); i++)
            for (int j = 0; j < intMap.GetLength(2); j++)
            {
                string tileInfoString = "";
                /*this checks all map tiles around the one we're looking and simplifies it into a string of 5 digits
                 "01101" looks like:

                    0     100
                   110 or 110 (*corners are not inculed in the string)
                    1     111
                
                 on the map */
                for (int k = -1; k < 2; k++)
                    for (int l = -1; l < 2; l++)
                    {
                        //stops corners from being put in the string (removes the origin tile [i,j])
                        if (k != l && k + l != 0)
                        {
                            //checks if the tile looked at is outside the map data, if it is, default to a wall
                            if (i + k < 0 || j + l < 0 || i + k > intMap.GetLength(1) - 1 || j + l > intMap.GetLength(2) - 1)
                                tileInfoString += 1;
                            else
                                tileInfoString += intMap[0, i + k, j + l];
                        }
                        //puts the origin tile [i,j] back in the string
                        if (k == 0 && l == 0)
                            tileInfoString += intMap[0, i, j];
                    }

                // Checks the tileInfoString against all possible wall configurations to find out what asset and rotation each should be
                if (tileInfoString[2] == '0' && intMap[1, i, j] != 1)
                    tileMap[0, i, j] = new Structs.Tile("Floor", floorTile, 0, 0);
                else
                    switch (tileInfoString)
                    {
                        case "1" +
                            "111" +
                             "1":
                            tileMap[0, i, j] = new Structs.Tile("WallTileNoSides", wallTileNoSides, 0, 1);
                            break;

                        case "0" +
                            "111" +
                             "1":
                            tileMap[0, i, j] = new Structs.Tile("WallTileOneSideNorth", wallTileOneSide, 0, 1);
                            break;

                        case "1" +
                            "110" +
                             "1":
                            tileMap[0, i, j] = new Structs.Tile("WallTileOneSideEast", wallTileOneSide, 90, 1);
                            break;

                        case  "1" +
                             "111" +
                              "0":
                            tileMap[0, i, j] = new Structs.Tile("WallTileOneSideSouth", wallTileOneSide, 180, 1);
                            break;

                        case  "1" +
                             "011" +
                              "1":
                            tileMap[0, i, j] = new Structs.Tile("WallTileOneSideWest", wallTileOneSide, 270, 1);
                            break;

                        case  "0" +
                             "110" +
                              "1":
                            tileMap[0, i, j] = new Structs.Tile("WallTileTwoSidesCornerNorth", wallTileTwoSidesCorner, 0, 1);
                            break;

                        case  "1" +
                             "110" +
                              "0":
                            tileMap[0, i, j] = new Structs.Tile("WallTileTwoSidesCornerEast", wallTileTwoSidesCorner, 90, 1);
                            break;

                        case  "1" +
                             "011" +
                              "0":
                            tileMap[0, i, j] = new Structs.Tile("WallTileTwoSidesCornerSouth", wallTileTwoSidesCorner, 180, 1);
                            break;

                        case  "0" +
                             "011" +
                              "1":
                            tileMap[0, i, j] = new Structs.Tile("WallTileTwoSidesCornerWest", wallTileTwoSidesCorner, 270, 1);
                            break;

                        case  "0" +
                             "111" +
                              "0":
                            tileMap[0, i, j] = new Structs.Tile("WallTileTwoSidesStraightHorizontal", wallTileTwoSidesStraight, 0, 1);
                            break;

                        case  "1" +
                             "010" +
                              "1":
                            tileMap[0, i, j] = new Structs.Tile("WallTileTwoSidesStraightVertical", wallTileTwoSidesStraight, 90, 1);
                            break;

                        case  "0" +
                             "010" +
                              "1":
                            tileMap[0, i, j] = new Structs.Tile("WallTileThreeSidesNorth", wallTileThreeSides, 0, 1);
                            break;

                        case  "0" +
                             "110" +
                              "0":
                            tileMap[0, i, j] = new Structs.Tile("WallTileThreeSidesEast", wallTileThreeSides, 90, 1);
                            break;

                        case  "1" +
                             "010" +
                              "0":
                            tileMap[0, i, j] = new Structs.Tile("WallTileThreeSidesSouth", wallTileThreeSides, 180, 1);
                            break;

                        case  "0" +
                             "011" +
                              "0":
                            tileMap[0, i, j] = new Structs.Tile("WallTileThreeSidesWest", wallTileThreeSides, 270, 1);
                            break;

                        case  "0" +
                             "010" +
                              "0":
                            tileMap[0, i, j] = new Structs.Tile("WallTileFourSides", wallTileFourSides, 0, 1);
                            break;

                    }
            }

        // Checks the obstacle and properties layer and adds said obstacles and there properties to the tile map
        for (int i = 0; i < intMap.GetLength(1); i++)
            for (int j = 0; j < intMap.GetLength(2); j++)
                switch (intMap[1, i, j])
                {
                    // Adds spikes to tile map
                    case 1:
                        if (intMap[0, i, j] != 1)
                        {
                            tileMap[1, i, j] = new Structs.Tile(Global.SPIKES_NAME, spikes, 0, 0);
                            // Checks the properties layer to see if the spikes are using alt timing
                            if (intMap[2, i, j] == 1)
                                tileMap[1, i, j].spikeAltTiming = true;
                        }
                        break;

                    // Adds shooters to tile map
                    case 2:
                        if (intMap[0, i, j] != 1)
                        {
                            tileMap[1, i, j] = new Structs.Tile(Global.SHOOTER_NAME, shooter, 0, 1);
                            // Checks the properties layer to see which direction the shooter being added should point
                            switch (intMap[2, i, j])
                            {
                                case 1:
                                    tileMap[1, i, j].yRotation = 90;
                                    break;
                                case 2:
                                    tileMap[1, i, j].yRotation = 180;
                                    break;
                                case 3:
                                    tileMap[1, i, j].yRotation = 270;
                                    break;
                            }
                        }
                        break;

                    // Adds the player spawn to the tile map
                    case 3:
                        if (intMap[0, i, j] != 1)
                            tileMap[1, i, j] = new Structs.Tile("PlayerSpawn", null, 0, 1);
                        else
                            Debug.LogError("Player spawn in invaild location");
                        break;

                }

        return tileMap;
    }

    private List<string> FillObstacleTypeList(LevelData levelData)
    {
        
        List<string> obstacleTypesInLevel = new List<string>();
        // Populates a list (obstacleTypesInLevel) with the names of each obstacle type currently in the level data
        for (int i = 0; i < levelData.tileMap.GetLength(1); i++)
            for (int j = 0; j < levelData.tileMap.GetLength(2); j++)
                // Skips all obstacles (tiles on the obstacle layer) that are not assigned
                if (levelData.tileMap[1, i, j].name != null)
                {
                    // Checks obstacle names and adds said name if it is defined in the switch statment and has yet to be added to the list (obstacleTypesInLevel), then adds that name to the list (obstacleTypesInLevel)
                    Debug.Log(levelData.tileMap[1, i, j].name);
                    switch (levelData.tileMap[1, i, j].name)
                    {
                        case var _ when levelData.tileMap[1, i, j].name == Global.SPIKES_NAME && !obstacleTypesInLevel.Contains(Global.SPIKES_NAME):
                            obstacleTypesInLevel.Add(Global.SPIKES_NAME);
                            break;

                        case var _ when levelData.tileMap[1, i, j].name == Global.SHOOTER_NAME && !obstacleTypesInLevel.Contains(Global.SHOOTER_NAME):
                            obstacleTypesInLevel.Add(Global.SHOOTER_NAME);
                            break;
                    }
                }

        if (levelData.isLavaOn)
            obstacleTypesInLevel.Add(Global.LAVA_NAME);

        return obstacleTypesInLevel;
    }
}
