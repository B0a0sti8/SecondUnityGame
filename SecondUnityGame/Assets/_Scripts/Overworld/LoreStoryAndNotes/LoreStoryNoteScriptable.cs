using UnityEngine;


[CreateAssetMenu(fileName = "New Lore Note", menuName = "Scriptable Objects/Lore Story and Notes")]
public class LoreStoryNoteScriptable : ScriptableObject
{
    public string loreNoteTitle;
    public string loreNoteContent;
    public string loreEpos;
    public int eposOrderNumber;

    public LoreStoryNoteScriptable Clone()
    {
        return (LoreStoryNoteScriptable)this.MemberwiseClone();
    }
}
