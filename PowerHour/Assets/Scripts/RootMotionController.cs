using UnityEngine;
using Polarith.AI.Move;
public class RootMotionController : MonoBehaviour
{

    public Animator animator;
    public AIMContext context;

    [TargetObjective(true)]
    public int ObjectiveAsSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        if (context == null)
        {
            context = GetComponent<AIMContext>();
        }

        animator.applyRootMotion = true;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetDirection = context.DecidedDirection;
        float step = 2.0f * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDirection, step, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);

        animator.SetBool("isWalking", true);
    }
}
