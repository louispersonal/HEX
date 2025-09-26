using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexView : MonoBehaviour
{
	private BaseHex _data;

	public void Initialize(BaseHex data)
	{
		_data = data;
		gameObject.transform.position = BaseHexGrid.Instance.AxialToSceneConversion(data.Coord);
	}

	public void Terminate()
	{
		
	}
}