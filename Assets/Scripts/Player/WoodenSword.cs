using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenSword : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Animator>().GetBool("IsAttacking"))
        {
            GetComponent<Animator>().SetBool("IsAttacking", false);
        }
        if (Input.GetButtonDown("Fire1"))
        {
            GetComponent<Animator>().SetBool("IsAttacking", true);
        }

    }

    void ChangeRotation(float angle)
    {
        var rotationVector = transform.rotation.eulerAngles;
        rotationVector.z = angle;
        transform.rotation = Quaternion.Euler(rotationVector);
    }

}


