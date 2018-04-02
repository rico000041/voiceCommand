using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class leftLaser : MonoBehaviour {

	public Rigidbody2D rbLaser;
	public GameObject laser;
	public int bulletTime = 0;
	int bullet = 0;
	public int bulletSpeed = 4;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		bullet ++;
		rbLaser.velocity = transform.right * (bulletSpeed * -1);
		

		if(bullet >= bulletTime){
			Destroy(laser);
		}
	}
	
	void OnCollisionEnter2D(Collision2D col){
		if(col.gameObject.tag == "enemy" || col.gameObject.tag == "signElectric" || col.gameObject.tag == "ground"){
			Destroy(laser);
		}
	}
}
