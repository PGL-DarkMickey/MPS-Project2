using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;

public class InputReceiver : MonoBehaviour {


	// TODO:
	// adjust score receiving to include other factors such as visual effects
	// add restart button
	// add new levels and words to database

	public int nr_levels;
	public int strikes;
	private List<List<string>> words;//cate o lista de cuvinte pentru fiecare nivel
	private Text display_text;
	private string input_text;
	private int p;
	private int level;
	private bool active;
	private int current,previous;
	private int score;
	private int written_words;
	private bool game_over;

	void SetNewWord(){
		previous = current;
		while (current==previous)
			current = Random.Range (0, words[level].Count - 1);

		input_text = words[level][current];
		display_text.text = input_text;
		display_text.color = Color.white;
		p = 0;//reset position in text
	}

	void readFromFile(string file){
		for (int i = 0; i < nr_levels; i++)
			words.Add (new List<string> ());
		
		StreamReader sr = new StreamReader(file);
		string line;
		List<string> list;
		int l = 0;

		while(!sr.EndOfStream){
			line = sr.ReadLine( );
			list = new List<string>(line.Split(' '));
			foreach (string word in list) {
				words[l].Add (word);
			}
			l++;
		}

		sr.Close( );
	}

	// Use this for initialization
	void Start () {
		display_text = GetComponent<Text> ();
		display_text.supportRichText=true;

		GameObject.Find ("Score").GetComponent<Text> ().text = "";

		words = new List<List<string>> ();
		active = true;
		game_over = false;
		level = 0;
		score = 0;
		written_words = 0;
		current = -1; previous=-1;
		readFromFile ("words-database");
		SetNewWord ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!active || game_over)
			return;
		string aux;
		int i;
		if (Input.anyKeyDown && !Input.GetMouseButton(0) && !Input.GetMouseButton(1)) {
			if (Input.GetKeyDown (input_text [p].ToString ())) {
				aux = input_text [p].ToString ();
				i = 26 * p;
				display_text.text = display_text.text.Remove (i, 1);
				display_text.text = display_text.text.Insert (i, "?");
				display_text.text = display_text.text.Replace ("?", "<color=#00ff00ff>" + aux + "</color>");

				p++;
				if (p == input_text.Length) {//when player successfully writes the word
					written_words+=1;
					score += (level+1)*input_text.Length;

					if (written_words == (level+1) * 5) {//when player writes enough words he advances to the next level
						level++;
						previous = -1;
						current = -1;
						if (level == nr_levels) {
							display_text.text = "YOU WON!";
							display_text.color = Color.green;
							display_text.fontSize = 100;
							GameObject.Find ("Score").GetComponent<Text> ().text = "Final score: "+score;
							GameObject.Find ("Score").GetComponent<Text> ().color = Color.green;
							game_over=true;
							return;
						}
					}
					SetNewWord ();
				}
			}
			else StartCoroutine(ErrorWait());
		}
	}

	IEnumerator ErrorWait() {
		strikes -= 1;
		if (strikes == 0) {
			game_over = true;
			display_text.text = "YOU LOST!";
			display_text.color = Color.red;
			display_text.fontSize = 100;
			GameObject.Find ("Score").GetComponent<Text> ().text = "Final score: "+score;
			GameObject.Find ("Score").GetComponent<Text> ().color = Color.red;

			print ("Done");
		}
		else {
			active = false;
			display_text.text = input_text;
			display_text.color = Color.red;
			yield return new WaitForSeconds (1);
			SetNewWord ();
			active = true;
		}
	}
}
