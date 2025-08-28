using UnityEngine;
using UnityEngine.UI;

public class HpUI : MonoBehaviour
{
    public Hp hp;
    public Slider slider;

    void Update()
    {
        slider.maxValue = hp.maxHp;
        slider.value    = hp.currentHp;
    }
}