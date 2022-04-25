using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public abstract class Item : MonoBehaviour
{
    private ItemsInfo itemsInfo;
    protected int index;
    private SpriteRenderer _itemSprite;
    protected GameObject description;

    protected bool isActivated = false;
    protected float duration = 10f;

    protected PlayerController player;
    protected SpriteRenderer playerRenderer;

    protected Color playerColorChanged;

    protected virtual void Start()
    {
        _itemSprite = GetComponent<SpriteRenderer>();
        itemsInfo = ItemsInfo.Instance;

        description = itemsInfo.Descriptions[index];
        description.SetActive(false);

        GameObject playerObject = GameObject.FindWithTag("Player");
        player = playerObject.GetComponent<PlayerController>();
        playerRenderer = playerObject.GetComponent<SpriteRenderer>();
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (!_itemSprite.enabled || !col.CompareTag("Player"))
        {
            return;
        }

        _itemSprite.enabled = false;

        description.SetActive(true);
        isActivated = true;

        StartCoroutine(Activate());
    }

    protected abstract IEnumerator Activate();
}