using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageGun : MonoBehaviour
{
    public float Damge;
    public float BulletRange;
    private Transform PlayerCamera;


    private void Start()
    {
        PlayerCamera = Camera.main.transform;
    }

    public void Shoot()
    {
        Ray gunRay = new Ray(PlayerCamera.position, PlayerCamera.forward);
        if (Physics.Raycast(gunRay,out RaycastHit hitInfO, BulletRange))
        {
            if (hitInfO.collider.gameObject.TryGetComponent(out Entity enemy))
            {
                enemy.Health -= Damge;
            }
        }
    }




}
