using System;
using UnityEngine;
using Sirenix.OdinInspector;

[Serializable]
public class LineOfDialogue
{
    [TextArea(4, 6), FoldoutGroup("Dialogue")]
    public string question, response;
    public float minAproval = -1f;

    public Dialogue nextDialogue;
}
