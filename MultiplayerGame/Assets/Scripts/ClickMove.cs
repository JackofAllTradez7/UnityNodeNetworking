using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickMove : MonoBehaviour
{
    public GameObject player;
    public float score = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnClick(Vector3 pos)
    {
        var moveTo = player.GetComponent<NavigatePos>();
        var netMove = player.GetComponent<NetworkMove>();

        score = score + 10;
        Debug.Log(score);

        moveTo.NavigateTo(pos);
        netMove.OnMove(pos);
    }
}
