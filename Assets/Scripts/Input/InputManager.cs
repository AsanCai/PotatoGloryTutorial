using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputManager {
	private static Dictionary<string, VirtualAxis> m_VirtualAxes;
    private static Dictionary<string, VirtualButton> m_VirtualButtons;


	// 私有构造器	
	static InputManager() {
		m_VirtualAxes = new Dictionary<string, VirtualAxis>();
    	m_VirtualButtons = new Dictionary<string, VirtualButton>();
	}

#region 用于管理的API
	// 判断轴是否存在
	public static bool AxisExists(string name) {
		return m_VirtualAxes.ContainsKey(name);
	}
	
	// 判断按钮是否注册
	public static bool ButtonExists(string name) {
		return m_VirtualButtons.ContainsKey(name);
	}

	// 注册Axis
	public static void RegisterVirtualAxis(VirtualAxis axis) {
		// 如果已经存在同名Axis，那么提示error
		if (m_VirtualAxes.ContainsKey(axis.Name)) {
			Debug.LogError("There is already a virtual axis named " + axis.Name + " registered.");
		} else {
			// 添加新轴
			m_VirtualAxes.Add(axis.Name, axis);
		}
	}

	// 注册Button
	public static void RegisterVirtualButton(VirtualButton button) {
		// 如果已经存在同名Button，那么提示error
		if (m_VirtualButtons.ContainsKey(button.Name)) {
			Debug.LogError("There is already a virtual button named " + button.Name + " registered.");
		} else {
			// 添加新按钮
			m_VirtualButtons.Add(button.Name, button);
		}
	}

	// 注销Axis
	public static void UnRegisterVirtualAxis(VirtualAxis axis) {		
		// 删除指定的Axis
		if (m_VirtualAxes.ContainsKey(axis.Name)) {
			m_VirtualAxes.Remove(axis.Name);
		}
	}

	// 注销Button
	public static void UnRegisterVirtualButton(VirtualButton button) {
		// 删除指定的Button
		if (m_VirtualButtons.ContainsKey(button.Name)) {
			m_VirtualButtons.Remove(button.Name);
		}
	}

	// 按下按钮
	public static void SetButtonDown(VirtualButton button) {
		if(InputManager.ButtonExists(button.Name)) {
			button.Pressed();
		} else {
			 Debug.LogError("There is not a virtual button named " + button.Name + " registered.");
		}
	}

	// 松开按钮
	public static void SetButtonUp(VirtualButton button) {
		if(InputManager.ButtonExists(button.Name)) {
			button.Released();
		} else {
			 Debug.LogError("There is not a virtual button named " + button.Name + " registered.");
		}
	}
#endregion

#region 用于获取输入的API
	public static float GetAxis(string name) {
		if(m_VirtualAxes.ContainsKey(name)) {
			return m_VirtualAxes[name].GetValue();
		} else {
			Debug.LogError("There is not axis named " + name + " registered.");
			return 0f;
		}
	}


	public static float GetAxisRaw(string name) {
		if(m_VirtualAxes.ContainsKey(name)) {
			return m_VirtualAxes[name].GetValueRaw();
		} else {
			Debug.LogError("There is not axis named " + name + " registered.");
			return 0f;
		}
	}


	public static bool GetButton(string name) {
		if(m_VirtualButtons.ContainsKey(name)) {
			return m_VirtualButtons[name].GetButton();
		} else {
			Debug.LogError("There is not button named " + name + " registered.");
			return false;
		}
	}


	public static bool GetButtonDown(string name) {
		if(m_VirtualButtons.ContainsKey(name)) {
			return m_VirtualButtons[name].GetButtonDown();
		} else {
			Debug.LogError("There is not button named " + name + " registered.");
			return false;
		}
	}


	public static bool GetButtonUp(string name) {
		if(m_VirtualButtons.ContainsKey(name)) {
			return m_VirtualButtons[name].GetButtonUp();
		} else {
			Debug.LogError("There is not button named " + name + " registered.");
			return false;
		}
	}
#endregion
}