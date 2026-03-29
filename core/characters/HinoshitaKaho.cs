using System.Collections.Generic;
using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;
using RuriMegu.Core.Cards;
using RuriMegu.Core.Extensions;
using RuriMegu.Core.Relics;

namespace RuriMegu.Core.Characters;

/// <summary>
/// Hinoshita Kaho (日野下花帆)
/// </summary>
public class HinoshitaKaho : PlaceholderCharacterModel {
  public const string CharacterId = "HinoshitaKaho";

  public static readonly Color Color = new("f8b400");

  public override Color NameColor => Color;
  public override CharacterGender Gender => CharacterGender.Feminine;
  public override int StartingHp => 80;

  public override IEnumerable<CardModel> StartingDeck => [
    ModelDb.Card<StrikeLinkura>(),
    ModelDb.Card<StrikeLinkura>(),
    ModelDb.Card<StrikeLinkura>(),
    ModelDb.Card<StrikeLinkura>(),
    ModelDb.Card<StrikeLinkura>(),
    ModelDb.Card<DefendLinkura>(),
    ModelDb.Card<DefendLinkura>(),
    ModelDb.Card<DefendLinkura>(),
    ModelDb.Card<DefendLinkura>(),
    ModelDb.Card<DefendLinkura>(),
  ];

  public override IReadOnlyList<RelicModel> StartingRelics => [
    ModelDb.Relic<LinkuraSystem>(),
  ];

  public override CardPoolModel CardPool => ModelDb.CardPool<LinkuraCardPool>();
  public override RelicPoolModel RelicPool => ModelDb.RelicPool<LinkuraRelicPool>();
  public override PotionPoolModel PotionPool => ModelDb.PotionPool<LinkuraPotionPool>();

  // Asset paths - placeholder until custom art is added
  public override string CustomIconTexturePath => "character_icon_kaho.png".CharacterUiPath();
  public override string CustomCharacterSelectIconPath => "char_select_kaho.png".CharacterUiPath();
  public override string CustomCharacterSelectLockedIconPath => "char_select_kaho_locked.png".CharacterUiPath();
  public override string CustomMapMarkerPath => "map_marker_kaho.png".CharacterUiPath();
}
