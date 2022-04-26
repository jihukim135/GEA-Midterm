using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public abstract class Item : MonoBehaviour
{
    private ItemsInfo _itemsInfo;
    private SpriteRenderer _itemRenderer;

    private int index;
    protected GameObject description;
    protected float duration = 10f;

    protected PlayerController player;
    protected SpriteRenderer playerRenderer;

    protected Color playerColorChanged;

    protected virtual void Start()
    {
        _itemRenderer = GetComponent<SpriteRenderer>();
        _itemsInfo = ItemsInfo.Instance;

        index = _itemsInfo.GetItemClassIndex(GetType().Name);
        description = _itemsInfo.Descriptions[index];
        description.SetActive(false);

        GameObject playerObject = GameObject.FindWithTag("Player");
        player = playerObject.GetComponent<PlayerController>();
        playerRenderer = playerObject.GetComponent<SpriteRenderer>();
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (!_itemRenderer.enabled || !col.CompareTag("Player"))
        {
            return;
        }

        _itemRenderer.enabled = false;

        StartCoroutine(ConsumeAndSetDescription());
    }

    private IEnumerator ConsumeAndSetDescription()
    {
        description.SetActive(true);

        yield return StartCoroutine(ApplyItemEffect());

        description.SetActive(false);
    }

    protected abstract IEnumerator ApplyItemEffect();
}