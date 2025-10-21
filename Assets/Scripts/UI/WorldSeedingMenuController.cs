using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSeedingMenuController : MonoBehaviour
{
    [SerializeField] MenuView _worldSeedingMenuView;
    [SerializeField] UnityEngine.UI.RawImage _previewImage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SeedWorld()
    {
        WorldSeedingController.SeedWorld(WorldManager.Instance.HexGrid, WorldManager.Instance.MapDefinition);
    }

    public void EnableMenu()
    {
        _worldSeedingMenuView.gameObject.SetActive(true);
    }

    public void DisableMenu()
    {
        _worldSeedingMenuView.gameObject.SetActive(false);
    }
}
