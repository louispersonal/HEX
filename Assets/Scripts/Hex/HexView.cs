using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexView : MonoBehaviour
{
	private BaseHex _data;

	[SerializeField] ParticleSystem _lowVegetationParticles;
	[SerializeField] ParticleSystem _highVegetationParticles;

	public void Initialize(BaseHex data)
	{
		_data = data;
		gameObject.transform.position = BaseHexGrid.Instance.AxialToSceneConversion(data.Coord);
		_lowVegetationParticles.Play();
		_lowVegetationParticles.Emit(20);
    }

	public void Terminate()
	{
		
	}
}