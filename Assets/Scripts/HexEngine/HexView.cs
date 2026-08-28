using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

public class HexView : MonoBehaviour, ISelectable
{
	public HexData Data { get; private set; }

    public WorldData WorldData { get { return Data.WorldData; } }
    public GameData GameData { get { return GameController.Instance.SessionManager.GameData; } }

    [SerializeField] private SpriteRenderer _spriteRenderer;

	[SerializeField] ParticleSystem _lowVegetationParticles;
	[SerializeField] int _maxLowVegetationParticles;
	[SerializeField] ParticleSystem _highVegetationParticles;
    [SerializeField] int _maxHighVegetationParticles;
    [SerializeField] private SpriteRenderer _elevationOverlayRenderer;
    [SerializeField] private SpriteRenderer _geoFeatureRenderer;
    [SerializeField] private SpriteRenderer _lakeRenderer;
    [SerializeField] private SpriteRenderer _outline;

    [SerializeField] List<Sprite> _elevationOverlays;
    [SerializeField] List<Sprite> _geoFeatures;

	[SerializeField] Sprite _desertSprite;
	[SerializeField] Sprite _tundraSprite;
	[SerializeField] Sprite _taigaSprite;
	[SerializeField] Sprite _tropicalSprite;
	[SerializeField] Sprite _temperateSprite;
	[SerializeField] Sprite _steppeSprite;
	[SerializeField] Sprite _savannaSprite;
	[SerializeField] Sprite _seaSprite;

	[SerializeField] RiverOverlayController _riverOverlay;

	[Header("Vegetation Sprites")] 
	[SerializeField] private Sprite[] DesertLowSprites;
	[SerializeField] private Sprite[] DesertHighSprites;
	
	[SerializeField] private Sprite[] TundraLowSprites;
	[SerializeField] private Sprite[] TundraHighSprites;
	
	[SerializeField] private Sprite[] TaigaLowSprites;
	[SerializeField] private Sprite[] TaigaHighSprites;
	
	[SerializeField] private Sprite[] TropicalLowSprites;
	[SerializeField] private Sprite[] TropicalHighSprites;
	
	[SerializeField] private Sprite[] SavannaLowSprites;
	[SerializeField] private Sprite[] SavannaHighSprites;
	
	[SerializeField] private Sprite[] TemperateLowSprites;
	[SerializeField] private Sprite[] TemperateHighSprites;
	
	[SerializeField] private Sprite[] SteppeLowSprites;
	[SerializeField] private Sprite[] SteppeHighSprites;

    public static float SceneSize = 3.695f; //1 unit in unity world space - compute from vertical hex size in pixels/200

    public void Initialize(HexData data, bool enableParticles)
	{
		Data = data;
		gameObject.transform.position = HexGridGeometry.AxialToScene(Data.Coord);
		if (enableParticles)
		{
			EnableParticles();
		}

		else
		{
			DisableParticles();
		}
		
		_elevationOverlayRenderer.sprite = null;
		_geoFeatureRenderer.sprite = null;

        if (WorldData.GeoFeatures.TryGetObjectAt(Data.Coord, out GeoFeature feature))
		{
			int featureSpriteIndex = (int)feature.Type;
			_geoFeatureRenderer.sprite = _geoFeatures[featureSpriteIndex];
		}
		else
		{
			SetElevationSprite();
        }

		SetSprite();

        _spriteRenderer.color = Color.white;

		_riverOverlay.InitializeOverlays(Data);
		
		_lakeRenderer.gameObject.SetActive(false);
		if (WorldData.Lakes.ContainsAt(Data.Coord)) _lakeRenderer.gameObject.SetActive(true);

		if (GameData.Pops.ContainsKey(Data.Coord)) 
			GameSceneController.Instance.AllPopsView.SpawnPop(GameData.Pops[Data.Coord]);
    }

	public void Terminate()
	{
		if (GameData.Pops.ContainsKey(Data.Coord)) 
			GameSceneController.Instance.AllPopsView.DeSpawnPop(Data.Coord);
	}

