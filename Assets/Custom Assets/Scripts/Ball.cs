using UnityEngine;
using System.Collections;

public enum eBall
{
	Left,
	Right,
	F_Left,
	F_Right
}

public class Ball : MonoBehaviour {

	public GameObject score;
	public GameObject otherScore;
	public GameObject paddle;

	public Vector3 initialImpulse;
	public Vector3 dropLocation;
	public float dropSpeed;
	public float height;
	public float maxSpeed;
	public float minSpeed;
	public float initialDropDelay;
	public eBall ball;
	
	Vector3 leftImpulse = new Vector3(-2,0,0);
	Vector3 rightImpulse = new Vector3(2,0,0);
	Vector3 leftImpulse_F;
	Vector3 rightImpulse_F;
	int normalBrickScore = 1;
	int goalBrickScore = 3;
	float z;
	float goalPointPercentage = 0.20f;
	float currentDropDelay;


	// Use this for initialization
	void Start () {

		if (ball == eBall.F_Left || ball == eBall.F_Right) {
			Destroy(gameObject, 5.0f);
		}
		//z = Random.Range(-5.0f,5.0f);
		leftImpulse_F = new Vector3(-2,0,0);
		rightImpulse_F = new Vector3(2,0,0);

		currentDropDelay = initialDropDelay;
		//dropBall(dropLocation);
	}

	// Update is called once per frame
	void Update () {
		if (currentDropDelay > 0) {
			currentDropDelay -= Time.deltaTime;
		} else if (currentDropDelay == -1000) {
			dropBall (dropLocation);
			currentDropDelay = -2000;
		} else if (currentDropDelay != -2000) {
			currentDropDelay = -1000;
		}

		if (currentDropDelay == -2000) {
			if (rigidbody.position.y < height) {
				rigidbody.MovePosition (new Vector3 (rigidbody.position.x, height, rigidbody.position.z));
				rigidbody.constraints = RigidbodyConstraints.FreezePositionY;
				collider.isTrigger = false;
				score.GetComponent<Scores> ().increaseMultiplier();
				rigidbody.AddForce (initialImpulse * score.GetComponent<Scores> ().getMultiplier (), ForceMode.Impulse);
			} else if (rigidbody.position.y == height) {
				if (rigidbody.velocity.magnitude < minSpeed) {
					if (rigidbody.velocity.magnitude != 0) {
						rigidbody.AddForce (rigidbody.velocity * (1 - minSpeed / rigidbody.velocity.magnitude), ForceMode.Impulse);
					} else {
						rigidbody.AddForce (initialImpulse, ForceMode.Impulse);
					}
				} else if (rigidbody.velocity.magnitude > maxSpeed) {
					rigidbody.AddForce (rigidbody.velocity * -1 * (1 - maxSpeed / rigidbody.velocity.magnitude), ForceMode.Impulse);
				}
			}
		}

		/*if (ball == eBall.F_Left) {
			if (rigidbody.transform.position.x > 11) {
				Destroy(gameObject);
			}
		} else if (ball == eBall.F_Right) {
			if(rigidbody.transform.position.x < -11) {
				Destroy(gameObject);
			}
		}*/
	}


