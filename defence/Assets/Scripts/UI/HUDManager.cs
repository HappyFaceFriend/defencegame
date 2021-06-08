using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    public GameObject FlowerHPBarPrefab { get { return flowerHpBarPrefab; } }
    public GameObject MonsterHpBarPrefab { get { return monsterHpBarPrefab; } }
    public GameObject TowerEnergyBarPrefab { get { return towerEnergyBarPrefab; } }

    [Header("StatusBar Prefabs")]
    [SerializeField] GameObject flowerHpBarPrefab;
    [SerializeField] GameObject monsterHpBarPrefab;
    [SerializeField] GameObject towerEnergyBarPrefab;
    public StatusBar InstantiateStatusBar(GameObject prefab)
    {
        StatusBar temp = Instantiate(prefab, transform).GetComponent<StatusBar>();
        return temp;
    }
}
