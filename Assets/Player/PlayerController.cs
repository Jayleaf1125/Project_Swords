using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Ground Settings")]
    [SerializeField] LayerMask _groundLayer;

    public Rigidbody2D Rb { get; private set; }
    public bool IsGrounded { get; set; }

    [Header("State Machine")]
    public StateMachine<PlayerController> StateMachine { get; private set; }
    public GroundState GroundState { get; private set; } = new();
    public AirState AirState { get; private set; } = new();

    [HideInInspector] public string CurrentState;

    private void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        SetupStateMachine();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StateMachine.Update();
    }

    private void FixedUpdate()
    {
        CheckGrounded();
        StateMachine.FixedUpdate();
    }

    void SetupStateMachine()
    {
        StateMachine = new StateMachine<PlayerController>(this);
        GroundState.Init(StateMachine, this);
        AirState.Init(StateMachine, this);

        StateMachine.ChangeState(GroundState, true);
    }

    void CheckGrounded()
    {
        const float HIT_OFFSET = 0.5f;
        const float RAY_DIST = 0.75f;

        RaycastHit2D midHit = Physics2D.Raycast(transform.position, Vector2.down, RAY_DIST, _groundLayer);
        RaycastHit2D leftHit = Physics2D.Raycast(new Vector2(transform.position.x - HIT_OFFSET, transform.position.y), Vector2.down, RAY_DIST, _groundLayer);
        RaycastHit2D rightHit = Physics2D.Raycast(new Vector2(transform.position.x + HIT_OFFSET, transform.position.y), Vector2.down, RAY_DIST, _groundLayer);

        Debug.DrawRay(transform.position, Vector2.down * RAY_DIST, Color.red);
        Debug.DrawRay(new Vector2(transform.position.x - HIT_OFFSET, transform.position.y), Vector2.down * RAY_DIST, Color.red);
        Debug.DrawRay(new Vector2(transform.position.x + HIT_OFFSET, transform.position.y), Vector2.down * RAY_DIST, Color.red);

        IsGrounded = (leftHit.collider != null) || (midHit.collider != null) || (rightHit.collider != null);
    }

    private void OnDrawGizmos()
    {
        #if UNITY_EDITOR
                if (Application.isPlaying)
                {
                    Gizmos.color = IsGrounded ? Color.green : Color.red;

                    GUIStyle style = new GUIStyle();
                    style.alignment = TextAnchor.MiddleCenter;
                    style.normal.textColor = Color.red;
                    style.fontSize = 40;
                    Handles.Label(transform.position + Vector3.up,
                        StateMachine.CurrentState.GetType().Name + ">" + CurrentState, style);
                }
        #endif
    }

}
