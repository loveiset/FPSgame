using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    Transform m_transform;

    protected EnemySpawn m_spawn;

    Player m_player;

    Animator m_ani;
    NavMeshAgent m_agent;

    float m_movSpeed = 0.5f;

    float m_rotSpeed = 120;

    float m_timer = 2;

    int m_life = 10;


	// Use this for initialization
	void Start () {
        m_transform = this.transform;
        m_ani = this.GetComponent<Animator>();
        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        m_agent = GetComponent<NavMeshAgent>();
        m_agent.SetDestination(m_player.m_transform.position);

	
	}
	
	// Update is called once per frame
	void Update () {

        if (m_player.m_life <= 0)
        {
            return;
        }

        AnimatorStateInfo stateInfo = m_ani.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.nameHash == Animator.StringToHash("Base Layer.death") && !m_ani.IsInTransition(0))
        {
            Debug.Log("before death");
            if (stateInfo.normalizedTime >= 1.0f)
            {
                OnDeath();
            }
        }
        if (stateInfo.nameHash == Animator.StringToHash("Base Layer.idle")&&!m_ani.IsInTransition(0))
        {
            m_ani.SetBool("idle", false);

            m_timer -= Time.deltaTime;
            if (m_timer >0)
            {
                return;
            }
            if (Vector3.Distance(m_transform.position, m_player.m_transform.position) < 1.5f)
            {
                m_ani.SetBool("attack", true);
            }
            else
            {
                m_timer = 1;
                m_agent.SetDestination(m_player.m_transform.position);
                m_ani.SetBool("run", true);
            }
        }

        if (stateInfo.nameHash == Animator.StringToHash("Base Layer.run") && !m_ani.IsInTransition(0))
        {
            m_ani.SetBool("run", false);

            m_timer -= Time.deltaTime;
            if (m_timer < 0)
            {
                m_agent.SetDestination(m_player.m_transform.position);
                m_timer = 1;

            }
            MoveTo();
            if (Vector3.Distance(m_transform.position, m_player.m_transform.position) <= 1.5f)
            {
                m_agent.ResetPath();
                m_ani.SetBool("attack",true);
            }
        }

        if(stateInfo.nameHash ==Animator.StringToHash("Base Layer.attack")&&!m_ani.IsInTransition(0))
        {
            RotTo();
            m_ani.SetBool("attack", false);

            if (stateInfo.normalizedTime>=1.0f)
            {
                m_ani.SetBool("idle", true);
                m_timer = 2;
                m_player.OnDamage(1);
            }

        }

	
	}

    void MoveTo()
    {
        float speed = m_movSpeed * Time.deltaTime;
        m_agent.Move(m_transform.TransformDirection(new Vector3(0, 0, speed)));
    }

    void RotTo()
    {
        Vector3 oldangle = m_transform.eulerAngles;
        m_transform.LookAt(m_player.m_transform);
        float target = m_transform.eulerAngles.y;

        float speed = m_rotSpeed * Time.deltaTime;
        float angle = Mathf.MoveTowardsAngle(oldangle.y, target, speed);

        m_transform.eulerAngles = new Vector3(0, angle, 0);

    }
    public void OnDamage(int damage)
    {
        m_life -= damage;
        if (m_life <= 0)
        {
            m_ani.SetBool("death", true);
        }
    }

    public void Init(EnemySpawn spawn)
    {
        m_spawn = spawn;
        m_spawn.m_enemyCount++;
    }

    public void OnDeath()
    {
        m_spawn.m_enemyCount--;
        GameManager.Instance.SetScore(100);
        Destroy(this.gameObject);
    }
}
