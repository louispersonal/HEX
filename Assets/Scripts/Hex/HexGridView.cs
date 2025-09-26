using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

public class HexGridView : MonoBehaviour
{
	HashSet<AxialCoordinate> _needNow;
	HashSet<AxialCoordinate> _bufferBand;
	HashSet<AxialCoordinate> _liveCoords;
	Dictionary<AxialCoordinate, HexView> _liveHexes;
	ObjectPool<HexView> _hexPool;
	Dictionary<AxialCoordinate, float> _graceUntil;
	HashSet<AxialCoordinate> _target;
	List<AxialCoordinate> _spawnList;
    List<AxialCoordinate> _despawnNow;
    int _bufferSize = 2;
	int _radius;
	(float q, float r) _cameraCenter;
	float _despawnGraceSeconds = 5f;

	[SerializeField] HexView _hexViewPrefab;
	
	private void Awake()
	{
		_needNow = new HashSet<AxialCoordinate>();
		_bufferBand = new HashSet<AxialCoordinate>();
		_liveCoords = new HashSet<AxialCoordinate>();
		_liveHexes = new Dictionary<AxialCoordinate, HexView>();
		_graceUntil = new Dictionary<AxialCoordinate, float>();
		_target     = new HashSet<AxialCoordinate>();
		_spawnList  = new List<AxialCoordinate>();
		_despawnNow = new List<AxialCoordinate>();

		_hexPool = new ObjectPool<HexView>(CreateHex, OnTakeFromPool, OnReturnedToPool, OnDestroyPooledObject, collectionCheck:false, defaultCapacity:100, maxSize:500);
	}
	
	private HexView CreateHex()
	{
		HexView hexView = Instantiate(_hexViewPrefab);
		hexView.gameObject.SetActive(false);
		return hexView;
	}

	private void OnTakeFromPool(HexView hexView)
	{
		hexView.gameObject.SetActive(true);
	}

	private void OnReturnedToPool(HexView hexView)
	{
		hexView.gameObject.SetActive(false);
	}

    void OnDestroyPooledObject(HexView hexView)
    {
        Destroy(hexView.gameObject);
    }

    private void Update()
	{
		Camera cam = Camera.main;
		float planeZ = 0f;
		
		(float q, float r) newCenter = SceneToFractionalAxialConversion(ProjectViewportToPlane(cam, new Vector2(0.5f, 0.5f), planeZ));
		int newRadius = ComputeRadiusFractionalAxial(newCenter);
		
		if (DistanceBetweenFractionalAxialCoords(newCenter, _cameraCenter) < 0.1 && newRadius == _radius)
		{
			_cameraCenter = newCenter;
			_radius = newRadius;
			return;
		}
		
		_cameraCenter = newCenter;
		_radius = newRadius;
		
		// Proceed with updating sets
		UpdateNeedNowSet();
		UpdateBufferBandSet();

		SyncPoolWithTarget();
	}
	
	public void UpdateNeedNowSet()
	{
		AxialCoordinate cameraCoord = new AxialCoordinate(Mathf.RoundToInt(_cameraCenter.q), Mathf.RoundToInt(_cameraCenter.r));
		_needNow = AxialCoordinate.CoordsWithinRadiusOfCoord(cameraCoord, _radius).ToHashSet();
	}
	
	public void UpdateBufferBandSet()
	{
		AxialCoordinate cameraCoord = new AxialCoordinate(Mathf.RoundToInt(_cameraCenter.q), Mathf.RoundToInt(_cameraCenter.r));
		_bufferBand = AxialCoordinate.CoordsInRingsOfRadii(cameraCoord, _radius + 1, _radius + _bufferSize).ToHashSet();
	}
	
	void BuildTarget()
	{
		_target.Clear();
		_target.UnionWith(_needNow);
		_target.UnionWith(_bufferBand);
	}
	
