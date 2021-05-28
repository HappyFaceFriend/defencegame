using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGuide : MonoBehaviour
{
    [SerializeField] GridManager gridManager;
    public Vector3 position { get { return transform.position; } set { transform.position = value; } }
    public Sprite sprite {
        get
        {
            if (spriteRenderer != null)
                return spriteRenderer.sprite;
            return null;
        } 
        set { 
            if(spriteRenderer!= null)
                spriteRenderer.sprite = value; 
        } 
    }
    [SerializeField]
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Awake()
    {
    }
    private void Update()
    {
        if(gridManager.GetObjectAt(position) != null) // and not mixable
        {
            spriteRenderer.color = new Color(1, 0, 0, spriteRenderer.color.a);
        }
        else
        {
            spriteRenderer.color = new Color(1, 1, 1, spriteRenderer.color.a);
        }
    }

}
