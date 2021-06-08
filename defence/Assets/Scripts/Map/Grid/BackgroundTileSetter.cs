using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundTileSetter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GridManager gridManager;
    [Header("Value of child tiles")]
    [SerializeField] GridBackground tileType;
    Transform[] childObjects;

    private void Start()
    {
        childObjects = GetComponentsInChildren<Transform>();
        for(int i=1; i<childObjects.Length; i++)
        {
            Vector2Int topRight = Vector2Int.RoundToInt(childObjects[i].position + childObjects[i].localScale/2);
            Vector2Int bottomLeft = Vector2Int.RoundToInt(childObjects[i].position - childObjects[i].localScale / 2);
            
            for(float x = bottomLeft.x + 0.5f; x<= topRight.x - 0.5f; x++)
            {
                for(float y = bottomLeft.y + 0.5f; y<= topRight.y - 0.5f; y++)
                {
                    if (gridManager.IsInBound(new Vector3(x, y)))
                    {
                        gridManager.SetBackgroundAt(new Vector3(x, y), tileType);
                    }
                }
            }
        }
    }
}
