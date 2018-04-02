using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using KKSpeech;
using UnityEngine.SceneManagement;

public class tutorialScene : MonoBehaviour {

	public Button startRecordingButton;
	public Text resultText;
	public Button doneButton;
	public Rigidbody2D rb;
	public GameObject laser;
	public GameObject leftLaser;
	public Slider sliderHealth;
	public string playerFront = "right";
	Animator anim;
	private float m_JumpForce = 320f;
	public GameObject myPlayer;
	bool inAir = false;
	bool runWalk = false;
	int jumpLimit = 0;
	int idle = 0;
	int jump = 1;
	int walk = 2;
	int run = 3;
	int stop = 4;
	int attack = 5;
	int hit = 6;
	int die = 7;
	int status = 0;
	int fireGun = 0;
	int reload = 0;
	bool controlActive = false;

	int damageToPlayer = 5;
	int playerHealth = 100;

	public GameObject btnLeft;
	public GameObject btnRight;
	public GameObject btnJump;
	public GameObject btnWalk;
	public GameObject btnRun;
	public GameObject btnStop;
	public GameObject btnAttack;
	public Button controlBtn;
	// Use this for initialization
	void Start () {
		btnLeft.SetActive(false);
		btnRight.SetActive(false);
		btnJump.SetActive(false);
		btnWalk.SetActive(false);
		btnRun.SetActive(false);
		btnStop.SetActive(false);
		btnAttack.SetActive(false);

		Button startplaybtn = startRecordingButton.GetComponent<Button>();
        startplaybtn.onClick.AddListener(startPlay);
        
        Button donebtn = doneButton.GetComponent<Button>();
        donebtn.onClick.AddListener(done);

        anim = myPlayer.GetComponent<Animator>();
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
	public void control(){
		if(controlActive == false){
		controlActive = true;

		btnLeft.SetActive(true);
		btnRight.SetActive(true);
		btnJump.SetActive(true);
		btnWalk.SetActive(true);
		btnRun.SetActive(true);
		btnStop.SetActive(true);
		btnAttack.SetActive(true);
		Debug.Log("active");
		}
		else{
		controlActive = false;

		btnLeft.SetActive(false);
		btnRight.SetActive(false);
		btnJump.SetActive(false);
		btnWalk.SetActive(false);
		btnRun.SetActive(false);
		btnStop.SetActive(false);
		btnAttack.SetActive(false);

		Debug.Log("not active");
		}
	}
	//yield return new WaitForSeconds(waitTime);
	// Update is called once per frame
	IEnumerator WaitForThreeSeconds(){
		yield return new WaitForSeconds(.5f);
		if(playerFront == "right"){
				Instantiate(laser,rb.transform.position+(new Vector3(1.5f,.5f,-3)),transform.rotation);
			}
			else{
				Instantiate(leftLaser,rb.transform.position+(new Vector3(-1.5f,.5f,3)),transform.rotation);
			}

		runWalk = false;
	}
	IEnumerator idleAfterHit(){
		yield return new WaitForSeconds(.75f);
		stateStop();
	}
	IEnumerator playerDie(){
		yield return new WaitForSeconds(3);
		SceneManager.LoadScene("mainMenu");
	}
	void Update () {
		sliderHealth.value = playerHealth;

		if(inAir==false){
			if(Input.GetKey("w")){
			stateWalk();
			}
			if(Input.GetKeyDown("r")){
				stateRun();
			}
			if(Input.GetKeyDown("q")){
				stateAttack();
			}

		}
		

		if(status == attack){
			if(fireGun <= 16){
			fireGun++;
			}
			else{
				status = idle;
				fireGun = 0;
				reload = 0;
				stateIdle();
				runWalk = false;
			}
		}
		
		if(status == walk){
			if(inAir==false){
				if(playerFront == "right"){
				rb.velocity = transform.right * 2f;
				}
				else{
					rb.velocity = transform.right * -2f;
				}
			}
			
		}
		else if(status == run){
			if(inAir==false){
				if(playerFront == "right"){
				rb.velocity = transform.right * 4f;
				}
				else{
					rb.velocity = transform.right * -4f;
				}
			}
			
		}
		else if(status == stop){
			anim.SetInteger("state",idle);
		}
		if(Input.GetKeyDown("space")){
			stateJump();
		}
		if(Input.GetKeyDown("i")){
			stateIdle();
		}
		if(Input.GetKeyDown("s")){
			stateStop();
		}
		if(Input.GetKeyDown("a")){
			left();
		}
		if(Input.GetKeyDown("d")){
			right();
		}
		

	}
	void done(){
		SceneManager.LoadScene("LoadingStage1");
	}
	void OnCollisionEnter2D(Collision2D col){
		if(status == jump){
			if(col.gameObject.tag == "ground"){
			anim.SetInteger("state",idle);
			jumpLimit = 0;
			inAir = false;
			runWalk = false;
			}
		}
		if(col.gameObject.tag == "heart"){
			Destroy(GameObject.FindWithTag("heart"));
			if(playerHealth >= 96){
				playerHealth = 100;
			}
			else{
				playerHealth += 15;
			}
			
		}
		if(col.gameObject.tag == "enemy" || col.gameObject.tag == "signElectric"){
			playerHealth -= damageToPlayer;

			jumpLimit = 0;
			inAir = false;
			runWalk = false;
		
			if(sliderHealth.value <= 0){
				status = die;
				anim.SetInteger("state",die);
				stateDie();
			}
			else{
				status = hit;
				anim.SetInteger("state",6);
				StartCoroutine("idleAfterHit");
			}
		}

		if(col.gameObject.tag == "portal2"){
			SceneManager.LoadScene("loadingStage2");
		}
		if(col.gameObject.tag == "portal3"){
			SceneManager.LoadScene("loadingStage3");
		}
		if(col.gameObject.tag == "finalStage"){
			SceneManager.LoadScene("loadingFinal");
		}
		
	}
	public void trigger(){
		startRecordingButton.onClick.Invoke();
		
	}
	public void stateJump(){
		if(status != attack){

			if(jumpLimit == 0){
			rb.AddForce(new Vector2(1.5f, m_JumpForce));
			anim.SetInteger("state",jump);
			jumpLimit ++;
			inAir = true;
			runWalk = true;
			}
			status = jump;
		}
	
	}
	public void stateWalk(){
		if(status != attack){
			if(inAir == false){
			anim.SetInteger("state",walk);
			status = walk;
			runWalk = true;
			}
		}
		
	}
	public void stateRun(){
		if(status != attack){
			if(inAir == false){

			anim.SetInteger("state",run);
			status = run;
			runWalk = true;	
			}
		}
		
	}
	public void stateStop(){
		if(inAir == false){
			anim.SetInteger("state",stop);
			status = stop;
			runWalk = false;
		}
		
	}
	public void stateIdle(){
		if(inAir == false){
			anim.SetInteger("state",idle);
			status = idle;
			runWalk = false;
		}
		
	}
	public void stateAttack(){
		if(runWalk == false && inAir == false){
			anim.SetInteger("state",attack);
			status = attack;

			if(reload == 0){
				reload = 1;
				runWalk = true;
				StartCoroutine("WaitForThreeSeconds");
			}

		}
		
	}
	public void stateDie(){
		status = die;
		anim.SetInteger("state",die);
		StartCoroutine("playerDie");
	}
	public void left(){
		rb.transform.localScale = new Vector3(-0.7679994f, 0.7679994f, 0.7679994f);
		playerFront = "left";
	}
	public void right(){
		rb.transform.localScale = new Vector3(0.7679994f, 0.7679994f, 0.7679994f);
		playerFront = "right";
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

		if(result == "hello"){
			SceneManager.LoadScene("mainMenu");
		}
		if(result == "jump"){
			stateJump();
		}
		if(result == "attack"){
			if(inAir==false){
				stateAttack();
			}
		}
		if(result == "left"){
			left();
		}
		if(result == "right"){
			right();
		}
		if(result == "done"){
			done();
		}
		if(result == "walk"){
			if(inAir==false){
				stateWalk();
			}
		}
		if(result == "run" || result == "friend" || result == "friends"  || result == "ran"){
			if(inAir==false){
				stateRun();
			}
		}
		if(result == "stop"){
			stateIdle();
		}
		if(result == "final"){
			SceneManager.LoadScene("finalLevel");
		}
		trigger();
	}

	public void OnPartialResult(string result) {
		resultText.text = result;

		if(result == "hello"){
			SceneManager.LoadScene("mainMenu");
		}
		if(result == "jump"){
			stateJump();
		}
		if(result == "attack"){
			if(inAir==false){
				stateAttack();
			}
		}
		if(result == "left"){
			left();
		}
		if(result == "right"){
			right();
		}
		if(result == "done"){
			done();
		}
		if(result == "walk"){
			if(inAir==false){
				stateWalk();
			}
		}
		if(result == "run"){
			if(inAir==false){
				stateRun();
			}
		}
		if(result == "stop"){
			stateIdle();
		}
		if(result == "final"){
			SceneManager.LoadScene("finalLevel");
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
