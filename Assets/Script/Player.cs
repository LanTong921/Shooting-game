using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 10;
    public Joystick joyStick;
    public Transform firePoint;
    public GameObject bulletPrefab;

    private CharacterController controller;

    private GameObject focusEnemy;
   
    void Start()
    {
         controller = GetComponent<CharacterController>();

         StartCoroutine(KeepShooting());
    }

    
    void Update()
    {
         // 找到最近的一個目標 Enemy 的物件
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");

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


         float h = joyStick.Horizontal;
         float v = joyStick.Vertical;

         Vector3 dir = new Vector3(h, 0, v);

        if (dir.magnitude > 0.1f)
        {
            float faceAngle = Mathf.Atan2(h, v) * Mathf.Rad2Deg;

            Quaternion targetRotation = Quaternion.Euler(0, faceAngle, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 0.3f);
        }

        else
        {
            
            if (focusEnemy)
            {
                var targetRotation = Quaternion.LookRotation(focusEnemy.transform.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 20 * Time.deltaTime);
            }
        }

        
        if (!controller.isGrounded)
        {
            dir.y = -9.8f * 30 * Time.deltaTime;
        }

         controller.Move(dir * speed * Time.deltaTime);
    }

    
     void Fire()
    {
        Instantiate(bulletPrefab, firePoint.transform.position, transform.rotation);
    }


    IEnumerator KeepShooting()
    {
        while (true)
        {
           
            Fire();

            
            yield return new WaitForSeconds(0.4f);
        }
    }
}
