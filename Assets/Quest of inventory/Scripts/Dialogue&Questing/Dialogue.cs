using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Dialogue : SerializedMonoBehaviour
{
    public string faction;
    public string greeting;
    public LineOfDialogue goodbye;
    public List<LineOfDialogue> dialogueOptions = new List<LineOfDialogue>();
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
