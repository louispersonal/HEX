using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

public class HexGridView : MonoBehaviour
{
	public static HexGridView Instance { get; private set; }
	public HexGrid HexGrid { get { return GameController.Instance.SessionManager.WorldData.Grid; } }

	public Vector2[] CameraCorners { get; private set; }
	public Vector2[] WorldCorners { get; private set; }
	
	HashSet<AxialCoordinate> _needNow;
	HashSet<AxialCoordinate> _bufferBand;
	HashSet<AxialCoordinate> _targetCoords;
	HashSet<AxialCoordinate> _liveCoords;
	Dictionary<AxialCoordinate, HexView> _liveHexes;
	ObjectPool<HexView> _hexPool;
	Dictionary<AxialCoordinate, float> _graceUntil;
	List<AxialCoordinate> _spawnList;
    List<AxialCoordinate> _despawnNow;
    int _bufferSize = 2;
	int _radius;
	(float q, float r) _cameraCenter;
	public (float q, float r) CameraCenter { get  { return _cameraCenter; } }

	[SerializeField] HexView _hexViewPrefab;
	
	private void Awake()
	{
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        _needNow = new HashSet<AxialCoordinate>();
		_bufferBand = new HashSet<AxialCoordinate>();
		_targetCoords = new HashSet<AxialCoordinate>();
		_liveCoords = new HashSet<AxialCoordinate>();
		_liveHexes = new Dictionary<AxialCoordinate, HexView>();
		_graceUntil = new Dictionary<AxialCoordinate, float>();
		_spawnList  = new List<AxialCoordinate>();
		_despawnNow = new List<AxialCoordinate>();

		_hexPool = new ObjectPool<HexView>(CreateHex, OnTakeFromPool, OnReturnedToPool, OnDestroyPooledObject, collectionCheck:false, defaultCapacity:100, maxSize:500);

		CameraCorners = new Vector2[4];
	}

	private void Start()
	{
		FindWorldCorners();
	}

	private void FindWorldCorners()
	{
		AxialCoordinate tl = AxialGeometry.OddRToAxial((HexGrid.RowBounds.min, HexGrid.ColBounds.min));
		AxialCoordinate bl = AxialGeometry.OddRToAxial((HexGrid.RowBounds.max, HexGrid.ColBounds.min));
		AxialCoordinate tr = AxialGeometry.OddRToAxial((HexGrid.RowBounds.min, HexGrid.ColBounds.max));
		AxialCoordinate br = AxialGeometry.OddRToAxial((HexGrid.RowBounds.max, HexGrid.ColBounds.max));
		
		WorldCorners = new Vector2[4];
		WorldCorners[0] = HexGridGeometry.AxialToScene(bl);
		WorldCorners[1] = HexGridGeometry.AxialToScene(tl);
		WorldCorners[2] = HexGridGeometry.AxialToScene(tr);
		WorldCorners[3] = HexGridGeometry.AxialToScene(br);
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
		
		(float q, float r) newCenter = HexGridGeometry.SceneToFractionalAxial(ProjectViewportToPlane(cam, new Vector2(0.5f, 0.5f), planeZ));
		int newRadius = ComputeRadiusFractionalAxial(newCenter);
		
		_cameraCenter = newCenter;
		_radius = newRadius;

		// Proceed with updating sets
		AxialCoordinate cameraCoord = new AxialCoordinate(Mathf.RoundToInt(_cameraCenter.q), Mathf.RoundToInt(_cameraCenter.r));
		UpdateNeedNowSet(cameraCoord);
		UpdateBufferBandSet(cameraCoord);

		SyncPoolWithTarget();
	}
	
	public void UpdateNeedNowSet(AxialCoordinate cameraCoord)
	{
		_needNow = AxialGeometry.CoordsWithinRadiusOfCoord(cameraCoord, _radius).ToHashSet();
	}
	
	public void UpdateBufferBandSet(AxialCoordinate cameraCoord)
	{
		_bufferBand = AxialGeometry.CoordsInRingsOfRadii(cameraCoord, _radius + 1, _radius + _bufferSize).ToHashSet();
	}
	
	void SyncPoolWithTarget()
	{
		_spawnList.Clear();
		_despawnNow.Clear();
		_targetCoords.Clear();
		_targetCoords.UnionWith(_needNow);
		_targetCoords.UnionWith(_bufferBand);


		foreach(var need in _targetCoords)
		{
			if (!_liveCoords.Contains(need))
			{
                _spawnList.Add(need);
            }
		}

		foreach(var live in _liveCoords)
		{
			if (!_targetCoords.Contains(live))
			{
				_despawnNow.Add(live);
			}
		}

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
		if (_liveCoords.Contains(c) || !HexGrid.TryGetHex(c, out HexData data)) return;

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
		
		CameraCorners[0] = ProjectViewportToPlane(cam, new Vector2(0f, 0f), planeZ);
		CameraCorners[2] = ProjectViewportToPlane(cam, new Vector2(1f, 0f), planeZ);
		CameraCorners[1] = ProjectViewportToPlane(cam, new Vector2(0f, 1f), planeZ);
		CameraCorners[3] = ProjectViewportToPlane(cam, new Vector2(1f, 1f), planeZ);
		
		(float q, float r) bl = HexGridGeometry.SceneToFractionalAxial(CameraCorners[0]);
		(float q, float r) br = HexGridGeometry.SceneToFractionalAxial(CameraCorners[1]);
		(float q, float r) tl = HexGridGeometry.SceneToFractionalAxial(CameraCorners[2]);
		(float q, float r) tr = HexGridGeometry.SceneToFractionalAxial(CameraCorners[3]);
		
		return Mathf.CeilToInt(Mathf.Max(DistanceBetweenFractionalAxialCoords(bl,center), DistanceBetweenFractionalAxialCoords(br,center), DistanceBetweenFractionalAxialCoords(tl,center), DistanceBetweenFractionalAxialCoords(tr,center)));
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

    public static Vector3 MouseToPlane(Camera cam, float PlaneZ)
    {
        var plane = new Plane(Vector3.forward, new Vector3(0f, 0f, PlaneZ));
		Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out float enter))
            return ray.GetPoint(enter);
        return cam.transform.position + ray.direction * 1000f;
    }
}