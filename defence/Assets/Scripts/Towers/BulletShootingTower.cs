using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShootingTower : TowerFSMBase
{
    [Header("BulletShootingTower References")]
    [SerializeField] Transform gunTransform;
    [SerializeField] GameObject bulletPrefab;

    [Header("BulletShootingTower Stats")]
    [SerializeField] float shootInterval;
    [SerializeField] float range;
    [SerializeField] float attack;

    MonsterFSMBase targetMonster;

    float eTime;
    new void Awake()
    {
        base.Awake();
        eTime = shootInterval;
    }
    IEnumerator Idle()
    {
        do
        {
            if (eTime < shootInterval)
                eTime += Time.deltaTime;
            targetMonster = levelManager.GetFirstMonsterInRange(gunTransform.position, range);

            if (targetMonster != null)
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
            if (targetMonster.CurrentState == MonsterFSMBase.State.Dead)
            {
                SetState(State.Idle);
                continue;
            }
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
                if(eTime >= shootInterval && Battery > 0)
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
        temp.GetComponent<NormalBullet>().Fire(targetMonster.transform, attack);
        SetBattery(Battery - 25);
    }
}
