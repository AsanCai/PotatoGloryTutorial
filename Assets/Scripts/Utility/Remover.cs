using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Remover : MonoBehaviour {
	[Tooltip("浪花预设")]
	public GameObject SplashPrefab;

	private BoxCollider2D m_Trigger;

	private void Awake() {
		// 获取引用
		m_Trigger = GetComponent<BoxCollider2D>();
		// 确保已经被设置为Trigger
		m_Trigger.isTrigger = true;
	}

    private void OnTriggerEnter2D(Collider2D collision) {
		// 角色掉进河里，游戏失败
		if(collision.CompareTag("Player")) {
			GameStateManager.Instance.SetGameResult(false);
		}

        // 实例化水花对象，水花对象会自动播放声音和动画
		Instantiate(SplashPrefab, collision.transform.position, transform.rotation);
		// 销毁掉下去的物体
		Destroy(collision.gameObject);
    }
}