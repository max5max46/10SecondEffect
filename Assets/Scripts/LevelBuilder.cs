using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{

    public GameObject player;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private GameObject floorTile;
    [SerializeField] private GameObject wallTileNoSides;
    [SerializeField] private GameObject wallTileOneSide;
    [SerializeField] private GameObject wallTileTwoSidesCorner;
    [SerializeField] private GameObject wallTileTwoSidesStraight;
    [SerializeField] private GameObject wallTileThreeSides;
    [SerializeField] private GameObject wallTileFourSides;
    [SerializeField] private GameObject shooter;
    [SerializeField] private float scale = 1;


    private struct Tile
    {
        public string name;
        public GameObject asset;
        public float yRotation;
        public int height;
        public GameObject gameObject;
        public Tile(string name, GameObject asset, float yRotation = 0, int height = 0)
        {
            this.name = name;
            this.asset = asset;
            this.yRotation = yRotation;
            this.height = height;
            gameObject = null;
        }
    }

    private GameObject[,] floorTiles;
    private GameObject[,] wallTiles;
    private int[,,] loadedMap;
    private Tile[,,] tileMap;


    // Start is called before the first frame update
    void Start()
    {
        /* 
            [Level Layer]
            floor tile = 0
            wall tile = 1

            [Obstacle Layer]
            nothing = 0
            shooter = 2

            [Properties]
            default = 0
            shooter facing direction = 0,1,2,3 (North, East, South, West)
        */
        loadedMap = new int[,,]
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
                { 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        },
        {
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        }
        };

        IntMapToTileMap();

        float xEvenOffset = 0;
        float yEvenOffset = 0;

        Debug.Log("layer Amount:" + tileMap.GetLength(0) + " Y:" + tileMap.GetLength(1) + " X:" + tileMap.GetLength(2));

        player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + (scale / 2) + (transform.localScale.y/2), player.transform.position.z);

        if (loadedMap.GetLength(2) % 2 == 0) xEvenOffset = scale / 2;
        if (loadedMap.GetLength(1) % 2 == 0) yEvenOffset = scale / 2;


        // adds all game objects to scene using tileMap (k = layer, i = y, j = x)
        for (int k = 0; k < 2; k++)
            for (int i = 0; i < tileMap.GetLength(1); i++)
                for (int j = 0; j < tileMap.GetLength(2); j++)
                    //checks if the tile attempting to be instantiated is defined
                    if (k == 0 || loadedMap[k, i, j] != 0)
                    {
                        //makes a instantance of an object based off the asset in tile, then sets the new object to the gameObject variable in the same tile
                        GameObject gObject = Instantiate(tileMap[k, i, j].asset, new Vector3(((-(tileMap.GetLength(1) / 2) + j) * scale) + xEvenOffset, tileMap[k, i, j].height * scale, (((tileMap.GetLength(0) / 2) - i) * scale) - yEvenOffset), Quaternion.Euler(new Vector3(0, tileMap[k, i, j].yRotation, 0)));
                        tileMap[k, i, j].gameObject = gObject;

                        //all gameObjects change scale accordingly
                        tileMap[k, i, j].gameObject.transform.localScale = new Vector3(scale, scale, scale);

                        //Subscribes all obstacles to the event One Second Has Passed
                        if (k == 1)
                            levelManager.SubscribeToOneSecondHasPassed(tileMap[k, i, j].gameObject.GetComponent<Obstacle>());
                    }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void IntMapToTileMap()
    {
        tileMap = new Tile[loadedMap.GetLength(0), loadedMap.GetLength(1), loadedMap.GetLength(2)];

        //looks through all tiles and there surroundings to find what kind of tile they should be (ex: a one sided wall facing North)
        for (int i = 0; i < loadedMap.GetLength(1); i++)
            for (int j = 0; j < loadedMap.GetLength(2); j++)
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
                            if (i + k < 0 || j + l < 0 || i + k > loadedMap.GetLength(1) - 1 || j + l > loadedMap.GetLength(2) - 1)
                                tileInfoString += 1;
                            else
                                tileInfoString += loadedMap[0, i+k, j+l];
                        }
                        //puts the origin tile [i,j] back in the string
                        if (k == 0 && l == 0)
                            tileInfoString += loadedMap[0, i, j];
                    }

                //Debug.Log(tileInfoString);

                if (tileInfoString[2] == '0')
                    tileMap[0, i, j] = new Tile("Floor", floorTile, 0, 0);
                else
                    switch (tileInfoString)
                    {
                        case  "1" +
                             "111" +
                              "1":
                            tileMap[0, i, j] = new Tile("WallTileNoSides", wallTileNoSides, 0, 1);
                            break;

                        case  "0" +
                             "111" +
                              "1":
                            tileMap[0, i, j] = new Tile("WallTileOneSideNorth", wallTileOneSide, 0, 1);
                            break;

                        case  "1" +
                             "110" +
                              "1":
                            tileMap[0, i, j] = new Tile("WallTileOneSideEast", wallTileOneSide, 90, 1);
                            break;

                        case  "1" +
                             "111" +
                              "0":
                            tileMap[0, i, j] = new Tile("WallTileOneSideSouth", wallTileOneSide, 180, 1);
                            break;

                        case  "1" +
                             "011" +
                              "1":
                            tileMap[0, i, j] = new Tile("WallTileOneSideWest", wallTileOneSide, 270, 1);
                            break;

                        case  "0" +
                             "110" +
                              "1":
                            tileMap[0, i, j] = new Tile("WallTileTwoSidesCornerNorth", wallTileTwoSidesCorner, 0, 1);
                            break;

                        case  "1" +
                             "110" +
                              "0":
                            tileMap[0, i, j] = new Tile("WallTileTwoSidesCornerEast", wallTileTwoSidesCorner, 90, 1);
                            break;

                        case  "1" +
                             "011" +
                              "0":
                            tileMap[0, i, j] = new Tile("WallTileTwoSidesCornerSouth", wallTileTwoSidesCorner, 180, 1);
                            break;

                        case  "0" +
                             "011" +
                              "1":
                            tileMap[0, i, j] = new Tile("WallTileTwoSidesCornerWest", wallTileTwoSidesCorner, 270, 1);
                            break;

                        case  "0" +
                             "111" +
                              "0":
                            tileMap[0, i, j] = new Tile("WallTileTwoSidesStraightHorizontal", wallTileTwoSidesStraight, 0, 1);
                            break;

                        case  "1" +
                             "010" +
                              "1":
                            tileMap[0, i, j] = new Tile("WallTileTwoSidesStraightVertical", wallTileTwoSidesStraight, 90, 1);
                            break;

                        case  "0" +
                             "010" +
                              "1":
                            tileMap[0, i, j] = new Tile("WallTileThreeSidesNorth", wallTileThreeSides, 0, 1);
                            break;

                        case  "0" +
                             "110" +
                              "0":
                            tileMap[0, i, j] = new Tile("WallTileThreeSidesEast", wallTileThreeSides, 90, 1);
                            break;

                        case  "1" +
                             "010" +
                              "0":
                            tileMap[0, i, j] = new Tile("WallTileThreeSidesSouth", wallTileThreeSides, 180, 1);
                            break;

                        case  "0" +
                             "011" +
                              "0":
                            tileMap[0, i, j] = new Tile("WallTileThreeSidesWest", wallTileThreeSides, 270, 1);
                            break;

                        case  "0" +
                             "010" +
                              "0":
                            tileMap[0, i, j] = new Tile("WallTileFourSides", wallTileFourSides, 0, 1);
                            break;

                    }
            }

        for (int i = 0; i < loadedMap.GetLength(1); i++)
            for (int j = 0; j < loadedMap.GetLength(2); j++)
                    switch (loadedMap[1, i, j])
                    {
                        case 2:
                            if (loadedMap[0, i, j] != 1)
                                switch(loadedMap[2, i, j])
                                {
                                    case 0:
                                        tileMap[1, i, j] = new Tile("ShooterNorth", shooter, 0, 1);
                                        break;
                                    case 1:
                                        tileMap[1, i, j] = new Tile("ShooterEast", shooter, 90, 1);
                                        break;
                                    case 2:
                                        tileMap[1, i, j] = new Tile("ShooterSouth", shooter, 180, 1);
                                        break;
                                    case 3:
                                        tileMap[1, i, j] = new Tile("ShooterWest", shooter, 270, 1);
                                        break;
                                }
                        break;
                    }
    }
}
