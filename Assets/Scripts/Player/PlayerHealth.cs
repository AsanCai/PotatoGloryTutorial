using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {
	[Tooltip("角色的最大生命值")]
    public float MaxHP = 100f;
	[Tooltip("角色被怪物伤害时受到的击退力大小")]
    public float HurtForce = 100f;
	[Tooltip("角色的受伤音效")]
    public AudioClip[] OuchClips;
	[Tooltip("角色受伤时减少的血量")]
    public float DamageAmount = 10f;
	[Tooltip("角色受伤后的免伤时间")]
    public float FreeDamagePeriod = 0.35f;
	[Tooltip("血量条")]
    public SpriteRenderer HealthSprite;

	// 角色当前的血量
	private float m_CurrentHP;
	// 上一次受到伤害的时间
	private float m_LastFreeDamageTime;
	// 血量条的初始长度
	private Vector3 m_InitHealthScale;


	private void Start() {
		// 初始化变量
		m_CurrentHP = MaxHP;
		m_LastFreeDamageTime = 0f;
		m_InitHealthScale = HealthSprite.transform.localScale;
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		// 判断此时是否处于免伤状态
		if(Time.time > m_LastFreeDamageTime + FreeDamagePeriod) {
			// 假如撞到怪物
			if(collision.gameObject.tag == "Enemy") {
				// 检测当前血量
				if(m_CurrentHP > 0f) {
					// 调用受伤函数
					TakeDamage(collision.transform);
					
					// 更新上次受伤害的时间
					m_LastFreeDamageTime = Time.time;
				} else {
					// 角色死亡
					Death();
				}
			}
		}
    }

    // 受伤函数
    public void TakeDamage(Transform enemy) {
        // 给角色加上后退的力，制造击退效果
        Vector3 hurtVector = transform.position - enemy.position + Vector3.up * 5f;
        GetComponent<Rigidbody2D>().AddForce(hurtVector * HurtForce);

        // 更新角色的生命值
        m_CurrentHP -= DamageAmount;

        // 更新生命条
		UpdateHealthBar();

        // 随机播放音频
        int i = Random.Range(0, OuchClips.Length);
        AudioSource.PlayClipAtPoint(OuchClips[i], transform.position);
    }

	private void UpdateHealthBar() {
        if(HealthSprite != null) {
			// 更新血量条颜色
			HealthSprite.color = Color.Lerp(Color.green, Color.red, 1 - m_CurrentHP * 0.01f);
			// 更新血量条长度
			HealthSprite.transform.localScale = Vector3.Scale(m_InitHealthScale, new Vector3(m_CurrentHP * 0.01f, 1, 1));
		} else {
			Debug.LogError("请设置HealthSprite");
		}
    }

	private void Death() {
		// 禁用碰撞体
		Collider2D[] cols = GetComponents<Collider2D>();
		foreach(Collider2D c in cols) {
			c.enabled = false;
		}

		// 禁用脚本
		GetComponent<PlayerController>().enabled = false;

		// 播放死亡动画
		GetComponent<Animator>().SetTrigger("Death");
	}
}