using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBullet : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    Transform targetTransform;
    float range;
    public void Fire(Transform targetTransform)
    {
        this.targetTransform = targetTransform;
        this.enabled = true;
    }

    private void Update()
    {
        Vector3 distanceVector = targetTransform.position - transform.position;
        float angleTowardsTarget = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angleTowardsTarget);
        transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, moveSpeed * Time.deltaTime);

    }
    public void CollideWithMonster(MonsterFSM monster)
    {
        Destroy(gameObject);
    }
}
