using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitGenerator : MonoBehaviour
{
    [SerializeField] GameObject kitPrefab;
    [SerializeField] Transform pos;
    [SerializeField] int kitMaxCount;
    [SerializeField] Transform parents;

    public List<GameObject> kitList = new List<GameObject>();
    public List<Vector3> posList = new List<Vector3>();

    private void Start()
    {
        PosSetting();
        InvokeRepeating(nameof(KitCreating), 0f, 30f);
    }

    // 키트 생성 포지션 set
    private void PosSetting()
    {
        foreach (Transform child in pos)
        {
            posList.Add(child.position);
        }
    }

    // 키트 생성 (30초마다 재생성)
    private void KitCreating()
    {
        for (int i = 0; i < kitMaxCount; i++)
        {
            if(kitList[i] == null)
            {
                GameObject kit = Instantiate(kitPrefab, posList[i], Quaternion.identity);
                kit.transform.SetParent(parents);
                kitList[i] = kit;
            }
        }
    }
}
