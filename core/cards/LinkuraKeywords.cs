using BaseLib.Patches.Content;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace RuriMegu.Core.Cards;
#pragma warning disable CA2211

public static class LinkuraKeywords {

  [CustomEnum("Collect_Hearts"), KeywordProperties(AutoKeywordPosition.After)]
  public static CardKeyword CollectHearts;
}
