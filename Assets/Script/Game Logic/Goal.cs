using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour {
    
    [Range(1,2)] public int player;

    private GameManager manager;
	// Use this for initialization
	void Start () {
        manager = GameManager.sharedInstance;
	}
	
	// Update is called once per frame
	void Update () {
	}
    
    void OnTriggerEnter2D(Collider2D col) {
        if(col.tag == "Ball") {
            manager.Add1Point(player);
            Destroy(col.gameObject);
        }
    }
}
