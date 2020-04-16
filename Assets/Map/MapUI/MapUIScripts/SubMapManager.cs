﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubMapManager : MonoBehaviour {

	#region internal structrue
	private struct Construction {
		public GameObject establishment;
		public int rank;
	}
	#endregion

	#region field
	private List<GameObject> avesSettled = new List<GameObject>();

	[HideInInspector]
	/// <summary>
	/// 树枝
	/// </summary>
	public GameObject branch;

	[HideInInspector]
	/// <summary>
	/// 地块唯一标识，一块地只能有一种鸟，暂用string标识，考虑改为枚举
	/// </summary>
	public string avesFlag;

	/// <summary>
	/// 产出树枝计时器
	/// </summary>
	private float manufactureBranchesTimer = 0;

	/// <summary>
	/// 产出树枝的时间间隔
	/// </summary>
	[SerializeField]
	private float manufactureBranchesTimeThreshold = 10f;

	/// <summary>
	/// 子地图上的设施建造信息
	/// </summary>
	private Construction construction;
	#endregion



	#region monobehaviour
	private void Awake() {
		branch = Resources.Load<GameObject>("Branch");
		branch = transform.Find("Branch").gameObject;
		branch.SetActive(false);
	}

	private void Update() {
		ManufactureBranches();
	}
	#endregion

	#region private methods
	private void ManufactureBranches() {
		if (GameGuide.Instance.isGameGuiding) {
			if (avesSettled.Count > 0) {
				// 新手引导期间只产生一此树枝
			}
		}
		if (avesSettled.Count == 0) {
			// 没有已入住鸟类，无树枝产出
			return;
		}
		manufactureBranchesTimer += Time.deltaTime;
		if (manufactureBranchesTimer < manufactureBranchesTimeThreshold) {
			// 时间未满不产出树枝
			return;
		}
		manufactureBranchesTimer = 0;
		// 根据入住鸟类数量、当地建筑设施等产出树枝
		int avesCount = avesSettled.Count;
		int branchCount = 1;
		if (branch.activeSelf) {
			branch.GetComponent<Branch>().AddCount(branchCount);
		} else {
			branch.SetActive(true);
			branch.GetComponent<Branch>().AddCount(branchCount);
		}

	}
	#endregion

	#region public methods
	public bool AvailableForNewAves() {
		if (avesSettled.Count <= 3) {
			return true;
		} else {
			return false;
		}
	}

	public bool AddNewAves(GameObject newAves) {
		if (avesSettled.Count == 0) {
			// 若地块还没有任何鸟类，标记地块标识
			avesFlag = newAves.name;
		} else {
			if (avesFlag != newAves.name) {
				Debug.Log("该地图已被其他鸟类占领");
				return false;
			}
		}
		avesSettled.Add(newAves);
		newAves.transform.position = transform.position;
		newAves.transform.parent = transform;
		newAves.SetActive(true);
		return true;
	}

	public void AddBranchEventsForGuide() {
		branch.AddComponent<BranchEventsForGuide>();
	}

	/// <summary>
	/// 设施建造
	/// </summary>
	public void Construct(GameObject establishment, int rank) {
		if (GameGuide.Instance.isGameGuiding) {
			DialogueController.Instance.ShowDialogue();
			DialogueManager.Instance.UpdateDialogueStatus();
			DialogueManager.Instance.PlayNext();
		}
		construction.establishment = establishment;
		construction.rank = rank;
		construction.establishment.transform.parent = transform;
		construction.establishment.transform.localPosition = Vector2.zero;
	}

	#endregion

}
