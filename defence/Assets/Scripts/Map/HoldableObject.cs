using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HoldableObject : GridObject
{
    protected Transform imageCopyTransform;
    public ImageCopy GenerateImageCopy(Vector3 position)
    {
        Transform imageCopy = Instantiate(imageCopyTransform.gameObject, position, Quaternion.identity).GetComponent<Transform>();
        SpriteRenderer[] renderers = imageCopy.GetComponentsInChildren<SpriteRenderer>();
        return new ImageCopy(imageCopy.gameObject, renderers);
    }
}
