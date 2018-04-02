using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zombieEnemy : MonoBehaviour {

	public GameObject enemy;
	public Rigidbody2D rbEnemy;
	int enemyHealt = 100;
	public int enemyRight = 0;
	public int enemyLeft = 0;
	Animator anim;

	public GameObject zombieAttack;
	// Use this for initialization
	void Start () {
		anim = enemy.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
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
		if(enemyRight == 90){
			anim.SetBool("isAttack",true);
			StartCoroutine("WaitForThreeSeconds");
		}
		else{
			anim.SetBool("isAttack",false);
		}

		if(enemyLeft == 90){
			anim.SetBool("isAttack",true);
			StartCoroutine("WaitForThreeSeconds");
		}
		
	}
	IEnumerator WaitForThreeSeconds(){
		yield return new WaitForSeconds(1);
		
	}
	void OnCollisionEnter2D(Collision2D col){
		if(col.gameObject.tag == "bullet"){
			enemyHealt -= 20;
		}

		if(enemyHealt <= 0){
			anim.SetInteger("die",1);
			StartCoroutine("dieEnemy");
		}
	}
	IEnumerator dieEnemy(){
		yield return new WaitForSeconds(1);
		Destroy(enemy);
	}
}
