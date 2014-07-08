using UnityEngine;
using System.Collections;

public class ScoreScreen : MonoBehaviour {

	public GUIStyle box;
	public GUIStyle label;
	public GUIStyle button;

	public int currentLevel;
	public int maxLevels;

	public int greenLives, orangeLives, greenScore, orangeScore, greenWins, orangeWins, greenTotalWins, orangeTotalWins;

	int padding = 0;
	int width = 200;
	bool showing;

	// Use this for initialization
	void Start () {
		greenLives = orangeLives = greenScore = orangeScore = greenWins = orangeWins = greenTotalWins = orangeTotalWins = 0;
		showing = false;
		DontDestroyOnLoad(transform.gameObject);
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnGUI() {
		if (showing == true) {
			GUI.Box(new Rect(padding, padding, Screen.width - padding*2, Screen.height - padding*2), "S  t a  t u  s", box);

			GUI.Label(new Rect(Screen.width / 2 - width / 2, Screen.height / 2 - 120, width, 40), "Match    Lives: ", label);
			GUI.Label(new Rect(Screen.width / 2 + width / 2, Screen.height / 2 - 120, width, 40), greenLives.ToString() + " : " + orangeLives.ToString(), label);

			GUI.Label(new Rect(Screen.width / 2 - width / 2, Screen.height / 2 - 40, width, 40), "Match    Scores: ", label);
			GUI.Label(new Rect(Screen.width / 2 + width / 2, Screen.height / 2 - 40, width, 40), greenScore.ToString() + " : " + orangeScore.ToString(), label);

			GUI.Label(new Rect(Screen.width / 2 - width / 2, Screen.height / 2 + 40, width, 40), "Current    Wins: ", label);
			GUI.Label(new Rect(Screen.width / 2 + width / 2, Screen.height / 2 + 40, width, 40), greenWins.ToString() + " : " + orangeWins.ToString(), label);

			GUI.Label(new Rect(Screen.width / 2 - width / 2, Screen.height / 2 + 120, width, 40), "Total    Wins: ", label);
			GUI.Label(new Rect(Screen.width / 2 + width / 2, Screen.height / 2 + 120, width, 40), greenTotalWins.ToString() + " : " + orangeTotalWins.ToString(), label);

			if (GUI.Button (new Rect (Screen.width / 2 - 100, Screen.height - 120, 240, 60), "C o n ti n u e", button)) {
				showing = false;
				Time.timeScale = 1;
				currentLevel++;
				if (greenWins >= 2) {
					greenTotalWins++;
					greenWins = 0;
					orangeWins = 0;
					currentLevel = 1;
				} else if (orangeWins >= 2) {
					orangeTotalWins++;
					greenWins = 0;
					orangeWins = 0;
					currentLevel = 1;
				}
				else if (currentLevel > maxLevels) {
					if (greenWins > orangeWins) {
						greenTotalWins++;
					} else orangeTotalWins++;

					greenWins = 0;
					orangeWins = 0;
					currentLevel = 1;
				}
				Application.LoadLevel(currentLevel);
			}
		}
	}
	
	public void activate() {
		Time.timeScale = 0;
		showing = true;
	}
}