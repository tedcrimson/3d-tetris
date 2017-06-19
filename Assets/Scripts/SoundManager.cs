using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public enum Sound{
    Pause, Resume, Die, Grow, Move, ButtonClick
}

public class SoundManager : MonoBehaviour {

    public GameObject musicButton;
    public GameObject soundButton;

    public AudioClip Pause;
    public AudioClip Resume;
    public AudioClip Die;
    public AudioClip Grow;
    public AudioClip Move;

    public AudioClip ButtonClick;

    public AudioSource musicSource;
    public AudioSource soundSource;

	private static SoundManager instance = null;
    public static SoundManager Instance { get { return instance; } }



    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            musicSource = GetComponent<AudioSource>();
            musicSource.volume = PlayerPrefsManager.MusicToggle;
            musicSource.Play();
            DontDestroyOnLoad(this.gameObject);
        }

        SoundManager.instance.SetButtons(musicButton, 0);
        SoundManager.instance.SetButtons(soundButton, 1);

    }

    public void SetButtons(GameObject button, int t){

        Button onButton = button.transform.GetChild(0).GetComponent<Button>();
        Button offButton = button.transform.GetChild(1).GetComponent<Button>();

        onButton.gameObject.SetActive(1 == (t == 0 ? PlayerPrefsManager.MusicToggle : PlayerPrefsManager.SoundToggle));
        offButton.gameObject.SetActive(0 == (t == 0 ? PlayerPrefsManager.MusicToggle : PlayerPrefsManager.SoundToggle));

        onButton.onClick.AddListener(()=> {
            offButton.gameObject.SetActive(true);
            onButton.gameObject.SetActive(false);
            Toggle(0, t);
        });

        offButton.onClick.AddListener(()=> {
            onButton.gameObject.SetActive(true);
            offButton.gameObject.SetActive(false);
            Toggle(1, t);
        });
    }

    public void Toggle(int i, int t){
        if(t == 0){//MUSIC
            musicSource.volume = i;
            PlayerPrefsManager.MusicToggle = i;
        }else{ // Sound
            soundSource.volume = i;
            PlayerPrefsManager.SoundToggle = i;
        }
    }

    public void SpeedUpMusic(float x){
        musicSource.pitch /= x;
    }

    public void PlaySound(Sound s){
        AudioClip c = null;
        switch(s){
            case Sound.Pause:
                c = Pause;
                break;
            case Sound.Resume:
                c = Resume;
                break;
            case Sound.Grow:
                c = Grow;
                break;
            case Sound.Move:
                c = Move;
                break;
            case Sound.Die:
                c = Die;
                break;
            case Sound.ButtonClick:
                c = ButtonClick;
                break;
        }
        soundSource.PlayOneShot(c);
    }


}
