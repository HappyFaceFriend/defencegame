using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    public bool IsHoldingObject { get { return heldObject != null; } }
    public GridObject SelectedObject { get { return selectedObject; } }
    public GridBackground SelectedBackground {  get { return selectedBackground; } }

    [Header("References")]
    [SerializeField] PlayerFSM player;
    [SerializeField] GridManager gridManager;
    [SerializeField] ItemGuide itemGuide;

    [Header("Holding Positions")]
    [SerializeField] Vector3 sidePos;
    [SerializeField] Vector3 upPos;
    [SerializeField] Vector3 downPos;

    [Header("Status")]
    [ReadOnly] [SerializeField] HoldableObject heldObject;
    [ReadOnly] [SerializeField] GridObject selectedObject;
    [ReadOnly] [SerializeField] GridBackground selectedBackground;
    [ReadOnly] [SerializeField] Vector2Int cellInFront;


    void Awake()
    {
        cellInFront = gridManager.WorldPosToGridIndex(player.transform.position) + Vector2Int.RoundToInt(player.LastInputVector);
    }
    bool IsInteractable()
    {
        if (selectedBackground == GridBackground.OutOfBound)
            return false;
        if (selectedObject != null && !IsHoldingObject) //can grab object
            return true;
        if (IsHoldingObject && (selectedObject == null) && (selectedBackground == GridBackground.Empty))//can put down object
            return true;
        return false;
    }
    private void Update()
    {
        //detect item at direction
        cellInFront = gridManager.WorldPosToGridIndex(player.transform.position) + Vector2Int.RoundToInt(player.LastInputVector);
        itemGuide.position = gridManager.GridIndexToWorldPos(cellInFront);
        if (gridManager.IsInBound(itemGuide.position))
        {
            selectedObject = gridManager.GetObjectAt(cellInFront);
            selectedBackground = gridManager.GetBackgroundAt(cellInFront);
        }
        else
        {
            selectedObject = null;
            selectedBackground = GridBackground.OutOfBound;
        }
        //TODO : currently ignoring add-ons to tower
        itemGuide.SetState(selectedObject != null, IsInteractable(), selectedBackground);
        //change how the player holds item as direction changes (visuals)
        if (player.LastInputVector.y == 1)
            transform.localPosition = upPos;
        else if (player.LastInputVector.y == -1)
            transform.localPosition = downPos;
        if (player.LastInputVector.x != 0)
            transform.localPosition = sidePos;
        if (heldObject != null)
            heldObject.transform.position = transform.position;
    }

    public void HoldSelectedObject()
    {
        heldObject = gridManager.RemoveObjectAt(selectedObject.transform.position) as HoldableObject;
        player.IsHolding = true;

        itemGuide.InitCopiedImage(heldObject.GenerateImageCopy(itemGuide.transform.position));
        if(heldObject is TowerFSMBase)
            (heldObject as TowerFSMBase).SetHeldByPlayer(true);
        heldObject.GetComponent<Collider2D>().isTrigger = true;
    }

    public void PutDownObject()
    {
        gridManager.AddObjectAt(itemGuide.position, heldObject);
        player.IsHolding = false;

        heldObject.transform.position = gridManager.WorldPosToGridPos(itemGuide.position);
        heldObject.GetComponent<Collider2D>().isTrigger = false;
        if (heldObject is TowerFSMBase)
            (heldObject as TowerFSMBase).SetHeldByPlayer(false);
        heldObject = null;
        itemGuide.DestroyImageCopy();
    }

    public void ChargeTowerInFront(float chargeSpeed)
    {
        TowerFSMBase tower = selectedObject as TowerFSMBase;
        if(tower != null)
            tower.SetBattery(tower.Battery + chargeSpeed * Time.deltaTime);

    }
}
