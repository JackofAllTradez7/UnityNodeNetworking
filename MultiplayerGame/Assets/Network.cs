using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System;

public class Network : MonoBehaviour
{

    static SocketIOComponent socket;
    public GameObject playerPrefab;
    Dictionary<string, GameObject> players;
    public GameObject localPlayer;

    // Start is called before the first frame update
    void Start()
    {
        socket = GetComponent<SocketIOComponent>();
        socket.On("open", OnConnected);
        socket.On("spawn", OnSpawned);
        socket.On("move", OnMoved);
        socket.On("disconnected", OnDisconnected);
        socket.On("requestPosition", OnRequestPosition);
        socket.On("updatePosition", OnUpdatePosition);

        players = new Dictionary<string, GameObject>();
    }

    private void OnUpdatePosition(SocketIOEvent e)
    {
        var id = e.data["id"].ToString();
        var player = players[id];
        var pos = new Vector3(GetFloatFromJson(e.data, "x"), 0, GetFloatFromJson(e.data, "z"));
        player.transform.position = pos;
    }

    private void OnRequestPosition(SocketIOEvent obj)
    {
        //sending local position to server
        socket.Emit("updatePosition", new JSONObject(VectorToJson(localPlayer.transform.position)));
    }

    void OnSpawned(SocketIOEvent e)
    {
        var player = Instantiate(playerPrefab);
        Debug.Log("Spawned " + e.data);
        players.Add(e.data["id"].ToString(), player);
        Debug.Log(players.Count);
    }

    void OnConnected(SocketIOEvent e)
    {
        Debug.Log("Connected to server");
        // sends json data to server
        JSONObject data = new JSONObject ();
        data.AddField("Big Epic Jeff", "yes");
        socket.Emit("Jeff", data);
        
    }

    private void OnMoved(SocketIOEvent e)
    {
        Debug.Log("Network player is moving" + e.data);
        var id = e.data["id"].ToString();
        var player = players[id];


        var pos = new Vector3(GetFloatFromJson(e.data, "x"), 0, GetFloatFromJson(e.data, "z"));
        var navigatepos = player.GetComponent<NavigatePos>();
        navigatepos.NavigateTo(pos);
    }

    private void OnDisconnected(SocketIOEvent e)
    {
        var player = players[e.data["id"].ToString()];
        Destroy(player);
        players.Remove(e.data["id"].ToString());
    }

    float GetFloatFromJson(JSONObject data, string key)
    {
        return float.Parse(data[key].ToString().Replace("\"",""));
    }

    public static string VectorToJson(Vector3 vector)
    {
        return string.Format(@"{{""x"":""{0}"",""z"":""{1}""}}", vector.x, vector.z);
    }

}
