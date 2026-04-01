using System;

namespace RuriMegu.Core.Utils;

public sealed class Subscription(Action dispose) : IDisposable {
  private readonly Action _dispose = dispose;
  private bool _isDisposed;

  public void Dispose() {
    if (_isDisposed) {
      return;
    }

    _isDisposed = true;
    _dispose();
  }
}
