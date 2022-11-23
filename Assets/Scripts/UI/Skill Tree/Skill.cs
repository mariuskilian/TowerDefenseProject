using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using UnityEngine.UI;

public class Skill : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector] public List<Skill> nextSkills;

    [HideInInspector] public Button button;
    [HideInInspector] public MageClass mageClass;
    [HideInInspector] public int cost;

    protected string skillName;
    protected string skillDesc; // ...ription

    public bool isFirstSkill = false;
    public bool completesBranch = false;

    private bool _unlockable;
    public bool Unlockable { get { return _unlockable; } set { _unlockable = value; UpdateButtonAppearance(); } }
    private bool _unlocked;
    public bool Unlocked { get { return _unlocked; } set { _unlocked = value; UpdateButtonAppearance(); } }

    protected MagesJSONParser.Mage mageJson;
    protected SkillManager SM;

    public void Start()
    {
        InitButton();
        InitStats();
        InitNextSkills();
        SM = SkillManager.Instance;
    }

    void InitButton()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(TryUnlockSkill);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.hovered.Count > 0 && eventData.hovered[0] != this) return;
        SkillDescriptionManager.Instance.SetText(skillName, skillDesc, cost);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SkillDescriptionManager.Instance.ClearText();
    }

    void InitStats()
    {
        mageJson = MagesJSONParser.Instance.magesJson.mages[(int)mageClass];
        if (isFirstSkill)
        {
            cost = mageJson.cost;
            skillName = mageJson.name;
            skillDesc = mageJson.description;
        }
        else foreach (var skillJson in mageJson.skills) if (skillJson.id == gameObject.name)
                {
                    cost = skillJson.cost;
                    skillName = skillJson.name;
                    skillDesc = skillJson.description;
                }
        Unlockable = isFirstSkill;
        Unlocked = false;
    }

    void InitNextSkills()
    {
        nextSkills = new List<Skill>();
        foreach (Transform skillT in transform)
        {
            Skill s;
            if (skillT.TryGetComponent<Skill>(out s)) nextSkills.Add(s);
        }
    }

    void OnEnable()
    {
        GameStateManager.OnStateChange += StateChangeHandler;
    }
    void OnDisable()
    {
        GameStateManager.OnStateChange -= StateChangeHandler;
    }

    void StateChangeHandler(GameState newState)
    {
        switch (newState)
        {
            case GameState.PRE_ROUND:
            case GameState.IDLE:
                UpdateButtonAppearance();
                break;
            default:
                break;
        }
    }

    public virtual void TryUnlockSkill()
    {
        if (SM.TryUnlockSkill(this))
        {
            Unlocked = true;
            if (completesBranch) FinalSkill.Instance.CheckUnlockable();
            foreach (Skill ns in nextSkills) ns.Unlockable = true;
            if (isFirstSkill) SM.mageSpawner.SpawnMage(mageClass);
        }
    }

    public void LockSkill()
    {
        foreach (Skill ns in nextSkills) ns.Unlockable = false;
        Unlocked = false;
    }

    void UpdateButtonAppearance()
    {
        // Udpate button based on if it is unlockable and/or unlocked
        button.interactable = GameStateManager.Instance.State == GameState.IDLE && Unlockable && !Unlocked;
        // Set color of disabled button
        var colors = button.colors;
        if (Unlocked) colors.disabledColor = new Color(0x52 / 255f, 0xE7 / 255f, 0x62 / 255f);
        else if (!Unlockable) colors.disabledColor = new Color(226 / 255f, 82 / 255f, 82 / 255f, 1);
        else colors.disabledColor = new Color(200 / 255f, 200 / 255f, 200 / 255f, 1);
        button.colors = colors;
    }
}