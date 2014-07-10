using UnityEngine;
using System.Collections;

public class ScoreScreen : MonoBehaviour {

	public GUIStyle blank;
	public GUIStyle box;
	public GUIStyle boxG;
	public GUIStyle boxO;
	public GUIStyle label, labelG, labelO;
	public GUIStyle button;
	public GUIStyle checkboxL;
	public GUIStyle checkboxR;
	public Texture level1, level2, level3;

	public int currentLevel;
	public int maxLevels;

	public int greenLives, orangeLives, greenScore, orangeScore, greenWins, orangeWins, greenTotalWins, orangeTotalWins;

	static int instanceCount = 0;

	int padding = 0;
	int width = 240;
	bool showing;
	bool greenWon = false, orangeWon = false;
	public static int greenAISelection, orangeAISelection, greenLivesSelection, orangeLivesSelection;
	public static int levelSelection;
	string[] AIOptions = new string[] {"Human", "Easy   AI", "Medium   AI", "Hard   AI"};
	string[] livesOptions = new string[] {"3   Lives", "5   Lives"};

	// Use this for initialization
	void Start () {
		instanceCount++;

		if (instanceCount > 1) {
			instanceCount--;
			Destroy(gameObject);
			return;
		}
		
		greenLives = orangeLives = greenScore = orangeScore = greenWins = orangeWins = greenTotalWins = orangeTotalWins = 0;
		greenAISelection = orangeAISelection = greenLivesSelection = orangeLivesSelection = 0;
		levelSelection = -1;
		showing = false;
		DontDestroyOnLoad(transform.gameObject);
		activate();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnGUI() {
		if (showing == true) {
			if (greenWon) {
				GUI.Box(new Rect(padding, padding, Screen.width - padding*2, Screen.height - padding*2), "G R E E N    W I N S!", boxG);
				greenWon = false;

				//Continue button
				if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height - 120, 240, 60), "Main  Menu", button)) {
					greenScore = 0;
					orangeScore = 0;
					greenWins = 0;
					orangeWins = 0;

					showing = false;
					Time.timeScale = 1;
					Screen.showCursor = false;
					Application.LoadLevel(0);
				}
			} else if (orangeWon) {
				GUI.Box(new Rect(padding, padding, Screen.width - padding*2, Screen.height - padding*2), "O R A N G E    W I N S!", boxO);
				orangeWon = false;

				//Continue button
				if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height - 20, 240, 60), "Main  Menu", button)) {
					greenScore = 0;
					orangeScore = 0;
					greenWins = 0;
					orangeWins = 0;
					
					showing = false;
					Time.timeScale = 1;
					Screen.showCursor = false;
					Application.LoadLevel(0);
				}
			}  else {
				GUI.Box(new Rect(padding, padding, Screen.width - padding*2, Screen.height - padding*2), "S  t a  t u  s", box);

				//Level selection buttons
				if (GUI.Button(new Rect(Screen.width / 2 - 300 - 112, Screen.height - 200, 224, 128), level1, blank)) {
					currentLevel = 1;
					goToCurrentLevel();
				}
				
				if (GUI.Button(new Rect(Screen.width / 2 - 112, Screen.height - 200, 224, 128), level2, blank)) {
					currentLevel = 2;
					goToCurrentLevel();
				}
				
				if (GUI.Button(new Rect(Screen.width / 2 + 300 - 112, Screen.height - 200, 224, 128), level3, blank)) {
					currentLevel = 3;
					goToCurrentLevel();
				}
			}

			//GUI.Label(new Rect(Screen.width / 2 - width / 2, Screen.height / 2 - 120, width, 40), "Match    Lives: ", label);
			//GUI.Label(new Rect(Screen.width / 2 + width / 2, Screen.height / 2 - 120, width, 40), greenLives.ToString() + " : " + orangeLives.ToString(), label);

			GUI.Label(new Rect(Screen.width / 2 - width / 2, Screen.height / 2 - 240, width, 40), "Match    Scores: ", label);
			GUI.Label(new Rect(Screen.width / 2 + width / 2, Screen.height / 2 - 240, width, 40), greenScore.ToString() + " : " + orangeScore.ToString(), label);

			GUI.Label(new Rect(Screen.width / 2 - width / 2, Screen.height / 2 - 200, width, 40), "Current    Wins: ", label);
			GUI.Label(new Rect(Screen.width / 2 + width / 2, Screen.height / 2 - 200, width, 40), greenWins.ToString() + " : " + orangeWins.ToString(), label);

			//GUI.Label(new Rect(Screen.width / 2 - width / 2, Screen.height / 2 + 120, width, 40), "Total    Wins: ", label);
			//GUI.Label(new Rect(Screen.width / 2 + width / 2, Screen.height / 2 + 120, width, 40), greenTotalWins.ToString() + " : " + orangeTotalWins.ToString(), label);

			//Green lives selections
			greenLivesSelection = GUI.SelectionGrid(new Rect(Screen.width / 4 - 60, Screen.height / 2 - 148, 128, 64), greenLivesSelection, livesOptions, 1, checkboxL);

			//Green AI selections
			GUI.Label(new Rect(Screen.width / 4 - 60, Screen.height / 2 - 100, 256, 256), "Player  1", labelG);
			greenAISelection = GUI.SelectionGrid(new Rect(Screen.width / 4 - 60, Screen.height / 2 - 60, 128, 128), greenAISelection, AIOptions, 1, checkboxL);

			//Orange lives selections
			orangeLivesSelection = GUI.SelectionGrid(new Rect(Screen.width / 4 * 3 - 60, Screen.height / 2 - 148, 128, 64), orangeLivesSelection, livesOptions, 1, checkboxL);

			//Orange AI selections
			GUI.Label(new Rect(Screen.width / 4 * 3 - 60, Screen.height / 2 - 100, 256, 256), "Player  2", labelO);
			orangeAISelection = GUI.SelectionGrid(new Rect(Screen.width / 4 * 3 - 60, Screen.height / 2 - 60, 128, 128), orangeAISelection, AIOptions, 1, checkboxL);
		}
	}

	private void goToCurrentLevel() {
		showing = false;
		Time.timeScale = 1;
		Screen.showCursor = false;
		Application.LoadLevel(currentLevel);
	}
		
		public int getCurrentLevel() {
		return currentLevel;
	}

	public void setCurrentLevel(int one) {
		currentLevel = one;
	}

	public void handleScore() {
		if (greenLives + orangeLives <= 0) {
			if (greenScore >= orangeScore) {
				greenWins++;
			} else {
				orangeWins++;
			}
			activate();
		}
	}
	
	public void activate() {
		if (greenWins >= maxLevels / 2 + maxLevels % 2) {
			//greenTotalWins++;
			greenWon = true;
			currentLevel = 0;
		} else if (orangeWins >= maxLevels / 2 + maxLevels % 2) {
			//orangeTotalWins++;
			orangeWon = true;
			currentLevel = 0;
		} else {
			currentLevel++;
		}

		levelSelection = -1;
		Screen.showCursor = true;
		Time.timeScale = 0;
		showing = true;
	}
}