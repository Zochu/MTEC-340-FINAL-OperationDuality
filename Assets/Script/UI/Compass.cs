using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] GameObject iconPrefab;
    [SerializeField] RawImage compassParent;
    List<CompassIcon> icons = new List<CompassIcon>();
    [SerializeField]List<GameObject> iconObjects = new List<GameObject>();

    [Space(10)]
    [SerializeField] float maxDistance = 300f;
    float compassUnit;


    private void Start()
    {
        compassUnit = compassParent.rectTransform.rect.width / 360f;
        maxDistance = 300;
    }

    private void Update()
    {
        foreach (CompassIcon icon in icons)
        {
            icon.image.rectTransform.anchoredPosition = GetPosion(icon);
            float distance = Vector2.Distance(new Vector2(player.transform.position.x, player.transform.position.z), icon.position);
            float scale = 0f;

            if (distance < maxDistance) scale = 1f - (distance / maxDistance);

            icon.image.rectTransform.localScale = Vector3.one * scale;
        }
    }

    public void AddIcon(CompassIcon icon)
    {
        GameObject newIcon = Instantiate(iconPrefab, compassParent.transform);
        icon.image = newIcon.GetComponent<Image>();
        icon.image.sprite = icon.icon;

        icons.Add(icon);
        iconObjects.Add(newIcon);
    }

    public void RemoveIcon(CompassIcon icon)
    {
        int index = icons.IndexOf(icon);
        if (index >= 0)
        {
            Destroy(iconObjects[index]);
            iconObjects.RemoveAt(index);
            icons.RemoveAt(index);
        }
    }

    Vector2 GetPosion (CompassIcon icon)
    {
        Vector2 playerPosition = new Vector2(player.transform.position.x, player.transform.position.z);
        Vector2 playerForward = new Vector2(player.transform.forward.x, player.transform.forward.z);

        float angle = Vector2.SignedAngle(icon.position - playerPosition, playerForward);
        return new Vector2(compassUnit * angle, 0f);

    }


}
