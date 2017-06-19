using UnityEngine;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour {

	public void LoadGame(){
		SceneManager.LoadScene ("Game");
	}

	public void LoadMenu(){
		SoundManager.Instance.PlaySound(Sound.ButtonClick);
		SceneManager.LoadScene ("Main");
	}
}
