using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeartsDisplay : MonoBehaviour {

	public GameObject heart;
	public GameObject le_input;
	public GameObject flame;

	[HideInInspector]
	public int last_strikes=0;

	private List<GameObject> hearts = new List<GameObject>();
	private bool flame_active=false;
	private GameObject my_flame;

	// Update is called once per frame
	void Update () {
		int strikes_left=le_input.GetComponent<InputReceiver>().strikes_left;
		int i;
		int x = -58, y = -31, z=50;
		int level = le_input.GetComponent<InputReceiver> ().level;
		int nr_levels = le_input.GetComponent<InputReceiver> ().nr_levels;

		if (level < nr_levels) {
			if (last_strikes < strikes_left)
				for (i = last_strikes; i < strikes_left; i++) {
					GameObject h;
					h = Instantiate (heart, new Vector3 (x + i * 5, y, z), Quaternion.identity) as GameObject;
					hearts.Add (h);
				}
			else{
				for (i = last_strikes - 1; i >= strikes_left; i--) {
					Destroy (hearts [i]);
					hearts.RemoveAt (i);
				}
			}
		}

		last_strikes = strikes_left;

		if (level == nr_levels && !flame_active && last_strikes>0) {
			flame_active = true;
			my_flame = Instantiate (flame, new Vector3 (0,0,0), Quaternion.Euler(-90, 0, 0)) as GameObject;
		}
		if (strikes_left == 0 || level==1) {
			flame_active = false;
			Destroy (my_flame);
		}
	}
}
