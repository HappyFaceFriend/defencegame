using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerFSM : FSMBase
{

    [SerializeField] LevelManager levelManager;
    [SerializeField] Transform gunTransform;

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float shootInterval;

    [SerializeField] float range;
    [SerializeField] bool worksWhileHolding;
    
    MonsterFSM targetMonster;
    bool isHeld;

    float eTime;
    public enum AttackMode
    {
        FirstEnemy, LastEnemy, UntilKill
    }
    new void Awake()
    {
        base.Awake();
        eTime = shootInterval;
    }
    void Update()
    {
    }
    public void SetHeldByPlayer(bool isHeld)
    {
        this.isHeld = isHeld;
        if (isHeld)
            SetState(State.Idle);
    }
    IEnumerator Idle()
    {
        do
        {
            if (eTime < shootInterval)
                eTime += Time.deltaTime;
            targetMonster = levelManager.GetFirstMonsterInRange(gunTransform.position, range);
            
            if(!isHeld || worksWhileHolding)
            {
                if (targetMonster != null)
                {
                    SetState(State.Attack);
                }
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
                eTime += Time.deltaTime;
                if(eTime >= shootInterval)
                {
                    eTime -= shootInterval;
                    Shoot();
                }
            }
            yield return null;
        } while (!isNewState);
    }
    void Shoot()
    {
        GameObject temp = Instantiate(bulletPrefab, gunTransform.position, Quaternion.identity);
        temp.GetComponent<NormalBullet>().Fire(targetMonster.transform);
    }
}
