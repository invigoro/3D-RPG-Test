using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WeaponSwapScript : MonoBehaviour
{
    public GameObject currentWeapon;


    bool weaponIsSwitching = false;

    //List<GameObject> children
    //{
    //    get
    //    {
    //        transform.Select()
    //    }
    //}
    // Start is called before the first frame update
    void Start()
    {
        //currentWeapon = this.GetComponentsInChildren<Transform>().Where(x => x != this.transform && x.gameObject.activeInHierarchy).FirstOrDefault().gameObject; //first active weapon
    }

    // Update is called once per frame
    void Update()
    {

        var change = !Mathf.Approximately(Input.GetAxis("Weapon"), 0);



        if (change && !weaponIsSwitching)
        {
            //Debug.Log(transform.childCount);
            //Debug.Log(this.GetComponentsInChildren<Transform>().Length);
            var availableWeapons = this.GetComponentsInChildren<Transform>(true).Where(x => x != this.transform).Select(x => x.gameObject).ToList();
            var newWeaponIndex = availableWeapons.IndexOf(currentWeapon) + 1;
            newWeaponIndex = newWeaponIndex >= availableWeapons.Count() ? 0 : newWeaponIndex;
            //Debug.Log($"Array Length: {availableWeapons.Count()}, New Index: {newWeaponIndex}");
            var newWeapon = availableWeapons[newWeaponIndex];
            //Debug.Log($"New Weapon: {newWeapon.name}");
            newWeapon.SetActive(true);
            currentWeapon.SetActive(false);
            currentWeapon = newWeapon;
            weaponIsSwitching = true;
        }
        else if(!change)
        {
            weaponIsSwitching = false;
        }
    }
}
