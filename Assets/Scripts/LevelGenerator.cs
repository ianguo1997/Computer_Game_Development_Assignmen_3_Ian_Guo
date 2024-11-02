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
    private int rows;
    private int cols;

    void Start()
    {
        rows = levelMap.GetLength(0);
        cols = levelMap.GetLength(1);
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
                int mirroredX = cols - x - 1;
                Vector3 position = new Vector3((cols + x) * tileSize, -y * tileSize, 0);
                InstantiateTile(levelMap[y, mirroredX], position, mirroredX, y);
            }
        }

        // Mirror vertically (bottom-left quadrant)
        for (int y = 0; y < rows; y++)
        {
            int mirroredY = rows - y - 1;
            for (int x = 0; x < cols; x++)
            {
                Vector3 position = new Vector3(x * tileSize, -(rows + y) * tileSize, 0);
                InstantiateTile(levelMap[mirroredY, x], position, x, mirroredY);
            }
        }

        // Mirror both horizontally and vertically (bottom-right quadrant)
        for (int y = 0; y < rows; y++)
        {
            int mirroredY = rows - y - 1;
            for (int x = 0; x < cols; x++)
            {
                int mirroredX = cols - x - 1;
                Vector3 position = new Vector3((cols + x) * tileSize, -(rows + y) * tileSize, 0);
                InstantiateTile(levelMap[mirroredY, mirroredX], position, mirroredX, mirroredY);
            }
        }
    }

    private void InstantiateTile(int tileType, Vector3 position, int x, int y)
    {
        GameObject tilePrefab = GetTilePrefab(tileType);
        if (tilePrefab != null)
        {
            GameObject newTile = Instantiate(tilePrefab, position, Quaternion.identity);
            newTile.transform.parent = transform;
            AdjustRotation(newTile, tileType, x, y);
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

    /// <summary>
    /// 根据 Tile 类型和其相邻 Tile 的类型调整 Tile 的旋转。
    /// </summary>
    /// <param name="tile">当前 Tile 的 GameObject</param>
    /// <param name="tileType">当前 Tile 的类型</param>
    /// <param name="x">Tile 的 x 坐标</param>
    /// <param name="y">Tile 的 y 坐标</param>
    /// <param name="isMirrored">是否为镜像生成的 Tile</param>
    private void AdjustRotation(GameObject tile, int tileType, int x, int y)
    {
        // 获取相邻 Tile 的类型
        int up = GetTileType(x, y + 1);
        int down = GetTileType(x, y - 1);
        int left = GetTileType(x - 1, y);
        int right = GetTileType(x + 1, y);

        switch (tileType)
        {
            case 2: // Outside Wall
                AdjustOutsideWall(tile, up, down, left, right);
                break;

            case 1: // Outside Corner
                AdjustOutsideCorner(tile, up, down, left, right);
                break;

            case 3: // Inside Corner
                AdjustInsideCorner(tile, up, down, left, right);
                break;

            case 4: // Inside Wall
                AdjustInsideWall(tile, up, down, left, right);
                break;

            case 7: // T-Junction
                AdjustTJunction(tile, up, down, left, right);
                break;

            // 可以在这里添加更多 TileType 的调整逻辑

            default:
                // 对于不需要旋转的 Tile 类型，可以不做处理
                break;
        }

        // 可选：打印调试信息
        // Debug.Log($"Tile at ({x}, {y}), Type: {tileType}, Rotation: {tile.transform.rotation.eulerAngles}");
    }

    /// <summary>
    /// 获取指定坐标的 Tile 类型，如果越界则返回 -1。
    /// </summary>
    private int GetTileType(int x, int y)
    {
        // y 是从上到下递减，映射到 levelMap 的 rowIndex
        int rowIndex = rows - 1 - y;
        if (x >= 0 && x < cols && rowIndex >= 0 && rowIndex < rows)
        {
            return levelMap[rowIndex, x];
        }
        else
        {
            return -1; // 表示越界
        }
    }

    /// <summary>
    /// 调整外部墙（TileType 2）的旋转。
    /// </summary>
    private void AdjustOutsideWall(GameObject tile, int up, int down, int left, int right)
    {
        bool vertical = (up == 2 || down == 2);
        bool horizontal = (left == 1 || right == 1);

        if (!vertical && horizontal)
        {
            // 竖直方向，不需要旋转
            tile.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (horizontal && vertical)
        {
            // 水平方向，旋转90度
            tile.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else
        {
            // 如果同时有水平和垂直邻居，可以根据具体需求设置旋转角度
            // 这里暂时保持默认旋转
            tile.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    /// <summary>
    /// 调整外部拐角（TileType 1）的旋转。
    /// </summary>
    private void AdjustOutsideCorner(GameObject tile, int up, int down, int left, int right)
    {
        // 判断拐角的方向
        if (up == 1 && left == 1)
        {
            // 左上角，旋转0度
            tile.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (up == 1 && right == 1)
        {
            // 右上角，旋转90度
            tile.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (down == 1 && right == 1)
        {
            // 右下角，旋转180度
            tile.transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else if (down == 1 && left == 1)
        {
            // 左下角，旋转270度
            tile.transform.rotation = Quaternion.Euler(0, 0, 270);
        }
        else
        {
            // 默认旋转或处理异常情况
            tile.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    /// <summary>
    /// 调整内部拐角（TileType 3）的旋转。
    /// </summary>
    private void AdjustInsideCorner(GameObject tile, int up, int down, int left, int right)
    {
        // 判断内部拐角的方向（基于空白区域）
        if (up == 0 && right == 0)
        {
            // 左上内部拐角，旋转0度
            tile.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (up == 0 && left == 0)
        {
            // 右上内部拐角，旋转90度
            tile.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (down == 0 && left == 0)
        {
            // 右下内部拐角，旋转180度
            tile.transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else if (down == 0 && right == 0)
        {
            // 左下内部拐角，旋转270度
            tile.transform.rotation = Quaternion.Euler(0, 0, 270);
        }
        else
        {
            // 默认旋转或处理异常情况
            tile.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    /// <summary>
    /// 调整内部墙体（TileType 4）的旋转。
    /// </summary>
    private void AdjustInsideWall(GameObject tile, int up, int down, int left, int right)
    {
        bool vertical = (up == 4 || down == 4);
        bool horizontal = (left == 4 || right == 4);

        if (vertical && !horizontal)
        {
            // 竖直墙体，保持默认旋转
            tile.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (horizontal && !vertical)
        {
            // 水平墙体，旋转90度
            tile.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else
        {
            // 如果同时有水平和垂直邻居，可以根据具体需求设置旋转角度
            // 这里暂时保持默认旋转
            tile.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }


    /// <summary>
    /// 调整 T 形路口（TileType 7）的旋转。
    /// </summary>
    private void AdjustTJunction(GameObject tile, int up, int down, int left, int right)
    {
        int connections = 0;
        if (up == 2) connections++;
        if (down == 2) connections++;
        if (left == 2) connections++;
        if (right == 2) connections++;

        if (connections != 3)
        {
            // 处理非 T 形路口的情况
            return;
        }

        // 判断缺失的方向，根据缺失的方向来确定 T 的指向
        if (up != 2)
        {
            tile.transform.rotation = Quaternion.Euler(0, 0, 0); // T 向上
        }
        else if (down != 2)
        {
            tile.transform.rotation = Quaternion.Euler(0, 0, 180); // T 向下
        }
        else if (left != 2)
        {
            tile.transform.rotation = Quaternion.Euler(0, 0, 90); // T 向左
        }
        else if (right != 2)
        {
            tile.transform.rotation = Quaternion.Euler(0, 0, 270); // T 向右
        }
        else
        {
            // 默认旋转或处理异常情况
            tile.transform.rotation = Quaternion.Euler(0, 0, 0);
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
