using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{

    public ItemType Type { get { return type; } }
    public TowerFSMBase TowerComponent { get { return towerComponent; } }

    [SerializeField] GridManager gridManager;

    Transform imageTransform;
    public enum ItemType { Tower }

    ItemType type;
    TowerFSMBase towerComponent;
    public ImageCopy GenerateImageCopy(Transform parentTransform)
    {
        Transform imageCopy = Instantiate(imageTransform.gameObject, parentTransform.position, Quaternion.identity).GetComponent<Transform>();
        SpriteRenderer[] renderers = imageCopy.GetComponentsInChildren<SpriteRenderer>();
        return new ImageCopy(imageCopy.gameObject, renderers);
    }
    
    private void Awake()
    {
        towerComponent = GetComponent<TowerFSMBase>();
        if (towerComponent != null)
        {
            type = ItemType.Tower;
            imageTransform = towerComponent.ImagesTransform;
        }
    }
    
    private void Start()
    {
        gridManager.PutObjectAt(transform.position, this);
    }
    /*public void SetSelected(bool selected)
    {
        if (selected)
            spriteRenderer.color = new Color(0, 0, 1);
        else
            spriteRenderer.color = new Color(1, 1, 1);

    }*/
}
