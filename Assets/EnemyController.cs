using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    public float speed = 0.7f;
    string currentScene;
    private Transform playerTransform;
    private SpriteRenderer enemyRenderer;
    private Sprite defaultSprite, newSprite;
    private bool isColliding;
    private Vector2 lastBounceDirection;
    [SerializeField] private Slider _playerHealthSlider;
    private Texture2D attackTexture, texture;
    Vector3 directionToPlayer, normalizedDirection;    
    byte[] fileData;
    public static int hp;

    private Color[] oryginalPixels1, oryginalPixels2;

    void Start()
    {        
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
            playerTransform = playerObject.transform;

        enemyRenderer = GetComponent<SpriteRenderer>();
        defaultSprite = enemyRenderer.sprite;
        currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "level_1")
            attackTexture = LoadTexture("Assets/Textures/braun_punch.png");
        else if (currentScene == "level_2")
            attackTexture = LoadTexture("Assets/Textures/tusk_attacking.png");
        else if (currentScene == "level_3")
            attackTexture = LoadTexture("Assets/Textures/prezes_attacking.png");
        else if (currentScene == "level_4")
            attackTexture = LoadTexture("Assets/Textures/biedron_attacking.png");
        else if (currentScene == "level_5")
            attackTexture = LoadTexture("Assets/Textures/korwin_attacking.png");
        else if (currentScene == "end_game")
            hp = 100;

        isColliding = false;
        lastBounceDirection = Vector2.zero;
        newSprite = Sprite.Create(attackTexture, new Rect(0, 0, attackTexture.width, attackTexture.height), new Vector2(0.5f, 0.5f));

        if (hp == 0)
            hp = 100;
    }

    void Update()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -7.69f, 999f), Mathf.Clamp(transform.position.y, -3.29f, 3.26f), transform.position.z);
        _playerHealthSlider.value = hp;
        if (playerTransform != null)
            MoveTowardsPlayer();
    }

    void MoveTowardsPlayer()
    {
        directionToPlayer = playerTransform.position - transform.position;
        normalizedDirection = directionToPlayer.normalized;

        if (!isColliding)        
            transform.Translate(normalizedDirection * speed * Time.deltaTime);        
        else        
            transform.Translate(lastBounceDirection * speed * Time.deltaTime);        
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        isColliding = true;
        lastBounceDirection = (Vector2)transform.position - other.contacts[0].point;
        lastBounceDirection.Normalize();
        
        if (Random.Range(0, 5) == 0)                    
            Attack();        
    }

    void OnCollisionExit2D(Collision2D other)
    {
        isColliding = false;
    }

    public void HitColor(){

        Color[] pixels = oryginalPixels1 = texture.GetPixels();
        Color[] pixels2 = oryginalPixels2 = texture.GetPixels();

        for(int i=0; i<pixels.Length; i++){
            pixels[i] = Color.white;
            pixels2[i] = Color.white;
        }

        texture.SetPixels(pixels);
        attackTexture.SetPixels(pixels2);
        texture.Apply();
        attackTexture.Apply();
        Invoke("colorBack", 1000f);
    }

    void colorBack(Color[] color1, Color[] color2){
        texture.SetPixels(oryginalPixels1);
        attackTexture.SetPixels(oryginalPixels2);
        texture.Apply();
        attackTexture.Apply();
    }

    void Attack()
    {
        hp -= 5;        

        if (_playerHealthSlider.value <= 0)
        {
            SceneManager.LoadScene("game_over");
            hp = 100;
        }            
        
        ChangeEnemyTexture();
        StartCoroutine(DelayBeforeNextAttack());
    }

    void ChangeEnemyTexture()
    {        
        enemyRenderer.sprite = newSprite;
    }

    IEnumerator DelayBeforeNextAttack()
    {
        yield return new WaitForSeconds(2f);
                RestoreEnemyTexture();
    }    

    void RestoreEnemyTexture()
    {
        enemyRenderer.sprite = defaultSprite;
    }

    Texture2D LoadTexture(string path)
    {
        fileData = System.IO.File.ReadAllBytes(path);
        texture = new Texture2D(2, 2);     
        texture.LoadImage(fileData);
        return texture;
    }
}
