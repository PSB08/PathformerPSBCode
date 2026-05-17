using System.Collections.Generic;
using UnityEngine;

namespace Code.Object.NodeMake
{
    public class Node : MonoBehaviour
    {
        public List<Node> connectedNodes = new();

        public void ConnectTo(Node other)
        {
            if (!connectedNodes.Contains(other))
            {
                connectedNodes.Add(other);
                other.connectedNodes.Add(this);
            }
        }

        public void DisconnectFrom(Node other)
        {
            if (connectedNodes.Contains(other))
            {
                connectedNodes.Remove(other);
                other.connectedNodes.Remove(this);
            }
        }
        
    }   
}
