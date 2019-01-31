using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSceneManager : MonoBehaviour {
	// 加载SinglePlayerGameScene场景开始游戏
	public void StartGame() {
		SceneManager.LoadScene("SinglePlayerGameScene");
	}

	// 退出应用
	public void ExitGame() {
		Application.Quit();
	}
}