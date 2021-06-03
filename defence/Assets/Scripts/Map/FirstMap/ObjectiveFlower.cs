using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveFlower : MonoBehaviour
{
    [SerializeField] LevelManager levelManager;
    [SerializeField] StatusBar hpBar;
    [SerializeField] float maxHp;
    [ReadOnly] [SerializeField] float currentHp;
    [ReadOnly] [SerializeField] bool isDead = false;
    [SerializeField] int flowerCount;
    List<MonsterFSMBase> attackedMonsters;
    

    void Awake()
    {
        attackedMonsters = new List<MonsterFSMBase>();
        currentHp = maxHp;
        hpBar.Init(maxHp, transform, new Vector2(0, 1f));
    }

    public void GetDamage(MonsterFSMBase monster, float damage)
    {
        if (currentHp - damage <= 0)
        {
            isDead = true;
            damage = currentHp;
        }
        currentHp -= damage;
        hpBar.Value = currentHp;
        levelManager.SucceedRate -= (damage / maxHp) * 50;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
    MonsterFSMBase monster= collision.gameObject.GetComponent<MonsterFSMBase>();
        if (monster == null)
            return;
        if (attackedMonsters.Contains(monster))
            return;
        attackedMonsters.Add(monster);
        GetDamage(monster, maxHp * 0.2f);
        monster.MultiplyMoveSpeed(2f);
    }
}
