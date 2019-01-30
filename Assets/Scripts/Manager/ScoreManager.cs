using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ScoreManager {
	[Tooltip("游戏胜利的目标分数")]
	public int TargetScore = 5000;

	[Tooltip("保存嘲讽音效")]
    public AudioClip[] TauntClips;
    [Tooltip("得分之后播放嘲讽音效的概率")]
    public float TauntProbaility = 50f;
    [Tooltip("嘲讽的间隔")]
    public float TauntDelay = 1f;
	[Tooltip("显示目标分数")]
	public Text TargetScoreText;
	[Tooltip("显示当前的分数")]
	public Text ScoreText;
	
	// 当前的分数
	private int m_CurrentScore;
	// 上一次播放的嘲讽音效的下标
	private int m_TauntIndex;
	// 上一次播放嘲讽音效的时间
	private float m_LastTauntTime;
	// 当前管理器是否停止工作
	private bool m_Stop;

	private Transform m_Player;

	public void Init() {
		m_CurrentScore = 0;
		m_TauntIndex = 0;
		m_LastTauntTime = Time.time;
		m_Stop = false;

		// 初始化目标分数
		TargetScoreText.text = "" + TargetScore;
		// 初始化当前分数
		ScoreText.text = "" + m_CurrentScore;

		m_Player = GameObject.FindGameObjectWithTag("Player").transform;;
	}

	// 管理器停止工作
	public void Stop() {
		m_Stop = true;
	}

	public void AddScore(int score) {
		// 管理器停止工作，不执行任何操作
		if(m_Stop) {
			return;
		}

		// 增加分数
		m_CurrentScore += score;
		// 更新当前分数
		ScoreText.text = "" + m_CurrentScore;

		// 达到目标分数，游戏胜利
		if(m_CurrentScore >= TargetScore) {
			GameStateManager.Instance.SetGameResult(true);
		}
		
		if(m_LastTauntTime <= Time.time + TauntDelay) {
			float tauntChance = UnityEngine.Random.Range(0f, 100f);

			if(tauntChance > TauntProbaility) {
				// 播放嘲讽音效
				m_TauntIndex = TauntRandom();
				AudioSource.PlayClipAtPoint(TauntClips[m_TauntIndex], m_Player.position);
			}
		}
	}


    //确保相邻两次嘲讽音效不相同
    private int TauntRandom() {
        int i = UnityEngine.Random.Range(0, TauntClips.Length);

        if (i == m_TauntIndex)
            return TauntRandom();
        else
            return i;
    }
}