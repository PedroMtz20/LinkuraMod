using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using RuriMegu.Core.Characters;

namespace RuriMegu.Core.Utils;

public static class LinkuraAnimation {
  public const string ANIM_NAME_CAST = "cast";
  public const string ANIM_NAME_BURST = "burst";
  public const string ANIM_NAME_COLLECT = "collect";

  public static async Task PlayCastAnim(this Player player) {
    if (player.Character is LinkuraCharacterModel linkuraChara) {
      await CreatureCmd.TriggerAnim(player.Creature, linkuraChara.AnimNameCast, linkuraChara.CastAnimDelay);
    }
  }

  public static async Task PlayBurstAnim(this Player player) {
    if (player.Character is LinkuraCharacterModel linkuraChara) {
      await CreatureCmd.TriggerAnim(player.Creature, linkuraChara.AnimNameBurst, linkuraChara.BurstAnimDelay);
    }
  }

  public static async Task PlayCollectAnim(this Player player) {
    if (player.Character is LinkuraCharacterModel linkuraChara) {
      await CreatureCmd.TriggerAnim(player.Creature, linkuraChara.AnimNameCollect, linkuraChara.CollectAnimDelay);
    }
  }
}
