using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ContentHandler : MonoBehaviour
{

    private TextMeshPro valueContent;

    // Start is called before the first frame update
    void Start()
    {
        valueContent = GetComponentInChildren<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        valueContent.text = KeyboardHandler.Instance.text;
    }
}
