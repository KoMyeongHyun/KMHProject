using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class Hand : MonoBehaviour
{
	[SerializeField]
	private Transform cameraTrans;
	[SerializeField]
	private Transform handTrans;
	[SerializeField]
	private Vector3 positionOffset;
	[SerializeField]
	private float rotationSpeed;

	private Animation handAni;
	private FirstPersonController player;

	void Start ()
	{
		handTrans.localPosition = positionOffset;

		handAni = handTrans.GetComponent<Animation> ();
		handAni.wrapMode = WrapMode.PingPong;
		handAni.Stop ();

		player = GameObject.FindGameObjectWithTag("Player")
			.GetComponent<FirstPersonController>();
	}

	void Update()
	{
		if (player.WalkState == WALK_STATE.STOP) 
		{
			handAni.Stop ();
			return;
		}

		float aniSpeed = 0.0f;
		switch (player.WalkState) 
		{
		case WALK_STATE.WALK:
			aniSpeed = 1.0f;
			break;
		case WALK_STATE.SLOW_WALK:
			aniSpeed = 0.5f;
			break;
		case WALK_STATE.RUN:
			aniSpeed = 1.7f;
			break;
		}
		handAni ["MovingHand"].speed = aniSpeed;

		if (handAni.isPlaying == false) 
		{
			handAni.Play ();
		}
	}

	void LateUpdate ()
	{
		//애니메이션 재생시켜 주면서 lerp으로 handT.position으로 가도록 도와줘야 하나?
		//handT position의 로컬 값을 변경해주자!!!
		transform.position = handTrans.position;

		//transform.rotation = cameraT.rotation;
		transform.rotation = Quaternion.Slerp(transform.rotation, cameraTrans.rotation, 
			rotationSpeed*Time.deltaTime);
	}
}
