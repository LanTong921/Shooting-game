using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 10;
    public Joystick joyStick;
    private CharacterController controller;
    private GameObject focusEnemy;
   
    void Start()
    {
         controller = GetComponent<CharacterController>();
    }

    
    void Update()
    {
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
        

    
}
