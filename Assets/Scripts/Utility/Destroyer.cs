using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour {

	// 销毁自身
	private void DestroyGameObject() {
        Destroy(gameObject);
    }
}