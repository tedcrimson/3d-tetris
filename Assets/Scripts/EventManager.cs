using UnityEngine;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour {

	public void Pause(GameObject panel){
		if(panel!=null)panel.SetActive(true);
		Time.timeScale = 0.0f;
	}
	public void Resume(GameObject panel){
		if(panel!=null)panel.SetActive(false);
		Time.timeScale = 1f;
	}
	public void Restart(){
		Resume(null);
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
	public void Home(){
		Resume(null);
		SceneManager.LoadScene(0);
	}

	public void LoadGame(){
		SceneManager.LoadScene ("Game");
	}

	public void LoadMenu(){
		SoundManager.Instance.PlaySound(Sound.ButtonClick);
		SceneManager.LoadScene ("Main");
	}
}
