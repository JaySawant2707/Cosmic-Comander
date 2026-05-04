using System.Collections;
using UnityEngine;

public class AstraBoss : MonoBehaviour
{
    public enum BossState { Intro, Attack, Stunned, Dead }

    [Header("State")]
    public BossState currentState;

    [Header("Timings")]
    [SerializeField] private float introDuration = 2f;
    [SerializeField] private float stunDuration = 4f;

    [Header("References")]
    [SerializeField] private VerticalLaserSystem verticalLaser;
    [SerializeField] private HorizontalLaser horizontalLaser;
    [SerializeField] private GameObject sideBlockers;
    [SerializeField] private GameObject weakPoint;
    [SerializeField] private VerticalDoor[] doors;
    [SerializeField] private GameObject laserParent;
    [SerializeField] private SideBlockerController sideBlockerController;

    private bool isInvincible = true;

    public void StartBossFight()
    {
        verticalLaser.Init(laserParent.transform);
        horizontalLaser.Init(laserParent.transform);
        StartCoroutine(Intro());
    }

    IEnumerator Intro()
    {
        currentState = BossState.Intro;

        // TODO: Play spawn animation, sound effects, etc
        OpenDoors(false);

        yield return new WaitForSeconds(introDuration);

        StartCoroutine(AttackPhase());
    }

    IEnumerator AttackPhase()
    {
        currentState = BossState.Attack;
        isInvincible = true;

        sideBlockers.SetActive(true);
        yield return sideBlockerController.ActivateWithWarning();
        weakPoint.SetActive(false);

        yield return verticalLaser.RunSweep();

        yield return new WaitForSeconds(0.5f);

        yield return horizontalLaser.Fire();

        yield return new WaitForSeconds(0.5f);

        yield return verticalLaser.RunSweep();

        StartCoroutine(StunnedPhase());
    }

    IEnumerator StunnedPhase()
    {
        currentState = BossState.Stunned;
        isInvincible = false;

        sideBlockerController.Deactivate();
        weakPoint.SetActive(true);

        yield return new WaitForSeconds(stunDuration);

        // BUG 3 FIX: Don't restart the attack loop if the boss was killed during stun window
        if (currentState != BossState.Dead)
        {
            StartCoroutine(AttackPhase());
        }
    }

    public bool CanTakeDamage()
    {
        return !isInvincible;
    }

    public void OpenDoors(bool state)
    {
        foreach (var door in doors)
        {
            if (state)
                door.OpenDoor();
            else
                door.CloseDoor();
        }
    }

    // BUG 3 FIX: Called by BossWeakPoint when health reaches 0
    // Stops the state machine immediately so StunnedPhase can't re-enter AttackPhase
    public void OnBossDefeated()
    {
        currentState = BossState.Dead;
        StopAllCoroutines();

        // BUG 2 FIX: Also stop coroutines on laser subsystems so Blink() doesn't
        // crash trying to access GameObjects that were already destroyed
        verticalLaser.StopAllCoroutines();
        horizontalLaser.StopAllCoroutines();
    }

    public void ResetEverythingAndDisable()
    {
        StopAllCoroutines();
        currentState = BossState.Intro;
        isInvincible = true;
        sideBlockers.SetActive(false);
        weakPoint.SetActive(false);
        OpenDoors(true);

        // BUG 2 FIX: Stop coroutines on laser systems before destroying their objects
        verticalLaser.StopAllCoroutines();
        horizontalLaser.StopAllCoroutines();

        // BUG 1 FIX: GetComponentsInChildren includes the parent itself, which would
        // destroy laserParent and cause a NullReferenceException on the next spawn.
        // Iterate direct children only using the Transform enumerator instead.
        foreach (Transform child in laserParent.transform)
        {
            Destroy(child.gameObject);
        }

        this.gameObject.SetActive(false);
    }
}