using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Wander))]
public class Enemy : MonoBehaviour {
	[Tooltip("障碍物检测点")]
	[SerializeField]
    private Transform FrontCheck;

	private Wander m_Wander;
	private LayerMask m_LayerMask;

	private void Awake() {
		m_Wander = GetComponent<Wander>();	
	}
     
	private void Start() {
		m_LayerMask = LayerMask.GetMask("Obstacle");
	}

	private void Update () {
		Collider2D[] frontHits = Physics2D.OverlapPointAll(FrontCheck.position, m_LayerMask);

        if(frontHits.Length > 0) {
			m_Wander.Flip();
		}
	}
}
