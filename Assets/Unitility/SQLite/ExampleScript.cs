using UnityEngine;
using System.Collections;
using System.Linq;
using Assets.Unitility.SQLite;

public class ExampleScript : MonoBehaviour {

    ExampleDbAccessor _db = new ExampleDbAccessor();

    // Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.Space))
	    {
	        _db.AddScore("Player 1", Random.Range(0,2), Random.Range(100, 10000));

	        Debug.Log("Top Score for specific level:");
            Debug.Log(_db.GetHighscoreForLevel("Player 1", 0));
	        Debug.Log("All Level Highscores:");
	        Debug.Log(string.Join(",",_db.GetHighscores("Player 1").Select(score => score.ToString()).ToArray()));
	    }
	}
}
