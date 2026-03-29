using BaseLib.Patches.Content;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace RuriMegu.Core;

public static class LinkuraKeywords {

  [CustomEnum("BurstHeart"), KeywordProperties(AutoKeywordPosition.Before)]
  public static CardKeyword BurstHeart;

  [CustomEnum("CollectHeart"), KeywordProperties(AutoKeywordPosition.Before)]
  public static CardKeyword CollectHeart;
}
