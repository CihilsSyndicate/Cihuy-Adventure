using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinCounter : MonoBehaviour
{
    public static CoinCounter instance;

    public Text[] textCoin;
    public int currentCoin;

    public static CoinCounter Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        // Inisialisasi instance singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // Hancurkan objek jika instance sudah ada
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        currentCoin = PlayerPrefs.GetInt("Coins");
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < textCoin.Length; i++)
        {
            textCoin[i].text = currentCoin.ToString();
        }
    }

    public void IncreaseCoin(int value)
    {
        currentCoin += value;
        PlayerPrefs.SetInt("Coins", currentCoin);
    }

    public void DecreaseCoin(int value)
    {
        currentCoin -= value;
        PlayerPrefs.SetInt("Coins", currentCoin);
    }
}
