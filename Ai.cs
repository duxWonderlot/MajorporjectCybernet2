using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//this class is for everycombat ai in the game
public class Ai : MonoBehaviour
{
    public float health = 50.0f;
    public NavMeshAgent nav;
    [SerializeField]
    private Transform target;
    [SerializeField]
    public float attackradius = 4.0f,snipingradius =5.0f;
    public bool istrue = false,istrue1 =false;
    public GameObject returnpoint;
    private float Range = 1000.0f;
    public Transform []petrolpoints;
    public Vector3 newpos,newpos1;
    int n=0, shots;
    public float lookradius =20.0f;
    public float bulletspeed = 10.0f;
    public Enemybul bullet1;
    public Transform Sp;
    public bool SnipeMode= false;
    public bool othermades = false;
    public Transform[] coverpoints;
    // public GameObject player;

    private void Start()
    {
        this.nav = GetComponent<NavMeshAgent>();
        // player = GameObject.FindGameObjectWithTag("Player"); 
        istrue = false;     
        
    }
    public void TakeDamage( float amount) {

        health = -amount;
        if (health <= 0f) {
            Die();
        }

    }

    public void Die() {

        this.gameObject.SetActive(false);
    }


    private void FixedUpdate()
    {

        Sniping(SnipeMode);
        if (othermades)
        {
            float distance = Vector3.Distance(target.position, this.transform.position);
            if (distance <= lookradius)
            {
                nav.stoppingDistance = 20;
                nav.SetDestination(target.position);

                if (distance <= nav.stoppingDistance)
                {
                    //Attack
                    print("Attack!");
                    Shoot();
                    Facetarget();
                    ConverBfAttack();  
                } 
            }
            else
            {
                nav.stoppingDistance = 0;
                Petrolling();
            }

            if (Input.GetKeyDown(KeyCode.Q)) // this will turn to shooting button
            {
                shots += 1;
                if (shots >= 6)
                {
                    istrue = !istrue;
                    shots = 0;
                }
            }
            else if (istrue == false)
            {
                Petrolling();
            }
            else if (istrue == true)
            {
                Spotted();
            }
            print(shots);
        }
    }


    void Facetarget() {

        Vector3 direction = (target.position - this.transform.position).normalized;
        Quaternion lookbro = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookbro, Time.deltaTime * 3.0f);
    }
  public bool Spotted() {   // sound detection in the area
        

        float distancetoplayer = Vector3.Distance(target.transform.position , this.transform.position);
        if (!this.nav.pathPending && attackradius <= distancetoplayer)
        {
            Spotwitharay();
            Vector3 targetvec = target.transform.position;
            this.nav.SetDestination(targetvec);
        }
        else if(attackradius >= distancetoplayer)
        {
            this.nav.SetDestination(petrolpoints[n].transform.position);

        }
        if (nav == null)
        {
            print("nav not attached!");
        }
        else
        {

            //Setdesination();
        }

        return false;

    }

    void ConverBfAttack()
    {
        float changpos = 0;
        changpos += 4 * Time.deltaTime;
        int rm = UnityEngine.Random.Range(0, 3);
        newpos1 = coverpoints[rm].position;
        if(changpos >= 10)
        {
            rm = UnityEngine.Random.Range(0, 2);
        } 
    }
    void Sniping(bool isActive)
    {
        if (isActive)
        {
            float playerspotteddistance = Vector3.Distance(target.transform.position, this.transform.position);

            if (attackradius >= playerspotteddistance)
            {
                this.transform.LookAt(target.transform.position);
                Shoot();
            }
            else {

                //this.transform.rotation = Quaternion.identity;
            }
        }

    }


    void Petrolling() {
       
        print("petrolling.....");
        if (this.nav.remainingDistance < 0.6f && !this.nav.pathPending)
        {
            n = (n + 1) % petrolpoints.Length;
            newpos = petrolpoints[n].transform.position;
            this.nav.SetDestination(newpos);
            
        }
        print("look at this shit---->"+n);
       
    }

    void Shoot()
    {
        Enemybul newbul = Instantiate(bullet1, Sp.position, Sp.rotation) as Enemybul;
        newbul.bspeed = bulletspeed;
    }

    void Spotwitharay() {
        RaycastHit hit;
       
        if (Physics.Raycast(this.transform.position, this.transform.forward, out hit, Range)) {

            if (hit.collider.gameObject.tag == "Player") {
              Vector3 target2 = target.transform.position;
                this.nav.SetDestination(target2);
                print("hows there!");

            }

        }

        Debug.DrawRay(this.transform.position, this.transform.forward, Color.yellow, Range);
    }

    void Behaviour() {
       //Cover and Shoot from nearstpoint


       //Shoot
       //callback
    }
}

