using Assets.Scripts;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Movments")]
    public float interpVelocity;
    public float minDistance;
    public float followDistance;
    public float speed;
    public GameObject target;
    public Vector3 offset;
    public LevelManager levelManager;

    [Header("Shake")]
    Vector3 targetPos;
    Vector3 shakeOffset;
    public bool isShaking;




    private float rightBound;
    private float leftBound;
    private float topBound;
    private float bottomBound;



    private Camera cam;

    public LayerMask cameraWallLayer;

    private float shakeAmount = 0;

    // Use this for initialization
    void Start()
    {
        targetPos = transform.position;
        levelManager = FindObjectOfType<LevelManager>();
        isShaking = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        levelManager = Persistence.LevelManager;

        if (target)
        {
            Vector3 posNoZ = transform.position;
            posNoZ.z = target.transform.position.z;

            Vector3 targetDirection = (target.transform.position - posNoZ);

            interpVelocity = targetDirection.magnitude * speed;

            targetPos = transform.position + (targetDirection.normalized * interpVelocity * Time.deltaTime);

            Vector3 nextPosition = Vector3.Lerp(transform.position, targetPos + offset, 0.25f);

            cam = this.gameObject.GetComponent<Camera>();
            float camVertExtent = cam.orthographicSize;
            float camHorzExtent = cam.aspect * camVertExtent;

            float space = 0.0f;

            Vector2 posHautGauche = new Vector2(transform.position.x - camHorzExtent, transform.position.y + camVertExtent);
            Vector2 posHautDroit = new Vector2(transform.position.x + camHorzExtent, transform.position.y + camVertExtent);
            Vector2 posBasGauche = new Vector2(transform.position.x - camHorzExtent, transform.position.y - camVertExtent);
            Vector2 posBasDroit = new Vector2(transform.position.x + camHorzExtent, transform.position.y - camVertExtent);
            
            Debug.DrawLine(new Vector2(posHautGauche.x + space, posHautGauche.y), new Vector2(posHautDroit.x - space, posHautDroit.y), Color.red);
            Debug.DrawLine(new Vector2(posBasGauche.x + space, posBasGauche.y), new Vector2(posBasDroit.x - space, posBasDroit.y), Color.red);
            Debug.DrawLine(new Vector2(posHautGauche.x, posHautGauche.y - space), new Vector2(posBasGauche.x, posBasGauche.y + space), Color.red);
            Debug.DrawLine(new Vector2(posHautDroit.x, posHautDroit.y - space), new Vector2(posBasDroit.x, posBasDroit.y + space), Color.red);

            //Pour les Camera Colliders

            RaycastHit2D collideHaut = Physics2D.Linecast(new Vector2(posHautGauche.x + space, posHautGauche.y), new Vector2(posHautDroit.x - space, posHautDroit.y), cameraWallLayer);
            RaycastHit2D collideBas = Physics2D.Linecast(new Vector2(posBasGauche.x + space, posBasGauche.y), new Vector2(posBasDroit.x - space, posBasDroit.y), cameraWallLayer);
            RaycastHit2D collideGauche = Physics2D.Linecast(new Vector2(posHautGauche.x, posHautGauche.y - space), new Vector2(posBasGauche.x, posBasGauche.y + space), cameraWallLayer);
            RaycastHit2D collideDroit = Physics2D.Linecast(new Vector2(posHautDroit.x, posHautDroit.y - space), new Vector2(posBasDroit.x, posBasDroit.y + space), cameraWallLayer);

            if (collideHaut && (nextPosition.y >= transform.position.y))
                nextPosition = new Vector3(nextPosition.x, transform.position.y, nextPosition.z);
            if (collideBas && (nextPosition.y <= transform.position.y))
                nextPosition = new Vector3(nextPosition.x, transform.position.y, nextPosition.z);
            if (collideGauche && (nextPosition.x <= transform.position.x))
                nextPosition = new Vector3(transform.position.x, nextPosition.y, nextPosition.z);
            if (collideDroit && (nextPosition.x >= transform.position.x))
                nextPosition = new Vector3(transform.position.x, nextPosition.y, nextPosition.z);

            if (isShaking)
            {
                if (nextPosition.y + camVertExtent > levelManager.transform.position.y + levelManager.Haut)
                    nextPosition = new Vector3(gameObject.transform.position.x, transform.position.y, gameObject.transform.position.z);
                if (nextPosition.y - camVertExtent < levelManager.transform.position.y - levelManager.Bas)
                    nextPosition = new Vector3(gameObject.transform.position.x, transform.position.y, gameObject.transform.position.z);
                if (nextPosition.x - camHorzExtent < levelManager.transform.position.x - levelManager.Gauche)
                    nextPosition = new Vector3(transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
                if (nextPosition.x + camHorzExtent > levelManager.transform.position.x + levelManager.Droite)
                    nextPosition = new Vector3(transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);

                nextPosition += shakeOffset;
            }
            
            transform.position = nextPosition;

            if (!isShaking)
                LimitToBounds();
        }
    }

    public void LimitToBounds()
    {
        cam = this.gameObject.GetComponent<Camera>();
        float camVertExtent = cam.orthographicSize;
        float camHorzExtent = cam.aspect * camVertExtent;

        if (levelManager != null)
        {
            if (transform.position.y + camVertExtent > levelManager.transform.position.y + levelManager.Haut)
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, levelManager.transform.position.y + levelManager.Haut - camVertExtent, gameObject.transform.position.z);
            if (transform.position.y - camVertExtent < levelManager.transform.position.y - levelManager.Bas)
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, levelManager.transform.position.y - levelManager.Bas + camVertExtent, gameObject.transform.position.z);
            if (transform.position.x - camHorzExtent < levelManager.transform.position.x - levelManager.Gauche)
                gameObject.transform.position = new Vector3(levelManager.transform.position.x - levelManager.Gauche + camHorzExtent, gameObject.transform.position.y, gameObject.transform.position.z);
            if (transform.position.x + camHorzExtent > levelManager.transform.position.x + levelManager.Droite)
                gameObject.transform.position = new Vector3(levelManager.transform.position.x + levelManager.Droite - camHorzExtent, gameObject.transform.position.y, gameObject.transform.position.z);
         }
    }

    private void Update()
    {

    }

    public void Shake(float amount, float length)
    {
        Debug.Log("Shake");
        shakeAmount = amount;
        isShaking = true;
        InvokeRepeating("BeginShake", 0, 0.01f);
        Invoke("StopShake", length);
    }
    void BeginShake()
    {
        if(shakeAmount > 0)
        {
            Vector3 camPos = transform.position;

            float offsetX = Random.value * shakeAmount * 2 - shakeAmount;
            float offsetY = Random.value * shakeAmount * 2 - shakeAmount;

            shakeOffset.x = offsetX;
            shakeOffset.y = offsetY;
        }
    }
    void StopShake()
    {
        CancelInvoke("BeginShake");
        shakeOffset = Vector3.zero;
        isShaking = false;
    }
}

