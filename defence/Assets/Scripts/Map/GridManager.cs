using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField]
    Vector2Int topRight;
    [SerializeField]
    Vector2Int bottomLeft;

    public
    GridObject[,] gridObjects;
    public GridObject selectedObject;

    private void Awake()
    {
        Vector2Int gridSize = topRight - bottomLeft;
        gridObjects = new GridObject[gridSize.y, gridSize.x];
        for(int i=0; i<gridSize.y; i++)
        {
            for(int j=0; j<gridSize.x; j++)
                gridObjects[i, j] = null;
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
    public GridObject GetObjectAt(Vector3 worldPosition)
    {
        Vector2Int index = WorldPosToGridIndex(worldPosition);
        return gridObjects[index.y, index.x];
    }
    public GridObject GetObjectAt(Vector2Int gridIndex)
    {
        return gridObjects[gridIndex.y, gridIndex.x];
    }
    public GridObject RemoveObjectAt(Vector3 worldPosition)
    {
        Vector2Int index = WorldPosToGridIndex(worldPosition);
        GridObject temp = gridObjects[index.y, index.x];
        gridObjects[index.y, index.x] = null;
        return temp;
    }
    public bool PutObjectAt(Vector3 worldPosition, GridObject targetObject)
    {
        Vector2Int index = WorldPosToGridIndex(worldPosition);
        if (gridObjects[index.y, index.x] != null)
            return false;
        gridObjects[index.y, index.x] = targetObject;
        return true;
    }
    
}
