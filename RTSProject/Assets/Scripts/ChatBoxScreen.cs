using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ChatBoxScreen : MonoBehaviour
{
 
    [SerializeField] private InputField _chatInput;

    [SerializeField] private Text _chatOutput;

    [SerializeField] private ScrollRect _chatScrollRect;

    private bool _focusedRequested = false;

   
    public void SetChatInput(string pInput)
    {
        _chatInput.text = pInput;
        _focusedRequested = true;
    }

    public void RegisterChatInputHandler(UnityAction pChatHandler)
    {
        _chatInput.onEndEdit.AddListener((value) => pChatHandler());
    }

    public void UnregisterChatInputHandlers()
    {
        _chatInput.onEndEdit.RemoveAllListeners();
    }

    public void AddChatLine(string pChatLine)
    {
        // _chatOutput.text += pChatLine + "\n";
        // _chatScrollRect.verticalNormalizedPosition = 0;
    }

    private void Update()
    {
        checkFocus();
    }

    private void checkFocus()
    {
        if (_focusedRequested)
        {
            _chatInput.ActivateInputField();
            _chatInput.Select();
            _focusedRequested = false;
        }
    }

}
