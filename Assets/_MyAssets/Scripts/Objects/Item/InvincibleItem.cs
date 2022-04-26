using System;
using UnityEngine;
using System.Collections;

public class InvincibleItem : Item
{
    protected override void Start()
    {
        base.Start();
        playerColorChanged = new Color(0f, 1f, 1f, 1f);
    }

    protected override IEnumerator ApplyItemEffect()
    {
        player.IsInvincible = true;

        Color color = playerColorChanged;
        playerRenderer.color = playerColorChanged;

        while (playerRenderer.color.r < 1f)
        {
            color.r += Time.deltaTime / duration;
            playerRenderer.color = color;

            yield return null;
        }

        player.IsInvincible = false;
    }
}