using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class GameController : MonoBehaviour {

	public int TotalEnemies = 8;
	Walk Player;
	public Text ScoreText;
	public Transform EnemyTemplate;

	// Use this for initialization
	void Start () 
	{
		//GameOverText = GetComponent<Text>();
		Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Walk>();
		
	}
	
	// Update is called once per frame
	//void Update () 
	//{
	//	ScoreText.text = string.Format("Score: {0}", Score);
		
	//}

	//public void Respawn()
	//{
	//	Score++;
	//	var spawnPoints = GameObject.FindGameObjectsWithTag("Respawn");
	//	var spawnOne = spawnPoints.ElementAt(Random.Range(0, spawnPoints.Length));
	//	var spawnTwo = spawnPoints.ElementAt(Random.Range(0, spawnPoints.Length));

	//	Instantiate(EnemyTemplate, spawnOne.transform.position, EnemyTemplate.transform.rotation);
	//	Instantiate(EnemyTemplate, spawnTwo.transform.position, EnemyTemplate.transform.rotation);
	//}
}
