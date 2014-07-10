using UnityEngine;
using System.Collections;

public class ScoreScreen : MonoBehaviour {

	public GUIStyle box;
	public GUIStyle boxG;
	public GUIStyle boxO;
	public GUIStyle label;
	public GUIStyle button;

	public int currentLevel;
	public int maxLevels;

	public int greenLives, orangeLives, greenScore, orangeScore, greenWins, orangeWins, greenTotalWins, orangeTotalWins;

	static int instanceCount = 0;

	int padding = 0;
	int width = 200;
	bool showing;
	bool win = false;

	// Use this for initialization
	void Start () {

		instanceCount++;

		if (instanceCount > 1) {
			instanceCount--;
			Destroy(gameObject);
			return;
		}

		greenLives = orangeLives = greenScore = orangeScore = greenWins = orangeWins = greenTotalWins = orangeTotalWins = 0;
		showing = false;
		DontDestroyOnLoad(transform.gameObject);
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnGUI() {
		if (showing == true) {
			if (greenWins >= maxLevels / 2 + maxLevels % 2) {
				GUI.Box(new Rect(padding, padding, Screen.width - padding*2, Screen.height - padding*2), "G R E E N    W I N S!", boxG);
			} else if (orangeWins  >= maxLevels / 2 + maxLevels % 2) {
				GUI.Box(new Rect(padding, padding, Screen.width - padding*2, Screen.height - padding*2), "O R A N G E    W I N S!", boxO);
			}  else {
				GUI.Box(new Rect(padding, padding, Screen.width - padding*2, Screen.height - padding*2), "S  t a  t u  s", box);
			}

			//GUI.Label(new Rect(Screen.width / 2 - width / 2, Screen.height / 2 - 120, width, 40), "Match    Lives: ", label);
			//GUI.Label(new Rect(Screen.width / 2 + width / 2, Screen.height / 2 - 120, width, 40), greenLives.ToString() + " : " + orangeLives.ToString(), label);

			GUI.Label(new Rect(Screen.width / 2 - width / 2, Screen.height / 2 - 40, width, 40), "Match    Scores: ", label);
			GUI.Label(new Rect(Screen.width / 2 + width / 2, Screen.height / 2 - 40, width, 40), greenScore.ToString() + " : " + orangeScore.ToString(), label);

			GUI.Label(new Rect(Screen.width / 2 - width / 2, Screen.height / 2 + 40, width, 40), "Current    Wins: ", label);
			GUI.Label(new Rect(Screen.width / 2 + width / 2, Screen.height / 2 + 40, width, 40), greenWins.ToString() + " : " + orangeWins.ToString(), label);

			//GUI.Label(new Rect(Screen.width / 2 - width / 2, Screen.height / 2 + 120, width, 40), "Total    Wins: ", label);
			//GUI.Label(new Rect(Screen.width / 2 + width / 2, Screen.height / 2 + 120, width, 40), greenTotalWins.ToString() + " : " + orangeTotalWins.ToString(), label);

			if (GUI.Button (new Rect (Screen.width / 2 - 100, Screen.height - 120, 240, 60), "C o n ti n u e", button)) {

				if (currentLevel == 1) {
					greenWins = 0;
					orangeWins = 0;
				}

				if (win == true) {
					greenScore = 0;
					orangeScore = 0;
					greenWins = 0;
					orangeWins = 0;
					orangeLives = 3;
					greenLives = 3;
					win = false;
				}

				showing = false;
				Time.timeScale = 1;
				Screen.showCursor = false;
				Application.LoadLevel(currentLevel);
			}
		}
	}
	
	public void activate() {
		if (greenWins >= maxLevels / 2 + maxLevels % 2) {
			//greenTotalWins++;
			win = true;
			currentLevel = 0;
		
		} else if (orangeWins >= maxLevels / 2 + maxLevels % 2) {
			//orangeTotalWins++;
			win = true;
			currentLevel = 0;
		
		} else {
			currentLevel++;
		}

		Screen.showCursor = true;
		Time.timeScale = 0;
		showing = true;
	}
}