using System;
using System.Collections.Generic;
using System.IO;
using APIPlugin;
using BepInEx;
using DiskCardGame;
using UnityEngine;
using static System.IO.File;

namespace SigilADay_julianperge
{
	public static class SigilUtils
	{
		public static AbilityInfo CreateInfoWithDefaultSettings(
			string rulebookName, string rulebookDescription, bool withDialogue = false, int powerLevel = 0
		)
		{
			AbilityInfo info = ScriptableObject.CreateInstance<AbilityInfo>();
			info.powerLevel = powerLevel;
			info.rulebookName = rulebookName;
			info.rulebookDescription = rulebookDescription;
			info.metaCategories = new List<AbilityMetaCategory>()
			{
				AbilityMetaCategory.Part1Modular, AbilityMetaCategory.Part1Rulebook
			};

			if (withDialogue)
			{
				info.abilityLearnedDialogue = SetAbilityInfoDialogue(
					rulebookDescription.Replace("[creature]", "a card bearing this sigil")
				);
			}

			return info;
		}

		public static DialogueEvent.LineSet SetAbilityInfoDialogue(string dialogue)
		{
			return new DialogueEvent.LineSet(
				new List<DialogueEvent.Line>()
				{
					new DialogueEvent.Line()
					{
						text = dialogue
					}
				}
			);
		}

		public static Texture2D LoadTextureFromResource(byte[] resourceFile)
		{
			var texture = new Texture2D(2, 2);
			texture.LoadImage(resourceFile);
			return texture;
		}

		public static AbilityIdentifier GetAbilityId(string rulebookName)
		{
			return AbilityIdentifier.GetAbilityIdentifier(SigilADay_julianperge.Plugin.PluginGuid, rulebookName);
		}

		public static SpecialAbilityIdentifier GetSpecialAbilityId(string rulebookName)
		{
			return SpecialAbilityIdentifier.GetID(SigilADay_julianperge.Plugin.PluginGuid, rulebookName);
		}

		public static string GetFullPathOfFile(string fileToLookFor)
		{
			return Directory.GetFiles(Paths.PluginPath, fileToLookFor, SearchOption.AllDirectories)[0];
		}

		public static byte[] ReadArtworkFileAsBytes(string nameOfCardArt)
		{
			return ReadAllBytes(GetFullPathOfFile(nameOfCardArt));
		}

		public static Texture2D LoadImageAndGetTexture(string nameOfCardArt)
		{
			Texture2D texture = new Texture2D(2, 2);
			byte[] imgBytes = ReadArtworkFileAsBytes(nameOfCardArt);
			bool isLoaded = texture.LoadImage(imgBytes);
			return texture;
		}

		public static readonly Predicate<PlayableCard> IsNonLivingCard = playableCard =>
			playableCard
			&& playableCard.Info
			&& playableCard.Info.traits.Exists(t => t is Trait.Terrain or Trait.Pelt);


		public static readonly Predicate<PlayableCard> IsLivingCard = playableCard => !IsNonLivingCard.Invoke(playableCard);

		public static String GetLogOfCardInSlot(PlayableCard playableCard)
		{
			return $"Card [{playableCard.Info.name}] Slot [{playableCard.Slot.Index}]";
		}
	}
}