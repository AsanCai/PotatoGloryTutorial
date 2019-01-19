using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour {
    [Tooltip("是否在Awake时执行销毁操作")]
    public bool DestroyOnAwake = false;
    [Tooltip("销毁延迟时间")]
    public float AwakeDestroyDelay = 0f;

    private void Awake() {
        if(DestroyOnAwake) {
            Destroy(this.gameObject, AwakeDestroyDelay);
        }
    }

	// 销毁自身
	private void DestroyGameObject() {
        Destroy(gameObject);
    }
}