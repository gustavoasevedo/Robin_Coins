using System;
using UnityEngine;
public class CoinItem : MonoBehaviour
{

    internal CoinInterface coinInterface;
    [SerializeField] Sprite coinSprite;

    internal void initCoinSprite()
    {
        GetComponent<SpriteRenderer>().sprite = coinSprite;
    }

    internal void initCoinInterface(CoinInterface coinInterface)
    {
        this.coinInterface = coinInterface;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        coinInterface.onCoinObtained(this);
    }
}
