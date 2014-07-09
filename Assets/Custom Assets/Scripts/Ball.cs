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
	float goalPointPercentage = 0.20f;
	float currentDropDelay;


	// Use this for initialization
	void Start () {
		leftImpulse_F = new Vector3(-24, 0, Random.Range (-3.0f, 3.0f));
		rightImpulse_F = new Vector3(24, 0, Random.Range (-3.0f, 3.0f));

		if (ball == eBall.F_Left || ball == eBall.F_Right) {
			Destroy(gameObject, 2.0f);
			initialDropDelay = -2000;
			if (ball == eBall.F_Right) rigidbody.AddForce(leftImpulse_F, ForceMode.Impulse);
			else rigidbody.AddForce(rightImpulse_F, ForceMode.Impulse);
		}


		currentDropDelay = initialDropDelay;
	}

	// Update is called once per frame
	void Update () {
		if (ball != eBall.F_Left && ball != eBall.F_Right) {
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
					score.GetComponent<Scores> ().increaseMultiplier ();
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
		}
	}


	void OnCollisionEnter(Collision Collection) {
		if (ball == eBall.Left) {
			if(Collection.gameObject.name == "Brick") {
				audio.Play();
				Destroy(Collection.gameObject);
				score.GetComponent<Scores>().AddScore(normalBrickScore);
			} else if (Collection.gameObject.name == "Brick_O") {
				Destroy(Collection.gameObject);
				score.GetComponent<Scores>().AddScore(goalBrickScore);
			} else if (Collection.gameObject.name == "Brick_G") {
				Destroy(Collection.gameObject);
			} else if (Collection.gameObject.name == "Player Left") {
				Player player = Collection.gameObject.GetComponent<Player>();
				if (player.transform.localPosition.z + player.transform.localScale.z / 2 > transform.localPosition.z ||
				    player.transform.localPosition.z - player.transform.localScale.z / 2 < transform.localPosition.z) {
					rigidbody.AddForce(rightImpulse, ForceMode.Impulse);
					rigidbody.AddForce(new Vector3(0, 0, Collection.gameObject.GetComponent<Player>().friction * (Mathf.Abs(transform.localPosition.z - player.transform.localPosition.z) / (player.transform.localScale.z / 2)) * Collection.gameObject.GetComponent<Player>().inputSpeed * 
						                               Collection.gameObject.GetComponent<Player>().speed), ForceMode.Impulse);
				}
			} else if (Collection.gameObject.name == "Player Right") {
				Player player = Collection.gameObject.GetComponent<Player>();
				if (player.transform.localPosition.z + player.transform.localScale.z / 2 > transform.localPosition.z ||
				    player.transform.localPosition.z - player.transform.localScale.z / 2 < transform.localPosition.z) {
					rigidbody.AddForce(new Vector3(0, 0, Collection.gameObject.GetComponent<Player>().friction * (Mathf.Abs(transform.localPosition.z - player.transform.localPosition.z) / (player.transform.localScale.z / 2)) * Collection.gameObject.GetComponent<Player>().inputSpeed * 
					                               Collection.gameObject.GetComponent<Player>().speed), ForceMode.Impulse);
				}
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
			} else if (Collection.gameObject.name == "Big Wall") {
				rigidbody.AddForce(leftImpulse, ForceMode.Impulse);
			}
		} else if (ball == eBall.Right) {
			if(Collection.gameObject.name == "Brick") {
				audio.Play();
				Destroy(Collection.gameObject);
				score.GetComponent<Scores>().AddScore(normalBrickScore);
			} else if (Collection.gameObject.name == "Brick_G") {
				Destroy(Collection.gameObject);
				score.GetComponent<Scores>().AddScore(goalBrickScore);
			} else if (Collection.gameObject.name == "Brick_O") {
				Destroy(Collection.gameObject);
			} else if (Collection.gameObject.name == "Player Right") {
				Player player = Collection.gameObject.GetComponent<Player>();
				if (player.transform.localPosition.z + player.transform.localScale.z / 2 > transform.localPosition.z ||
				    player.transform.localPosition.z - player.transform.localScale.z / 2 < transform.localPosition.z) {
					rigidbody.AddForce(leftImpulse, ForceMode.Impulse);
					rigidbody.AddForce(new Vector3(0, 0, Collection.gameObject.GetComponent<Player>().friction * (Mathf.Abs(transform.localPosition.z - player.transform.localPosition.z) / (player.transform.localScale.z / 2)) * Collection.gameObject.GetComponent<Player>().inputSpeed * 
					                               Collection.gameObject.GetComponent<Player>().speed), ForceMode.Impulse);
				}
			} else if (Collection.gameObject.name == "Player Left") {
				Player player = Collection.gameObject.GetComponent<Player>();
				if (player.transform.localPosition.z + player.transform.localScale.z / 2 > transform.localPosition.z ||
				    player.transform.localPosition.z - player.transform.localScale.z / 2 < transform.localPosition.z) {
					rigidbody.AddForce(new Vector3(0, 0, Collection.gameObject.GetComponent<Player>().friction * (Mathf.Abs(transform.localPosition.z - player.transform.localPosition.z) / (player.transform.localScale.z / 2)) * Collection.gameObject.GetComponent<Player>().inputSpeed * 
					                               Collection.gameObject.GetComponent<Player>().speed), ForceMode.Impulse);
				}
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
			} else if (Collection.gameObject.name == "Big Wall") {
				rigidbody.AddForce(rightImpulse, ForceMode.Impulse);
			}
		} 
		//FIREBALLS
		else if (ball == eBall.F_Left){
			if (Collection.gameObject.name == "Brick") {
				audio.Play();
				Destroy(Collection.gameObject);
				rigidbody.AddForce(-rigidbody.velocity + rightImpulse_F, ForceMode.Impulse);
				score.GetComponent<Scores>().AddScore(normalBrickScore);
			}
		}
		//FIREBALLS
		else if (ball == eBall.F_Right){
			if (Collection.gameObject.name == "Brick") {
				audio.Play();
				Destroy(Collection.gameObject);
				rigidbody.AddForce(-rigidbody.velocity + leftImpulse_F, ForceMode.Impulse);
				score.GetComponent<Scores>().AddScore(normalBrickScore);
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
