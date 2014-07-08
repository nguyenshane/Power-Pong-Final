using UnityEngine;
using System.Collections;

public enum eScore {
	Green,
	Orange
}

public class Scores : MonoBehaviour {

	public eScore Score;

	public GameObject scoreNumber;
	public GameObject livesNumber;

	public float goalSpeedMultiplier;
	public int maxLives;

	private float currentMultiplier;
	private int score;
	private int lives;

	private ScoreScreen scoreScreen;


	// Use this for initialization
	void Start () {
		score = 0;
		lives = maxLives;
		currentMultiplier = 1;
		scoreScreen = GameObject.Find ("ScoreScreen").GetComponent<ScoreScreen> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetScore (int newScore) {
		score = newScore;
		scoreNumber.guiText.text = score.ToString();

		if (Score == eScore.Green) scoreScreen.greenScore = score;
		else scoreScreen.orangeScore = score;
	}

	public void AddScore (int newScore) {
		score += newScore;
		scoreNumber.guiText.text = score.ToString();

		if (Score == eScore.Green) scoreScreen.greenScore = score;
		else scoreScreen.orangeScore = score;
	}

	public void RemoveLife() {
		lives--;
		livesNumber.guiText.text = lives.ToString();

		if (Score == eScore.Green) scoreScreen.greenLives = lives;
		else scoreScreen.orangeLives = lives;

		if (lives <= 0) {
			if (Score == eScore.Green) scoreScreen.orangeWins++;
			else scoreScreen.greenWins++;

			scoreScreen.activate();
		}
	}

	public int getLives() {
		return lives;
	}

	public int getScore() {
		return score;
	}

	public float getMultiplier() {
		return currentMultiplier;
	}

	public void increaseMultiplier() {
		currentMultiplier += goalSpeedMultiplier;
	}
}
