using BaseLib.Patches.Content;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace RuriMegu.Core;

public static class LinkuraKeywords {

  [CustomEnum("BurstHearts"), KeywordProperties(AutoKeywordPosition.Before)]
  public static readonly CardKeyword BurstHearts;

  [CustomEnum("CollectHearts"), KeywordProperties(AutoKeywordPosition.Before)]
  public static readonly CardKeyword CollectHearts;
}
