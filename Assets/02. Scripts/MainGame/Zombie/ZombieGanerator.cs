using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieGanerator : MonoBehaviour
{
    [SerializeField] GameObject zombiePrefab;
    [SerializeField] Transform pos;
    [SerializeField] Transform parents;
    [SerializeField] int pullingCount;
    [SerializeField] int activeZombieMaxCount;

    public List<GameObject> zombieList = new List<GameObject>();
    public List<Vector3> posList = new List<Vector3>();

    private void Start()
    {
        PosSetting();
        ObjectPulling();
        ActivaingZombie();
        InvokeRepeating(nameof(ReActivaingZombie), 5f, 5f);
    }

    // 좀비 생성 포지션 set
    private void PosSetting()
    {
        foreach (Transform child in pos)
        {
            posList.Add(child.position);
        }
    }

    // 좀비 생성 포지션 get
    private Vector3 PosGetting()
    {
        int randomIndex = Random.Range(0, posList.Count);
        return posList[randomIndex];
    }

    // 오브젝트 풀링
    private void ObjectPulling()
    {
        for(int i =0; i< pullingCount; i++)
        {
            GameObject zombie = Instantiate(zombiePrefab, PosGetting(), Quaternion.identity);
            zombie.transform.SetParent(parents);
            zombie.SetActive(false);
            zombieList.Add(zombie);
        }
    }

    // 좀비 활성화
    private void ActivaingZombie()
    {
        for (int i = 0; i < activeZombieMaxCount; i++)
        {
            zombieList[i].SetActive(true);
        }
    }

    // 좀비 재활성화 (30초 주기)
    private void ReActivaingZombie()
    {
        // 만약 zombieList 안에 활성화되어있는 좀비가 activeZombieMaxCount보다 작다면, 그만큼 다시 활성화
        int activeCount = zombieList.FindAll(z => z.activeInHierarchy).Count;

        for (int i = 0; i < zombieList.Count && activeCount < activeZombieMaxCount; i++)
        {
            if (!zombieList[i].activeInHierarchy)
            {
                zombieList[i].transform.position = PosGetting();
                zombieList[i].SetActive(true);
                activeCount++;
            }
        }
    }
}
