using UnityEngine;
using System.Collections;
using System.Linq;
using Assets.Unitility.SQLite;

public class ExampleScript : MonoBehaviour {

    ExampleSQLiteAccessor _db = new ExampleSQLiteAccessor();

    // Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.Space))
	    {
	        _db.AddScore("Player 1", Random.Range(0,2), Random.Range(100, 10000));
	        
            Debug.Log(string.Format(
                "Highscore for Player 1 for level 0: {0}", 
                _db.GetHighscoreForLevel("Player 1", 0).Score
                ));
	        Debug.Log(string.Format(
	            "All highscores for Player 1: {0}",
	            string.Join(",",
	                _db.GetHighscores("Player 1")
	                    .Select(score => string.Format("Level {0}: {1}", score.Level, score.Score))
	                    .ToArray())

	            ));
	    }
	}
}
