using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] MenuView _mainMenuView;
    [SerializeField] WorldGenerationMenuController _worldGenerationMenuController;
    [SerializeField] WorldSeedingMenuController _worldSeedingMenuController;

    public static MainMenuController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        EnableMenu();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartNewGame()
    {
        DisableMenu();
        _worldGenerationMenuController.EnableMenu();
    }

    public void EnableMenu()
    {
        _mainMenuView.gameObject.SetActive(true);
    }

    public void DisableMenu()
    {
        _mainMenuView.gameObject.SetActive(false);
    }

    public void GoToSeedMenu()
    {
        _worldGenerationMenuController.DisableMenu();
        _worldSeedingMenuController.EnableMenu();
    }
}
