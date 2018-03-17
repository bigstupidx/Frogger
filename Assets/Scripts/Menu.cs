using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
	// Load game
	public void LoadGame()
	{
		// Destroy GameManager from previous game (Handled this way so that audio plays between scene changes)
		GameObject go = GameObject.Find("GameManager");
		if (go != null)
			Destroy(go);

		SceneManager.LoadSceneAsync("Game");
	}
}