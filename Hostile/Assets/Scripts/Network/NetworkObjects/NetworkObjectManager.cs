using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

[RequireComponent(typeof(PhotonView))]
public class NetworkObjectManager : MonoBehaviour
{
    public GameObject parent;
    protected PhotonView pv;

    //public enum prefabID; -- Doit être créé dans les classes filles 
    public List<NetworkObject> netObjPrefabs;

    protected List<NetworkObject> netObjList;
    public List<NetworkObject> NetObjList {get => netObjList;}

    protected virtual void Awake()
    {
        netObjList = new List<NetworkObject>();
        pv = GetComponent<PhotonView>();
    }

    public void InstantiateNetworkObject(int netObjPrefabID, Vector3 position, Vector3 rotation){
        pv.RPC("InstantiateNetworkObject_Rpc",RpcTarget.AllViaServer,netObjPrefabID,position, rotation);
    }

    [PunRPC]
    public void InstantiateNetworkObject_Rpc(int netObjPrefabID, Vector3 position, Vector3 rotation){
        try{
            NetworkObject netObj = GameObject.Instantiate(netObjPrefabs[netObjPrefabID],position,Quaternion.Euler(rotation));
            AddToList(netObj);
        }
        catch(Exception e){
            Debug.Log("InstantiateNetworkObject_Rpc: Could not instantiante Network Object");
            Debug.LogError(e);
        }
    }

    public void AddToList(NetworkObject obj){
        if(netObjList.Count == 0)
            obj.ID = 1;
        else
            obj.ID = netObjList[netObjList.Count-1].ID + 1;
        netObjList.Add(obj);
    }

    public void DeleteNetworkObject(int ID){
        try{
            int listIndex = BinarySearch(ID);
            NetworkObject netObj = netObjList[listIndex];
            NetObjList.RemoveAt(listIndex);
            Destroy(netObj.gameObject);
        }
        catch(Exception e){
            Debug.Log("RemoveEntity: Could not find specified ID in networkObjectList");
            Debug.LogError(e);
        }
    }

    public NetworkObject GetNetworkObject(int ID){
        try{
            return netObjList[BinarySearch(ID)];
        }
        catch(Exception e){
            Debug.Log(String.Format("GetNetworkObject: Could not find specified ID:{0} in networkObjectList of size {1}",ID,netObjList.Count));
            Debug.LogError(e);
            return null;
        }
    }

    public int BinarySearch(int ID){
        int a = 0;
        int b = NetObjList.Count-1;
        int m = -1;
        Debug.Log("Desired ID"+ID);
        while(a<=b){
            m = (a+b)/2;
            if(ID < netObjList[m].ID)
                b = m-1;
            else if(ID > netObjList[m].ID)
                a = m+1;
            else
                return m;
        }
        Debug.Log("Binary Search failed m:"+m+" a:"+a+" b:"+b);
        return -1;
    }

}
