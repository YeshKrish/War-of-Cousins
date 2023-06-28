using UnityEngine;
using Photon.Pun;

public class SyncParent : MonoBehaviour, IPunObservable
{
    private Transform targetParent;

    public void SetParent(Transform parent)
    {
        targetParent = parent;
        if (PhotonNetwork.IsConnectedAndReady && !PhotonNetwork.IsMasterClient)
        {
            // Synchronize the parent across the network for the client
            GetComponent<PhotonView>().RequestOwnership();
        }
        else
        {
            // Set the parent locally for the host
            transform.SetParent(targetParent);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Send the parent's view ID to the network
            if (targetParent != null)
            {
                PhotonView parentView = targetParent.GetComponent<PhotonView>();
                if (parentView != null)
                {
                    stream.SendNext(parentView.ViewID);
                }
                else
                {
                    stream.SendNext(0); // No parent view ID
                }
            }
            else
            {
                stream.SendNext(0); // No parent view ID
            }
        }
        else if (stream.IsReading)
        {
            // Receive the parent's view ID from the network
            int parentViewID = (int)stream.ReceiveNext();

            if (parentViewID != 0)
            {
                // Find the parent transform based on the view ID
                PhotonView parentView = PhotonView.Find(parentViewID);
                if (parentView != null)
                {
                    targetParent = parentView.transform;
                    transform.SetParent(targetParent);
                }
            }
            else
            {
                targetParent = null;
                transform.SetParent(null);
            }
        }
    }
}
