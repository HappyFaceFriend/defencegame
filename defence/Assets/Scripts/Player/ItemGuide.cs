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
    [SerializeField] SpriteRenderer guideBox;

    [Header("Box Colors")]
    [SerializeField] Color boxColorSelected;
    [SerializeField] Color boxColorUnselected;
    [SerializeField] Color boxColorRed;

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
    public void SetState(bool isSelected, bool isInteractable, GridBackground background)
    {
        bool isNothingInFront = (!isSelected && background == GridBackground.Empty);
        if (!isSelected)
            gridTargetAnim.SetBool("IsSelected", false);
        else
            gridTargetAnim.SetBool("IsSelected", true);

        if (!isNothingInFront && !isInteractable)
        {
            guideBox.color = boxColorRed;
        }
        else if (isNothingInFront && !isInteractable)
        {
            guideBox.color = boxColorUnselected;
        }
        else
        {
            guideBox.color = boxColorSelected;
        }

        if (copiedImage != null)
        {
            if (!isInteractable)
                copiedImage.SetColor(ghostUnplacable);
            else
                copiedImage.SetColor(ghostPlacable);
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
