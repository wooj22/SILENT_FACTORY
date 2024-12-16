using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] Text essenceUI;
    [SerializeField] Text kitUI;
    [SerializeField] Text ammunition762UI;
    [SerializeField] Text ammunition45UI;
    [SerializeField] Text ammunition12UI;

    // ½Àµæ ÇöÈ² count
    public int essence;
    public int kit;
    public int ammunition762;
    public int ammunition45;
    public int ammunition12;

    public void UpdateEssence(int n)
    {
        essence += n;
        essenceUI.text = essence + " / 10";
    }

    public void UpdateKit(int n)
    {
        kit += n;
        kitUI.text = " * " + kit;
    }

    public void Update762(int n)
    {
        ammunition762 += n;
        if (ammunition762 < 0) ammunition762 = 0;
        ammunition762UI.text = " * " + ammunition762;
    }

    public void Update45(int n)
    {
        ammunition45 += n;
        if (ammunition45 < 0) ammunition45 = 0;
        ammunition45UI.text = " * " + ammunition45;
    }

    public void Update12(int n)
    {
        ammunition12 += n;
        if (ammunition12 < 0) ammunition12 = 0;
        ammunition12UI.text = " * " + ammunition12;
    }
}
