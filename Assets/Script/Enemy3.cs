using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3 : MonoBehaviour
{
     public Transform firePoint;
     public GameObject bulletPrefab;
     private GameObject focusEnemy;
     private float hp = 100f;

    void Start()
    {
         StartCoroutine(KeepShooting());
    }

    
    void Update()
    {
         GameObject[] enemys = GameObject.FindGameObjectsWithTag("Player");

         float miniDist = 9999;
         foreach (GameObject enemy in enemys)
         {
            // 計算距離
            float d = Vector3.Distance(transform.position, enemy.transform.position);

            // 跟上一個最近的比較，有比較小就記錄下來
            if (d < miniDist)
            {
                miniDist = d;
                focusEnemy = enemy;
            }
        }

        if (focusEnemy)
            {
                var targetRotation = Quaternion.LookRotation(focusEnemy.transform.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 20 * Time.deltaTime);
            }
    }

     void Fire()
    {
        // 產生出子彈
        Instantiate(bulletPrefab, firePoint.transform.position, transform.rotation);
    }


    // 一直射擊的 Coroutine 函式
    IEnumerator KeepShooting()
    {
        while (true)
        {
            // 射擊
            Fire();

            // 暫停 0.5 秒
            yield return new WaitForSeconds(0.5f);
        }
    }
     private void OnTriggerEnter(Collider other)
    {
       
        if (other.tag == "Bullet")
        {

            Bullet bullet = other.GetComponent<Bullet>();

           
            hp -= bullet.atk;

           
            if (hp <= 0)
            {
                gameObject.SetActive(false);
                Destroy(gameObject);
            }
        }
    }
}
