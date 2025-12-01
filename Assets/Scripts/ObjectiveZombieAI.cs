using UnityEngine;
using UnityEngine.AI;

public class ObjectiveZombieAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public Transform objective;

    public Animator animator;

    public float chaseRange = 10f;       // Range to chase OBJECTIVE
    public float playerAgroRange = 3f;   // If player gets too close
    public float attackRange = 1.5f;
    public float attackCooldown = 1.5f;
    private float nextAttackTime;

    private Health playerHealth;
    private Health objectiveHealth;

    [Header("Audio")]
    public AudioSource attackAudio;
    public AudioSource footstepAudio;

    private bool attackingPlayer = false;

    void Start()
    {
        if (!agent) agent = GetComponent<NavMeshAgent>();
        if (!animator) animator = GetComponent<Animator>();

        // Find player + health
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p)
        {
            player = p.transform;
            playerHealth = p.GetComponent<Health>();
        }

        // Find objective
        GameObject o = GameObject.FindGameObjectWithTag("Objective");
        if (o)
        {
            objective = o.transform;
            objectiveHealth = o.GetComponent<Health>();
        }

        if (footstepAudio)
            footstepAudio.Stop();
    }

    void Update()
    {
        if (!objective) return;

        float distToPlayer = Vector3.Distance(transform.position, player.position);
        float distToObjective = Vector3.Distance(transform.position, objective.position);

        // PLAYER TAKES PRIORITY IF TOO CLOSE
        if (distToPlayer <= playerAgroRange)
        {
            attackingPlayer = true;
            HandleAttackTarget(player, playerHealth);
        }
        else if (attackingPlayer && distToPlayer > playerAgroRange * 2f)
        {
            // Reset after player escapes
            attackingPlayer = false;
        }

        // OBJECTIVE is priority when not provoked
        if (!attackingPlayer)
        {
            if (distToObjective <= attackRange)
                HandleAttackTarget(objective, objectiveHealth);
            else if (distToObjective <= chaseRange)
                ChaseTarget(objective);
            else
                Idle();
        }
        else
        {
            // Chase / attack player
            if (distToPlayer <= attackRange)
                HandleAttackTarget(player, playerHealth);
            else
                ChaseTarget(player);
        }
    }

    void ChaseTarget(Transform target)
    {
        agent.isStopped = false;
        agent.SetDestination(target.position);

        animator.SetBool("isWalking", true);
        animator.SetBool("isAttacking", false);

        if (footstepAudio && !footstepAudio.isPlaying)
            footstepAudio.Play();
    }

    void HandleAttackTarget(Transform target, Health targetHealth)
    {
        agent.isStopped = true;

        // Face target
        Vector3 lookPos = target.position - transform.position;
        lookPos.y = 0;
        transform.rotation = Quaternion.LookRotation(lookPos);

        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", true);

        if (footstepAudio && footstepAudio.isPlaying)
            footstepAudio.Stop();

        if (Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackCooldown;

            if (attackAudio)
                attackAudio.Play();

            if (targetHealth != null)
                targetHealth.TakeDamage(10f);
        }
    }

    void Idle()
    {
        agent.isStopped = true;
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", false);

        if (footstepAudio && footstepAudio.isPlaying)
            footstepAudio.Stop();
    }
}
