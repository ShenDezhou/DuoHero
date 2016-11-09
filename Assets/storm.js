#pragma strict

var wz:WindZone;

function Start () {
	wz = GetComponent.<WindZone>();
		
}

function Update () {
//	transform.Rotate(Vector3(0,90,0)*Time.deltaTime);
	wz.windMain=Random.Range(0.0f, 1.0f);


}