using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabAnim : MonoBehaviour {

    Animator anim;
    [SerializeField]private float gazeDuration = 1f;
    [SerializeField] private string GazeValue = "LookAtMe";
    private float gazeTimer = 0;
    //[SerializeField] float senseAngle;
	// Use this for initialization
	void Start () {
        anim = this.GetComponent<Animator>();
	}
	
   
	// Update is called once per frame
	void Update () {

        if ( CheckGazeAtMe())
        {

            gazeTimer += Time.deltaTime;
            if (gazeTimer > gazeDuration)
                anim.SetBool(GazeValue, true);
            else
                anim.SetBool(GazeValue, false);
        }
        else
        {
            gazeTimer = 0;
            anim.SetBool(GazeValue, false);
        }
	}

    bool CheckGazeAtMe()
    {
        Vector3 camPos = Camera.main.transform.position;
        Vector3 forward = Camera.main.transform.forward;

        RaycastHit hitInfo;
        if (Physics.Raycast(camPos, forward, out hitInfo, 1000f))
        {
            if (hitInfo.collider.gameObject == gameObject) {
                return true;
            }
        }

        return false;

    }

    public void OnEndAnimation()
    {
        gazeTimer = 0;
    }

    private void OnDrawGizmosSelected()
    {
        //Gizmos.color = Color.yellow;
        //Vector3 camPos = Camera.main.transform.position;
        //Vector3 forward = transform.position - camPos;
        //Vector3 rangeOne = Quaternion.AngleAxis(senseAngle, Vector3.up) * forward ;
        //Vector3 rangeTwo = Quaternion.AngleAxis(-senseAngle, Vector3.up) * forward;
        //Gizmos.DrawLine(camPos, camPos + rangeOne * 1000f);
        //Gizmos.DrawLine(camPos, camPos + rangeTwo * 1000f);
    }
}
