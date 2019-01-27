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

	[Tooltip("是否使用随机时间间隔来实例化预设对象")]
	public bool RandomGenerateInterval = false;
	[Tooltip("实例化预设对象的最短时间间隔")]
	public float MinGenerateInterval;
	[Tooltip("实例化预设对象的最长时间间隔")]
	public float MaxGenerateInterval;
	[Tooltip("实例化预设对象的固定时间间隔")]
	public float GenerateInterval = 3f;

	[Tooltip("是否在随机的X坐标上实例化预设对象")]
	public bool RandomGeneratePositionX = false;
	[Tooltip("实例化预设对象时的最小X坐标")]
	public float MinGeneratePositionX;
	[Tooltip("实例化预设对象时的最大X坐标")]
	public float MaxGeneratePositionX;

	[Tooltip("是否在随机的Y坐标上实例化预设对象")]
	public bool RandomGeneratePositionY = false;
	[Tooltip("实例化预设对象时的最小Y坐标")]
	public float MinGeneratePositionY;
	[Tooltip("实例化预设对象时的最大Y坐标")]
	public float MaxGeneratePositionY;

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
		// 开始随机生成的协程
        StartCoroutine(RandomGenerate());
	}

	private IEnumerator RandomGenerate() {
		yield return new WaitForSeconds(GenerateDelay);

		while(true) {
			// 确定下一次实例化预设对象的时间间隔
			float interval = GenerateInterval;
			if(RandomGenerateInterval) {
				interval = Random.Range(MinGenerateInterval, MaxGenerateInterval);
			}

			yield return new WaitForSeconds(interval);
			
			// 实例化预设对象
			Generate();
		}
	}
	
	// 实例化预设对象
	private void Generate() {
        // 随机选择要实例化的预设的下标
        int index = Random.Range(0, Prefabs.Length);

		// 确定生成位置的X坐标
		float x = transform.position.x;
		if(RandomGeneratePositionX) {
			x = Random.Range(MinGeneratePositionX, MaxGeneratePositionX);
		}
		
		// 确定生成位置的Y坐标
		float y = transform.position.y;
		if(RandomGeneratePositionY) {
			y = Random.Range(MinGeneratePositionY, MaxGeneratePositionY);
		}

		// 更新位置
		transform.position = new Vector3(x, y, transform.position.z);

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