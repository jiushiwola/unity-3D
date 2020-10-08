using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUserAction {
    void MoveBoat();
    void MoveRole(Role roleModel);
    void Check();
}
