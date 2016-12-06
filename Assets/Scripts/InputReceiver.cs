using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;

public class InputReceiver : MonoBehaviour {

	private string[] words={"papirus","ornitorinc","asparagus","dactilografie","asparagus","laringolog","serotonina","manuscript","vindicativ"};
	private Text display_text;
	private string input_text;
	private int p;
	private bool active = true;
	private int current=-1,previous=-1;

	void SetNewWord(){
		previous = current;
		while (current==previous)
			current = Random.Range (0, words.Length - 1);

		input_text = words [current];
		display_text.text = input_text;
		display_text.color = Color.white;
		p = 0;
	}
	// Use this for initialization
	void Start () {
		display_text = GetComponent<Text> ();
		display_text.supportRichText=true;

		SetNewWord ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!active)
			return;
		string aux;
		int i;
		if (Input.anyKeyDown) {
			if (Input.GetKeyDown (input_text [p].ToString ())) {
				aux = input_text [p].ToString ();
				i = 26 * p;
				display_text.text = display_text.text.Remove (i, 1);
				display_text.text = display_text.text.Insert (i, "?");
				display_text.text = display_text.text.Replace ("?", "<color=#00ff00ff>" + aux + "</color>");

				p++;
				if (p == input_text.Length) SetNewWord ();
			}
			else StartCoroutine(ErrorWait());
		}
	}

	IEnumerator ErrorWait() {
		active = false;
		display_text.text = input_text;
		display_text.color = Color.red;
		yield return new WaitForSeconds(1);
		SetNewWord ();
		active = true;
	}
}
