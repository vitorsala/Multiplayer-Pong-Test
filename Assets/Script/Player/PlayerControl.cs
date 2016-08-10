using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerControl : NetworkBehaviour {

    public float speed;

    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        if(!isLocalPlayer) return;
        float verticalInput = Input.GetAxis("Vertical") * Time.deltaTime * speed;
        transform.Translate(0, verticalInput, 0);
    }

    public override void OnStartLocalPlayer() {
        base.OnStartLocalPlayer();
        GetComponent<SpriteRenderer>().color = Color.cyan;
    }
}
