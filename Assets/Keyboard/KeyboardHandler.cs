using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum FontState
{
    Lower, //Minuscule
    Upper, //Majuscule
    FullUpper //Verrouillage majuscule
}
public class KeyboardHandler : MonoBehaviour
{

    private static KeyboardHandler _Instance = null;
    public static KeyboardHandler Instance
    {
        get
        {
            if (_Instance == null) _Instance = FindObjectOfType<KeyboardHandler>();
            return _Instance;
        }
    }

    private FontState currentFontState;
    public string text { get; private set; }

    private bool isAzerty = true;

    private Interactable[] interactables;
    private TextMeshPro[] Texts;
    private Animator animator;
    private RadialView radView;
    private Billboard billBoard;

    [SerializeField]
    private bool StartOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        interactables = GetComponentsInChildren<Interactable>();
        Texts = GetComponentsInChildren<TextMeshPro>();
        animator = GetComponent<Animator>();
        radView = GetComponent<RadialView>();
        billBoard = GetComponent<Billboard>();
        animator.SetBool("isAzerty", isAzerty);
        currentFontState = FontState.Lower;
        if (StartOpen == false)
        {
            Close(false);
        }
    }

    public void AddLetter(string value)
    {
        switch (currentFontState)
        {
            case FontState.Lower:
                text += value;
                break;
            case FontState.Upper:
                text += value.ToUpper();
                currentFontState = FontState.Lower;
                UpperToLower();
                break;
            case FontState.FullUpper:
                text += value.ToUpper();
                break;
        }
    }

    public void RemoveLast()
    {
        if (text != "")
        {
            text = text.Remove(text.Length - 1);
        }
    }

    public void PassLine()
    {
        text += "\n";
    }

    public void SetUpper()
    {
        currentFontState += (int)currentFontState != 2 ?1:-2;
        UpperToLower();
    }

    /// <summary>
    /// Function for closing the keyboard
    /// </summary>
    /// <param name="isPersistant">False for clear the text</param>
    public void Close(bool isPersistant)
    {
        if (!isPersistant && text != "")
        {
            text = "";
        }

        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void Open()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void ChangeLanguage()
    {
        isAzerty = !isAzerty;
        animator.SetBool("isAzerty", isAzerty);
    }

    private void AzertyToQwerty()
    {
        for(int i = 0; i < interactables.Length; i++)
        {
            switch (interactables[i].gameObject.name)
            {
                case "A":
                    interactables[i].OnClick.RemoveAllListeners();
                    interactables[i].OnClick.AddListener(delegate {AddLetter(isAzerty?"a" : "q"); });
                    Texts[i].text = isAzerty ? "a" : "q";
                    break;
                case "Z":
                    interactables[i].OnClick.RemoveAllListeners();
                    interactables[i].OnClick.AddListener(delegate { AddLetter(isAzerty ? "z" : "w"); });
                    Texts[i].text = isAzerty ? "z" : "w";
                    break;
                case "Q":
                    interactables[i].OnClick.RemoveAllListeners();
                    interactables[i].OnClick.AddListener(delegate {AddLetter(isAzerty?"q" : "a"); });
                    Texts[i].text = isAzerty ? "q" : "a";
                    break;
                case "W":
                    interactables[i].OnClick.RemoveAllListeners();
                    interactables[i].OnClick.AddListener(delegate { AddLetter(isAzerty ? "w" : "z"); });
                    Texts[i].text = isAzerty ? "w" : "z";
                    break;
                case "AzertyToQwerty":
                    Texts[i].text = isAzerty ? "Qwerty" : "Azerty";
                    break;
            }
        }
    }

    private void UpperToLower()
    {
        for(int i = 0; i < Texts.Length; i++)
        {
            if(Texts[i].text.Length == 1)
            {
                if (currentFontState != FontState.Lower) {
                    Texts[i].text = Texts[i].text.ToUpper();
                }
                else
                {
                    Texts[i].text = Texts[i].text.ToLower();
                }
            }
        }
    }


    /// <summary>
    /// Attaching the keyboard to an Object to follow it under
    /// </summary>
    /// <param name="go">Attach this object, if null let it go</param>
    public void AttachKeyboard(Transform go = null)
    {
        if (go != null)
        {
            radView.enabled = false;
            billBoard.enabled = false;
            this.transform.SetParent(go);
            transform.localPosition = Vector3.down*0.6f;
            transform.localRotation = Quaternion.identity;
        }
        else
        {
            this.transform.SetParent(null);
            radView.enabled = true;
            billBoard.enabled = true;
        }
    }
}
