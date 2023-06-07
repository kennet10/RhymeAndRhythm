using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class KeybindsManager : MonoBehaviour
{
    public Animator animator;
    public bool debugMode;

    private string key1 = "E";
    private string key2 = "R";
    private string key3 = "U";
    private string key4 = "I";
    private string key5 = "D";
    private string key6 = "F";
    private string key7 = "J";
    private string key8 = "K";

    TextMeshProUGUI textField;
    GameObject panel;
    GameObject pressAnyKey;
    List<string> keyList;

    private bool waitingForKey;
    private int keyIndex;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);

        // make list of keys
        keyList = new List<string>();
        string[] tempList = { key1, key2, key3, key4, key5, key6, key7, key8 };
        keyList.AddRange(tempList);
        if(debugMode){Debug.Log("keyList working properly");}

        panel = GameObject.FindGameObjectWithTag("keybinds_panel");
        pressAnyKey = GameObject.FindGameObjectWithTag("press_any_key");

        StartPanel();

        waitingForKey = false;
        keyIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (waitingForKey)
        {
            // Check if any key is pressed
            if (Input.anyKeyDown)
            {
                // Get the new key
                KeyCode newKey = GetAnyKeyDown();

                // Update the game keybind with the new key
                UpdateGameKeybind(newKey);

                // Reset the UI text and stop waiting for key input
                textField.text = newKey.ToString();
                waitingForKey = false;
            }
        }
    }

    KeyCode GetAnyKeyDown()
    {
        foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(keyCode))
            {
                return keyCode;
            }
        }
        return KeyCode.None;
    }

    // initialize the panel setup
    public void StartPanel()
    {
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("key_tag");
        int counter = 0;
        foreach (GameObject obj in objectsWithTag)
        {
            TextMeshProUGUI textComponent = obj.GetComponent<TextMeshProUGUI>();

            if (textComponent != null)
            {
                textComponent.text = "" + keyList[counter];
                counter += 1;
            }
            else
            {
                Debug.Log("GameObject does not have a Text component.");
            }
        }

        panel.SetActive(false);
    }

    public void OpenPanel()
    {
        panel.SetActive(true);
        animator.SetBool("clickx", false);
        pressAnyKey.SetActive(false);
    }

    public void ClosePanel()
    {
        animator.SetBool("clickx", true);
        Invoke("SetActiveFalse", 1);
    }

    public void SetActiveFalse()
    {
        panel.SetActive(false);
    }

    public void RequestKey()
    {
        if (!waitingForKey)
        {
            // Start waiting for key input
            waitingForKey = true;

            Button button = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
            TextMeshProUGUI targetText = button.GetComponentInChildren<TextMeshProUGUI>();

            textField = targetText;
            textField.text = "...";

            // Determine the button index based on the button name
            //keyIndex = GetButtonIndex(button.name);
            string name = button.name;
            keyIndex = int.Parse(name.Substring(name.Length - 1)) - 1;

            pressAnyKey.SetActive(true);
        }
    }

    void UpdateGameKeybind(KeyCode newKey)
    {
        // Update the appropriate keybind based on the keyIndex

        if (keyIndex >= 0 && keyIndex < keyList.Count)
        {
            keyList[keyIndex] = newKey.ToString();
        }

        //for (int i = 0; i < keyList.Count; i++)
        {
        //    keyList[i] = "t";
        }

        pressAnyKey.SetActive(false);

        if (debugMode){ShowKeyList();}

    }

    void ShowKeyList()
    {
        string temp = "key settings:\n";
        for (int i = 0; i < keyList.Count; i++)
        {
            temp += keyList[i] + "";
            if (i == 3){temp += '-';}
        }
        Debug.Log(temp);

    }
}

    
