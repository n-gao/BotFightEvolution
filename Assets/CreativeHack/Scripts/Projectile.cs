using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	private static List<Projectile> _projectiles = new List<Projectile>();
	public Character Creator;
	public Vector2 Direction;

	private Rigidbody2D rigid;
	private bool active;

	public static Projectile CreateProjectile(Character creator, Vector3 dir, GameObject prefab) {
		foreach(var pro in _projectiles) {
			if (!pro.active) {
				pro.Enable(creator, dir);
				return pro;
			}
		}
		var n = GameObject.Instantiate(prefab);
		var p = n.GetComponent<Projectile>();
		p.Enable(creator, dir);
		return p;
	}

	void Awake() {
		_projectiles.Add(this);
	}

	// Use this for initialization
	void OnEnable() {
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Projectile" || other.tag == "Tile" || other.gameObject == Creator.gameObject)
			return;
		Disable();
		if (other.tag == "Player")
			other.SendMessage("GetHit", Creator);
	}

	public void Disable() {
		rigid.Sleep();
		this.active = false;
		_projectiles.Add(this);
		rigid.transform.position = new Vector3(-1000, -1000);
	}

	void Enable(Character Creator, Vector3 dir) {
		this.Creator = Creator;
		this.Direction = dir;
		this.active = true;
		_projectiles.Remove(this);
		transform.position = Creator.transform.position;
		transform.rotation = Quaternion.FromToRotation(Vector2.down, Direction);
		rigid = GetComponent<Rigidbody2D>();
		rigid.WakeUp();
		rigid.velocity =  new Vector3(Direction.x, Direction.y).normalized;
	}
}
