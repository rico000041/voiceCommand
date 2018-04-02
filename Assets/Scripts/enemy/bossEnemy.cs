using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class bossEnemy : MonoBehaviour {

	Animator anim;
	public Rigidbody2D rb;
	public GameObject finalBoss;
	public int bossHealth = 1000;
	public Slider bossSlider;

	int idle = 0;
	int attack = 1;
	int die = 2;
	int smash = 3;
	int bossHalfHealth = 4;

	int smashTrigger = 0;
	// Use this for initialization
	void Start () {
		anim = finalBoss.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		bossSlider.value = bossHealth;
		smashTrigger ++;
		if(bossHealth <= 0){
			anim.SetInteger("bossState",die);
			StartCoroutine("bossDie");
		}
		if(smashTrigger >= 200){
			anim.SetInteger("bossState",smash);
			StartCoroutine("bossSmash");
		}

		if(bossHealth == 500){
			anim.SetInteger("bossState",bossHalfHealth);
			bossHealth-= 1;
			StartCoroutine("bossHalf");
		}

	}
	void OnCollisionEnter2D(Collision2D col){
		if(col.gameObject.tag == "Player"){
			anim.SetInteger("bossState",attack);
			smashTrigger = 0;
			StartCoroutine("bossAttack");

		}
		if(col.gameObject.tag == "bullet"){
			bossHealth-= 20;
		}
	}
	IEnumerator bossDie(){
		yield return new WaitForSeconds(5);
		Destroy(finalBoss);
	}
	IEnumerator bossAttack(){
		yield return new WaitForSeconds(1);
		anim.SetInteger("bossState",idle);
	}
	IEnumerator bossSmash(){
		yield return new WaitForSeconds(1);
		anim.SetInteger("bossState",idle);
		smashTrigger = 0;
	}
	IEnumerator bossHalf(){
		yield return new WaitForSeconds(2.5f);
		anim.SetInteger("bossState",idle);
		smashTrigger = 0;
	}
}

