using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageCopy
{
    public GameObject gameObject { get { return _gameObject; } }
    GameObject _gameObject;
    SpriteRenderer []spriteRenderers;
    public ImageCopy(GameObject gameObject, SpriteRenderer [] spriteRenderers)
    {
        this._gameObject = gameObject;
        this.spriteRenderers = spriteRenderers;
    }

    public void SetColor(Color color)
    {
        foreach (SpriteRenderer renderer in spriteRenderers)
            renderer.color = color;
    }
    public void SetLayer(string layerName)
    {
        foreach (SpriteRenderer renderer in spriteRenderers)
            renderer.sortingLayerName = layerName;
    }
    public void SetPosition(Vector3 position)
    {
        gameObject.transform.position = position;
    }
}
