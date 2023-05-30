using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class KeybindsManager : MonoBehaviour
{
    

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
    List<string> keyList;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);

        // make list of keys
        keyList = new List<string>();
        string[] tempList = {key1, key2, key3, key4, key5, key6, key7, key8};
        keyList.AddRange(tempList);
        Debug.Log("keyList working properly");

        panel = GameObject.FindGameObjectWithTag("keybinds_panel");
        StartPanel();


        
    }

    // Update is called once per frame
    void Update()
    {

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

        // ---------------------
        panel.SetActive(false);
    }

    public void OpenPanel()
    {
        panel.SetActive(true);
    }

    public void ClosePanel()
    {
        panel.SetActive(false);
    }

    public void RequestKey()
    {

    }

}
