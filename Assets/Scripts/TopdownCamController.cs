using UnityEngine;

[RequireComponent(typeof(Camera))]
public class TopdownCamController : MonoBehaviour
{
    public float planeZ = 0f;               // Your grid plane
    public float panSpeedBase = 8f;         // units/sec at d=1
    public float panSpeedScale = 0.8f;      // multiply by distance^scale
    public float zoomStep = 0.1f;           // wheel step (10% per notch)
    public float minDistance = 5f;          // clamp dolly distance
    public float maxDistance = 200f;

    Camera cam;

    void Awake() => cam = GetComponent<Camera>();

    void Update()
    {
        // 1) PAN with WASD (in-plane)
        Vector2 input = new Vector2(
            (Input.GetKey(KeyCode.D) ? 1 : 0) - (Input.GetKey(KeyCode.A) ? 1 : 0),
            (Input.GetKey(KeyCode.W) ? 1 : 0) - (Input.GetKey(KeyCode.S) ? 1 : 0)
        );

        if (input.sqrMagnitude > 0f)
        {
            // Distance to plane controls world-units-per-second for constant screen-space speed
            float d = DistanceToPlaneZ(transform.position, planeZ, transform.forward);
            float speed = panSpeedBase * Mathf.Pow(Mathf.Max(1f, d), panSpeedScale);

            // Move in XY; if you add yaw later, project cam.right/up onto plane instead
            Vector3 delta = new Vector3(input.x, input.y, 0f) * (speed * Time.deltaTime);
            transform.position += delta;
        }

        // 2) ZOOM with mouse wheel (dolly along forward) — zoom to cursor
        float scroll = Input.mouseScrollDelta.y;
        if (Mathf.Abs(scroll) > 0.0001f)
        {
            ZoomTowardCursor(scroll);
        }
    }

    void ZoomTowardCursor(float scrollDelta)
    {
        // Current distance to plane along forward
        float d0 = DistanceToPlaneZ(transform.position, planeZ, transform.forward);

        // Exponential step feels better than linear
        float factor = Mathf.Pow(1f - zoomStep, scrollDelta); // scroll>0 -> closer
        float d1 = Mathf.Clamp(d0 * factor, minDistance, maxDistance);

        // Ray from cursor to plane gives a focus point to zoom toward
        Vector3 focus = RayToPlaneZ(cam, Input.mousePosition, planeZ);

        // Move camera along ray so that new distance = d1 and keep focus under cursor
        // Direction from camera toward focus projected on forward axis:
        Vector3 fwd = transform.forward; // should be (0,0,1) in your setup
        transform.position = focus - fwd * d1;
    }

    static float DistanceToPlaneZ(Vector3 pos, float planeZ, Vector3 forward)
    {
        // For pure top-down forward=(0,0,1), this is just |planeZ - pos.z|
        // This version also works if you later tilt slightly:
        // signed distance along forward from cam to plane z=planeZ
        float denom = Mathf.Abs(Vector3.Dot(forward, Vector3.forward));
        if (denom < 1e-6f) return Mathf.Abs(planeZ - pos.z); // fallback
        // Project vector from cam to any point on plane onto forward
        Vector3 toPlane = new Vector3(pos.x, pos.y, planeZ) - pos;
        return Mathf.Abs(Vector3.Dot(toPlane, forward));
    }

    static Vector3 RayToPlaneZ(Camera cam, Vector3 screenPos, float planeZ)
    {
        Ray r = cam.ScreenPointToRay(screenPos);
        // Intersect with plane normal +Z at z=planeZ
        float t = (planeZ - r.origin.z) / r.direction.z;
        return r.origin + r.direction * t;
    }
}