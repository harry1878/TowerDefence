using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Get { get; set; } = null;

    public GameObject[] Prefabs;
    public float RoundWaitTime = 10f; //끝나고 대기할 시간
    public float SpawnTime = 1.5f; // 한마리 뽑을때 마다 대기할 시간
    public float SpawnLimit = 10f; // 최대 몬스터 개수 

    //유니티와 시리얼라이즈를 하지말라는 속성
    [NonSerialized] public int count = 0;
    private int monsterCount = 0;
    private void Awake()
    {
        Get = this;
    }

    public void OnStart()
    {
        StartCoroutine(UpdateSpawnTimer(SpawnTime));
    }

    private IEnumerator UpdateSpawnTimer(float checkTime)
    {
        float currTime;
        while (monsterCount != SpawnLimit)
        {
            currTime = Time.time;
            while (Time.time - currTime <= checkTime)
                yield return null; //한 프레임 쉬고 다시 실행

            SpawnEnemy(0);
        }
        yield break;
    }

    private void SpawnEnemy(int index)
    {
        GameObject g = Instantiate(
            Prefabs[index],
            WayPointManager.Get.GetWaypoint(0).position,
            Quaternion.identity,
            null);

        monsterCount++;
    }
}
