using System.Reflection;
using BepInEx;
using DiskCardGame;
using InscryptionAPI.Card;
using UnityEngine;
using static System.IO.File;

namespace SigilADay_julianperge;

public static class SigilUtils
{
	public static AbilityInfo CreateInfoWithDefaultSettings(
		string rulebookName,
		string rulebookDescription,
		bool withDialogue = true,
		int powerLevel = 0
	)
	{
		AbilityInfo info = ScriptableObject.CreateInstance<AbilityInfo>();
		info.powerLevel = powerLevel;
		info.rulebookName = rulebookName;
		info.rulebookDescription = rulebookDescription;
		info.metaCategories = new List<AbilityMetaCategory>
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
			new List<DialogueEvent.Line> { new() { text = dialogue } }
		);
	}

	public static AbilityManager.FullAbility CreateAbility<T>(
		byte[] texture,
		string rulebookName,
		string rulebookDescription,
		int powerLevel = 0
	) where T : AbilityBehaviour
	{
		return CreateAbility<T>(
			LoadTextureFromResource(texture),
			rulebookName,
			rulebookDescription,
			powerLevel
		);
	}

	public static AbilityManager.FullAbility CreateAbility<T>(
		Texture texture,
		string rulebookName,
		string rulebookDescription,
		int powerLevel = 0
	) where T : AbilityBehaviour
	{
		return CreateAbility<T>(
			CreateInfoWithDefaultSettings(rulebookName, rulebookDescription, powerLevel: powerLevel),
			texture
		);
	}

	private static AbilityManager.FullAbility CreateAbility<T>(
		AbilityInfo info,
		Texture texture
	) where T : AbilityBehaviour
	{
		Type type = typeof(T);
		// instantiate
		AbilityManager.FullAbility ability = AbilityManager.Add(Plugin.PluginGuid, info, type, texture);

		// Get static field
		FieldInfo field = type.GetField(
			"ability",
			BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance
		);
		field.SetValue(null, ability.Id);

		return ability;
	}

	public static Texture2D LoadTextureFromResource(byte[] resourceFile)
	{
		var texture = new Texture2D(2, 2);
		texture.LoadImage(resourceFile);
		return texture;
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

	public static string GetLogOfCardInSlot(PlayableCard playableCard)
	{
		return $"Card [{playableCard.Info.name}] Slot [{playableCard.Slot.Index}]";
	}
}
