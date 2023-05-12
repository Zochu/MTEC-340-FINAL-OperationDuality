using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompassIcon : MonoBehaviour
{
    public Sprite icon;
    public Image image;
    public Vector2 position
    {
        get => new Vector2(transform.position.x, transform.position.z);
    }

    [SerializeField] Compass compass;

    private void Start()
    {
        compass.AddIcon(this);
    }

    private void OnDestroy()
    {
        Remove();
    }

    public void Remove()
    {
        compass.RemoveIcon(this);
    }

    public void Add()
    {
        compass.AddIcon(this);
    }

}
