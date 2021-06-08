using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public float SucceedRate { get { return succeedRate; } set { succeedRate = value; } }


    [Header("References")]
    [SerializeField] string levelDataFilePath;
    [SerializeField] GridManager gridManager;
    [SerializeField] TowerMaker towerMaker;
    [SerializeField] HUDManager hudManager;
    [SerializeField] RouteManager[] routeManagers;
    [SerializeField] GameObject[] monsterPrefabs;

    [Header("Values")]
    [SerializeField] [ReadOnly] float succeedRate = 100;

    [Header("UI References")]
    //[SerializeField] Text succeedRate = 100;
    [SerializeField] TimeBoard timeBoard;
    [SerializeField] StartPanel startPanel;

    List<MonsterFSMBase> monsterList;

    LevelSpawnData levelSpawnData;
    int currentWave = 0;
    float leftTime = 0f;

    enum State { Ready, Wave, Rest}
    State currentState;

    public MonsterFSMBase GetFirstMonsterInRange(Vector3 centerPosition, float range)
    {
        for(int i=0; i<monsterList.Count; i++)
        {
            if(monsterList[i].CurrentState == MonsterFSMBase.State.Dead)
            {
                monsterList.Remove(monsterList[i]);
                i--;
                continue;
            }
            if((centerPosition - monsterList[i].transform.position).sqrMagnitude <= range*range)
            {
                return monsterList[i];
            }
        }
        return null;

    }

    private void Awake()
    {
        monsterList = new List<MonsterFSMBase>();
        levelSpawnData = new LevelSpawnData(levelDataFilePath);
    }
    private void Start()
    {
        towerMaker.transform.position = gridManager.WorldPosToGridPos(towerMaker.transform.position);
        currentState = State.Ready;
        //StartLevel();
    }
    public void StartLevel()
    {
        StartCoroutine(StartWave());
    }
    private void Update()
    {
        if(currentState != State.Ready)
        {
            leftTime -= Time.deltaTime;
            if (leftTime < 0)
                leftTime = 0;
            int time = (int)leftTime;
            timeBoard.Time = "" + (time / 60).ToString().PadLeft(2,'0') + ":" + (time % 60).ToString().PadLeft(2, '0');
        }
    }
    IEnumerator StartWave()
    {
        currentState = State.Wave;
        int spawnIndex = 0;
        leftTime = levelSpawnData.WaveDatas[currentWave].totalTime;
        timeBoard.Wave = currentWave;
        SpawnCall nextSpwan;
        while (spawnIndex < levelSpawnData.WaveDatas[currentWave].spawnCalls.Count)
        {
            nextSpwan = levelSpawnData.WaveDatas[currentWave].spawnCalls[spawnIndex];
            yield return new WaitForSeconds(nextSpwan.time);
            MonsterFSMBase newMonster = Instantiate(monsterPrefabs[nextSpwan.id], routeManagers[nextSpwan.routeNumber].SpawnPoint, Quaternion.identity).GetComponent<MonsterFSMBase>();
            newMonster.GetComponent<RouteMovement>().routeManager = routeManagers[nextSpwan.routeNumber];
            StatusBar hpBar = hudManager.InstantiateStatusBar(hudManager.MonsterHpBarPrefab);
            newMonster.Init(hpBar);
            monsterList.Add(newMonster);
            spawnIndex++;
        }
        yield return null;
    }
}
