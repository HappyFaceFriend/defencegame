using System.Collections;
using UnityEngine;

public class TowerMaker : InteractableObject
{
    [Header("References")]
    [SerializeField] GameObject towerPrefab;
    [SerializeField] GridManager gridManager;
    [SerializeField] LevelManager levelManager;
    [SerializeField] Transform towerTable;
    [SerializeField] HUDManager hudManager;

    [Header("Stats")]
    [SerializeField] float animDuration;

    TowerFSMBase movingTower;
    Animator animator;
    bool isGenerating;
    bool isTowerSet;
    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        isGenerating = false;
        isTowerSet = false;
        animator.speed = 0;
    }
    private void Start()
    {
        CreateTower();
        gridManager.SetBackgroundAt(towerTable.position, GridBackground.Wall);
        gridManager.AddObjectAt(transform.position, this);
    }
    IEnumerator CreateTowerCoroutine()
    {
        float offset = 1.5f;
        movingTower = Instantiate(towerPrefab, towerTable.position + new Vector3(0,offset,0), Quaternion.identity).GetComponent<TowerFSMBase>();
        animator.speed = 1;
        while(movingTower.transform.position.y > towerTable.position.y)
        {
            yield return null;
            movingTower.transform.Translate(new Vector3(0, -5/animDuration/16, 0) * Time.deltaTime);
        }
        animator.speed = 0;
        movingTower.transform.position = towerTable.position;
        
        movingTower.Init(levelManager, hudManager.InstantiateStatusBar(hudManager.TowerEnergyBarPrefab));
        gridManager.AddObjectAt(towerTable.position, movingTower);
        isTowerSet = true;
        isGenerating = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isTowerSet)
        {
            if (gridManager.GetObjectAt(movingTower.transform.position) == null)
            {
                isTowerSet = false;
            }
        }
    }
    void CreateTower()
    {
        if (!isGenerating && !isTowerSet)
        {
            isGenerating = true;
            StartCoroutine(CreateTowerCoroutine());
        }
    }
    public override void Interact()
    {
        CreateTower();
    }
}
