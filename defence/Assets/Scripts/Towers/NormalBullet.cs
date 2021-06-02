using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBullet : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    Transform targetTransform;
    float range;
    float attack;
    public void Fire(Transform targetTransform, float attack)
    {
        this.targetTransform = targetTransform;
        this.enabled = true;
        this.attack = attack;
    }

    private void Update()
    {
        Vector3 distanceVector = targetTransform.position - transform.position;
        float angleTowardsTarget = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angleTowardsTarget);
        transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, moveSpeed * Time.deltaTime);

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        MonsterFSMBase monster = collision.gameObject.GetComponent<MonsterFSMBase>();
        if (monster == null)
            return;
        monster.TakeDamage(attack);
        Destroy(gameObject);
    }
}
