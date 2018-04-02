using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class loadingStage1 : MonoBehaviour {

	public GameObject loadingScreenObj;
	public Slider slider;
	public Text percentText;

	// Use this for initialization
	void Start () {
		slider.value = 0;
	}

	
	// Update is called once per frame
	void Update () {
		slider.value += 1;

		percentText.text = slider.value.ToString() + " %";
		if(slider.value == 100){
			SceneManager.LoadScene("Level1");
		}

		
	}
}
