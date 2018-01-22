using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldGenerator : MonoBehaviour {

	public Brain brain;
	public GameObject mapPrefab;

	public int x = 1;
	public int y = 1;

	// Use this for initialization
	void Awake () {
		for (int i = -x; i < x; i++) {
			for (int j = -y; j < y; j++) {
				var map = GameObject.Instantiate(mapPrefab);
				map.transform.position = new Vector3(i * 5, j * 5, 0);
				var chars = map.GetComponentsInChildren<Character>();
				foreach(var ch in chars) {
					ch.brain = brain;
				}
			}
		}
	}
}
