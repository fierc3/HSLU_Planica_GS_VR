using System.Collections;
using TMPro;
using UnityEngine;

public class PreCheck : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI infoText;

    void Start()
    {
        StartCoroutine(CheckHardwareReady());
    }
    private IEnumerator CheckHardwareReady()
    {
        yield return new WaitForEndOfFrame(); // wait one frame
        if (ViewResetter.IsHardwarePresent())
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("OpeningScene");
        }
        else
        {
            infoText.SetText("Please start the application while wearing the headset!");
        }
    }
}
