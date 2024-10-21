using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using System.Threading;

public class DialogueManager : MonoBehaviour
{
    //private enum BoxStyles { Null, GenreSelection, format_0, format_1 }
    private string[] csvFilePath = { 
        "TestDialog",   //0
    };


    //[SerializeField] GameManager gameManager;

    [SerializeField] DialogueBox dialogueBox;
    [SerializeField] DialogueName dialogueName;
    //[SerializeField] DialogueOption[] dialogueOption;

    //[SerializeField] Sprite[] boxSprites;

    private int selectedFile = 0;
    private List<DialogueData> dialogueList;
    private DialogueData lastData;
    private int index = 0;
    private bool dialogueEnabled = false;
    //private int optionCount = 0;

    //private boxSelection style;

    public delegate void DialogueEndHandler();
    public static event DialogueEndHandler OnDialogueEnd;
    //private string lastActionMap;


    /*private struct boxSelection
    {
        public BoxStyles box;
        public Sprite mainSprite;
        public Sprite optionSprite;
        public Sprite nameSprite;
        public Vector2 pos;
        public Vector2 mainSize;
        public Vector2 mainPos;
        public Vector2 namePos;
        public Vector2 optionSize;
        public Vector2 optionPos;
    }*/
    public void LoadDialogueData()
    {
        lastData = null;
        dialogueList = CSVImporter.GetDialogueData(csvFilePath[selectedFile]);
    }
    public void LoadDialogueData(int num)
    {
        index = 0;
        selectedFile = num;
        LoadDialogueData();
    }
    public void LoadDialogueData(int num, int ind)
    {
        index = ind;
        selectedFile = num;
        LoadDialogueData();
    }
    public void DialogueBasicClick()
    {
        /*if (dialogueBox.GetOptional())
        {
            dialogueBox.SetWritingText(false);
            dialogueEnabled = false;
        }else*/ if (dialogueBox.GetWritingText())
        {
            //Write instant text
            dialogueBox.SetWritingText(false);
        }
        else
        {
            if (lastData != null && lastData.type == "end")
            {
                DisableDialogueSystem();
                /*switch (lastData.extraType)
                {
                    case "characterDesign":
                        gameManager.StartCharacterDesign();
                        break;
                    case "freeMovement":
                        playerControls.SwitchCurrentActionMap("OnLab");
                        // Aviso a NPCs para que se sigan moviendo
                        if (OnDialogueEnd != null)
                        {
                            OnDialogueEnd();
                        }
                        break;
                    case "lastActionMap":
                        playerControls.SwitchCurrentActionMap(lastActionMap);
                        // Aviso a NPCs para que se sigan moviendo
                        if (OnDialogueEnd != null)
                        {
                            OnDialogueEnd();
                        }
                        break;
                    case "startDoormanGame":
                        gameManager.StartDoormanGame();
                        break;
                    case "continueDoormanGame":
                        gameManager.ContinueDoormanGame();
                        break;
                    case "goToLabLevel1":
                        gameManager.GoToNextLabLevel();
                        break;
                    case "endLevel1":
                        gameManager.EndLevel();
                        break;
                    case "endLevel2":
                        gameManager.EndLevel();
                        break;
                    case "endGame":
                        gameManager.EndGame();
                        break;
                    default:
                        break;
                }*/
            }
            else
            {
                //Write next dialogue
                NextDialogue();
            }
        }
    }
    public void NextDialogue()
    {
        if(!(index < dialogueList.Count)) 
        {
            return;
        }
        DialogueData nextData = dialogueList[index];
        
        switch (nextData.type)
        {
            case "":
                SetTextInTime(nextData);
                break;
            case "instant":
                SetText(nextData);
                break;
            case "end":
                SetTextInTime(nextData);
                break;
            /*case "optional":
                SetTextInTime(nextData);
                string[] indexValues = nextData.nextIndex.Split("_");
                SetOptions(indexValues);
                break;*/
        }
        if(nextData.nextIndex != "")// && nextData.type != "optional")
        {
            index = int.Parse(nextData.nextIndex);
        }
        else
        {
            index++;
        }
        if (lastData != null)
        {
            if (lastData.speaker != nextData.speaker)
            {
                
            }
        }
        lastData = nextData;
    }
    /*public void NextDialogueFromOptions(int nextIndex, int selectedOption)
    {
        DialogueData optionData = dialogueList[selectedOption];
        HideOptions();
        dialogueBox.SetOptional(false);
        dialogueEnabled = true;
        //Option consecuences
        switch (optionData.type)
        {
            case "genreSelection":
                if(optionData.genre == "sm")
                {
                    GameConfiguration.SaveGenre(GameConfiguration.Genre.m);
                }
                else if(optionData.genre == "sf")
                {
                    GameConfiguration.SaveGenre(GameConfiguration.Genre.f);
                }
                else
                {
                    GameConfiguration.SaveGenre(GameConfiguration.Genre.nb);
                }
                DisableDialogueSystem();
                gameManager.GenreSelected();
                break;
            default:
                NextDialogue();
                break;
        }
        if(nextIndex != -1)
        {
            index = nextIndex;
        }
    }*/
    private void SetTextInTime(DialogueData nextData)
    {
        dialogueName.SetText(nextData.speaker);
        dialogueBox.SetTextInTime(nextData.text);
    }
    private void SetText(DialogueData nextData)
    {
        dialogueName.SetText(nextData.speaker);
        dialogueBox.SetText(nextData.text);
    }
    public void DisableDialogueSystem()
    {
        dialogueEnabled = false;
        dialogueBox.gameObject.SetActive(false);
        dialogueName.gameObject.SetActive(false);
    }

