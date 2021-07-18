using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// todo: add modular questing
public class DialogueManager : SerializedMonoBehaviour
{
    public static DialogueManager dManager;
    private Dialogue loadedDialogue;

    [SerializeField] TextMeshProUGUI responseText;
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform content;

    private void Awake()
    {
        if (dManager == null) dManager = this;
        else Destroy(this);
    }

    public void LoadDialogue(Dialogue _dialogue)
    {
        transform.GetChild(0).gameObject.SetActive(true);
            loadedDialogue = _dialogue;
                ClearButtons();

        var _i = 0;
        Button _spawnedButton;
        foreach (var _data in _dialogue.dialogueOptions)
        {
            var _currentApproval = FactionManager.Instance.FactionsApproval(loadedDialogue.faction);
            if (_currentApproval != null && _currentApproval > _data.minAproval)
            {
                var _x = _i;
                _spawnedButton = Instantiate(prefab, content).GetComponent<Button>();
                _spawnedButton.GetComponentInChildren<TextMeshProUGUI>().text = _data.question;
                _spawnedButton.onClick.AddListener(() => ButtonPressed(_x));
                _i++;
            }
        }

        _spawnedButton = Instantiate(prefab, content).GetComponent<Button>();
        _spawnedButton.GetComponentInChildren<TextMeshProUGUI>().text = _dialogue.goodbye.question;
        _spawnedButton.onClick.AddListener(EndConversation);

        print(_dialogue.greeting);
        DisplayResponse(loadedDialogue.greeting);
    }

    public void EndConversation()
    {
        ClearButtons();
        DisplayResponse(loadedDialogue.goodbye.response);
        
        if(loadedDialogue.goodbye.nextDialogue != null) LoadDialogue(loadedDialogue.goodbye.nextDialogue);
        else transform.GetChild(0).gameObject.SetActive(false);
    }

    private void ButtonPressed(int _index)
    {
        if (loadedDialogue.dialogueOptions[_index].nextDialogue != null) LoadDialogue(loadedDialogue.dialogueOptions[_index].nextDialogue);
        else DisplayResponse(loadedDialogue.dialogueOptions[_index].response);
    }

    private void DisplayResponse(string _response) => responseText.text = _response;
    private void ClearButtons() { foreach (Transform child in content) Destroy(child.gameObject); }    
}
