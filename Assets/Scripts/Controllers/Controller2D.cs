// Collider2d
// 
// Controls how 2D entity is moved and handles it's collisions with raycasts
// This is used over Rigidbodies so that we have more control over the collisions
// and physics of the game and are not contrained but Unity physics

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller2D : RaycastController
{

    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;

        public bool contactAbove, contactBelow;
        public bool contactLeft, contactRight;

        public int faceDirection;            // 1 for right, -1 for left
        public bool fallingThroughPlatform;
        public bool canWallSlideOnCollision;

        public bool climbingSlope;
        public bool descendingSlope;

        public float slopeAngle, slopeAngleOld;     // Angle of slope (angle accoring to the bottom corner raycast)

        public Vector2 originalTranslation;         // Unedited original translation received in Move

        public float horizontalHitAngle;
        public float verticalHitAngle;

        public void Reset()
        {
            above = below = false;
            left = right = false;

            contactAbove = contactBelow = false;
            contactLeft = contactRight = false;

            canWallSlideOnCollision = false;

            climbingSlope = false;
            descendingSlope = false;

            slopeAngleOld = slopeAngle;
            slopeAngle = 0;

            horizontalHitAngle = 0.0f;
            verticalHitAngle = 0.0f;
        }
    }

    public bool debugDraw;

    private float maxClimbAngle = 70.0f;
    private float maxDescendAngle = 70.0f;

    public CollisionInfo collisions;
    private Vector2 playerInput;

    protected override void Start()
    {
        base.Start();
    }

    public void AirMove(Vector2 translation, bool standingOnPlatform = false)
    {
        Move(translation, new Vector2(0.0f, -1.0f), standingOnPlatform);
    }

    public void Move(Vector2 translation, bool standingOnPlatform = false)
    {
        Move(translation, Vector2.zero, standingOnPlatform);
    }

    public void Move(Vector2 translation, Vector2 input, bool standingOnPlatform = false)
    {
        UpdateRaycastOrigins();
        collisions.Reset();
        collisions.originalTranslation = translation;
        playerInput = input;

        if (translation.x != 0)
        {
            collisions.faceDirection = (int)Mathf.Sign(translation.x);
        }

        if (translation.y < 0)
        {
            DescendSlope(ref translation);
        }

        HorizontalCollisions(ref translation);

        if (translation.y != 0.0f)
        {
            VerticalCollisions(ref translation);
        }

        transform.Translate(translation);

        if (standingOnPlatform)
        {
            collisions.below = true;
        }
    }

    void HorizontalCollisions(ref Vector2 translation)
    {
        float directionX = collisions.faceDirection;
        float rayLength = Mathf.Abs(translation.x) + raycastOriginsX.skinWidths.x;

        if (Mathf.Abs(translation.x) < PIXEL_WIDTH)
        {
            rayLength = raycastOriginsX.skinWidths.x + PIXEL_WIDTH;
        }

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1.0f) ? raycastOriginsX.bottomLeft : raycastOriginsX.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);   // x translation comes before y translation
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, groundCollisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            if (hit)
            {
                /*if (hit.distance == 0.0f)
                {
                    continue;
                }*/

                collisions.contactLeft = directionX == -1;
                collisions.contactRight = directionX == 1;

                if (hit.collider.tag == "Ground")
                {
                    collisions.canWallSlideOnCollision = true;
                }

                if (hit.collider.tag == "Through")
                {
                    continue;
                }

                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                collisions.horizontalHitAngle = slopeAngle;
                if (i == 0 && slopeAngle <= maxClimbAngle)
                {
                    // Override descending slope if you are climbing a slope on the other side of the collider
                    if (collisions.descendingSlope)
                    {
                        collisions.descendingSlope = false;
                        translation = collisions.originalTranslation;
                    }
                    float distanceToSlopeStart = 0;
                    // Starting a new slope
                    if (slopeAngle != collisions.slopeAngleOld)
                    {
                        distanceToSlopeStart = hit.distance - raycastOriginsX.skinWidths.x;
                        translation.x -= distanceToSlopeStart * directionX;         // Remove the movement it take to get to the slope
                    }
                    ClimbSlope(ref translation, slopeAngle);
                    translation.x += distanceToSlopeStart * directionX;             // Add back the movement to the slope
                }

                // Only need to check rest of the rays for collision if we are not on a slope
                if (!collisions.climbingSlope || slopeAngle > maxClimbAngle)
                {
                    translation.x = (hit.distance - raycastOriginsX.skinWidths.x) * directionX;
                    rayLength = hit.distance;      // update ray length to hit distance. Anything further away in the original rayLength is further than this collision

                    // Move y the correct amount on a slope if it gets blocked
                    if (collisions.climbingSlope)
                    {
                        // Use collisions.slopeAngle because slopeAngle is the angle detected by the current ray
                        translation.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(translation.x);
                    }

                    collisions.left = directionX == -1;
                    collisions.right = directionX == 1;
                }
            }
        }
    }

    void VerticalCollisions(ref Vector2 translation)
    {
        float directionY = Mathf.Sign(translation.y);
        float rayLength = Mathf.Abs(translation.y) + raycastOriginsY.skinWidths.y;

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1.0f) ? raycastOriginsY.bottomLeft : raycastOriginsY.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + translation.x);      // x translation comes before y translation
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, groundCollisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            if (hit)
            {

                collisions.contactBelow = directionY == -1;
                collisions.contactAbove = directionY == 1;

                if (hit.collider.tag == "Through")
                {
                    if (directionY == 1 || hit.distance == 0.0f)
                    {
                        continue;
                    }

                    if (collisions.fallingThroughPlatform)
                    {
                        continue;
                    }

                    if (playerInput.y <= -0.9f)
                    {
                        collisions.fallingThroughPlatform = true;
                        Invoke("ResetFallingThroughPlatform", 0.5f);
                        continue;
                    }
                }

                translation.y = (hit.distance - raycastOriginsY.skinWidths.y) * directionY;
                rayLength = hit.distance;      // update ray length to hit distance. Anything further away in the original rayLength is further than this collision

                // Move x the correct amount on a slope if it gets blocked
                if (collisions.climbingSlope)
                {
                    // Use collisions.slopeAngle because slopeAngle is the angle detected by the current ray
                    translation.x = translation.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(translation.x);
                }

                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
            }
        }

        // After moving vertically we want to recheck if the slope has changed and adjust the x translation accordingly
        if (collisions.climbingSlope)
        {
            float directionX = Mathf.Sign(translation.x);
            rayLength = Mathf.Abs(translation.x) + raycastOriginsX.skinWidths.x;
            Vector2 rayOrigin = ((directionX == -1) ? raycastOriginsX.bottomLeft : raycastOriginsX.bottomRight) + Vector2.up * translation.y;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, groundCollisionMask);

            if (hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (slopeAngle != collisions.slopeAngle)
                {
                    translation.x = (hit.distance - raycastOriginsX.skinWidths.x) * directionX;
                    collisions.slopeAngle = slopeAngle;
                }
            }
        }
    }

    void ClimbSlope(ref Vector2 translation, float slopeAngle)
    {
        float moveDistance = Mathf.Abs(translation.x);
        float climbTranslationY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
        if (translation.y <= climbTranslationY)
        {
            translation.y = climbTranslationY;
            translation.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(translation.x);
            collisions.below = true;
            collisions.climbingSlope = true;
            collisions.slopeAngle = slopeAngle;
        }
    }

    // Cast rays downward to detect slope
    void DescendSlope(ref Vector2 translation)
    {
        float directionX = Mathf.Sign(translation.x);
        Vector2 rayOrigin = (directionX == -1) ? raycastOriginsY.bottomRight : raycastOriginsY.bottomLeft;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, groundCollisionMask);

        if (hit)
        {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            collisions.verticalHitAngle = slopeAngle;
            if (slopeAngle != 0 && slopeAngle <= maxDescendAngle)
            {
                // check if slope and motion are in the same direction
                if (Mathf.Sign(hit.normal.x) == directionX)
                {
                    // Check is slope is close enough
                    float verticalComponent = Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(translation.x);
                    if ((hit.distance - raycastOriginsY.skinWidths.y) <= verticalComponent)
                    {
                        float moveDistance = Mathf.Abs(translation.x);
                        float descendTranslationY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                        translation.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(translation.x);
                        translation.y -= descendTranslationY;

                        collisions.slopeAngle = slopeAngle;
                        collisions.descendingSlope = true;
                        collisions.below = true;
                    }
                }
            }
        }
    }

    private void CollisionDebug()
    {
        debugInfo.ResetDebug();
        debugInfo.AddDebugLine("Above: " + collisions.above.ToString());
        debugInfo.AddDebugLine("Below: " + collisions.below.ToString());
        debugInfo.AddDebugLine("Left: " + collisions.left.ToString());
        debugInfo.AddDebugLine("Right: " + collisions.right.ToString());
        debugInfo.AddDebugLine("Climbing Slope: " + collisions.climbingSlope.ToString());
        debugInfo.AddDebugLine("Descending Slope: " + collisions.descendingSlope.ToString());
        debugInfo.AddDebugLine("Slope Angle: " + collisions.slopeAngle.ToString());

        debugInfo.CreateTextBox(0, 0);
    }

    void ResetFallingThroughPlatform()
    {
        collisions.fallingThroughPlatform = false;
    }

    private void OnGUI()
    {
        if (debugDraw)
        {
            CollisionDebug();
        }
    }
}
