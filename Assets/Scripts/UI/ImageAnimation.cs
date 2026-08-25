using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ImageAnimation : MonoBehaviour
{
    // Enum to represent all the posible animations
    public enum AnimationType { NoAnim, LostLimb_1, LostLimb_2, LostLimb_3, RecoverLimb_1, RecoverLimb_2, RecoverLimb_3 }
    public GameManager gm; // Reference to the game manager
    public Image animationContainer; // Reference to the image that will contain the animation
    public Sprite[] animationSprites; // List of all the animation sprites
    private float timePerFrame = 0.5f; // Time in seconds that each frame of animation takes
    private int[] lostLimb1_Index = new int[] { 0, 1, 2}; // Auxiliary arrays of indexes to play the corresponding animations
    private int[] lostLimb2_Index = new int[] { 2, 3, 4 };
    private int[] lostLimb3_Index = new int[] { 4, 5, 6 };
    private int[] recoverLimb1_Index = new int[] { 2, 1, 0 };
    private int[] recoverLimb2_Index = new int[] { 4, 3, 2 };
    private int[] recoverLimb3_Index = new int[] { 6, 5, 4 };
    private int[] currentAnimationIndex; // Array of indexes of the selected animation

    public void SetAnimation(AnimationType anim)
    {
        // Set the corresponding index array depending on the selected animation
        switch (anim)
        {
            case AnimationType.LostLimb_1:
                currentAnimationIndex = lostLimb1_Index;
                return;
            case AnimationType.LostLimb_2:
                currentAnimationIndex = lostLimb2_Index;
                return;
            case AnimationType.LostLimb_3:
                currentAnimationIndex = lostLimb3_Index;
                return;
            case AnimationType.RecoverLimb_1:
                currentAnimationIndex = recoverLimb1_Index;
                return;
            case AnimationType.RecoverLimb_2:
                currentAnimationIndex = recoverLimb2_Index;
                return;
            case AnimationType.RecoverLimb_3:
                currentAnimationIndex = recoverLimb3_Index;
                return;
            default:
                currentAnimationIndex = new int[3];
                return;
        }
    }

    /**
     * This methoid is called from the game manager when this animation is needed
     */
    public void PlayAnimation(bool lost)
    {
        // If the animation is the loosing a limb
        if (lost)
        {
            // Play the corresponding sound
            SoundManager.sm.PlaySoundOneShot(SoundManager.SoundTrack.Cross_Fail);
        }
        // Play the animation
        StartCoroutine(PlayAnim());
    }

    private IEnumerator PlayAnim()
    {
        // Set the wait variable
        WaitForSeconds wait = new WaitForSeconds(timePerFrame);
        // Loop through the animation indexes
        for (int i = 0; i < currentAnimationIndex.Length; i++)
        {
            // Set the animation container to the appropriate animation frame
            animationContainer.sprite = animationSprites[currentAnimationIndex[i]];
            // Wait the desired time
            yield return wait;
        }
        // Disable the animation panel
        gameObject.SetActive(false);
        // Signal the game manager to allow movement to the player
        gm.SetOnLimbAction(false);
    }
}
