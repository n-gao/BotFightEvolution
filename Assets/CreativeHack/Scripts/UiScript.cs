using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UiScript : MonoBehaviour {

	public static UiScript Instance {get; private set;}

	public Text team1;
	public Text team2;

	void Awake() {
		Instance = this;
		SetScores(0, 0);
	}

	public void SetScores(int t1, int t2) {
		team1.text = "" + t1;
		team2.text = "" + t2;
	}
}
