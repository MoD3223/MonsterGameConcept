using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterGameConcept
{
    public static class Player
    {
        public static uint Money = 0;
        public static TimeSpan TimePlayed = TimeSpan.Zero;
        public static DateTime currentTime = new DateTime();
        public static void Start()
        {
            //Load save from file if exists
            currentTime = DateTime.Now;
        }

        public static void OnQuit()
        {
            TimePlayed += (DateTime.Now - currentTime);
            //TODO: Add more stats
            //Add autosave
            
        }



    }
}




/*
[System.Serializable]
public class NPCDialogue
{
    public string npcName;
    public List<string> helloMessages;
    public string goodbyeMessage;
    private bool showChoiceWindow = false;
    private bool choiceMade = false;
}




using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    public List<NPCDialogue> npcDialogues;
    public GameObject helloWindow;
    public GameObject goodbyeWindow;
    public Text helloText;
    public Text goodbyeText;

    private bool isInteracting = false;
    private int currentHelloIndex = 0;
    private NPCDialogue currentNPCDialogue;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(isInteractive){
            InteractWithObject();
             }
            else(){
                goodbyeWindow.SetActive(false);
                isInteracting = true;
                    }
        }
    }

    void InteractWithObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            NPCInteraction npcInteraction = hit.collider.GetComponent<NPCInteraction>(); //Shouldnt there be hit.collider.compareTag??
            if (npcInteraction != null)
            {
                currentNPCDialogue = npcDialogues.Find(d => d.npcName == NPCDialogue.npcName);
                if (currentNPCDialogue != null)
                {
                    if (currentHelloIndex < currentNPCDialogue.helloMessages.Count)
                    {
                        helloText.text = currentNPCDialogue.helloMessages[currentHelloIndex];
                        helloWindow.SetActive(true);
                        currentHelloIndex++;
                    }
                    else if (currentHelloIndex == currentNPCDialogue.helloMessages.Count && currentNPCDialogue.showChoiceWindow)
                        {
                            showChoiceWindow = true;
                            choiceText.text = "Do you want to continue?";
                            choiceWindow.SetActive(true);
                        }

                    else if (showChoiceWindow)
                    {
                        // Process the choice made by the player
                        ProcessChoice();
                    }
                    else
                    {
                        CloseWindows();
                    }
                }
            }
        }
    }

    void CloseWindows()
    {
        helloWindow.SetActive(false);
        goodbyeText.text = currentNPCDialogue.goodbyeMessage;
        goodbyeWindow.SetActive(true);
        isInteracting = false;
        currentHelloIndex = 0;
    }


void ProcessChoice()
    {
        // Process the player's choice (e.g., set dialogue options, trigger events, etc.)
        if (choiceMade)
        {
            CloseWindows();
        }
    }


public void OnChoiceButtonClicked(bool choice)
    {
        // Handle button click event from the choice window
        choiceMade = true;

        if (choice)
        {
            // Handle the "YES" button click
        }
        else
        {
            // Handle the "NO" button click
        }
    }

}





 */