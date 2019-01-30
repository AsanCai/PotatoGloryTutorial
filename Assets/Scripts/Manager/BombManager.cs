using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class BombManager {
	[Tooltip("炸弹的初始数量")]
    public int InitBombNumber = 4;
	[Tooltip("炸弹UI")]
	public Image BombUI;
	[Tooltip("显示炸弹的数量")]
	public Text BombNumberText;

	// 当前的炸弹数量
	private int m_CurrentBombNumber;
	// 当前管理器是否停止工作
	private bool m_Stop;

	public void Init() {
		m_CurrentBombNumber = InitBombNumber;
		m_Stop = false;

		// 更新UI
		UpdateUI();
	}

	// 管理器停止工作
	public void Stop() {
		m_Stop = true;
	}
	
	// 释放炸弹
	public bool ReleaseBomb(int bombNum) {
		// 管理器停止工作，不执行任何操作
		if(m_Stop) {
			return false;
		}

		int temp = m_CurrentBombNumber - bombNum;

		if(temp >= 0) {
			m_CurrentBombNumber = temp;
			// 更新UI
			UpdateUI();

			return true;
		} else {
			return false;
		}
	}

	// 拾取炸弹
	public void PickupBomb(int bombNum) {
		// 管理器停止工作，不执行任何操作
		if(m_Stop) {
			return;
		}

		m_CurrentBombNumber += bombNum;
		// 更新UI
		UpdateUI();
	}

	// 更新UI
	private void UpdateUI() {
		BombNumberText.text = "" + m_CurrentBombNumber;

		if(m_CurrentBombNumber <= 0) {
			BombUI.color = new Color(255, 0, 0, BombUI.color.a / 2);
		} else {
			BombUI.color = new Color(255, 255, 255, BombUI.color.a);
		}
	}
}