﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FightController : MonoBehaviour
{
	[HideInInspector] public bool select_friend;
	[HideInInspector] public bool select_enemy;
	
	public List<Friends> friends;
	[HideInInspector] public List<Friends> friends2;
	public List<Enemies> enemies;
	public GameObject[] EnemyPref = new GameObject[1];//UPDATE
	public Spells spell;
	[HideInInspector] public Friends UseFriend;
	[HideInInspector] public Friends TargetFriend;
	[HideInInspector] public Enemies TargetEnemy;
	public Friends CurrentUnit;
	public GameObject restart;
	public GameObject SelectFriend;
	public GameObject SelectEnemy;
	[HideInInspector] public bool res = false;
	[HideInInspector] public int resEn = 0;
	[HideInInspector] public float timeResEn = 0;
	[HideInInspector] public float startTimeResEn = 3;
	public GameObject[] pos = new GameObject[3];
	public GameObject[] posEn = new GameObject[3];
	
	public TextMeshProUGUI textTimeRes;

	public AudioSource music1;
	public AudioSource music2;

    void Start()
    {
		//Здесь была отрисовка иконок
		StartCoroutine(PlayMusic());
    }

	IEnumerator PlayMusic()
	{
		yield return new WaitForSeconds(1);
		music1.Play();
		//yield return new WaitForSeconds(6.0857f);
		yield return new WaitForSeconds(music1.clip.length);
		music2.Play();
	}

	void ChangeCurrentUnit()
	{
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			if (CurrentUnit==null)
			{
				CurrentUnit = friends[0];
				CurrentUnit.ActiveHero();
			}
			else
			{
				CurrentUnit = friends[ (friends.IndexOf(CurrentUnit)+1) % 3];
				CurrentUnit.ActiveHero();
			}
			SelectFriend.SetActive(false);
			SelectEnemy.SetActive(false);
			select_friend=false;
			select_enemy=false;
			print("Выбран " + CurrentUnit.gameObject.name);
		}
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{

			CurrentUnit = friends[0];
			CurrentUnit.ActiveHero();
			SelectFriend.SetActive(false);
			SelectEnemy.SetActive(false);
			select_friend=false;
			select_enemy=false;
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			CurrentUnit = friends[1];
			CurrentUnit.ActiveHero();
			SelectFriend.SetActive(false);
			SelectEnemy.SetActive(false);
			select_friend=false;
			select_enemy=false;
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			CurrentUnit = friends[2];
			CurrentUnit.ActiveHero();
			SelectFriend.SetActive(false);
			SelectEnemy.SetActive(false);
			select_friend=false;
			select_enemy=false;
		}
	}

	public int AliveHeroes()
	{
		int res = 0;
		foreach(Friends fr in friends)
		{
			if(fr.alive)
				res++;
		}
		//print("Кол-во живых героев" + res);
		return res;
	}
	
    private void Update()
    {
		if (AliveHeroes() == 0)
		{
			restart.SetActive(true);
			//Снятие эффектов у врагов
		}
		else
		{
			ChangeCurrentUnit();
		}
		if (res)
		{
			restart.SetActive(false);
            for (int i = 0; i <= friends2.Count-1; i++)
            {
				friends.Add(friends2[i]);
				friends2[i].transform.position = pos[i].transform.position;
            }
			//UPDATE
            for (int i = 0; i <= enemies.Count-1; i++)
            {
				enemies[i].hp = enemies[i].maxhp;
				enemies[i].Combat = true;
            }
			//UPDATE
			res = false;
		}
		if ((enemies.Count == 0) & (resEn == 0))
        {
            resEn = 1;
            timeResEn = startTimeResEn;
        }
        timeResEn -= Time.deltaTime;
        if (timeResEn>0)
        { textTimeRes.text = Mathf.Ceil(timeResEn).ToString(); } else { textTimeRes.text = ' '.ToString(); }
        if ((resEn == 1) & (timeResEn <= 0))
        {
			//UPDATE
			int r = Random.Range(1,4);
            for (int i = 0; i <r ; i++)
            {
				var enemy = Instantiate(EnemyPref[Random.Range(0,EnemyPref.Length)], posEn[i].transform.position, transform.rotation) as GameObject;
				enemy.GetComponent<Enemies>().fightController=this;
            }
            resEn = 0;
			//UPDATE
        }
	}


    public void SpellUseTarget()
	{
		
		if (select_friend) 
		{
			if (TargetFriend!=null)
			{
				spell.Use(TargetFriend);
				//!
				//UseFriend.reloadTime=UseFriend.spell_cooldown[spell_num];
				select_friend=false;
				SelectFriend.SetActive(false);
				TargetFriend=null;
				//!Отрисовать затемнение на иконках способностей
				/*for(int i=0; i<3; ++i)
					UseFriend.CreateSpellReload(i,UseFriend.spell_cooldown[spell_num]);*/
			}
		}				
		else
		if (select_enemy)
		{
			if (TargetEnemy!=null)
			{
				print("ABBA");
				spell.Use(TargetEnemy);
				//!
				//UseFriend.reloadTime=UseFriend.spell_cooldown[spell_num];
				select_enemy=false;
				SelectEnemy.SetActive(false);
				TargetEnemy=null;
				//!Отрисовать затемнение на иконках способностей
				/*for(int i=0; i<3; ++i)
					UseFriend.CreateSpellReload(i,UseFriend.spell_cooldown[spell_num]);*/
			}
		}
	}

	public void SpellUseAll()
	{
		spell.Use();
		//character.SetReload(spell.reloadtime);
		//Затемнение иконок способностей
	}

	
}
