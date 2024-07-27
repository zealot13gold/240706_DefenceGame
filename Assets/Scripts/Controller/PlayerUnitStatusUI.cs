using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUnitStatusUI : MonoBehaviour
{
    // 단일 유닛 선택 시 UI
    public GameObject SingleUnitUI;
    // 유닛 그룹 선택 시 UI
    public GameObject UnitGroupUI;

    // UI에 들어갈 수 있는 모든 유닛 이미지
    public Dictionary<string, Sprite> wholeUnitDict;
    [SerializeField] public List<string> wholeUnitNames;
    [SerializeField] public List<Sprite> wholeImages;

    // 단일 유닛 정보창 UI
    public GameObject unitImageObject;
    public GameObject unitNameObject;
    public GameObject unitHPObject;
    public GameObject unitKillsEnemiesObject;

    private Image unitImage;
    private TextMeshProUGUI unitName;
    private TextMeshProUGUI unitHP;
    private TextMeshProUGUI unitKillsEnemies;

    // 유닛 그룹 정보창 UI
    public Sprite[] unitImages;

    private void Awake()
    {
        SingleUnitUI.SetActive(false);
        UnitGroupUI.SetActive(false);

        wholeUnitDict = new Dictionary<string, Sprite>();

        unitImage = unitImageObject.GetComponent<Image>();
        unitName = unitNameObject.GetComponent<TextMeshProUGUI>();
        unitHP = unitHPObject.GetComponent<TextMeshProUGUI>();
        unitKillsEnemies = unitKillsEnemiesObject.GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        CreateUnitList();
    }

    void Update()
    {
        if(PlayerController.Instance.chosenObject.Count == 1)
        {
            SingleUnitUI.SetActive(true);
            UnitGroupUI.SetActive(false);
            DisplaySingleUnitUI();
        }
        else if(PlayerController.Instance.chosenObject.Count >=2)
        {
            SingleUnitUI.SetActive(false);
            UnitGroupUI.SetActive(true);
            DisplayUnitGroupUI();
        }
        else 
        {
            SingleUnitUI.SetActive(false);
            UnitGroupUI.SetActive(false);
        }
    }

    void DisplaySingleUnitUI()
    {
        GameObject chosenUnit = PlayerController.Instance.chosenObject[0];                                  // 선택된 유닛이 하나 -> 리스트의 첫 번째 유닛
        PlayerHealth chosenUnitHP =chosenUnit. GetComponent<PlayerHealth>();



        if (wholeUnitDict.ContainsKey(chosenUnit.name))
        {
            unitName.text = chosenUnit.name;                                                                // 유닛 이름
            unitImage.sprite = wholeUnitDict[chosenUnit.name];                                              // 유닛 이미지
            unitHP.text = chosenUnitHP.currentHP.ToString() + " / " + chosenUnitHP.maxHP.ToString();        // 유닛 현재 체력
            // 해당 유닛이 사살한 적의 수 추가
        }
    }

    void DisplayUnitGroupUI()
    {
        // 유닛 아이콘 풀링
        // 유닛 아이콘에 이미지, 체력바 적용
        // 아이콘 위치(x, y축)
    }

    void CreateUnitList()
    {
        int numOfUnitName = wholeUnitNames.Count;
        int numOfUnitImage = wholeImages.Count;

        Debug.LogFormat("whole Dict Key: {0}, Value: {1}", numOfUnitName, numOfUnitImage);
        int count = Mathf.Min(numOfUnitName, numOfUnitImage);

        for(int i=0; i< count; i++)
        {
            wholeUnitDict.Add(wholeUnitNames[i], wholeImages[i]);
            Debug.LogFormat("wholeUnitDict에 {0} : {1} 추가", wholeUnitNames[i], wholeImages[i]);
        }
    }
}
