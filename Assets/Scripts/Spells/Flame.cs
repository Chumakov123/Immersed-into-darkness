﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame : Spells
{
	public float time;
	public GameObject FlamePref;
	public GameObject SmokePref;

	private GameObject smokePr;
	private EfFlame flame;
	private int start;

    private void Update()
    {
        if (start == 1)
        {
			if (flame.time <= 0)
			{
				Destroy(smokePr);
			}
		}
    }

    public override void SpellUseTarget(Characters character)
	{
		print("Применен поджег");
		if (character.EfIce==null)
		{
			if (character.EfFlame==null)
			{
				flame = (Instantiate(FlamePref, character.transform.position, transform.rotation)).GetComponent<EfFlame>();
				smokePr = Instantiate(SmokePref);
				smokePr.transform.position = new Vector3(character.transform.position.x, character.transform.position.y, character.transform.position.z);
				start = 1;
				character.EfFlame=flame;
				flame.time=time;
				flame.character=character;
			}
			else
			{
				character.EfFlame.time=time;
			}
		}
		else
		{
			character.FreezeParticle.GetComponent<ParticleSystem>().Stop();
			Destroy(character.EfIce.gameObject);
			Destroy(character.FreezeParticle.gameObject,5f);
			character.FreezeParticle=null;
		}
	}
}