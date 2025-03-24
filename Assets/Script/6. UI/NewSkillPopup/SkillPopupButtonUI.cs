using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillPopupButtonUI : UI_Base
{
    enum Images
    {
        SkillIcon,
        Background,
        Background_Selected,
        IconFrame_Selected,
    }

    enum Texts
    { 
        Name,
        Description,
    }

    private Image _skillIcon;
    private Image _background;
    private Image _background_Selected;
    private Image _iconFrame_Selected;

    private TextMeshProUGUI _name;
    private TextMeshProUGUI _description;

    public bool IsSelected => _background_Selected.gameObject.activeSelf; 
 
    public override void Init()
    {
        Bind<Image>(typeof(Images));
        Bind<TextMeshProUGUI>(typeof(Texts));

        _skillIcon = Get<Image>((int)Images.SkillIcon);
        _background = Get<Image>((int)Images.Background);
        _background_Selected = Get<Image>((int)Images.Background_Selected);
        _iconFrame_Selected = Get<Image>((int)Images.IconFrame_Selected);

        _name = Get<TextMeshProUGUI>((int)Texts.Name);
        _description = Get<TextMeshProUGUI>((int)Texts.Description);

        SetSelected(false);
    }
    public void SetInfo(SkillInfo skillInfo)
    {
        _name.text = skillInfo.Name;
        _description.text = skillInfo.Description;
        _skillIcon.sprite = Managers.Resource.Load<Sprite>($"Sprite/Skill/{skillInfo.Image}"); 
    } 
    public void SetSelected(bool isSelected)
    {
        _background.gameObject.SetActive(!isSelected);
        _background_Selected.gameObject.SetActive(isSelected);
        _iconFrame_Selected.gameObject.SetActive(isSelected); 
    } 
}