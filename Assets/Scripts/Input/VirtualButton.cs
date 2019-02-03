using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualButton {
	// 按钮的名称
	public string Name { get; private set; }

	// 按下按钮的帧数
	private int m_LastPressedFrame = -5;
	// 释放按钮的帧数
	private int m_ReleasedFrame = -5;
	// 按钮是否处于被按压的状态
	private bool m_Pressed;

	// 构造函数
	public VirtualButton(string name) {
		this.Name = name;
	}

	// 按压按钮
	public void Pressed() {
		if (m_Pressed) {
			return;
		}

		m_Pressed = true;
		// 记录第一次按压按钮时的帧数
		m_LastPressedFrame = Time.frameCount;
	}

	// 松开按钮
	public void Released() {
		m_Pressed = false;
		// 记录松开按钮时的帧数
		m_ReleasedFrame = Time.frameCount;
	}

	// 获取当前是否按压按钮
	public bool GetButton() {
		return m_Pressed;
	}

	// 获取当前是否刚刚按下按钮
	public bool GetButtonDown() {
		return (m_LastPressedFrame == Time.frameCount - 1);
	}

	// 获取当前是否刚刚松开按钮
	public bool GetButtonUp() {
		return (m_ReleasedFrame == Time.frameCount - 1);
	}
}