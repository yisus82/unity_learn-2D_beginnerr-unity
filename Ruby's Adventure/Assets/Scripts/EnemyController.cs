using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    
    public float speed = 1.0f;
    public Direction initialDirection = Direction.Down;
    public float changeTime = 3.0f;
    public int damageAmount = 1;
  
    private Rigidbody2D _rigidbody2d;
    private float _timer;
    private Direction _direction;
    private bool _vertical = true;

    private void Start()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _timer = changeTime;
        _direction = initialDirection;
        _vertical = initialDirection is Direction.Up or Direction.Down;
    }

    private void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer > 0)
        {
            return;
        }
        
        if (_vertical)
        {
            _direction = _direction == Direction.Up ? Direction.Down : Direction.Up;
        }
        else
        {
            _direction = _direction == Direction.Right ? Direction.Left : Direction.Right;
        }
        
        _timer = changeTime;
    }

    private void FixedUpdate()
    {    
        var position = _rigidbody2d.position;
     
        switch (_direction)
        {
            case Direction.Up:
                position.y += speed * Time.fixedDeltaTime;
                break;
            case Direction.Down:
                position.y -= speed * Time.fixedDeltaTime;
                break;
            case Direction.Left:
                position.x -= speed * Time.fixedDeltaTime;
                break;
            case Direction.Right:
                position.x += speed * Time.fixedDeltaTime;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        _rigidbody2d.MovePosition(position);
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        var player = other.gameObject.GetComponent<PlayerController>();

        if (player == null)
        {
            return;
        }

        if (player.health <= 0)
        {
            return;
        }

        player.ChangeHealth(-damageAmount);
    }
}
