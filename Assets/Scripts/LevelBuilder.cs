using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{


    [SerializeField] private GameObject floorTile;
    [SerializeField] private GameObject wallTileNoSides;
    [SerializeField] private GameObject wallTileOneSide;
    [SerializeField] private GameObject wallTileTwoSidesCorner;
    [SerializeField] private GameObject wallTileTwoSidesStraight;
    [SerializeField] private GameObject wallTileThreeSides;
    [SerializeField] private GameObject wallTileFourSides;
    [SerializeField] private float scale = 1;


    private struct Tile
    {
        public string name;
        public GameObject asset;
        public float yRotation;
        public int height;
        public Tile(string name, GameObject gameObject, float yRotation = 0, int height = 0)
        {
            this.name = name;
            this.asset = gameObject;
            this.yRotation = yRotation;
            this.height = height;
        }
    }

    private GameObject[,] floorTiles;
    private GameObject[,] wallTiles;
    private int[,] loadedMap;
    private Tile[,] tileMap;


    // Start is called before the first frame update
    void Start()
    {
        loadedMap = new int[,]            
        {
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                {1, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 1},
                {1, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 1},
                {1, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 1},
                {1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1},
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        };



        floorTile.transform.localScale = new Vector3(scale, scale, scale);
        wallTileNoSides.transform.localScale = new Vector3(scale, scale, scale);
        wallTileOneSide.transform.localScale = new Vector3(scale, scale, scale);
        wallTileTwoSidesCorner.transform.localScale = new Vector3(scale, scale, scale);
        wallTileTwoSidesStraight.transform.localScale = new Vector3(scale, scale, scale);
        wallTileThreeSides.transform.localScale = new Vector3(scale, scale, scale);
        wallTileFourSides.transform.localScale = new Vector3(scale, scale, scale);

        IntMapToTileMap();

        float xEvenOffset = 0;
        float yEvenOffset = 0;

        if (loadedMap.GetLength(1) % 2 == 0) xEvenOffset = scale / 2;
        if (loadedMap.GetLength(0) % 2 == 0) yEvenOffset = scale / 2;

        // adds all game objects to scene using tileMap
        for (int i = 0; i < tileMap.GetLength(0); i++)
            for (int j = 0; j < tileMap.GetLength(1); j++)
                Instantiate(tileMap[i, j].asset, new Vector3(((-(tileMap.GetLength(1) / 2) + j) * scale) + xEvenOffset, tileMap[i, j].height * scale, (((tileMap.GetLength(0) / 2) - i) * scale) - yEvenOffset), Quaternion.Euler(new Vector3(0, tileMap[i, j].yRotation, 0)));

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void IntMapToTileMap()
    {
        tileMap = new Tile[loadedMap.GetLength(0), loadedMap.GetLength(1)];

        //looks through all tiles and there surroundings to find what kind of tile they should be (ex: a one sided wall facing North)
        for (int i = 0; i < loadedMap.GetLength(0); i++)
            for (int j = 0; j < loadedMap.GetLength(1); j++)
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
                            //checks if the tile looked at is outside the map data, if it is default to a wall
                            if (i + k < 0 || j + l < 0 || i + k > loadedMap.GetLength(0) - 1 || j + l > loadedMap.GetLength(1) - 1)
                                tileInfoString += 1;
                            else
                                tileInfoString += loadedMap[i+k, j+l];
                        }
                        //puts the origin tile [i,j] back in the string
                        if (k == 0 && l == 0)
                            tileInfoString += loadedMap[i, j];
                    }

                //Debug.Log(tileInfoString);

                if (tileInfoString[2] == '0')
                    tileMap[i, j] = new Tile("Floor", floorTile, 0, 0);
                else
                    switch (tileInfoString)
                    {
                        case  "1" +
                             "111" +
                              "1":
                            tileMap[i, j] = new Tile("WallTileNoSides", wallTileNoSides, 0, 1);
                            break;

                        case  "0" +
                             "111" +
                              "1":
                            tileMap[i, j] = new Tile("WallTileOneSideNorth", wallTileOneSide, 0, 1);
                            break;

                        case  "1" +
                             "110" +
                              "1":
                            tileMap[i, j] = new Tile("WallTileOneSideEast", wallTileOneSide, 90, 1);
                            break;

                        case  "1" +
                             "111" +
                              "0":
                            tileMap[i, j] = new Tile("WallTileOneSideSouth", wallTileOneSide, 180, 1);
                            break;

                        case  "1" +
                             "011" +
                              "1":
                            tileMap[i, j] = new Tile("WallTileOneSideWest", wallTileOneSide, 270, 1);
                            break;

                        case  "0" +
                             "110" +
                              "1":
                            tileMap[i, j] = new Tile("WallTileTwoSidesCornerNorth", wallTileTwoSidesCorner, 0, 1);
                            break;

                        case  "1" +
                             "110" +
                              "0":
                            tileMap[i, j] = new Tile("WallTileTwoSidesCornerEast", wallTileTwoSidesCorner, 90, 1);
                            break;

                        case  "1" +
                             "011" +
                              "0":
                            tileMap[i, j] = new Tile("WallTileTwoSidesCornerSouth", wallTileTwoSidesCorner, 180, 1);
                            break;

                        case  "0" +
                             "011" +
                              "1":
                            tileMap[i, j] = new Tile("WallTileTwoSidesCornerWest", wallTileTwoSidesCorner, 270, 1);
                            break;

                        case  "0" +
                             "111" +
                              "0":
                            tileMap[i, j] = new Tile("WallTileTwoSidesStraightHorizontal", wallTileTwoSidesStraight, 0, 1);
                            break;

                        case  "1" +
                             "010" +
                              "1":
                            tileMap[i, j] = new Tile("WallTileTwoSidesStraightVertical", wallTileTwoSidesStraight, 90, 1);
                            break;

                        case  "0" +
                             "010" +
                              "1":
                            tileMap[i, j] = new Tile("WallTileThreeSidesNorth", wallTileThreeSides, 0, 1);
                            break;

                        case  "0" +
                             "110" +
                              "0":
                            tileMap[i, j] = new Tile("WallTileThreeSidesEast", wallTileThreeSides, 90, 1);
                            break;

                        case  "1" +
                             "010" +
                              "0":
                            tileMap[i, j] = new Tile("WallTileThreeSidesSouth", wallTileThreeSides, 180, 1);
                            break;

                        case  "0" +
                             "011" +
                              "0":
                            tileMap[i, j] = new Tile("WallTileThreeSidesWest", wallTileThreeSides, 270, 1);
                            break;

                        case  "0" +
                             "010" +
                              "0":
                            tileMap[i, j] = new Tile("WallTileFourSides", wallTileFourSides, 0, 1);
                            break;

                    }
            }
    }
}
