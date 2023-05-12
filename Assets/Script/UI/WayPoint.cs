using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WayPoint : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI meter;
    [SerializeField] Vector3 offset;
    [SerializeField] Transform pistol;
    [SerializeField] Transform key;
    [SerializeField] Transform projector;
    [SerializeField] Transform escapePoint;
    
    Transform target;

    float minX, maxX;
    float minY, maxY;

    Vector2 position;

    private void Start()
    {
        target = pistol != null ? pistol : key;

        minX = image.GetPixelAdjustedRect().width / 2;
        maxX = Screen.width - minX;

        minY = image.GetPixelAdjustedRect().height / 2;
        maxY = Screen.width - minY;
    }


    private void LateUpdate()
    {
        position = Camera.main.WorldToScreenPoint(target.position + offset);

        if (Vector3.Dot((target.position - transform.position), transform.forward) < 0)
        {
            position.x = position.x < Screen.width / 2 ? position.x = maxX : position.x = minX;
        }

        position.x = Mathf.Clamp(position.x, minX, maxX);
        position.y = Mathf.Clamp(position.y, minY, maxY);

        image.transform.position = position;
        meter.text = ((int)Vector3.Distance(target.position, transform.position)).ToString() + "m";
    }

    public void SetToKey()
    {
        target = key;
    }

    public void SetToProjector()
    {
        target = projector;
    }

    public void SetToEscape()
    {
        target = escapePoint;
    }

}
