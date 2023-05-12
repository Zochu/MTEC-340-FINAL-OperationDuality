using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    public IEnumerator Shake(float duration, float strength)
    {
        Vector3 originalPosition = transform.position;
        Vector3 playerPosition = playerTransform.position;

        float elapsed = 0;

        while(elapsed < duration)
        {
            float x = Random.Range(-1, 1) * strength;
            float y = Random.Range(-1, 1) * strength;

            transform.position = originalPosition + new Vector3(x, y, 0);
            transform.position += playerTransform.position - playerPosition;
            playerPosition = playerTransform.position; 

            elapsed += Time.deltaTime;

            yield return new WaitForFixedUpdate();
        }

        transform.localPosition = Vector3.zero;
    }
}
