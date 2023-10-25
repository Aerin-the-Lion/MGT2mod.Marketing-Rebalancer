using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using BepInEx.Configuration;
using UnityEngine;
using HarmonyLib;
using UnityEngine.UI;
using System.Runtime.CompilerServices;
using System.Reflection;

namespace MarketingRebalancer
{


	public class MarketingRebalancer : MonoBehaviour
	{


		public static float _MAX_HYPE { get; private set; }
		public static bool _IS_ENABLED { get; private set; }

		public void Init()
		{
			_MAX_HYPE = Main.CFG_MAX_HYPE.Value;
			_IS_ENABLED = Main.CFG_IS_ENABLED.Value;
		}

		[HarmonyPostfix, HarmonyPatch(typeof(gameScript), "SellGame")]
		static void SellGame_ModifyHypePostPatch(gameScript __instance)
		{

			//if (!__instance.IsMyGame())
			//{
			//__instance.hype = _MAX_HYPE;
			//}
		}

		[HarmonyPrefix, HarmonyPatch(typeof(gameScript), "AddHype")]
		static bool AddHype_ModifyHypePrePatch(float f, gameScript __instance, ref bool __runOriginal)
		{
			MarketingRebalancer mR_ = new MarketingRebalancer();
			mR_.Init();
			if (!_IS_ENABLED){return true;}

			if (__instance.hype <= _MAX_HYPE)
			{
				__instance.hype += f;
				if (__instance.hype > _MAX_HYPE)
				{
					__instance.hype = _MAX_HYPE;
				}
			}
			if (__instance.hype > _MAX_HYPE && f < 0f)
			{
				__instance.hype += f;
			}
			if (__instance.hype < 0f)
			{
				__instance.hype = 0f;
			}

			return false;
		}


		public void GameKampagne_InitToDefault(Menu_Marketing_GameKampagne mmG_)
        {
			//MarketingのHype上限値
			mmG_.maxHype[0] = 15;
			mmG_.maxHype[1] = 25;
			mmG_.maxHype[2] = 40;
			mmG_.maxHype[3] = 60;
			mmG_.maxHype[4] = 80;
			mmG_.maxHype[5] = 100;
			//Marketingの一回あたりのHype
			mmG_.hypeProKampagne[0] = 5;
			mmG_.hypeProKampagne[1] = 10;
			mmG_.hypeProKampagne[2] = 15;
			mmG_.hypeProKampagne[3] = 20;
			mmG_.hypeProKampagne[4] = 25;
			mmG_.hypeProKampagne[5] = 30;
			//Marketingの作業工数
			mmG_.workPoints[0] = 25;
			mmG_.workPoints[1] = 50;
			mmG_.workPoints[2] = 100;
			mmG_.workPoints[3] = 200;
			mmG_.workPoints[4] = 400;
			mmG_.workPoints[5] = 600;

			//Marketingの値段
			mmG_.preise[0] = 8500;
			mmG_.preise[1] = 25000;
			mmG_.preise[2] = 50000;
			mmG_.preise[3] = 100000;
			mmG_.preise[4] = 250000;
			mmG_.preise[5] = 500000;
		}
		public void GameKampagne_InitToModded(Menu_Marketing_GameKampagne mmG_)
		{
			//MarketingのHype上限値
			mmG_.maxHype[0] = Mathf.RoundToInt(15 * Main.CFG_POSTERS_MAXHYPE.Value);
			mmG_.maxHype[1] = Mathf.RoundToInt(25 * Main.CFG_GAMEMAGAZINES_MAXHYPE.Value);
			mmG_.maxHype[2] = Mathf.RoundToInt(40 * Main.CFG_RADIO_MAXHYPE.Value);
			mmG_.maxHype[3] = Mathf.RoundToInt(60 * Main.CFG_INTERNETADS_MAXHYPE.Value);
			mmG_.maxHype[4] = Mathf.RoundToInt(80 * Main.CFG_TOPSTREAMER_MAXHYPE.Value);
			mmG_.maxHype[5] = Mathf.RoundToInt(100 * Main.CFG_TV_MAXHYPE.Value);
			//Marketingの一回あたりのHype
			mmG_.hypeProKampagne[0] = Mathf.RoundToInt(5 * Main.CFG_POSTERS_HYPE_PROM.Value);
			mmG_.hypeProKampagne[1] = Mathf.RoundToInt(10 * Main.CFG_GAMEMAGAZINES_HYPE_PROM.Value);
			mmG_.hypeProKampagne[2] = Mathf.RoundToInt(15 * Main.CFG_RADIO_HYPE_PROM.Value);
			mmG_.hypeProKampagne[3] = Mathf.RoundToInt(20 * Main.CFG_INTERNETADS_HYPE_PROM.Value);
			mmG_.hypeProKampagne[4] = Mathf.RoundToInt(25 * Main.CFG_TOPSTREAMER_HYPE_PROM.Value);
			mmG_.hypeProKampagne[5] = Mathf.RoundToInt(30 * Main.CFG_TV_HYPE_PROM.Value);
			//Marketingの作業工数
			mmG_.workPoints[0] = Mathf.RoundToInt(25 * Main.CFG_POSTERS_WORKPOINTS.Value);
			mmG_.workPoints[1] = Mathf.RoundToInt(50 * Main.CFG_GAMEMAGAZINES_WORKPOINTS.Value);
			mmG_.workPoints[2] = Mathf.RoundToInt(100 * Main.CFG_RADIO_WORKPOINTS.Value);
			mmG_.workPoints[3] = Mathf.RoundToInt(200 * Main.CFG_INTERNETADS_WORKPOINTS.Value);
			mmG_.workPoints[4] = Mathf.RoundToInt(400 * Main.CFG_TOPSTREAMER_WORKPOINTS.Value);
			mmG_.workPoints[5] = Mathf.RoundToInt(600 * Main.CFG_TV_WORKPOINTS.Value);

			//Marketingの値段
			mmG_.preise[0] = Mathf.RoundToInt(8500 * Main.CFG_POSTERS_PRICE.Value);
			mmG_.preise[1] = Mathf.RoundToInt(25000 * Main.CFG_GAMEMAGAZINES_PRICE.Value);
			mmG_.preise[2] = Mathf.RoundToInt(50000 * Main.CFG_RADIO_PRICE.Value);
			mmG_.preise[3] = Mathf.RoundToInt(100000 * Main.CFG_INTERNETADS_PRICE.Value);
			mmG_.preise[4] = Mathf.RoundToInt(250000 * Main.CFG_TOPSTREAMER_PRICE.Value);
			mmG_.preise[5] = Mathf.RoundToInt(500000 * Main.CFG_TV_PRICE.Value);
		}

