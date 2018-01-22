using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreativeHackDecision : MonoBehaviour, Decision {

	public float[] Decide(List<float> state, List<Camera> observation, float reward, bool done, float[] memory)
	{
		return new float[]{
			Random.Range(-1f, 1f),
		 	Random.Range(-1f, 1f),
		 	Random.Range(-1f, 1f),
			Random.Range(-1f, 1f)
			};
	}

    public float[] MakeMemory(List<float> state, List<Camera> observation, float reward, bool done, float[] memory)
    {
        return new float[0];
    }
}
