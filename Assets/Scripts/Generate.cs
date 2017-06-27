using UnityEngine;
//using UnityEngine.Advertisements;
using UnityEngine.UI;
using System.Collections;
#if UNITY_IPHONE
using ADBannerView = UnityEngine.iOS.ADBannerView;
#endif
public class Generate : MonoBehaviour {

	public GameObject obstacles;
	public GameObject ground;
	public Player player;
	public float startTime = 0f;
	public float repeatTime = 2.5f;
	public Text scoreText;
	public Button endButton;

	int score = 0;

	bool vidsEnabled = false;

	public delegate void OnFailDelegate();
	public static OnFailDelegate onFailDelegate;

	Vector3 touchDown;
	float touchDownTime;
	Vector3 touchUp;
	float touchUpTime;

	public AudioSource failSound;
	public AudioSource winSound;

	int TotalPlays {
		get {
			return PlayerPrefs.GetInt ("totalPlays");
		}

		set {
			PlayerPrefs.SetInt ("totalPlays", value);
		}
	}

	int HighScore {
		get {
			return PlayerPrefs.GetInt ("highScore");
		}

		set {
			PlayerPrefs.SetInt ("highScore", value);
		}
	}

//	#if UNITY_IPHONE
//	private ADBannerView banner;
//	void SetupBanner() {
//		banner = new ADBannerView(ADBannerView.Type.MediumRect, ADBannerView.Layout.Center);
//
//		ADBannerView.onBannerWasClicked += OnBannerClicked;
//		ADBannerView.onBannerWasLoaded += OnBannerLoaded;
//		ADBannerView.onBannerFailedToLoad += OnBannerFailedToLoad;
//	}
//
//	void OnBannerClicked()
//	{
//		Debug.Log("Clicked!\n");
//	}
//
//	void OnBannerLoaded()
//	{
//		Debug.Log("Loaded!\n");
//		banner.visible = true;
//	}
//
//	void OnBannerFailedToLoad()
//	{
//		Debug.Log("FAIL!\n");
//		banner.visible = false;
//	}
//	#endif

	// Use this for initialization
	void Start () {
		if (ground != null) {
			Instantiate (ground);
		}

		if (obstacles != null) {
			InvokeRepeating ("CreateObstacle", startTime, repeatTime);
		}
	}

	public void AddToScore() {
		score++;
		score = 53;
		scoreText.text = string.Format ("{0}", Mathf.Max (score, 0));
		if (score > 0) {
			winSound.Play ();
		}
	}
	
	void CreateObstacle() {
		if (player.dead) {
			return;
		}
		Instantiate (obstacles);
	}

	void Update() {
		if (Input.GetMouseButtonDown (0)) {
			RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			if (hit.collider != null) {
				if (hit.transform.gameObject.tag == "Player") {
					touchDown = Input.mousePosition;
					touchDownTime = Time.time;
				}
			}
		} else if (Input.GetMouseButtonUp (0) && touchDownTime > 0) {
			
			RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			if (hit.collider == null || hit.transform.gameObject.tag != "Player") {
				touchUp = Input.mousePosition;
				touchUpTime = Time.time;

				HandleJump ();
			}
		}

	}

	void HandleJump() {
	    float forceReductionRatio = 5.5f;
	    Vector3 power = (touchUp - touchDown) / forceReductionRatio;
		float timeDelta = touchUpTime - touchDownTime;
		player.Jump (power / timeDelta);

		touchDown = Vector3.zero;
		touchDownTime = 0;
		touchUp = Vector3.zero;
		touchUpTime = 0;
	}

	public void Lose() {
		if (score > HighScore) {
			HighScore = score;
		}
		onFailDelegate ();
		failSound.Play ();
		StartCoroutine (ExecuteAfterTime (0.5f));
	}

	IEnumerator ExecuteAfterTime(float time)
	{
		yield return new WaitForSeconds(time);
		ShowEndUI ();
	}

	public void GoBack() {
		Application.LoadLevel ("Start");
	}

	public void ShowEndUI() {
		endButton.gameObject.SetActive (true);
		#if UNITY_IPHONE
		if (TotalPlays % 5 == 0) {
//			SetupBanner();
		}
		#endif
	}

	public void Restart() {
		TotalPlays++;
		if (TotalPlays % 25 == 0 && vidsEnabled) {
//			if (Advertisement.IsReady()) {
//				Advertisement.Show();
//				return;
//			}
		}
		Application.LoadLevel (Application.loadedLevel);
	}
}
