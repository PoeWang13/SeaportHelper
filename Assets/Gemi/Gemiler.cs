using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Gemiler : MonoBehaviour
{
    public enum OnValidType
    {
        Null,
        SortGold,
        Save,
        Load
    }
    public OnValidType onValid;
    private void OnValidate()
    {
        if (onValid == OnValidType.SortGold)
        {
            onValid = OnValidType.Null;
            gemiDatalar.kullanılmayanGemiler.Sort(SortGold);
            gemiDatalar.kullanılanGemiler.Sort(SortGold);
            SaveGemiler();
        }
        else if (onValid == OnValidType.Save)
        {
            onValid = OnValidType.Null;
            SaveGemiler();
        }
        else if (onValid == OnValidType.Load)
        {
            onValid = OnValidType.Null;
            LoadGemiler();
        }
    }
    [SerializeField] private GemiJob gemiJob;
    [Header("Gemi List Atamaları")]
    [SerializeField] private GemiSourceButton gemiSourceButton;
    [SerializeField] private Transform gemiKullanılanSecmeButtonParent;
    [SerializeField] private Transform gemiKullanılmayanSecmeButtonParent;
    [Header("Gemi Source Atamaları")]
    [SerializeField] private Toggle gemiKullanılan;
    [SerializeField] private TextMeshProUGUI gemiKullanılanText;
    [SerializeField] private TMP_InputField gemiName;
    [SerializeField] private TMP_InputField gemiBuyukluk;
    [SerializeField] private TMP_InputField gemiGold;
    [SerializeField] private TMP_InputField gemiOdun;
    [SerializeField] private TMP_InputField gemiKaynak;
    [SerializeField] private TMP_Dropdown gemiKaynakType;
    [SerializeField] private TMP_InputField gemiLimit;
    [SerializeField] private TMP_InputField gemiLimitNext;
    [SerializeField] private TextMeshProUGUI gemiLimitArtis;

    [Header("Gemi Datalar")]
    public PlayerData gemiDatalar = new PlayerData();
    // Gemi Source
    private GameObject seciliButon;
    private GemiSource seciliGemi;
    private int SortGold(GemiSource gemi1, GemiSource gemi2)
    {
        gemi1.gemiDepo[2] = gemi1.gemiDepo[1] - gemi1.gemiDepo[0];
        gemi2.gemiDepo[2] = gemi2.gemiDepo[1] - gemi2.gemiDepo[0];
        int g1 = gemi1.gemiGold / gemi1.gemiBuyukluk / gemi1.gemiDepo[2];
        int g2 = gemi2.gemiGold / gemi2.gemiBuyukluk / gemi2.gemiDepo[2];
        // Küçükten büyüğe doğru sıralar
        if (g1 > g2)
        {
            return 1;
        }
        else if (g2 > g1)
        {
            return -1;
        }
        return 0;
    }
    public void GemilerHazirlik()
    {
        for (int e = 0; e < gemiKullanılanSecmeButtonParent.childCount; e++)
        {
            Destroy(gemiKullanılanSecmeButtonParent.GetChild(e).gameObject);
        }
        for (int e = 0; e < gemiKullanılmayanSecmeButtonParent.childCount; e++)
        {
            Destroy(gemiKullanılmayanSecmeButtonParent.GetChild(e).gameObject);
        }
        LoadGemiler();
        // Kullanılan Gemiler
        for (int e = 0; e < gemiDatalar.kullanılanGemiler.Count; e++)
        {
            GemiSourceButton but = Instantiate(gemiSourceButton, gemiKullanılanSecmeButtonParent);
            but.SetSource(this, gemiDatalar.kullanılanGemiler[e]);
            but.GetComponentInChildren<TextMeshProUGUI>().text = gemiDatalar.kullanılanGemiler[e].gemiName;
            but.name = gemiDatalar.kullanılanGemiler[e].gemiName;
        }
        // Kullanılmayan Gemiler
        for (int e = 0; e < gemiDatalar.kullanılmayanGemiler.Count; e++)
        {
            GemiSourceButton but = Instantiate(gemiSourceButton, gemiKullanılmayanSecmeButtonParent);
            but.SetSource(this, gemiDatalar.kullanılmayanGemiler[e]);
            but.GetComponentInChildren<TextMeshProUGUI>().text = gemiDatalar.kullanılmayanGemiler[e].gemiName;
            but.name = gemiDatalar.kullanılmayanGemiler[e].gemiName;
        }
    }
    private void SaveGemiler()
    {
        SaveManager.SavePlayer(gemiDatalar);
    }
    private void LoadGemiler()
    {
        gemiDatalar = SaveManager.LoadPlayer();
    }
    public void GemiSec(GemiSource gemiSource, GameObject secili)
    {
        gemiKullanılan.isOn = gemiSource.isGemiKullanılan;
        seciliGemi = gemiSource;
        seciliButon = secili;
        if (gemiSource.isGemiKullanılan)
        {
            gemiKullanılanText.text = "Kullanılan Gemi";
        }
        else
        {
            gemiKullanılanText.text = "Kullanılmayan Gemi";
        }
        gemiName.text = seciliGemi.gemiName;
        gemiBuyukluk.text = seciliGemi.gemiBuyukluk.ToString();
        gemiGold.text = seciliGemi.gemiGold.ToString();
        gemiOdun.text = seciliGemi.gemiOdun.ToString();
        gemiKaynak.text = seciliGemi.gemiKaynak.ToString();
        gemiKaynakType.value = (int)seciliGemi.gemiKaynakType;
        gemiLimit.text = seciliGemi.gemiDepo[0].ToString();
        gemiLimitNext.text = seciliGemi.gemiDepo[1].ToString();
        gemiLimitArtis.text = seciliGemi.gemiDepo[2].ToString();
    }
    public void GemiEkle()
    {
        seciliGemi = new GemiSource();
        seciliGemi.gemiName = gemiName.text;
        int deger = 0;
        if (int.TryParse(gemiBuyukluk.text, out deger))
        {
            seciliGemi.gemiBuyukluk = deger;
        }
        else
        {
            gemiJob.UyariPanel("Gemi Büyüklükk için lütfen rakam giriniz.");
        }
        if (int.TryParse(gemiGold.text, out deger))
        {
            seciliGemi.gemiGold = deger;
        }
        else
        {
            gemiJob.UyariPanel("Gemi Gold için lütfen rakam giriniz.");
        }
        if (int.TryParse(gemiOdun.text, out deger))
        {
            seciliGemi.gemiOdun = deger;
        }
        else
        {
            gemiJob.UyariPanel("Gemi Odun için lütfen rakam giriniz.");
        }
        if (int.TryParse(gemiKaynak.text, out deger))
        {
            seciliGemi.gemiKaynak = deger;
        }
        else
        {
            gemiJob.UyariPanel("Gemi Kaynak için lütfen rakam giriniz.");
        }
        seciliGemi.gemiKaynakType = (KaynakType)gemiKaynakType.value;
        if (int.TryParse(gemiLimit.text, out deger))
        {
            seciliGemi.gemiDepo[0] = deger;
        }
        else
        {
            gemiJob.UyariPanel("Gemi Limit için lütfen rakam giriniz.");
        }
        if (int.TryParse(gemiLimitNext.text, out deger))
        {
            seciliGemi.gemiDepo[1] = deger;
        }
        else
        {
            gemiJob.UyariPanel("Gemi Limit Next için lütfen rakam giriniz.");
        }
        if (seciliGemi.gemiDepo[1] - seciliGemi.gemiDepo[0] > 0)
        {
            seciliGemi.gemiDepo[2] = seciliGemi.gemiDepo[1] - seciliGemi.gemiDepo[0];
        }
        else
        {
            seciliGemi.gemiDepo[2] = 0;
        }
        seciliGemi.isGemiKullanılan = gemiKullanılan.isOn;
        GemiSourceButton but;
        if (gemiKullanılan.isOn)
        {
            gemiDatalar.kullanılanGemiler.Add(seciliGemi);
            but = Instantiate(gemiSourceButton, gemiKullanılanSecmeButtonParent);
        }
        else
        {
            gemiDatalar.kullanılmayanGemiler.Add(seciliGemi);
            but = Instantiate(gemiSourceButton, gemiKullanılmayanSecmeButtonParent);
        }
        but.SetSource(this, seciliGemi);
        but.GetComponentInChildren<TextMeshProUGUI>().text = seciliGemi.gemiName;
        but.name = seciliGemi.gemiName;
        InputlarReset();
    }
    public void GemiSil()
    {
        Destroy(seciliButon);
        if (seciliGemi.isGemiKullanılan)
        {
            gemiDatalar.kullanılanGemiler.Remove(seciliGemi);
        }
        else
        {
            gemiDatalar.kullanılmayanGemiler.Remove(seciliGemi);
        }
        InputlarReset();
    }
    public void GemiSırala()
    {
        for (int e = 0; e < gemiKullanılanSecmeButtonParent.childCount; e++)
        {
            Destroy(gemiKullanılanSecmeButtonParent.GetChild(e).gameObject);
        }
        gemiDatalar.kullanılanGemiler.Sort(SortGold);
        for (int e = 0; e < gemiDatalar.kullanılanGemiler.Count; e++)
        {
            GemiSourceButton but = Instantiate(gemiSourceButton, gemiKullanılanSecmeButtonParent);
            but.SetSource(this, gemiDatalar.kullanılanGemiler[e]);
            but.GetComponentInChildren<TextMeshProUGUI>().text = gemiDatalar.kullanılanGemiler[e].gemiName;
            but.name = gemiDatalar.kullanılanGemiler[e].gemiName;
        }
        InputlarReset();
    }
    public void GemiKullanılıyormu(bool isKullanılan)
    {
        if (isKullanılan)
        {
            gemiKullanılanText.text = "Kullanılan Gemi";
        }
        else
        {
            gemiKullanılanText.text = "Kullanılmayan Gemi";
        }
    }
    public void LimitArtisKucuk(string kucukRakam)
    {
        if (int.TryParse(kucukRakam, out int limitKucuk))
        {
            if (int.TryParse(gemiLimitNext.text, out int limitBuyuk))
            {
                if (limitBuyuk - limitKucuk > 0)
                {
                    gemiLimitArtis.text = (limitBuyuk - limitKucuk).ToString();
                }
                else
                {
                    gemiLimitArtis.text = "Error";
                }
            }
        }
        else
        {
            string dogruYazi = kucukRakam;
            dogruYazi = dogruYazi.Substring(0, dogruYazi.Length - 1);
            if (int.TryParse(dogruYazi, out int yeni))
            {
                gemiLimit.text = dogruYazi;
            }
            else
            {
                return;
            }
        }
    }
    public void LimitArtisBuyuk(string buyukRakam)
    {
        if (int.TryParse(buyukRakam, out int limitBuyuk))
        {
            if (int.TryParse(gemiLimit.text, out int limitKucuk))
            {
                if (limitBuyuk - limitKucuk > 0)
                {
                    gemiLimitArtis.text = (limitBuyuk - limitKucuk).ToString();
                }
                else
                {
                    gemiLimitArtis.text = "Error";
                }
            }
        }
        else
        {
            string dogruYazi = buyukRakam;
            dogruYazi = dogruYazi.Substring(0, dogruYazi.Length - 1);
            if (int.TryParse(dogruYazi, out int yeni))
            {
                gemiLimitNext.text = dogruYazi;
            }
            else
            {
                return;
            }
        }
    }
    public void ExitGemiler()
    {
        Application.Quit();
    }
    private void InputlarReset()
    {
        gemiKullanılan.isOn = true;
        gemiKullanılanText.text = "Kullanılan Gemi";
        gemiName.text = "Gemi Name";
        gemiBuyukluk.text = "Gemi Buyukluk";
        gemiGold.text = "Gemi Gold";
        gemiOdun.text = "Gemi Odun";
        gemiKaynak.text = "Gemi Kaynak";
        gemiKaynakType.value = 0;
        gemiLimit.text = "Gemi Limit";
        gemiLimitNext.text = "Gemi Limit Next";
        gemiLimitArtis.text = "Gemi Limit Artis";
        seciliGemi = null;
        seciliButon = null;
        SaveGemiler();
    }
}