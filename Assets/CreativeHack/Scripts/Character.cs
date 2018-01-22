using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Rigidbody2D))]
public class Character : Agent {

	public List<float> res = new List<float>(2808);
	public GenerateMap map;
	public GameObject projectilePrefab;

	private bool shooting {get {return shootDirection.HasValue;}}
	private bool moving {get {return moveDirection.HasValue;}}
	private Vector2? shootDirection;
	private Vector2? moveDirection;

	private Rigidbody2D rigid;

	public float moveSpeed = 5;
	public float shootDelay = 0.2f;

	private float lastShoot = 0;

	private float startTime = 0;

	private List<Projectile> projectiles = new List<Projectile>();

	public override List<float> CollectState(){
		var result = res;
		res.Clear();
		int team = GetTeam();
		result.AddRange(map.GetStates(team));
		// result.AddRange(map.GetVelocities());

		result.Add(transform.localPosition.x);
		result.Add(transform.localPosition.y);

		result.Add(rigid.velocity.x);
		result.Add(rigid.velocity.y);

		if (team == 0) {
			result.Add(map.Team2.transform.localPosition.x);
			result.Add(map.Team2.transform.localPosition.y);
			result.Add(map.Team2.GetComponent<Rigidbody2D>().velocity.x);
			result.Add(map.Team2.GetComponent<Rigidbody2D>().velocity.y);
		} else {
			result.Add(map.Team1.transform.localPosition.x);
			result.Add(map.Team1.transform.localPosition.y);
			result.Add(map.Team1.GetComponent<Rigidbody2D>().velocity.x);
			result.Add(map.Team1.GetComponent<Rigidbody2D>().velocity.y);
		}
		return result;
	}

    public override void InitializeAgent()
    {
		base.InitializeAgent();
		rigid = GetComponent<Rigidbody2D>();
    }

	// Use this for initialization
	public override void AgentReset () {
		float x, y;
		x = Random.Range(map.BottomLeft.x, map.TopRight.x);
		y = Random.Range(map.BottomLeft.y, map.TopRight.y);
		while ((new Vector3(x, y, transform.localPosition.z) - map.GetOpponent(gameObject).transform.localPosition).magnitude < map.TopRight.magnitude/1.25f) {
			x = Random.Range(map.BottomLeft.x, map.TopRight.x);
			y = Random.Range(map.BottomLeft.y, map.TopRight.y);
		}
		transform.localPosition = new Vector3(x, y, transform.localPosition.z);
		foreach(var p in projectiles) {
			if (p != null && p.Creator == this) {
				p.Disable();
			}
		}
		projectiles.Clear();
		startTime = Time.unscaledTime;
		rigid.velocity = Vector2.zero;
	}

	public override void AgentOnDone() {
	}
	
	// Update is called once per frame
	public override void AgentStep (float[] action) {
		// if (!done) {
		// 	reward = -0.005f;
		// }
		//User Input
        if (brain.brainType == BrainType.Player)
        {
            if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f)
            {
                action[0] = Input.GetAxis("Horizontal");
            } else
            {
                action[0] = float.NaN;
            }
            if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f)
            {
                action[1] = Input.GetAxis("Vertical");
            } else
            {
                action[1] = float.NaN;
            }
            
            Vector2 position = Input.mousePosition;
            Vector2 pos = Camera.main.WorldToScreenPoint(transform.position);
            var delta = position - pos;
            action[2] = delta.x;
            action[3] = delta.y;
        }

		if (action.Length > 3)
        {
            if (float.IsNaN(action[0]) && float.IsNaN(action[1]))
            {
                moveDirection = null;
            }
            else
            {
                moveDirection = new Vector2(float.IsNaN(action[0]) ? 0 : action[0], float.IsNaN(action[1]) ? 0 : action[1]).normalized;
            }
            if (float.IsNaN(action[2]) && float.IsNaN(action[3]))
            {
                shootDirection = null;
            }
            else
            {
                shootDirection = new Vector2(float.IsNaN(action[2]) ? 0 : action[2], float.IsNaN(action[3]) ? 0 : action[3]).normalized;
            }
		}
		if (shooting) {
			Shoot(shootDirection.Value);
		}
		if (moving) {
			Move(moveDirection.Value);
		}
		if (rigid.velocity.magnitude > moveSpeed) {
			rigid.velocity = rigid.velocity.normalized * moveSpeed;
		}
		transform.rotation = Quaternion.FromToRotation(Vector3.up, rigid.velocity);
	}

	void Shoot(Vector2 direction) {
		if (Time.time - lastShoot < shootDelay) {
			return;
		}
		var pro = Projectile.CreateProjectile(this, direction.normalized, projectilePrefab);
		lastShoot = Time.time;
		projectiles.Add(pro);
	}

	void Move(Vector2 direction) {
		var dir = direction.normalized;
		rigid.AddForce(dir * moveSpeed * Time.deltaTime * 50);
	}

	private static int[] kills = new int[2];
	
	void GetHit(Character other) {
		// print("got hit");
		done = true;
		reward = -1f;
		other.SendMessage("Killed", this);
	}

	void Killed(Character other) {
		done = true;
		reward = 1f;
		kills[GetTeam()] += 1;
		print($"{kills[0]}:{kills[1]}");
		if (UiScript.Instance != null) {
			UiScript.Instance.SetScores(kills[0], kills[1]);
		}
		// Some useless Azure integration
		// var request = new UnityWebRequest($"http://botfight-evolution.azurewebsites.net/save?bot={brain.brainType == BrainType.Player}&duration={GetDuration()}&winningTeam={GetTeam()}");
		// request.SendWebRequest();
	}

	float GetDuration() {
		return Time.unscaledTime - startTime;
	}

	int GetTeam() {
		return map.Team1 == gameObject ? 0 : 1;
	}
}
