using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreativeHackGoodDecision : MonoBehaviour, Decision {

	public float[] Decide(List<float> state, List<Camera> observation, float reward, bool done, float[] memory)
	{
		var myPos = new Vector2(state[2800], state[2801]);
		var enemyPos = new Vector2(state[2804], state[2805]);
		var enemyVel = new Vector2(state[2806], state[2807]);
		var estimate = enemyPos + enemyVel;
		var dir = (estimate - myPos).normalized;
		return new float[]{
			Random.Range(-1f, 1f),
		 	Random.Range(-1f, 1f),
		 	dir.x,
			dir.y
			};
	}

    public float[] MakeMemory(List<float> state, List<Camera> observation, float reward, bool done, float[] memory)
    {
        return new float[0];
    }
}
