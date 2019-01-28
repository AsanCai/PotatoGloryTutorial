using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Wander))]
[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour {
	[Tooltip("角色受伤时减少的血量")]
    public float DamageAmount = 10f;
	[Tooltip("角色被怪物伤害时受到的击退力大小")]
    public float HurtForce = 500f;
    [Tooltip("障碍物检测点")]
    [SerializeField]
    private Transform FrontCheck;
    [Tooltip("怪物的血量")]
    public float MaxHP = 10f;
    [Tooltip("怪物受伤时用来展示的图片")]
    public Sprite DamagedSprite;
    [Tooltip("怪物死亡时用来展示的图片")]
    public Sprite DeadSprite;
    [Tooltip("怪物死亡时用来展示DeadSprite")]
    public SpriteRenderer BodySpriteRenderer;

    [Tooltip("怪物死亡时的音效")]
    public AudioClip[] DeathClips;
    [Tooltip("得分特效")]
    public GameObject ScorePrefab;

    private Wander m_Wander;
	private Rigidbody2D m_Rigidbody2D;

    private LayerMask m_LayerMask;
    private float m_CurrentHP;
    private bool m_Hurt;
    private bool m_Dead;

    private void Awake() {
        // 获取引用
        m_Wander = GetComponent<Wander>();
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }
        
    private void Start() {
        // 初始化变量
        m_LayerMask = LayerMask.GetMask("Obstacle");
        m_CurrentHP = MaxHP;
        m_Hurt = false;
        m_Dead = false;
    }

    private void Update () {
		// 死亡之后不执行任何操作
		if(m_Dead) {
			return;
		}

        Collider2D[] frontHits = Physics2D.OverlapPointAll(FrontCheck.position, m_LayerMask);

        if(frontHits.Length > 0) {
            m_Wander.Flip();
        }
    }

	private void OnCollisionEnter2D(Collision2D collision) {
		// 对角色造成伤害
		if(collision.gameObject.CompareTag("Player")) {
			collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(this.transform, HurtForce, DamageAmount);
		}
    }

    // 受伤函数
    public void TakeDamage(Transform weapon, float hurtForce, float damage) {
        // 减少当前的HP
        m_CurrentHP -= damage;

		// 制造击退效果
        Vector3 hurtVector = transform.position - weapon.position;
        m_Rigidbody2D.AddForce(hurtVector.normalized * hurtForce);

        // 判断当前是否第一次受伤
        if(!m_Hurt) {
			m_Hurt = true;

            if(DamagedSprite != null) {
                // 禁用原有的Sprite
				SpriteRenderer[] children = GetComponentsInChildren<SpriteRenderer>();
				foreach(SpriteRenderer child in children) {
					child.enabled = false;
				}

				// 显示怪物受伤图片
				if(BodySpriteRenderer != null) {
					BodySpriteRenderer.enabled = true;
					BodySpriteRenderer.sprite = DamagedSprite;
				} else {
					Debug.LogError("请设置BodySpriteRenderer");
				}
            } else {
                Debug.LogWarning("请设置DamagedSprite");
            }
        }
		
        // 判断当前的是否死亡
        if(m_CurrentHP <= 0 && !m_Dead) {
            m_Dead = true;
            Death();
        }
    }

    private void Death() {
		// 禁用Wander.cs
		m_Wander.enabled = false;

        if(DeadSprite != null) {
			// 禁用原有的Sprite
			SpriteRenderer[] children = GetComponentsInChildren<SpriteRenderer>();
			foreach(SpriteRenderer child in children) {
				child.enabled = false;
			}

			// 显示怪物死亡图片
			if(BodySpriteRenderer != null) {
				BodySpriteRenderer.enabled = true;
				BodySpriteRenderer.sprite = DeadSprite;
			} else {
				Debug.LogError("请设置BodySpriteRenderer");
			}
		} else {
			Debug.LogWarning("请设置DeadSprite");
		}

        // 将所有的Collider2D都设置为Trigger，避免和其他物体产生物理碰撞
        Collider2D[] cols = GetComponents<Collider2D>();
        foreach(Collider2D c in cols) {
            c.isTrigger = true;
        }

        // 随机播放死亡的音效
        if(DeathClips != null && DeathClips.Length > 0) {
            int i = Random.Range(0, DeathClips.Length);
            AudioSource.PlayClipAtPoint(DeathClips[i], transform.position);
        } else {
			Debug.LogWarning("请设置DeathClips");
		}

        // 生成得分特效
        if(ScorePrefab != null) {
            Vector3 scorePos = this.transform.position + Vector3.up * 1.5f;
            Instantiate(ScorePrefab, scorePos, Quaternion.identity);
        } else {
            Debug.LogError("请设置ScorePrefab");
        }

        // 增加分数
        GameStateManager.Instance.ScoreManagerInstance.AddScore(100);
    }
}