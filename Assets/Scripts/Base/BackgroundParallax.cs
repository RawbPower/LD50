using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    public Camera cam;
    public Transform subject;

    Vector2 startPosition;
    public float backgroundZ;

    float twoAspect => cam.aspect * 2;
    float tileWidth => (twoAspect > 3 ? twoAspect : 3);
    float viewWidth => loopSpriteRenderer.sprite.rect.width / loopSpriteRenderer.sprite.pixelsPerUnit;
    Vector2 travel => (Vector2)cam.transform.position - startPosition; //2D distance travelled from our starting position
    float distanceFromSubject => backgroundZ - subject.position.z;
    float clippingPlane => (cam.transform.position.z + (distanceFromSubject > 0 ? cam.farClipPlane : cam.nearClipPlane));
    float parallaxFactor => Mathf.Abs(distanceFromSubject) / clippingPlane;

    public SpriteRenderer loopSpriteRenderer;

    void Start()
    {
        startPosition = transform.position + new Vector3(20.0f, 0.0f);

        if (loopSpriteRenderer != null)
        {
            float spriteSizeX = loopSpriteRenderer.sprite.rect.width / loopSpriteRenderer.sprite.pixelsPerUnit;
            float spriteSizeY = loopSpriteRenderer.sprite.rect.height / loopSpriteRenderer.sprite.pixelsPerUnit;

            loopSpriteRenderer.drawMode = SpriteDrawMode.Tiled;
            loopSpriteRenderer.size = new Vector2(spriteSizeX * tileWidth, spriteSizeY);
            transform.localScale = Vector3.one;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float newPosX = startPosition.x + travel.x * parallaxFactor;
        float newPosY = startPosition.y + travel.y;
        transform.position = new Vector3(newPosX, newPosY, 0.0f);

        Vector2 totalTravel = cam.transform.position - transform.position;
        float boundsOffset = (viewWidth / 2) * (totalTravel.x > 0 ? 1 : -1);
        float screens = (int)((totalTravel.x + boundsOffset) / viewWidth);
        transform.position += new Vector3(screens * viewWidth, 0);
    }
}
