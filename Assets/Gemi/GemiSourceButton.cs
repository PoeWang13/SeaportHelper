using UnityEngine;

public class GemiSourceButton : MonoBehaviour
{
    // Script Atamaları
    private Gemiler gemiler;
    private GemiSource seciliGemi;

    public void SetSource(Gemiler gemi, GemiSource secili)
    {
        gemiler = gemi;
        seciliGemi = secili;
    }
    public void GemiSec()
    {
        gemiler.GemiSec(seciliGemi, gameObject);
    }
}