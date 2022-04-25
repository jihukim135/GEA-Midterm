using UnityEngine;
using System.Collections;

public class InfiniteJumpItem : Item
{
    protected override void Start()
    {
        index = 1;
        base.Start();
        playerColorChanged = new Color(1f, 0f, 1f, 1f);
    }

    protected override IEnumerator Activate()
    {
        int originalMax = 2;
        player.MaxJumpCount = int.MaxValue;

        Color color = playerColorChanged;
        playerRenderer.color = color;

        while (playerRenderer.color.g < 1f)
        {
            color.g += Time.deltaTime / duration;
            playerRenderer.color = color;

            yield return null;
        }

        player.MaxJumpCount = originalMax;
        
        description.SetActive(false);
        isActivated = false;
    }
}