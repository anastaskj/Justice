using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour
{
    [SerializeField]
    float duration;
    [SerializeField]
    float timer;
    [SerializeField]
    float yMoveSpeed;

    Color textColor;

    private void Start()
    {
        textColor = GetComponent<Text>().color;
    }

    private void Update()
    {
        transform.position += new Vector3(0, yMoveSpeed, 0);

        timer -= Time.deltaTime;
        if (timer < 0)
        {
            textColor.a-= duration * Time.deltaTime;
            if (textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
