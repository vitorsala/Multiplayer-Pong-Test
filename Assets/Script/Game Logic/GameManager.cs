using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class GameManager : NetworkBehaviour {
    public static GameManager sharedInstance;

    public Canvas playingCanvas;
    public Canvas waitingCanvas;

    public Text p1ScoreText;
    public Text p2ScoreText;

    public int pointLimit = 20;

    public GameObject ball;

    [SyncVar]
    private int p1Points;

    [SyncVar]
    private int p2Points;

    public enum GameState {
        Waiting, Starting, Playing, Goal, Paused, Ending, End
    }

    [HideInInspector, SyncVar] public GameState gameState;

    private bool controlFlag;
    private float controlTimer;
	// Use this for initialization
	void Start () {
        GameManager.sharedInstance = this;

        controlFlag = false;
        controlTimer = 0f;

        gameState = GameState.Waiting;
	}
	
	// Update is called once per frame
	void Update () {
        switch(gameState) {
            case GameState.Waiting:

                if(!controlFlag) {
                    if(GameObject.FindGameObjectWithTag("Ball") != null)
                        Destroy(GameObject.FindGameObjectWithTag("Ball"));
                    waitingCanvas.enabled = true;
                    playingCanvas.enabled = false;
                    waitingCanvas.GetComponentsInChildren<Text>()[0].text = "Esperando por jogador...";

                    p1Points = 0;
                    p2Points = 0;

                    controlFlag = true;
                }

                if(GameObject.FindGameObjectsWithTag("Player").Length > 1) {
                    gameState = GameState.Starting;
                    controlFlag = false;
                }
                break;

            case GameState.Starting:
                if(!controlFlag) {
                    foreach(GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
                        player.GetComponent<PlayerControl>().enabled = false;
                        player.transform.position = new Vector3(player.transform.position.x, 0, 0);
                    }
                    controlTimer = 3.9f;
                    controlFlag = true;
                }
                else if(controlTimer > 0) {
                    controlTimer -= Time.deltaTime;
                    waitingCanvas.GetComponentsInChildren<Text>()[0].text = ((int)controlTimer).ToString();
                }
                else {
                    foreach(GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
                        player.GetComponent<PlayerControl>().enabled = true;
                    }
                    waitingCanvas.enabled = false;
                    playingCanvas.enabled = true;
                    gameState = GameState.Playing;
                    controlFlag = false;
                }
                break;

            case GameState.Playing:
                if(GameObject.FindGameObjectsWithTag("Player").Length <= 1) {   // if client player leaves, go back to waiting.
                    gameState = GameState.Waiting;
                }
                else if(GameObject.FindGameObjectsWithTag("Ball").Length == 0) { // Check if ball exist on scene.
                    spawnBall();
                }
                // Update score text
                p1ScoreText.text = p1Points.ToString();
                p2ScoreText.text = p2Points.ToString();
                break;

            case GameState.Goal:

                break;

            case GameState.Ending:

                break;

            case GameState.End:

                break;

            case GameState.Paused:

                break;
        }
	}
    
    public void Add1Point(int player) {
        if(player == 1) {
            p1Points++;
        }
        else {
            p2Points++;
        }
    }

    private Vector2[] directions = new Vector2[] { new Vector2(1, 1), new Vector2(1, -1), new Vector2(-1, -1), new Vector2(-1, 1) };

    public void spawnBall() {
        Transform spawnPoint = GameObject.Find("BallSpawnPoint").transform;
        Instantiate(ball);
        ball.transform.position = spawnPoint.position;
        BallMovement bm = ball.GetComponent<BallMovement>();
        bm.direction = directions[Random.Range(0, directions.Length)].normalized;
        bm.speed = 2f;
    }
}
