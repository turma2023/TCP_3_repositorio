using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class Skill : NetworkBehaviour
{
    public bool HasUsed { get; private set; }
    public Image SkillIcon { get; private set; }

    public void EnableUse()
    {
        HasUsed = false;
        SkillIcon.color = Color.white;
        SkillIcon.GetComponentInChildren<TextMeshProUGUI>(true).gameObject.SetActive(true);
    }

    public void DisableUse()
    {
        HasUsed = true;
        SkillIcon.color = Color.gray;
        SkillIcon.GetComponentInChildren<TextMeshProUGUI>().gameObject.SetActive(false);
    }

    public void SetImage(Image image)
    {
        SkillIcon = image;
    }

}
