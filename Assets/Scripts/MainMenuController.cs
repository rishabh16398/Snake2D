using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour {
    public Text Hs;
	// Use this for initialization
	void Start () {
        HsFunc();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Play()
    {
        SceneManager.LoadScene(1);
    }
    void HsFunc()
    {
        Hs.text = PlayerPrefs.GetInt("HighScore").ToString();
    }
}
