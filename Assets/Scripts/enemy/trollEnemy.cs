using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trollEnemy : MonoBehaviour {

	public GameObject enemy;
	public Rigidbody2D rbEnemy;
	public int enemyRight = 0;
	public int enemyLeft = 0;
	string playerFR;
	int enemyHealt = 100;
	Animator anim;
	// Use this for initialization
	void Start () {
		anim = enemy.GetComponent<Animator>();
		
	}
	
	// Update is called once per frame
	void Update () {
		playerFR = GameObject.Find("Player").GetComponent<tutorialScene>().playerFront;

		if(enemyRight < 150){
			rbEnemy.transform.localScale = new Vector3(-2.934256f,2.934256f,2.934256f);
			rbEnemy.velocity = transform.right * 1;
			enemyRight ++;
		}
		else{
			rbEnemy.velocity = transform.right * -1;
			rbEnemy.transform.localScale = new Vector3(2.934256f,2.934256f,2.934256f);
			enemyLeft ++;
		}
		if(enemyLeft == 150){
			enemyRight = 0;
			enemyLeft = 0;
		}


	}
	IEnumerator WaitForThreeSeconds(){
		yield return new WaitForSeconds(1);
		anim.SetBool("isAttack",false);
		
	}
	IEnumerator dieEnemy(){
		yield return new WaitForSeconds(1);
		Destroy(enemy);
	}
	IEnumerator hitEnemy(){
		yield return new WaitForSeconds(.3f);
		anim.SetInteger("enemyState",0);
	}
	void OnCollisionEnter2D(Collision2D col){
		if(col.gameObject.tag == "bullet"){
			enemyHealt -= 20;
			if(enemyHealt > 0){
				anim.SetInteger("enemyState",2);
				StartCoroutine("hitEnemy");
			}
			
		}

		if(enemyHealt <= 0){
			anim.SetInteger("enemyState",1);
			StartCoroutine("dieEnemy");
		}
		if(col.gameObject.tag == "Player"){
			if(playerFR == "left"){
				enemyRight = 0;
				enemyLeft = 0;
			}
			else{
				enemyRight = 150;
			}
			anim.SetBool("isAttack",true);
			StartCoroutine("WaitForThreeSeconds");
		}
		
	}
}