	private IEnumerator ParticleBurstAndFreeze(ParticleSystem s, int numParticles, bool isLow)
	{
		s.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

		s.useAutoRandomSeed = false;
		s.randomSeed = AxialGeometry.GetSeedFromAxial(Data.Coord, isLow? (uint) 1 :  2);
		Sprite[] sprites = isLow ? GetBiomeLowVegetationSprites() : GetBiomeHighVegetationSprites();
		SwitchParticleTextures(s, sprites);
		
		s.Play();
		s.Emit(numParticles);

		yield return null;

		s.Pause();
	}

	private void SwitchParticleTextures(ParticleSystem s, Sprite[] sprites)
	{
		var textureSheet = s.textureSheetAnimation;

		while (textureSheet.spriteCount > 0)
		{
			textureSheet.RemoveSprite(textureSheet.spriteCount - 1);
		}
		
		foreach (Sprite sprite in sprites)
		{
			textureSheet.AddSprite(sprite);
		}
	}
	
	private void SetElevationSprite()
	{
		if (Data.ExtraData.Elevation < 0.5f)
		{
			_elevationOverlayRenderer.sprite = _elevationOverlays[0];
		}
		else if (Data.ExtraData.Elevation < 0.75f)
		{
			_elevationOverlayRenderer.sprite = _elevationOverlays[1];
		}
		else
		{
			_elevationOverlayRenderer.sprite = _elevationOverlays[2];
		}
	}
	
	private void SetSprite()
	{
		if (Data.ExtraData.IsSea)
		{
			_spriteRenderer.sprite = _seaSprite;
			return;
		}
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

	private Sprite[] GetBiomeLowVegetationSprites()
	{
		switch (Data.ExtraData.Biome)
		{
			case Biome.Desert:
				return DesertLowSprites;
			case Biome.Tundra:
				return TundraLowSprites;
			case Biome.Taiga:
				return TaigaLowSprites;
			case Biome.Tropical:
				return TropicalLowSprites;
			case Biome.Temperate:
				return TemperateLowSprites;
			case Biome.Steppe:
				return SteppeLowSprites;
			case Biome.Savanna:
				return SavannaLowSprites;
			default:
				return DesertLowSprites;
		}
	}
	
	private Sprite[] GetBiomeHighVegetationSprites()
	{
		switch (Data.ExtraData.Biome)
		{
			case Biome.Desert:
				return DesertHighSprites;
			case Biome.Tundra:
				return TundraHighSprites;
			case Biome.Taiga:
				return TaigaHighSprites;
			case Biome.Tropical:
				return TropicalHighSprites;
			case Biome.Temperate:
				return TemperateHighSprites;
			case Biome.Steppe:
				return SteppeHighSprites;
			case Biome.Savanna:
				return SavannaHighSprites;
			default:
				return DesertHighSprites;
		}
	}
	
	//Selection interface members
	
	public void OnSelected()
	{
		_outline.gameObject.SetActive(true);
	}

	public void OnDeselected()
	{
		_outline.gameObject.SetActive(false);
	}

	public void EnableParticles()
	{
		if (Data.ExtraData.IsSea) return;
		
		_lowVegetationParticles.gameObject.SetActive(true);
		_highVegetationParticles.gameObject.SetActive(true);
		StartCoroutine(ParticleBurstAndFreeze(_lowVegetationParticles, 
			Mathf.RoundToInt(Data.ExtraData.LowVegetation * _maxLowVegetationParticles),
			true));
		StartCoroutine(ParticleBurstAndFreeze(_highVegetationParticles, 
			Mathf.RoundToInt(Data.ExtraData.HighVegetation * _maxHighVegetationParticles),
			false));
	}

	public void DisableParticles()
	{
		_lowVegetationParticles.gameObject.SetActive(false);
		_highVegetationParticles.gameObject.SetActive(false);
	}
}