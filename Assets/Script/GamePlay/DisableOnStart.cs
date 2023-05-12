
using UnityEngine;

public class DisableOnStart : MonoBehaviour
{
    public string characterScript;

    [SerializeField] CharacterShadow c_shadow;
    [SerializeField] CharacterLight c_light;
    [SerializeField] GameObject shadowHUD;
    [SerializeField] GameObject lightHUD;

    private void Awake()
    {
        string character = PlayerPrefs.GetString("PlayerCharacter");


        if (character == "Light")
        {
            c_light.enabled = true;
            lightHUD.SetActive(true);
            c_shadow.enabled = false;
            shadowHUD.SetActive(false);
        }
        else if (character == "Shadow")
        {
            c_light.enabled = false;
            lightHUD.SetActive(false);
            c_shadow.enabled = true;
            shadowHUD.SetActive(true);
        }
    }
}
