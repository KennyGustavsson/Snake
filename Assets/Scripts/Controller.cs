using System.Collections;
using UnityEngine;

public class Controller : MonoBehaviour{
    [Header("Snake")]
    public int startingTailLength = 2;
    public Color snakeColor = Color.black;
    public Vector2Int startingLocation = new Vector2Int(GameManager.Instance.width / 2, GameManager.Instance.height / 2);

    [Header("Controls")] 
    public KeyCode up = KeyCode.W;
    public KeyCode down = KeyCode.S;
    public KeyCode left = KeyCode.A;
    public KeyCode right = KeyCode.D;
    
    private HeadNode _snake = null;
    private Vector2Int _dir = Vector2Int.up;
    
    private void Awake(){
        _snake = new HeadNode(startingLocation, snakeColor, GameManager.Instance.tileColor);

        for (int i = 0; i < startingTailLength; i++){
            _snake.AddTail();
        }
    }

    private void Start(){
        StartCoroutine(MoveSnake());
    }

    private void Update(){
        if(Input.GetKeyDown(up) && _dir != Vector2Int.down) _dir = Vector2Int.up;
        else if(Input.GetKeyDown(left) && _dir != Vector2Int.right) _dir = Vector2Int.left;
        else if(Input.GetKeyDown(down) && _dir != Vector2Int.up) _dir = Vector2Int.down;
        else if(Input.GetKeyDown(right) && _dir != Vector2Int.left) _dir = Vector2Int.right;
    }

    private IEnumerator MoveSnake(){
        while (true){
            while (GameManager.Instance.pause){
                yield return new WaitForEndOfFrame();
            }
            
            _snake.MoveSnake(_dir);
            yield return new WaitForSeconds(GameManager.Instance.gameSpeed);
        }
    }
}
