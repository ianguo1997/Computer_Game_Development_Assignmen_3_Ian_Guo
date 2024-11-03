using UnityEngine;
using System.Collections.Generic; // This line is needed for Dictionary

public class LevelGenerator : MonoBehaviour
{
    public int[,] levelMap = new int[,]
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

    public float tileSize = 1.0f;

    // Dictionary to store rotations for each position in the original quadrant
    private Dictionary<Vector2, Quaternion> originalRotations = new Dictionary<Vector2, Quaternion>();

    void Start()
    {
        // ?????????????
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // ??????
        GenerateLevel();  // ???????????
        MirrorLevel();    // ???????????

    }



    /// <summary>
    /// ????????????????
    /// </summary>
    private void GenerateLevel()
    {
        for (int y = 0; y < levelMap.GetLength(0); y++)
        {
            for (int x = 0; x < levelMap.GetLength(1); x++)
            {
                Vector3 position = new Vector3(x * tileSize, -y * tileSize, 0);
                GameObject tile = InstantiateTile(levelMap[y, x], position, x, y);

                if (tile != null)
                {
                    originalRotations[new Vector2(x, y)] = tile.transform.rotation;
                    // ????????????????
                    // Debug.Log($"Stored rotation for ({x}, {y}): {tile.transform.rotation.eulerAngles}");
                }
            }
        }
    }

