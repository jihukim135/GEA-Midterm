using UnityEngine;
using System.Collections;

public class InfiniteJumpItem : Item
{
    protected override void Start()
    {
        base.Start();
        playerColorChanged = new Color(1f, 0f, 1f, 1f);
    }

    protected override IEnumerator ApplyItemEffect()
    {
        player.IsJumpInfinite = true;

        Color color = playerColorChanged;
        playerRenderer.color = color;

        while (playerRenderer.color.g < 1f)
        {
            color.g += Time.deltaTime / duration;
            playerRenderer.color = color;

            yield return null;
        }

        player.IsJumpInfinite = false;
    }
}