	void OnCollisionEnter(Collision Collection) {
		if (ball == eBall.Left) {
			if(Collection.gameObject.name == "Brick") {
				audio.Play();
				Destroy(Collection.gameObject);
				rigidbody.AddForce(leftImpulse, ForceMode.Impulse);
				score.GetComponent<Scores>().AddScore(normalBrickScore);
			} else if (Collection.gameObject.name == "Brick_O") {
				GameObject.Find("ExplosionSound").audio.Play();
				Destroy(Collection.gameObject);
				score.GetComponent<Scores>().AddScore(goalBrickScore);
			} else if (Collection.gameObject.name == "Brick_G") {
				GameObject.Find("ExplosionSound").audio.Play();
				Destroy(Collection.gameObject);
			} else if (Collection.gameObject.name == "Player Left") {
				rigidbody.AddForce(rightImpulse, ForceMode.Impulse);
				rigidbody.AddForce(new Vector3(0, 0, Collection.gameObject.GetComponent<Player>().friction * Collection.gameObject.GetComponent<Player>().inputSpeed * 
				                               Collection.gameObject.GetComponent<Player>().speed), ForceMode.Impulse);
			} else if (Collection.gameObject.name == "Player Right") {
				rigidbody.AddForce(new Vector3(0, 0, Collection.gameObject.GetComponent<Player>().friction * Collection.gameObject.GetComponent<Player>().inputSpeed * 
				                               Collection.gameObject.GetComponent<Player>().speed), ForceMode.Impulse);
			} else if (Collection.gameObject.name == "Orange_Goal") {
				int points = (int)(otherScore.GetComponent<Scores>().getScore () * goalPointPercentage);
				score.GetComponent<Scores>().AddScore(points);
				otherScore.GetComponent<Scores>().AddScore(-1*points);
				otherScore.GetComponent<Scores>().RemoveLife();
				dropBall(dropLocation);
			} else if (Collection.gameObject.name == "Green_Goal") {
				int points = (int)(score.GetComponent<Scores>().getScore () * goalPointPercentage);
				score.GetComponent<Scores>().AddScore(-1*points);
				score.GetComponent<Scores>().RemoveLife();
				dropBall(dropLocation);
			}
		} else if (ball == eBall.Right) {
			if(Collection.gameObject.name == "Brick") {
				audio.Play();
				Destroy(Collection.gameObject);
				rigidbody.AddForce(rightImpulse, ForceMode.Impulse);
				score.GetComponent<Scores>().AddScore(normalBrickScore);
			} else if (Collection.gameObject.name == "Brick_G") {
				GameObject.Find("ExplosionSound").audio.Play();
				Destroy(Collection.gameObject);
				score.GetComponent<Scores>().AddScore(goalBrickScore);
			} else if (Collection.gameObject.name == "Brick_O") {
				GameObject.Find("ExplosionSound").audio.Play();
				Destroy(Collection.gameObject);
			} else if (Collection.gameObject.name == "Player Right") {
				rigidbody.AddForce(leftImpulse, ForceMode.Impulse);
				rigidbody.AddForce(new Vector3(0, 0, Collection.gameObject.GetComponent<Player>().friction * Collection.gameObject.GetComponent<Player>().inputSpeed * 
				                               Collection.gameObject.GetComponent<Player>().speed), ForceMode.Impulse);
			} else if (Collection.gameObject.name == "Player Left") {
				rigidbody.AddForce(new Vector3(0, 0, Collection.gameObject.GetComponent<Player>().friction * Collection.gameObject.GetComponent<Player>().inputSpeed * 
				                               Collection.gameObject.GetComponent<Player>().speed), ForceMode.Impulse);
			} else if (Collection.gameObject.name == "Green_Goal") {
				int points = (int)(otherScore.GetComponent<Scores>().getScore() * goalPointPercentage);
				score.GetComponent<Scores>().AddScore(points);
				otherScore.GetComponent<Scores>().AddScore(-1*points);
				otherScore.GetComponent<Scores>().RemoveLife();
				dropBall(dropLocation);
			} else if (Collection.gameObject.name == "Orange_Goal" && ball != eBall.F_Left && ball != eBall.F_Right) {
				int points = (int)(score.GetComponent<Scores>().getScore() * goalPointPercentage);
				score.GetComponent<Scores>().AddScore(-1*points);
				score.GetComponent<Scores>().RemoveLife();
				dropBall(dropLocation);
			}
		} 
		//FIREBALLS
		else if (ball == eBall.F_Left){
			if (Collection.gameObject.name == "Brick") {
				audio.Play();
				Destroy(Collection.gameObject);
				rigidbody.AddForce(rightImpulse_F, ForceMode.Impulse);

			}
		}
		//FIREBALLS
		else if (ball == eBall.F_Right){
			if (Collection.gameObject.name == "Brick") {
				audio.Play();
				Destroy(Collection.gameObject);
				rigidbody.AddForce(leftImpulse_F, ForceMode.Impulse);
			}
		}
	}

	void dropBall(Vector3 location) {
		rigidbody.transform.position = location;
		rigidbody.constraints = RigidbodyConstraints.None;
		collider.isTrigger = true;
		rigidbody.AddForce(-rigidbody.velocity + new Vector3(0, dropSpeed, 0), ForceMode.Impulse);
	}
}
