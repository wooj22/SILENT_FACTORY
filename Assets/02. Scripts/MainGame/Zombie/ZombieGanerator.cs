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

    // ���� ���� ������ set
    private void PosSetting()
    {
        foreach (Transform child in pos)
        {
            posList.Add(child.position);
        }
    }

    // ���� ���� ������ get
    private Vector3 PosGetting()
    {
        int randomIndex = Random.Range(0, posList.Count);
        return posList[randomIndex];
    }

    // ������Ʈ Ǯ��
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

    // ���� Ȱ��ȭ
    private void ActivaingZombie()
    {
        for (int i = 0; i < activeZombieMaxCount; i++)
        {
            zombieList[i].SetActive(true);
        }
    }

    // ���� ��Ȱ��ȭ (30�� �ֱ�)
    private void ReActivaingZombie()
    {
        // ���� zombieList �ȿ� Ȱ��ȭ�Ǿ��ִ� ���� activeZombieMaxCount���� �۴ٸ�, �׸�ŭ �ٽ� Ȱ��ȭ
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
