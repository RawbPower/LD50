using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class RaycastController : MonoBehaviour
{
    public const float PIXEL_WIDTH = 1.0f / 16.0f;
    // Stores the inset bound corners
    protected struct RaycastOriginsX
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
        public Vector2 skinWidths;
    }

    protected struct RaycastOriginsY
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
        public Vector2 skinWidths;
    }

    public LayerMask groundCollisionMask;

    // Number of raycasts in each dimension
    public int horizontalRayCount = 6;
    public int verticalRayCount = 4;
    // Spacing between rays
    protected float horizontalRaySpacing;
    protected float verticalRaySpacing;

    protected BoxCollider2D col;
    protected RaycastOriginsX raycastOriginsX;
    protected RaycastOriginsY raycastOriginsY;

    protected DebugInfo debugInfo;

    protected virtual void Awake()
    {
        col = GetComponent<BoxCollider2D>();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        CalculateRaySpacing();
        debugInfo = new DebugInfo();
    }

    // Store inset corners of collider
    protected virtual void UpdateRaycastOrigins()
    {
        Bounds bounds = col.bounds;
        raycastOriginsX.skinWidths = new Vector2(bounds.size.x * 0.4f, PIXEL_WIDTH);
        bounds.Expand(-raycastOriginsX.skinWidths * 2.0f);

        raycastOriginsX.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOriginsX.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOriginsX.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOriginsX.topRight = new Vector2(bounds.max.x, bounds.max.y);

        bounds = col.bounds;
        raycastOriginsY.skinWidths = new Vector2(PIXEL_WIDTH, bounds.size.y * 0.4f);
        bounds.Expand(-raycastOriginsY.skinWidths * 2.0f);

        raycastOriginsY.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOriginsY.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOriginsY.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOriginsY.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    // Calculate ray spacing from the number of rays
    protected void CalculateRaySpacing()
    {
        Bounds bounds = col.bounds;
        raycastOriginsX.skinWidths = new Vector2(bounds.size.x * 0.4f, PIXEL_WIDTH);
        bounds.Expand(-raycastOriginsX.skinWidths * 2.0f);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);

        bounds = col.bounds;
        raycastOriginsY.skinWidths = new Vector2(PIXEL_WIDTH, bounds.size.y * 0.4f);
        bounds.Expand(-raycastOriginsY.skinWidths * 2.0f);


        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }
}
