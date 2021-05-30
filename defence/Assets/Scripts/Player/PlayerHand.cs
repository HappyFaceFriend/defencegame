using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] Vector3 sidePos;
    [SerializeField] Vector3 upPos;
    [SerializeField] Vector3 downPos;
    [SerializeField] PlayerFSM player;
    [SerializeField] GridManager gridManager;

    [SerializeField] ItemGuide itemGuide;

    [SerializeField] float batteryChargeSpeed;

    GridObject heldObject;
    GridObject selectedObject;

    void Awake()
    {
    }
    public void SpacePressed()
    {
        if (heldObject == null && selectedObject != null)
            HoldSelectedObject();
        else if (heldObject != null && selectedObject == null)
            PutDownObject();
    }
    private void Update()
    {
        if(Input.GetKey(KeyCode.C))
        {
            if(selectedObject.Type == GridObject.ItemType.Tower)
            {
                if (player.CurrentOtherState != PlayerFSM.OtherState.BatteryCharging)
                    player.SetState(PlayerFSM.OtherState.BatteryCharging);
                selectedObject.TowerComponent.SetBattery(selectedObject.TowerComponent.Battery + batteryChargeSpeed * Time.deltaTime);
            }
        }
        if(Input.GetKeyUp(KeyCode.C))
        {
            if (player.CurrentOtherState == PlayerFSM.OtherState.BatteryCharging)
                player.SetState(PlayerFSM.State.Idle);
        }

        //detect item at direction
        itemGuide.position = gridManager.WorldPosToGridPos(VectorUtils.Vec3toVec2(player.transform.position) + player.LastInputVector);
        GridObject newObject = gridManager.GetObjectAt(itemGuide.position);
        if(newObject != selectedObject)
        {
            if(selectedObject != null)
                selectedObject.SetSelected(false);
            selectedObject = newObject;
            if (selectedObject != null && heldObject == null)
                selectedObject.SetSelected(true);
        }

        //change how the player holds item as direction changes (visuals)
        if (player.LastInputVector.y == 1)
            transform.localPosition = upPos;
        else if (player.LastInputVector.y == -1)
            transform.localPosition = downPos;
        if (player.LastInputVector.x != 0)
            transform.localPosition = sidePos;

        if (heldObject != null)
        {
            heldObject.transform.position = transform.position;
        }

    }


    public void HoldSelectedObject()
    {
        gridManager.RemoveObjectAt(selectedObject.transform.position);
        heldObject = selectedObject;
        player.SetIsHolding(true);

        itemGuide.sprite=heldObject.spriteRenderer.sprite;
        if(heldObject.Type == GridObject.ItemType.Tower)
            heldObject.TowerComponent.SetHeldByPlayer(true);

        heldObject.spriteRenderer.transform.position += new Vector3(0, 0, -0.3f);
        heldObject.GetComponent<Collider2D>().isTrigger = true;
    }

    public void PutDownObject()
    {
        gridManager.PutObjectAt(itemGuide.position, heldObject);
        heldObject.transform.position = gridManager.WorldPosToGridPos(itemGuide.position);
        heldObject.spriteRenderer.transform.position -= new Vector3(0, 0, -0.3f);
        heldObject.GetComponent<Collider2D>().isTrigger = false;
        if (heldObject.Type == GridObject.ItemType.Tower)
            heldObject.TowerComponent.SetHeldByPlayer(true);
        heldObject = null;
        player.SetIsHolding(false);

        itemGuide.sprite = null;
    }
}
