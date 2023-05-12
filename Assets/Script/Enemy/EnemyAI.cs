using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    //[SerializeField] PlayerBehaviour pb;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform player;
    [SerializeField] CompassIcon icon;
    [SerializeField] LayerMask groundLayer, playerLayer;
    [SerializeField] GameObject bullet;
    [SerializeField] float bulletSpeed;
    [SerializeField] Transform attackPoint;
    [SerializeField] GameObject muzzleFlash;

    [SerializeField]  public float health;
    [SerializeField] float damage;

    [SerializeField] Vector3 walkPoint;
    [SerializeField] float walkPointRange;
    [SerializeField] float pauseTime;
    bool walkPointSet;
    bool isPatroling;

    [SerializeField] float timeBetweenAttacks;
    bool isAttacking;

    [SerializeField] float sightRange, attackRange;
    bool playerInSight, playerInAttack;

    GameObject temp;

    MovingBar bar;
    Canvas can;

    [Header("Wwise")]
    [SerializeField] AK.Wwise.Event enemyShootEvent;
    [SerializeField] AK.Wwise.Event enemyDeadEvent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        //pb = GameObject.Find("PlayerController/Player").GetComponent<PlayerBehaviour>();
        can = GetComponentInChildren<Canvas>();
        bar = GetComponentInChildren<MovingBar>();
        icon = GetComponent<CompassIcon>();
        can.enabled = false;
        
    }

    private void Update()
    {
        //Debug.Log(health);
        playerInSight = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        playerInAttack = Physics.CheckSphere(transform.position, attackRange, playerLayer);
        //if (playerInSight) Debug.Log("enemy nesrby");

        if (!playerInSight && !playerInAttack && !isPatroling)
            //StartCoroutine(Patrol());
            Patrol();
        else if (playerInSight && !playerInAttack)
        {
            //Physics.Raycast(transform.position, player.position - transform.position, out RaycastHit rayhit, sightRange);
            //if (rayhit.collider.CompareTag("Player"))
            //    Chase();
            //else
            //    Patrol();
            Chase();
        }
        else if (playerInSight && playerInAttack)
        {
            //Physics.Raycast(transform.position, player.position, out RaycastHit rayhit, sightRange);
            //if (rayhit.collider.CompareTag("Player"))
            //    Attack();
            //else
            //    Patrol();
            Attack();
        }

        
    }

    private void Patrol()
    {
        if (!walkPointSet) SearchNextPoint();
        else if (walkPointSet)
        {
            temp = new GameObject("Temp");
            temp.transform.position = walkPoint;
            agent.SetDestination(walkPoint);
            transform.LookAt(temp.transform);
            Destroy(temp);
        }
        Vector3 walkPointDistance = transform.position - walkPoint;

        if (walkPointDistance.magnitude < 1)
            //StartCoroutine(WaitRightThere());
            walkPointSet = false;
    }

    //private IEnumerator WaitRightThere()
    //{
    //    yield return new WaitForSeconds(pauseTime);
    //    walkPointSet = false;
    //}

    //private IEnumerator Patrol()
    //{
    //    Debug.Log("Patroling");
    //    isPatroling = true;

    //    yield return new WaitForSeconds(pauseTime);

    //    if (!walkPointSet)
    //        SearchNextPoint();
    //    else if (walkPointSet)
    //    {
    //        temp = new GameObject("Temp");
    //        temp.transform.position = walkPoint;
    //        agent.SetDestination(walkPoint);
    //        transform.LookAt(temp.transform);
    //        Destroy(temp);
    //    }

    //    Vector3 walkPointDistance = transform.position - walkPoint;

    //    if (walkPointDistance.magnitude < 1)
    //    {
    //        walkPointSet = false;
    //        isPatroling = false;
    //    }

    //}

    private void Chase()
    {
        //Debug.Log("Chase");
        agent.SetDestination(player.position);
    }

    private void Attack()
    {
        //Debug.Log("Attack");
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if(!isAttacking)
        {
            //Rigidbody rb = Instantiate(bullet, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            //rb.AddForce(transform.forward * bulletSpeed, ForceMode.Impulse);
            //rb.AddForce(transform.up * bulletSpeed / 4, ForceMode.Impulse);


            if (Physics.Raycast(attackPoint.position, player.position - attackPoint.position, out RaycastHit attackHit, attackRange))
            {
                //Debug.Log(attackHit.transform.gameObject.name);
                //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward * 10), Color.red);
                enemyShootEvent.Post(gameObject);
                if (attackHit.collider.CompareTag("Player"))
                {

                    attackHit.transform.gameObject.GetComponentInParent<PlayerBehaviour>().TakeDamage(damage);
                    
                    Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity, transform);
                }
            }

            isAttacking = true;
            Invoke(nameof(AttackReset), timeBetweenAttacks);
        }
    }

    private void AttackReset()
    {
        isAttacking = false;
    }

    private void SearchNextPoint()
    {
        //Debug.Log("Searching Next Point");
        float zRandom = Random.Range(-walkPointRange, walkPointRange);
        float xRandom = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + xRandom, transform.position.y, transform.position.z + zRandom);
        

        if (Physics.Raycast(walkPoint, -transform.up, 2, groundLayer)) walkPointSet = true;
    }

    public void TakeDamage(float damage)
    {
        can.enabled = true;
        health -= damage;
        SetHealthBar();
        if (health <= 0) Invoke(nameof(EnemyDie), 0.5f);
    }

    public void SetHealthBar()
    {
        bar.SetValue(health);
    }


    private void OnEnable()
    {
        //icon.enabled = true;
        icon.Add();
    }

    private void EnemyDie()
    {
        //Destroy(this.gameObject);
        icon.Remove();
        enemyDeadEvent.Post(gameObject);
        //icon.enabled = false;
        this.gameObject.SetActive(false);
    }

}
