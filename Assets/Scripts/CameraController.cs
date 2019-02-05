using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    Vector2 mouseLook;
    Vector2 smoothV;
    public float sensitivity = 5.0F;
    public float smoothing = 2.0F;

    GameObject player;

    // Use this for initialization
    void Start () {

        player = this.transform.parent.gameObject;
    }
	
	// Update is called once per frame
	void Update () {

        //Use mouse to look
        var md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        md = Vector2.Scale(md, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
        smoothV.x = Mathf.Lerp(smoothV.x, md.x, 1F / smoothing);
        smoothV.y = Mathf.Lerp(smoothV.y, md.y, 1F / smoothing);
        mouseLook += smoothV;

        transform.localRotation = Quaternion.AngleAxis(angle: -mouseLook.y, axis: Vector3.right);
        player.transform.localRotation = Quaternion.AngleAxis(angle: mouseLook.x, axis: player.transform.up);

    }
}
