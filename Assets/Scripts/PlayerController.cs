using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 0;
    public TextMeshProUGUI countText;
    public TextMeshProUGUI winTextObject;

    private Rigidbody rb;
    private int count;
    private float movementX;
    private float movementY;    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;

        SetCountText();
        winTextObject.gameObject.SetActive(false);
    }

    // Chiamata ogni volta che viene premuto un input dell'utente
    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void SetCountText()
    {
        countText.text = $"Count: {count}";

        if (count >= 21)
        {
            var name = makeRequest();
            winTextObject.text = $"{name} win!";

            winTextObject.gameObject.SetActive(true);
        }
    }

    // Action of the up click
    public void OnClickUp()
    {
        movementY += 0.1f; 
    }

    // Action of the down click
    public void OnClickDown()
    {
        movementY -= 0.1f;
    }

    // Action of the left click
    public void OnClickLeft()
    {
        movementX -= 0.1f;
    }

    // Action of the right click
    public void OnClickRight()
    {
        movementX += 0.1f;
    }

    // Chiamata 25 volte a frame (l'update viene chiamata 1 volta a frame)
    void FixedUpdate() 
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);    
    }

    // Viene chiamata quando due game object collidono.
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("PickUp"))
        {
            // Agisco solo sui game object di tag Pick Up
            other.gameObject.SetActive(false);
            count += 1;
            SetCountText();
        }  
    } 

    private string makeRequest()
    {
        var restClient = new RestSharp.RestClient("https://gorest.co.in/public/v1");
        var request = new RestSharp.RestRequest("/users/32", RestSharp.Method.GET);

        var result = restClient.Execute<Root>(request).Data;
        return result.data.name;
    }
}

public class Data
{
    public int id { get; set; }
    public string name { get; set; }
    public string email { get; set; }
    public string gender { get; set; }
    public string status { get; set; }
}

public class Root
{
    public object meta { get; set; }
    public Data data { get; set; }
}