using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class SarahPlayerBT : MonoBehaviour
{
    private CharacterController sarah;
    public Rigidbody ball;

    // ��Ϊ�����
    private BTNode behaviorTree;
    private bool isTreeInitialized = false;

    // ���������
    public PlayerInputController playerInput;
    private int playerSkillPressCount = 0;
    private float skillPressWindow = 3f; // 3������������
    private float lastSkillPressTime = 0f;
    private bool isSkillActive = false;
    private float skillDuration = 5f;
    private float skillTimer = 0f;

    // AI ״̬
    [System.Serializable]
    public class AIState
    {
        public string stateName;
        public Color stateColor;
        public float moveSpeed;
        public float predictionX;
        public float predictionZ;
        public float jumpHeight;
        public float jumpTime;
        public float jumpCondition;
    }

    [Header("AI States")]
    public AIState aggressiveState;
    public AIState defensiveState;
    public AIState skillState; // ����״̬
    private AIState currentState;

    [Header("Clamp Settings")]
    public float minX, maxX, minZ, maxZ;

    // ��������
    private Material aiMaterial;
    private bool isJumping = false;
    private bool offGround = false;

    // DIO ʱ�����
    private bool isInDioTime = false;
    private Dictionary<string, float> originalValues = new Dictionary<string, float>();

    private void Awake()
    {
        sarah = GetComponent<CharacterController>();
        aiMaterial = GetComponent<Renderer>().material;

        // ����������������
        playerInput = new PlayerInputController();
        
        if (playerInput == null)
        {
            Debug.LogWarning("PlayerInputController not found in scene!");
        }

        // ��ʼ��AI״̬
        InitializeAIStates();

        // ������Ϊ��
        BuildBehaviorTree();

        // �¼�����
        EventManage.AddListener("DioTimeStarted", StartDioTime);
        EventManage.AddListener("DioTimeEnded", EndDioTime);
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    private void InitializeAIStates()
    {
        // ��ʼ������״̬
        aggressiveState = new AIState
        {
            stateName = "Aggressive",
            stateColor = Color.red,
            moveSpeed = 8f,
            predictionX = 0.5f,
            predictionZ = 1.2f,
            jumpHeight = 7f,
            jumpTime = 7f,
            jumpCondition = 2.5f
        };

        // ��ʼ������״̬
        defensiveState = new AIState
        {
            stateName = "Defensive",
            stateColor = Color.blue,
            moveSpeed = 5f,
            predictionX = 1.0f,
            predictionZ = 0.8f,
            jumpHeight = 2f,
            jumpTime = 2f,
            jumpCondition = 2.5f
        };

        // ��ʼ������״̬
        skillState = new AIState
        {
            stateName = "Skill",
            stateColor = Color.green,
            moveSpeed = 12f, // ����״̬�¸���
            predictionX = 0.2f,
            predictionZ = 1.5f,
            jumpHeight = 10f, // ����״̬�����ø���
            jumpTime = 5f,
            jumpCondition = 1.5f
        };

        // ���ó�ʼ״̬
        currentState = aggressiveState;
    }

    private void BuildBehaviorTree()
    {
        // �����򻯵���Ϊ���ṹ
        behaviorTree = new Selector(new List<BTNode> {
            // ���ȼ�1: ������״̬
            new Sequence(new List<BTNode> {
                new Condition(() => isSkillActive),
                new ActionNode(ExecuteSkillBehavior)
            }),
            
            // ���ȼ�2: ����DIOʱ��
            new Sequence(new List<BTNode> {
                new Condition(() => isInDioTime),
                new ActionNode(HandleDioTimeBehavior)
            }),
            
            // ���ȼ�3: ִ�е�ǰ״̬��Ϊ
            new Sequence(new List<BTNode> {
                new Condition(() => true), // ����ִ��
                new ActionNode(ExecuteCurrentStateBehavior)
            })
        });

        isTreeInitialized = true;
    }

    private void Update()
    {
        if (!isTreeInitialized) return;

        // �����Ҽ�������
        DetectPlayerSkillInput();

        // ���¼��ܼ�ʱ��
        if (isSkillActive)
        {
            skillTimer -= Time.deltaTime;
            if (skillTimer <= 0f)
            {
                EndSkill();
            }
        }

        // ִ����Ϊ��
        behaviorTree.Execute();
    }

    private void DetectPlayerSkillInput()
    {
        if (playerInput == null) return;

        // �������Ƿ����˼��ܼ�
        // ������Ҫ�������PlayerInputControllerʵ��ʵ��������
        bool skillPressed = false;

        // ����1: ���PlayerInputController�й������¼�������
        // skillPressed = playerInput.IsSkillPressed;

        // ����2: ͨ�������⣨���Ƽ�������Ϊ��ѡ��
        // var skillProperty = playerInput.GetType().GetProperty("gameplay.skill");
        // if (skillProperty != null)
        // {
        //     skillPressed = (bool)skillProperty.GetValue(playerInput);
        // }

        // ����3: ����PlayerInputController��һ�������������Լ������
        // ��������һ�������ʵ�֣�����Ҫ����ʵ������޸�
        skillPressed = CheckPlayerSkillInput();

        if (skillPressed)
        {
            float currentTime = Time.time;

            // ����Ƿ���ʱ�䴰����
            if (currentTime - lastSkillPressTime <= skillPressWindow)
            {
                playerSkillPressCount++;
                Debug.Log($"Player skill press count: {playerSkillPressCount}");
            }
            else
            {
                // ���ü���
                playerSkillPressCount = 1;
            }

            lastSkillPressTime = currentTime;

            // �����������10�Σ�����AI����
            if (playerSkillPressCount >= 10 && !isSkillActive)
            {
                ActivateSkill();
                playerSkillPressCount = 0; // ���ü���
            }
        }
    }

    // ���������Ҫ�������PlayerInputControllerʵ��ʵ�����޸�
    private bool CheckPlayerSkillInput()
    {
        // ����1: ʹ��Unity������ϵͳ
        // return Input.GetKeyDown(KeyCode.F) || Input.GetButtonDown("Fire1");

        // ����2: ���PlayerInputControllerʹ���µ�Input System
        // ������Ҫ�������ʵ������������

        // ��ʱʵ�� - ʹ��F����Ϊ���ܼ�
        return Input.GetKeyDown(KeyCode.F);
    }

    private void ActivateSkill()
    {
        isSkillActive = true;
        skillTimer = skillDuration;

        Debug.Log("AI Skill Activated! Za Warudo!");

        // ���浱ǰ״̬���Ա㼼�ܽ�����ָ�
        if (currentState.stateName != "Skill")
        {
            // ������Ա���֮ǰ��״̬����������������ֻ�ڼ����ڼ�ı���Ϊ
        }

        // ��������Ч��
        StartCoroutine(SkillEffectCoroutine());
    }

    private void EndSkill()
    {
        isSkillActive = false;
        Debug.Log("AI Skill Ended");
    }

    private IEnumerator SkillEffectCoroutine()
    {
        // ������Ч - ����ı���ɫ��˸
        Color originalColor = aiMaterial.color;
        float flashDuration = 0.2f;
        int flashCount = 6;

        for (int i = 0; i < flashCount; i++)
        {
            aiMaterial.SetColor("_BaseColor", Color.white);
            yield return new WaitForSeconds(flashDuration);
            aiMaterial.SetColor("_BaseColor", skillState.stateColor);
            yield return new WaitForSeconds(flashDuration);
        }
    }

    private void ExecuteSkillBehavior()
    {
        // ����״̬�µ���Ϊ
        aiMaterial.SetColor("_BaseColor", skillState.stateColor);

        // ʹ�ü���״̬�Ĳ���
        Vector3 targetPosition = CalculateTargetPosition(skillState);
        MoveToPosition(targetPosition, skillState.moveSpeed);

        if (ball.position.y > transform.position.y + skillState.jumpCondition && !isJumping)
        {
            StartCoroutine(Jump(skillState.jumpHeight, skillState.jumpTime));
        }
    }

    private void ExecuteCurrentStateBehavior()
    {
        // ���ݵ�ǰ״ִ̬����Ӧ��Ϊ
        if (currentState.stateName == "Aggressive")
        {
            ExecuteAggressiveBehavior();
        }
        else if (currentState.stateName == "Defensive")
        {
            ExecuteDefensiveBehavior();
        }
    }

    private void ExecuteAggressiveBehavior()
    {
        aiMaterial.SetColor("_BaseColor", aggressiveState.stateColor);

        Vector3 targetPosition = CalculateTargetPosition(aggressiveState);
        MoveToPosition(targetPosition, aggressiveState.moveSpeed);

        if (ball.position.y > transform.position.y + aggressiveState.jumpCondition && !isJumping)
        {
            StartCoroutine(Jump(aggressiveState.jumpHeight, aggressiveState.jumpTime));
        }
    }

    private void ExecuteDefensiveBehavior()
    {
        aiMaterial.SetColor("_BaseColor", defensiveState.stateColor);

        Vector3 targetPosition = CalculateDefensivePosition(defensiveState);
        MoveToPosition(targetPosition, defensiveState.moveSpeed);

        if (ball.position.y > transform.position.y + defensiveState.jumpCondition && !isJumping)
        {
            StartCoroutine(Jump(defensiveState.jumpHeight, defensiveState.jumpTime));
        }
    }

    private Vector3 CalculateTargetPosition(AIState state)
    {
        float predictedX = ball.position.x + (ball.linearVelocity.x * state.predictionX);
        float predictedZ = ball.position.z + (ball.linearVelocity.z * state.predictionZ);

        // �����ڳ��ط�Χ��
        predictedX = Mathf.Clamp(predictedX, minX, maxX);
        predictedZ = Mathf.Clamp(predictedZ, minZ, maxZ);

        return new Vector3(predictedX, transform.position.y, predictedZ);
    }

    private Vector3 CalculateDefensivePosition(AIState state)
    {
        // ����λ�ø�ƫ�򳡵غ�
        float baseZ = minZ + (maxZ - minZ) * 0.3f;
        float predictedX = ball.position.x + (ball.linearVelocity.x * state.predictionX);
        float predictedZ = baseZ + (ball.linearVelocity.z * state.predictionZ * 0.5f);

        predictedX = Mathf.Clamp(predictedX, minX, maxX);
        predictedZ = Mathf.Clamp(predictedZ, minZ, maxZ);

        return new Vector3(predictedX, transform.position.y, predictedZ);
    }

    private void MoveToPosition(Vector3 targetPosition, float speed)
    {
        // �򵥵��ƶ�ʵ��
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            speed * Time.deltaTime
        );
    }

    private void HandleDioTimeBehavior()
    {
        // DIOʱ���ڵ�������Ϊ
        if (!isInDioTime) return;

        // ��DIOʱ����ʹ�ý��͵��ٶ�
        float slowedSpeed = currentState.moveSpeed * 0.3f;
        Vector3 defensivePos = CalculateDefensivePosition(currentState);
        MoveToPosition(defensivePos, slowedSpeed);
    }

    // ��ԾЭ��
    public IEnumerator Jump(float height, float jumpDuration)
    {
        isJumping = true;
        offGround = true;
        float timer = 0;
        Vector3 startPosition = transform.position;

        while (timer < jumpDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / jumpDuration;
            float newY = startPosition.y + (height * Mathf.Sin(progress * Mathf.PI));
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            yield return null;
        }

        transform.position = new Vector3(transform.position.x, startPosition.y, transform.position.z);
        offGround = false;
        isJumping = false;
    }

    // ״̬�л����������Դ��ⲿ���ã�
    public void SwitchToAggressive()
    {
        currentState = aggressiveState;
    }

    public void SwitchToDefensive()
    {
        currentState = defensiveState;
    }

    // DIOʱ����ط���
    private void StartDioTime()
    {
        if (isInDioTime) return;
        isInDioTime = true;
    }

    private void EndDioTime()
    {
        if (!isInDioTime) return;
        isInDioTime = false;
    }

    private void OnDestroy()
    {
        EventManage.RemoveListener("DioTimeStarted", StartDioTime);
        EventManage.RemoveListener("DioTimeEnded", EndDioTime);
    }

    // ������Ϣ
    private void OnGUI()
    {
        if (Application.isEditor)
        {
            GUILayout.BeginArea(new Rect(10, 10, 300, 200));
            GUILayout.Label($"AI State: {currentState.stateName}");
            GUILayout.Label($"Skill Active: {isSkillActive}");
            GUILayout.Label($"Skill Timer: {skillTimer:F1}");
            GUILayout.Label($"Player Skill Presses: {playerSkillPressCount}/10");
            GUILayout.Label($"In Dio Time: {isInDioTime}");
            GUILayout.EndArea();
        }
    }
}

