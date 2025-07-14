using UnityEngine;
using QFramework;
using UnityEditor.PackageManager;

namespace GameRuntime.Room
{
	public partial class Door : ViewController
	{
		protected enum State
		{
			Open,
			Close
		}

		protected FSM<State> fsm = new FSM<State>();

		public RoomGenerateDir DoorDir;


		public Vector2 DoorPos;

		void Start()
		{
			fsm.State(State.Open)
				.OnEnter(() =>
				{
					SelfBoxCollider2D.isTrigger = true;
					SelfSpriteRenderer.sprite = DoorOpen;
				}).OnExit(() =>
				{
					AudioKit.PlaySound("resources://DoorOpen");
				});
			fsm.State(State.Close)
				.OnEnter(() =>
				{
					SelfBoxCollider2D.isTrigger = false;
					SelfSpriteRenderer.sprite = DoorClose;
				}).OnExit(() =>
				{
					AudioKit.PlaySound("resources://DoorOpen");
				});
			fsm.StartState(State.Open);
		}

		public void OpenDoor()
		{
			fsm.ChangeState(State.Open);
		}

		public void CloseDoor()
		{
			fsm.ChangeState(State.Close);
		}
	}
}
