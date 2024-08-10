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

    // 선택된 플레이어 유닛 정보
    private GameObject chosenUnit;
    private PlayerHealth chosenUnitHP;

    // 유닛 그룹 정보창 UI
    public Image[] unitIcons;

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
            DisplaySingleUnitUI();
        }
        else if(PlayerController.Instance.chosenObject.Count >=2)
        {
            DisplayUnitGroupUI(PlayerController.Instance.chosenObject.Count);
        }
        else 
        {
            SingleUnitUI.SetActive(false);
            UnitGroupUI.SetActive(false);
        }
    }

    void DisplaySingleUnitUI()
    {
        SingleUnitUI.SetActive(true);
        UnitGroupUI.SetActive(false);

        chosenUnit = PlayerController.Instance.chosenObject[0];                                  // 선택된 유닛이 하나 -> 리스트의 첫 번째 유닛
        Debug.LogFormat("{0} 단독 선택", chosenUnit.name);

        chosenUnitHP =chosenUnit. GetComponent<PlayerHealth>();

        if (wholeUnitDict.ContainsKey(chosenUnit.name))
        {
            unitName.text = chosenUnit.name;                                                                // 유닛 이름
            unitImage.sprite = wholeUnitDict[chosenUnit.name];                                              // 유닛 이미지
            unitHP.text = chosenUnitHP.currentHP.ToString() + " / " + chosenUnitHP.maxHP.ToString();        // 유닛 현재 체력
            // 해당 유닛이 사살한 적의 수 추가
        }
    }

    void DisplayUnitGroupUI(int unitNum)
    {
        SingleUnitUI.SetActive(false);
        UnitGroupUI.SetActive(true);

        Debug.LogFormat("플레이어 유닛 {0}개 선택", unitNum);

        for (int name = 0; name < PlayerUnitPooling.Instance.playerUnitNames.Length; name++)
        {
            string unitName=PlayerUnitPooling.Instance.playerUnitNames[name];
            int numOfUnits=0;

            Debug.LogFormat("검색할 유닛 이름: {0}", unitName);

            for (int i = 0; i < unitNum; i++)
            {
                if (PlayerController.Instance.chosenObject[i].name==unitName)
                {
                    numOfUnits++;
                }
            }

            if(numOfUnits>0)
            {
                Debug.LogFormat("{0} {1}개 복수선택", unitName, numOfUnits);
                unitIcons[name].gameObject.SetActive(true);
                unitIcons[name].sprite = wholeUnitDict[unitName];
                unitIcons[name].transform.Find("UnitNumber").GetComponent<TextMeshProUGUI>().text = numOfUnits.ToString();
            }
        }

        for(int i= PlayerUnitPooling.Instance.playerUnitNames.Length; i<unitIcons.Length; i++)
        {
            unitIcons[i].gameObject.SetActive(false);
        }
    }

    void CreateUnitList()
    {
        int numOfUnitName = wholeUnitNames.Count;
        int numOfUnitImage = wholeImages.Count;

        //Debug.LogFormat("whole Dict Key: {0}, Value: {1}", numOfUnitName, numOfUnitImage);
        int count = Mathf.Min(numOfUnitName, numOfUnitImage);

        for(int i=0; i< count; i++)
        {
            wholeUnitDict.Add(wholeUnitNames[i], wholeImages[i]);
            //Debug.LogFormat("wholeUnitDict에 {0} : {1} 추가", wholeUnitNames[i], wholeImages[i]);
        }
    }
}
