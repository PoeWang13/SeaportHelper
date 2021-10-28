using TMPro;
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GerekenGemiDurumlar
{
    public int fazlalık;
    public List<int> gerekenGemiKargolar = new List<int>();

    public GerekenGemiDurumlar(int fazla, List<int> gerekenler)
    {
        fazlalık = fazla;
        for (int e = 0; e < gerekenler.Count; e++)
        {
            gerekenGemiKargolar.Add(gerekenler[e]);
        }
    }
}
public class GemiKargoHesaplama : MonoBehaviour
{
    [SerializeField] private GemiJob gemiJob;
    [Header("Gemi Atamaları")]
    [SerializeField] private GameObject gemiKargoObject;
    [SerializeField] private Transform gemiKargoParent;
    [SerializeField] private TMP_InputField gemiKargoInput;
    public PlayerData gemiDatalar = new PlayerData();
    public void GemilerHazirlik()
    {
        for (int e = 0; e < gemiKargoParent.childCount; e++)
        {
            Destroy(gemiKargoParent.GetChild(e).gameObject);
        }
        LoadGemiler();
        for (int e = 0; e < gemiDatalar.kullanılanKargolar.Count; e++)
        {
            GameObject but = Instantiate(gemiKargoObject, gemiKargoParent);
            but.GetComponentInChildren<TextMeshProUGUI>().text = gemiDatalar.kullanılanKargolar[e].ToString();
            but.name = gemiDatalar.kullanılanKargolar[e].ToString();
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
    public void GemiSirala()
    {
        gemiDatalar.kullanılanKargolar.Sort(KargoListSort);
        for (int e = 0; e < gemiKargoParent.childCount; e++)
        {
            Destroy(gemiKargoParent.GetChild(e).gameObject);
        }
        for (int e = 0; e < gemiDatalar.kullanılanKargolar.Count; e++)
        {
            GameObject but = Instantiate(gemiKargoObject, gemiKargoParent);
            but.GetComponentInChildren<TextMeshProUGUI>().text = gemiDatalar.kullanılanKargolar[e].ToString();
            but.name = gemiDatalar.kullanılanKargolar[e].ToString();
        }
        SaveGemiler();
    }
    public void GemiEkle()
    {
        if (int.TryParse(gemiKargoInput.text, out int kargo))
        {
            gemiDatalar.kullanılanKargolar.Add(kargo);
            GemiSirala();
        }
        else
        {
            gemiJob.UyariPanel("Lütfen rakam giriniz.");
        }
    }
    public void GemiSil()
    {
        if (int.TryParse(gemiKargoInput.text, out int kargo))
        {
            gemiDatalar.kullanılanKargolar.Remove(kargo);
            GemiSirala();
            SaveGemiler();
        }
        else
        {
            gemiJob.UyariPanel("Lütfen rakam giriniz.");
        }
    }
    private int KargoListSort(int kucuk, int buyuk)
    {
        if (buyuk < kucuk)
        {
            return -1;
        }
        else if (buyuk > kucuk)
        {
            return 1;
        }
        return 0;
    }
    [Header("Gereken Gemi Atamaları")]
    [SerializeField] private Transform bulunanKargoParent;
    [SerializeField] private TMP_InputField istenenMalMiktar;
    [SerializeField] private TMP_InputField malOran;
    [SerializeField] private List<GerekenGemiDurumlar> bulunanGemiKargolar = new List<GerekenGemiDurumlar>();
    private int kargoMiktarı;
    private int devamYeri = 0;
    [SerializeField] private List<int> bulunanGemiler = new List<int>();
    [SerializeField] private List<int> bulunanGemilerKaynak = new List<int>();
    [SerializeField] private int gemiKa;
    private int BulunanKargoListSort(GerekenGemiDurumlar kucuk, GerekenGemiDurumlar buyuk)
    {
        if (buyuk.fazlalık > kucuk.fazlalık)
        {
            return -1;
        }
        else if (buyuk.fazlalık < kucuk.fazlalık)
        {
            return 1;
        }
        return 0;
    }
    public void GerekenGemileriBul()
    {
        bulunanGemiler.Clear();
        bulunanGemilerKaynak.Clear();
        bulunanGemiKargolar.Clear();
        // Gereken Kargo Miktarını ogren
        if (int.TryParse(istenenMalMiktar.text, out int istenen))
        {
            if (int.TryParse(malOran.text, out int oran))
            {
                kargoMiktarı = Mathf.CeilToInt(istenen * 1.0f / oran);
            }
            else
            {
                gemiJob.UyariPanel("Lütfen rakam giriniz.");
            }
        }
        else
        {
            gemiJob.UyariPanel("Lütfen rakam giriniz.");
        }
        // Gidecek Gemileri Ogren
        Kargolar();

        // Karsilastirma Yap
        bulunanGemiKargolar.Sort(BulunanKargoListSort);

        // Gemileri listele
        KargolarListele();
    }
    private void Kargolar()
    {
        bulunanGemilerKaynak.Clear();
        int toplamBulunma = 0;
        int[] silinecekler;
        while (toplamBulunma < 10)
        {
            //Debug.Log("Toplam Bulunma---" + toplamBulunma);
            if (toplamBulunma == 0)
            {
                bool yukTamam = false;
                int toplamYuk = 0;
                for (int e = 0; e < gemiDatalar.kullanılanKargolar.Count && !yukTamam; e++)
                {
                    toplamYuk += gemiDatalar.kullanılanKargolar[e];
                    bulunanGemiler.Add(gemiDatalar.kullanılanKargolar[e]);
                    if (toplamYuk > kargoMiktarı)
                    {
                        devamYeri = e + 1;
                        yukTamam = true;
                        bulunanGemiKargolar.Add(new GerekenGemiDurumlar(toplamYuk - kargoMiktarı, bulunanGemiler));
                        if (toplamYuk - kargoMiktarı <= 50) // Duzgun kargo list bulundu. Devam etmeye gerek yok
                        {
                            toplamBulunma = 10;
                        }
                    }
                }
                for (int e = 0; e < bulunanGemiler.Count; e++)
                {
                    bulunanGemilerKaynak.Add(bulunanGemiler[e]);
                }
            }
            else
            {
                if (toplamBulunma == 1)
                {
                    /// son
                    silinecekler = new int[1] { bulunanGemiler[bulunanGemiler.Count - 1] };
                }
                else if (toplamBulunma == 2)
                {
                    /// bas
                    silinecekler = new int[1] { bulunanGemiler[0] };
                }
                else if (toplamBulunma == 3)
                {
                    /// son - bas
                    silinecekler = new int[2] { bulunanGemiler[bulunanGemiler.Count - 1], bulunanGemiler[0] };
                }
                else if (toplamBulunma == 4)
                {
                    /// son - son
                    silinecekler = new int[2] { bulunanGemiler[bulunanGemiler.Count - 1],
                    bulunanGemiler[bulunanGemiler.Count - 2] };
                }
                else if (toplamBulunma == 5)
                {
                    /// son - son - bas
                    silinecekler = new int[3] { bulunanGemiler[bulunanGemiler.Count - 1],
                    bulunanGemiler[bulunanGemiler.Count - 2], bulunanGemiler[0] };
                }
                else if (toplamBulunma == 6)
                {
                    /// son - bas - bas
                    silinecekler = new int[3] { bulunanGemiler[bulunanGemiler.Count - 1],
                    bulunanGemiler[0], bulunanGemiler[1] };
                }
                else if (toplamBulunma == 7)
                {
                    /// son - son - son
                    silinecekler = new int[3] { bulunanGemiler[bulunanGemiler.Count - 1],
                    bulunanGemiler[bulunanGemiler.Count - 2], bulunanGemiler[bulunanGemiler.Count - 3] };
                }
                else if (toplamBulunma == 8)
                {
                    /// son - son - bas - bas
                    silinecekler = new int[4] { bulunanGemiler[bulunanGemiler.Count - 1],
                    bulunanGemiler[bulunanGemiler.Count - 2], bulunanGemiler[0], bulunanGemiler[1] };
                }
                else
                {
                    /// son - son - son - bas
                    silinecekler = new int[4] { bulunanGemiler[bulunanGemiler.Count - 1],
                    bulunanGemiler[bulunanGemiler.Count - 2], bulunanGemiler[bulunanGemiler.Count - 3],
                    bulunanGemiler[0] };
                }
                if (KargoDuzenle(silinecekler) == 10)
                {
                    toplamBulunma = 10;
                }
            }
            toplamBulunma++;
        }
    }
    private int KargoDuzenle(params int[] silinecekler)
    {
        if (bulunanGemiler.Count >= silinecekler.Length)
        {
            //Debug.Log("Bulunan Gemi Count : " + bulunanGemiler.Count);
            for (int e = 0; e < silinecekler.Length; e++)
            {
                bool silindi = false;
                for (int h = 0; h < bulunanGemiler.Count && !silindi; h++)
                {
                    if (silinecekler[e] == bulunanGemiler[h])
                    {
                        bulunanGemiler.Remove(silinecekler[e]);
                        silindi = true;
                    }
                }
            }
            if (GemiKargo())
            {
                return 10;
            }
            bulunanGemiler.Clear();
            for (int e = 0; e < bulunanGemilerKaynak.Count; e++)
            {
                bulunanGemiler.Add(bulunanGemilerKaynak[e]);
            }
            return 0;
        }
        else
        {
            return 10;
        }
    }
    private bool GemiKargo()
    {
        int toplamYuk = 0;
        bool yukTamam = false;
        for (int e = 0; e < bulunanGemiler.Count; e++)
        {
            toplamYuk += bulunanGemiler[e];
        }
        for (int e = devamYeri; e < gemiDatalar.kullanılanKargolar.Count && !yukTamam; e++)
        {
            toplamYuk += gemiDatalar.kullanılanKargolar[e];
            bulunanGemiler.Add(gemiDatalar.kullanılanKargolar[e]);
            if (toplamYuk > kargoMiktarı)
            {
                if (toplamYuk - kargoMiktarı <= 50) // Duzgun kargo list bulundu. Devam etmeye gerek yok
                {
                    yukTamam = true;
                    bulunanGemiKargolar.Add(new GerekenGemiDurumlar(toplamYuk - kargoMiktarı, bulunanGemiler));
                    return true;
                }
                else
                {
                    toplamYuk -= gemiDatalar.kullanılanKargolar[e];
                    bulunanGemiler.RemoveAt(bulunanGemiler.Count - 1);
                }
            }
        }
        return false;
    }
    private void KargolarListele()
    {
        //Debug.Log("KargolarListele");
        for (int e = 0; e < bulunanKargoParent.childCount; e++)
        {
            Destroy(bulunanKargoParent.GetChild(e).gameObject);
        }
        for (int e = 0; e < bulunanGemiKargolar[0].gerekenGemiKargolar.Count; e++)
        {
            GameObject but = Instantiate(gemiKargoObject, bulunanKargoParent);
            but.GetComponentInChildren<TextMeshProUGUI>().text = bulunanGemiKargolar[0].gerekenGemiKargolar[e].ToString();
            but.name = bulunanGemiKargolar[0].gerekenGemiKargolar[e].ToString();
        }
    }
}