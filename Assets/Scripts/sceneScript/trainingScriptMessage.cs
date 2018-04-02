using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using KKSpeech;
using UnityEngine.SceneManagement;


public class trainingScriptMessage : MonoBehaviour {

	public Button startRecordingButton;
	public Button yesBtn;
	public Button noBtn;
	public Text resultText;

	// Use this for initialization
	void Start () {
		Button yesButton = yesBtn.GetComponent<Button>();
        yesButton.onClick.AddListener(yesTutorial);

        Button noButton = noBtn.GetComponent<Button>();
        noButton.onClick.AddListener(noTutorial);

        Button startplaybtn = startRecordingButton.GetComponent<Button>();
        startplaybtn.onClick.AddListener(startPlay);
        
        SpeechRecognizer.SetDetectionLanguage("en-PH");

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
	
	// Update is called once per frame
	void Update () {
		
	}
	void yesTutorial(){
		SceneManager.LoadScene("loadingScreen");
	}
	void noTutorial(){
		SceneManager.LoadScene("loadingStage1");
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
	public void trigger(){
		startRecordingButton.onClick.Invoke();
	}
	public void OnFinalResult(string result) {
		resultText.text = result;

		if(result == "yes"){
			yesTutorial();
		}
		if(result == "no"){
			noTutorial();
		}
		trigger();
	}

	public void OnPartialResult(string result) {
		resultText.text = result;

		if(result == "yes"){
			yesTutorial();
		}
		if(result == "no"){
			noTutorial();
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
