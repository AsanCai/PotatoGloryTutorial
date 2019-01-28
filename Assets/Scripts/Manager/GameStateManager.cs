using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 游戏状态
public enum GameState {
	Init,
	Start,
	Running,
	End
}

[RequireComponent(typeof(AudioSource))]
public class GameStateManager : MonoBehaviour {
	// 静态实例
	private static GameStateManager m_Instance = null;
	// 使用Property来访问静态实例
	public static GameStateManager Instance {
		get {
			if (m_Instance == null) {
				m_Instance = FindObjectOfType(typeof(GameStateManager)) as GameStateManager;
				
				// 场景中没有添加了GameStateManager.cs脚本的GameObject，就自动创建一个
				if (m_Instance == null) {
					GameObject obj = new GameObject("GameStateManager");
					m_Instance = obj.AddComponent<GameStateManager>();
				}
			}
			
			// 返回静态实例的引用
			return m_Instance;
		}
	}

	[Tooltip("游戏运行时的背景音乐")]
	public AudioClip BackgroundMusic;
	[Tooltip("游戏胜利时的音效")]
	public AudioClip GameWinClip;
	[Tooltip("游戏失败时的音效")]
	public AudioClip GameLoseClip;
	[Tooltip("场景中所有Generator的父物体")]
	public GameObject Generator;
	[Tooltip("ScoreManager的实例")]
	public ScoreManager ScoreManagerInstance = new ScoreManager();

	// 游戏处于哪个状态
	private GameState m_CurrentState;
	// 游戏是否处于暂停状态
	private bool m_IsPaused;
	// 游戏结果，true为胜利，false为失败
	private bool m_GameResult;
	
	private AudioSource m_AudioSource;

#region MonoBehaviour的事件函数
	private void Awake() {
        // 初始化组件
        m_AudioSource = GetComponent<AudioSource>();
        m_AudioSource.playOnAwake = false;
    }

    private void Start() {
        // 初始化成员变量
        m_IsPaused = false;
        m_CurrentState = GameState.Init;
		
        // 开始游戏主循环
        StartCoroutine(GameMainLoop());
    }
#endregion

#region 自定义游戏状态函数
	private IEnumerator GameMainLoop() {
		GameInit();

		while(m_CurrentState == GameState.Init) {
			yield return null;
		}

		GameStart();

		while(m_CurrentState == GameState.Running) {
			GameRunning();

			yield return null;
		}

		GameEnd();
	}

	// 游戏初始化
	private void GameInit() {
		// 执行一些游戏预操作，例如初始化其他Manager、播放过场动画和进行倒计时等
		ScoreManagerInstance.Init();

		// 进入游戏开始状态
		m_CurrentState = GameState.Start;
	}

	// 游戏开始
	private void GameStart() {
		// 开始播放背景音乐
		if(BackgroundMusic != null) {
			m_AudioSource.clip = BackgroundMusic;
			m_AudioSource.loop = true;
			m_AudioSource.Play();
		} else {
			Debug.LogError("请设置BackgroundMusic");
		}

		// 进入游戏运行状态
		m_CurrentState = GameState.Running;
	}

	// 暂停游戏
	private void GamePause() {
		// 暂停背景音乐的播放
		m_AudioSource.Pause();
		// 暂停游戏
		Time.timeScale = 0f;

		m_IsPaused = true;
	}

	// 继续游戏
	private void GameContinue() {
		// 恢复背景音乐的播放
		Time.timeScale = 1f;
		// 恢复游戏
		m_AudioSource.UnPause();

		m_IsPaused = false;
	}

	// 游戏运行
	private void GameRunning() {
		// 暂停或者恢复游戏
        if(Input.GetKeyDown(KeyCode.P)) {
            if(m_IsPaused) {
                GameContinue();
            } else {
                GamePause();
            }
        }
	}

	// 游戏结束
	private void GameEnd() {
		// 停止播放背景音乐
		m_AudioSource.Stop();
		m_AudioSource.loop = false;

		float delay = 0f;

		if(m_GameResult) {
			if(GameWinClip != null) {
				AudioSource.PlayClipAtPoint(GameWinClip, this.transform.position);
				delay = GameWinClip.length;
			} else {
				Debug.LogError("请设置GameWinClip");
			}
		} else {
			if(GameLoseClip != null) {
				AudioSource.PlayClipAtPoint(GameLoseClip, this.transform.position);
				delay = GameLoseClip.length;
			} else {
				Debug.LogError("请设置GameLoseClip");
			}
		}

		// 播放完音效之后，删除场景中的所有Generator
		Destroy(Generator, delay);
	}
#endregion

#region  外部调用函数
	// 设置游戏结果
	public void SetGameResult(bool result) {
		m_GameResult = result;
		m_CurrentState = GameState.End;
	}
#endregion
}