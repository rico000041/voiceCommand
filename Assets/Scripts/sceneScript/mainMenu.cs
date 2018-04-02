using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KKSpeech;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour {

	public Button startRecordingButton;
	public Button startButton;
	public Button optionsButton;
	public Button quitButton;
	public Toggle easyMode;
	public Toggle normalMode;
	public Toggle hardMode;
	public Text resultText;
	public GameObject panel;
	public GameObject startGB;
	public GameObject quitGB;
	bool optionActive = false;
	string mode = "easy";
	// Use this for initialization
	void Start () {
		Button startplaybtn = startRecordingButton.GetComponent<Button>();
        startplaybtn.onClick.AddListener(startPlay);

        Button startbtn = startButton.GetComponent<Button>();
        startbtn.onClick.AddListener(play);
        Button optionsbtn = optionsButton.GetComponent<Button>();
        optionsbtn.onClick.AddListener(options);
        Button quitbtn = quitButton.GetComponent<Button>();
        quitbtn.onClick.AddListener(quit);

        SpeechRecognizer.SetDetectionLanguage("en-PH");

        panel.SetActive(optionActive);

        if (SpeechRecognizer.ExistsOnDevice()) {
			SpeechRecognizerListener listener = GameObject.FindObjectOfType<SpeechRecognizerListener>();
			listener.onAuthorizationStatusFetched.AddListener(OnAuthorizationStatusFetched);
			listener.onAvailabilityChanged.AddListener(OnAvailabilityChange);
			listener.onErrorDuringRecording.AddListener(OnError);
			listener.onErrorOnStartRecording.AddListener(OnError);
			listener.onFinalResults.AddListener(OnFinalResult);
			listener.onPartialResults.AddListener(OnPartialResult);
			listener.onEndOfSpeech.AddListener(OnEndOfSpeech);
			SpeechRecognizer.RequestAccess();
		} else {
			resultText.text = "Sorry, but this device doesn't support speech recognition";
		}
		trigger();

	}
	void Update(){
		Debug.Log(mode);
	}
	
	// Update is called once per frame
	public void trigger(){
		startRecordingButton.onClick.Invoke();
	}
	public void easy(){
		mode = "easy";
	
	}
	public void normal(){
		mode = "normal";
	}
	public void hard(){
		mode = "hard";
	}
	void play(){
		SceneManager.LoadScene("trainingScene");
	}
	public void options(){
		if(optionActive == false){
			panel.SetActive(true);
			optionActive = true;

			startGB.SetActive(false);
			quitGB.SetActive(false);
		}
		else{
			panel.SetActive(false);
			optionActive = false;

			startGB.SetActive(true);
			quitGB.SetActive(true);
		}

		
	}
	void quit(){
		Application.Quit();
	}
	void startPlay(){
		if(SpeechRecognizer.IsRecording()) {
		SpeechRecognizer.StopIfRecording();
		startRecordingButton.GetComponentInChildren<Text>().text = "Start Recording";
		} else{
		SpeechRecognizer.StartRecording(true);
		startRecordingButton.GetComponentInChildren<Text>().text = "Stop Recording";
		resultText.text = "Say Something";
		}
	}

	public void OnFinalResult(string result) {
		resultText.text = result;

		if(result == "start"){
			play();
		}
		if(result == "options"){
			options();
		}
		if(result == "quit"){
			quit();
		}
		if(result == "test"){
			SceneManager.LoadScene("ExampleScene");
		}
		trigger();
	}

	public void OnPartialResult(string result) {
		resultText.text = result;

		if(result == "start"){
			play();
		}
		if(result == "options"){
			options();
		}
		if(result == "quit"){
			quit();
		}
		if(result == "test"){
			SceneManager.LoadScene("ExampleScene");
		}
		trigger();
	}
	public void OnAvailabilityChange(bool available) {
		startRecordingButton.enabled = available;
		if (!available) {
			resultText.text = "Speech Recognition not available";
		} else {
			resultText.text = "Say something :-)";
		}
	}

	public void OnAuthorizationStatusFetched(AuthorizationStatus status) {
		switch (status) {
		case AuthorizationStatus.Authorized:
			startRecordingButton.enabled = true;
			break;
		default:
			startRecordingButton.enabled = false;
			resultText.text = "Cannot use Speech Recognition, authorization status is " + status;
			break;
		}
	}

	public void OnEndOfSpeech() {
		startRecordingButton.GetComponentInChildren<Text>().text = "Start Recording";
		trigger();
	}

	public void OnError(string error) {
		Debug.LogError(error);
		resultText.text = "Something went wrong... Try again! \n [" + error + "]";
		startRecordingButton.GetComponentInChildren<Text>().text = "Start Recording";
		trigger();
	}
}
