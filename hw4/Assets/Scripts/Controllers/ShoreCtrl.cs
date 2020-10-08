using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoreCtrl
{
    Shore shoreModel;
    public void CreateShore(Vector3 position) {
        if (shoreModel == null) {
            shoreModel = new Shore(position);
        }
    }
    public Shore GetShore() {
        return shoreModel;
    }

    //将角色添加到岸上，返回角色在岸上的相对坐标
    public Vector3 AddRole(Role roleModel) {
        roleModel.role.transform.parent = shoreModel.shore.transform;
        roleModel.inBoat = false;
        if (roleModel.isPriest) shoreModel.priestCount++;
        else shoreModel.devilCount++;
        return Position.role_shore[roleModel.id];
    }

    //将角色从岸上移除
    public void RemoveRole(Role roleModel) {
        if (roleModel.isPriest) shoreModel.priestCount--;
        else shoreModel.devilCount--;
    }
}
