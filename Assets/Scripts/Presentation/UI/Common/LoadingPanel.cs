using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadingPanel : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Image _loadingBar;
    [SerializeField] TextMeshProUGUI _loadingText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateStatus(float amountDone, string loadingText)
    {
        _loadingBar.fillAmount = amountDone;
        _loadingText.text = loadingText;
    }
}
