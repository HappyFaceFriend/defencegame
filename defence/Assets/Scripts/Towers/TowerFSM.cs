using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerFSM : FSMBase
{

    [SerializeField] LevelManager levelManager;
    [SerializeField] Transform gunTransform;

    [SerializeField] float range;
    MonsterFSM targetMonster;
    public enum AttackMode
    {
        FirstEnemy, LastEnemy, UntilKill
    }
    new void Awake()
    {
        base.Awake();
    }
    void Update()
    {
    }
    IEnumerator Idle()
    {
        do
        {
            targetMonster = levelManager.GetFirstMonsterInRange(gunTransform.position, range);
            if(targetMonster!= null)
            {
                SetState(State.Attack);
            }
            yield return null;
        } while (!isNewState);
    }
    IEnumerator Attack()
    {
        do
        {
            Vector3 distanceVector = targetMonster.transform.position - gunTransform.position;
            if(distanceVector.sqrMagnitude >= range*range)
            {
                SetState(State.Idle);
            }
            else
            {
                float angleTowardsTarget = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;
                gunTransform.rotation = Quaternion.Euler(0, 0, angleTowardsTarget);
            }
            yield return null;
        } while (!isNewState);
    }
}
