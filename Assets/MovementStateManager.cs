using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MovementStateManager : MonoBehaviour
{
    public float speed = 5f;
    string currentScene;
    Vector3 currentPosition, newPosition;
    private SpriteRenderer playerRenderer;
    private Sprite defaultSprite;
    bool combat, attacking;
    Texture2D texture;
    byte[] fileData;
    Sprite nowySprite;
    [SerializeField] private Slider _braunHealthSlider;

    EnemyController EC = new EnemyController();
    

    void Start()
    {

        playerRenderer = GetComponent<SpriteRenderer>();
        defaultSprite = playerRenderer.sprite;
        combat = true;
        attacking = false;
        texture = LoadTexture("Assets/Textures/player_attack.png");
        currentScene = SceneManager.GetActiveScene().name;
    }
    
    void Update() {
        MoveOnKeyPress();

        if ( Input.GetKeyDown(KeyCode.Space)) {
            ChangePlayerTexture();

            if (attacking){
                _braunHealthSlider.value -= 5f;
                EC.HitColor();
                }
        }            

        if (Input.GetKeyUp(KeyCode.Space))        
            playerRenderer.sprite = defaultSprite;

        if (_braunHealthSlider.value <= 0) {
            if (currentScene == "level_1")
                SceneManager.LoadScene("level_2");
            else if (currentScene == "level_2")
                SceneManager.LoadScene("level_3");
            else if (currentScene == "level_3")
                SceneManager.LoadScene("level_4");
            else if (currentScene == "level_4")
                SceneManager.LoadScene("level_5");
            else if (currentScene == "level_5")
                SceneManager.LoadScene("end_game");
        }               
    }

    void OnCollisionEnter2D(Collision2D other) {
        attacking = true;        
    }

    void OnCollisionExit2D(Collision2D other) {
        attacking = false;
    }

    void MoveOnKeyPress() {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0f).normalized;

        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");

        if (direction.magnitude > 0 && transform.position.y >= -3.29 && transform.position.y <= 3.26)
        {
            currentPosition = transform.position;
            newPosition = currentPosition + direction * speed * Time.deltaTime;

            newPosition.x = Mathf.Clamp(newPosition.x, -7.69f, 999f);
            newPosition.y = Mathf.Clamp(newPosition.y, -3.29f, 3.26f);
            
            transform.position = newPosition;

            if ( !combat ) {
                float deltaX = newPosition.x - currentPosition.x;
                camera.transform.position += new Vector3(deltaX, 0, 0);
            }            
        }
    }

    void ChangePlayerTexture()
    {        
            nowySprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            playerRenderer.sprite = nowySprite;            
    }
    
    Texture2D LoadTexture(string path)
    {
        fileData = System.IO.File.ReadAllBytes(path);
        Texture2D newTexture = new Texture2D(2, 2);
        newTexture.LoadImage(fileData);
        return newTexture;
    }
}
