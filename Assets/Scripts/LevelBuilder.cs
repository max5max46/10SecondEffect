using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{


    [SerializeField] private GameObject floorTile;
    [SerializeField] private GameObject wallTile;
    [SerializeField] private float scale = 1;

    private enum TileType
    {
        Floor,
        WallN,
        WallS,
        WallE,
        WallW,
        WallCornerNE,
        WallCornerNW,
        WallCornerSE,
        WallCornerSW,
        WallDeadEndN,
        WallDeadEndS,
        WallDeadEndE,
        WallDeadEndW,
        WallPillar
    }

    private GameObject[,] floorTiles;
    private GameObject[,] wallTiles;
    private int[,] loadedMap;
    private TileType[,] map;


    // Start is called before the first frame update
    void Start()
    {
        loadedMap = new int[,]            
        {
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                {1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        };





        floorTile.transform.localScale = new Vector3(scale, scale, scale);
        wallTile.transform.localScale = new Vector3(scale, scale, scale);



        float xEvenOffset = 0;
        float yEvenOffset = 0;

        if (loadedMap.GetLength(1) % 2 == 0) xEvenOffset = scale / 2;
        if (loadedMap.GetLength(0) % 2 == 0) yEvenOffset = scale / 2;


        floorTiles = new GameObject[loadedMap.GetLength(0), loadedMap.GetLength(1)];
        wallTiles = new GameObject[loadedMap.GetLength(0), loadedMap.GetLength(1)];

        for (int i = 0; i < loadedMap.GetLength(0); i++)
            for (int j = 0; j < loadedMap.GetLength(1); j++)
                floorTiles[i, j] = floorTile;

        for (int i = 0; i < loadedMap.GetLength(0); i++)
            for (int j = 0; j < loadedMap.GetLength(1); j++)
                if (loadedMap[i, j] == 1)
                    wallTiles[i, j] = wallTile;

        for (int i = 0; i < floorTiles.GetLength(0); i++)
            for (int j = 0; j < floorTiles.GetLength(1); j++)
                Instantiate(floorTiles[i, j], new Vector3(((-(floorTiles.GetLength(1) / 2) + j) * scale) + xEvenOffset, 0, (((floorTiles.GetLength(0) / 2) - i) * scale) - yEvenOffset), Quaternion.identity);

        for (int i = 0; i < wallTiles.GetLength(0); i++)
            for (int j = 0; j < wallTiles.GetLength(1); j++)
                if (wallTiles[i, j] == wallTile)
                    Instantiate(wallTiles[i, j], new Vector3(((-(wallTiles.GetLength(1) / 2) + j) * scale) + xEvenOffset, scale, (((wallTiles.GetLength(0) / 2) - i) * scale) - yEvenOffset), Quaternion.identity);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void MapIntToEnum()
    {
        map = new TileType[loadedMap.GetLength(0), loadedMap.GetLength(1)];

        for (int i = 0; i < loadedMap.GetLength(0); i++)
            for (int j = 0; j < loadedMap.GetLength(1); j++)
                if (loadedMap[i, j] == 1)
                {
                    if (loadedMap[i - 1, j] == 0)
                        map[i, j] = TileType.WallN;
                    if (loadedMap[i + 1, j] == 0)
                        map[i, j] = TileType.WallS;
                }
                else
                    map[i, j] = TileType.Floor;
    }
}
