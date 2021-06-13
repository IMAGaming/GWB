using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private Text context = default;
    [SerializeField] private Text characterName = default;
    [SerializeField] private Image characterHead = default;
    [SerializeField] private GameObject dialogueUI = default;

    public void Start()
    {
        if (dialogueUI == null)
            dialogueUI = transform.Find("DialogueUI").gameObject;
    }

    // 更改mask大小来实现打字效果
    public void TextLineOnUpdate(string line, float progress)
    {
        context.text = line;
    }

    public void SetUpDialogueUI(string name, string line, Sprite img)
    {
        characterName.text = name;
        context.text = line;
        characterHead.sprite = img;
    }

}
