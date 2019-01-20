using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
	[Tooltip("水平方向上最大偏移量")]
    public float HorizontalMargin = 2f;
	[Tooltip("竖直方向上最大偏移量")]
    public float VerticalMargin = 2f;
    [Tooltip("水平方向上跟随角色的速度")]
    public float HorizontalFollowSpeed = 2f;
	[Tooltip("竖直方向上跟随角色的速度")]
    public float VerticalFollowSpeed = 2f;
	[Tooltip("摄像机可移动的范围")]
	public BoxCollider2D Region;

	// 摄像机中心点在水平方向上可移动的范围
	private Vector2 m_HorizontalRegion;
	// 摄像机中心点在竖直方向上可移动的范围
	private Vector2 m_VerticalRegion;

	// 角色Transform组件的引用
    private Transform m_Player;

    private void Awake() {
		// 获取引用
        m_Player = GameObject.FindGameObjectWithTag("Player").transform;

		if(m_Player == null) {
			Debug.LogError("请添加Tag为Player的GameObject");
		}
    }

	private void Start() {
		Camera camera = this.GetComponent<Camera>();
        
		// 获取视口右上角对应的世界坐标
        Vector3 cornerPos = camera.ViewportToWorldPoint(new Vector3(1f, 1f, Mathf.Abs(transform.position.z)));

		// 此时，摄像机的世界坐标就是视口中心点的世界坐标
		// 计算摄像机视口的宽度
		float cameraWidth = 2 * (cornerPos.x - transform.position.x);
		// 计算视口的高度
		float cameraHeight = 2 * (cornerPos.y - transform.position.y);


		// 计算Box Collider2D中心点的世界坐标
		Vector2 regionPosition = new Vector2(
			Region.transform.position.x + Region.offset.x,
			Region.transform.position.y + Region.offset.y
		);

		// 计算Box Collider2D和摄像机视口的宽度差的一半
		float halfDeltaWidth = (Region.size.x - cameraWidth) / 2;
		// 计算Box Collider2D和摄像机视口的高度差的一半
		float halfDeltaHeight = (Region.size.y - cameraHeight) / 2;

		if(halfDeltaWidth < 0) {
			Debug.LogError("Box Collider2D的宽度小于摄像机视口的宽度");
		}

		if(halfDeltaHeight < 0) {
			Debug.LogError("Box Collider2D的高度小于摄像机视口的高度");
		}

		// 计算摄像机中心点水平方向上可移动的范围
		m_HorizontalRegion = new Vector2(
			regionPosition.x - halfDeltaWidth,
			regionPosition.x + halfDeltaWidth
		);

		// 计算摄像机中心点竖直方向上可移动的范围
		m_VerticalRegion = new Vector2(
			regionPosition.y - halfDeltaHeight,
			regionPosition.y + halfDeltaHeight
		);
	}

	private void LateUpdate() {
        TrackPlayer();
    }

	// 跟随玩家
    private void TrackPlayer() {
        float targetX = transform.position.x;
        float targetY = transform.position.y;

		// 如果超出了偏移量，计算摄像机跟随后的位置
        if (CheckHorizontalMargin()) {
            targetX = Mathf.Lerp(transform.position.x, m_Player.position.x, HorizontalFollowSpeed * Time.deltaTime);
        }

        if (CheckVerticalMargin()) {
            targetY = Mathf.Lerp(transform.position.y, m_Player.position.y, VerticalFollowSpeed * Time.deltaTime);
        }

		targetX = Mathf.Clamp(targetX, m_HorizontalRegion.x, m_HorizontalRegion.y);
        targetY = Mathf.Clamp(targetY, m_VerticalRegion.x, m_VerticalRegion.y);

		// 更新摄像机的位置
        transform.position = new Vector3(targetX, targetY, transform.position.z);
    }

	// 判断水平方向上是否超出了最大偏移量
    private bool CheckHorizontalMargin() {
        return Mathf.Abs(transform.position.x - m_Player.position.x) > HorizontalMargin;
    }

	// 判断竖直方向上是否超出了最大偏移量
    private bool CheckVerticalMargin() {
        return Mathf.Abs(transform.position.y - m_Player.position.y) > VerticalMargin;
    }
}