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
            AdjustRotation(newTile, tileType, x, y, isMirrored); // Pass isMirrored to AdjustRotation
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

    private void AdjustRotation(GameObject tile, int tileType, int x, int y, bool isMirrored)
    {
        // Outside Wall (tileType == 2) - Horizontal or Vertical
        if (tileType == 2)
        {
            if ((y > 0 && levelMap[y - 1, x] == 2) || (y < levelMap.GetLength(0) - 1 && levelMap[y + 1, x] == 2))
            {
                tile.transform.rotation = Quaternion.Euler(0, 0, 0); // Vertical
            }
            else
            {
                tile.transform.rotation = Quaternion.Euler(0, 0, 90); // Horizontal
            }
        }
        // Outside Corner (tileType == 1)
        else if (tileType == 1)
        {
            if ((y > 0 && levelMap[y - 1, x] == 2) && (x < levelMap.GetLength(1) - 1 && levelMap[y, x + 1] == 2))
            {
                tile.transform.rotation = Quaternion.Euler(0, 0, 0); // Top-Left Corner
            }
            else if ((y > 0 && levelMap[y - 1, x] == 2) && (x > 0 && levelMap[y, x - 1] == 2))
            {
                tile.transform.rotation = Quaternion.Euler(0, 0, 90); // Top-Right Corner
            }
            else if ((y < levelMap.GetLength(0) - 1 && levelMap[y + 1, x] == 2) && (x < levelMap.GetLength(1) - 1 && levelMap[y, x + 1] == 2))
            {
                tile.transform.rotation = Quaternion.Euler(0, 0, 270); // Bottom-Left Corner
            }
            else if ((y < levelMap.GetLength(0) - 1 && levelMap[y + 1, x] == 2) && (x > 0 && levelMap[y, x - 1] == 2))
            {
                tile.transform.rotation = Quaternion.Euler(0, 0, 180); // Bottom-Right Corner
            }
        }
        // Inside Corner (tileType == 3)
        else if (tileType == 3)
        {
            if ((y > 0 && levelMap[y - 1, x] == 0) && (x < levelMap.GetLength(1) - 1 && levelMap[y, x + 1] == 0))
            {
                tile.transform.rotation = Quaternion.Euler(0, 0, 0); // Top-Left Inside Corner
            }
            else if ((y > 0 && levelMap[y - 1, x] == 0) && (x > 0 && levelMap[y, x - 1] == 0))
            {
                tile.transform.rotation = Quaternion.Euler(0, 0, 90); // Top-Right Inside Corner
            }
            else if ((y < levelMap.GetLength(0) - 1 && levelMap[y + 1, x] == 0) && (x < levelMap.GetLength(1) - 1 && levelMap[y, x + 1] == 0))
            {
                tile.transform.rotation = Quaternion.Euler(0, 0, 270); // Bottom-Left Inside Corner
            }
            else if ((y < levelMap.GetLength(0) - 1 && levelMap[y + 1, x] == 0) && (x > 0 && levelMap[y, x - 1] == 0))
            {
                tile.transform.rotation = Quaternion.Euler(0, 0, 180); // Bottom-Right Inside Corner
            }
        }
        // T-Junction (tileType == 7)
        else if (tileType == 7)
        {
            if ((y > 0 && levelMap[y - 1, x] == 2) && (x < levelMap.GetLength(1) - 1 && levelMap[y, x + 1] == 2) && (x > 0 && levelMap[y, x - 1] == 2))
            {
                tile.transform.rotation = Quaternion.Euler(0, 0, 0); // T pointing down
            }
            else if ((y < levelMap.GetLength(0) - 1 && levelMap[y + 1, x] == 2) && (x < levelMap.GetLength(1) - 1 && levelMap[y, x + 1] == 2) && (x > 0 && levelMap[y, x - 1] == 2))
            {
                tile.transform.rotation = Quaternion.Euler(0, 0, 180); // T pointing up
            }
            else if ((x > 0 && levelMap[y, x - 1] == 2) && (y > 0 && levelMap[y - 1, x] == 2) && (y < levelMap.GetLength(0) - 1 && levelMap[y + 1, x] == 2))
            {
                tile.transform.rotation = Quaternion.Euler(0, 0, 90); // T pointing right
            }
            else if ((x < levelMap.GetLength(1) - 1 && levelMap[y, x + 1] == 2) && (y > 0 && levelMap[y - 1, x] == 2) && (y < levelMap.GetLength(0) - 1 && levelMap[y + 1, x] == 2))
            {
                tile.transform.rotation = Quaternion.Euler(0, 0, 270); // T pointing left
            }
        }
    }


}

//using UnityEngine;

