using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Skills/UI Skill Data")]
public class UISkillData : ScriptableObject
{
    [field: SerializeField] public Sprite Skill1Icon { get; private set; }
    [field: SerializeField] public Sprite Skill2Icon { get; private set; }
    [field: SerializeField] public Sprite UltimateIcon { get; private set; }
}
