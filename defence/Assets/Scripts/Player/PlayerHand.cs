using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    public bool IsHoldingObject { get { return heldObject != null; } }
    public bool IsObjectInFront { get { return selectedObject != null; } }
    
    public GridObject SelectedObject {  get { return selectedObject; } }
    [Header("References")]
    [SerializeField] PlayerFSM player;
    [SerializeField] GridManager gridManager;
    [SerializeField] ItemGuide itemGuide;
    [Header("Holding Positions")]
    [SerializeField] Vector3 sidePos;
    [SerializeField] Vector3 upPos;
    [SerializeField] Vector3 downPos;


    GridObject heldObject;
    GridObject selectedObject;

    Vector2Int cellInFront;

    void Awake()
    {
        cellInFront = gridManager.WorldPosToGridIndex(player.transform.position) + Vector2Int.RoundToInt(player.LastInputVector);
    }
    private void Update()
    {
        //detect item at direction
        cellInFront = gridManager.WorldPosToGridIndex(player.transform.position) + Vector2Int.RoundToInt(player.LastInputVector);
        itemGuide.position = gridManager.GridIndexToWorldPos(cellInFront);
        selectedObject = gridManager.GetObjectAt(cellInFront);
        //TODO : currently ignoring add-ons to tower
        itemGuide.SetState(IsObjectInFront, IsObjectInFront && !IsHoldingObject || !IsObjectInFront && IsHoldingObject);
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
        heldObject = gridManager.RemoveObjectAt(selectedObject.transform.position);
        player.IsHolding = true;

        itemGuide.InitCopiedImage(heldObject.GenerateImageCopy(itemGuide.transform));
        if(heldObject.Type == GridObject.ItemType.Tower)
            heldObject.TowerComponent.SetHeldByPlayer(true);
        heldObject.GetComponent<Collider2D>().isTrigger = true;
    }

    public void PutDownObject()
    {
        gridManager.PutObjectAt(itemGuide.position, heldObject);
        player.IsHolding = false;

        heldObject.transform.position = gridManager.WorldPosToGridPos(itemGuide.position);
        heldObject.GetComponent<Collider2D>().isTrigger = false;
        if (heldObject.Type == GridObject.ItemType.Tower)
            heldObject.TowerComponent.SetHeldByPlayer(false);
        heldObject = null;

        Destroy(itemGuide.CopiedImage.gameObject);
        itemGuide.CopiedImage = null;
    }

    public void ChargeTowerInFront(float chargeSpeed)
    {
        if(selectedObject.Type == GridObject.ItemType.Tower)
            selectedObject.TowerComponent.SetBattery(selectedObject.TowerComponent.Battery + chargeSpeed * Time.deltaTime);

    }
}
