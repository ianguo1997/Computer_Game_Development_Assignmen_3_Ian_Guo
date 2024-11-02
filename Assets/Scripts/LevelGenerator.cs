using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    private int[,] levelMap = new int[,]
    {
        {1,2,2,2,2,2,2,2,2,2,2,2,2,7},
        {2,5,5,5,5,5,5,5,5,5,5,5,5,4},
        {2,5,3,4,4,3,5,3,4,4,4,3,5,4},
        {2,6,4,0,0,4,5,4,0,0,0,4,5,4},
        {2,5,3,4,4,3,5,3,4,4,4,3,5,3},
        {2,5,5,5,5,5,5,5,5,5,5,5,5,5},
        {2,5,3,4,4,3,5,3,3,5,3,4,4,4},
        {2,5,3,4,4,3,5,4,4,5,3,4,4,3},
        {2,5,5,5,5,5,5,4,4,5,5,5,5,4},
        {1,2,2,2,2,1,5,4,3,4,4,3,0,4},
        {0,0,0,0,0,2,5,4,3,4,4,3,0,3},
        {0,0,0,0,0,2,5,4,4,0,0,0,0,0},
        {0,0,0,0,0,2,5,4,4,0,3,4,4,0},
        {2,2,2,2,2,1,5,3,3,0,4,0,0,0},
        {0,0,0,0,0,0,5,0,0,0,4,0,0,0},
    };

    public GameObject outsideCornerPrefab;
    public GameObject outsideWallPrefab;
    public GameObject insideCornerPrefab;
    public GameObject insideWallPrefab;
    public GameObject pelletPrefab;
    public GameObject powerPelletPrefab;
    public GameObject tJunctionPrefab;

    private float tileSize = 1.0f;

    void Start()
    {
        GenerateLevel();
        MirrorLevel();
    }

    private void GenerateLevel()
    {
        // Generate the original top-left quadrant
        for (int y = 0; y < levelMap.GetLength(0); y++)
        {
            for (int x = 0; x < levelMap.GetLength(1); x++)
            {
                Vector3 position = new Vector3(x * tileSize, -y * tileSize, 0);
                InstantiateTile(levelMap[y, x], position, x, y);
            }
        }
    }

    private void MirrorLevel()
    {
        int rows = levelMap.GetLength(0);
        int cols = levelMap.GetLength(1);

        // Mirror horizontally (top-right quadrant)
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                Vector3 position = new Vector3((cols + x) * tileSize, -y * tileSize, 0);
                InstantiateTile(levelMap[y, cols - x - 1], position, x, y, true);
            }
        }

        // Mirror vertically (bottom-left quadrant)
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                Vector3 position = new Vector3(x * tileSize, -(rows + y) * tileSize, 0);
                InstantiateTile(levelMap[rows - y - 1, x], position, x, y, true);
            }
        }

        // Mirror both horizontally and vertically (bottom-right quadrant)
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                Vector3 position = new Vector3((cols + x) * tileSize, -(rows + y) * tileSize, 0);
                InstantiateTile(levelMap[rows - y - 1, cols - x - 1], position, x, y, true);
            }
        }
    }

    private void InstantiateTile(int tileType, Vector3 position, int x, int y, bool isMirrored = false)
    {
        GameObject tilePrefab = GetTilePrefab(tileType);
        if (tilePrefab != null)
        {
            GameObject newTile = Instantiate(tilePrefab, position, Quaternion.identity);
            newTile.transform.parent = transform;
            AdjustRotation(newTile, x, y, isMirrored);
        }
    }

    private GameObject GetTilePrefab(int tileType)
    {
        switch (tileType)
        {
            case 1: return outsideCornerPrefab;
            case 2: return outsideWallPrefab;
            case 3: return insideCornerPrefab;
            case 4: return insideWallPrefab;
            case 5: return pelletPrefab;
            case 6: return powerPelletPrefab;
            case 7: return tJunctionPrefab;
            default: return null;
        }
    }

    private void AdjustRotation(GameObject tile, int x, int y, bool isMirrored)
    {
        int tileType = levelMap[y, x];

        // Adjust rotation for specific cases
        if (tileType == 2) // Outside wall
        {
            if (y > 0 && levelMap[y - 1, x] == 2)
                tile.transform.rotation = Quaternion.Euler(0, 0, isMirrored ? -90 : 90);
        }
        else if (tileType == 1) // Outside corner
        {
            if (x < levelMap.GetLength(1) - 1 && levelMap[y, x + 1] == 2)
                tile.transform.rotation = Quaternion.Euler(0, 0, 0);
            else
                tile.transform.rotation = Quaternion.Euler(0, 0, isMirrored ? -90 : 90);
        }
        // Adjust other rotations as needed based on tile type and mirroring
    }
}
