using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class Missile : MonoBehaviour {
    [Tooltip("导弹是否朝向右边")]
    public bool FacingRight = true;
    [Tooltip("爆炸效果")]
    public GameObject Explosion;
    [Tooltip("导弹飞行的速度")]
    public float Speed = 25f;
	[Tooltip("导弹造成的伤害")]
	public float DamageAmount = 10f;
    [Tooltip("击退力的大小")]
    public float HurtForce = 50f;

    private Rigidbody2D m_Rigidbody2D;
    private CapsuleCollider2D m_Trigger;

    private void Awake() {
        // 获取引用
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_Trigger = GetComponent<CapsuleCollider2D>();
    }

    private void Start() {
        // 确保Body Type为Kinematic
        m_Rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        
        // 根据导弹朝向设置速度
        if(FacingRight) {
            m_Rigidbody2D.velocity = new Vector2(Speed, 0);
        } else {
            m_Rigidbody2D.velocity = new Vector2(-Speed, 0);
        }

        // 确保勾选了Trigger
        m_Trigger.isTrigger = true;
    }

    public void Flip() {
        // 更新朝向
        FacingRight = !FacingRight;

        // 修改scale的x分量实现转向
        this.transform.localScale = Vector3.Scale(
			new Vector3(-1, 1, 1),
			this.transform.localScale
		);
    }

    // 导弹爆炸时调用的函数
    private void OnExplode() {
        if(Explosion != null) {
            // 随机生成一个四元数
            Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
            // 实例化爆炸对象
            Instantiate(Explosion, transform.position, randomRotation);
        } else {
            Debug.LogWarning("请设置Explosion");
        }

        //销毁自身
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        // 不对角色产生任何操作
        if(collider.CompareTag("Player")) {
            return;
        }

        // 对怪物造成伤害
        if(collider.CompareTag("Enemy")) {
            collider.GetComponent<Enemy>().TakeDamage(this.transform, HurtForce, DamageAmount);
        }

		OnExplode();
    }
}