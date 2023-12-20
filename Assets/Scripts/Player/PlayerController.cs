using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Event Listening")]
    public SceneLoadEventSO sceneLoadEvent;
    public VoidEventSO afterSceneLoadedEvent;
    public VoidEventSO loadDataEvent;
    public VoidEventSO backToMenuEvent;
    public VoidEventSO newGameEvent;
    public VoidEventSO restartGameEvent;
    public IntEventSO chestOpenEvent;
    public PlayerInputControl inputControl;
    public Vector2 inputDirection;

    [Header("Basic Parameters")]
    public int level;
    public float speed;
    public float kickMoveSpeed;
    public float jumpForce;
    public float strikePower;
    public Vector2 fireOffset;
    public Rigidbody2D rb;
    public GameObject fireBallPrefab;
    public GameObject superFireBallPrefab;
    public FireGenerator fireGenerator;
    public GameObject playerShine;
    private CapsuleCollider2D coll;
    private PhysicsCheck physicsCheck;
    private PlayerAnimation playerAnimation;
    private Character character;
    private AudioDefinition audioDefinition;
    public AudioClip jumpAudioClip;
    public AudioClip strikeAudioClip;
    public AudioClip fireAudioClip;
    public AudioClip superFireAudioClip;
    public TrailRenderer tr;
    public float originalGravity;
    private float runSpeed;
    public float hurtForce;
    private float walkSpeed => speed / 2f;
    public int kickPowerCost;
    public int strikePowerCost;
    public int superFireBallPowerCost;
    public int fireBallPowerCost;
    private Vector2 originalOffset;
    private Vector2 originalSize;

    [Header("Physics Material")]
    public PhysicsMaterial2D normal;
    public PhysicsMaterial2D wall;

    [Header("State")]
    public bool isCrouch;
    public bool isHurt;
    public bool isDead;
    public bool isKick;
    public bool isKickMove;
    public bool isStrike;
    public bool isFire;
    public bool isShine;
    public bool canDoubleJump;
    public bool canStrike;
    public int faceDir;

    [Header("Skill")]
    public bool learnedDoubleJump;
    public bool learnedStrike;
    public bool learnedSuperFireBall;
    public bool learnedSwim;
    public bool learnedShine;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        coll = GetComponent<CapsuleCollider2D>();
        playerAnimation = GetComponent<PlayerAnimation>();
        character = GetComponent<Character>();
        audioDefinition = GetComponent<AudioDefinition>();
        originalGravity = rb.gravityScale;
        originalOffset = coll.offset;
        originalSize = coll.size;
        faceDir = (int)transform.localScale.x;
        inputControl = new PlayerInputControl();
        inputControl.Gameplay.Jump.started += Jump;
        #region walk
        runSpeed = speed;
        inputControl.Gameplay.WalkButton.performed += ctx =>
        {
            if (physicsCheck.isOnGround)
            {
                speed = walkSpeed;
            }
        };
        inputControl.Gameplay.WalkButton.canceled += ctx =>
        {
            if (physicsCheck.isOnGround)
            {
                speed = runSpeed;
            }
        };
        #endregion

        inputControl.Gameplay.Kick.started += PlayerKick;
        inputControl.Gameplay.Strike.started += Strike;
        inputControl.Gameplay.FireBall.started += FireBall;
        inputControl.Gameplay.SuperFireBall.started += SuperFireBall;
        inputControl.Gameplay.Shine.started += Shine;
        inputControl.Enable();

    }

    private void Shine(InputAction.CallbackContext obj)
    {
        if (learnedShine)
        {
            isShine = !isShine;
            playerShine.SetActive(isShine);
        }
    }

    private void SuperFireBall(InputAction.CallbackContext obj)
    {
        if (learnedSuperFireBall)
        {
            if (!isFire && !isKick && !isStrike && character.currentPower >= superFireBallPowerCost)
            {
                audioDefinition.audioClip = superFireAudioClip;
                audioDefinition.PlayAudioClip();
                playerAnimation.PlayerFire();
                isFire = true;
                fireGenerator.GenerateBullet(superFireBallPrefab);
                character.OnPlayerSkill(superFireBallPowerCost);
            }
        }
    }

    private void FireBall(InputAction.CallbackContext obj)
    {
        if (!isFire && !isKick && !isStrike && character.currentPower >= fireBallPowerCost)
        {
            audioDefinition.audioClip = fireAudioClip;
            audioDefinition.PlayAudioClip();
            playerAnimation.PlayerFire();
            isFire = true;
            fireGenerator.GenerateBullet(fireBallPrefab);
            character.OnPlayerSkill(fireBallPowerCost);
        }
    }

    private void OnEnable()
    {
        sceneLoadEvent.LoadRequestEvent += OnLoadEvent;
        afterSceneLoadedEvent.OnEventRaised += OnAfterSceneLoadedEvent;
        loadDataEvent.OnEventRaised += OnLoadDataEvent;
        backToMenuEvent.OnEventRaised += OnLoadDataEvent;
        chestOpenEvent.OnEventRaised += OnChestOpenEvent;
        newGameEvent.OnEventRaised += OnNewGameEvent;
        restartGameEvent.OnEventRaised += OnRestartGameEvent;
    }


    private void OnDisable()
    {
        inputControl.Disable();
        sceneLoadEvent.LoadRequestEvent -= OnLoadEvent;
        afterSceneLoadedEvent.OnEventRaised -= OnAfterSceneLoadedEvent;
        loadDataEvent.OnEventRaised -= OnLoadDataEvent;
        backToMenuEvent.OnEventRaised -= OnLoadDataEvent;
        chestOpenEvent.OnEventRaised -= OnChestOpenEvent;
        newGameEvent.OnEventRaised -= OnNewGameEvent;
        restartGameEvent.OnEventRaised -= OnRestartGameEvent;
    }
    private void OnRestartGameEvent()
    {
        isDead = false;
    }
    private void OnNewGameEvent()
    {
        ChangeToInitial();
    }

    private void ChangeToInitial()
    {
        learnedDoubleJump = false;
        learnedStrike = false;
        learnedSuperFireBall = false;
        learnedSwim = false;
        learnedShine = false;
        level = 0;
        character.maxHealth = 50;
        character.maxPower = 15;
        character.powerRecoverSpeed = 1.5f;
        isDead = false;
    }

    private void OnChestOpenEvent(int num)
    {
        switch (num)
        {
            case 1:
                if (learnedSwim == false)
                {
                    learnedSwim = true;
                    character.canSwim = learnedSwim;
                    LevelUp();
                }
                break;
            case 2:
                if (learnedDoubleJump == false)
                {
                    learnedDoubleJump = true;
                    LevelUp();
                }
                break;
            case 3:
                if (learnedStrike == false)
                {
                    learnedStrike = true;
                    LevelUp();
                }
                break;
            case 4:
                if (learnedShine == false)
                {
                    learnedShine = true;
                    LevelUp();
                }
                break;
            case 5:
                if (learnedSuperFireBall == false)
                {
                    learnedSuperFireBall = true;
                    LevelUp();
                }
                break;
            default:
                break;
        }
        character.currentHealth = character.maxHealth;
        character.currentPower = character.maxPower;
        character.OnHealthChange?.Invoke(character);
    }

    private void LevelUp()
    {
        level += 1;
        switch (level)
        {
            case 1:
                character.maxHealth = 70;
                character.maxPower = 20;
                character.powerRecoverSpeed = 2.0f;
                break;
            case 2:
                character.maxHealth = 90;
                character.maxPower = 25;
                character.powerRecoverSpeed = 2.5f;
                break;
            case 3:
                character.maxHealth = 110;
                character.maxPower = 30;
                character.powerRecoverSpeed = 3.0f;
                break;
            case 4:
                character.maxHealth = 130;
                character.maxPower = 35;
                character.powerRecoverSpeed = 3.5f;
                break;
            case 5:
                character.maxHealth = 150;
                character.maxPower = 40;
                character.powerRecoverSpeed = 4.0f;
                break;
            default:
                break;
        }
    }
    //读取游戏进度
    private void OnLoadDataEvent()
    {
        isDead = false;
    }

    private void OnAfterSceneLoadedEvent()
    {
        inputControl.Gameplay.Enable();
    }

    private void OnLoadEvent(GameSceneSO arg0, Vector3 arg1, bool arg2)
    {
        inputControl.Gameplay.Disable();
    }
    private void Update()
    {
        inputDirection = inputControl.Gameplay.Move.ReadValue<Vector2>();
        CheckState();
        if (physicsCheck.isOnGround)
        {
            if (!canDoubleJump)
            {
                canDoubleJump = true;
            }
            if (!canStrike)
            {
                canStrike = true;
            }
        }
    }
    private void FixedUpdate()
    {
        if (!isHurt && !isKick && !isStrike)
        {
            Move();
        }
        else if (isStrike)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
        }
        if (isKickMove)
        {
            KickMove();
        }
        
    }

    public void Move()
    {
        if (!isCrouch)
        {
            rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb.velocity.y);
        }

        //int faceDir = (int)transform.localScale.x;

        if (inputDirection.x > 0 && faceDir == -1)
        {
            faceDir = 1;
            fireGenerator.transform.Rotate(0f, 180f, 0f);

        }
        if (inputDirection.x < 0 && faceDir == 1)
        {
            faceDir = -1;
            fireGenerator.transform.Rotate(0f, 180f, 0f);
        }

        transform.localScale = new Vector3(faceDir, 1, 1);

        isCrouch = inputDirection.y < -0.1f && physicsCheck.isOnGround;
        if (isCrouch)
        {
            coll.size = new Vector2(0.64f, 0.96f);
            coll.offset = new Vector2(0f, 0.55f);
        }
        else
        {
            coll.size = originalSize;
            coll.offset = originalOffset;
        }
    }
    private void Jump(InputAction.CallbackContext obj)
    {
        if (!isStrike)
        {
            if (physicsCheck.isOnGround)
            {
                rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
                audioDefinition.audioClip = jumpAudioClip;
                audioDefinition.PlayAudioClip();
            }
            else if (learnedDoubleJump && canDoubleJump)
            {
                //DoubleJump
                canDoubleJump = false;
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
                audioDefinition.audioClip = jumpAudioClip;
                audioDefinition.PlayAudioClip();
            }
        }
    }

    private void Strike(InputAction.CallbackContext obj)
    {
        if (learnedStrike)
        {
            if (!isStrike && !isFire && !isKick && character.currentPower >= strikePowerCost && canStrike && !physicsCheck.isOnGround) 
            {
                isStrike = true;
                canStrike = false;
                rb.gravityScale = 0;
                rb.velocity = new Vector2(transform.localScale.x * strikePower, 0f);
                tr.emitting = true;
                gameObject.layer = LayerMask.NameToLayer("Enemy");
                audioDefinition.audioClip = strikeAudioClip;
                audioDefinition.PlayAudioClip();
                character.OnPlayerSkill(strikePowerCost);
            }
        }
    }

    private void PlayerKick(InputAction.CallbackContext obj)
    {
        //if (!physicsCheck.isOnGround)
        //{
        //    return;
        //}
        if (!isKick && !isFire && !isStrike && character.currentPower >= kickPowerCost) 
        {
            playerAnimation.PlayerKick();
            isKick = true;
            gameObject.layer = LayerMask.NameToLayer("Enemy");
            character.OnPlayerSkill(kickPowerCost);
        }
    }

    private void KickMove()
    {
        if (!isHurt)
        {
            rb.velocity = new Vector2(faceDir * kickMoveSpeed * Time.deltaTime, rb.velocity.y);
        }
    }
    #region Event
    public void GetHurt(Transform attacker)
    {
        isHurt = true;
        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2((transform.position.x - attacker.position.x), 0).normalized;
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
    }

    public void PlayerDead()
    {
        isDead = true;
        inputControl.Gameplay.Disable();
    }
    #endregion

    private void CheckState()
    {
        coll.sharedMaterial = physicsCheck.isOnGround ? normal : wall;
    }
}
