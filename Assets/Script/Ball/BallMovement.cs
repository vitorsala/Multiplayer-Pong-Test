using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class BallMovement : NetworkBehaviour {

    [SyncVar]
    public Vector3 direction;

    [SyncVar]
    public float speed;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(direction * Time.deltaTime * speed);	
	}
    
    void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.tag == "Player") {
            direction.x = -direction.x;
        }
        else if(collision.gameObject.tag == "Wall") {
            direction.y = -direction.y;
        }
    }
}