		[HarmonyPrefix, HarmonyPatch(typeof(Menu_Marketing_GameKampagne), "Init")]
		static bool GameKampagne_Init_ModifyHypePrePatch(Menu_Marketing_GameKampagne __instance)
		{
			//Modify Init.
			MarketingRebalancer mR_ = new MarketingRebalancer();
			mR_.Init();
			if (!_IS_ENABLED)
			{
				mR_.GameKampagne_InitToDefault(__instance);
				return true;
			}
			//Modify Init End.

			mR_.GameKampagne_InitToModded(__instance);

			//__instance.hypeProKampagne[5] = 150;
			//__instance.maxHype[5] = Mathf.RoundToInt(_MAX_HYPE);
			//__instance.workPoints[5] = 10;
			return true;
		}

		/*
		[HarmonyPostfix, HarmonyPatch(typeof(Menu_Result_MarketingSpezial), "Init")]
		static void Spezial_Init_ModifyHypePrePatch(gameScript gS_, int kampagne, Menu_Marketing_GameKampagne __instance, mainScript ___mS_, textScript ______tS_, GUI_Main ___guiMain_)
		{

			switch (kampagne)
			{
				default:
					{
						if (kampagne != 100)
						{
							return;
						}
						int num2 = 2000 + UnityEngine.Random.Range(0, 3000);
						//__instance.uiObjects[0].GetComponent<Text>().text = ___tS_.GetText(1437);
						//__instance.uiObjects[2].GetComponent<Text>().text = "-" + num2.ToString();
						//__instance.uiObjects[3].GetComponent<Image>().sprite = ___guiMain_.uiSprites[34];
						__instance.uiObjects[0].GetComponent<Text>().text = null;
						__instance.uiObjects[2].GetComponent<Text>().text = null;
						__instance.uiObjects[3].GetComponent<Image>().sprite = null;
						__instance.uiObjects[0].SetActive(false);
						__instance.uiObjects[1].SetActive(false);
						__instance.uiObjects[2].SetActive(false);
						__instance.uiObjects[3].SetActive(false);
						__instance.uiObjects[4].SetActive(false);
						gS_.hype -= 500f;
						___mS_.AddFans(1, -1); 
						return;
					}
			}
		}

		*/

