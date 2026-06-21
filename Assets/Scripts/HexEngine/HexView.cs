using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexView : MonoBehaviour
{
	public HexData Data { get; private set; }

    public WorldData WorldData { get { return Data.WorldData; } }

    [SerializeField] private SpriteRenderer _spriteRenderer;

	[SerializeField] ParticleSystem _lowVegetationParticles;
	[SerializeField] int _maxLowVegetationParticles;
	[SerializeField] ParticleSystem _highVegetationParticles;
    [SerializeField] int _maxHighVegetationParticles;
    [SerializeField] private SpriteRenderer _elevationOverlayRenderer;

    [SerializeField] List<Sprite> _elevationOverlays;
	[SerializeField] Color _seaColor;

	[SerializeField] Sprite _desertSprite;
	[SerializeField] Sprite _tundraSprite;
	[SerializeField] Sprite _taigaSprite;
	[SerializeField] Sprite _tropicalSprite;
	[SerializeField] Sprite _temperateSprite;
	[SerializeField] Sprite _steppeSprite;
	[SerializeField] Sprite _savannaSprite;

	[SerializeField] RiverOverlayController _riverOverlay;

    public static float SceneSize = 1.15f; //1 unit in unity world space

    public void Initialize(HexData data)
	{
		Data = data;
		gameObject.transform.position = HexGridGeometry.AxialToScene(Data.Coord);
		StartCoroutine(ParticleBurstAndFreeze(_lowVegetationParticles, Mathf.RoundToInt(Data.ExtraData.LowVegetation * _maxLowVegetationParticles)));
		StartCoroutine(ParticleBurstAndFreeze(_highVegetationParticles, Mathf.RoundToInt(Data.ExtraData.HighVegetation * _maxHighVegetationParticles)));
        _elevationOverlayRenderer.sprite = _elevationOverlays[Mathf.RoundToInt(Data.ExtraData.Elevation * (_elevationOverlays.Count - 1))];
		SetSprite();

		if (Data.ExtraData.IsSea)
		{
            _spriteRenderer.color = _seaColor;
        }
		else
		{
            _spriteRenderer.color = Color.white;
        }

		_riverOverlay.InitializeOverlays(Data);
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

	private void SetSprite()
	{
		switch (Data.ExtraData.Biome)
		{
			case Biome.Desert:
                _spriteRenderer.sprite = _desertSprite;
                break;
            case Biome.Tundra:
                _spriteRenderer.sprite = _tundraSprite;
                break;
            case Biome.Taiga:
                _spriteRenderer.sprite = _taigaSprite;
                break;
            case Biome.Tropical:
                _spriteRenderer.sprite = _tropicalSprite;
                break;
            case Biome.Temperate:
                _spriteRenderer.sprite = _temperateSprite;
                break;
            case Biome.Steppe:
                _spriteRenderer.sprite = _steppeSprite;
                break;
            case Biome.Savanna:
                _spriteRenderer.sprite = _savannaSprite;
                break;
			default:
				_spriteRenderer.sprite = _desertSprite;
				break;
        }
	}
}