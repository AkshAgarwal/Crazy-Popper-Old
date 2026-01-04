using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] bool right, left, up, down;
    int speed = 4;

    void Update()
    {// MOVE PROJECTILE IN SPECIFIED DIRECTON
        if (right)
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        if (left)
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        if (up)
            transform.Translate(Vector3.up * speed * Time.deltaTime);
        if (down)
            transform.Translate(Vector3.down * speed * Time.deltaTime);
    }
    private void OnCollisionEnter2D(Collision2D col)
    {// CHECK TO WHICH OBJECT PROJECTILE IS COLLIDED
        if (col.gameObject.CompareTag("wall"))
            Destroy(this.gameObject);
        if (col.gameObject.CompareTag("purple"))
        {
            col.gameObject.GetComponent<Player>().CheckPlayerState(Player.PLAYERCOLOR.Purple);
            Destroy(this.gameObject);
        }
        if (col.gameObject.CompareTag("blue"))
        {
            col.gameObject.GetComponent<Player>().CheckPlayerState(Player.PLAYERCOLOR.Blue);
            Destroy(this.gameObject);
        }
        if (col.gameObject.CompareTag("yellow"))
        {
            col.gameObject.GetComponent<Player>().CheckPlayerState(Player.PLAYERCOLOR.Yellow);
            Destroy(this.gameObject);
        }
    }
}
