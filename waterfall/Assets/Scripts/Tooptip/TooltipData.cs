using UnityEngine;

[CreateAssetMenu(fileName = "TooltipData", menuName = "Scriptable Object/Tooltip", order = int.MaxValue)]
public class TooltipData : ScriptableObject
{
   public string Name;
   public Sprite Icon;
   public string explainingText;
   public string description;
}
