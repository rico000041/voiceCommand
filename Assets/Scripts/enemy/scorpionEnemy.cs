using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scorpionEnemy : MonoBehaviour {

	public GameObject enemy;
	public Rigidbody2D rbEnemy;
	public int enemyHealth = 100;
	public int enemyRight = 0;
	public int enemyLeft = 0;
	Animator anim;
	string playerFR;

	// Use this for initialization
	void Start () {
		anim = enemy.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		playerFR = GameObject.Find("Player").GetComponent<tutorialScene>().playerFront;
		
		if(enemyRight < 100){
			rbEnemy.transform.localScale = new Vector3(-3.431503f,4.160697f,3.431503f);
			rbEnemy.velocity = transform.right * 1;
			enemyRight ++;
		}
		else{
			rbEnemy.velocity = transform.right * -1;
			rbEnemy.transform.localScale = new Vector3(3.431503f,4.160697f,3.431503f);
			enemyLeft ++;
		}
		if(enemyLeft == 100){
			enemyRight = 0;
			enemyLeft = 0;
		}


		if(enemyHealth <= 0){
			anim.SetBool("isDie",true);
			StartCoroutine("dieEnemy");
		}

	}
	void OnCollisionEnter2D(Collision2D col){
		if(col.gameObject.tag == "bullet"){
			enemyHealth -= 20;
			anim.SetInteger("enemyState",1);
			StartCoroutine("hitEnemy");
		}
		if(col.gameObject.tag == "Player"){
			if(playerFR == "left"){
				enemyRight = 0;
				enemyLeft = 0;
			}
			else{
				enemyRight = 100;
			}
		}

	}
	IEnumerator dieEnemy(){
		yield return new WaitForSeconds(1.5f);
		Destroy(enemy);
	}
	IEnumerator hitEnemy(){
		yield return new WaitForSeconds(.5f);
		anim.SetInteger("enemyState",0);
	}
}
