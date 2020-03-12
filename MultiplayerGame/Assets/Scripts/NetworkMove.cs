using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using UnityEngine.UI;

public class NetworkMove : MonoBehaviour
{
    public SocketIOComponent socket;
    public GameObject floor;
    public string name;
    public GameObject nameScreen;
    public InputField iField;


    public void OnMove(Vector3 pos)
    {
        Debug.Log("Sending Position to server " + Network.VectorToJson(pos));
        socket.Emit("move", new JSONObject(Network.VectorToJson(pos)));
        socket.Emit("score", new JSONObject(string.Format(@"{{""score"":""{0}"",""name"":""{1}""}}", floor.GetComponent<ClickMove>().score.ToString(), name)));
       // name = iField.text;

       
    }

    public void NewText(string cool)
    {
        name = cool;

        
    }

   
}
