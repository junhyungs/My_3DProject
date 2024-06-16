using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackController : MonoBehaviour
{
  
    private void Update()
    {
        LeftClick();
        RighClick();
    }

    private void LeftClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            WeaponManager.Instance.UseWeaponActive(true);//테스트 용도로 사용된 코드.
            WeaponManager.Instance.UseWeapon();
            LookAtMouse();
        }
        else
            WeaponManager.Instance.UseWeaponActive(false);
    }

    private void RighClick()
    {
        if(Input.GetMouseButton(1))
        {
            SkillManager.Instance.UseSkill();
            LookAtMouse();
        }
    }



    private void LookAtMouse()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay( Input.mousePosition );

        if(Physics.Raycast(mouseRay, out RaycastHit hit, 100))
        {
            Vector3 lookPosition = new Vector3(hit.point.x,transform.position.y, hit.point.z);

            float distance = Vector3.Distance(transform.position, hit.point);

            if(distance > 0.1f)
            {
                transform.LookAt(lookPosition);
            }
        }
    }


}
