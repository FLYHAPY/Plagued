using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueSystem: MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public Button optionButtonLeft;
    public Button optionButtonRight;
    public DialogueNode currentNode;
    public GameObject crossHair;
    public bool dialogueended;
    public float timer;
    private bool collided;
    public bool inDialogue;


    void Start()
    {
        InitializeDialogueTree();
        optionButtonLeft.gameObject.SetActive(false);
        optionButtonRight.gameObject.SetActive(false);
        dialogueText.gameObject.SetActive(false);
    }

    void InitializeDialogueTree()
    {
        DialogueNode node1 = new DialogueNode("Yo I want to stop the bigbang");
        DialogueNode node2 = new DialogueNode("Really that easy, that's no fun", "Good");
        DialogueNode node3 = new DialogueNode("We will battle then", "Bad");

        node1.LeftOption = new DialogueOption("OK", node2);
        node1.RightOption = new DialogueOption("NO", node3);


        currentNode = node1;
    }

    public void StartDialogue()
    {
        crossHair.SetActive(false);
        dialogueText.gameObject.SetActive(true);
        dialogueText.text = currentNode.DialogueText;
        DisplayOptions();
    }

    void ContinueDialogue(DialogueOption selectedOption)
    {
        currentNode = selectedOption.NextNode;
        dialogueText.text = currentNode.DialogueText;

        optionButtonLeft.gameObject.SetActive(false);
        optionButtonRight.gameObject.SetActive(false);

        if (currentNode.LeftOption != null || currentNode.RightOption != null)
        {
            DisplayOptions();
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            dialogueended = true;
        }
    }

    private void Update()
    {
        if (dialogueended)
        {
            Time.timeScale = 1f;
            timer += Time.deltaTime;
        }
        if (timer > 3) 
        {
            crossHair.SetActive(true);
            dialogueended = false;
            dialogueText.gameObject.SetActive(false);
        }

        if(collided)
        {
            if (Input.GetKeyUp(KeyCode.Q))
            {
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                inDialogue = true;
                StartDialogue();
            }
        }
    }

    void DisplayOptions()
    {
        if (currentNode.LeftOption != null)
        {
            optionButtonLeft.gameObject.SetActive(true);
            optionButtonLeft.GetComponentInChildren<TextMeshProUGUI>().text = currentNode.LeftOption.OptionText;
            optionButtonLeft.onClick.RemoveAllListeners();
            optionButtonLeft.onClick.AddListener(() => ContinueDialogue(currentNode.LeftOption));
        }

        if (currentNode.RightOption != null)
        {
            optionButtonRight.gameObject.SetActive(true);
            optionButtonRight.GetComponentInChildren<TextMeshProUGUI>().text = currentNode.RightOption.OptionText;
            optionButtonRight.onClick.RemoveAllListeners();
            optionButtonRight.onClick.AddListener(() => ContinueDialogue(currentNode.RightOption));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player2")
        {
            collided = true;
        }
    }
}

public class DialogueNode
{
    public string DialogueText;
    public string? Finaloutcome; 
    public DialogueOption LeftOption;
    public DialogueOption RightOption;

    public DialogueNode(string text, string? finaloutcome = null)
    {
        DialogueText = text;
        Finaloutcome = finaloutcome;
    }
}

public class DialogueOption
{
    public string OptionText;
    public DialogueNode NextNode;

    public DialogueOption(string text, DialogueNode nextNode)
    {
        OptionText = text;
        NextNode = nextNode;
    }
}