		[HarmonyPrefix, HarmonyPatch(typeof(Menu_Result_MarketingSpezial), "Init")]
		static bool Spezial_Init_ModifyHypePrePatch(gameScript gS_, int kampagne, Menu_Result_MarketingSpezial __instance, mainScript ___mS_, textScript ___tS_, GUI_Main ___guiMain_, sfxScript ___sfx_, genres ___genres_, GameObject ___main_)
		{
			//Hype関連初期化処理
			MarketingRebalancer mR_ = new MarketingRebalancer();
			mR_.Init();
			if (!_IS_ENABLED) { return true; }

			// FindScripts
			___main_ = GameObject.Find("Main");
			___mS_ = ___main_.GetComponent<mainScript>();
			___tS_ = ___main_.GetComponent<textScript>();
			___sfx_ = GameObject.Find("SFX").GetComponent<sfxScript>();
			___guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
			___genres_ = ___main_.GetComponent<genres>();

			//本来、Hypeが100を超えて、レビュー内容がダメだった時の判定がkampagne = 100となっている
			if (kampagne == 100)
			{
				//HypeがBombastic Adsの数値を超えていなければ処理をせず、メニューを閉じる。ユーザー目線では開きかけたメニューは全く見えない状態となる。
				if (!(gS_.hype > Main.CFG_BOMBASTIC_ADS.Value))
				{
					//強制的に閉じさせています。もしかしたら不具合あるかも。今のところは、他のニュースもちゃんと表示出てます。
					if (gS_ && ___guiMain_.uiObjects[296].activeSelf)
                    {
						___guiMain_.DeactivateMenu(___guiMain_.uiObjects[296]);	//強制的にActiveSelf(false)させる
						___guiMain_.CloseMenu();	//開いているメニューを閉じる。
					}
					kampagne = -1;
					return false;
				}
            }

			if (!gS_)
			{
				__instance.BUTTON_Abbrechen();
			}
			___sfx_.PlaySound(54, true);
			__instance.uiObjects[1].GetComponent<Text>().text = gS_.GetNameWithTag();
			Debug.Log("kampagne : " + kampagne);
			if (kampagne < 100)
			{
				__instance.uiObjects[4].GetComponent<Image>().sprite = ___guiMain_.uiObjects[294].GetComponent<Menu_MarketingSpezial>().sprites[kampagne];
				gS_.specialMarketing[kampagne] = -1;
			}
			else
			{
				Debug.Log("Init : 4");
				__instance.uiObjects[4].GetComponent<Image>().sprite = ___guiMain_.uiSprites[19];
				Debug.Log("Init : 4.1");
			}
			switch (kampagne)
			{
				case 0:
					if ((!gS_.inDevelopment && !gS_.schublade) || gS_.reviewTotal > 0)
					{
						__instance.BUTTON_Abbrechen();
						return false;
					}
					gS_.CalcReview(true);
					if (gS_.reviewTotal < 40)
					{
						int num = -1 - UnityEngine.Random.Range(0, gS_.reviewTotal) / 5;
						gS_.AddHype((float)num);
						__instance.uiObjects[0].GetComponent<Text>().text = ___tS_.GetText(1426);
						__instance.uiObjects[2].GetComponent<Text>().text = Mathf.RoundToInt((float)num).ToString();
						__instance.uiObjects[3].GetComponent<Image>().sprite = ___guiMain_.uiSprites[33];
						gS_.specialMarketing[kampagne] = -1;
					}
					else
					{
						int num = 1 + UnityEngine.Random.Range(0, gS_.reviewTotal) / 5;
						gS_.AddHype((float)num);
						__instance.uiObjects[0].GetComponent<Text>().text = ___tS_.GetText(1427);
						__instance.uiObjects[2].GetComponent<Text>().text = "+" + Mathf.RoundToInt((float)num).ToString();
						__instance.uiObjects[3].GetComponent<Image>().sprite = ___guiMain_.uiSprites[33];
						gS_.specialMarketing[kampagne] = -1;
					}
					gS_.ClearReview();
					return false;
				case 1:
					if ((!gS_.inDevelopment && !gS_.schublade) || gS_.reviewTotal > 0)
					{
						__instance.BUTTON_Abbrechen();
						return false;
					}
					gS_.CalcReview(true);
					if (gS_.reviewTotal < 50)
					{
						int num = -3;
						gS_.AddHype((float)num);
						__instance.uiObjects[0].GetComponent<Text>().text = ___tS_.GetText(1428);
						__instance.uiObjects[2].GetComponent<Text>().text = "-3%";
						__instance.uiObjects[3].GetComponent<Image>().sprite = ___guiMain_.uiSprites[30];
						gS_.specialMarketing[kampagne] = num;
					}
					else
					{
						int num = 3;
						gS_.AddHype((float)num);
						__instance.uiObjects[0].GetComponent<Text>().text = ___tS_.GetText(1429);
						__instance.uiObjects[2].GetComponent<Text>().text = "+3%";
						__instance.uiObjects[3].GetComponent<Image>().sprite = ___guiMain_.uiSprites[30];
						gS_.specialMarketing[kampagne] = num;
					}
					gS_.ClearReview();
					return false;
				//Overhypeの場合
				case 2:
					if (!gS_.inDevelopment && !gS_.schublade)
					{
						__instance.BUTTON_Abbrechen();
						return false;
					}
					if (UnityEngine.Random.Range(0, 100) > 50)
					{
						__instance.uiObjects[0].GetComponent<Text>().text = ___tS_.GetText(1430);
						__instance.uiObjects[2].GetComponent<Text>().text = "+0";
						__instance.uiObjects[3].GetComponent<Image>().sprite = ___guiMain_.uiSprites[33];
						gS_.specialMarketing[kampagne] = -1;
						return false;
					}
					if (___mS_.achScript_)
					{
						___mS_.achScript_.SetAchivement(41);
					}
					__instance.uiObjects[0].GetComponent<Text>().text = ___tS_.GetText(1431);
					//__instance.uiObjects[2].GetComponent<Text>().text = "200";
					__instance.uiObjects[2].GetComponent<Text>().text = gS_.hype + "+" + Main.CFG_OVERHYPE.Value;
					__instance.uiObjects[3].GetComponent<Image>().sprite = ___guiMain_.uiSprites[33];
					//gS_.hype = 200f;
					gS_.hype = gS_.hype + Main.CFG_OVERHYPE.Value;
					gS_.specialMarketing[kampagne] = -1;
					return false;
				case 3:
					{
						int num = UnityEngine.Random.Range((gS_.userPositiv + gS_.userNegativ) / 10 + 50, gS_.userPositiv + gS_.userNegativ + 100);
						if (UnityEngine.Random.Range(0, 100) > 50)
						{
							int num2 = 500 + UnityEngine.Random.Range(___genres_.GetAmountFans() / 20, ___genres_.GetAmountFans() / 10);
							__instance.uiObjects[0].GetComponent<Text>().text = ___tS_.GetText(1432);
							__instance.uiObjects[2].GetComponent<Text>().text = "-" + num2.ToString();
							__instance.uiObjects[3].GetComponent<Image>().sprite = ___guiMain_.uiSprites[34];
							gS_.specialMarketing[kampagne] = -1;
							___mS_.AddFans(-num2, -1);
							return false;
						}
						__instance.uiObjects[0].GetComponent<Text>().text = ___tS_.GetText(1433);
						__instance.uiObjects[2].GetComponent<Text>().text = "+" + num.ToString();
						__instance.uiObjects[3].GetComponent<Image>().sprite = ___guiMain_.uiSprites[35];
						gS_.userPositiv += num;
						gS_.specialMarketing[kampagne] = -1;
						return false;
					}
				case 4:
					{
						int num;
						if (gS_.reviewTotal < 50)
						{
							num = -1 - UnityEngine.Random.Range(0, gS_.reviewTotal) / 5;
							gS_.AddHype((float)num);
							__instance.uiObjects[0].GetComponent<Text>().text = ___tS_.GetText(1435);
							__instance.uiObjects[2].GetComponent<Text>().text = Mathf.RoundToInt((float)num).ToString();
							__instance.uiObjects[3].GetComponent<Image>().sprite = ___guiMain_.uiSprites[33];
							gS_.specialMarketing[kampagne] = -1;
							return false;
						}
						num = 1 + UnityEngine.Random.Range(0, gS_.reviewTotal) / 5;
						gS_.AddHype((float)num);
						__instance.uiObjects[0].GetComponent<Text>().text = ___tS_.GetText(1436);
						__instance.uiObjects[2].GetComponent<Text>().text = "+" + Mathf.RoundToInt((float)num).ToString();
						__instance.uiObjects[3].GetComponent<Image>().sprite = ___guiMain_.uiSprites[33];
						gS_.specialMarketing[kampagne] = -1;
						return false;
					}
				default:
					{
						if (kampagne != 100)
						{
							return false;
						}
						int num2 = 2000 + UnityEngine.Random.Range(0, 3000);
						__instance.uiObjects[0].GetComponent<Text>().text = ___tS_.GetText(1437);
						__instance.uiObjects[2].GetComponent<Text>().text = "-" + num2.ToString();
						__instance.uiObjects[3].GetComponent<Image>().sprite = ___guiMain_.uiSprites[34];
						gS_.hype -= (gS_.hype / 10);
						___mS_.AddFans(-num2, -1);
						return false;
					}
			}
		}


		/*
[HarmonyTranspiler, HarmonyPatch(typeof(games), "iWaitForSpecialMarketing")]
static bool Patch_iWaitForSpecialMarketing()
{
	bool done = false;
	while (!done)
	{
		if (gS_ && !___guiMain_.uiObjects[296].activeSelf)
		{
			done = true;
			___guiMain_.OpenMenu(false);
			___guiMain_.ActivateMenu(___guiMain_.uiObjects[296]);
			___guiMain_.uiObjects[296].GetComponent<Menu_Result_MarketingSpezial>().Init(gS_, kampagne);
		}
		yield return null;
	}
	yield break;
}
*/

	}
}
