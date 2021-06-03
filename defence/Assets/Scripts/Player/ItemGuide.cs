using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGuide : MonoBehaviour
{
    public Vector3 position { get { return transform.position; } set { transform.position = value; } }
    public ImageCopy CopiedImage {  get { return copiedImage; } set { copiedImage = value; } }

    [Header("References")]
    [SerializeField] GridManager gridManager;
    [SerializeField] Animator gridTargetAnim;
    [SerializeField] SpriteRenderer whiteBox;
    [Header("Whitebox Colors")]
    [SerializeField] Color whiteBoxSelected;
    [SerializeField] Color whiteBoxUnselected;
    [SerializeField] Color whiteBoxRed;
    [Header("Ghost Colors")]
    [SerializeField] Color ghostPlacable;
    [SerializeField] Color ghostUnplacable;
    ImageCopy copiedImage = null;
    /*
    �Ͼ� : ���� ���� (�����̽��� ���� �� ����)
    �Ķ� : ���� ���� (�����̽��� ���� �� ����)
    ���� : ���� �ִµ� (�����̽��� ���� �� ����)

    Ʋ : ���� ������ ������Ʈ ǥ�ÿ� ���� = ���տ� ���� ������ ����
     * */
    private void Update()
    {
        if (copiedImage != null)
            copiedImage.SetPosition(transform.position);
    }
    public void SetState(bool isSelected, bool isInteractable)
    {
        if(!isSelected)
        {
            gridTargetAnim.SetBool("IsSelected", false);
            whiteBox.color = whiteBoxUnselected;
            if (copiedImage != null)
                copiedImage.SetColor(ghostPlacable);

        }
        else
        {
            gridTargetAnim.SetBool("IsSelected", true);
            if (isInteractable)
            {
                whiteBox.color = whiteBoxSelected;
                if(copiedImage != null)
                    copiedImage.SetColor(ghostPlacable);
            }
            else
            {
                whiteBox.color = whiteBoxRed;
                if (copiedImage != null)
                    copiedImage.SetColor(ghostUnplacable);
            }
        }
    }

    public void InitCopiedImage(ImageCopy imageCopy)
    {
        copiedImage = imageCopy;
        imageCopy.SetLayer("ItemGuide");
        imageCopy.SetColor(ghostPlacable);
    }

    public void DestroyImageCopy()
    {
        Destroy(copiedImage.gameObject);
        copiedImage = null;
    }
}
