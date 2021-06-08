using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveFlower : MonoBehaviour
{
    [SerializeField] LevelManager levelManager;
    [ReadOnly] [SerializeField] bool isDead = false;
    [SerializeField] int flowerCount;
    List<MonsterFSMBase> attackedMonsters;
    

    void Awake()
    {
        attackedMonsters = new List<MonsterFSMBase>();
        
        //hpBar.Init(hp, transform, new Vector2(0, 1f));
    }

    public void GetDamage(MonsterFSMBase monster, float damage)
    {
        /*if (hp.Value - damage <= 0)
        {
            isDead = true;
            damage = hp.Value;
        }
        hp.Value -= damage;
        levelManager.SucceedRate -= (damage / hp.MaxValue) * 100 / flowerCount;*/
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        MonsterFSMBase monster= collision.gameObject.GetComponent<MonsterFSMBase>();
        if (monster == null)
            return;
        if (attackedMonsters.Contains(monster))
            return;
        attackedMonsters.Add(monster);
        //GetDamage(monster, hp.MaxValue * 0.2f);
        monster.MultiplyMoveSpeed(2f);
    }
}
