using TMPro;
using UnityEngine;
using System.Collections;
public class GemiJob : MonoBehaviour
{
    [Header("Script Atamaları")]
    [SerializeField] private bool isGemiUpgrade;
    [SerializeField] private GameObject uyariPanel;
    [SerializeField] private TextMeshProUGUI uyariText;
    [SerializeField] private TextMeshProUGUI upgrade_kargo_Text;
    [SerializeField] private GameObject gemiUpgradePanel;
    [SerializeField] private Gemiler gemiUpgrade;
    [SerializeField] private GameObject gemiKargoPanel;
    [SerializeField] private GemiKargoHesaplama gemiKargo;
    private void Start()
    {
        GemiJobDurum();
    }
    public void GemiJobDurum()
    {
        isGemiUpgrade = !isGemiUpgrade;
        if (isGemiUpgrade)
        {
            gemiUpgrade.GemilerHazirlik();
            upgrade_kargo_Text.text = "Bölüm : Gemi Kargo Hesaplama";
        }
        else
        {
            gemiKargo.GemilerHazirlik();
            upgrade_kargo_Text.text = "Bölüm : Gemi Upgrade";
        }
        gemiUpgradePanel.SetActive(isGemiUpgrade);
        gemiKargoPanel.SetActive(!isGemiUpgrade);
    }
    public void UyariPanel(string uyari)
    {
        StartCoroutine(UyariYap(uyari));
    }
    IEnumerator UyariYap(string uyari)
    {
        uyariText.text = uyari;
        uyariPanel.SetActive(true);
        yield return new WaitForSeconds(2);
    }
}