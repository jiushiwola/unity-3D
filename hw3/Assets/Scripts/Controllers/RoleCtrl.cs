using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleCtrl : ClickAction
{
    Role roleModel;
    IUserAction userAction;

    public RoleCtrl() {
        userAction = SSDirector.GetInstance().CurrentSceneController as IUserAction;

    }

    public void CreateRole(Vector3 position, bool isPriest, int id) {
        if (roleModel != null) {
            Object.DestroyImmediate(roleModel.role);
        }
        roleModel = new Role(position, isPriest, id);
        roleModel.role.GetComponent<Click>().setClickAction(this);
    }

    public Role GetRoleModel() {
        return roleModel;
    }

    public void DealClick() {
        userAction.MoveRole(roleModel);
    }
}
