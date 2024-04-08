using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProjectileGun : MonoBehaviour
{
    //bullet
    public GameObject bullet;

    //bullet force
    public float shootForce, upwardForce;

    //Gun stats
    public float timeBetweenShooting, spread, realoadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    [SerializeField]
    int bulletsLeft, bulletsShot;

    //bools 
    bool shooting, readyToShoot, reloading;

    //Reference
    public Camera fpsCam;
    public Transform attackpoint;

    //Graphucs
    public GameObject muzzleFlash;
    public TextMeshProUGUI ammunitionDisplay;


    //bug fixing
    public bool allowInvoke = true;


    private void Awake()
    {
        //make sure magazine is full
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    private void Update()
    {
        MyInput();

        //Set ammo dispaly, if it exists
        if (ammunitionDisplay != null)
            ammunitionDisplay.SetText(bulletsLeft / bulletsPerTap + " / " + magazineSize / bulletsPerTap);

    }

     private void MyInput()
     {
        //Check if allowed to hold down button and take corresponding input
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);


        //Reloading
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();
        //Reload automatically when tying to shoot without ammo
        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0) Reload();

        //Shooting
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            //Set bullents shot to 0
            bulletsShot = 0;


            Shoot();
        }
     } 

    private void Shoot ()
    {
        readyToShoot = false;

        //Find the exact hit position using a raycast
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); //Just a ray through the middle of your
        RaycastHit hit;

        //check if ray hits something
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75); //Just a point far away from the player


        //Calculate direction from attackPoint to targetPoint
        Vector3 directionWithoutSpread = targetPoint - attackpoint.position;


        //Calucalte spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);


        //Calucalute new direction with spread 
        Vector3 direactionWithSpread = directionWithoutSpread + new Vector3(x,y,0);


        //Instantiate bullet/projectile
        GameObject curreBullet = Instantiate(bullet,attackpoint.position, Quaternion.identity);
        curreBullet.transform.forward = direactionWithSpread.normalized;


        //Add forces to bullet
        curreBullet.GetComponent<Rigidbody>().AddForce(direactionWithSpread.normalized * shootForce, ForceMode.Impulse);
        curreBullet.GetComponent<Rigidbody>().AddForce(fpsCam.transform.up * upwardForce, ForceMode.Impulse);

        //Instantiate muzzle flash, if you have one
        if (muzzleFlash != null)
            Instantiate(muzzleFlash, attackpoint.position, Quaternion.identity);
        
        bulletsLeft--;
        bulletsShot++;

        //Invoke resetShot fuction (if not already invoked)
        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
        }

        //if more than one bulletsPerTap make sure to repeat shoot function
        if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShooting);



    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", realoadTime);
    }

    private void ReloadFinshed()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }



}
