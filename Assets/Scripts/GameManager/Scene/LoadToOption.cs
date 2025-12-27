using UnityEngine;

public class LoadToOption : MonoBehaviour
{
    [Header("게임 옵션 창")]
    public GameObject option;

    public void ClickOptionButton()
    {
        option.SetActive(true);
    }
}
