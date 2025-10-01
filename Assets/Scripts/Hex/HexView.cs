using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexView : MonoBehaviour
{
	private Hex _data;

	[SerializeField] ParticleSystem _lowVegetationParticles;
	[SerializeField] ParticleSystem _highVegetationParticles;

	public void Initialize(Hex data)
	{
		_data = data;
		gameObject.transform.position = BaseHexGrid.Instance.AxialToSceneConversion(data.Coord);
		StartCoroutine(ParticleBurstAndFreeze(_lowVegetationParticles, 20));
		StartCoroutine(ParticleBurstAndFreeze(_highVegetationParticles, 5));
    }

	public void Terminate()
	{
		
	}

	private IEnumerator ParticleBurstAndFreeze(ParticleSystem s, int numParticles)
	{
		s.Play();
		s.Emit(numParticles);
		yield return null;
		s.Pause();
	}
}