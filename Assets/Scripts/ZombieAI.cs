using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public Animator animator;

    public float chaseRange = 10f;
    public float attackRange = 1.5f;
    public float attackCooldown = 1.5f;
    private float nextAttackTime;

    private Health playerHealth;

    [Header("Audio")]
    public AudioSource attackAudio;      //  plays attack sounds
    public AudioSource footstepAudio;    // plays footsteps

    void Start()
    {
        if (!agent) agent = GetComponent<NavMeshAgent>();
        if (!animator) animator = GetComponent<Animator>();

        //find player + health
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
        {
            player = p.transform;
            playerHealth = p.GetComponent<Health>();
        }

        //footsteps off if idle
        if (footstepAudio)
            footstepAudio.Stop();
    }

    void Update()
    {
        if (!player) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            Attack();
        }
        else if (distance <= chaseRange)
        {
            ChasePlayer();
        }
        else
        {
            Idle();
        }
    }

    void ChasePlayer()
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);

        animator.SetBool("isWalking", true);
        animator.SetBool("isAttacking", false);

        //start footsteps when moving
        if (footstepAudio && !footstepAudio.isPlaying)
            footstepAudio.Play();
    }

    void Idle()
    {
        agent.isStopped = true;

        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", false);

        //  Stop footsteps
        if (footstepAudio && footstepAudio.isPlaying)
            footstepAudio.Stop();
    }

    void Attack()
    {
        agent.isStopped = true;

        //;ook at player
        Vector3 lookPos = player.position - transform.position;
        lookPos.y = 0;
        transform.rotation = Quaternion.LookRotation(lookPos);

        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", true);

        //stop footsteps while attacking
        if (footstepAudio && footstepAudio.isPlaying)
            footstepAudio.Stop();

        //cooldown + Damage
        if (Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackCooldown;
            if (attackAudio)
            {
                attackAudio.Play();
                Debug.Log("ATTACK SOUND PLAYING");
            }
            else
            {
                Debug.Log("AttackAudio is NULL");
            }


            if (playerHealth != null)
                playerHealth.TakeDamage(10f);

            Debug.Log("Zombie attacks player!");
        }
    }
}
