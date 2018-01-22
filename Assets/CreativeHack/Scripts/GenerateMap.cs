using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class GenerateMap : MonoBehaviour {

	public GameObject tilePrefab;
	public GameObject obstaclePrefab;
	public GameObject[,] tiles;
	private Map map = new Map();

	public GameObject Team1;
	public GameObject Team2;

	public float scaleing = 1/7f;

	public Vector2 BottomLeft {
		get {
			return new Vector2((2-map.MapSizeX/2)*scaleing, (2-map.MapSizeY/2)*scaleing);
		}
	}

	public Vector2 TopRight {
		get {
			return new Vector2((map.MapSizeX/2-2)*scaleing, (map.MapSizeY/2-2)*scaleing);
		}
	}

	// Use this for initialization
	void Start () {
		int x = map.MapSizeX;
		int y = map.MapSizeY;
		tiles = new GameObject[x, y];
		for(int i=0; i < x; i++) {
			for(int j=0; j < y; j++) {
				tiles[i, j] = GameObject.Instantiate(tilePrefab);
				tiles[i, j].transform.parent = transform;
				tiles[i, j].transform.localPosition = new Vector3((i-x/2)*scaleing, (j-y/2)*scaleing, 0);
				tiles[i, j].name = $"tile_{i}_{j}";
				tiles[i, j].GetComponent<TileScript>().MyField = map.Fields[i, j];
				tiles[i, j].GetComponent<TileScript>().Map = this;

				if (i == 0 || i == x-1 || j == 0 || j == y-1) {
					var obst = GameObject.Instantiate(obstaclePrefab);
					obst.transform.parent = transform;
					obst.transform.localPosition = new Vector3((i-x/2)*scaleing, (j-y/2)*scaleing, -0.5f);
					if (j == 0 || j == y-1) {
						obst.transform.rotation = Quaternion.Euler(0, 0, 90);
					}
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private List<float> _stateCache = new List<float>();
	public List<float> GetStates(int team) {
		var fields = map.Fields;
		var result = _stateCache;
		result.Clear();
		foreach(var f in fields) {
			if (f.Status == FieldStatus.Blocked) {
				result.Add(1);
			} else {
				result.Add(0);
			}
			if (f.Status == FieldStatus.Character && f.Team == team) {
				result.Add(1);
			} else {
				result.Add(0);
			}
			if (f.Status == FieldStatus.Character && f.Team != team) {
				result.Add(1);
			} else {
				result.Add(0);
			}
			if (f.Status == FieldStatus.Projectile && f.Team == team) {
				result.Add(1);
			} else {
				result.Add(0);
			}
			if (f.Status == FieldStatus.Projectile && f.Team != team) {
				result.Add(1);
			} else {
				result.Add(0);
			}
			float x = 0;
			float y = 0;
			if (f.Occupier != null) {
				var rigid = f.Occupier.GetComponent<Rigidbody2D>();
				if (rigid != null) {
					var vel = rigid.velocity;
					x = vel.x;
					y = vel.y;
				}
			}
			result.Add(x);
			result.Add(y);
		}
		return result;
	}

	public GameObject GetOpponent(GameObject player){
		return player == Team1 ? Team2 : Team1;
	}

	private List<float> _velCache = new List<float>();
	public List<float> GetVelocities() {
		var fields = map.Fields;
		var result = _velCache;
		result.Clear();
		foreach(var f in fields) {
			float x = 0;
			float y = 0;
			if (f.Occupier != null) {
				var rigid = f.Occupier.GetComponent<Rigidbody2D>();
				if (rigid != null) {
					var vel = rigid.velocity;
					x = vel.x;
					y = vel.y;
				}
			}
			result.Add(x);
			result.Add(y);
		}
		return result;
	}
}
