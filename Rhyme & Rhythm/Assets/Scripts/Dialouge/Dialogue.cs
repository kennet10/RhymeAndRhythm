using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class Dialogue : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textComponent;
    [SerializeField]
    private int nextScene;
    [SerializeField]
    [TextArea(10, 15)]
    private string[] lines;
    [SerializeField]
    private float textSpeed;
    

    private int index;

    // Start is called before the first frame update
    void Start()
    {
        textComponent.text = string.Empty;
        StartDialogue();
    }


    private void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach(char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    public void NextLineButton()
    {
        if (textComponent.text == lines[index])
        {
            NextLine();
        }
        else
        {
            StopAllCoroutines();
            textComponent.text = lines[index];
        }
    }

    private void NextLine()
    {
        if(index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
            SceneManager.LoadSceneAsync(nextScene);
        }
    }

    public void PrevLineButton()
    {
        if (textComponent.text == lines[index])
        {
            PrevLine();
        }
        else
        {
            StopAllCoroutines();
            textComponent.text = lines[index];
        }
    }
    private void PrevLine()
    {
        if (index > 0)
        {
            index--;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }

    }
    public void SkipScene()
    {
        gameObject.SetActive(false);
        SceneManager.LoadSceneAsync(nextScene);
    }
}
