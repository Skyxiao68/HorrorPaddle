using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class SarahPlayerBT : MonoBehaviour
{
    private CharacterController sarah;
    public Rigidbody ball;

    // 行为树相关
    private BTNode behaviorTree;
    private bool isTreeInitialized = false;

    // 玩家输入检测
    public PlayerInputController playerInput;
    private int playerSkillPressCount = 0;
    private float skillPressWindow = 3f; // 3秒内连续按下
    private float lastSkillPressTime = 0f;
    private bool isSkillActive = false;
    private float skillDuration = 5f;
    private float skillTimer = 0f;

    // AI 状态
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
    public AIState skillState; // 技能状态
    private AIState currentState;

    [Header("Clamp Settings")]
    public float minX, maxX, minZ, maxZ;

    // 其他变量
    private Material aiMaterial;
    private bool isJumping = false;
    private bool offGround = false;

    // DIO 时间相关
    private bool isInDioTime = false;
    private Dictionary<string, float> originalValues = new Dictionary<string, float>();

    private void Awake()
    {
        sarah = GetComponent<CharacterController>();
        aiMaterial = GetComponent<Renderer>().material;

        // 查找玩家输入控制器
        playerInput = new PlayerInputController();
        
        if (playerInput == null)
        {
            Debug.LogWarning("PlayerInputController not found in scene!");
        }

        // 初始化AI状态
        InitializeAIStates();

        // 构建行为树
        BuildBehaviorTree();

        // 事件监听
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
        // 初始化进攻状态
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

        // 初始化防守状态
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

        // 初始化技能状态
        skillState = new AIState
        {
            stateName = "Skill",
            stateColor = Color.green,
            moveSpeed = 12f, // 技能状态下更快
            predictionX = 0.2f,
            predictionZ = 1.5f,
            jumpHeight = 10f, // 技能状态下跳得更高
            jumpTime = 5f,
            jumpCondition = 1.5f
        };

        // 设置初始状态
        currentState = aggressiveState;
    }

    private void BuildBehaviorTree()
    {
        // 构建简化的行为树结构
        behaviorTree = new Selector(new List<BTNode> {
            // 优先级1: 处理技能状态
            new Sequence(new List<BTNode> {
                new Condition(() => isSkillActive),
                new ActionNode(ExecuteSkillBehavior)
            }),
            
            // 优先级2: 处理DIO时间
            new Sequence(new List<BTNode> {
                new Condition(() => isInDioTime),
                new ActionNode(HandleDioTimeBehavior)
            }),
            
            // 优先级3: 执行当前状态行为
            new Sequence(new List<BTNode> {
                new Condition(() => true), // 总是执行
                new ActionNode(ExecuteCurrentStateBehavior)
            })
        });

        isTreeInitialized = true;
    }

    private void Update()
    {
        if (!isTreeInitialized) return;

        // 检测玩家技能输入
        DetectPlayerSkillInput();

        // 更新技能计时器
        if (isSkillActive)
        {
            skillTimer -= Time.deltaTime;
            if (skillTimer <= 0f)
            {
                EndSkill();
            }
        }

        // 执行行为树
        behaviorTree.Execute();
    }

    private void DetectPlayerSkillInput()
    {
        if (playerInput == null) return;

        // 检测玩家是否按下了技能键
        // 这里需要根据你的PlayerInputController实际实现来调整
        bool skillPressed = false;

        // 方法1: 如果PlayerInputController有公开的事件或属性
        // skillPressed = playerInput.IsSkillPressed;

        // 方法2: 通过反射检测（不推荐，但作为备选）
        // var skillProperty = playerInput.GetType().GetProperty("gameplay.skill");
        // if (skillProperty != null)
        // {
        //     skillPressed = (bool)skillProperty.GetValue(playerInput);
        // }

        // 方法3: 假设PlayerInputController有一个公开方法可以检测输入
        // 这里我用一个虚拟的实现，你需要根据实际情况修改
        skillPressed = CheckPlayerSkillInput();

        if (skillPressed)
        {
            float currentTime = Time.time;

            // 检查是否在时间窗口内
            if (currentTime - lastSkillPressTime <= skillPressWindow)
            {
                playerSkillPressCount++;
                Debug.Log($"Player skill press count: {playerSkillPressCount}");
            }
            else
            {
                // 重置计数
                playerSkillPressCount = 1;
            }

            lastSkillPressTime = currentTime;

            // 如果连续按下10次，激活AI技能
            if (playerSkillPressCount >= 10 && !isSkillActive)
            {
                ActivateSkill();
                playerSkillPressCount = 0; // 重置计数
            }
        }
    }

    // 这个方法需要根据你的PlayerInputController实际实现来修改
    private bool CheckPlayerSkillInput()
    {
        // 方法1: 使用Unity的输入系统
        // return Input.GetKeyDown(KeyCode.F) || Input.GetButtonDown("Fire1");

        // 方法2: 如果PlayerInputController使用新的Input System
        // 这里需要根据你的实际设置来调整

        // 临时实现 - 使用F键作为技能键
        return Input.GetKeyDown(KeyCode.F);
    }

    private void ActivateSkill()
    {
        isSkillActive = true;
        skillTimer = skillDuration;

        Debug.Log("AI Skill Activated! Za Warudo!");

        // 保存当前状态，以便技能结束后恢复
        if (currentState.stateName != "Skill")
        {
            // 这里可以保存之前的状态，但根据需求我们只在技能期间改变行为
        }

        // 触发技能效果
        StartCoroutine(SkillEffectCoroutine());
    }

    private void EndSkill()
    {
        isSkillActive = false;
        Debug.Log("AI Skill Ended");
    }

    private IEnumerator SkillEffectCoroutine()
    {
        // 技能特效 - 比如改变颜色闪烁
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
        // 技能状态下的行为
        aiMaterial.SetColor("_BaseColor", skillState.stateColor);

        // 使用技能状态的参数
        Vector3 targetPosition = CalculateTargetPosition(skillState);
        MoveToPosition(targetPosition, skillState.moveSpeed);

        if (ball.position.y > transform.position.y + skillState.jumpCondition && !isJumping)
        {
            StartCoroutine(Jump(skillState.jumpHeight, skillState.jumpTime));
        }
    }

    private void ExecuteCurrentStateBehavior()
    {
        // 根据当前状态执行相应行为
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

        // 限制在场地范围内
        predictedX = Mathf.Clamp(predictedX, minX, maxX);
        predictedZ = Mathf.Clamp(predictedZ, minZ, maxZ);

        return new Vector3(predictedX, transform.position.y, predictedZ);
    }

    private Vector3 CalculateDefensivePosition(AIState state)
    {
        // 防守位置更偏向场地后方
        float baseZ = minZ + (maxZ - minZ) * 0.3f;
        float predictedX = ball.position.x + (ball.linearVelocity.x * state.predictionX);
        float predictedZ = baseZ + (ball.linearVelocity.z * state.predictionZ * 0.5f);

        predictedX = Mathf.Clamp(predictedX, minX, maxX);
        predictedZ = Mathf.Clamp(predictedZ, minZ, maxZ);

        return new Vector3(predictedX, transform.position.y, predictedZ);
    }

    private void MoveToPosition(Vector3 targetPosition, float speed)
    {
        // 简单的移动实现
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            speed * Time.deltaTime
        );
    }

    private void HandleDioTimeBehavior()
    {
        // DIO时间内的特殊行为
        if (!isInDioTime) return;

        // 在DIO时间内使用降低的速度
        float slowedSpeed = currentState.moveSpeed * 0.3f;
        Vector3 defensivePos = CalculateDefensivePosition(currentState);
        MoveToPosition(defensivePos, slowedSpeed);
    }

    // 跳跃协程
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

    // 状态切换方法（可以从外部调用）
    public void SwitchToAggressive()
    {
        currentState = aggressiveState;
    }

    public void SwitchToDefensive()
    {
        currentState = defensiveState;
    }

    // DIO时间相关方法
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

    // 调试信息
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

