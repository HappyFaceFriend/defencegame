using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] string levelDataFilePath;
    [SerializeField] GameObject monsterPrefab;
    [SerializeField] RouteManager[] routeManagers;
    
    List<MonsterFSM> monsterList;

    LevelSpawnData levelSpawnData;
    int currentWave = 0;

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
        levelSpawnData = new LevelSpawnData(levelDataFilePath);
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
        int spawnIndex = 0;
        SpawnCall nextSpwan;
        while (spawnIndex < levelSpawnData.WaveDatas[currentWave].spawnCalls.Count)
        {
            nextSpwan = levelSpawnData.WaveDatas[currentWave].spawnCalls[spawnIndex];
            yield return new WaitForSeconds(nextSpwan.time);
            GameObject newMonster = Instantiate(monsterPrefab, routeManagers[nextSpwan.routeNumber].SpawnPoint, Quaternion.identity);
            newMonster.GetComponent<RouteMovement>().routeManager = routeManagers[nextSpwan.routeNumber];
            monsterList.Add(newMonster.GetComponent<MonsterFSM>());
            spawnIndex++;
        }
        yield return null;
    }
}
