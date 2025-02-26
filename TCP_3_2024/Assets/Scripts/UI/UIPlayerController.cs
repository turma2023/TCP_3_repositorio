using UnityEngine;
using UnityEngine.UI;

public class UIPlayerController : MonoBehaviour
{
    [Header("UI Skills References")]
    [SerializeField] private GameObject skill1;
    [SerializeField] private GameObject skill2;
    [SerializeField] private GameObject ultimate;


    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void Initialize(UISkillData UISkillData)
    {
        skill1.GetComponent<Image>().sprite = UISkillData.Skill1Icon;
        skill2.GetComponent<Image>().sprite = UISkillData.Skill2Icon;
        ultimate.GetComponent<Image>().sprite = UISkillData.UltimateIcon;
    }

    public Image GetSkill1Icon()
    {
        return skill1.GetComponent<Image>();
    }

    public Image GetSkill2Icon()
    {
        return skill2.GetComponent<Image>();
    }

    public Image GetUltimateIcon()
    {
        return ultimate.GetComponent<Image>();
    }
}