    private void MirrorLevel()
    {
        int rows = levelMap.GetLength(0);
        int cols = levelMap.GetLength(1);

        // ??????????
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                int originalX = cols - x - 1;
                Vector3 position = new Vector3((cols + x) * tileSize, -y * tileSize, 0);
                GameObject tile = InstantiateTile(levelMap[y, originalX], position, x, y);

                if (tile != null)
                {
                    Vector2 originalPos = new Vector2(originalX, y);
                    if (originalRotations.TryGetValue(originalPos, out Quaternion originalRotation))
                    {
                        tile.transform.rotation = AdjustTileRotation(levelMap[y, originalX], originalRotation, mirrorX: true, mirrorY: false);

                        // ??? InnerCorner ? OutsideCorner????? 90 ?
                        if (levelMap[y, originalX] == 1 || levelMap[y, originalX] == 3)
                        {
                            tile.transform.Rotate(0, 0, 90);
                        }

                        // ??? T-Junction????? 180 ?
                        if (levelMap[y, originalX] == 7)
                        {
                            tile.transform.Rotate(0, 0, 180);
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"Original rotation not found for ({originalX}, {y})");
                    }
                }
            }
        }

        // ??????????
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                int originalY = rows - y - 1;
                Vector3 position = new Vector3(x * tileSize, -(rows + y) * tileSize, 0);
                GameObject tile = InstantiateTile(levelMap[originalY, x], position, x, y);

                if (tile != null)
                {
                    Vector2 originalPos = new Vector2(x, originalY);
                    if (originalRotations.TryGetValue(originalPos, out Quaternion originalRotation))
                    {
                        tile.transform.rotation = AdjustTileRotation(levelMap[originalY, x], originalRotation, mirrorX: false, mirrorY: true);

                        // ??? InnerCorner ? OutsideCorner????? 90 ?
                        if (levelMap[originalY, x] == 1 || levelMap[originalY, x] == 3)
                        {
                            tile.transform.Rotate(0, 0, 90);
                        }

                        // ??? T-Junction????? 180 ?
                        if (levelMap[originalY, x] == 7)
                        {
                            tile.transform.Rotate(0, 0, 180);
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"Original rotation not found for ({x}, {originalY})");
                    }
                }
            }
        }

        // ???????????????
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                int originalX = cols - x - 1;
                int originalY = rows - y - 1;
                Vector3 position = new Vector3((cols + x) * tileSize, -(rows + y) * tileSize, 0);
                GameObject tile = InstantiateTile(levelMap[originalY, originalX], position, x, y);

                if (tile != null)
                {
                    Vector2 originalPos = new Vector2(originalX, originalY);
                    if (originalRotations.TryGetValue(originalPos, out Quaternion originalRotation))
                    {
                        tile.transform.rotation = AdjustTileRotation(levelMap[originalY, originalX], originalRotation, mirrorX: true, mirrorY: true);
                        // ??????????????????
                    }
                    else
                    {
                        Debug.LogWarning($"Original rotation not found for ({originalX}, {originalY})");
                    }
                }
            }
        }
    }


    ///// <summary>
    ///// ?????????????????????
    ///// </summary>
    //private void MirrorLevel()
    //{
    //    int rows = levelMap.GetLength(0);
    //    int cols = levelMap.GetLength(1);

    //    // ??????????
    //    for (int y = 0; y < rows; y++)
    //    {
    //        for (int x = 0; x < cols; x++)
    //        {
    //            int originalX = cols - x - 1;
    //            Vector3 position = new Vector3((cols + x) * tileSize, -y * tileSize, 0);
    //            GameObject tile = InstantiateTile(levelMap[y, originalX], position, x, y);

    //            if (tile != null)
    //            {
    //                Vector2 originalPos = new Vector2(originalX, y);
    //                if (originalRotations.TryGetValue(originalPos, out Quaternion originalRotation))
    //                {
    //                    tile.transform.rotation = AdjustTileRotation(levelMap[y, originalX], originalRotation, mirrorX: true, mirrorY: false);
    //                }
    //                else
    //                {
    //                    Debug.LogWarning($"Original rotation not found for ({originalX}, {y})");
    //                }
    //            }
    //        }
    //    }

    //    // ??????????
    //    for (int y = 0; y < rows; y++)
    //    {
    //        for (int x = 0; x < cols; x++)
    //        {
    //            int originalY = rows - y - 1;
    //            Vector3 position = new Vector3(x * tileSize, -(rows + y) * tileSize, 0);
    //            GameObject tile = InstantiateTile(levelMap[originalY, x], position, x, y);

    //            if (tile != null)
    //            {
    //                Vector2 originalPos = new Vector2(x, originalY);
    //                if (originalRotations.TryGetValue(originalPos, out Quaternion originalRotation))
    //                {
    //                    tile.transform.rotation = AdjustTileRotation(levelMap[originalY, x], originalRotation, mirrorX: false, mirrorY: true);
    //                }
    //                else
    //                {
    //                    Debug.LogWarning($"Original rotation not found for ({x}, {originalY})");
    //                }
    //            }
    //        }
    //    }

    //    // ???????????????
    //    for (int y = 0; y < rows; y++)
    //    {
    //        for (int x = 0; x < cols; x++)
    //        {
    //            int originalX = cols - x - 1;
    //            int originalY = rows - y - 1;
    //            Vector3 position = new Vector3((cols + x) * tileSize, -(rows + y) * tileSize, 0);
    //            GameObject tile = InstantiateTile(levelMap[originalY, originalX], position, x, y);

    //            if (tile != null)
    //            {
    //                Vector2 originalPos = new Vector2(originalX, originalY);
    //                if (originalRotations.TryGetValue(originalPos, out Quaternion originalRotation))
    //                {
    //                    tile.transform.rotation = AdjustTileRotation(levelMap[originalY, originalX], originalRotation, mirrorX: true, mirrorY: true);
    //                }
    //                else
    //                {
    //                    Debug.LogWarning($"Original rotation not found for ({originalX}, {originalY})");
    //                }
    //            }
    //        }
    //    }
    //}

    /// <summary>
    /// ???????????????????
    /// </summary>
    /// <param name="tileType">?????</param>
    /// <param name="originalRotation">?????</param>
    /// <param name="mirrorX">???????</param>
    /// <param name="mirrorY">???????</param>
    /// <returns>???????</returns>
    private Quaternion AdjustTileRotation(int tileType, Quaternion originalRotation, bool mirrorX, bool mirrorY)
    {
        Vector3 euler = originalRotation.eulerAngles;

        // ?????????????????
        if (tileType == 1 || tileType == 3 || tileType == 7)
        {
            if (mirrorX)
            {
                euler.z = (180 - euler.z) % 360;
            }

            if (mirrorY)
            {
                euler.z = (360 - euler.z) % 360;
            }

            // ?????
            euler.z = (euler.z + 360) % 360;
        }

        return Quaternion.Euler(euler);
    }

    /// <summary>
    /// ??????????????
    /// </summary>
    /// <param name="tileType">?????</param>
    /// <param name="position">?????</param>
    /// <param name="x">???X???</param>
    /// <param name="y">???Y???</param>
    /// <returns>?????????</returns>
    private GameObject InstantiateTile(int tileType, Vector3 position, int x, int y)
    {
        GameObject tilePrefab = GetTilePrefab(tileType);
        if (tilePrefab != null)
        {
            GameObject newTile = Instantiate(tilePrefab, position, Quaternion.identity);
            newTile.transform.parent = transform;
            AdjustRotation(newTile, tileType, x, y);
            return newTile;
        }
        return null;
    }

    /// <summary>
    /// ???????????????
    /// </summary>
    /// <param name="tileType">?????</param>
    /// <returns>???????</returns>
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



    private void AdjustRotation(GameObject tile, int tileType, int x, int y)
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


            // Inside Wall (tileType == 4) / ?? (tileType == 4)
            else if (tileType == 4)
            {
                // ?????? Pellet ???????? (z = 0)
                // If there is a Pellet or empty space above or below, set to vertical (z = 0)
                if ((y > 0 && (levelMap[y - 1, x] == 5 || levelMap[y - 1, x] == 0)) ||
                    (y < levelMap.GetLength(0) - 1 && (levelMap[y + 1, x] == 5 || levelMap[y + 1, x] == 0)))
                {
                    tile.transform.rotation = Quaternion.Euler(0, 0, 90); // Vertical / ??
                }
                // ?????? Pellet ???????? (z = 90)
                // If there is a Pellet or empty space on the left or right, set to horizontal (z = 90)
                else if ((x > 0 && (levelMap[y, x - 1] == 5 || levelMap[y, x - 1] == 0)) ||
                         (x < levelMap.GetLength(1) - 1 && (levelMap[y, x + 1] == 5 || levelMap[y, x + 1] == 0)))
                {
                    tile.transform.rotation = Quaternion.Euler(0, 0, 0); // Horizontal / ??
                }
            }






            // Outside Corner (tileType == 1) / ??? (tileType == 1)
            else if (tileType == 1)
            {
                // Right and bottom are outside walls, keep default rotation / ??????????????
                if ((x < levelMap.GetLength(1) - 1 && levelMap[y, x + 1] == 2) && (y < levelMap.GetLength(0) - 1 && levelMap[y + 1, x] == 2))
                {
                    tile.transform.rotation = Quaternion.Euler(0, 0, 0); // Default rotation / ???
                }
                // Top and right are outside walls, rotate 90 degrees / ???????????90?
                else if ((y > 0 && levelMap[y - 1, x] == 2) && (x < levelMap.GetLength(1) - 1 && levelMap[y, x + 1] == 2))
                {
                    tile.transform.rotation = Quaternion.Euler(0, 0, 90); // Rotate 90 degrees / ??90?
                }
                // Top and left are outside walls, rotate 180 degrees / ???????????180?
                else if ((y > 0 && levelMap[y - 1, x] == 2) && (x > 0 && levelMap[y, x - 1] == 2))
                {
                    tile.transform.rotation = Quaternion.Euler(0, 0, 180); // Rotate 180 degrees / ??180?
                }
                // Left and bottom are outside walls, rotate 270 degrees / ???????????270?
                else if ((x > 0 && levelMap[y, x - 1] == 2) && (y < levelMap.GetLength(0) - 1 && levelMap[y + 1, x] == 2))
                {
                    tile.transform.rotation = Quaternion.Euler(0, 0, 270); // Rotate 270 degrees / ??270?
                }
            }


            // Inside Corner (tileType == 3) / ?? (tileType == 3)
            else if (tileType == 3)
            {



                // Left and above are pellets, set rotation to 0 (z = 0) / ???????pellet???z = 0
                if ((x > 0 && levelMap[y, x - 1] == 5) && (y > 0 && levelMap[y - 1, x] == 5))
                {
                    tile.transform.rotation = Quaternion.Euler(0, 0, 0); // Default rotation / ????
                }
                // Left and below are pellets, set rotation to 90 (z = 90) / ???????pellet???z = 90
                else if ((x > 0 && levelMap[y, x - 1] == 5) && (y < levelMap.GetLength(0) - 1 && levelMap[y + 1, x] == 5))
                {
                    tile.transform.rotation = Quaternion.Euler(0, 0, 90); // Rotate 90 degrees / ??90?
                }
                // Below and right are pellets, set rotation to 180 (z = 180) / ???????pellet???z = 180
                else if ((y < levelMap.GetLength(0) - 1 && levelMap[y + 1, x] == 5) && (x < levelMap.GetLength(1) - 1 && levelMap[y, x + 1] == 5))
                {
                    tile.transform.rotation = Quaternion.Euler(0, 0, 180); // Rotate 180 degrees / ??180?
                }
                // Above and right are pellets, set rotation to 270 (z = 270) / ???????pellet???z = 270
                else if ((y > 0 && levelMap[y - 1, x] == 5) && (x < levelMap.GetLength(1) - 1 && levelMap[y, x + 1] == 5))
                {
                    tile.transform.rotation = Quaternion.Euler(0, 0, 270); // Rotate 270 degrees / ??270?
                }

                // ???????????????????? Pellet ?????? z = 0
                else if ((x < levelMap.GetLength(1) - 1 && levelMap[y, x + 1] == 4) &&
                         (y < levelMap.GetLength(0) - 1 && levelMap[y + 1, x] == 4) &&
                         (x < levelMap.GetLength(1) - 1 && y < levelMap.GetLength(0) - 1 &&
                          (levelMap[y + 1, x + 1] == 5 || levelMap[y + 1, x + 1] == 0)))
                {
                    tile.transform.rotation = Quaternion.Euler(0, 0, 0); // ?? z = 0 / Set z = 0
                }

                // ????????????? Pellet ?????? z = 90
                else if ((y > 0 && levelMap[y - 1, x] == 4) &&
                         (x < levelMap.GetLength(1) - 1 && levelMap[y, x + 1] == 4) &&
                         (y > 0 && x < levelMap.GetLength(1) - 1 &&
                          (levelMap[y - 1, x + 1] == 5 || levelMap[y - 1, x + 1] == 0)))
                {
                    tile.transform.rotation = Quaternion.Euler(0, 0, 90); // ?? z = 90 / Set z = 90
                }

                // ????????????? Pellet ?????? z = 180
                else if ((y > 0 && levelMap[y - 1, x] == 4) &&
                         (x > 0 && levelMap[y, x - 1] == 4) &&
                         (y > 0 && x > 0 && (levelMap[y - 1, x - 1] == 5 || levelMap[y - 1, x - 1] == 0)))
                {
                    tile.transform.rotation = Quaternion.Euler(0, 0, 180); // ?? z = 180 / Set z = 180
                }

                // ????????????? Pellet ?????? z = 270
                else if ((x > 0 && levelMap[y, x - 1] == 4) &&
                         (y < levelMap.GetLength(0) - 1 && levelMap[y + 1, x] == 4) &&
                         (x > 0 && y < levelMap.GetLength(0) - 1 &&
                          (levelMap[y + 1, x - 1] == 5 || levelMap[y + 1, x - 1] == 0)))
                {
                    tile.transform.rotation = Quaternion.Euler(0, 0, 270); // ?? z = 270 / Set z = 270
                }


                // Left and above are either Pellet or Empty, set rotation to 0 (z = 0)
                // ??????pellet?????z = 0
                else if ((x > 0 && (levelMap[y, x - 1] == 5 || levelMap[y, x - 1] == 0)) &&
                    (y > 0 && (levelMap[y - 1, x] == 5 || levelMap[y - 1, x] == 0)))
                {
                    tile.transform.rotation = Quaternion.Euler(0, 0, 0); // Default rotation / ????
                }
                // Left and below are either Pellet or Empty, set rotation to 90 (z = 90)
                // ??????pellet?????z = 90
                else if ((x > 0 && (levelMap[y, x - 1] == 5 || levelMap[y, x - 1] == 0)) &&
                         (y < levelMap.GetLength(0) - 1 && (levelMap[y + 1, x] == 5 || levelMap[y + 1, x] == 0)))
                {
                    tile.transform.rotation = Quaternion.Euler(0, 0, 90); // Rotate 90 degrees / ??90?
                }
                // Below and right are either Pellet or Empty, set rotation to 180 (z = 180)
                // ??????pellet?????z = 180
                else if ((y < levelMap.GetLength(0) - 1 && (levelMap[y + 1, x] == 5 || levelMap[y + 1, x] == 0)) &&
                         (x < levelMap.GetLength(1) - 1 && (levelMap[y, x + 1] == 5 || levelMap[y, x + 1] == 0)))
                {
                    tile.transform.rotation = Quaternion.Euler(0, 0, 180); // Rotate 180 degrees / ??180?
                }
                // Above and right are either Pellet or Empty, set rotation to 270 (z = 270)
                // ??????pellet?????z = 270
                else if ((y > 0 && (levelMap[y - 1, x] == 5 || levelMap[y - 1, x] == 0)) &&
                         (x < levelMap.GetLength(1) - 1 && (levelMap[y, x + 1] == 5 || levelMap[y, x + 1] == 0)))
                {
                    tile.transform.rotation = Quaternion.Euler(0, 0, 270); // Rotate 270 degrees / ??270?
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
