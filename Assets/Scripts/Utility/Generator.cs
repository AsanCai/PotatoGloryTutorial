using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 朝向
public enum Orientation {
	Left,		// 固定朝左
	Right,		// 固定朝右
	Random,		// 随机朝向
	None		// 不需要考虑朝向
}

public class Generator : MonoBehaviour {
    [Tooltip("多久之后开始实例化预设对象")]
	public float GenerateDelay = 2f;
	[Tooltip("实例化预设对象的时间间隔")]
	public float GenerateInterval = 3f;
	[Tooltip("预设对象的朝向")]
	public Orientation PrefabOrientation = Orientation.Right;
	[Tooltip("预设对象")]
    public GameObject[] Prefabs;

	private ParticleSystem m_Particle;

	private void Awake() {
		// 获取引用
		m_Particle = GetComponent<ParticleSystem>();

		if(Prefabs == null || Prefabs.Length == 0) {
			Debug.LogError("请至少为Prefabs添加一个预设对象");
		}
	}

    private void Start () {
        // GenerateDelay秒之后第一次调用Generate函数，然后每隔GenerateInterval调用Generate函数一次
        InvokeRepeating("Generate", GenerateDelay, GenerateInterval);
	}
	
	private void Generate() {
        // 随机选择一个预设进行生成
        int index = Random.Range(0, Prefabs.Length);

		// 实例化预设对象
        GameObject prefab = Instantiate(Prefabs[index], transform.position, Quaternion.identity);

		// 播放粒子特效
		if(m_Particle != null) {
			m_Particle.Play();
		}

		// 不需要考虑朝向
		if(PrefabOrientation == Orientation.None) {
			return;
		}

		if(PrefabOrientation == Orientation.Left) {
			Wander wander = prefab.GetComponent<Wander>();
			if(wander.FacingRight) {
				wander.Flip();
			}
			return;
		} 
		
		if(PrefabOrientation == Orientation.Right) {
			Wander wander = prefab.GetComponent<Wander>();
			if(!wander.FacingRight) {
				wander.Flip();
			}
			return;
		}

		if(PrefabOrientation == Orientation.Random) {
			Wander wander = prefab.GetComponent<Wander>();
			// 有一半的概率进行翻转
			if(Random.value <= 0.5) {
				wander.Flip();
			}
			return;
		}
    }
}