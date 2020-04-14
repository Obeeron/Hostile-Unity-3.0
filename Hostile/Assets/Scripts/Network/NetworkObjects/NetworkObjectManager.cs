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

    public List<NetworkObject> netObjList;
    public List<NetworkObject> NetObjList {get => netObjList;}

    public int referenceCode = 0;

    protected virtual void Awake()
    {
        netObjList = new List<NetworkObject>();
        pv = GetComponent<PhotonView>();
    }

    public void InstantiateNetworkObject(int netObjPrefabID, Vector3 position, Vector3 rotation, int newRefCode = 0)
    {

        //Debug.Log("spawning " + netObjPrefabID);

        pv.RPC("InstantiateNetworkObject_Local", RpcTarget.AllViaServer, netObjPrefabID, position, rotation, newRefCode);

    }

    [PunRPC]
    public void InstantiateNetworkObject_Local(int netObjPrefabID, Vector3 position, Vector3 rotation, int newRefCode = 0)
    {
        try
        {

            GameObject netObj = Instantiate(netObjPrefabs[netObjPrefabID].gameObject, position, Quaternion.Euler(rotation), parent.transform);

            AddToList(netObj.GetComponent<NetworkObject>(), newRefCode);
            //Debug.Log("spawned " + netObjPrefabID);

        }

        catch (Exception e)

        {

            Debug.Log("InstantiateNetworkObject_Rpc: Could not instantiante Network Object: " + netObjPrefabID + " prefab list size: " + netObjPrefabs.Count);

            Debug.LogError(e);

        }
    }

    public void AddToList(NetworkObject obj, int newRefCode){
        if(netObjList.Count == 0)
            obj.ID = 0;
        else
            obj.ID = netObjList[netObjList.Count-1].ID + 1;
        Debug.Log("Voici mon id attribué : " + obj.ID);
        netObjList.Add(obj);
        referenceCode = newRefCode;
    }

    public void DeleteNetworkObject(int ID){
        pv.RPC("DeleteNetworkObject_RPC",RpcTarget.AllViaServer,ID);
    }

    [PunRPC]
    public void DeleteNetworkObject_RPC(int ID){
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
