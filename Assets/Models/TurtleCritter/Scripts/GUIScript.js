#pragma strict
var myObject : Transform[];

function Start () {
	Idle();
}
function Idle() {
	myObject[0].GetComponent(WanderScript).animal.transform.GetComponent.<Animation>().CrossFade("Idle");
	myObject[0].GetComponent(WanderScript).animal.transform.GetComponent.<Animation>()["Idle"].speed = 1;
	myObject[0].GetComponent(WanderScript).animal.transform.GetComponent.<Animation>()["Idle"].wrapMode = WrapMode.Loop;
}
function Walk() {
	myObject[0].GetComponent(WanderScript).animal.transform.GetComponent.<Animation>().CrossFade("Walk");
	myObject[0].GetComponent(WanderScript).animal.transform.GetComponent.<Animation>()["Walk"].speed = 1;
	myObject[0].GetComponent(WanderScript).animal.transform.GetComponent.<Animation>()["Walk"].wrapMode = WrapMode.Loop;
}
function Hide() {
	myObject[0].GetComponent(WanderScript).animal.transform.GetComponent.<Animation>().CrossFade("Hide");
	myObject[0].GetComponent(WanderScript).animal.transform.GetComponent.<Animation>()["Hide"].speed = 1;
	myObject[0].GetComponent(WanderScript).animal.transform.GetComponent.<Animation>()["Hide"].wrapMode = WrapMode.Loop;
}
function Death() {
	myObject[0].GetComponent(WanderScript).animal.transform.GetComponent.<Animation>().CrossFade("Death");
	myObject[0].GetComponent(WanderScript).animal.transform.GetComponent.<Animation>()["Death"].speed = 1;
	myObject[0].GetComponent(WanderScript).animal.transform.GetComponent.<Animation>()["Death"].wrapMode = WrapMode.Once;
}