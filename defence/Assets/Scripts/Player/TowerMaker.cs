using System.Collections;
using UnityEngine;

public class TowerMaker : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject towerPrefab;
    [SerializeField] GridManager gridManager;
    [SerializeField] LevelManager levelManager;
    [SerializeField] GridObject button;
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
        gridManager.PutObjectAt(button.transform.position, button);
    }
    public void CreateTower()
    {
        if (!isGenerating && !isTowerSet)
        {
            isGenerating = true;
            StartCoroutine(CreateTowerCoroutine());
        }
    }
    IEnumerator CreateTowerCoroutine()
    {
        float offset = 1.5f;
        movingTower = Instantiate(towerPrefab, transform.position + new Vector3(0,offset,0), Quaternion.identity).GetComponent<TowerFSMBase>();
        animator.speed = 1;
        while(movingTower.transform.position.y > transform.position.y)
        {
            yield return null;
            movingTower.transform.Translate(new Vector3(0, -5/animDuration/16, 0) * Time.deltaTime);
        }
        animator.speed = 0;
        movingTower.transform.position = transform.position;
        movingTower.SetReferences(levelManager);
        gridManager.PutObjectAt(movingTower.transform.position, movingTower.GetComponent<GridObject>());
        isTowerSet = true;
        isGenerating = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isTowerSet)
        {
            if (gridManager.GetObjectAt(movingTower.transform.position) == null)
                isTowerSet = false;
        }
    }
}