//public class LevelGenerator : MonoBehaviour
//{
//    private int[,] levelMap = new int[,]
//    {
//        {1,2,2,2,2,2,2,2,2,2,2,2,2,7},
//        {2,5,5,5,5,5,5,5,5,5,5,5,5,4},
//        {2,5,3,4,4,3,5,3,4,4,4,3,5,4},
//        {2,6,4,0,0,4,5,4,0,0,0,4,5,4},
//        {2,5,3,4,4,3,5,3,4,4,4,3,5,3},
//        {2,5,5,5,5,5,5,5,5,5,5,5,5,5},
//        {2,5,3,4,4,3,5,3,3,5,3,4,4,4},
//        {2,5,3,4,4,3,5,4,4,5,3,4,4,3},
//        {2,5,5,5,5,5,5,4,4,5,5,5,5,4},
//        {1,2,2,2,2,1,5,4,3,4,4,3,0,4},
//        {0,0,0,0,0,2,5,4,3,4,4,3,0,3},
//        {0,0,0,0,0,2,5,4,4,0,0,0,0,0},
//        {0,0,0,0,0,2,5,4,4,0,3,4,4,0},
//        {2,2,2,2,2,1,5,3,3,0,4,0,0,0},
//        {0,0,0,0,0,0,5,0,0,0,4,0,0,0},
//    };

//    public GameObject outsideCornerPrefab;
//    public GameObject outsideWallPrefab;
//    public GameObject insideCornerPrefab;
//    public GameObject insideWallPrefab;
//    public GameObject pelletPrefab;
//    public GameObject powerPelletPrefab;
//    public GameObject tJunctionPrefab;

//    private float tileSize = 1.0f;

//    void Start()
//    {
//        GenerateLevel();
//        MirrorLevel();
//    }

//    private void GenerateLevel()
//    {
//        // Generate the original top-left quadrant
//        for (int y = 0; y < levelMap.GetLength(0); y++)
//        {
//            for (int x = 0; x < levelMap.GetLength(1); x++)
//            {
//                Vector3 position = new Vector3(x * tileSize, -y * tileSize, 0);
//                InstantiateTile(levelMap[y, x], position, x, y);
//            }
//        }
//    }

//    private void MirrorLevel()
//    {
//        int rows = levelMap.GetLength(0);
//        int cols = levelMap.GetLength(1);

//        // Mirror horizontally (top-right quadrant)
//        for (int y = 0; y < rows; y++)
//        {
//            for (int x = 0; x < cols; x++)
//            {
//                Vector3 position = new Vector3((cols + x) * tileSize, -y * tileSize, 0);
//                InstantiateTile(levelMap[y, cols - x - 1], position, x, y, true);
//            }
//        }

//        // Mirror vertically (bottom-left quadrant)
//        for (int y = 0; y < rows; y++)
//        {
//            for (int x = 0; x < cols; x++)
//            {
//                Vector3 position = new Vector3(x * tileSize, -(rows + y) * tileSize, 0);
//                InstantiateTile(levelMap[rows - y - 1, x], position, x, y, true);
//            }
//        }

//        // Mirror both horizontally and vertically (bottom-right quadrant)
//        for (int y = 0; y < rows; y++)
//        {
//            for (int x = 0; x < cols; x++)
//            {
//                Vector3 position = new Vector3((cols + x) * tileSize, -(rows + y) * tileSize, 0);
//                InstantiateTile(levelMap[rows - y - 1, cols - x - 1], position, x, y, true);
//            }
//        }
//    }

//    private void InstantiateTile(int tileType, Vector3 position, int x, int y, bool isMirrored = false)
//    {
//        GameObject tilePrefab = GetTilePrefab(tileType);
//        if (tilePrefab != null)
//        {
//            GameObject newTile = Instantiate(tilePrefab, position, Quaternion.identity);
//            newTile.transform.parent = transform;
//            AdjustRotation(newTile, x, y, isMirrored);
//        }
//    }

//    private GameObject GetTilePrefab(int tileType)
//    {
//        switch (tileType)
//        {
//            case 1: return outsideCornerPrefab;
//            case 2: return outsideWallPrefab;
//            case 3: return insideCornerPrefab;
//            case 4: return insideWallPrefab;
//            case 5: return pelletPrefab;
//            case 6: return powerPelletPrefab;
//            case 7: return tJunctionPrefab;
//            default: return null;
//        }
//    }


//    private void AdjustRotation(GameObject tile, int tileType, int x, int y, bool isMirrored)
//    {
//        // Adjust rotation based on tile type and surrounding tiles
//        if (tileType == 2) // Outside wall
//        {
//            // If there is another wall tile above or below, rotate to vertical
//            if ((y > 0 && levelMap[y - 1, x] == 2) || (y < levelMap.GetLength(0) - 1 && levelMap[y + 1, x] == 2))
//            {
//                tile.transform.rotation = Quaternion.Euler(0, 0, 90);
//            }
//        }
//        else if (tileType == 1) // Outside corner
//        {
//            // Determine rotation based on position in the map and adjacency to other wall pieces
//            if (x < levelMap.GetLength(1) - 1 && levelMap[y, x + 1] == 2)
//            {
//                tile.transform.rotation = Quaternion.Euler(0, 0, 0); // Default rotation
//            }
//            else if (y < levelMap.GetLength(0) - 1 && levelMap[y + 1, x] == 2)
//            {
//                tile.transform.rotation = Quaternion.Euler(0, 0, 90);
//            }
//            else if (x > 0 && levelMap[y, x - 1] == 2)
//            {
//                tile.transform.rotation = Quaternion.Euler(0, 0, 180);
//            }
//            else
//            {
//                tile.transform.rotation = Quaternion.Euler(0, 0, 270);
//            }
//        }
//        // Similar cases can be added for other tile types if needed
//    }
