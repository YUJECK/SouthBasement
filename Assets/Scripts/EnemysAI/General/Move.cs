using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Pathfinding))]
[RequireComponent(typeof(Rigidbody2D))]

public class Move : MonoBehaviour
{
    [Header("��������� ������������")]
    [SerializeField] private Movement movement = Movement.Wandering;
    [SerializeField] private float _speed = 3; // �������� ������������ 
    [SerializeField] private float searchRate = 1f; // ������� �������� 
    private float nextSearchTime = 0f;
    public float speed
    {
        get { return _speed; }
        set
        {
            if (value <= 0f)
            {
                _speed = 1;
                return;
            }
            if (value > 8f)
            {
                _speed = 8;
                return;
            }
            _speed = value;
        }
    }
    [HideInInspector] public bool isNowWalk; //���� �� ������ 
    private bool isStopped = false; //���������� ��
    private List<Vector2> path = new List<Vector2>(); //����
    private EnemyTarget target; //������

    [Header("�������")]
    public UnityEvent onFlip = new UnityEvent();
    public UnityEvent onArrive = new UnityEvent();

    //���������� ��� ��������
    private bool flippedOnRight;
    private Vector2 lastPos;

    //������ �� ������ �������
    [Header("������")]
    [SerializeField] private TriggerCheker stopCheker;
    [SerializeField] private TargetSelection targetSelection;
    private Pathfinding pathfinding;
    private Grid grid;
    private Rigidbody2D rb;

    //������ ������ ����
    public void FindNewPath(EnemyTarget target)
    {
        if (path.Count != 0) ResetTarget();
        this.target = target;
        path = pathfinding.FindPath(
           new Vector2(transform.position.x / grid.nodeSize, transform.position.y / grid.nodeSize),
           new Vector2(target.transform.position.x / grid.nodeSize, target.transform.position.y / grid.nodeSize));
    }
    public void ResetTarget(EnemyTarget target = null)
    {
        if (target != null && target == this.target) { this.target = null; }
        path.Clear();
        pathfinding.ResetGridChanges();
    }
    private void SetNextSearchTime() { nextSearchTime = Time.time + searchRate; }

    //���� ������� � �������
    public void SetStop(bool active) { isStopped = active; }
    public bool GetStop() { return isStopped; }

    //����� ��������    
    private void FlipThisObject() { if (!isStopped) { transform.Rotate(0f, 180f, 0f); onFlip.Invoke(); } }
    public void FlipOther(Transform _transform) { _transform.Rotate(180f, 0f, 0f); }

    //���������� ������
    private void Start()
    {
        pathfinding = GetComponent<Pathfinding>();
        rb = GetComponent<Rigidbody2D>();
        grid = FindObjectOfType<Grid>();
        targetSelection.onTargetChange.AddListener(FindNewPath);
        targetSelection.onResetTarget.AddListener(ResetTarget);
        if (movement == Movement.Wandering) onArrive.AddListener(targetSelection.RefindTarget);
        isStopped = false;
    }
    private void FixedUpdate() //���������� ������
    {
        //����� velocity
        if (rb != null) rb.velocity = Vector2.zero;
        if (!isStopped && pathfinding.grid.isGridCreated)
        {
            //���������� ����� ����
            if (target != null && target.targetMoveType == TargetType.Movable && (Time.time >= nextSearchTime || path.Count == 0))
            {
                ResetTarget();
                FindNewPath(target);
                SetNextSearchTime();
            }

            //�������
            if (new Vector3(lastPos.x, lastPos.y, transform.position.z) != transform.position && isNowWalk)
            {
                if (lastPos.x < transform.position.x && flippedOnRight)
                {
                    FlipThisObject();
                    flippedOnRight = false;
                }
                else if (lastPos.x > transform.position.x && !flippedOnRight)
                {
                    FlipThisObject();
                    flippedOnRight = true;
                }
            }
            lastPos = transform.position;
            //�������� 
            if (path.Count != 0)
            {
                transform.position = Vector2.MoveTowards(transform.position, path[0], speed * Time.deltaTime);
                isNowWalk = true;

                if (transform.position == new Vector3(path[0].x, path[0].y, transform.position.z))
                {
                    path.RemoveAt(0);
                    //������� ��������� ����
                    if (pathfinding.gridChanges.Count != 0)
                    {
                        grid.grid[pathfinding.gridChanges[0].x, pathfinding.gridChanges[0].y] = 0;
                        pathfinding.gridChanges.RemoveAt(0);
                    }
                }
            }
            else if (path.Count == 0)
            {
                if (target != null && target.targetMoveType == TargetType.Static)
                {
                    ResetTarget();
                    onArrive.Invoke();
                }

                isNowWalk = false;
            }
        }
    }
}

public enum Movement
{
    Wandering,
    StaticPursuit
}