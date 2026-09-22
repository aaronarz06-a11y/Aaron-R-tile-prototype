using UnityEngine;
using System.Collections.Generic;

public class GridMap : MonoBehaviour
{
    public GameObject tilePrefab;
    public GameObject hazardPrefab;
    public GameObject exitPrefab;
    public GameObject conveyorPrefab;
    public int columns = 5;
    public int rows = 5;
    public float tileSize = 2f;
    public int hazardCount = 2;
    public int exitCount = 1;
    public int conveyorCount = 1;
    private List<Vector3> tilePositions = new List<Vector3>();
    private List<GameObject> currentTiles = new List<GameObject>();

    void Start()
    {
        CreateGrid();
    }

    void CreateGrid()
    {
        tilePositions.Clear();

        //stores all positions that are not holes
        for (int x = 0; x < columns; x++)
        {
            for (int z = 0; z < rows; z++)
            {
                //leaves spaces empty to create holes
                if (x == 2 && z == 0)
                {
                    continue;
                }

                if (x == 0 && z == 2)
                {
                    continue;
                }

                if (x == 4 && z == 2)
                {
                    continue;
                }

                Vector3 position = new Vector3(x * tileSize, 0, z * tileSize);

                tilePositions.Add(position);
            }
        }
        CreateTiles();
    }

    void CreateTiles()
    {
        List<GameObject> tileTypes = new List<GameObject>();

        //adds hazards
        for (int i = 0; i < hazardCount; i++)
        {
            tileTypes.Add(hazardPrefab);
        }

        //adds exit
        for (int i = 0; i < exitCount; i++)
        {
            tileTypes.Add(exitPrefab);
        }

        //adds conveyors
        for (int i = 0; i < conveyorCount; i++)
        {
            tileTypes.Add(conveyorPrefab);
        }

        //makes sure there aren't more special tiles than available spaces
        if (tileTypes.Count > tilePositions.Count)
        {
            Debug.LogError("There are too many special tiles for the size of the grid!");
            return;
        }

        //fills the remaining spaces with normal tiles
        int normalTileCount = tilePositions.Count - tileTypes.Count;

        for (int i = 0; i < normalTileCount; i++)
        {
            tileTypes.Add(tilePrefab);
        }

        ShuffleTiles(tileTypes);

        //creates each tile
        for (int i = 0; i < tilePositions.Count; i++)
        {
            GameObject newTile = Instantiate(tileTypes[i], tilePositions[i], Quaternion.identity, transform);

            currentTiles.Add(newTile);
        }
    }

    //changes the locations of all non hole tiles
    public void ChangeTiles()
    {
        //deletes the old tiles
        foreach (GameObject tile in currentTiles)
        {
            Destroy(tile);
        }

        currentTiles.Clear();

        //creates the tiles in new random locations
        CreateTiles();
    }

    //randomize the tile types
    void ShuffleTiles(List<GameObject> tiles)
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            int randomIndex = Random.Range(i, tiles.Count);

            GameObject temporary = tiles[i];
            tiles[i] = tiles[randomIndex];
            tiles[randomIndex] = temporary;
        }
    }
}