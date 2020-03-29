﻿using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Get { get; set; } = null;

    public GameObject[] Prefabs;
    public Material PrefabMaterial;
    public GameObject HealthBar;
    public float RoundWaitTime = 10f; //끝나고 대기할 시간
    public float SpawnTime = 1.5f; // 한마리 뽑을때 마다 대기할 시간
    public float SpawnLimit = 10f; // 최대 몬스터 개수 
    public Text TextButton;
    public Slider NextSlider;

    //유니티와 시리얼라이즈를 하지말라는 속성
    [NonSerialized] public int roundCount = 1;

    //List?
    //배열처럼 처음 선언할때 개수에 정함이 없고, 삽입, 삭제가 용이한 클래스
    private List<Enemy> enemies = new List<Enemy>();

    public Enemy GetEnemyInRange(Vector3 position,float range)
    {
        for(int i=0;i < enemies.Count;++i)
        {
            float distance = 
                Vector3.Distance(
                enemies[i].transform.position,
                position);

            if (distance <= range) return enemies[i];
        }
        return null;
    }

    public void DestroyEnemy(Enemy enemy)
    {
        enemies.Remove(enemy);
    }

    private void Awake()
    {
        Get = this;
    }

    public void OnStart()
    {
        //StartCoroutine?

        StartCoroutine(UpdateSpawnTimer(SpawnTime));
    }

    // 반복자
    private IEnumerator UpdateSpawnTimer(float checkTime)
    {
        TextButton.text = "In Coming ..";
        PrefabMaterial.color = new Color(
            UnityEngine.Random.Range(0f, 1f),
            UnityEngine.Random.Range(0f, 1f),
            UnityEngine.Random.Range(0f, 1f));

        int enemyType = UnityEngine.Random.Range(0, Prefabs.Length);

        int count = 0;
        float currTime;
        while (count++ != SpawnLimit)
        {
            currTime = Time.time;
            while (Time.time - currTime <= checkTime)
            {
                yield return null; //한 프레임 쉬고 다시 실행
            }

            SpawnEnemy(enemyType);
           
        }

        while (enemies.Count != 0)
            yield return null;

        roundCount++;
        TextButton.text = string.Format("Next {0} Round", roundCount);
        NextSlider.value = 0;

        currTime = Time.time;
        while(Time.time -currTime <= RoundWaitTime)
        {
            NextSlider.value = (Time.time - currTime) / RoundWaitTime;
            yield return null;
        }

        StartCoroutine(UpdateSpawnTimer(SpawnTime));
        yield break;
    }

    private void SpawnEnemy(int index)
    {
        //Instantiate 
        //복사본을 생성한다
        GameObject g = Instantiate(
            Prefabs[index],   //복사할 대상
            WayPointManager.Get.GetWaypoint(0).position,   //웨이포인트의 첫 번쨰 위치로 이동
            Quaternion.identity,    //회전값을 초기화한다(0, 0, 0)
            null);

        Enemy e = g.GetComponent<Enemy>();
        e.SetParameter(roundCount);

        e.HealthBar = Instantiate(HealthBar, null).GetComponent<RectTransform>();

        enemies.Add(e);

        //Quaternion.identity
        //단위행렬 
        // Ex:4x4 기준
        //1 0 0 0
        //0 1 0 0
        //0 0 1 0
        //0 0 0 1
    }
}
