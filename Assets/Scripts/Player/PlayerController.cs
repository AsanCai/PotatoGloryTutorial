using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour {
	[Tooltip("角色初始朝向是否朝向右边")]
    public bool FacingRight = true;
	[Tooltip("移动时角色加速的力大小")]
	public float MoveForce = 365f;
	[Tooltip("角色移动的最大速度")]
    public float MaxSpeed = 5f;
	[Tooltip("跳跃时向上加速的力大小")]
	public float JumpForce = 1000f;
	[Tooltip("检测角色是否落地")]
    public Transform GroundCheck;

	[Tooltip("跳跃音效")]
    public AudioClip[] JumpClips;

	[Tooltip("显示血量条的物体")]
	public Transform HealthBarDisplay;

	// 获取用户输入
	private Vector2 m_Input;
	// 记录角色当前是否处于准备跳跃状态
	private bool m_IsReadyToJump;
	// 记录角色当前是否正处于跳跃状态
	private bool m_IsJumping;
	// 记录角色当前是否处于着地状态
	private bool m_GroundedStatus;

	// 组件引用变量
	private Rigidbody2D m_Rigidbody2D;
	private Animator m_Animator;

	private void Awake() {
		// 获取组件引用
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		m_Animator = GetComponent<Animator>();
	}

	private void Start() {
		// 监测变量是否正确赋值
		if(GroundCheck == null) {
			Debug.LogError("请先设置GroundCheck");
		}

		// 初始化变量
		m_Input = new Vector2();
		m_IsReadyToJump = false;
		m_IsJumping = false;
		m_GroundedStatus = false;
	}

	private void Update() {
		// 通过检测角色和groundCheck之间是否存在Ground层的物体来判断当前是否落地
		m_GroundedStatus = Physics2D.Linecast(
			transform.position,
			GroundCheck.position,
			LayerMask.GetMask("Obstacle")
		);
		
		// 设置动画状态机控制参数
		m_Animator.SetBool("Grounded", m_GroundedStatus);

#if UNITY_STANDALONE                                //PC端使用Input来获取输入
		// 着地时，如果当前不处于跳跃状态且按下了跳跃键，进入准备跳跃状态
		if(m_GroundedStatus && !m_IsJumping && Input.GetButtonDown("Jump")) {
			m_IsReadyToJump = true;
		}

		m_Input.x = Input.GetAxis("Horizontal");
#elif UNITY_IOS || UNITY_ANDROID                    //移动端使用InputManager来获取输入
		// 着地时，如果当前不处于跳跃状态且按下了跳跃键，进入准备跳跃状态
		if(m_GroundedStatus && !m_IsJumping && InputManager.GetButtonDown("Jump")) {
			m_IsReadyToJump = true;
		}

		m_Input.x = InputManager.GetAxis("Horizontal");
#endif

		// 刚刚落地，退出跳跃状态
		if(m_GroundedStatus && m_IsJumping) {
			m_IsJumping = false;
		}
	}

	private void FixedUpdate() {
		//获取水平输入
		float h = m_Input.x;

		// 设置动画状态机控制参数
		m_Animator.SetFloat("Speed", Mathf.Abs(h));

		// 若h * m_Rigidbody2D.velocity.x为正数且小于MaxSpeed，表示需要继续加速
        // 若h * m_Rigidbody2D.velocity.x为负数，则表示需要反向加速
        if(h * m_Rigidbody2D.velocity.x < MaxSpeed) {
            m_Rigidbody2D.AddForce(Vector2.right * h * MoveForce);
        }

        //设置物体速度的阈值
        if(Mathf.Abs(m_Rigidbody2D.velocity.x) > MaxSpeed) {
            m_Rigidbody2D.velocity = new Vector2(
				Mathf.Sign(m_Rigidbody2D.velocity.x) * MaxSpeed,
				m_Rigidbody2D.velocity.y
			);
        }

        //判断当前是否需要转向
        if(h > 0 && !FacingRight) {
            Flip();
        }else if(h < 0 && FacingRight) {
            Flip();
        }

		// 跳跃
		if(m_IsReadyToJump) {
			Jump();
		}
	}

	private void Jump() {
		// 进入跳跃状态
		m_IsJumping = true;

		// 设置一个竖直向上的力
		m_Rigidbody2D.AddForce(new Vector2(0f, JumpForce));

		// 设置动画状态机控制参数
		m_Animator.SetTrigger("Jump");

		// 退出准备跳跃状态，避免重复跳跃
		m_IsReadyToJump = false;

		//随机在角色当前所处的位置播放一个跳跃的音频
        if(JumpClips.Length > 0) {
			int i = Random.Range(0, JumpClips.Length);
        	AudioSource.PlayClipAtPoint(JumpClips[i], transform.position);
		}
	}

	private void Flip() {
        // 修改当前朝向
        FacingRight = !FacingRight;

        // 修改scale的x分量实现转向
        this.transform.localScale = Vector3.Scale(
			new Vector3(-1, 1, 1),
			this.transform.localScale
		);

		if(HealthBarDisplay != null) {
			// 在角色转向时翻转HealthBarDisplay，确保HealthBarDisplay不随角色转向而翻转
			HealthBarDisplay.localScale = Vector3.Scale(
				new Vector3(-1, 1, 1),
				HealthBarDisplay.localScale
			);
		} else {
			Debug.LogWarning("请设置HealthBarDisplay");
		}
	}
}