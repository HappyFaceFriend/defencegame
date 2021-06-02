using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{

    public ItemType Type { get { return type; } }
    public TowerFSMBase TowerComponent { get { return towerComponent; } }
    public TowerMaker TowerMakerComponent { get { return towerMakerComponent; } }


    Transform imageTransform;
    public enum ItemType { Tower, TowerMaker }

    ItemType type;
    TowerFSMBase towerComponent;
    [SerializeField] TowerMaker towerMakerComponent;
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
        else
        {
            type = ItemType.TowerMaker;
        }
    }
    
    private void Start()
    {
        //gridManager.PutObjectAt(transform.position, this);

    }
    /*public void SetSelected(bool selected)
    {
        if (selected)
            spriteRenderer.color = new Color(0, 0, 1);
        else
            spriteRenderer.color = new Color(1, 1, 1);

    }*/
}
