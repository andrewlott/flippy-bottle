using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartScript : MonoBehaviour {

	public GameObject obstacles;
	public GameObject ground;
	public float startTime = 0f;
	public float repeatTime = 1.5f;
	public int highScore;
	public Text highScoreText;

	int HighScore {
		get {
			return PlayerPrefs.GetInt ("highScore");
		}

		set {
			PlayerPrefs.SetInt ("highScore", value);
		}
	}

	// Use this for initialization
	void Start () {
		highScoreText.text = string.Format ("{0}", HighScore);
		if (ground != null) {
			Instantiate (ground);
		}

		if (obstacles != null) {
			InvokeRepeating ("CreateObstacle", startTime, repeatTime);
		}
	}
	
	void CreateObstacle() {
		Instantiate (obstacles);
	}

	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			Application.LoadLevel("Main");
		}
	}

}
