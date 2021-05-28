using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] Transform spawnPoint;
    [SerializeField] GameObject monsterPrefab;
    [SerializeField] RouteManager routeManager;

    List<MonsterFSM> monsterList;


    public MonsterFSM GetFirstMonsterInRange(Vector3 centerPosition, float range)
    {
        for(int i=0; i<monsterList.Count; i++)
        {
            if((centerPosition - monsterList[i].transform.position).sqrMagnitude <= range*range)
            {
                return monsterList[i];
            }
        }
        return null;

    }

    private void Awake()
    {
        monsterList = new List<MonsterFSM>();
    }
    private void Start()
    {
        StartLevel();
    }
    void StartLevel()
    {
        StartCoroutine(StartWave());
    }

    IEnumerator StartWave()
    {
        float interval = 2f;
        while(true)
        {
            GameObject newMonster = Instantiate(monsterPrefab, spawnPoint.position, Quaternion.identity);
            newMonster.GetComponent<RouteMovement>().routeManager = routeManager;
            monsterList.Add(newMonster.GetComponent<MonsterFSM>());
            yield return new WaitForSeconds(interval);
        }
        yield return null;
    }
}
