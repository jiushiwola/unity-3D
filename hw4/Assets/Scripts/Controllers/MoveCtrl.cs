using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCtrl 
{
    GameObject moveObject;
    public bool GetIsMoving() {
        return (moveObject != null && moveObject.GetComponent<Move>().isMoving == true);
    }

    public void SetMove(Vector3 destination, GameObject moveObject) {
        Move test;
        this.moveObject = moveObject;
        if (!moveObject.TryGetComponent<Move>(out test)) {
            moveObject.AddComponent<Move>();
        }

        this.moveObject.GetComponent<Move>().destination = destination;
        if (this.moveObject.transform.localPosition.y > destination.y) {
            this.moveObject.GetComponent<Move>().mid_destination = new Vector3(destination.x, this.moveObject.transform.localPosition.y, destination.z);
        }
        else {
            this.moveObject.GetComponent<Move>().mid_destination = new Vector3(this.moveObject.transform.localPosition.x, destination.y, destination.z);
        }
    }
}
