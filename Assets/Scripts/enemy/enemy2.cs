using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy2 : MonoBehaviour {

	public GameObject enemy;
	int enemyHealt = 100;
	Animator anim;
	// Use this for initialization
	void Start () {
		anim = enemy.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if(enemyHealt <= 0){
			anim.SetInteger("stateEnemy2",1);
			StartCoroutine("WaitForThreeSeconds");
		}
	}

	IEnumerator WaitForThreeSeconds(){
		yield return new WaitForSeconds(1);
		Destroy(enemy);
	}

	void OnCollisionEnter2D(Collision2D col){
		if(col.gameObject.tag == "bullet"){
			enemyHealt -= 20;
		}
	}
}
