﻿using UnityEngine;
using System.Collections;

public abstract class Skill {

	public abstract string Name { get; set; }

	public virtual int cooldown { get; set; }
	protected int _counterCooldown = 0;

	public abstract IEnumerator Cast(GameObject caster);

	public abstract string GetTooltip(GameObject who);

	public bool CanCast(GameObject who) {
		return (_counterCooldown <= 0);
	}

	public void UseResources(GameObject who) {
		_counterCooldown = cooldown;

		who.BroadcastMessage("useEnergy");
	}

	public void OnRegen(GameObject who) {
		_counterCooldown--;
	}

	protected IEnumerator FindTarget(GameObject who, TargetingScheme scheme, int ox, int oy, int range) {
		GameTargeting.GetTarget(who, scheme, ox, oy, range);
		while (GameTargeting.IsTargeting) {
			yield return null;
		}
	}
}