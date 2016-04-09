using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	public float speed;
	private Rigidbody rb;
    private int score;
    public Text countText;
    private List<float[]> pos;
    private int lives;
    public Text livesText;
    public Text gameOverText;
    public Button playBtn;
    private float timeLeft;
    public Text timeLeftText;
    private float _Time;
    public AudioClip fail;
    public AudioClip gameOver;
    public AudioClip success;


	void Start(){
		rb = GetComponent<Rigidbody> ();
        gameOverText.text = "Pill Game";
        gameObject.SetActive(false);

        playBtn.onClick.AddListener(delegate ()
        {
            AudioSource.PlayClipAtPoint(success, transform.position);
            startGame();
        });
	}

	void FixedUpdate(){
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");
        
		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

		rb.AddForce (movement * speed);
	}

    void OnTriggerEnter(Collider other) {
        if (lives == 0)
        {
            endGame();
        }
        else
        {
            if (other.gameObject.CompareTag("Pill"))
            {
                failedCollision();
                AudioSource.PlayClipAtPoint(fail, transform.position);
                takeLife();
            }
            else if (other.gameObject.CompareTag("Target"))
            {
                other.gameObject.SetActive(false);
                score++;
                setScore();

                AudioSource.PlayClipAtPoint(success, transform.position);

                foreach (GameObject gb in GameObject.FindGameObjectsWithTag("Pill"))
                {
                    gb.transform.position = getRandomVector();
                }

                other.transform.position = getRandomVector();
                other.gameObject.SetActive(true);
                timeLeft = 10.0f;
            }
            else if (other.gameObject.CompareTag("Wall"))
            {
                failedCollision();
                AudioSource.PlayClipAtPoint(fail, transform.position);
                takeLife();
            }
        }
    }

    void setScore(){
        countText.text = "Score: " + score.ToString();
    }

    Vector3 getRandomVector(){

        if(pos.Count == 15) {
            pos.Clear();
        }

        float x = Mathf.Floor(Random.Range(-8, 8));
        float z = Mathf.Floor(Random.Range(-8, 8));

        float[] xz = { x, z };

        while (pos.Contains(xz)){
            if(!(x == 0 && z == 0) && !(x == gameObject.transform.position.x && z == gameObject.transform.position.z)){
                x = Mathf.Floor(Random.Range(-8, 8));
                z = Mathf.Floor(Random.Range(-8, 8));
            }
        }

        pos.Add(xz);
        
        return new Vector3(x, 1, z);
    }

    void failedCollision() {
        foreach(GameObject gb in GameObject.FindGameObjectsWithTag("Pill")) {
            gb.transform.position = getRandomVector();
        }

        gameObject.transform.position = new Vector3(0, 0, 0);
    }

    void takeLife()
    {
        lives--;
        livesText.text = "Lives: " + lives;

        if(lives == 0)
        {
            endGame();
        }
    }


    void startGame()
    {
        playBtn.gameObject.SetActive(false);
        score = 0;
        setScore();
        pos = new List<float[]>();
        lives = 1;
        livesText.text = "Lives: " + lives;
        gameOverText.text = "";
        timeLeft = 10.0f;
        timeLeftText.text = timeLeft.ToString();
        failedCollision();
        gameObject.SetActive(true);
    }

    void Update() {
        if (timeLeft < 0) {
            failedCollision();
            takeLife();
        } else {

            // _Time = _Time + Time.deltaTime;
            // float phase = Mathf.Sin(_Time / 2);

            // GameObject.FindGameObjectWithTag("Ground").gameObject.transform.localRotation = Quaternion.Euler(new Vector3(phase * 5, 0, 0));


            timeLeft -= Time.deltaTime;
            timeLeftText.text = timeLeft.ToString();
        }
    }


    void endGame()
    {
        AudioSource.PlayClipAtPoint(gameOver, transform.position);
        gameOverText.text = "Game Over";
        timeLeftText.text = "0";
        gameObject.transform.position = new Vector3(0, 0, 0);
        gameObject.SetActive(false);
        playBtn.gameObject.SetActive(true);
    }

}
