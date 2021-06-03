using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GridBackground
{
    Road, Wall, Empty, NoTower, NoPlayer
}

public class GridManager : MonoBehaviour
{
    [SerializeField]
    Vector2Int topRight;
    [SerializeField]
    Vector2Int bottomLeft;

    GridObject[,] objectGrid;
    GridBackground[,] backgroundGrid;

    private void Awake()
    {
        Vector2Int gridSize = topRight - bottomLeft;
        objectGrid = new GridObject[gridSize.y, gridSize.x];
        for (int i = 0; i < gridSize.y; i++)
        {
            for (int j = 0; j < gridSize.x; j++)
                objectGrid[i, j] = null;
        }
        backgroundGrid = new GridBackground[gridSize.y, gridSize.x];
        for (int i = 0; i < gridSize.y; i++)
        {
            for (int j = 0; j < gridSize.x; j++)
                backgroundGrid[i, j] = GridBackground.Empty;
        }

    }
    public Vector3 WorldPosToGridPos(Vector3 position)
    {
        return GridIndexToWorldPos(WorldPosToGridIndex(position));
    }
    public Vector2Int WorldPosToGridIndex(Vector3 position)
    {
        Vector2Int flooredVector = Vector2Int.FloorToInt(VectorUtils.Vec3toVec2(position));
        return flooredVector - bottomLeft;
    }
    public Vector3 GridIndexToWorldPos(Vector2Int gridIndex)
    {
        Vector2 pos = gridIndex + bottomLeft;
        return new Vector3(pos.x + 0.5f, pos.y + 0.5f, transform.position.z);
    }
    public GridBackground GetBackgroundAt(Vector2Int gridIndex)
    {
        return backgroundGrid[gridIndex.y, gridIndex.x];
    }
    public GridBackground GetBackgroundAt(Vector3 worldPosition)
    {
        return GetBackgroundAt(WorldPosToGridIndex(worldPosition));
    }
    public void SetBackgroundAt(Vector3 worldPosition, GridBackground type)
    {
        Vector2Int index = WorldPosToGridIndex(worldPosition);
        backgroundGrid[index.y, index.x] = type;
    }
    public void AddObjectAt(Vector3 worldPosition, GridObject target)
    {
        Vector2Int index = WorldPosToGridIndex(worldPosition);
        objectGrid[index.y, index.x] = target;
    }
    public GridObject RemoveObjectAt(Vector3 worldPosition)
    {
        Vector2Int index = WorldPosToGridIndex(worldPosition);
        GridObject temp = objectGrid[index.y, index.x];
        objectGrid[index.y, index.x] = null;
        return temp;
    }
    public GridObject GetObjectAt(Vector2Int gridIndex)
    {
        return objectGrid[gridIndex.y, gridIndex.x];
    }
    public GridObject GetObjectAt(Vector3 worldPosition)
    {
        return GetObjectAt(WorldPosToGridIndex(worldPosition));
    }
    

}