	void SyncPoolWithTarget()
	{
		BuildTarget();

		_spawnList.Clear();
		_despawnNow.Clear();

		foreach (var c in _target)
		{
            if (!_liveCoords.Contains(c))
			{
                _spawnList.Add(c);
            }
        }

		float now = Time.time;
		foreach (var c in _liveCoords)
		{
			if (_target.Contains(c))
			{
				if (_graceUntil.ContainsKey(c))
				{
                    _graceUntil.Remove(c);
                }
			}
			else
			{
				if (!_graceUntil.TryGetValue(c, out float until))
				{
                    _graceUntil[c] = now + _despawnGraceSeconds;
                }
			}
		}

		foreach (var kv in _graceUntil)
		{
            if (kv.Value <= now && !_target.Contains(kv.Key))
			{
                _despawnNow.Add(kv.Key);
            }
        }

        AxialCoordinate cameraCoord = new AxialCoordinate(Mathf.RoundToInt(_cameraCenter.q), Mathf.RoundToInt(_cameraCenter.r));

        _spawnList.Sort((a,b) =>
		{
			int da = Mathf.RoundToInt(AxialCoordinate.DistanceBetweenCoords(a, cameraCoord));
			int db = Mathf.RoundToInt(AxialCoordinate.DistanceBetweenCoords(b, cameraCoord));
			return da.CompareTo(db);
		});
		foreach (var c in _spawnList)
		{
            SpawnCoord(c);
        }

		foreach (var c in _despawnNow)
		{
			DespawnCoord(c);
			_graceUntil.Remove(c);
		}
	}

	void SpawnCoord(AxialCoordinate c)
	{
		if (_liveCoords.Contains(c)) return;

		if (!BaseHexGrid.Instance.TryGetHex(c, out BaseHex data)) return;
		HexView view = _hexPool.Get();
		view.Initialize(data);
		_liveHexes[c] = view;
		_liveCoords.Add(c);
	}

	void DespawnCoord(AxialCoordinate c)
	{
		if (!_liveCoords.Contains(c)) return;

        HexView view = _liveHexes[c];
		_liveHexes.Remove(c);
		_liveCoords.Remove(c);
		view.Terminate();
		_hexPool.Release(view);
	}

	public int ComputeRadiusFractionalAxial((float q, float r) center)
	{
		Camera cam = Camera.main;
		float planeZ = 0f;
		
		(float q, float r) bl = SceneToFractionalAxialConversion(ProjectViewportToPlane(cam, new Vector2(0f, 0f), planeZ));
		(float q, float r) br = SceneToFractionalAxialConversion(ProjectViewportToPlane(cam, new Vector2(1f, 0f), planeZ));
		(float q, float r) tl = SceneToFractionalAxialConversion(ProjectViewportToPlane(cam, new Vector2(0f, 1f), planeZ));
		(float q, float r) tr = SceneToFractionalAxialConversion(ProjectViewportToPlane(cam, new Vector2(1f, 1f), planeZ));
		
		return Mathf.CeilToInt(Mathf.Max(DistanceBetweenFractionalAxialCoords(bl,center), DistanceBetweenFractionalAxialCoords(br,center), DistanceBetweenFractionalAxialCoords(tl,center), DistanceBetweenFractionalAxialCoords(tr,center)));
	}
	
	public static (float q, float r) SceneToFractionalAxialConversion(Vector3 p)
    {
        float r = p.y * (2f / (3f * BaseHex.SceneSize));
        float q = (p.x / (Mathf.Sqrt(3f) * BaseHex.SceneSize)) - (p.y / (3f * BaseHex.SceneSize));

        return (q, r);
    }
	
	public static float DistanceBetweenFractionalAxialCoords((float q, float r) a, (float q, float r) b)
	{
		(float q, float r) diff = (a.q - b.q, a.r - b.r);
        return (Mathf.Abs(diff.q) + Mathf.Abs(diff.r) + Mathf.Abs(diff.q + diff.r)) / 2;
	}
	
    private static Vector3 ProjectViewportToPlane(Camera cam, Vector2 viewport01, float PlaneZ)
    {
        var plane = new Plane(Vector3.forward, new Vector3(0f, 0f, PlaneZ));
        Ray ray = cam.ViewportPointToRay(new Vector3(viewport01.x, viewport01.y, 0f));
        if (plane.Raycast(ray, out float enter))
            return ray.GetPoint(enter);
        return cam.transform.position + ray.direction * 1000f;
    }
}