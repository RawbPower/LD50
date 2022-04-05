using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : RaycastController
{

    struct PassengerMovement
    {
        public Transform transform;
        public Vector2 velocity;
        public bool standingOnPlatform;
        public bool moveBeforePlatform;

        public PassengerMovement(Transform _transform, Vector2 _velocity, bool _standingOnPlatform, bool _moveBeforePlatform)
        {
            transform = _transform;
            velocity = _velocity;
            standingOnPlatform = _standingOnPlatform;
            moveBeforePlatform = _moveBeforePlatform;
        }
    }

    public enum PlatformState { Default, WaitForPlayer, Inactive };

    public LayerMask passengerMask;

    public Vector2[] localWaypoints;
    private Vector2[] globalWaypoints;

    public float speed;
    public bool cyclic;
    public float waitTime;
    [Range(0,2)]
    public float easeAmount;

    // This will only work for linear waypoints
    public bool shouldWaitForPlayer = false;

    public bool startsActive = false;

    private bool isWaiting;
    private int fromWaypointIndex;
    private float ratioBetweenWaypoints;
    private float nextMoveTime;
    private GameObject player;
    private PlatformState platformState;

    List<PassengerMovement> passengerMovement;
    Dictionary<Transform, Controller2D> passengerDictionary = new Dictionary<Transform, Controller2D>();        // Dictionary sop we dont have to keep calling GetComponent

    protected override void Start()
    {
        base.Start();

        isWaiting = true;
        platformState = startsActive ? PlatformState.Default : PlatformState.Inactive;
        if (shouldWaitForPlayer)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        globalWaypoints = new Vector2[localWaypoints.Length];
        for (int i = 0; i < localWaypoints.Length; i++)
        {
            globalWaypoints[i] = localWaypoints[i] + new Vector2(transform.position.x, transform.position.y);
        }
    }

    private void Update()
    {
        UpdateRaycastOrigins();
        Vector2 translation = CalculatePlatformMovement();

        CalculatePassengerMovement(translation);

        MovePassengers(true);
        transform.Translate(translation);
        MovePassengers(false);
    }

    private float Ease(float x)
    {
        float a = 1 + easeAmount;
        return Mathf.Pow(x, a) / (Mathf.Pow(x, a) + Mathf.Pow(1 - x, a));
    }

    private Vector2 CalculatePlatformMovement()
    {
        if (shouldWaitForPlayer || platformState == PlatformState.Inactive)
        {
            // Check for a player on the platform using raycasts
            bool playerOnPlatform = false;
            float rayLength = raycastOriginsY.skinWidths.y;

            for (int i = 0; i < verticalRayCount; i++)
            {
                Vector2 rayOrigin = raycastOriginsY.topLeft;
                rayOrigin += Vector2.right * (verticalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, passengerMask);

                if (hit && hit.distance != 0.0f)
                {
                    if (hit.collider.gameObject.CompareTag("Player"))
                    {
                        playerOnPlatform = true;
                        break;
                    }
                }
            }

            PlatformState offPlatformState = platformState == PlatformState.Inactive ? PlatformState.Inactive : PlatformState.WaitForPlayer;

            platformState = playerOnPlatform ? PlatformState.Default : offPlatformState;
        }

        if (platformState == PlatformState.Inactive)
        {
            return Vector2.zero;
        }
        else if (platformState == PlatformState.WaitForPlayer && isWaiting)
        {
            int closestWaypointIndex = FindClosestWaypoint(player.transform.position);
            if (closestWaypointIndex != fromWaypointIndex)
            {
                isWaiting = false;
            }
            return Vector2.zero;
        }
        else
        {
            isWaiting = false;
            if (Time.time < nextMoveTime)
            {
                return Vector2.zero;
            }

            fromWaypointIndex %= globalWaypoints.Length;
            int toWaypointIndex = (fromWaypointIndex + 1) % globalWaypoints.Length;
            float distanceBetweenWaypoints = Vector2.Distance(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex]);
            ratioBetweenWaypoints += Time.deltaTime * speed / distanceBetweenWaypoints;
            ratioBetweenWaypoints = Mathf.Clamp01(ratioBetweenWaypoints);
            float easedRatioBetweenWaypoints = Ease(ratioBetweenWaypoints);

            Vector2 newPos = Vector2.Lerp(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex], easedRatioBetweenWaypoints);

            // When you reach next waypoint
            if (ratioBetweenWaypoints >= 1.0f)
            {
                OnArriveAtWaypoint();
            }

            return newPos - new Vector2(transform.position.x, transform.position.y);
        }
    }

    void MovePassengers(bool beforeMovePlatform)
    { 
        foreach (PassengerMovement passenger in passengerMovement)
        {
            if (!passengerDictionary.ContainsKey(passenger.transform))
            {
                passengerDictionary.Add(passenger.transform, passenger.transform.GetComponent<Controller2D>());
            }

            if (passenger.moveBeforePlatform == beforeMovePlatform)
            {
                passengerDictionary[passenger.transform].Move(passenger.velocity, passenger.standingOnPlatform);
            }
        }
    }


    void CalculatePassengerMovement(Vector2 translation)
    {
        HashSet<Transform> movePassengers = new HashSet<Transform>();
        passengerMovement = new List<PassengerMovement>();

        float directionX = Mathf.Sign(translation.x);
        float directionY = Mathf.Sign(translation.y);

        // Vertically moving platform
        if (translation.y != 0.0f)
        {
            float rayLength = Mathf.Abs(translation.y) + raycastOriginsY.skinWidths.y;

            for (int i = 0; i < verticalRayCount; i++)
            {
                Vector2 rayOrigin = (directionY == -1.0f) ? raycastOriginsY.bottomLeft : raycastOriginsY.topLeft;
                rayOrigin += Vector2.right * (verticalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, passengerMask);

                Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

                if (hit && hit.distance != 0.0f)
                {
                    if (!movePassengers.Contains(hit.transform))
                    {
                        movePassengers.Add(hit.transform);
                        float pushX = (directionY == 1) ? translation.x : 0.0f;
                        float pushY = translation.y - (hit.distance - raycastOriginsY.skinWidths.y) * directionY;

                        passengerMovement.Add(new PassengerMovement(hit.transform, new Vector2(pushX, pushY), directionY == 1, true));
                    }
                }
            }
        }

        // Horizontally moving platform
        if (translation.x != 0.0f)
        {
            float rayLength = Mathf.Abs(translation.x) + raycastOriginsX.skinWidths.x;

            for (int i = 0; i < horizontalRayCount; i++)
            {
                Vector2 rayOrigin = (directionX == -1.0f) ? raycastOriginsX.bottomLeft : raycastOriginsX.bottomRight;
                rayOrigin += Vector2.up * (horizontalRaySpacing * i);   // x translation comes before y translation
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, passengerMask);

                Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

                if (hit && hit.distance != 0.0f)
                {
                    if (!movePassengers.Contains(hit.transform))
                    {
                        movePassengers.Add(hit.transform);
                        float pushX = translation.x - (hit.distance - raycastOriginsX.skinWidths.x) * directionX;
                        float pushY = -PIXEL_WIDTH;

                        passengerMovement.Add(new PassengerMovement(hit.transform, new Vector2(pushX, pushY), false, true));
                    }
                }
            }
        }

        // Player on top of horizontally or downward moving platform
        if (directionY == -1 || (translation.y == 0.0f && translation.x != 0.0f))
        {
            float rayLength = raycastOriginsY.skinWidths.y + PIXEL_WIDTH;

            for (int i = 0; i < verticalRayCount; i++)
            {
                Vector2 rayOrigin = raycastOriginsY.topLeft + Vector2.right * (verticalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, passengerMask);

                Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

                if (hit && hit.distance != 0.0f)
                {
                    if (!movePassengers.Contains(hit.transform))
                    {
                        movePassengers.Add(hit.transform);
                        float pushX = translation.x;
                        float pushY = translation.y;

                        passengerMovement.Add(new PassengerMovement(hit.transform, new Vector2(pushX, pushY), true, false));
                    }
                }
            }
        }
    }

    // Store inset corners of collider
    protected override void UpdateRaycastOrigins()
    {
        Bounds bounds = col.bounds;
        raycastOriginsX.skinWidths = new Vector2(bounds.size.x * 0.1f, PIXEL_WIDTH);
        bounds.Expand(-raycastOriginsX.skinWidths * 2.0f);

        raycastOriginsX.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOriginsX.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOriginsX.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOriginsX.topRight = new Vector2(bounds.max.x, bounds.max.y);

        bounds = col.bounds;
        raycastOriginsY.skinWidths = new Vector2(PIXEL_WIDTH, bounds.size.y * 0.1f);
        bounds.Expand(-raycastOriginsY.skinWidths * 2.0f);

        raycastOriginsY.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOriginsY.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOriginsY.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOriginsY.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    private void OnDrawGizmos()
    {
        if (localWaypoints != null)
        {
            Gizmos.color = Color.green;
            float size = 0.3f;

            for (int i = 0; i < localWaypoints.Length; i++)
            {
                Vector2 globalWaypointPos = (Application.isPlaying) ? globalWaypoints[i] : localWaypoints[i] + new Vector2(transform.position.x, transform.position.y);
                Gizmos.DrawLine(globalWaypointPos - Vector2.up * size, globalWaypointPos + Vector2.up * size);
                Gizmos.DrawLine(globalWaypointPos - Vector2.left * size, globalWaypointPos + Vector2.left * size);
            }
        }
    }

    void OnArriveAtWaypoint()
    {
        Debug.Log("Arrived at waypoint");

        ratioBetweenWaypoints = 0.0f;

        fromWaypointIndex++;

        if (!cyclic)
        {
            if (fromWaypointIndex >= globalWaypoints.Length - 1)
            {
                fromWaypointIndex = 0;
                System.Array.Reverse(globalWaypoints);
            }
        }

        if (platformState == PlatformState.WaitForPlayer)
        {
            int closestWaypointIndex = FindClosestWaypoint(player.transform.position);
            if (closestWaypointIndex == fromWaypointIndex)
            {
                isWaiting = true;
            }
        }

        nextMoveTime = Time.time + waitTime;
    }

    int FindClosestWaypoint(Vector2 position)
    {
        int closestWaypointIndex = -1;
        float closestDistance = Mathf.Infinity;
        for (int i = 0; i < globalWaypoints.Length; i++)
        {
            float distance = Vector2.Distance(position, globalWaypoints[i]);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestWaypointIndex = i;
            }
        }

        return closestWaypointIndex;
    }
}
