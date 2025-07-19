using UnityEngine;
using QFramework;
using UnityEditor.PackageManager;

namespace GameRuntime.Room
{
	public partial class Door : ViewController
	{
		protected enum State
		{
			IdelOpen,
			IdelClose,
			Open,
			Close
		}

		protected FSM<State> fsm = new FSM<State>();

		public RoomGenerateDir DoorDir;


		public Vector2 DoorPos;

		void Start()
		{
			fsm.State(State.IdelClose).OnEnter(() =>
			{
				SelfBoxCollider2D.isTrigger = false;
				SelfSpriteRenderer.sprite = DoorClose;
			});
			fsm.State(State.IdelClose).OnEnter(() =>
			{
				SelfBoxCollider2D.isTrigger = false;
				SelfSpriteRenderer.sprite = DoorClose;
			}).OnExit(() =>
				{
					AudioKit.PlaySound("resources://DoorOpen");
				});
			fsm.State(State.Open)
				.OnEnter(() =>
				{
					AudioKit.PlaySound("resources://DoorOpen");
					SelfBoxCollider2D.isTrigger = true;
					SelfSpriteRenderer.sprite = DoorOpen;
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
			fsm.StartState(State.IdelClose);
		}

		void OnCollisionEnter2D(Collision2D collision)
		{
			if (collision.gameObject.CompareTag("Player") && fsm.CurrentStateId == State.IdelClose)
			{
				OpenDoor();
			}
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
