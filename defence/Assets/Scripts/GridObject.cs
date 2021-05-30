using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    public GridManager gridManager;


    public SpriteRenderer spriteRenderer;
    public ItemType Type { get { return type; } }
    public TowerFSMBase TowerComponent { get { return towerComponent; } }

    public enum ItemType { Tower }

    ItemType type;
    TowerFSMBase towerComponent;
    private void Awake()
    {
        towerComponent = GetComponent<TowerFSMBase>();
        if (towerComponent != null)
            type = ItemType.Tower;
    }
    private void Start()
    {
        gridManager.PutObjectAt(transform.position, this);
    }
    public void SetSelected(bool selected)
    {
        if (selected)
            spriteRenderer.color = new Color(0, 0, 1);
        else
            spriteRenderer.color = new Color(1, 1, 1);

    }
}
