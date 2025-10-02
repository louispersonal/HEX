using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexView : MonoBehaviour
{
	public Hex Data { get; private set; }

    [SerializeField] private SpriteRenderer _spriteRenderer;

	[SerializeField] ParticleSystem _lowVegetationParticles;
	[SerializeField] int _maxLowVegetationParticles;
	[SerializeField] ParticleSystem _highVegetationParticles;
    [SerializeField] int _maxHighVegetationParticles;

    [SerializeField] List<Sprite> _terrainSprites;
	[SerializeField] List<Color> _terrainColors;
	[SerializeField] Color _seaColor;

	public void Initialize(Hex data)
	{
		Data = data;
		gameObject.transform.position = BaseHexGrid.Instance.AxialToSceneConversion(Data.Coord);
		StartCoroutine(ParticleBurstAndFreeze(_lowVegetationParticles, Mathf.RoundToInt(Data.LowVegetation * _maxLowVegetationParticles)));
		StartCoroutine(ParticleBurstAndFreeze(_highVegetationParticles, Mathf.RoundToInt(Data.HighVegetation * _maxHighVegetationParticles)));
		_spriteRenderer.color = _terrainColors[Mathf.RoundToInt(Data.Precipitation * (_terrainColors.Count - 1))];
		_spriteRenderer.sprite = _terrainSprites[Mathf.RoundToInt(Data.Elevation * (_terrainSprites.Count - 1))];
		if (Data.IsSea) _spriteRenderer.color = _seaColor;
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