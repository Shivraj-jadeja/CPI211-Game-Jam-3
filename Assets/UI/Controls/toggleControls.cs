using UnityEngine;

public class toggleControls : MonoBehaviour
{
    public GameObject uiControls;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            uiControls.SetActive(!uiControls.activeSelf);
        }
    }
}