    /*private void SetOptions(string [] indexValues)
    {
        dialogueBox.SetOptional(true);
        optionCount = indexValues.Length;
        for(int i = 0; i < optionCount; i++)
        {
            dialogueOption[i].SetDialogueOption(dialogueList[int.Parse(indexValues[i])]);
        }
    }
    public void ShowOptions()
    {
        for(int i = 0; i < optionCount; i++)
        {
            dialogueOption[i].Show();
        }
    }
    public void HideOptions()
    {
        for(int i = 0; i< optionCount; i++)
        {
            dialogueOption[i].Hide();
        }
    }*/



    //Input Control
    //public PlayerInput playerControls;
    public InputActionReference click;

    public void EnableDialogueSystem()
    {
        LoadDialogueData();

        dialogueEnabled = true;
        dialogueName.gameObject.SetActive(true);
        dialogueBox.gameObject.SetActive(true);
    }
    void OnEnable()
    {
        click.action.started += ClickFunction;
    }

    void OnDisable()
    {
        click.action.started -= ClickFunction;
    }

    private void ClickFunction(InputAction.CallbackContext obj)
    {
        print("clickk");
        if (dialogueEnabled)
        {
            DialogueBasicClick();
        }
    }

    /*private void SelectStyle(BoxStyles newStyle)
    {
        if(style.box != newStyle)
        {
            style.box = newStyle;
            switch (newStyle)
            {
                case BoxStyles.GenreSelection:
                    style.mainSprite = boxSprites[4];
                    style.optionSprite = boxSprites[3];
                    style.mainSize = new Vector2(1530f, 350);
                    style.mainPos = new Vector2(0f, 260f);
                    style.optionSize = new Vector2(620f, 220f);
                    style.optionPos = new Vector2(0, 0);
                    // Extra Text Alignement
                    dialogueBox.GetTextMeshPro().verticalAlignment = VerticalAlignmentOptions.Middle;
                    break;
                case BoxStyles.format_0:
                    style.mainSprite = boxSprites[5];
                    style.nameSprite = boxSprites[1];
                    style.mainSize = new Vector2(1080f, 400f);
                    style.mainPos = new Vector2(-300f, 225f);
                    style.namePos = new Vector2(-500f, 75f);
                    // Extra Text Alignement
                    dialogueBox.GetTextMeshPro().verticalAlignment = VerticalAlignmentOptions.Top;
                    break;
                case BoxStyles.format_1:
                    style.mainSprite = boxSprites[0];
                    style.nameSprite = boxSprites[1];
                    style.optionSprite = boxSprites[3];
                    style.mainSize = new Vector2(1530f, 350f);
                    style.mainPos = new Vector2(0f, 330f);
                    style.namePos = new Vector2(-400, 217f);
                    break;
            }
            UpdateMaterials();
        }
    }
    private void UpdateMaterials()
    {
        Image image;
        // Main Box
        image = dialogueBox.GetComponent<Image>();
        image.sprite = style.mainSprite;
        image.rectTransform.sizeDelta = style.mainSize;
        image.rectTransform.anchoredPosition = style.mainPos;

        // Name Box
        image = dialogueName.GetComponent<Image>();
        image.sprite = style.nameSprite;
        image.rectTransform.anchoredPosition = style.namePos;

        // Option Box
        for (int i = 0; i < dialogueOption.Length; i++)
        {
            image = dialogueOption[i].GetComponent<Image>();
            image.sprite = style.optionSprite;
            image.rectTransform.sizeDelta = style.optionSize;
            image.rectTransform.anchoredPosition = new Vector2(style.optionPos.x, style.optionPos.y - i * style.optionSize.y * 0.9f);
        }
    }*/
}
