using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour {

	public GenerateMap Map;
	public Field MyField;

	private List<Collider2D> onField = new List<Collider2D>();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter2D(Collider2D other) {
		onField.Add(other);
		UpdateFieldStatus();
	}

	void OnTriggerExit2D(Collider2D other) {
		onField.Remove(other);
		UpdateFieldStatus();
	}

	void UpdateFieldStatus() {
		int highest = -1;
		FieldStatus status = FieldStatus.Free;
		int team = 0;
		GameObject occ = null;
		foreach(var item in onField) {
			if (item == null)
				continue;
			if (item.gameObject == Map.Team1 && highest < 6) {
				highest = 6;
				status = FieldStatus.Character;
				team = 0;
				occ = item.gameObject;
			}
			if (item.gameObject == Map.Team2 && highest < 6) {
				highest = 6;
				status = FieldStatus.Character;
				team = 1;
				occ = item.gameObject;
			}
			if (item.tag == "Projectile" && highest < 4) {
				highest = 4;
				status = FieldStatus.Projectile;
				if (item.GetComponent<Projectile>().Creator == Map.Team1) {
					team = 0;
				} else {
					team = 1;
				}
				occ = item.gameObject;
			}
			if (item.tag == "Obstacle" && highest < 5) {
				highest = 5;
				status = FieldStatus.Blocked;
				occ = item.gameObject;
			}
		}
		MyField.Status = status;
		MyField.Team = team;
		MyField.Occupier = occ;
	}
}
