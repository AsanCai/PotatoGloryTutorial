using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class AmmunitionBoxPickup : MonoBehaviour {
	[Tooltip("增加的炸弹数")]
	public int BombAmount = 1;
	
	[Tooltip("被拾取时播放的音效")]
	public AudioClip PickupEffect;

    private Animator m_Animator;
    private bool m_Landed;

    private void Awake() {
        m_Animator = transform.root.GetComponent<Animator>();

		GetComponent<CircleCollider2D>().isTrigger = true;
    }

	private void Start() {
		m_Landed = false;
	}

    private void OnTriggerEnter2D(Collider2D collision) {
        // 接触到地面
		if (collision.tag == "Ground" && !m_Landed) {
			m_Landed = true;

			// 脱离降落伞
			transform.parent = null;
			gameObject.AddComponent<Rigidbody2D>();

			// 播放降落伞的落地动画
			m_Animator.SetTrigger("Landing");

			return;
		}
		
		// 被角色拾取
		if(collision.tag == "Player") {
			// 增加炸弹数
			GameStateManager.Instance.BombManagerInstance.PickupBomb(BombAmount);

			// 播放拾取音效
            AudioSource.PlayClipAtPoint(PickupEffect, transform.position);

			// 销毁整个物体
            Destroy(transform.root.gameObject);
        }
    }
}