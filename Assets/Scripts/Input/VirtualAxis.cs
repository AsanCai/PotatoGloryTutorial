using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualAxis {
	public string Name { get; private set; }
	private float m_Value;

	// 构造函数
	public VirtualAxis(string name) {
		this.Name = name;
	}

	// 更新当前的值
	public void Update(float value) {
		m_Value = value;
	}

	// 返回当前的值
	public float GetValue() {
		return m_Value; 
	}

	// 返回初始值
	public float GetValueRaw() {
		return m_Value;
	}
}