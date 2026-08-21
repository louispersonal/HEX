using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private RawImage _miniMapImage;
    [SerializeField] private Image _windowImage;

    private bool _initialized;
    private MapTexture _mapTexture;
    private RectTransform _miniMapRect;
    private RectTransform _windowRect;

    public void SetTexture(MapTexture texture)
    {
        _mapTexture = texture;
        _miniMapImage.texture = texture.Texture;

        _miniMapRect = _miniMapImage.rectTransform;
        _windowRect = _windowImage.rectTransform;

        _initialized = true;
    }

    private void Update()
    {
        if (!_initialized) return;

        _windowRect.position = GetWindowWorldPosition();
        _windowRect.sizeDelta = GetWindowScale();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_miniMapRect,  eventData.position, 
            eventData.pressEventCamera, out Vector2 localPoint))
        {
            Vector2 normalizedPoint = new Vector2(
                Mathf.InverseLerp(_miniMapRect.rect.xMin, _miniMapRect.rect.xMax, localPoint.x),
                Mathf.InverseLerp(_miniMapRect.rect.yMin, _miniMapRect.rect.yMax, localPoint.y) );

            ForceCameraJump(normalizedPoint);
        }
    }

    private void ForceCameraJump(Vector2 normalizedPosition)
    {
        HexGridView view = GameSceneController.Instance.HexGridView;
        float worldSpan = view.WorldCorners[3].x - view.WorldCorners[0].x;
        float worldHeight = view.WorldCorners[1].y - view.WorldCorners[0].y;

        Vector2 newCameraPosition = new Vector2(view.WorldCorners[0].x + worldSpan * normalizedPosition.x,
            view.WorldCorners[0].y + worldHeight * normalizedPosition.y);
        
        GameSceneController.Instance.GameCamera.SnapToPosition(newCameraPosition);
    }
    
    private Vector3 GetWindowWorldPosition()
    {
        Vector2 mapPixelPosition = AxialGeometry.AxialToCartesian(
            GetCameraAxialPosition(),
            _mapTexture.HexSizePixels);

        float uiScaleX = _miniMapRect.rect.width / _mapTexture.Texture.width;
        float uiScaleY = _miniMapRect.rect.height / _mapTexture.Texture.height;

        Vector2 localPosition = new Vector2(
            mapPixelPosition.x * uiScaleX,
            mapPixelPosition.y * uiScaleY);

        Vector3[] corners = new Vector3[4];
        _miniMapRect.GetWorldCorners(corners);

        return corners[1] + (Vector3)localPosition;
    }

    private Vector2 GetWindowScale()
    {
        HexGridView view = GameSceneController.Instance.HexGridView;
        
        float worldSpan = view.WorldCorners[3].x - view.WorldCorners[0].x;
        float worldHeight = view.WorldCorners[1].y - view.WorldCorners[0].y;
        
        float cameraSpan = view.CameraCorners[3].x - view.CameraCorners[0].x;
        float cameraHeight = view.CameraCorners[1].y - view.CameraCorners[0].y;
        
        return new Vector2((cameraSpan / worldSpan) * _miniMapRect.sizeDelta.x, (cameraHeight / worldHeight) * _miniMapRect.sizeDelta.y);
    }
    
    private AxialCoordinate GetCameraAxialPosition()
    {
        (float q, float r) cameraFractionalPosition =
            GameSceneController.Instance.HexGridView.CameraCenter;

        return new AxialCoordinate(
            Mathf.RoundToInt(cameraFractionalPosition.q),
            Mathf.RoundToInt(cameraFractionalPosition.r));
    }
}