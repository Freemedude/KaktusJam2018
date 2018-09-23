using UnityEngine;

public class StartDialogue : MonoBehaviour
{
    public DialogueTrigger[] playerDialogue;
    private int playerDialogueIndex = 0;
    public DialogueTrigger[] swanobiDialogue;
    private int swanobiDialogueIndex = 0;

    public SwanobiController swanobiController;

    public void StartStory()
    {
        PlayerTalk();
        
        SwanobiTalk();

        swanobiController.MoveToPoint1();

        PlayerTalk();

        SwanobiTalk();

        PlayerTalk();

        SwanobiTalk();

        PlayerTalk();

        SwanobiTalk();

        swanobiController.MoveToPoint2();
    }

    private void PlayerTalk()
    {
        playerDialogue[playerDialogueIndex++].SendMessage("TriggerMessage");

    }

    private void SwanobiTalk()
    {
        swanobiDialogue[swanobiDialogueIndex++].SendMessage("TriggerMessage");

    }
    
}