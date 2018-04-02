using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zombieAttack : MonoBehaviour {

	public Rigidbody2D rbAttack;
	public GameObject gbAttack;
	public int bulletTime = 0;
	int bullet = 0;
	public int bulletSpeed = 4;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		rbAttack.velocity = transform.right * bulletSpeed;

		if(bullet >= bulletTime){
			Destroy(gbAttack);
		}
	}
	void OnCollisionEnter2D(Collision2D col){
		if(col.gameObject.tag == "player"){
			Destroy(gbAttack);
		}
	}
}
